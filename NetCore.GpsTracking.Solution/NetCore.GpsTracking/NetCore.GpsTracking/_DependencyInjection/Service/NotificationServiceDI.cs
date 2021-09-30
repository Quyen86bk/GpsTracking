using Microsoft.Extensions.DependencyInjection;
using NetCore.GpsTrackingModule.Services;
using NetCore.Websites;

namespace NetCore.GpsTrackingModule
{
    //Add to: _AllGpsTrackingService.cs
    //InjectNotification(services);
    public partial class AllGpsTrackingService
    {
        public static void InjectNotification(IServiceCollection services)
        {
            services.AddScoped<INotificationService, NotificationService>();
        }
    }

    public partial class AllGpsTrackingService
    {
        INotificationService _Notification;
        public INotificationService Notification
        {
            get
            {
                if (_Notification == null)
                {
                    _Notification = ServiceProvider.Get<INotificationService>();
                }
                return _Notification;
            }
        }
    }
}
