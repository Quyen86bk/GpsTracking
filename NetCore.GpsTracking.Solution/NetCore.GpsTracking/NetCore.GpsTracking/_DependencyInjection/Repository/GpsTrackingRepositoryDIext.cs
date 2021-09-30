using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCore.Library;
using NetCore.GpsTrackingModule.Data;
using NetCore.Websites;
using NetCore.Websites.Data;
using NetCore.Websites.Request;
using System;
using System.Threading.Tasks;

namespace NetCore.GpsTrackingModule
{
    //Add to: Startup.cs
    //GpsTrackingRepository.Inject(services);
    public partial class GpsTrackingRepository
    {
        static void InjectRepository(IServiceCollection services)
        {
            services.AddScoped<IRepository<GpsTrackingMany>, Repository<GpsTrackingMany>>();
            services.AddScoped<IRepository<GpsTrackingNameValue>, Repository<GpsTrackingNameValue>>();
            services.AddScoped<IRepository<Command>, Repository<Command>>();
            services.AddScoped<IRepository<Event>, Repository<Event>>();
            services.AddScoped<IRepository<Geofence>, Repository<Geofence>>();
            services.AddScoped<IRepository<GeofenceDetail>, Repository<GeofenceDetail>>();
            services.AddScoped<IRepository<GeofenceMapping>, Repository<GeofenceMapping>>();
            services.AddScoped<IRepository<GpsDevice>, Repository<GpsDevice>>();
            services.AddScoped<IRepository<GpsDeviceMapping>, Repository<GpsDeviceMapping>>();
            services.AddScoped<IRepository<Group>, Repository<Group>>();
            services.AddScoped<IRepository<Location>, Repository<Location>>();
            services.AddScoped<IRepository<Notification>, Repository<Notification>>();
            services.AddScoped<IRepository<Notificator>, Repository<Notificator>>();
            services.AddScoped<IRepository<NotificationMapping>, Repository<NotificationMapping>>();
            services.AddScoped<IRepository<NotificatorMapping>, Repository<NotificatorMapping>>();
            services.AddScoped<IRepository<Permission>, Repository<Permission>>();
            services.AddScoped<IRepository<ProfileInfo>, Repository<ProfileInfo>>();
            //Auto@Code@Do@Not@Change@InjectRepository
        }
    }

    public partial class GpsTrackingRepository
    {
        IRepository<GpsTrackingMany> _GpsTrackingMany;
        public IRepository<GpsTrackingMany> GpsTrackingMany
        {
            get
            {
                if (_GpsTrackingMany == null)
                {
                    _GpsTrackingMany = ServiceProvider.Get<IRepository<GpsTrackingMany>>().SetDB(DB);
                }
                return _GpsTrackingMany;
            }
        }

        IRepository<GpsTrackingNameValue> _GpsTrackingNameValue;
        public IRepository<GpsTrackingNameValue> GpsTrackingNameValue
        {
            get
            {
                if (_GpsTrackingNameValue == null)
                {
                    _GpsTrackingNameValue = ServiceProvider.Get<IRepository<GpsTrackingNameValue>>().SetDB(DB);
                }
                return _GpsTrackingNameValue;
            }
        }

        IRepository<Command> _Command;
        public IRepository<Command> Command
        {
            get
            {
                if (_Command == null)
                {
                    _Command = ServiceProvider.Get<IRepository<Command>>().SetDB(DB);
                }
                return _Command;
            }
        }

        IRepository<Event> _Event;
        public IRepository<Event> Event
        {
            get
            {
                if (_Event == null)
                {
                    _Event = ServiceProvider.Get<IRepository<Event>>().SetDB(DB);
                }
                return _Event;
            }
        }

        IRepository<Geofence> _Geofence;
        public IRepository<Geofence> Geofence
        {
            get
            {
                if (_Geofence == null)
                {
                    _Geofence = ServiceProvider.Get<IRepository<Geofence>>().SetDB(DB);
                }
                return _Geofence;
            }
        }

        IRepository<GeofenceDetail> _GeofenceDetail;
        public IRepository<GeofenceDetail> GeofenceDetail
        {
            get
            {
                if (_GeofenceDetail == null)
                {
                    _GeofenceDetail = ServiceProvider.Get<IRepository<GeofenceDetail>>().SetDB(DB);
                }
                return _GeofenceDetail;
            }
        }

        IRepository<GeofenceMapping> _GeofenceMapping;
        public IRepository<GeofenceMapping> GeofenceMapping
        {
            get
            {
                if (_GeofenceMapping == null)
                {
                    _GeofenceMapping = ServiceProvider.Get<IRepository<GeofenceMapping>>().SetDB(DB);
                }
                return _GeofenceMapping;
            }
        }

        IRepository<GpsDevice> _GpsDevice;
        public IRepository<GpsDevice> GpsDevice
        {
            get
            {
                if (_GpsDevice == null)
                {
                    _GpsDevice = ServiceProvider.Get<IRepository<GpsDevice>>().SetDB(DB);
                }
                return _GpsDevice;
            }
        }

        IRepository<GpsDeviceMapping> _GpsDeviceMapping;
        public IRepository<GpsDeviceMapping> GpsDeviceMapping
        {
            get
            {
                if (_GpsDeviceMapping == null)
                {
                    _GpsDeviceMapping = ServiceProvider.Get<IRepository<GpsDeviceMapping>>().SetDB(DB);
                }
                return _GpsDeviceMapping;
            }
        }

        IRepository<Group> _Group;
        public IRepository<Group> Group
        {
            get
            {
                if (_Group == null)
                {
                    _Group = ServiceProvider.Get<IRepository<Group>>().SetDB(DB);
                }
                return _Group;
            }
        }

        IRepository<Location> _Location;
        public IRepository<Location> Location
        {
            get
            {
                if (_Location == null)
                {
                    _Location = ServiceProvider.Get<IRepository<Location>>().SetDB(DB);
                }
                return _Location;
            }
        }

        IRepository<Notification> _Notification;
        public IRepository<Notification> Notification
        {
            get
            {
                if (_Notification == null)
                {
                    _Notification = ServiceProvider.Get<IRepository<Notification>>().SetDB(DB);
                }
                return _Notification;
            }
        }

        IRepository<NotificationMapping> _NotificationMapping;
        public IRepository<NotificationMapping> NotificationMapping
        {
            get
            {
                if (_NotificationMapping == null)
                {
                    _NotificationMapping = ServiceProvider.Get<IRepository<NotificationMapping>>().SetDB(DB);
                }
                return _NotificationMapping;
            }
        }

        IRepository<Notificator> _Notificator;
        public IRepository<Notificator> Notificator
        {
            get
            {
                if (_Notificator == null)
                {
                    _Notificator = ServiceProvider.Get<IRepository<Notificator>>().SetDB(DB);
                }
                return _Notificator;
            }
        }

        IRepository<NotificatorMapping> _NotificatorMapping;
        public IRepository<NotificatorMapping> NotificatorMapping
        {
            get
            {
                if (_NotificatorMapping == null)
                {
                    _NotificatorMapping = ServiceProvider.Get<IRepository<NotificatorMapping>>().SetDB(DB);
                }
                return _NotificatorMapping;
            }
        }

        IRepository<Permission> _Permission;
        public IRepository<Permission> Permission
        {
            get
            {
                if (_Permission == null)
                {
                    _Permission = ServiceProvider.Get<IRepository<Permission>>().SetDB(DB);
                }
                return _Permission;
            }
        }

        IRepository<ProfileInfo> _ProfileInfo;
        public IRepository<ProfileInfo> ProfileInfo
        {
            get
            {
                if (_ProfileInfo == null)
                {
                    _ProfileInfo = ServiceProvider.Get<IRepository<ProfileInfo>>().SetDB(DB);
                }
                return _ProfileInfo;
            }
        }
        //Auto@Code@Do@Not@Change@Repository
    }
}