using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Text;


namespace NetCore.GpsTrackingModule.Library
{
    public class MapHelper
    {
        public static double Distance(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            var point1 = new GeoCoordinate(latitude1, longitude1);
            var point2 = new GeoCoordinate(latitude2, longitude2);

            return point1.GetDistanceTo(point2);
        }
        public static double DistanceKm(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            return Distance(latitude1, longitude1, latitude2, longitude2) / 1000;
        }
    }
}
