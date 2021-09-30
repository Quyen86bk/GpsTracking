using Microsoft.EntityFrameworkCore;
using NetCore.CommonModule.Data;
using NetCore.ManageModule.Data;
using NetCore.GpsTrackingModule.Controllers;
using NetCore.TranslateModule.Data;
using NetCore.Websites.Data;
using NetCore.Websites.Data.dbLogin;
using System;

namespace NetCore.GpsTrackingModule.Data
{
    public partial class DBGpsTracking : DbContext
    {
        public readonly IServiceProvider ServiceProvider;
        public DBGpsTracking(DbContextOptions<DBGpsTracking> options, IServiceProvider _ServiceProvider)
            : base(options)
        {
            this.ServiceProvider = _ServiceProvider;
        }

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);

            OnModelCreating_Referenced(b, ServiceProvider);
            OnModelCreating_ConfigOrIgnore(b, ServiceProvider);
            OnModelCreating_FKwithIgnored(b, ServiceProvider);
            OnModelCreating_HasKey_HasBase(b, ServiceProvider);
            OnModelCreating_DisableDeleteCascade(b, ServiceProvider);
            OnModelCreating_Ignore(b, ServiceProvider);
        }
        public static void OnModelCreating_Referenced(ModelBuilder b, IServiceProvider _ServiceProvider)
        {
            DBManage.OnModelCreating_HasKey_HasBase(b, _ServiceProvider);
            DBTranslate.OnModelCreating_HasKey_HasBase(b, _ServiceProvider);
            DBCommon.OnModelCreating_HasKey_HasBase(b, _ServiceProvider);
        }
        public static void OnModelCreating_ConfigOrIgnore(ModelBuilder b, IServiceProvider _ServiceProvider)
        {
            DBLogin.ConfigOrIgnore(b, _ServiceProvider);

            DBManage.ConfigOrIgnore(b, _ServiceProvider);
            DBTranslate.ConfigOrIgnore(b, _ServiceProvider);
            DBCommon.ConfigOrIgnore(b, _ServiceProvider);
        }
        public static void OnModelCreating_FKwithIgnored(ModelBuilder b, IServiceProvider _ServiceProvider)
        {
        }
        public static void OnModelCreating_DisableDeleteCascade(ModelBuilder b, IServiceProvider _ServiceProvider)
        {
        }
        public static void OnModelCreating_Ignore(ModelBuilder b, IServiceProvider _ServiceProvider)
        {
            DBLogin.Ignore(b, _ServiceProvider);

            DBManage.ConfigOrIgnore(b, _ServiceProvider);
            DBTranslate.ConfigOrIgnore(b, _ServiceProvider);
            DBCommon.ConfigOrIgnore(b, _ServiceProvider);
        }

        public static void CreateOtherMapping(IServiceProvider _ServiceProvider, string server)
        {
            //Add to: GpsTrackingRepositoryDI.cs
            //DBGpsTracking.CreateOtherMapping(_ServiceProvider, server);
            var repository = new GpsTrackingRepository(_ServiceProvider);
            DBLogin.CreateMapping(_ServiceProvider, server, repository.DB);

            DBManage.CreateMapping(_ServiceProvider, server, repository.DB);
            DBTranslate.CreateMapping(_ServiceProvider, server, repository.DB);
            DBCommon.CreateMapping(_ServiceProvider, server, repository.DB);
        }
        public static void ConfigOrIgnore(ModelBuilder b, IServiceProvider _ServiceProvider)
        {
            if (Tool.IsDesignMode)
            {
                Ignore(b, _ServiceProvider);
            }
            else
            {
                ToTable(b, _ServiceProvider);
            }
        }
    }
}
