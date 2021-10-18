using NetCore.Library;
using NetCore.Websites.Index;
using System.Collections.Generic;

namespace NetCore.GpsTrackingModule.Index
{
    //Add to: Startup.cs
    //var Event = IndexConfig.Add(IndexEvent.ConfigIndex());
    public class IndexEvent
    {
        public static IndexConfig ConfigIndex()
        {
            var config = new IndexConfig(Startup.Module);            
            config.Entity.Name = "Event";
            config.Entity.Title = "Trạng thái";

            config.ComModel.Data = "Event-Model-Data";
            config.ComModel.DefaultModel = new
            {
                Id = lib.Id0,
                EventName = "",
                EventTime = "",
            };
            config.ComList.Content.Data.Columns.AddRange(
                new List<ComListDataColumn>
                {
                    new ComListDataColumn
                    {
                        Title = "Thiết bị",
                        Property = "GpsDevice.Name",
                    },
                    new ComListDataColumn
                    {
                        Title = "Thông báo",
                        Property = "EventName",
                    },
                    new ComListDataColumn
                    {
                        Title = "Thời điểm",
                        Property = "EventTime",
                        Type = "DateTime"
                    },
                    new ComListDataColumn
                    {
                        Title = "Giới hạn địa lý",
                        Property = "Geofence.Name",
                    },
                }
            );
            return config;
        }
    }
}
