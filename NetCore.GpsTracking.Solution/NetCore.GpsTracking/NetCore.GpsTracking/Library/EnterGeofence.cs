using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NetCore.GpsTrackingModule.Library
{
    public class GeofenceHelper
    {
        public static bool CheckEnter(LocationPoint[] geofence, LocationPoint location)
        {
            bool result = false;

            int j = geofence.Count() - 1;
            for (int i = 0; i < geofence.Count(); i++)
            {
                if (geofence[i].Longitude < location.Longitude && geofence[j].Longitude >= location.Longitude || geofence[j].Longitude < location.Longitude && geofence[i].Longitude >= location.Longitude)
                {
                    if (geofence[i].Latitude + (location.Longitude - geofence[i].Longitude) / (geofence[j].Longitude - geofence[i].Longitude) * (geofence[j].Latitude - geofence[i].Latitude) < location.Latitude)
                    {
                        result = !result;
                    }
                }
                j = i;
            }

            return result;
        }
    }

    public class LocationPoint
    {
        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }
}
