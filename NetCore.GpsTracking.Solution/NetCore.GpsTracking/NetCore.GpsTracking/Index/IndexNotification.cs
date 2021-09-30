using NetCore.Library;
using NetCore.Websites.Index;
using System.Collections.Generic;

namespace NetCore.GpsTrackingModule.Index
{
    //Add to: Startup.cs
    //var Notification = IndexConfig.Add(IndexNotification.ConfigIndex());
    public class IndexNotification
    {
        public static IndexConfig ConfigIndex()
        {
            var config = new IndexConfig(Startup.Module);            
            config.Entity.Name = "Notification";
            config.Entity.Title = "Cài đặt thông báo";

            config.ComModel.Data = "Notification-Model-Data";
            config.ComModel.DefaultModel = new
            {
                Id = lib.Id0,
                Name = "",
                Distribution = false,
                Notificators = new List<string>(),
            };
            config.ComList.Content.Data.Columns.AddRange(
                new List<ComListDataColumn>
                {
                    new ComListDataColumn
                    {
                        Title = "Thông báo",
                        Property = "Name",
                    },
                    new ComListDataColumn
                    {
                        Title = "Phương thức",
                        Property = "ListNotificators",
                    },
                    new ComListDataColumn
                    {
                        Title = "Phân quyền cho mọi thiết bị",
                        Property = "Distribution",
                    },
                }
            );
            return config;
        }
    }
}
