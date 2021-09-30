using Microsoft.Extensions.DependencyInjection;
using NetCore.GpsTrackingModule.Services;
using NetCore.Websites;

namespace NetCore.GpsTrackingModule
{
    //Add to: _AllGpsTrackingService.cs
    //InjectProfileInfo(services);
    public partial class AllGpsTrackingService
    {
        public static void InjectProfileInfo(IServiceCollection services)
        {
            services.AddScoped<IProfileInfoService, ProfileInfoService>();
        }
    }

    public partial class AllGpsTrackingService
    {
        IProfileInfoService _ProfileInfo;
        public IProfileInfoService ProfileInfo
        {
            get
            {
                if (_ProfileInfo == null)
                {
                    _ProfileInfo = ServiceProvider.Get<IProfileInfoService>();
                }
                return _ProfileInfo;
            }
        }
    }
}
