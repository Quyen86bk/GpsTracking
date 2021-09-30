using NetCore.Library;
using NetCore.Websites.Index;
using System.Collections.Generic;

namespace NetCore.GpsTrackingModule.Index
{
    //Add to: Startup.cs
    //var Group = IndexConfig.Add(IndexGroup.ConfigIndex());
    public class IndexGroup
    {
        public static IndexConfig ConfigIndex()
        {
            var config = new IndexConfig(Startup.Module);            
            config.Entity.Name = "Group";
            config.Entity.Title = "Cài đặt nhóm thiết bị";

            config.ComModel.Data = "Group-Model-Data";
            config.ComModel.DefaultModel = new
            {
                Id = lib.Id0,
                Name = "",
                GpsDevices = new List<string>(),
                Notifications = new List<string>(),
            };
            config.ComList.Content.Data.Columns.AddRange(
                new List<ComListDataColumn>
                {
                    new ComListDataColumn
                    {
                        Title = "Tên nhóm",
                        Property = "Name",
                        Width = 100,
                    },
                    new ComListDataColumn
                    {
                        Title = "Thiết bị thuộc nhóm",
                        Property = "ListGpsDevices",
                    },
                    new ComListDataColumn
                    {
                        Title = "Thông báo của nhóm",
                        Property = "ListNotifications",
                    },
                    new ComListDataColumn
                    {
                        Title = "Giới hạn địa lý",
                        Property = "ListGeofences",
                    },
                }
            );
            return config;
        }
    }
}
