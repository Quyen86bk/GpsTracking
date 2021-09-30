using Microsoft.Extensions.DependencyInjection;
using NetCore.GpsTrackingModule.Services;
using NetCore.Websites;

namespace NetCore.GpsTrackingModule
{
    //Add to: _AllGpsTrackingService.cs
    //InjectCommand(services);
    public partial class AllGpsTrackingService
    {
        public static void InjectCommand(IServiceCollection services)
        {
            services.AddScoped<ICommandService, CommandService>();
        }
    }

    public partial class AllGpsTrackingService
    {
        ICommandService _Command;
        public ICommandService Command
        {
            get
            {
                if (_Command == null)
                {
                    _Command = ServiceProvider.Get<ICommandService>();
                }
                return _Command;
            }
        }
    }
}
