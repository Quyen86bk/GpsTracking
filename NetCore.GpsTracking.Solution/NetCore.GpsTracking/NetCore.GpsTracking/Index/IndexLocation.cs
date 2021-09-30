using NetCore.Library;
using NetCore.Websites.Index;
using System.Collections.Generic;

namespace NetCore.GpsTrackingModule.Index
{
    //Add to: Startup.cs
    //var Location = IndexConfig.Add(IndexLocation.ConfigIndex());
    public class IndexLocation
    {
        public static IndexConfig ConfigIndex()
        {
            var config = new IndexConfig(Startup.Module);            
            config.Entity.Name = "Location";
            config.Entity.Title = "Vị trí";

            config.ComModel.Data = "Location-Model-Data";
            config.ComModel.DefaultModel = new
            {
                Id = lib.Id0,
                Protocol = "",
                DeviceTime = "",
                Address = "",
                Latitude = "",
                Longitude = "",
                Speed = "",
                Course = "",
            };
            config.ComList.Content.Data.Columns.AddRange(
                new List<ComListDataColumn>
                {
                    new ComListDataColumn
                    {
                        Title = "Code",
                        Property = "GpsDevice.Code",
                    },
                    new ComListDataColumn
                    {
                        Title = "Device",
                        Property = "GpsDevice.Name",
                    },

                    new ComListDataColumn
                    {
                        Title = "Giao thức",
                        Property = "Protocol",
                    },
                    new ComListDataColumn
                    {
                        Title = "Cập nhật lần cuối",
                        Property = "DeviceTime",
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
                        Title = "Tốc độ đối tượng",
                        Property = "Speed",
                    },
                    new ComListDataColumn
                    {
                        Title = "Hướng di chuyển",
                        Property = "Course",
                    },
                }
            );
            return config;
        }
    }
}
