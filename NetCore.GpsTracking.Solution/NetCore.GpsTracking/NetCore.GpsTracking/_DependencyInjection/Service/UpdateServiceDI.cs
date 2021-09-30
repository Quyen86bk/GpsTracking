using Microsoft.Extensions.DependencyInjection;
using NetCore.GpsTrackingModule.Services;
using NetCore.Websites;

namespace NetCore.GpsTrackingModule
{
    //Add to: _AllGpsTrackingService.cs
    //InjectUpdate(services);
    public partial class AllGpsTrackingService
    {
        public static void InjectUpdate(IServiceCollection services)
        {
            services.AddScoped<IUpdateService, UpdateService>();
        }
    }

    public partial class AllGpsTrackingService
    {
        IUpdateService _Update;
        public IUpdateService Update
        {
            get
            {
                if (_Update == null)
                {
                    _Update = ServiceProvider.Get<IUpdateService>();
                }
                return _Update;
            }
        }
    }
}
