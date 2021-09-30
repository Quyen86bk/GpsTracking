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
    public partial interface ICommandService
    {
        Task Save(Pipeline pipeline, Command model);

        Task Get(Pipeline pipeline, Guid id);

        Task GetPage(Pipeline pipeline, PageFilterVM filter);

        Task GetList(Pipeline pipeline, ListFilterVM filter);

        Task Delete(Pipeline pipeline, List<Guid> IDs);
    }

    public partial class CommandService : _ServiceGpsTracking, ICommandService
    {
        public CommandService(IServiceProvider _ServiceProvider)
            : base(_ServiceProvider)
        {
        }

        public async Task Save(Pipeline pipeline, Command model)
        {
            Command Command = null;
            if (lib.NotSelected(model.Id))
            {
                model.Id = Guid.NewGuid();
                Command = new Command
                {
                    Id = model.Id,
                    CreatedById = pipeline.UserId
                };
                DBs.Command.Insert(Command);
            }
            else
            {                
                Command = await DBs.Command.Query
                    .FirstOrDefaultAsync(x => x.Id == model.Id);
            }

            if (Command != null)
            {
                //UpdatedBy
                Command.UpdatedById = pipeline.UserId;
                Command.UpdatedTime = lib.Time;

                //Set
                Command.Name = model.Name;

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
            var model = await DBs.Command.Query
                .FirstOrDefaultAsync(x => x.Id == id);

            pipeline.Status = ResponseStatus.Successful;
            pipeline.Result = new { model };
        }

        public async Task GetPage(Pipeline pipeline, PageFilterVM filter)
        {
            //Select
            var Commands = DBs.Command.Query;

            //FilterData

            ////filter.RelationId
            //if (lib.Selected(filter.RelationId))
            //{
            //    if (filter.Option == 2)
            //        Commands = Commands.Where(x => x.ParentId == filter.RelationId);
            //    else
            //        Commands = Commands.Where(x => x.ParentId != filter.RelationId);
            //}

            //filter.Keyword
            if (lib.Selected(filter.Keyword))
                Commands = Commands.Where(x => x.Name.Contains(filter._Keyword));

            //filter.Date
            if (filter.HaveDate())
                Commands = Commands.Where(x => x.CreatedTime >= filter.FromDate && x.CreatedTime <= filter.ToDate);

            //Order
            Commands = Commands.OrderByDescending(x => x.CreatedTime);

            //Models
            var Total = Commands.Count();
            var Models = await Commands
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
            var Commands = DBs.Command.Query;

            //filter
            if (lib.Selected(filter.Id))
                Commands = Commands.Where(x => x.Id == filter.Id);

            //if (lib.Selected(filter.ParentId))
                //Commands = Commands.Where(x => x.ParentId == filter.ParentId);

            if (lib.Selected(filter.ExcludeId))
                Commands = Commands.Where(x => x.Id != filter.ExcludeId);

            if (lib.Selected(filter.Keyword))
                Commands = Commands.Where(x => x.Name.Contains(filter._Keyword));

            //Order
            Commands = Commands.OrderBy(x => x.Name);

            //Models
            var List = await Commands
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
            var Commands = DBs.Command.Query.Where(x => IDs.Any(y => y == x.Id));
            await Commands.ForEachAsync(x =>
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
