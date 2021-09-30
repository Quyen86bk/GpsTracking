using Microsoft.Extensions.DependencyInjection;
using NetCore.GpsTrackingModule.Services;
using NetCore.Websites;

namespace NetCore.GpsTrackingModule
{
    //Add to: _AllGpsTrackingService.cs
    //InjectGeofenceDetail(services);
    public partial class AllGpsTrackingService
    {
        public static void InjectGeofenceDetail(IServiceCollection services)
        {
            services.AddScoped<IGeofenceDetailService, GeofenceDetailService>();
        }
    }

    public partial class AllGpsTrackingService
    {
        IGeofenceDetailService _GeofenceDetail;
        public IGeofenceDetailService GeofenceDetail
        {
            get
            {
                if (_GeofenceDetail == null)
                {
                    _GeofenceDetail = ServiceProvider.Get<IGeofenceDetailService>();
                }
                return _GeofenceDetail;
            }
        }
    }
}
