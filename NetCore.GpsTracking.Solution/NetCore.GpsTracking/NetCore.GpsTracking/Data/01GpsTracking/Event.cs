using NetCore.Websites.Data;

namespace NetCore.GpsTrackingModule.Data
{
    public class Event : RecordBaseGuid
    {
        public string EventName { get; set; }
        public string EventTime { get; set; }
        public string GeofenceId { get; set; }
        public string LocationId { get; set; }

    }
}
