using NetCore.Library;
using NetCore.Websites.Index;
using System.Collections.Generic;

namespace NetCore.GpsTrackingModule.Index
{
    //Add to: Startup.cs
    //var GpsDevice = IndexConfig.Add(IndexGpsDevice.ConfigIndex());
    public class IndexGpsDevice
    {
        public static IndexConfig ConfigIndex()
        {
            var config = new IndexConfig(Startup.Module);            
            config.Entity.Name = "GpsDevice";
            config.Entity.Title = "Danh sách thiết bị";

            config.ComModel.Data = "GpsDevice-Model-Data";
            config.ComModel.DefaultModel = new
            {
                Id = lib.Id0,
                Name = "",
                Code = "",
                Phone = "",
                Category = "",
                Geofences = new List<string>(),
                Notifications = new List<string>(),
            };
            config.ComList.Content.Data.Columns.AddRange(
                new List<ComListDataColumn>
                {
                    new ComListDataColumn
                    {
                        Title = "Tên thiết bị",
                        Property = "Name",
                    },
                    new ComListDataColumn
                    {
                        Title = "Định danh thiết bị",
                        Property = "Code",
                    },
                    new ComListDataColumn
                    {
                        Title = "Số điện thoại",
                        Property = "Phone",
                    },
                    new ComListDataColumn
                    {
                        Title = "Đối tượng theo dõi",
                        Property = "CategoryName",
                    },
                    new ComListDataColumn
                    {
                        Title = "Phân quyền giới hạn địa lý",
                        Property = "ListGeofences",
                    },
                    new ComListDataColumn
                    {
                        Title = "Phân quyền thông báo",
                        Property = "ListNotifications",
                    },
                }
            );
            return config;
        }
    }
}
