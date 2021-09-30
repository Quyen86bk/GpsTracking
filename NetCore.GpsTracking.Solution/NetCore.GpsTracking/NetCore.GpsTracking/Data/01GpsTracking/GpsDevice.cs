using NetCore.Websites.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCore.GpsTrackingModule.Data
{
    public class GpsDevice : RecordBaseGuid
    {
        public List<GpsDeviceMapping> GpsDeviceMappings { get; set; }

        public string Name { get; set; }
        public string Code { get; set; }
        public string Phone { get; set; }
        public string Category { get; set; }
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
    }
}
