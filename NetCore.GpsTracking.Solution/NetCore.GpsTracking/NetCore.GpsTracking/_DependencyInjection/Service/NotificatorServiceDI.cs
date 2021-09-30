using Microsoft.Extensions.DependencyInjection;
using NetCore.GpsTrackingModule.Services;
using NetCore.Websites;

namespace NetCore.GpsTrackingModule
{
    //Add to: _AllGpsTrackingService.cs
    //InjectNotificator(services);
    public partial class AllGpsTrackingService
    {
        public static void InjectNotificator(IServiceCollection services)
        {
            services.AddScoped<INotificatorService, NotificatorService>();
        }
    }

    public partial class AllGpsTrackingService
    {
        INotificatorService _Notificator;
        public INotificatorService Notificator
        {
            get
            {
                if (_Notificator == null)
                {
                    _Notificator = ServiceProvider.Get<INotificatorService>();
                }
                return _Notificator;
            }
        }
    }
}
