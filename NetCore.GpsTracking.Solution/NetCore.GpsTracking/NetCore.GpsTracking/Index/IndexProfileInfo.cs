using NetCore.Library;
using NetCore.Websites.Index;
using System.Collections.Generic;

namespace NetCore.GpsTrackingModule.Index
{
    //Add to: Startup.cs
    //var ProfileInfo = IndexConfig.Add(IndexProfileInfo.ConfigIndex());
    public class IndexProfileInfo
    {
        public static IndexConfig ConfigIndex()
        {
            var config = new IndexConfig(Startup.Module);            
            config.Entity.Name = "ProfileInfo";
            config.Entity.Title = "Cài đặt thông tin";

            config.ComModel.Data = "ProfileInfo-Model-Data";
            config.ComModel.DefaultModel = new
            {
                Id = lib.Id0,
                Name = "",
                Email = "",
                Password = "",
                Phone = "",
                Admin = false,
                GpsDevices = new List<string>(),
                Geofences = new List<string>(),
                Notifications = new List<string>(),
            };
            config.ComList.Content.Data.Columns.AddRange(
                new List<ComListDataColumn>
                {
                    new ComListDataColumn
                    {
                        Title = "Họ tên người dùng",
                        Property = "Name",
                    },
                    new ComListDataColumn
                    {
                        Title = "Email đăng nhập",
                        Property = "Email",
                    },
                    new ComListDataColumn
                    {
                        Title = "Số điện thoại",
                        Property = "Phone",
                    },
                    new ComListDataColumn
                    {
                        Title = "Quyền quản trị viên",
                        Property = "Admin",
                    },
                    new ComListDataColumn
                    {
                        Title = "Phân quyền thiết bị",
                        Property = "ListGpsDevices",
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
