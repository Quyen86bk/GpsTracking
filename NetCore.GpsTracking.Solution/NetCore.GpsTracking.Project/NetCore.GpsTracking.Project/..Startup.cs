using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCore.Library;
using NetCore.Websites.Controllers;
using NetCore.Websites.Services.Account;

namespace NetCore.Project
{
    #region Program
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
           WebHost.CreateDefaultBuilder(args)
               .UseStartup<Startup>();
    }
    #endregion

    public class Startup
    {
        public const string Module = lib.Main;

        #region Configure
        public Startup(IConfiguration config, IHostingEnvironment env)
        {
            App.Id = lib.ToGuid("F926D1E1-82C9-4CDF-9332-1341B0F822F4");
            App.Name = "GpsTracking";
            App.Version = "1.0.0";
            MembershipConfig.UseEmail = true;
            NetCore.Websites.Startup.Configure_App(config, env);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            NetCore.Websites.Startup.Configure(app, env);
        }
        public void ConfigureServices(IServiceCollection services)
        {
            NetCore.Websites.Startup.ConfigureServices(services);
            NetCore.Base.Startup.ConfigureServices(services);

            ConfigureServices_Module(services);

            ControllerRegister.Register(services, typeof(NetCore.Project.Startup).Assembly);
            NetCore.Project.AllMainService.Inject(services);

            NetCore.Websites.Startup.CachePrepare_Module(-1);
        }
        #endregion

        public void ConfigureServices_Module(IServiceCollection services)
        {
            NetCore.GpsTrackingModule.Startup.ConfigureServices(services);
        }
    }
}
