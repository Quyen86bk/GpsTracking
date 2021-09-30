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
    public partial interface IGeofenceDetailService
    {
        Task Save(Pipeline pipeline, GeofenceDetail model);

        Task Get(Pipeline pipeline, Guid id);

        Task GetPage(Pipeline pipeline, PageFilterVM filter);

        Task GetList(Pipeline pipeline, ListFilterVM filter);

        Task Delete(Pipeline pipeline, List<Guid> IDs);
    }

    public partial class GeofenceDetailService : _ServiceGpsTracking, IGeofenceDetailService
    {
        public GeofenceDetailService(IServiceProvider _ServiceProvider)
            : base(_ServiceProvider)
        {
        }

        public async Task Save(Pipeline pipeline, GeofenceDetail model)
        {
            GeofenceDetail GeofenceDetail = null;
            if (lib.NotSelected(model.Id))
            {
                model.Id = Guid.NewGuid();
                GeofenceDetail = new GeofenceDetail
                {
                    Id = model.Id,
                    CreatedById = pipeline.UserId
                };
                DBs.GeofenceDetail.Insert(GeofenceDetail);
            }
            else
            {                
                GeofenceDetail = await DBs.GeofenceDetail.Query
                    .FirstOrDefaultAsync(x => x.Id == model.Id);
            }

            if (GeofenceDetail != null)
            {
                //UpdatedBy
                GeofenceDetail.UpdatedById = pipeline.UserId;
                GeofenceDetail.UpdatedTime = lib.Time;

                //Set
                GeofenceDetail.Longitude = model.Longitude;

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
            var model = await DBs.GeofenceDetail.Query
                .FirstOrDefaultAsync(x => x.Id == id);

            pipeline.Status = ResponseStatus.Successful;
            pipeline.Result = new { model };
        }

        public async Task GetPage(Pipeline pipeline, PageFilterVM filter)
        {
            //Select
            var GeofenceDetails = DBs.GeofenceDetail.Query;

            //FilterData

            ////filter.RelationId
            //if (lib.Selected(filter.RelationId))
            //{
            //    if (filter.Option == 2)
            //        GeofenceDetails = GeofenceDetails.Where(x => x.ParentId == filter.RelationId);
            //    else
            //        GeofenceDetails = GeofenceDetails.Where(x => x.ParentId != filter.RelationId);
            //}

            //filter.Keyword
            //if (lib.Selected(filter.Keyword))
            //    GeofenceDetails = GeofenceDetails.Where(x => x.Name.Contains(filter._Keyword));

            //filter.Date
            if (filter.HaveDate())
                GeofenceDetails = GeofenceDetails.Where(x => x.CreatedTime >= filter.FromDate && x.CreatedTime <= filter.ToDate);

            //Order
            GeofenceDetails = GeofenceDetails.OrderByDescending(x => x.CreatedTime);

            //Models
            var Total = GeofenceDetails.Count();
            var Models = await GeofenceDetails
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
            var GeofenceDetails = DBs.GeofenceDetail.Query;

            //filter
            if (lib.Selected(filter.Id))
                GeofenceDetails = GeofenceDetails.Where(x => x.Id == filter.Id);

            //if (lib.Selected(filter.ParentId))
                //GeofenceDetails = GeofenceDetails.Where(x => x.ParentId == filter.ParentId);

            if (lib.Selected(filter.ExcludeId))
                GeofenceDetails = GeofenceDetails.Where(x => x.Id != filter.ExcludeId);

            //if (lib.Selected(filter.Keyword))
            //    GeofenceDetails = GeofenceDetails.Where(x => x.Name.Contains(filter._Keyword));

            ////Order
            //GeofenceDetails = GeofenceDetails.OrderBy(x => x.Name);

            //Models
            var List = await GeofenceDetails
                .Take(Config.MaxSelectList(filter.Keyword))
                .ToListAsync();

            var Models = new List<ListVM>();
            foreach (var item in List)
            {
                Models.Add(new ListVM
                {
                    Value = item.Id,
                    //Name = item.Name,
                    //Info = item.Name,
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
            var GeofenceDetails = DBs.GeofenceDetail.Query.Where(x => IDs.Any(y => y == x.Id));
            await GeofenceDetails.ForEachAsync(x =>
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
