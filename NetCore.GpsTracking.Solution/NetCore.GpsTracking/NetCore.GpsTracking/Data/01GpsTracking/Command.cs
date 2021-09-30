using NetCore.Websites.Data;

namespace NetCore.GpsTrackingModule.Data
{
    public class Command : RecordBaseGuid
    {
        public string Name { get; set; }
        public string Param1 { get; set; }
        public string Param2 { get; set; }
        public string Param3 { get; set; }
        public string Param4 { get; set; }
    }
}
