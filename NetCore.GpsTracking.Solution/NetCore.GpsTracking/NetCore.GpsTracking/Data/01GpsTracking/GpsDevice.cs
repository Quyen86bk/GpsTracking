using NetCore.Websites.Data;
using System;
using NetCore.Library;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace NetCore.GpsTrackingModule.Data
{
    public class GpsDevice : RecordBaseGuid
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Phone { get; set; }

        public float LastLatitude { get; set; }
        public float LastLongitude { get; set; }

        public int StatusId { get; set; }
        public int EventTypeId { get; set; }

        [JsonIgnore]
        [NotMapped]
        public DeviceStatus Status
        {
            get
            {
                return (DeviceStatus)StatusId;
            }

            set
            {
                StatusId = (int)value;
            }
        }
        [NotMapped]
        public string StatusName
        {
            get
            {
                return Status.GetDescription();
            }
        }

        public int CategoryId { get; set; }
        [JsonIgnore]
        [NotMapped]
        public DeviceCategory Category
        {
            get
            {
                return (DeviceCategory)CategoryId;
            }

            set
            {
                CategoryId = (int)value;
            }
        }
        [NotMapped]
        public string CategoryName
        {
            get
            {
                return Category.GetDescription();
            }
        }

        public List<GeofenceMapping> GeofenceMappings { get; set; }
        public List<NotificationMapping> NotificationMappings { get; set; }

        [NotMapped]
        public List<string> Geofences { get; set; }
        [NotMapped]
        public string ListGeofences { get; set; }

        [NotMapped]
        public List<string> Notifications { get; set; }
        [NotMapped]
        public string ListNotifications { get; set; }

        public List<GpsDeviceMapping> GpsDeviceMappings { get; set; }
        public List<EventGeofenceMapping> EventGeofenceMappings { get; set; }
    }
    public enum DeviceStatus
    {
        [Description(lib.UnKnow)]
        UnKnow = 0,

        [Description("Online")]
        Online = 1,

        [Description("Offline")]
        Offline = 2,
    }

    public enum DeviceCategory
    {
        [Description(lib.UnKnow)]
        UnKnow = 0,

        [Description("Mặc định")]
        Default = 1,

        [Description("Ôtô")]
        Car = 2,

        [Description("Xe máy")]
        Bike = 3,
    }
}
