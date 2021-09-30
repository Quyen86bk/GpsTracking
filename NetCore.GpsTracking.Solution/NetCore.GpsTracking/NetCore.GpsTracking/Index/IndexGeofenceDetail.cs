using NetCore.Library;
using NetCore.Websites.Index;
using System.Collections.Generic;

namespace NetCore.GpsTrackingModule.Index
{
    //Add to: Startup.cs
    //var GeofenceDetail = IndexConfig.Add(IndexGeofenceDetail.ConfigIndex());
    public class IndexGeofenceDetail
    {
        public static IndexConfig ConfigIndex()
        {
            var config = new IndexConfig(Startup.Module);            
            config.Entity.Name = "GeofenceDetail";
            config.Entity.Title = "GeofenceDetail";

            config.ComModel.Data = "GeofenceDetail-Model-Data";
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
                        Title = "GeofenceDetail",
                        Property = "Name",
                        Width = 100,
                    },
                }
            );
            return config;
        }
    }
}
