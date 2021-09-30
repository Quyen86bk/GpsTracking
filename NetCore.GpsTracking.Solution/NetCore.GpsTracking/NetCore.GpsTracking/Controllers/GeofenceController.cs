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
    public partial class GeofenceController : _ControllerGpsTracking
    {
        public GeofenceController(IServiceProvider _ServiceProvider)
            : base(_ServiceProvider)
        {
        }

        [HttpPost]
        public async Task<ActionResult> Save(Geofence model)
        {
            try
            {
                await Services.Geofence.Save(pipeline, model);
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
                await Services.Geofence.Get(pipeline, id);
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
                await Services.Geofence.GetPage(pipeline, filter);
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
                await Services.Geofence.GetList(pipeline, filter);
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
                await Services.Geofence.Delete(pipeline, IDs);
            }
            catch (Exception ex)
            {
                pipeline.OnError(ex);
            }
            return pipeline.Response;
        }

        [HttpPost]
        public async Task<ActionResult> GetGeofences()
        {
            try
            {
                await Services.Geofence.GetGeofences(pipeline);
            }
            catch (Exception ex)
            {
                pipeline.OnError(ex);
            }
            return pipeline.Response;
        }

        [HttpPost]
        public async Task<ActionResult> GetGeofencesNames()
        {
            try
            {
                await Services.Geofence.GetGeofencesNames(pipeline);
            }
            catch (Exception ex)
            {
                pipeline.OnError(ex);
            }
            return pipeline.Response;
        }
    }
}
