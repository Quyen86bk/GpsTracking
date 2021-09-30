using Microsoft.AspNetCore.Mvc;
using NetCore.ManageModule.Authorize;
using NetCore.GpsTrackingModule.Data;
using NetCore.Websites;
using NetCore.Websites.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCore.GpsTrackingModule.Controllers
{
    [CoreAuthorize]
    [Route(Config.Api + Startup.Module + Config.Controller)]
    [ApiController]
    public partial class ProfileInfoController : _ControllerGpsTracking
    {
        public ProfileInfoController(IServiceProvider _ServiceProvider)
            : base(_ServiceProvider)
        {
        }

        [HttpPost]
        public async Task<ActionResult> Save(ProfileInfo model)
        {
            try
            {
                await Services.ProfileInfo.Save(pipeline, model);
            }
            catch (Exception ex)
            {
                pipeline.OnError(ex);
            }
            return pipeline.Response;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            try
            {
                await Services.ProfileInfo.Get(pipeline, id);
            }
            catch (Exception ex)
            {
                pipeline.OnError(ex);
            }
            return pipeline.Response;
        }

        [HttpPost]
        public async Task<ActionResult> GetPage(PageFilterVM filter)
        {
            try
            {
                await Services.ProfileInfo.GetPage(pipeline, filter);
            }
            catch (Exception ex)
            {
                pipeline.OnError(ex);
            }
            return pipeline.Response;
        }

        [HttpPost]
        public async Task<ActionResult> GetList(ListFilterVM filter)
        {
            try
            {
                await Services.ProfileInfo.GetList(pipeline, filter);
            }
            catch (Exception ex)
            {
                pipeline.OnError(ex);
            }
            return pipeline.Response;
        }

        [HttpPost]
        public async Task<ActionResult> Delete(List<Guid> IDs)
        {
            try
            {
                await Services.ProfileInfo.Delete(pipeline, IDs);
            }
            catch (Exception ex)
            {
                pipeline.OnError(ex);
            }
            return pipeline.Response;
        }
    }
}
