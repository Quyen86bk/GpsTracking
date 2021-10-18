using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.GpsTrackingModule.Models
{
    public class ReplayVM
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int StatusId { get; set; }

        public float Latitude { get; set; }
        public float Longitude { get; set; }

        public long Tick { get; set; }
        public string Time { get; set; }
    }
}
