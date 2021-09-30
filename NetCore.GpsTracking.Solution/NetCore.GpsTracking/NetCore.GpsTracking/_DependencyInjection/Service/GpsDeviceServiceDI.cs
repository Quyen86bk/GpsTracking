using Microsoft.Extensions.DependencyInjection;
using NetCore.GpsTrackingModule.Services;
using NetCore.Websites;

namespace NetCore.GpsTrackingModule
{
    //Add to: _AllGpsTrackingService.cs
    //InjectGpsDevice(services);
    public partial class AllGpsTrackingService
    {
        public static void InjectGpsDevice(IServiceCollection services)
        {
            services.AddScoped<IGpsDeviceService, GpsDeviceService>();
        }
    }

    public partial class AllGpsTrackingService
    {
        IGpsDeviceService _GpsDevice;
        public IGpsDeviceService GpsDevice
        {
            get
            {
                if (_GpsDevice == null)
                {
                    _GpsDevice = ServiceProvider.Get<IGpsDeviceService>();
                }
                return _GpsDevice;
            }
        }
    }
}
