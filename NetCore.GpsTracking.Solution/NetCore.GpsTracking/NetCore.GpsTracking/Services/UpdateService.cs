using Microsoft.EntityFrameworkCore;
using NetCore.Library;
using NetCore.GpsTrackingModule.Controllers;
using NetCore.GpsTrackingModule.Data;
using NetCore.Websites;
using NetCore.Websites.Models;
using NetCore.Websites.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetCore.GpsTrackingModule.Models;
using NetCore.GpsTrackingModule.Library;
using System.Device;
using System.Device.Location;

namespace NetCore.GpsTrackingModule.Services
{
    public partial interface IUpdateService
    {
        Task Index(Pipeline pipeline, int cmd, string deviceCode, int eventTypeId, float longitude, float latitude);
    }

    public partial class UpdateService : _ServiceGpsTracking, IUpdateService
    {
        public UpdateService(IServiceProvider _ServiceProvider)
            : base(_ServiceProvider)
        {
        }

        public async Task Index(Pipeline pipeline, int cmd, string deviceCode, int eventTypeId, float longitude, float latitude)
        {
            var gpsDevice = DBs.GpsDevice.Query.FirstOrDefault(x => x.Code == deviceCode);
            if (gpsDevice != null)
            {
                gpsDevice.LastLongitude = longitude;
                gpsDevice.LastLatitude = latitude;

                var location = new Location
                {
                    GpsDeviceId = gpsDevice.Id,
                    Longitude = longitude,
                    Latitude = latitude,
                    Address = GeoLocationToAddress.Get(longitude, latitude),
                };
                DBs.Location.Insert(location);

                string emailContent = "";

                var Geofences = DBs.Geofence.Query.Include(x => x.Details).ToList();
                foreach (var geofence in Geofences)
                {
                    var isEnter = GeofenceHelper.CheckEnter(
                         geofence.Details.Select(x => new LocationPoint { Longitude = x.Longitude, Latitude = x.Latitude }).ToArray(),
                         new LocationPoint { Longitude = location.Longitude, Latitude = location.Latitude }
                         );

                    var geofenceMapping = DBs.EventGeofenceMapping.Query.FirstOrDefault(x => x.GpsDeviceId == gpsDevice.Id && x.GeofenceId == geofence.Id);
                    if (geofenceMapping == null)
                    {
                        geofenceMapping = new EventGeofenceMapping
                        {
                            GpsDeviceId = gpsDevice.Id,
                            GeofenceId = geofence.Id,
                        };
                        DBs.EventGeofenceMapping.Insert(geofenceMapping);
                    }

                    bool changed = false;
                    if (isEnter)
                    {
                        if (geofenceMapping.EventType != EventType.EnterGeofence)
                        {
                            changed = true;
                            geofenceMapping.EventType = EventType.EnterGeofence;
                        }
                    }
                    else
                    {
                        if (geofenceMapping.EventType != EventType.ExitGeofence)
                        {
                            changed = true;
                            geofenceMapping.EventType = EventType.ExitGeofence;
                        }
                    }

                    if (changed)
                    {
                        var _event = new Event
                        {
                            GpsDeviceId = gpsDevice.Id,
                            LocationId = location.Id,

                            Type = geofenceMapping.EventType,
                            GeofenceId = geofence.Id,
                        };
                        DBs.Event.Insert(_event);

                        emailContent += " - " + _event.TypeName + " " + geofence.Name + "<br/>";
                    }

                }

                if (lib.NotNullEmpty(emailContent))
                {
                    emailContent = gpsDevice.Name + ":<br/>" + emailContent;

                    var mappedUsers = DBs.GpsDeviceMapping.Query.Where(x => x.GpsDeviceId == gpsDevice.Id);
                    var ProfileInfos = DBs.ProfileInfo.Query.Where(x => mappedUsers.Any(y => y.ProfileInfoId == x.Id));

                    EmailHelper.Send(ProfileInfos.Select(x => x.Email).ToList(), "Thiết bị ra/vào khu vực lúc " + lib.Time, emailContent);
                }

                //xác định nhóm của TB-1 -> nhóm X
                var GroupIDsThisDeviceJoined = DBs.GpsDeviceMapping.Query.Where(x => x.GpsDeviceId == gpsDevice.Id).Select(x => x.GroupId);

                //tìm tất cả các TB thuộc nhóm (X)
                var OtherDevicesSameGroupWithThis = DBs.GpsDeviceMapping.Query.Include(x => x.GpsDevice).Where(x => x.GpsDeviceId != gpsDevice.Id && GroupIDsThisDeviceJoined.Any(y => y == x.GroupId)).Select(x => x.GpsDevice).Distinct();

                //tính khoảng giữa TB-1 và các TB khác thuộc nhóm X
                var testContent2 = "";
                foreach (var otherDevice in OtherDevicesSameGroupWithThis)
                {
                    var distance = MapHelper.DistanceKm(gpsDevice.LastLatitude, gpsDevice.LastLongitude, otherDevice.LastLatitude, otherDevice.LastLongitude);
                    testContent2 += otherDevice.Name + " (" + otherDevice.LastLatitude + "°N, " + otherDevice.LastLongitude + "°E) khoảng cách " + distance + " km." + lib.Line;
                    if (distance <= 5)
                    {
                        var mappedUsers = DBs.GpsDeviceMapping.Query.Where(x => x.GpsDeviceId == gpsDevice.Id);
                        var ProfileInfos = DBs.ProfileInfo.Query.Where(x => mappedUsers.Any(y => y.ProfileInfoId == x.Id));
                        EmailHelper.Send(ProfileInfos.Select(x => x.Email).ToList(), "Hai đối tượng tiếp cận nhau lúc " + lib.Time, gpsDevice.Name + " cách " + testContent2);
                    }
                }

                testContent2 = gpsDevice.Name + "(" + gpsDevice.LastLatitude + ", " + gpsDevice.LastLongitude + ")" + lib.Line2 + testContent2;
                Log.Test(testContent2);

                //Event
                if (cmd == 2 && (eventTypeId == 1 || eventTypeId == 2))
                {
                    var _event = new Event
                    {
                        GpsDeviceId = gpsDevice.Id,
                        LocationId = location.Id,
                        TypeId = eventTypeId,
                    };
                    DBs.Event.Insert(_event);

                    //so sanh trang thai Moi nhat, so voi trang thai hien tai (trong DB)
                    if (gpsDevice.EventTypeId != eventTypeId)
                    {
                        var mappedUsers = DBs.GpsDeviceMapping.Query.Where(x => x.GpsDeviceId == gpsDevice.Id);
                        var ProfileInfos = DBs.ProfileInfo.Query.Where(x => mappedUsers.Any(y => y.ProfileInfoId == x.Id));

                        //gui email neu so sanh co khac nhau
                        EmailHelper.Send(ProfileInfos.Select(x => x.Email).ToList(), "Thiết bị " + _event.TypeName + " lúc " + lib.Time, gpsDevice.Name + ": " + _event.TypeName);
                    }

                    // online/offline
                    if (_event.Type == EventType.Online || _event.Type == EventType.Offline)
                        gpsDevice.StatusId = _event.TypeId;

                    //gpsDevice.EventTypeId
                    gpsDevice.EventTypeId = _event.TypeId;
                }

                //Save
                await DBs.Save();

                //Result
                pipeline.Status = ResponseStatus.Successful;
                pipeline.Result = new
                {
                };
            }
            else
                pipeline.Status = ResponseStatus.Error;
        }
    }
}
