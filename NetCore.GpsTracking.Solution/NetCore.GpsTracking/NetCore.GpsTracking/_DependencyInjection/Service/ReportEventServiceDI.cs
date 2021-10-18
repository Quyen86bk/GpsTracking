using Microsoft.Extensions.DependencyInjection;
using NetCore.GpsTrackingModule.Services;
using NetCore.Websites;

namespace NetCore.GpsTrackingModule
{
    //Add to: _AllGpsTrackingService.cs
    //InjectReportEvent(services);
    public partial class AllGpsTrackingService
    {
        public static void InjectReportEvent(IServiceCollection services)
        {
            services.AddScoped<IReportEventService, ReportEventService>();
        }
    }

    public partial class AllGpsTrackingService
    {
        IReportEventService _ReportEvent;
        public IReportEventService ReportEvent
        {
            get
            {
                if (_ReportEvent == null)
                {
                    _ReportEvent = ServiceProvider.Get<IReportEventService>();
                }
                return _ReportEvent;
            }
        }
    }
}
