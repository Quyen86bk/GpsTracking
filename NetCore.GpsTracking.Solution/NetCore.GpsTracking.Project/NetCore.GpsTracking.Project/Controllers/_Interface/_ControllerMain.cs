using NetCore.ManageModule.Controllers;
using NetCore.Websites.Controllers;
using System;

namespace NetCore.Project.Controllers
{
    public class _ControllerMain : _ControllerManage
    {
        public _ControllerMain(IServiceProvider _ServiceProvider)
            : base(_ServiceProvider)
        {
            Services = new AllMainService(_ServiceProvider);
        }
        public AllMainService Services { get; set; }
    }
}
