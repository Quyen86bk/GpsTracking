using NetCore.Library;
using NetCore.Websites.Index;
using System.Collections.Generic;

namespace NetCore.GpsTrackingModule.Index
{
    //Add to: Startup.cs
    //var Geofence = IndexConfig.Add(IndexGeofence.ConfigIndex());
    public class IndexGeofence
    {
        public static IndexConfig ConfigIndex()
        {
            var config = new IndexConfig(Startup.Module);            
            config.Entity.Name = "Geofence";
            config.Entity.Title = "Giới hạn địa lý";

            config.ComModel.Data = "Geofence-Model-Data";
            config.ComModel.DefaultModel = new
            {
                Id = lib.Id0,
                Name = "",
                Type = "",
                Area = "",
                Note = "",
            };
            config.ComList.Content.Data.Columns.AddRange(
                new List<ComListDataColumn>
                {
                    new ComListDataColumn
                    {
                        Title = "Giới hạn địa lý",
                        Property = "Name",
                    },
                    new ComListDataColumn
                    {
                        Title = "Ghi chú giới hạn",
                        Property = "Note",
                    }
                }
            );
            return config;
        }
    }
}
