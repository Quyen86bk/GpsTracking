using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCore.ManageModule.Authorize;
using NetCore.Websites;
using NetCore.Websites.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCore.Project.Controllers
{
    [Route(Config.Api + Startup.Module + Config.Controller)]
    [ApiController]
    public class SystemController : _ControllerMain
    {
        public SystemController(IServiceProvider _ServiceProvider)
            : base(_ServiceProvider)
        {
        }

        [AllowAnonymous]
        [HttpGet("{key}/{index}")]
        public async Task PrepareServerCache(Guid key, int index)
        {
            await Services.System.PrepareServerCache(key, index);
        }

        [CoreAuthorize]
        [HttpGet("{platformName}")]
        public async Task<ActionResult> GetClientConfig(string platformName)
        {
            try
            {
                await Services.System.GetClientConfig(pipeline, platformName);
            }
            catch (Exception ex)
            {
                pipeline.OnError(ex);
            }
            return pipeline.Response;
        }

        [CoreAuthorize]
        [HttpPost]
        public async Task<ActionResult> Prepare(List<PrepareVM> Models)
        {
            try
            {
                await Services.System.Prepare(pipeline, Models);
            }
            catch (Exception ex)
            {
                pipeline.OnError(ex);
            }
            return pipeline.Response;
        }

        [CoreAuthorize]
        [HttpPost]
        public async Task<ActionResult> UpdateLocalCache(List<PrepareVM> Models)
        {
            try
            {
                await Services.System.UpdateLocalCache(pipeline, Models);
            }
            catch (Exception ex)
            {
                pipeline.OnError(ex);
            }
            return pipeline.Response;
        }
    }
}
