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
    public partial class ReportEventController : _ControllerGpsTracking
    {
        public ReportEventController(IServiceProvider _ServiceProvider)
            : base(_ServiceProvider)
        {
        }

        [HttpPost]
        public async Task<ActionResult> GetPage(ReportEventFilterVM filter)
        {
            try
            {
                await Services.ReportEvent.GetPage(pipeline, filter);
            }
            catch (Exception ex)
            {
                pipeline.OnError(ex);
            }
            return pipeline.Response;
        }

        [HttpPost]
        public async Task<ActionResult> SendToEmail(ReportLocationFilterVM filter)
        {
            try
            {
                await Services.ReportLocation.GetPage(pipeline, filter);
            }
            catch (Exception ex)
            {
                pipeline.OnError(ex);
            }
            return pipeline.Response;
        }
    }
}
