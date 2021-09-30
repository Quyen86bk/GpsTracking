using NetCore.CommonModule;
using NetCore.CommonModule.Controllers;
using NetCore.Library;
using NetCore.ManageModule;
using NetCore.ManageModule.Controllers;
using NetCore.Project.Controllers;
using NetCore.Websites;
using NetCore.Websites.Models;
using NetCore.Websites.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore.Project.Services
{
    public interface ISystemService
    {
        Task PrepareServerCache(Guid key, int index);
        Task GetClientConfig(Pipeline pipeline, string platformName);

        Task Prepare(Pipeline pipeline, List<PrepareVM> Models);
        Task UpdateLocalCache(Pipeline pipeline, List<PrepareVM> Models);
    }

    public class SystemService : _ServiceMain, ISystemService
    {
        public SystemService(IServiceProvider _ServiceProvider)
            : base(_ServiceProvider)
        {
        }

        public async Task PrepareServerCache(Guid key, int index)
        {
            try
            {
                if (key == WebApp.HomePageKey)
                {
                    //DBs
                    var ManageDBs = new ManageRepository(ServiceProvider);
                    var CommonDBs = new CommonRepository(ServiceProvider);

                    //CacheActions
                    var CacheActions = new Task[] {
                        //Manage
                        new Task(() => ManageRepository.CacheDevices(ManageDBs)),

                        //Common
                        new Task(() => CommonRepository.CacheUnits(CommonDBs)),
                    };

                    if (index == -1)
                    {
                        for (int i = 0; i < CacheActions.Length; i++)
                            NetCore.Websites.Startup.CachePrepare_Module(i);
                    }
                    else if (CacheActions.Length > index)
                    {
                        CacheActions[index].Start();
                        CacheActions[index].Wait();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.SOS(ex);
            }
        }
        public async Task GetClientConfig(Pipeline pipeline, string platformName)
        {
            pipeline.Status = ResponseStatus.Successful;
            pipeline.Result = new
            {
                GpsTracking = new
                {
                },
            };
        }

        public async Task Prepare(Pipeline pipeline, List<PrepareVM> Models)
        {
            await GetLocalCache(pipeline, Models);

            pipeline.Status = ResponseStatus.Successful;
            pipeline.Result = new
            {
                Models
            };
        }
        public async Task UpdateLocalCache(Pipeline pipeline, List<PrepareVM> Models)
        {
            await GetLocalCache(pipeline, Models);

            pipeline.Status = ResponseStatus.Successful;
            pipeline.Result = new
            {
                Models
            };
        }

        async Task GetLocalCache(Pipeline pipeline, List<PrepareVM> Models)
        {
            var EnabledCaches = new List<DBCacheKey>
            {
                //new DBCacheKey("Manage", "Device"),
                //new DBCacheKey("Common", "Unit"),                
            };

            foreach (var enabled in EnabledCaches)
            {
                string key = DBCache.GetKey(enabled.Module, enabled.Entity);
                if (lib.NotNullEmpty(key))
                {
                    bool update = false;

                    var model = Models.FirstOrDefault(x => x.Module == enabled.Module && x.Entity == enabled.Entity && x.Type == enabled.Type);
                    if (model == null)
                    {
                        update = true;
                        model = new PrepareVM
                        {
                            Module = enabled.Module,
                            Entity = enabled.Entity,
                            Type = enabled.Type,
                        };
                        Models.Add(model);
                    }
                    else
                    {
                        update = model.Key != key;
                    }

                    if (update)
                    {
                        model.Caches = await GetLocalCache(pipeline, model.Module, model.Entity);
                        if (model.Caches != null)
                            model.Key = key;
                        else
                            model.Key = null;
                    }
                }
            }
        }
        async Task<object> GetLocalCache(Pipeline pipeline, string module, string entity)
        {
            object result = null;

            var ManageServices = new _ServiceManage(ServiceProvider);
            var CommonServices = new _ServiceCommon(ServiceProvider);

            //if (module == "Manage")
            //{
            //    if (entity == "Device")
            //        result = await ManageServices.Services.Device.GetLocalCache(pipeline);
            //}
            //else if (module == "Common")
            //{
            //    if (entity == "Unit")
            //        result = await CommonServices.Services.Unit.GetLocalCache(pipeline);
            //}

            return result;
        }
    }
}
