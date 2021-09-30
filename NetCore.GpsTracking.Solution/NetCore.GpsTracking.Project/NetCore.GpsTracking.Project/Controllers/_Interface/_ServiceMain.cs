using NetCore.Websites.Controllers;
using System;

namespace NetCore.Project.Controllers
{
    public class _ServiceMain : _ServiceCore
    {
        public _ServiceMain(IServiceProvider _ServiceProvider)
            : base(_ServiceProvider)
        {
            Services = new AllMainService(_ServiceProvider);
        }
        public AllMainService Services { get; set; }
    }
}
