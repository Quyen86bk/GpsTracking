using NetCore.Library;
using NetCore.Websites.Index;
using System.Collections.Generic;

namespace NetCore.GpsTrackingModule.Index
{
    //Add to: Startup.cs
    //var Command = IndexConfig.Add(IndexCommand.ConfigIndex());
    public class IndexCommand
    {
        public static IndexConfig ConfigIndex()
        {
            var config = new IndexConfig(Startup.Module);            
            config.Entity.Name = "Command";
            config.Entity.Title = "Cài đặt lệnh điều khiển";

            config.ComModel.Data = "Command-Model-Data";
            config.ComModel.DefaultModel = new
            {
                Id = lib.Id0,
                Name = "",
                Param1 = "",
                Param2 = "",
                Param3 = "",
                Param4 = "",
            };
            config.ComList.Content.Data.Columns.AddRange(
                new List<ComListDataColumn>
                {
                    new ComListDataColumn
                    {
                        Title = "Command",
                        Property = "Name",
                        Width = 100,
                    },
                    new ComListDataColumn
                    {
                        Property = "Param1",
                        Width = 100,
                    },
                    new ComListDataColumn
                    {
                        Property = "Param2",
                        Width = 100,
                    },
                    new ComListDataColumn
                    {
                        Property = "Param3",
                        Width = 100,
                    },
                    new ComListDataColumn
                    {
                        Property = "Param4",
                        Width = 100,
                    },
                }
            );
            return config;
        }
    }
}
