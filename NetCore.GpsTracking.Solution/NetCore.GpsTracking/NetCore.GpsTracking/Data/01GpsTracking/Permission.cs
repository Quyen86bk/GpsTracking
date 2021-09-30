using NetCore.Websites.Data;

namespace NetCore.GpsTrackingModule.Data
{
    public class Permission : RecordBaseGuid
    {
        public bool ReadOnly { get; set; }
        public bool Registration { get; set; }
    }
}
