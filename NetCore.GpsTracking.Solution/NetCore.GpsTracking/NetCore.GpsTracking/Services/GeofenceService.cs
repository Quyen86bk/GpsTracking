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
    public partial interface IGeofenceService
    {
        Task Save(Pipeline pipeline, Geofence model);

        Task Get(Pipeline pipeline, Guid id);

        Task GetPage(Pipeline pipeline, PageFilterVM filter);

        Task GetList(Pipeline pipeline, ListFilterVM filter);

        Task Delete(Pipeline pipeline, List<Guid> IDs);

        Task GetGeofences(Pipeline pipeline);

        Task GetGeofencesNames(Pipeline pipeline);
    }

    public partial class GeofenceService : _ServiceGpsTracking, IGeofenceService
    {
        public GeofenceService(IServiceProvider _ServiceProvider)
            : base(_ServiceProvider)
        {
        }

        public async Task Save(Pipeline pipeline, Geofence model)
        {
            Geofence Geofence = null;
            if (lib.NotSelected(model.Id))
            {
                model.Id = Guid.NewGuid();
                Geofence = new Geofence
                {
                    Id = model.Id,
                    CreatedById = pipeline.UserId
                };
                DBs.Geofence.Insert(Geofence);
            }
            else
            {
                Geofence = await DBs.Geofence.Query
                    .FirstOrDefaultAsync(x => x.Id == model.Id);
            }

            if (Geofence != null)
            {
                //UpdatedBy
                Geofence.UpdatedById = pipeline.UserId;
                Geofence.UpdatedTime = lib.Time;

                //Set
                Geofence.Name = model.Name;
                Geofence.Note = model.Note;

                //Detail
                foreach (var item in model.Geofences)
                {
                    var detail = new GeofenceDetail
                    {
                        GeofenceId = Geofence.Id,
                        Longitude = item[0],
                        Latitude = item[1],
                    };
                    DBs.GeofenceDetail.Insert(detail);
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
            var model = await DBs.Geofence.Query
                .FirstOrDefaultAsync(x => x.Id == id);

            pipeline.Status = ResponseStatus.Successful;
            pipeline.Result = new { model };
        }

        public async Task GetPage(Pipeline pipeline, PageFilterVM filter)
        {
            //Select
            var Geofences = DBs.Geofence.Query;

            //FilterData

            ////filter.RelationId
            //if (lib.Selected(filter.RelationId))
            //{
            //    if (filter.Option == 2)
            //        Geofences = Geofences.Where(x => x.ParentId == filter.RelationId);
            //    else
            //        Geofences = Geofences.Where(x => x.ParentId != filter.RelationId);
            //}

            //filter.Keyword
            if (lib.Selected(filter.Keyword))
                Geofences = Geofences.Where(x => x.Name.Contains(filter._Keyword));

            //filter.Date
            if (filter.HaveDate())
                Geofences = Geofences.Where(x => x.CreatedTime >= filter.FromDate && x.CreatedTime <= filter.ToDate);

            //Order
            Geofences = Geofences.OrderByDescending(x => x.CreatedTime);

            //Models
            var Total = Geofences.Count();
            var Models = await Geofences
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
            var Geofences = DBs.Geofence.Query;

            //filter
            if (lib.Selected(filter.Id))
                Geofences = Geofences.Where(x => x.Id == filter.Id);

            //if (lib.Selected(filter.ParentId))
            //Geofences = Geofences.Where(x => x.ParentId == filter.ParentId);

            if (lib.Selected(filter.ExcludeId))
                Geofences = Geofences.Where(x => x.Id != filter.ExcludeId);

            if (lib.Selected(filter.Keyword))
                Geofences = Geofences.Where(x => x.Name.Contains(filter._Keyword));

            //Order
            Geofences = Geofences.OrderBy(x => x.Name);

            //Models
            var List = await Geofences
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
            var Geofences = DBs.Geofence.Query.Where(x => IDs.Any(y => y == x.Id));
            await Geofences.ForEachAsync(x =>
            {
                x.IsDeleted = true;
                x.DeletedById = pipeline.UserId;
                x.DeletedTime = lib.Time;
            });

            await DBs.Save();
            pipeline.Status = ResponseStatus.Successful;
        }

        public async Task GetGeofences(Pipeline pipeline)
        {
            //Select
            var Geofences = DBs.Geofence.Query;

            //Models
            var List = await Geofences
                .Include(x => x.Details)
                .ToListAsync();

            var Models = new List<GeofenceVM>();
            foreach (var item in List)
            {
                Models.Add(new GeofenceVM
                {
                    Id = item.Id,
                    Name = item.Name,
                    Note = item.Note,
                    Geofences = item.Details.OrderBy(x => x.CreatedTime).Select(x => new List<float> { x.Longitude, x.Latitude }).ToList(),
                });
            }

            //Result
            pipeline.Status = ResponseStatus.Successful;
            pipeline.Result = new
            {
                Models,
            };
        }

        public async Task GetGeofencesNames(Pipeline pipeline)
        {
            //Select
            var Geofences = DBs.Geofence.Query;

            //Order
            Geofences = Geofences.OrderBy(x => x.Name);

            //Models
            var Models = await Geofences
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
