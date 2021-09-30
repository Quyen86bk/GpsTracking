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
    public partial interface IPermissionService
    {
        Task Save(Pipeline pipeline, Permission model);

        Task Get(Pipeline pipeline, Guid id);

        Task GetPage(Pipeline pipeline, PageFilterVM filter);

        Task GetList(Pipeline pipeline, ListFilterVM filter);

        Task Delete(Pipeline pipeline, List<Guid> IDs);
    }

    public partial class PermissionService : _ServiceGpsTracking, IPermissionService
    {
        public PermissionService(IServiceProvider _ServiceProvider)
            : base(_ServiceProvider)
        {
        }

        public async Task Save(Pipeline pipeline, Permission model)
        {
            Permission Permission = null;
            if (lib.NotSelected(model.Id))
            {
                model.Id = Guid.NewGuid();
                Permission = new Permission
                {
                    Id = model.Id,
                    CreatedById = pipeline.UserId
                };
                DBs.Permission.Insert(Permission);
            }
            else
            {
                Permission = await DBs.Permission.Query
                    .FirstOrDefaultAsync(x => x.Id == model.Id);
            }

            if (Permission != null)
            {
                //UpdatedBy
                Permission.UpdatedById = pipeline.UserId;
                Permission.UpdatedTime = lib.Time;

                //Set
                Permission.ReadOnly = model.ReadOnly;
                Permission.Registration = model.Registration;

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
            var model = await DBs.Permission.Query
                .FirstOrDefaultAsync(x => x.Id == id);

            pipeline.Status = ResponseStatus.Successful;
            pipeline.Result = new { model };
        }

        public async Task GetPage(Pipeline pipeline, PageFilterVM filter)
        {
            //Select
            var Permissions = DBs.Permission.Query;

            //FilterData

            ////filter.RelationId
            //if (lib.Selected(filter.RelationId))
            //{
            //    if (filter.Option == 2)
            //        Permissions = Permissions.Where(x => x.ParentId == filter.RelationId);
            //    else
            //        Permissions = Permissions.Where(x => x.ParentId != filter.RelationId);
            //}

            //filter.Keyword
            //if (lib.Selected(filter.Keyword))
              //  Permissions = Permissions.Where(x => x.Name.Contains(filter._Keyword));

            //filter.Date
            if (filter.HaveDate())
                Permissions = Permissions.Where(x => x.CreatedTime >= filter.FromDate && x.CreatedTime <= filter.ToDate);

            //Order
            Permissions = Permissions.OrderByDescending(x => x.CreatedTime);

            //Models
            var Total = Permissions.Count();
            var Models = await Permissions
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
            var Permissions = DBs.Permission.Query;

            //filter
            if (lib.Selected(filter.Id))
                Permissions = Permissions.Where(x => x.Id == filter.Id);

            //if (lib.Selected(filter.ParentId))
                //Permissions = Permissions.Where(x => x.ParentId == filter.ParentId);

            if (lib.Selected(filter.ExcludeId))
                Permissions = Permissions.Where(x => x.Id != filter.ExcludeId);

            //if (lib.Selected(filter.Keyword))
                //Permissions = Permissions.Where(x => x.Name.Contains(filter._Keyword));

            //Order
            //Permissions = Permissions.OrderBy(x => x.Name);

            //Models
            var List = await Permissions
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
            var Permissions = DBs.Permission.Query.Where(x => IDs.Any(y => y == x.Id));
            await Permissions.ForEachAsync(x =>
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
