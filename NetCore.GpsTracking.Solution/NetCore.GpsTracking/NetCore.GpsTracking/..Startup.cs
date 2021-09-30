using Microsoft.Extensions.DependencyInjection;
using NetCore.GpsTrackingModule.Index;
using NetCore.Websites.Controllers;
using NetCore.Websites.Index;

namespace NetCore.GpsTrackingModule
{
    public class Startup
    {
        public const string Module = "GpsTracking";

        #region Configure
        public static void ConfigureServices(IServiceCollection services)
        {
            ControllerRegister.Register(services, typeof(NetCore.GpsTrackingModule.Startup).Assembly);

            GpsTrackingRepository.Inject(services);            
            AllGpsTrackingService.Inject(services);

            ConfigIndex(services);
        }
        public static void ConfigIndex(IServiceCollection services)
        {
            var GpsDevice = IndexConfig.Add(IndexGpsDevice.ConfigIndex());
            IndexConfig.Add(IndexGpsDevice20.ConfigIndex(GpsDevice));

            var Location = IndexConfig.Add(IndexLocation.ConfigIndex());
            var Geofence = IndexConfig.Add(IndexGeofence.ConfigIndex());
            var Notification = IndexConfig.Add(IndexNotification.ConfigIndex());
            var Event = IndexConfig.Add(IndexEvent.ConfigIndex());
            var Command = IndexConfig.Add(IndexCommand.ConfigIndex());
            var Permission = IndexConfig.Add(IndexPermission.ConfigIndex());
            var Group = IndexConfig.Add(IndexGroup.ConfigIndex());
            var ProfileInfo = IndexConfig.Add(IndexProfileInfo.ConfigIndex());
            var Notificator = IndexConfig.Add(IndexNotificator.ConfigIndex());
            //Auto@Code@Do@Not@Change@ConfigIndex
        }
        #endregion
    }
}
