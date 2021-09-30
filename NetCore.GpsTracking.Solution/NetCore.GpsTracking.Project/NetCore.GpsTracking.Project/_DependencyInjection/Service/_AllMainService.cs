using Microsoft.Extensions.DependencyInjection;
using System;

namespace NetCore.Project
{
    public partial class AllMainService
    {
        readonly IServiceProvider ServiceProvider;
        public AllMainService(IServiceProvider _ServiceProvider)
        {
            this.ServiceProvider = _ServiceProvider;
        }
    }

    public partial class AllMainService
    {
        public static void Inject(IServiceCollection services)
        {
            InjectSystem(services);
        }
    }
}
