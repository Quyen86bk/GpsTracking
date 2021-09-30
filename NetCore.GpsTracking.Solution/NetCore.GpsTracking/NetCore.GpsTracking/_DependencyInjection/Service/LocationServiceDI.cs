using Microsoft.Extensions.DependencyInjection;
using NetCore.GpsTrackingModule.Services;
using NetCore.Websites;

namespace NetCore.GpsTrackingModule
{
    //Add to: _AllGpsTrackingService.cs
    //InjectLocation(services);
    public partial class AllGpsTrackingService
    {
        public static void InjectLocation(IServiceCollection services)
        {
            services.AddScoped<ILocationService, LocationService>();
        }
    }

    public partial class AllGpsTrackingService
    {
        ILocationService _Location;
        public ILocationService Location
        {
            get
            {
                if (_Location == null)
                {
                    _Location = ServiceProvider.Get<ILocationService>();
                }
                return _Location;
            }
        }
    }
}
