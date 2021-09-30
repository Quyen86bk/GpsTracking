using Microsoft.Extensions.DependencyInjection;
using NetCore.GpsTrackingModule.Services;
using NetCore.Websites;

namespace NetCore.GpsTrackingModule
{
    //Add to: _AllGpsTrackingService.cs
    //InjectGeofence(services);
    public partial class AllGpsTrackingService
    {
        public static void InjectGeofence(IServiceCollection services)
        {
            services.AddScoped<IGeofenceService, GeofenceService>();
        }
    }

    public partial class AllGpsTrackingService
    {
        IGeofenceService _Geofence;
        public IGeofenceService Geofence
        {
            get
            {
                if (_Geofence == null)
                {
                    _Geofence = ServiceProvider.Get<IGeofenceService>();
                }
                return _Geofence;
            }
        }
    }
}
