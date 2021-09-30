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

        Task GetLocations(Pipeline pipeline);
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

        public async Task GetLocations(Pipeline pipeline)
        {
            //Select
            var Locations = DBs.Location.Query;

            //Order
            Locations = Locations.OrderByDescending(x => x.CreatedTime);

            //Models
            var List = await Locations
                .Include(x => x.GpsDevice)
                .GroupBy(x => x.GpsDeviceId)
                .ToListAsync();

            var Models = new List<LocationVM>();
            foreach (var item in List)
            {
                Models.Add(new LocationVM
                {
                    Id = item.FirstOrDefault().Id,
                    Code = item.FirstOrDefault().GpsDevice.Code,

                    Locations = item.Select(x => new List<float> { x.Longitude, x.Latitude }).ToList(),
                    Last = item.OrderBy(x => x.CreatedTime)
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
    }
}
