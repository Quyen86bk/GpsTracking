using Microsoft.Extensions.DependencyInjection;
using NetCore.GpsTrackingModule.Data;
using System;

namespace NetCore.GpsTrackingModule
{
    public partial class AllGpsTrackingService
    {
        readonly IServiceProvider ServiceProvider;
        public AllGpsTrackingService(IServiceProvider _ServiceProvider)
        {
            this.ServiceProvider = _ServiceProvider;
        }
    }

    public partial class AllGpsTrackingService
    {
        public static void Inject(IServiceCollection services)
        {
            InjectGpsDevice(services);
            InjectLocation(services);
            InjectGeofence(services);
            InjectNotification(services);
            InjectNotificator(services);
            InjectEvent(services);
            InjectCommand(services);
            InjectPermission(services);
            InjectGroup(services);
            InjectUpdate(services);
            InjectProfileInfo(services);
            InjectReportLocation(services);
            InjectReportEvent(services);
            //Auto@Code@Do@Not@Change@AllServiceDI
        }
    }
}
