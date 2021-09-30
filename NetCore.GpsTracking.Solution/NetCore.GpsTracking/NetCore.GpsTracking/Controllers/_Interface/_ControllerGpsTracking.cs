using NetCore.Websites.Controllers;
using System;
using NetCore.ManageModule.Controllers;

namespace NetCore.GpsTrackingModule.Controllers
{
    public class _ControllerGpsTracking : _ControllerManage
    {
        public _ControllerGpsTracking(IServiceProvider _ServiceProvider)
        : base(_ServiceProvider)
        {
            Services = new AllGpsTrackingService(_ServiceProvider);            
        }

        public AllGpsTrackingService Services { get; set; }        
    }
}
