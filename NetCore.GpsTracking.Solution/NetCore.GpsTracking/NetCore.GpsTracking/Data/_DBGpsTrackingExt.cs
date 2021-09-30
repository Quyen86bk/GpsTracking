using Microsoft.EntityFrameworkCore;
using NetCore.GpsTrackingModule.Controllers;
using System;

namespace NetCore.GpsTrackingModule.Data
{
    public partial class DBGpsTracking
    {
        public static string RepositoryName = "GpsTracking";
        public static void OnModelCreating_HasKey_HasBase(ModelBuilder b, IServiceProvider _ServiceProvider)
        {
            //b.Entity<GpsDevice>().HasBaseType(typeof(GpsTrackingNameValue));
        }

        private static void Ignore(ModelBuilder b, IServiceProvider _ServiceProvider)
        {
            b.Ignore<GpsTrackingMany>();
            b.Ignore<GpsTrackingNameValue>();
            b.Ignore<Command>();
            b.Ignore<Event>();
            b.Ignore<Geofence>();
            b.Ignore<GeofenceDetail>();
            b.Ignore<GeofenceMapping>();
            b.Ignore<GpsDevice>();
            b.Ignore<GpsDeviceMapping>();
            b.Ignore<Group>();
            b.Ignore<Location>();
            b.Ignore<Notification>();
            b.Ignore<NotificationMapping>();
            b.Ignore<Notificator>();
            b.Ignore<NotificatorMapping>();
            b.Ignore<Permission>();
            b.Ignore<ProfileInfo>();
            //Auto@Code@Do@Not@Change@Ignore
        }
        private static void ToTable(ModelBuilder b, IServiceProvider _ServiceProvider)
        {
            b.Entity<GpsTrackingMany>().ToTable(RepositoryName + "_GpsTrackingMany");
            b.Entity<GpsTrackingNameValue>().ToTable(RepositoryName + "_GpsTrackingNameValue");
            b.Entity<Command>().ToTable(RepositoryName + "_Command");
            b.Entity<Event>().ToTable(RepositoryName + "_Event");
            b.Entity<Geofence>().ToTable(RepositoryName + "_Geofence");
            b.Entity<GeofenceDetail>().ToTable(RepositoryName + "_GeofenceDetail");
            b.Entity<GeofenceMapping>().ToTable(RepositoryName + "_GeofenceMapping");
            b.Entity<GpsDevice>().ToTable(RepositoryName + "_GpsDevice");
            b.Entity<GpsDeviceMapping>().ToTable(RepositoryName + "_GpsDeviceMapping");
            b.Entity<Group>().ToTable(RepositoryName + "_Group");
            b.Entity<Location>().ToTable(RepositoryName + "_Location");
            b.Entity<Notification>().ToTable(RepositoryName + "_Notification");
            b.Entity<NotificationMapping>().ToTable(RepositoryName + "_NotificationMapping");
            b.Entity<Notificator>().ToTable(RepositoryName + "_Notificator");
            b.Entity<NotificatorMapping>().ToTable(RepositoryName + "_NotificatorMapping");
            b.Entity<Permission>().ToTable(RepositoryName + "_Permission");
            b.Entity<ProfileInfo>().ToTable(RepositoryName + "_ProfileInfo");
            //Auto@Code@Do@Not@Change@ToTable
        }

        public static void CreateMapping(IServiceProvider _ServiceProvider, string server, DbContext toDbContext)
        {
            var repository = new GpsTrackingRepository(_ServiceProvider);
            var db = repository.DB.Database.GetDbConnection().Database;

            NetCore.Websites.Data.Tool.CreateMapping(server, db, RepositoryName, "GpsTrackingMany", "", toDbContext);
            NetCore.Websites.Data.Tool.CreateMapping(server, db, RepositoryName, "GpsTrackingNameValue", "", toDbContext);
            NetCore.Websites.Data.Tool.CreateMapping(server, db, RepositoryName, "Command", "", toDbContext);
            NetCore.Websites.Data.Tool.CreateMapping(server, db, RepositoryName, "Event", "", toDbContext);
            NetCore.Websites.Data.Tool.CreateMapping(server, db, RepositoryName, "Geofence", "", toDbContext);
            NetCore.Websites.Data.Tool.CreateMapping(server, db, RepositoryName, "GeofenceDetail", "", toDbContext);
            NetCore.Websites.Data.Tool.CreateMapping(server, db, RepositoryName, "GeofenceMapping", "", toDbContext);
            NetCore.Websites.Data.Tool.CreateMapping(server, db, RepositoryName, "GpsDevice", "", toDbContext);
            NetCore.Websites.Data.Tool.CreateMapping(server, db, RepositoryName, "GpsDeviceMapping", "", toDbContext);
            NetCore.Websites.Data.Tool.CreateMapping(server, db, RepositoryName, "Group", "", toDbContext);
            NetCore.Websites.Data.Tool.CreateMapping(server, db, RepositoryName, "Location", "", toDbContext);
            NetCore.Websites.Data.Tool.CreateMapping(server, db, RepositoryName, "Notification", "", toDbContext);
            NetCore.Websites.Data.Tool.CreateMapping(server, db, RepositoryName, "NotificationMapping", "", toDbContext);
            NetCore.Websites.Data.Tool.CreateMapping(server, db, RepositoryName, "Notificator", "", toDbContext);
            NetCore.Websites.Data.Tool.CreateMapping(server, db, RepositoryName, "NotificatorMapping", "", toDbContext);
            NetCore.Websites.Data.Tool.CreateMapping(server, db, RepositoryName, "Permission", "", toDbContext);
            NetCore.Websites.Data.Tool.CreateMapping(server, db, RepositoryName, "ProfileInfo", "", toDbContext);
            //Auto@Code@Do@Not@Change@CreateMapping
        }

        #region DbSet
        public DbSet<GpsTrackingMany> GpsTrackingMany { get; set; }
        public DbSet<GpsTrackingNameValue> GpsTrackingNameValue { get; set; }
        public DbSet<Command> Command { get; set; }
        public DbSet<Event> Event { get; set; }
        public DbSet<Geofence> Geofence { get; set; }
        public DbSet<GeofenceDetail> GeofenceDetail { get; set; }
        public DbSet<GeofenceMapping> GeofenceMapping { get; set; }
        public DbSet<GpsDevice> GpsDevice { get; set; }
        public DbSet<GpsDeviceMapping> GpsDeviceMapping { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<NotificationMapping> NotificationMapping { get; set; }
        public DbSet<Notificator> Notificator { get; set; }
        public DbSet<NotificatorMapping> NotificatorMapping { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<ProfileInfo> ProfileInfo { get; set; }
        //Auto@Code@Do@Not@Change@DbSet
        #endregion
    }
}
