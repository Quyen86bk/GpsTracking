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

namespace NetCore.GpsTrackingModule.Services
{
    public partial interface ILocationService
    {
        Task Save(Pipeline pipeline, Location model);

        Task Get(Pipeline pipeline, Guid id);

        Task GetPage(Pipeline pipeline, PageFilterVM filter);

        Task GetList(Pipeline pipeline, ListFilterVM filter);

        Task Delete(Pipeline pipeline, List<Guid> IDs);

        Task GetLocations(Pipeline pipeline, LocationFilterVM filter);

        Task GetHeatmaps(Pipeline pipeline, HeatmapFilterVM filter);

        Task GetReplays(Pipeline pipeline, ReplayFilterVM filter);
    }

    public partial class LocationService : _ServiceGpsTracking, ILocationService
    {
        public LocationService(IServiceProvider _ServiceProvider)
            : base(_ServiceProvider)
        {
        }

        public async Task Save(Pipeline pipeline, Location model)
        {
            Location Location = null;
            if (lib.NotSelected(model.Id))
            {
                model.Id = Guid.NewGuid();
                Location = new Location
                {
                    Id = model.Id,
                    CreatedById = pipeline.UserId
                };
                DBs.Location.Insert(Location);
            }
            else
            {
                Location = await DBs.Location.Query
                    .FirstOrDefaultAsync(x => x.Id == model.Id);
            }

            if (Location != null)
            {
                //UpdatedBy
                Location.UpdatedById = pipeline.UserId;
                Location.UpdatedTime = lib.Time;

                //Set
                Location.Protocol = model.Protocol;
                Location.DeviceTime = model.DeviceTime;
                Location.Address = model.Address;
                Location.Latitude = model.Latitude;
                Location.Longitude = model.Longitude;
                Location.Speed = model.Speed;
                Location.Course = model.Course;

                //Save
                await DBs.Save();
            }

            //Result
            pipeline.Status = ResponseStatus.Successful;
            pipeline.Result = new
            {
                model
            };
        }

        public async Task Get(Pipeline pipeline, Guid id)
        {
            var model = await DBs.Location.Query
                .FirstOrDefaultAsync(x => x.Id == id);

            pipeline.Status = ResponseStatus.Successful;
            pipeline.Result = new { model };
        }

        public async Task GetPage(Pipeline pipeline, PageFilterVM filter)
        {
            //Select
            var Locations = DBs.Location.Query;

            //filter.Keyword
            if (lib.Selected(filter.Keyword))
                Locations = Locations.Where(x => x.Address.Contains(filter._Keyword));

            //filter.Date
            if (filter.HaveDate())
                Locations = Locations.Where(x => x.CreatedTime >= filter.FromDate && x.CreatedTime <= filter.ToDate);

            //Order
            Locations = Locations.OrderByDescending(x => x.CreatedTime);

            //Models
            var Total = Locations.Count();
            var Models = await Locations
                .Include(x => x.GpsDevice)
                .Skip(filter.From).Take(Config.MaxSelectPage(filter.From, filter.To))
                .ToListAsync();

            //Result
            pipeline.Status = ResponseStatus.Successful;
            pipeline.Result = new
            {
                Models,
                Total,
            };
        }

        public async Task GetList(Pipeline pipeline, ListFilterVM filter)
        {
            //Select
            var Locations = DBs.Location.Query;

            //filter
            if (lib.Selected(filter.Id))
                Locations = Locations.Where(x => x.Id == filter.Id);

            //if (lib.Selected(filter.ParentId))
            //Locations = Locations.Where(x => x.ParentId == filter.ParentId);

            if (lib.Selected(filter.ExcludeId))
                Locations = Locations.Where(x => x.Id != filter.ExcludeId);

            if (lib.Selected(filter.Keyword))
                Locations = Locations.Where(x => x.Address.Contains(filter._Keyword));

            //Order
            Locations = Locations.OrderBy(x => x.Address);

            //Models
            var List = await Locations
                .Take(Config.MaxSelectList(filter.Keyword))
                .ToListAsync();

            var Models = new List<ListVM>();
            foreach (var item in List)
            {
                Models.Add(new ListVM
                {
                    Value = item.Id,
                    Name = item.Address,
                    Info = item.Address,
                });
            }

            //Result
            pipeline.Status = ResponseStatus.Successful;
            pipeline.Result = new
            {
                Models
            };
        }

        public async Task Delete(Pipeline pipeline, List<Guid> IDs)
        {
            var Locations = DBs.Location.Query.Where(x => IDs.Any(y => y == x.Id));
            await Locations.ForEachAsync(x =>
            {
                x.IsDeleted = true;
                x.DeletedById = pipeline.UserId;
                x.DeletedTime = lib.Time;
            });

            await DBs.Save();
            pipeline.Status = ResponseStatus.Successful;
        }

        //
        public async Task GetLocations(Pipeline pipeline, LocationFilterVM filter)
        {
            //Select
            var Locations = DBs.Location.Query;

            //Where / filter
            if (!pipeline.User.IsAdmin)
            {
                var mappedDevices = DBs.GpsDeviceMapping.Query.Where(x => x.ProfileInfoId == pipeline.UserId);
                Locations = Locations.Where(x => mappedDevices.Any(y => y.GpsDeviceId == x.GpsDeviceId));
            }

            //filter
            var gpsDeviceName = "All";
            if (lib.Selected(filter.GpsDeviceId))
            {
                var gpsDevice = DBs.GpsDevice.Query.FirstOrDefault(x => x.Id == filter.GpsDeviceId);
                if (gpsDevice != null)
                {
                    gpsDeviceName = gpsDevice.Name;
                    Locations = Locations.Where(x => x.GpsDeviceId == filter.GpsDeviceId);
                }
            }

            var groupName = "All";
            if (lib.Selected(filter.GroupId))
            {
                var group = DBs.Group.Query
                    .Include(x => x.GpsDeviceMappings)
                    .FirstOrDefault(x => x.Id == filter.GroupId);
                if (group != null)
                {
                    groupName = group.Name;
                    Locations = Locations.Where(x => group.GpsDeviceMappings.Any(y => y.GpsDeviceId == x.GpsDeviceId));
                }
            }

            //Order
            Locations = Locations.OrderByDescending(x => x.CreatedTime);

            //Models
            var List = await Locations
                .Include(x => x.GpsDevice).ThenInclude(x => x.EventGeofenceMappings).ThenInclude(x => x.Geofence)
                .GroupBy(x => x.GpsDeviceId)
                .ToListAsync();

            var timeFrom = new DateTime(lib.Time.Year, lib.Time.Month, lib.Time.Day);
            var timeTo = lib.Time;

            var Models = new List<LocationVM>();
            foreach (var item in List)
            {
                var model = new LocationVM
                {
                    Id = item.FirstOrDefault().GpsDevice.Id,
                    Code = item.FirstOrDefault().GpsDevice.Code,
                    Name = item.FirstOrDefault().GpsDevice.Name,
                    CategoryId = item.FirstOrDefault().GpsDevice.CategoryId,
                    StatusId = item.FirstOrDefault().GpsDevice.StatusId,
                    EventTypeId = item.FirstOrDefault().GpsDevice.EventTypeId,

                    Locations = item.Where(x => x.CreatedTime >= timeFrom && x.CreatedTime <= timeTo)
                        .OrderBy(x => x.CreatedTime)
                        .Select(x => new List<float> { x.Longitude, x.Latitude }).ToList(),

                    Last = item.OrderByDescending(x => x.CreatedTime)
                        .Select(x => new List<float> { x.Longitude, x.Latitude })
                        .FirstOrDefault(),

                    Address = item.FirstOrDefault().Address,
                };
                Models.Add(model);

                var EventGeofenceMappings = item.FirstOrDefault().GpsDevice.EventGeofenceMappings;
                foreach (var eventMapping in EventGeofenceMappings)
                {
                    model.EventGeofences.Add(new EventGeofenceVM
                    {
                        Event = eventMapping.EventTypeName,
                        Geofence = eventMapping.Geofence.Name
                    });
                }
            }

            //Result
            pipeline.Status = ResponseStatus.Successful;
            pipeline.Result = new
            {
                Models,
            };
        }

        public async Task GetHeatmaps(Pipeline pipeline, HeatmapFilterVM filter)
        {
            //Select
            var Heatmaps = DBs.Location.Query;

            //Where / filter
            if (!pipeline.User.IsAdmin)
            {
                var mappedDevices = DBs.GpsDeviceMapping.Query.Where(x => x.ProfileInfoId == pipeline.UserId);
                Heatmaps = Heatmaps.Where(x => mappedDevices.Any(y => y.GpsDeviceId == x.GpsDeviceId));
            }

            //filter.Keyword
            var gpsDeviceName = "All";
            if (lib.Selected(filter.GpsDeviceId))
            {
                var gpsDevice = DBs.GpsDevice.Query.FirstOrDefault(x => x.Id == filter.GpsDeviceId);
                if (gpsDevice != null)
                {
                    gpsDeviceName = gpsDevice.Name;
                    Heatmaps = Heatmaps.Where(x => x.GpsDeviceId == filter.GpsDeviceId);
                }
            }

            var groupName = "All";
            if (lib.Selected(filter.GroupId))
            {
                var group = DBs.Group.Query
                    .Include(x => x.GpsDeviceMappings)
                    .FirstOrDefault(x => x.Id == filter.GroupId);
                if (group != null)
                {
                    groupName = group.Name;
                    Heatmaps = Heatmaps.Where(x => group.GpsDeviceMappings.Any(y => y.GpsDeviceId == x.GpsDeviceId));
                }
            }

            if (lib.Selected(filter.TimeRange))
            {
                var timeFrom = new DateTime(lib.Time.Year, lib.Time.Month, lib.Time.Day);
                var timeTo = lib.Time;
                int BeginofWeek(DateTime dt)
                {
                    switch (dt.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            return 0;
                        case DayOfWeek.Tuesday:
                            return 1;
                        case DayOfWeek.Wednesday:
                            return 2;
                        case DayOfWeek.Thursday:
                            return 3;
                        case DayOfWeek.Friday:
                            return 4;
                        case DayOfWeek.Saturday:
                            return 5;
                        case DayOfWeek.Sunday:
                            return 6;
                    }
                    throw new Exception("Error!");
                }

                if (filter.TimeRange == 2)
                {
                    timeFrom = new DateTime(lib.Time.Year, lib.Time.Month, lib.Time.Day).AddDays(-1);
                    timeTo = new DateTime(lib.Time.Year, lib.Time.Month, lib.Time.Day).AddTicks(-1);
                }
                else if (filter.TimeRange == 3)
                {
                    timeFrom = lib.Time.AddDays(-1 * BeginofWeek(lib.Time));
                    timeTo = timeFrom.AddDays(6);
                }
                else if (filter.TimeRange == 4)
                {
                    timeFrom = new DateTime(lib.Time.Year, lib.Time.Month, 1);
                    timeTo = lib.Time;
                }

                filter.FromDate = timeFrom;
                filter.ToDate = timeTo;
            }

            //filter.Date
            if (filter.HaveDate())
                Heatmaps = Heatmaps.Where(x => x.CreatedTime >= filter.FromDate && x.CreatedTime <= filter.ToDate);

            //Order
            Heatmaps = Heatmaps.OrderByDescending(x => x.CreatedTime);

            //Models
            var List = await Heatmaps
                .Include(x => x.GpsDevice)
                .GroupBy(x => x.GpsDeviceId)
                .ToListAsync();

            var Models = new List<LocationVM>();
            foreach (var item in List)
            {
                Models.Add(new LocationVM
                {
                    Id = item.FirstOrDefault().GpsDevice.Id,
                    Code = item.FirstOrDefault().GpsDevice.Code,
                    Name = item.FirstOrDefault().GpsDevice.Name,
                    CategoryId = item.FirstOrDefault().GpsDevice.CategoryId,
                    StatusId = item.FirstOrDefault().GpsDevice.StatusId,

                    Locations = item
                        .OrderBy(x => x.CreatedTime)
                        .Select(x => new List<float> { x.Longitude, x.Latitude }).ToList(),

                    Last = item.OrderByDescending(x => x.CreatedTime)
                        .Select(x => new List<float> { x.Longitude, x.Latitude })
                        .FirstOrDefault(),
                });
            }

            //Result
            pipeline.Status = ResponseStatus.Successful;
            pipeline.Result = new
            {
                Models,
            };
        }

        public async Task GetReplays(Pipeline pipeline, ReplayFilterVM filter)
        {
            //Select
            var Replays = DBs.Location.Query;

            //Where / filter
            if (!pipeline.User.IsAdmin)
            {
                var mappedDevices = DBs.GpsDeviceMapping.Query.Where(x => x.ProfileInfoId == pipeline.UserId);
                Replays = Replays.Where(x => mappedDevices.Any(y => y.GpsDeviceId == x.GpsDeviceId));
            }

            //filter
            var gpsDeviceName = "All";
            if (lib.Selected(filter.GpsDeviceId))
            {
                var gpsDevice = DBs.GpsDevice.Query.FirstOrDefault(x => x.Id == filter.GpsDeviceId);
                if (gpsDevice != null)
                {
                    gpsDeviceName = gpsDevice.Name;
                    Replays = Replays.Where(x => x.GpsDeviceId == filter.GpsDeviceId);
                }
            }

            var groupName = "All";
            if (lib.Selected(filter.GroupId))
            {
                var group = DBs.Group.Query
                    .Include(x => x.GpsDeviceMappings)
                    .FirstOrDefault(x => x.Id == filter.GroupId);
                if (group != null)
                {
                    groupName = group.Name;
                    Replays = Replays.Where(x => group.GpsDeviceMappings.Any(y => y.GpsDeviceId == x.GpsDeviceId));
                }
            }

            if (lib.Selected(filter.TimeRange))
            {
                var timeFrom = new DateTime(lib.Time.Year, lib.Time.Month, lib.Time.Day);
                var timeTo = lib.Time;
                int BeginofWeek(DateTime dt)
                {
                    switch (dt.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            return 0;
                        case DayOfWeek.Tuesday:
                            return 1;
                        case DayOfWeek.Wednesday:
                            return 2;
                        case DayOfWeek.Thursday:
                            return 3;
                        case DayOfWeek.Friday:
                            return 4;
                        case DayOfWeek.Saturday:
                            return 5;
                        case DayOfWeek.Sunday:
                            return 6;
                    }
                    throw new Exception("Error!");
                }

                if (filter.TimeRange == 2)
                {
                    timeFrom = new DateTime(lib.Time.Year, lib.Time.Month, lib.Time.Day).AddDays(-1);
                    timeTo = new DateTime(lib.Time.Year, lib.Time.Month, lib.Time.Day).AddTicks(-1);
                }
                else if (filter.TimeRange == 3)
                {
                    timeFrom = lib.Time.AddDays(-1 * BeginofWeek(lib.Time));
                    timeTo = timeFrom.AddDays(6);
                }
                else if (filter.TimeRange == 4)
                {
                    timeFrom = new DateTime(lib.Time.Year, lib.Time.Month, 1);
                    timeTo = lib.Time;
                }

                filter.FromDate = timeFrom;
                filter.ToDate = timeTo;
            }

            //filter.Date
            if (filter.HaveDate())
                Replays = Replays.Where(x => x.CreatedTime >= filter.FromDate && x.CreatedTime <= filter.ToDate);

            //Order
            Replays = Replays.OrderByDescending(x => x.CreatedTime);

            //Models
            var List = await Replays
                .Include(x => x.GpsDevice)
                .GroupBy(x => x.GpsDeviceId)
                .ToListAsync();

            var Points = new List<ReplayVM>();
            foreach (var item in List)
            {
                var Locations = item.Select(x => new { x.Longitude, x.Latitude, Tick = lib.ToDateTime(x.CreatedTime).Ticks, Time = lib.NiceDateTime3(x.CreatedTime) }).ToList();
                foreach (var location in Locations)
                {
                    Points.Add(new ReplayVM
                    {
                        Id = item.FirstOrDefault().Id,
                        Code = item.FirstOrDefault().GpsDevice.Code,
                        Name = item.FirstOrDefault().GpsDevice.Name,
                        CategoryId = item.FirstOrDefault().GpsDevice.CategoryId,
                        StatusId = item.FirstOrDefault().GpsDevice.StatusId,

                        Longitude = location.Longitude,
                        Latitude = location.Latitude,

                        Tick = location.Tick,
                        Time = location.Time,
                    });
                }
            }

            var Routes = new List<LocationVM>();
            foreach (var item in List)
            {
                Routes.Add(new LocationVM
                {
                    Id = item.FirstOrDefault().Id,
                    Code = item.FirstOrDefault().GpsDevice.Code,
                    Name = item.FirstOrDefault().GpsDevice.Name,

                    Locations = item
                        .OrderBy(x => x.CreatedTime)
                        .Select(x => new List<float> { x.Longitude, x.Latitude }).ToList(),

                    Last = item.OrderByDescending(x => x.CreatedTime)
                        .Select(x => new List<float> { x.Longitude, x.Latitude })
                        .FirstOrDefault(),
                });
            }

            //Result
            pipeline.Status = ResponseStatus.Successful;
            pipeline.Result = new
            {
                Points,
                Routes,
            };
        }
    }
}
