using NetCore.Library;
using NetCore.Websites.Index;
using System.Collections.Generic;

namespace NetCore.GpsTrackingModule.Index
{
    //Add to: Startup.cs
    //var Notificator = IndexConfig.Add(IndexNotificator.ConfigIndex());
    public class IndexNotificator
    {
        public static IndexConfig ConfigIndex()
        {
            var config = new IndexConfig(Startup.Module);            
            config.Entity.Name = "Notificator";
            config.Entity.Title = "Thêm phương thức";

            config.ComModel.Data = "Notificator-Model-Data";
            config.ComModel.DefaultModel = new
            {
                Id = lib.Id0,
                Name = "",
            };
            config.ComList.Content.Data.Columns.AddRange(
                new List<ComListDataColumn>
                {
                    new ComListDataColumn
                    {
                        Title = "Notificator",
                        Property = "Name",
                        Width = 100,
                    },
                }
            );
            return config;
        }
    }
}
