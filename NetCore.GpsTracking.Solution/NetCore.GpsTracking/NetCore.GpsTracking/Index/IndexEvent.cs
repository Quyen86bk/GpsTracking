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
            config.Entity.Title = "Báo cáo trạng thái";

            config.ComModel.Data = "Event-Model-Data";
            config.ComModel.DefaultModel = new
            {
                Id = lib.Id0,
                EventName = "",
                EventTime = "",
                GeofenceId = "",
                LocationId = "",
            };
            config.ComList.Content.Data.Columns.AddRange(
                new List<ComListDataColumn>
                {
                    new ComListDataColumn
                    {
                        Title = "Trạng thái",
                        Property = "EventName",
                        Width = 100,
                    },
                    new ComListDataColumn
                    {
                        Title = "Thời điểm",
                        Property = "EventTime",
                    },
                    new ComListDataColumn
                    {
                        Title = "Giới hạn địa lý",
                        Property = "GeofenceId",
                    },
                    new ComListDataColumn
                    {
                        Title = "Vị trí",
                        Property = "LocationId",
                        Width = 100,
                    },
                }
            );
            return config;
        }
    }
}
