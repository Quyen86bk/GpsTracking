using NetCore.Library;
using NetCore.Websites.Index;
using System.Collections.Generic;

namespace NetCore.GpsTrackingModule.Index
{
    //Add to: Startup.cs
    //var Permission = IndexConfig.Add(IndexPermission.ConfigIndex());
    public class IndexPermission
    {
        public static IndexConfig ConfigIndex()
        {
            var config = new IndexConfig(Startup.Module);            
            config.Entity.Name = "Permission";
            config.Entity.Title = "Cấp quyền cho người dùng";

            config.ComModel.Data = "Permission-Model-Data";
            config.ComModel.DefaultModel = new
            {
                Id = lib.Id0,
                ReadOnly = false,
                Registration = false,
            };
            config.ComList.Content.Data.Columns.AddRange(
                new List<ComListDataColumn>
                {
                    new ComListDataColumn
                    {
                        Title = "Chế độ quan sát",
                        Property = "ReadOnly",
                        Width = 100,
                    },
                    new ComListDataColumn
                    {
                        Title = "Cấp quyền đăng ký tài khoản mới",
                        Property = "Registration",
                        Width = 100,
                    },
                }
            );
            return config;
        }
    }
}
