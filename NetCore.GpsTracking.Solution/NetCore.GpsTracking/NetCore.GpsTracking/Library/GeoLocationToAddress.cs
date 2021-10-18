using NetCore.GpsTrackingModule.Models;
using NetCore.Library;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.GpsTrackingModule.Library
{
    public class GeoLocationToAddress
    {   
        public static string Get(float longitude, float latitude)
        {
            string address = "";
            try
            {
                string url = "https://nominatim.openstreetmap.org/reverse?format=json&lat=" + latitude + "&lon=" + longitude;
                using (xWebClient webClient = new xWebClient())
                {
                    webClient.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0";
                    string addressJson = webClient.DownloadString(url);
                    
                    var geoLocation = json.JsonToObject<GeoLocationVM>(addressJson);
                    if (geoLocation != null)
                        address = geoLocation.display_name;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return address;
        }
    }
}
