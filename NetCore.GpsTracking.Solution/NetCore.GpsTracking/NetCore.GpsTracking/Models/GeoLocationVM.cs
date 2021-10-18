using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.GpsTrackingModule.Models
{
    public class GeoLocationVM
    {
        public List<GeoLocationDetailVM> features { get; set; }
        public string display_name { get; set; }
    }

    public class GeoLocationDetailVM
    {
        public string place_name { get; set; }
    }
}
