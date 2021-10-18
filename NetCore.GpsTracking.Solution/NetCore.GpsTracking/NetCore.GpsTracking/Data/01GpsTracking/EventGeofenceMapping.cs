using NetCore.Library;
using NetCore.Websites.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCore.GpsTrackingModule.Data
{
    public class EventGeofenceMapping : RecordBaseGuid
    {
        [ForeignKey("GpsDevice")]
        public Guid? GpsDeviceId { get; set; }
        [JsonIgnore]
        public GpsDevice GpsDevice { get; set; }

        [ForeignKey("Geofence")]
        public Guid? GeofenceId { get; set; }
        [JsonIgnore]
        public Geofence Geofence { get; set; }

        public int EventTypeId { get; set; }
        [JsonIgnore]
        [NotMapped]
        public EventType EventType
        {
            get
            {
                return (EventType)EventTypeId;
            }

            set
            {
                EventTypeId = (int)value;
            }
        }
        [NotMapped]
        public string EventTypeName
        {
            get
            {
                return EventType.GetDescription();
            }
        }
    }
}
