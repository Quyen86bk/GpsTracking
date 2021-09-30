using Microsoft.AspNetCore.Mvc;
using NetCore.Base.Controllers;
using NetCore.Library;
using NetCore.GpsTrackingModule;
using NetCore.Websites;
using NetCore.Websites.Controllers;
using NetCore.Websites.Request;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCore.Project.Controllers
{
    public class ToolController : _ControllerMain
    {
        readonly IServiceProvider ServiceProvider;
        public ToolController(IServiceProvider _ServiceProvider)
            : base(_ServiceProvider)
        {
            this.ServiceProvider = _ServiceProvider;
        }

        string message = "Disabled...";
        public async Task<IActionResult> Startup()
        {
            if (App.EnableTool)
            {
                await EF();                

                await Register();
                await Membership();

                message = "OK !";
            }
            return new xControllerBase().CreateOkResponse(message);
        }

        List<xModule> GetModules()
        {
            var Modules = new ToolBaseController(ServiceProvider).GetModules();
            Modules.Add(ControllerRegister.GetModules(11, NetCore.GpsTrackingModule.Startup.Module, typeof(NetCore.GpsTrackingModule.Startup).Assembly));

            return Modules;
        }
        public async Task<IActionResult> Membership()
        {
            if (App.EnableTool)
            {
                await new ToolBaseController(ServiceProvider).CreateDefaultMembership();
                message = "OK !";
            }
            return new xControllerBase().CreateOkResponse(message);
        }

        public async Task<IActionResult> EF()
        {
            if (App.EnableTool)
            {
                new ToolBaseController(ServiceProvider).EF();
                GpsTrackingRepository.Migrate(ServiceProvider);

                message = "OK !";
				await Mapping();
            }
            return new xControllerBase().CreateOkResponse(message);
        }
        public async Task<IActionResult> Mapping()
        {
            if (App.EnableTool)
            {
                string server = "";
                new ToolBaseController(ServiceProvider).Mapping(server);

                GpsTrackingRepository.Mapping(ServiceProvider, server);
                message = "OK !";
            }
            return new xControllerBase().CreateOkResponse(message);
        }

        public async Task<IActionResult> Register()
        {
            if (App.EnableTool)
            {
                new ToolBaseController(ServiceProvider).RegisterController(GetModules());
                message = "OK !";
            }
            return new xControllerBase().CreateOkResponse(message);
        }
    }
}
