using Microsoft.EntityFrameworkCore;
using NetCore.Library;
using NetCore.GpsTrackingModule.Controllers;
using NetCore.GpsTrackingModule.Data;
using NetCore.Websites;
using NetCore.Websites.Models;
using NetCore.Websites.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore.GpsTrackingModule.Services
{
    public partial interface IUpdateService
    {
        Task Index(Pipeline pipeline, string deviceCode, float longitude, float latitude);
    }

    public partial class UpdateService : _ServiceGpsTracking, IUpdateService
    {
        public UpdateService(IServiceProvider _ServiceProvider)
            : base(_ServiceProvider)
        {
        }

        public async Task Index(Pipeline pipeline, string deviceCode, float longitude, float latitude)
        {
            var gpsDevice = DBs.GpsDevice.Query.FirstOrDefault(x => x.Code == deviceCode);
            if (gpsDevice == null)
            {
                gpsDevice = new GpsDevice
                {
                    Code = deviceCode,
                    Name = "test",
                };
                DBs.GpsDevice.Insert(gpsDevice);
            }

            if (gpsDevice != null)
            {
                var location = new Location
                {
                    GpsDeviceId = gpsDevice.Id,
                    Longitude = longitude,
                    Latitude = latitude,
                };
                DBs.Location.Insert(location);
                await DBs.Save();

                //Result
                pipeline.Status = ResponseStatus.Successful;
                pipeline.Result = new
                {
                };
            }
            else
                pipeline.Status = ResponseStatus.Error;
        }
    }
}
