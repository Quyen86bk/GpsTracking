using NetCore.Library;
using NetCore.Websites.Index;

namespace NetCore.GpsTrackingModule.Index
{
    //Add to: Startup.cs
    //IndexConfig.Add(IndexGpsDevice20.ConfigIndex(GpsDevice));
    public class IndexGpsDevice20
    {
        public static IndexConfig ConfigIndex(IndexConfig copy)
        {
            var config = json.CloneObject<IndexConfig>(copy);
            config.Option = 20;
            config.OnlyModel();
            return config;
        }
    }
}
