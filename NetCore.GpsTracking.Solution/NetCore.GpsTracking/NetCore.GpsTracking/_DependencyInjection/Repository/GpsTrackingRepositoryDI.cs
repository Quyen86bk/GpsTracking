using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCore.Library;
using NetCore.GpsTrackingModule.Data;
using NetCore.Websites;
using NetCore.Websites.Data;
using NetCore.Websites.Request;
using System;
using System.Threading.Tasks;

namespace NetCore.GpsTrackingModule
{
    //Add to: Startup.cs
    //GpsTrackingRepository.Inject(services);
    public partial class GpsTrackingRepository
    {
        public static void Inject(IServiceCollection services)
        {
            services.AddDbContext<DBGpsTracking>(options => { options.UseSqlServer(WebApp.GetConnectionString("DBGpsTracking"), builder => builder.MigrationsHistoryTable("__EFMigrationsHistory_" + DBGpsTracking.RepositoryName).CommandTimeout((int)(TimeSpan.FromDays(1).TotalSeconds)).EnableRetryOnFailure().UseRowNumberForPaging()); options.EnableSensitiveDataLogging(); });
            InjectRepository(services);
        }
        public static void Mapping(IServiceProvider _ServiceProvider, string server)
        {
            DBGpsTracking.CreateOtherMapping(_ServiceProvider, server);
        }
        public static void Migrate(IServiceProvider _ServiceProvider)
        {
            var repository = new GpsTrackingRepository(_ServiceProvider);
            NetCore.Websites.Data.Tool.Migrate(repository.DB);
        }
    }

    public partial class GpsTrackingRepository
    {
        readonly IServiceProvider ServiceProvider;
        public GpsTrackingRepository(IServiceProvider _ServiceProvider)
        {
            this.ServiceProvider = _ServiceProvider;
        }

        public DBGpsTracking _db;
        public DBGpsTracking DB
        {
            get
            {
                if (_db == null)
                {
                    _db = ServiceProvider.Get<DBGpsTracking>();
                }
                return _db;
            }
        }

        bool Loaded { get { return _db != null && DB != null; } }
        public async Task Save()
        {
            if (Loaded)
                await DB.SaveChangesAsync();
        }
        public void SaveSync()
        {
            if (Loaded)
                DB.SaveChanges();
        }
        public async Task Save(Pipeline pipeline, RecordBase obj)
        {
            if (obj != null)
            {
                obj.UpdatedById = pipeline.UserId;
                obj.UpdatedTime = lib.Time;
            }
            await Save();
        }
        public void SaveSync(Pipeline pipeline, RecordBase obj)
        {
            if (obj != null)
            {
                obj.UpdatedById = pipeline.UserId;
                obj.UpdatedTime = lib.Time;
            }
            SaveSync();
        }
    }
}