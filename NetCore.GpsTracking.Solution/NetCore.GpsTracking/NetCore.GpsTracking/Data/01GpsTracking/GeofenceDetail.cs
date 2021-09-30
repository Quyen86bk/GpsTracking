using NetCore.Websites.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCore.GpsTrackingModule.Data
{
    public class GeofenceDetail : RecordBaseGuid
    {
        [ForeignKey("Geofence")]
        public Guid GeofenceId { get; set; }
        public Geofence Geofence { get; set; }

        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }
}
