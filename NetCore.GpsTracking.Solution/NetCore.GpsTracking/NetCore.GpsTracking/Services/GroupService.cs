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
    public partial interface IGroupService
    {
        Task Save(Pipeline pipeline, Group model);

        Task Get(Pipeline pipeline, Guid id);

        Task GetPage(Pipeline pipeline, PageFilterVM filter);

        Task GetList(Pipeline pipeline, ListFilterVM filter);

        Task Delete(Pipeline pipeline, List<Guid> IDs);
    }

    public partial class GroupService : _ServiceGpsTracking, IGroupService
    {
        public GroupService(IServiceProvider _ServiceProvider)
            : base(_ServiceProvider)
        {
        }

        public async Task Save(Pipeline pipeline, Group model)
        {
            Group Group = null;
            if (lib.NotSelected(model.Id))
            {
                model.Id = Guid.NewGuid();
                Group = new Group
                {
                    Id = model.Id,
                    CreatedById = pipeline.UserId
                };
                DBs.Group.Insert(Group);
            }
            else
            {                
                Group = await DBs.Group.Query
                    .FirstOrDefaultAsync(x => x.Id == model.Id);
            }

            if (Group != null)
            {
                //UpdatedBy
                Group.UpdatedById = pipeline.UserId;
                Group.UpdatedTime = lib.Time;

                //Set
                Group.Name = model.Name;

                //Remove GpsDeviceMappings

                var currentGpsDeviceMappings = DBs.GpsDeviceMapping.Query.Include(x => x.GpsDevice).Where(x => x.GroupId == Group.Id).ToList();
                var newGpsDeviceMappings = model.GpsDevices;
                var removedGpsDeviceMappings = currentGpsDeviceMappings.Where(x => !newGpsDeviceMappings.Any(y => y == x.GpsDevice.Name)).ToList();
                foreach (var item in removedGpsDeviceMappings)
                {
                    DBs.GpsDeviceMapping.Delete(item);
                }

                //Add new GpsDeviceMappings

                foreach (var item in model.GpsDevices)
                {
                    var gpsDevice = DBs.GpsDevice.Query.FirstOrDefault(x => x.Name == item);
                    if (gpsDevice != null)
                    {
                        var mapping = DBs.GpsDeviceMapping.Query.FirstOrDefault(x => x.GpsDeviceId == gpsDevice.Id && x.GroupId == Group.Id);
                        if (mapping == null)
                        {
                            mapping = new GpsDeviceMapping
                            {
                                GpsDeviceId = gpsDevice.Id,
                                GroupId = Group.Id,
                            };
                            DBs.GpsDeviceMapping.Insert(mapping);
                        }
                    }
                }

                //Remove NotificationMappings

                var currentNotificationMappings = DBs.NotificationMapping.Query.Include(x => x.Notification).Where(x => x.GroupId == Group.Id).ToList();
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
                        var mapping = DBs.NotificationMapping.Query.FirstOrDefault(x => x.NotificationId == notification.Id && x.GroupId == Group.Id);
                        if (mapping == null)
                        {
                            mapping = new NotificationMapping
                            {
                                NotificationId = notification.Id,
                                GroupId = Group.Id,
                            };
                            DBs.NotificationMapping.Insert(mapping);
                        }
                    }
                }

                //Remove GeofenceMappings

                var currentGeofenceMappings = DBs.GeofenceMapping.Query.Include(x => x.Geofence).Where(x => x.GroupId == Group.Id).ToList();
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
                        var mapping = DBs.GeofenceMapping.Query.FirstOrDefault(x => x.GeofenceId == geofence.Id && x.GroupId == Group.Id);
                        if (mapping == null)
                        {
                            mapping = new GeofenceMapping
                            {
                                GeofenceId = geofence.Id,
                                GroupId = Group.Id,
                            };
                            DBs.GeofenceMapping.Insert(mapping);
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
            var model = await DBs.Group.Query
                .Include(x => x.GpsDeviceMappings).ThenInclude(x => x.GpsDevice)
                .Include(x => x.NotificationMappings).ThenInclude(x => x.Notification)
                .Include(x => x.GeofenceMappings).ThenInclude(x => x.Geofence)
                .FirstOrDefaultAsync(x => x.Id == id);

            model.GpsDevices = model.GpsDeviceMappings.Select(x => x.GpsDevice.Name).ToList();
            model.Notifications = model.NotificationMappings.Select(x => x.Notification.Name).ToList();
            model.Geofences = model.GeofenceMappings.Select(x => x.Geofence.Name).ToList();

            pipeline.Status = ResponseStatus.Successful;
            pipeline.Result = new { model };
        }

        public async Task GetPage(Pipeline pipeline, PageFilterVM filter)
        {
            //Select
            var Groups = DBs.Group.Query;

            //FilterData

            ////filter.RelationId
            //if (lib.Selected(filter.RelationId))
            //{
            //    if (filter.Option == 2)
            //        Groups = Groups.Where(x => x.ParentId == filter.RelationId);
            //    else
            //        Groups = Groups.Where(x => x.ParentId != filter.RelationId);
            //}

            //filter.Keyword
            if (lib.Selected(filter.Keyword))
                Groups = Groups.Where(x => x.Name.Contains(filter._Keyword));

            //filter.Date
            if (filter.HaveDate())
                Groups = Groups.Where(x => x.CreatedTime >= filter.FromDate && x.CreatedTime <= filter.ToDate);

            //Order
            Groups = Groups.OrderByDescending(x => x.CreatedTime);

            //Models
            var Total = Groups.Count();
            var Models = await Groups
                .Include(x => x.GpsDeviceMappings).ThenInclude(x => x.GpsDevice)
                .Include(x => x.NotificationMappings).ThenInclude(x => x.Notification)
                .Include(x => x.GeofenceMappings).ThenInclude(x => x.Geofence)
                .Skip(filter.From).Take(Config.MaxSelectPage(filter.From, filter.To))
                .ToListAsync();

            foreach (var model in Models)
            {
                //GpsDeviceMappings
                foreach (var mapping in model.GpsDeviceMappings.Select(x => x.GpsDevice.Name).ToList())
                    model.ListGpsDevices += mapping + ", ";
                model.ListGpsDevices = lib.RemoveLast(model.ListGpsDevices, ",");

                //NotificationMappings
                foreach (var mapping in model.NotificationMappings.Select(x => x.Notification.Name).ToList())
                    model.ListNotifications += mapping + ", ";
                model.ListNotifications = lib.RemoveLast(model.ListNotifications, ",");

                //GeofenceMappings
                foreach (var mapping in model.GeofenceMappings.Select(x => x.Geofence.Name).ToList())
                    model.ListGeofences += mapping + ", ";
                model.ListGeofences = lib.RemoveLast(model.ListGeofences, ",");
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
            var Groups = DBs.Group.Query;

            //filter
            if (lib.Selected(filter.Id))
                Groups = Groups.Where(x => x.Id == filter.Id);

            //if (lib.Selected(filter.ParentId))
                //Groups = Groups.Where(x => x.ParentId == filter.ParentId);

            if (lib.Selected(filter.ExcludeId))
                Groups = Groups.Where(x => x.Id != filter.ExcludeId);

            if (lib.Selected(filter.Keyword))
                Groups = Groups.Where(x => x.Name.Contains(filter._Keyword));

            //Order
            Groups = Groups.OrderBy(x => x.Name);

            //Models
            var List = await Groups
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
            var Groups = DBs.Group.Query.Where(x => IDs.Any(y => y == x.Id));
            await Groups.ForEachAsync(x =>
            {
                x.IsDeleted = true;
                x.DeletedById = pipeline.UserId;
                x.DeletedTime = lib.Time;
            });

            await DBs.Save();
            pipeline.Status = ResponseStatus.Successful;
        }
    }
}
