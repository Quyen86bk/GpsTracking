using NetCore.Library;
using NetCore.Websites.Index;
using System.Collections.Generic;

namespace NetCore.GpsTrackingModule.Index
{
    //Add to: Startup.cs
    //var ReportEvent = IndexConfig.Add(IndexReportEvent.ConfigIndex());
    public class IndexReportEvent
    {
        public static IndexConfig ConfigIndex()
        {
            var config = new IndexConfig(Startup.Module);            
            config.Entity.Name = "ReportEvent";
            config.Entity.Title = "Báo cáo trạng thái";
            config.Entity.EnableExport = true;
            config.ComMain.Filter = "ReportEvent-Filter";
            config.ComMain.Utility = "ReportEvent-Utility";
            config.ComList.Content.Action.Title = null;
            config.ComList.EnableRowSelect = false;
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
                        Title = "Trạng thái",
                        Property = "TypeName",
                    },
                    new ComListDataColumn
                    {
                        Title = "Giới hạn địa lý",
                        Property = "Geofence.Name",
                    },
                    new ComListDataColumn
                    {
                        Title = "Cập nhật lần cuối",
                        Property = "CreatedTime",
                        Type = "DateTime"
                    },
                }
            );
            return config;
        }
    }
}
