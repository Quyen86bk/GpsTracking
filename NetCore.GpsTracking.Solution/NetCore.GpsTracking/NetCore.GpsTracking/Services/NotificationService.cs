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
    public partial interface INotificationService
    {
        Task Save(Pipeline pipeline, Notification model);

        Task Get(Pipeline pipeline, Guid id);

        Task GetPage(Pipeline pipeline, PageFilterVM filter);

        Task GetList(Pipeline pipeline, ListFilterVM filter);

        Task Delete(Pipeline pipeline, List<Guid> IDs);

        Task GetNotifications(Pipeline pipeline); 
    }

    public partial class NotificationService : _ServiceGpsTracking, INotificationService
    {
        public NotificationService(IServiceProvider _ServiceProvider)
            : base(_ServiceProvider)
        {
        }

        public async Task Save(Pipeline pipeline, Notification model)
        {
            Notification Notification = null;
            if (lib.NotSelected(model.Id))
            {
                model.Id = Guid.NewGuid();
                Notification = new Notification
                {
                    Id = model.Id,
                    CreatedById = pipeline.UserId
                };
                DBs.Notification.Insert(Notification);
            }
            else
            {
                Notification = await DBs.Notification.Query
                    .FirstOrDefaultAsync(x => x.Id == model.Id);
            }

            if (Notification != null)
            {
                //UpdatedBy
                Notification.UpdatedById = pipeline.UserId;
                Notification.UpdatedTime = lib.Time;

                //Set
                Notification.Name = model.Name;
                Notification.Distribution = model.Distribution;

                //Remove NotificatorMappings

                var currentNotificatorMappings = DBs.NotificatorMapping.Query.Include(x => x.Notificator).Where(x => x.NotificationId == Notification.Id).ToList();
                var newNotificatorMappings = model.Notificators;
                var removedNotificatorMappings = currentNotificatorMappings.Where(x => !newNotificatorMappings.Any(y => y == x.Notificator.Name)).ToList();
                foreach (var item in removedNotificatorMappings)
                {
                    DBs.NotificatorMapping.Delete(item);
                }

                //Add new NotificatorMappings

                foreach (var item in model.Notificators)
                {
                    var notificator = DBs.Notificator.Query.FirstOrDefault(x => x.Name == item);
                    if (notificator != null)
                    {
                        var mapping = DBs.NotificatorMapping.Query.FirstOrDefault(x => x.NotificatorId == notificator.Id && x.NotificationId == Notification.Id);
                        if (mapping == null)
                        {
                            mapping = new NotificatorMapping
                            {
                                NotificatorId = notificator.Id,
                                NotificationId = Notification.Id,
                            };
                            DBs.NotificatorMapping.Insert(mapping);
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
            var model = await DBs.Notification.Query
                .Include(x => x.NotificatorMappings).ThenInclude(x => x.Notificator)
                .FirstOrDefaultAsync(x => x.Id == id);

            model.Notificators = model.NotificatorMappings.Select(x => x.Notificator.Name).ToList();

            pipeline.Status = ResponseStatus.Successful;
            pipeline.Result = new { model };
        }

        public async Task GetPage(Pipeline pipeline, PageFilterVM filter)
        {
            //Select
            var Notifications = DBs.Notification.Query;

            //FilterData

            ////filter.RelationId
            //if (lib.Selected(filter.RelationId))
            //{
            //    if (filter.Option == 2)
            //        Notifications = Notifications.Where(x => x.ParentId == filter.RelationId);
            //    else
            //        Notifications = Notifications.Where(x => x.ParentId != filter.RelationId);
            //}

            //filter.Keyword
            if (lib.Selected(filter.Keyword))
                Notifications = Notifications.Where(x => x.Name.Contains(filter._Keyword));

            //filter.Date
            if (filter.HaveDate())
                Notifications = Notifications.Where(x => x.CreatedTime >= filter.FromDate && x.CreatedTime <= filter.ToDate);

            //Order
            Notifications = Notifications.OrderByDescending(x => x.CreatedTime);

            //Models
            var Total = Notifications.Count();
            var Models = await Notifications
                .Include(x => x.NotificatorMappings).ThenInclude(x => x.Notificator)
                .Skip(filter.From).Take(Config.MaxSelectPage(filter.From, filter.To))
                .ToListAsync();

            foreach (var model in Models)
            {
                //NotificatorMappings
                foreach (var mapping in model.NotificatorMappings.Select(x => x.Notificator.Name).ToList())
                    model.ListNotificators += mapping + ", ";
                model.ListNotificators = lib.RemoveLast(model.ListNotificators, ",");
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
            var Notifications = DBs.Notification.Query;

            //filter
            if (lib.Selected(filter.Id))
                Notifications = Notifications.Where(x => x.Id == filter.Id);

            //if (lib.Selected(filter.ParentId))
            //Notifications = Notifications.Where(x => x.ParentId == filter.ParentId);

            if (lib.Selected(filter.ExcludeId))
                Notifications = Notifications.Where(x => x.Id != filter.ExcludeId);

            if (lib.Selected(filter.Keyword))
                Notifications = Notifications.Where(x => x.Name.Contains(filter._Keyword));

            //Order
            Notifications = Notifications.OrderBy(x => x.Name);

            //Models
            var List = await Notifications
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
            var Notifications = DBs.Notification.Query.Where(x => IDs.Any(y => y == x.Id));
            await Notifications.ForEachAsync(x =>
            {
                x.IsDeleted = true;
                x.DeletedById = pipeline.UserId;
                x.DeletedTime = lib.Time;
            });

            await DBs.Save();
            pipeline.Status = ResponseStatus.Successful;
        }

        public async Task GetNotifications(Pipeline pipeline)
        {
            //Select
            var Notifications = DBs.Notification.Query;

            //Order
            Notifications = Notifications.OrderBy(x => x.Name);

            //Models
            var Models = await Notifications
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
