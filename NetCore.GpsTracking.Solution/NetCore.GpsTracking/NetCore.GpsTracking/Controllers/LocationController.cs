using Microsoft.AspNetCore.Mvc;
using NetCore.ManageModule.Authorize;
using NetCore.GpsTrackingModule.Data;
using NetCore.Websites;
using NetCore.Websites.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetCore.GpsTrackingModule.Models;

namespace NetCore.GpsTrackingModule.Controllers
{
    [CoreAuthorize]
    [Route(Config.Api + Startup.Module + Config.Controller)]
    [ApiController]
    public partial class LocationController : _ControllerGpsTracking
    {
        public LocationController(IServiceProvider _ServiceProvider)
            : base(_ServiceProvider)
        {
        }

        [HttpPost]
        public async Task<ActionResult> Save(Location model)
        {
            try
            {
                await Services.Location.Save(pipeline, model);
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
                await Services.Location.Get(pipeline, id);
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
                await Services.Location.GetPage(pipeline, filter);
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
                await Services.Location.GetList(pipeline, filter);
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
                await Services.Location.Delete(pipeline, IDs);
            }
            catch (Exception ex)
            {
                pipeline.OnError(ex);
            }
            return pipeline.Response;
        }

        [HttpPost]
        public async Task<ActionResult> GetLocations(LocationFilterVM filter)
        {
            try
            {
                await Services.Location.GetLocations(pipeline, filter);
            }
            catch (Exception ex)
            {
                pipeline.OnError(ex);
            }
            return pipeline.Response;
        }

        [HttpPost]
        public async Task<ActionResult> GetHeatmaps(HeatmapFilterVM filter)
        {
            try
            {
                await Services.Location.GetHeatmaps(pipeline, filter);
            }
            catch (Exception ex)
            {
                pipeline.OnError(ex);
            }
            return pipeline.Response;
        }

        [HttpPost]
        public async Task<ActionResult> GetReplays(ReplayFilterVM filter)
        {
            try
            {
                await Services.Location.GetReplays(pipeline, filter);
            }
            catch (Exception ex)
            {
                pipeline.OnError(ex);
            }
            return pipeline.Response;
        }
    }
}
