using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.GpsTrackingModule.Models
{
    public class LocationVM
    {
        public Guid Id { get; set; }
        public string Code { get; set; }

        public List<List<float>> Locations { get; set; }
        public List<float> Last { get; set; }
    }
}
