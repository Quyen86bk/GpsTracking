using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.GpsTrackingModule.Models
{
    public class GeofenceVM
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Note { get; set; }

        public List<List<float>> Geofences { get; set; }
    }
}
