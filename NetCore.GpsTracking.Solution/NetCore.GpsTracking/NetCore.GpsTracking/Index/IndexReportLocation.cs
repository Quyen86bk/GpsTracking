using NetCore.Library;
using NetCore.Websites.Index;
using System.Collections.Generic;

namespace NetCore.GpsTrackingModule.Index
{
    //Add to: Startup.cs
    //var ReportLocation = IndexConfig.Add(IndexReportLocation.ConfigIndex());
    public class IndexReportLocation
    {
        public static IndexConfig ConfigIndex()
        {
            var config = new IndexConfig(Startup.Module);            
            config.Entity.Name = "ReportLocation";
            config.Entity.Title = "Báo cáo lộ trình";
            config.Entity.EnableExport = true;
            config.ComMain.Filter = "ReportLocation-Filter";
            config.ComMain.Utility = "ReportLocation-Utility";
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
                        Title = "Cập nhật lần cuối",
                        Property = "CreatedTime",
                        Type = "DateTime"
                    },
                    new ComListDataColumn
                    {
                        Title = "Địa chỉ",
                        Property = "Address",
                    },
                    new ComListDataColumn
                    {
                        Title = "Vĩ độ",
                        Property = "Latitude",
                    },
                    new ComListDataColumn
                    {
                        Title = "Kinh độ",
                        Property = "Longitude",
                    },
                    new ComListDataColumn
                    {
                        Title = "Tốc độ đối tượng (km/h)",
                        Property = "Speed",
                    },
                }
            );
            return config;
        }
    }
}
