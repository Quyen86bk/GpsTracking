using NetCore.GpsTrackingModule.Data;
using NetCore.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NetCore.GpsTrackingModule.Models
{
    public class LocationVM
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public int CategoryId { get; set; }

        public int StatusId { get; set; }

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
        public List<List<float>> Locations { get; set; }
        public List<float> Last { get; set; }
        public List<int> Status { get; set; }
        public string Address { get; set; }
        public List<EventGeofenceVM> EventGeofences { get; set; } = new List<EventGeofenceVM>();
    }

    public class EventGeofenceVM
    {
        public string Event { get; set; }
        public string Geofence { get; set; }
    }
}
