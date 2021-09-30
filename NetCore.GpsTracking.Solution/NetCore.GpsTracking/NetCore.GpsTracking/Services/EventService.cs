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
    public partial interface IEventService
    {
        Task Save(Pipeline pipeline, Event model);

        Task Get(Pipeline pipeline, Guid id);

        Task GetPage(Pipeline pipeline, PageFilterVM filter);

        Task GetList(Pipeline pipeline, ListFilterVM filter);

        Task Delete(Pipeline pipeline, List<Guid> IDs);
    }

    public partial class EventService : _ServiceGpsTracking, IEventService
    {
        public EventService(IServiceProvider _ServiceProvider)
            : base(_ServiceProvider)
        {
        }

        public async Task Save(Pipeline pipeline, Event model)
        {
            Event Event = null;
            if (lib.NotSelected(model.Id))
            {
                model.Id = Guid.NewGuid();
                Event = new Event
                {
                    Id = model.Id,
                    CreatedById = pipeline.UserId
                };
                DBs.Event.Insert(Event);
            }
            else
            {
                Event = await DBs.Event.Query
                    .FirstOrDefaultAsync(x => x.Id == model.Id);
            }

            if (Event != null)
            {
                //UpdatedBy
                Event.UpdatedById = pipeline.UserId;
                Event.UpdatedTime = lib.Time;

                //Set
                Event.EventName = model.EventName;
                Event.EventTime = model.EventTime;
                Event.GeofenceId = model.GeofenceId;
                Event.LocationId = model.LocationId;

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
            var model = await DBs.Event.Query
                .FirstOrDefaultAsync(x => x.Id == id);

            pipeline.Status = ResponseStatus.Successful;
            pipeline.Result = new { model };
        }

        public async Task GetPage(Pipeline pipeline, PageFilterVM filter)
        {
            //Select
            var Events = DBs.Event.Query;

            //FilterData

            ////filter.RelationId
            //if (lib.Selected(filter.RelationId))
            //{
            //    if (filter.Option == 2)
            //        Events = Events.Where(x => x.ParentId == filter.RelationId);
            //    else
            //        Events = Events.Where(x => x.ParentId != filter.RelationId);
            //}

            //filter.Keyword
            if (lib.Selected(filter.Keyword))
                Events = Events.Where(x => x.EventName.Contains(filter._Keyword));

            //filter.Date
            if (filter.HaveDate())
                Events = Events.Where(x => x.CreatedTime >= filter.FromDate && x.CreatedTime <= filter.ToDate);

            //Order
            Events = Events.OrderByDescending(x => x.CreatedTime);

            //Models
            var Total = Events.Count();
            var Models = await Events
                .Skip(filter.From).Take(Config.MaxSelectPage(filter.From, filter.To))
                .ToListAsync();

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
            var Events = DBs.Event.Query;

            //filter
            if (lib.Selected(filter.Id))
                Events = Events.Where(x => x.Id == filter.Id);

            //if (lib.Selected(filter.ParentId))
            //Events = Events.Where(x => x.ParentId == filter.ParentId);

            if (lib.Selected(filter.ExcludeId))
                Events = Events.Where(x => x.Id != filter.ExcludeId);

            if (lib.Selected(filter.Keyword))
                Events = Events.Where(x => x.EventName.Contains(filter._Keyword));

            //Order
            Events = Events.OrderBy(x => x.EventName);

            //Models
            var List = await Events
                .Take(Config.MaxSelectList(filter.Keyword))
                .ToListAsync();

            var Models = new List<ListVM>();
            foreach (var item in List)
            {
                Models.Add(new ListVM
                {
                    Value = item.Id,
                    Name = item.EventName,
                    Info = item.EventName,
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
            var Events = DBs.Event.Query.Where(x => IDs.Any(y => y == x.Id));
            await Events.ForEachAsync(x =>
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
