using Microsoft.Extensions.DependencyInjection;
using NetCore.GpsTrackingModule.Services;
using NetCore.Websites;

namespace NetCore.GpsTrackingModule
{
    //Add to: _AllGpsTrackingService.cs
    //InjectGroup(services);
    public partial class AllGpsTrackingService
    {
        public static void InjectGroup(IServiceCollection services)
        {
            services.AddScoped<IGroupService, GroupService>();
        }
    }

    public partial class AllGpsTrackingService
    {
        IGroupService _Group;
        public IGroupService Group
        {
            get
            {
                if (_Group == null)
                {
                    _Group = ServiceProvider.Get<IGroupService>();
                }
                return _Group;
            }
        }
    }
}
