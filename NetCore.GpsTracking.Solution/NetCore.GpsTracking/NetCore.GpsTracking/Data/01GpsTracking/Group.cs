using NetCore.Websites.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCore.GpsTrackingModule.Data
{
    public class Group : RecordBaseGuid
    {
        public string Name { get; set; }
        public List<GpsDeviceMapping> GpsDeviceMappings { get; set; }
        public List<NotificationMapping> NotificationMappings { get; set; }
        public List<GeofenceMapping> GeofenceMappings { get; set; }

        [NotMapped]
        public List<string> GpsDevices { get; set; }
        [NotMapped]
        public string ListGpsDevices { get; set; }

        [NotMapped]
        public List<string> Notifications { get; set; }
        [NotMapped]
        public string ListNotifications { get; set; }

        [NotMapped]
        public List<string> Geofences { get; set; }
        [NotMapped]
        public string ListGeofences { get; set; }
    }
}
