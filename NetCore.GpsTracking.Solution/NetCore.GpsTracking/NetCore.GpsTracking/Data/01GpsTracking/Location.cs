using NetCore.Websites.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCore.GpsTrackingModule.Data
{
    public class Location : RecordBaseGuid
    {
        [ForeignKey("GpsDevice")]
        public Guid GpsDeviceId { get; set; }
        public GpsDevice GpsDevice { get; set; }

        public string Protocol { get; set; }
        public DateTime DeviceTime { get; set; }

        public string Address { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public float Speed { get; set; }
        public float Course { get; set; }
    }
}
