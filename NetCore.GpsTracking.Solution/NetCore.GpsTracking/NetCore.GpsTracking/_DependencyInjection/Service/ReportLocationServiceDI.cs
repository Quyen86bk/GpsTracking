using Microsoft.Extensions.DependencyInjection;
using NetCore.GpsTrackingModule.Services;
using NetCore.Websites;

namespace NetCore.GpsTrackingModule
{
    //Add to: _AllGpsTrackingService.cs
    //InjectReportLocation(services);
    public partial class AllGpsTrackingService
    {
        public static void InjectReportLocation(IServiceCollection services)
        {
            services.AddScoped<IReportLocationService, ReportLocationService>();
        }
    }

    public partial class AllGpsTrackingService
    {
        IReportLocationService _ReportLocation;
        public IReportLocationService ReportLocation
        {
            get
            {
                if (_ReportLocation == null)
                {
                    _ReportLocation = ServiceProvider.Get<IReportLocationService>();
                }
                return _ReportLocation;
            }
        }
    }
}
