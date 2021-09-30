using NetCore.CommonModule.Controllers;
using NetCore.ManageModule.Controllers;
using NetCore.TranslateModule.Controllers;
using NetCore.Websites.Controllers;
using System;

namespace NetCore.GpsTrackingModule.Controllers
{
    public class _ServiceGpsTracking : _ServiceCore
    {        
        public _ServiceGpsTracking(IServiceProvider _ServiceProvider)
            : base(_ServiceProvider)
        {
            _ServiceManage = new _ServiceManage(_ServiceProvider);
            _ServiceTranslate = new _ServiceTranslate(_ServiceProvider);
            _ServiceCommon = new _ServiceCommon(_ServiceProvider);

            DBs = new GpsTrackingRepository(_ServiceProvider);
            Services = new AllGpsTrackingService(_ServiceProvider);
        }        

        public _ServiceManage _ServiceManage { get; set; }
        public _ServiceTranslate _ServiceTranslate { get; set; }
        public _ServiceCommon _ServiceCommon { get; set; }        

        public GpsTrackingRepository DBs { get; set; }        
        public AllGpsTrackingService Services { get; set; }
    }
}
