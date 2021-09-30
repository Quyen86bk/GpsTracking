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

namespace NetCore.GpsTrackingModule.Services
{
    public partial interface IGpsDeviceService
    {
        Task Save(Pipeline pipeline, GpsDevice model);

        Task Get(Pipeline pipeline, Guid id);

        Task GetPage(Pipeline pipeline, PageFilterVM filter);

        Task GetList(Pipeline pipeline, ListFilterVM filter);

        Task Delete(Pipeline pipeline, List<Guid> IDs);

        Task GetGpsDevices(Pipeline pipeline);
    }

    public partial class GpsDeviceService : _ServiceGpsTracking, IGpsDeviceService
    {
        public GpsDeviceService(IServiceProvider _ServiceProvider)
            : base(_ServiceProvider)
        {
        }

        public async Task Save(Pipeline pipeline, GpsDevice model)
        {
            GpsDevice GpsDevice = null;
            if (lib.NotSelected(model.Id))
            {
                model.Id = Guid.NewGuid();
                GpsDevice = new GpsDevice
                {
                    Id = model.Id,
                    CreatedById = pipeline.UserId
                };
                DBs.GpsDevice.Insert(GpsDevice);
            }
            else
            {
                GpsDevice = await DBs.GpsDevice.Query
                    .FirstOrDefaultAsync(x => x.Id == model.Id);
            }

            if (GpsDevice != null)
            {
                //UpdatedBy
                GpsDevice.UpdatedById = pipeline.UserId;
                GpsDevice.UpdatedTime = lib.Time;

                //Set
                GpsDevice.Name = model.Name;
                GpsDevice.Code = model.Code;
                GpsDevice.Phone = model.Phone;
                GpsDevice.Category = model.Category;

                //Remove GeofenceMappings

                var currentGeofenceMappings = DBs.GeofenceMapping.Query.Include(x => x.Geofence).Where(x => x.GpsDeviceId == GpsDevice.Id).ToList();
                var newGeofenceMappings = model.Geofences;
                var removedGeofenceMappings = currentGeofenceMappings.Where(x => !newGeofenceMappings.Any(y => y == x.Geofence.Name)).ToList();
                foreach (var item in removedGeofenceMappings)
                {
                    DBs.GeofenceMapping.Delete(item);
                }

                //Add new GeofenceMappings

                foreach (var item in model.Geofences)
                {
                    var geofence = DBs.Geofence.Query.FirstOrDefault(x => x.Name == item);
                    if (geofence != null)
                    {
                        var mapping = DBs.GeofenceMapping.Query.FirstOrDefault(x => x.GeofenceId == geofence.Id && x.GpsDeviceId == GpsDevice.Id);
                        if (mapping == null)
                        {
                            mapping = new GeofenceMapping
                            {
                                GeofenceId = geofence.Id,
                                GpsDeviceId = GpsDevice.Id,
                            };
                            DBs.GeofenceMapping.Insert(mapping);
                        }
                    }
                }

                //Remove NotificationMappings

                var currentNotificationMappings = DBs.NotificationMapping.Query.Include(x => x.Notification).Where(x => x.GpsDeviceId == GpsDevice.Id).ToList();
                var newNotificationMappings = model.Notifications;
                var removedNotificationMappings = currentNotificationMappings.Where(x => !newNotificationMappings.Any(y => y == x.Notification.Name)).ToList();
                foreach (var item in removedNotificationMappings)
                {
                    DBs.NotificationMapping.Delete(item);
                }

                //Add new NotificationMappings

                foreach (var item in model.Notifications)
                {
                    var notification = DBs.Notification.Query.FirstOrDefault(x => x.Name == item);
                    if (notification != null)
                    {
                        var mapping = DBs.NotificationMapping.Query.FirstOrDefault(x => x.NotificationId == notification.Id && x.GpsDeviceId == GpsDevice.Id);
                        if (mapping == null)
                        {
                            mapping = new NotificationMapping
                            {
                                NotificationId = notification.Id,
                                GpsDeviceId = GpsDevice.Id,
                            };
                            DBs.NotificationMapping.Insert(mapping);
                        }
                    }
                }

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
            var model = await DBs.GpsDevice.Query
                .Include(x => x.GeofenceMappings).ThenInclude(x => x.Geofence)
                .Include(x => x.NotificationMappings).ThenInclude(x => x.Notification)
                .FirstOrDefaultAsync(x => x.Id == id);

            model.Geofences = model.GeofenceMappings.Select(x => x.Geofence.Name).ToList();
            model.Notifications = model.NotificationMappings.Select(x => x.Notification.Name).ToList();

            pipeline.Status = ResponseStatus.Successful;
            pipeline.Result = new { model };
        }

        public async Task GetPage(Pipeline pipeline, PageFilterVM filter)
        {
            //Select
            var GpsDevices = DBs.GpsDevice.Query;

            //FilterData

            ////filter.RelationId
            //if (lib.Selected(filter.RelationId))
            //{
            //    if (filter.Option == 2)
            //        GpsDevices = GpsDevices.Where(x => x.ParentId == filter.RelationId);
            //    else
            //        GpsDevices = GpsDevices.Where(x => x.ParentId != filter.RelationId);
            //}

            //filter.Keyword
            if (lib.Selected(filter.Keyword))
                GpsDevices = GpsDevices.Where(x => x.Name.Contains(filter._Keyword));

            //filter.Date
            if (filter.HaveDate())
                GpsDevices = GpsDevices.Where(x => x.CreatedTime >= filter.FromDate && x.CreatedTime <= filter.ToDate);

            //Order
            GpsDevices = GpsDevices.OrderByDescending(x => x.CreatedTime);

            //Models
            var Total = GpsDevices.Count();
            var Models = await GpsDevices
                .Include(x => x.GeofenceMappings).ThenInclude(x => x.Geofence)
                .Include(x => x.NotificationMappings).ThenInclude(x => x.Notification)
                .Skip(filter.From).Take(Config.MaxSelectPage(filter.From, filter.To))
                .ToListAsync();

            foreach (var model in Models)
            {
                //GeofenceMappings
                foreach (var mapping in model.GeofenceMappings.Select(x => x.Geofence.Name).ToList())
                    model.ListGeofences += mapping + ", ";
                model.ListGeofences = lib.RemoveLast(model.ListGeofences, ",");

                //NotificationMappings
                foreach (var mapping in model.NotificationMappings.Select(x => x.Notification.Name).ToList())
                    model.ListNotifications += mapping + ", ";
                model.ListNotifications = lib.RemoveLast(model.ListNotifications, ",");
            }

            //Result
            pipeline.Status = ResponseStatus.Successful;
            pipeline.Result = new
            {
                Models,
                Total,
                FilterData = new
                {
                }
            };
        }

        public async Task GetList(Pipeline pipeline, ListFilterVM filter)
        {
            //Select
            var GpsDevices = DBs.GpsDevice.Query;

            //filter
            if (lib.Selected(filter.Id))
                GpsDevices = GpsDevices.Where(x => x.Id == filter.Id);

            //if (lib.Selected(filter.ParentId))
            //GpsDevices = GpsDevices.Where(x => x.ParentId == filter.ParentId);

            if (lib.Selected(filter.ExcludeId))
                GpsDevices = GpsDevices.Where(x => x.Id != filter.ExcludeId);

            if (lib.Selected(filter.Keyword))
                GpsDevices = GpsDevices.Where(x => x.Name.Contains(filter._Keyword));

            //Order
            GpsDevices = GpsDevices.OrderBy(x => x.Name);

            //Models
            var List = await GpsDevices
                .Take(Config.MaxSelectList(filter.Keyword))
                .ToListAsync();

            var Models = new List<ListVM>();
            foreach (var item in List)
            {
                Models.Add(new ListVM
                {
                    Value = item.Id,
                    Name = item.Name,
                    Info = item.Name,
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
            var GpsDevices = DBs.GpsDevice.Query.Where(x => IDs.Any(y => y == x.Id));
            await GpsDevices.ForEachAsync(x =>
            {
                x.IsDeleted = true;
                x.DeletedById = pipeline.UserId;
                x.DeletedTime = lib.Time;
            });

            await DBs.Save();
            pipeline.Status = ResponseStatus.Successful;
        }

        public async Task GetGpsDevices(Pipeline pipeline)
        {
            //Select
            var GpsDevices = DBs.GpsDevice.Query;

            //Order
            GpsDevices = GpsDevices.OrderBy(x => x.Name);

            //Models
            var Models = await GpsDevices
                .Select(x => x.Name)
                .ToListAsync();

            //Result
            pipeline.Status = ResponseStatus.Successful;
            pipeline.Result = new
            {
                Models,
            };
        }
    }
}
