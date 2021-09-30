using Microsoft.Extensions.DependencyInjection;
using NetCore.GpsTrackingModule.Services;
using NetCore.Websites;

namespace NetCore.GpsTrackingModule
{
    //Add to: _AllGpsTrackingService.cs
    //InjectEvent(services);
    public partial class AllGpsTrackingService
    {
        public static void InjectEvent(IServiceCollection services)
        {
            services.AddScoped<IEventService, EventService>();
        }
    }

    public partial class AllGpsTrackingService
    {
        IEventService _Event;
        public IEventService Event
        {
            get
            {
                if (_Event == null)
                {
                    _Event = ServiceProvider.Get<IEventService>();
                }
                return _Event;
            }
        }
    }
}
