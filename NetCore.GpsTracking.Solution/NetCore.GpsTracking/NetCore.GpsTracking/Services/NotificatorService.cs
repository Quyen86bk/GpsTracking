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
    public partial interface INotificatorService
    {
        Task Save(Pipeline pipeline, Notificator model);

        Task Get(Pipeline pipeline, Guid id);

        Task GetPage(Pipeline pipeline, PageFilterVM filter);

        Task GetList(Pipeline pipeline, ListFilterVM filter);

        Task Delete(Pipeline pipeline, List<Guid> IDs);

        Task GetNotificators(Pipeline pipeline);
    }

    public partial class NotificatorService : _ServiceGpsTracking, INotificatorService
    {
        public NotificatorService(IServiceProvider _ServiceProvider)
            : base(_ServiceProvider)
        {
        }

        public async Task Save(Pipeline pipeline, Notificator model)
        {
            Notificator Notificator = null;
            if (lib.NotSelected(model.Id))
            {
                model.Id = Guid.NewGuid();
                Notificator = new Notificator
                {
                    Id = model.Id,
                    CreatedById = pipeline.UserId
                };
                DBs.Notificator.Insert(Notificator);
            }
            else
            {                
                Notificator = await DBs.Notificator.Query
                    .FirstOrDefaultAsync(x => x.Id == model.Id);
            }

            if (Notificator != null)
            {
                //UpdatedBy
                Notificator.UpdatedById = pipeline.UserId;
                Notificator.UpdatedTime = lib.Time;

                //Set
                Notificator.Name = model.Name;

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
            var model = await DBs.Notificator.Query
                .FirstOrDefaultAsync(x => x.Id == id);

            pipeline.Status = ResponseStatus.Successful;
            pipeline.Result = new { model };
        }

        public async Task GetPage(Pipeline pipeline, PageFilterVM filter)
        {
            //Select
            var Notificators = DBs.Notificator.Query;

            //FilterData

            ////filter.RelationId
            //if (lib.Selected(filter.RelationId))
            //{
            //    if (filter.Option == 2)
            //        Notificators = Notificators.Where(x => x.ParentId == filter.RelationId);
            //    else
            //        Notificators = Notificators.Where(x => x.ParentId != filter.RelationId);
            //}

            //filter.Keyword
            if (lib.Selected(filter.Keyword))
                Notificators = Notificators.Where(x => x.Name.Contains(filter._Keyword));

            //filter.Date
            if (filter.HaveDate())
                Notificators = Notificators.Where(x => x.CreatedTime >= filter.FromDate && x.CreatedTime <= filter.ToDate);

            //Order
            Notificators = Notificators.OrderByDescending(x => x.CreatedTime);

            //Models
            var Total = Notificators.Count();
            var Models = await Notificators
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
            var Notificators = DBs.Notificator.Query;

            //filter
            if (lib.Selected(filter.Id))
                Notificators = Notificators.Where(x => x.Id == filter.Id);

            //if (lib.Selected(filter.ParentId))
                //Notificators = Notificators.Where(x => x.ParentId == filter.ParentId);

            if (lib.Selected(filter.ExcludeId))
                Notificators = Notificators.Where(x => x.Id != filter.ExcludeId);

            if (lib.Selected(filter.Keyword))
                Notificators = Notificators.Where(x => x.Name.Contains(filter._Keyword));

            //Order
            Notificators = Notificators.OrderBy(x => x.Name);

            //Models
            var List = await Notificators
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
            var Notificators = DBs.Notificator.Query.Where(x => IDs.Any(y => y == x.Id));
            await Notificators.ForEachAsync(x =>
            {
                x.IsDeleted = true;
                x.DeletedById = pipeline.UserId;
                x.DeletedTime = lib.Time;
            });

            await DBs.Save();
            pipeline.Status = ResponseStatus.Successful;
        }

        public async Task GetNotificators(Pipeline pipeline)
        {
            //Select
            var Notificators = DBs.Notificator.Query;

            //Order
            Notificators = Notificators.OrderBy(x => x.Name);

            //Models
            var Models = await Notificators
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
