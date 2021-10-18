using NetCore.Library;
using NetCore.Websites.Data;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCore.GpsTrackingModule.Data
{
    public class Event : RecordBaseGuid
    {
        [ForeignKey("GpsDevice")]
        public Guid GpsDeviceId { get; set; }
        public GpsDevice GpsDevice { get; set; }

        [ForeignKey("Location")]
        public Guid? LocationId { get; set; }
        public Location Location { get; set; }

        [ForeignKey("Geofence")]
        public Guid? GeofenceId { get; set; }
        public Geofence Geofence { get; set; }

        public int TypeId { get; set; }
        [JsonIgnore]
        [NotMapped]
        public EventType Type
        {
            get
            {
                return (EventType)TypeId;
            }

            set
            {
                TypeId = (int)value;
            }
        }
        [NotMapped]
        public string TypeName
        {
            get
            {
                return Type.GetDescription();
            }
        }
    }

    public enum EventType
    {
        [Description(lib.UnKnow)]
        UnKnow = 0,

        [Description("Online")]
        Online = 1,

        [Description("Offline")]
        Offline = 2,

        [Description("Đi vào Khu vực")]
        EnterGeofence = 3,

        [Description("Ra khỏi Khu vực")]
        ExitGeofence = 4,

        [Description("Tiếp cận Thiết bị khác")]
        Meet = 5,
    }
}
