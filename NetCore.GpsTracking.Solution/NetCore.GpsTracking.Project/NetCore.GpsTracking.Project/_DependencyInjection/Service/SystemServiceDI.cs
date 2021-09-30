using Microsoft.Extensions.DependencyInjection;
using NetCore.Project.Services;
using NetCore.Websites;

namespace NetCore.Project
{
    //Add to: _AllService
    //InjectSystem(services);
    public partial class AllMainService
    {
        public static void InjectSystem(IServiceCollection services)
        {
            services.AddScoped<ISystemService, SystemService>();
        }
    }

    public partial class AllMainService
    {
        ISystemService _System;
        public ISystemService System
        {
            get
            {
                if (_System == null)
                {
                    _System = ServiceProvider.Get<ISystemService>();
                }
                return _System;
            }
        }
    }
}
