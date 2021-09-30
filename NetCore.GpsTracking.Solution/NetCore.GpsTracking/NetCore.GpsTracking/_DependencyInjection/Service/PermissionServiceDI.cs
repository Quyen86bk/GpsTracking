using Microsoft.Extensions.DependencyInjection;
using NetCore.GpsTrackingModule.Services;
using NetCore.Websites;

namespace NetCore.GpsTrackingModule
{
    //Add to: _AllGpsTrackingService.cs
    //InjectPermission(services);
    public partial class AllGpsTrackingService
    {
        public static void InjectPermission(IServiceCollection services)
        {
            services.AddScoped<IPermissionService, PermissionService>();
        }
    }

    public partial class AllGpsTrackingService
    {
        IPermissionService _Permission;
        public IPermissionService Permission
        {
            get
            {
                if (_Permission == null)
                {
                    _Permission = ServiceProvider.Get<IPermissionService>();
                }
                return _Permission;
            }
        }
    }
}
