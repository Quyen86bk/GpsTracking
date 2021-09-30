using NetCore.Websites.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCore.GpsTrackingModule.Data
{
    public class Geofence : RecordBaseGuid
    {
        public List<GeofenceMapping> GeofenceMappings { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }

        public List<GeofenceDetail> Details { get; set; }
        [NotMapped]
        public List<List<float>> Geofences { get; set; }
    }
}
