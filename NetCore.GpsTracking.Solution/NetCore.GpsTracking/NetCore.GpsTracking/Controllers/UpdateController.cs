using Microsoft.AspNetCore.Mvc;
using NetCore.ManageModule.Authorize;
using NetCore.GpsTrackingModule.Data;
using NetCore.Websites;
using NetCore.Websites.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace NetCore.GpsTrackingModule.Controllers
{
    [AllowAnonymous]
    //[CoreAuthorize]
    //[Route("/Api/Update")]
    [ApiController]
    public partial class UpdateController : _ControllerGpsTracking
    {
        public UpdateController(IServiceProvider _ServiceProvider)
            : base(_ServiceProvider)
        {
        }

        [Route("Api/Update/Index")]
        [HttpGet]
        public async Task<ActionResult> Index(int cmd, string deviceCode, int eventTypeId, float longitude, float latitude)
        {
            try
            {
                await Services.Update.Index(pipeline, cmd, deviceCode, eventTypeId, longitude, latitude);
            }
            catch (Exception ex)
            {
                pipeline.OnError(ex);
            }
            return pipeline.Response;
        }
    }
}
