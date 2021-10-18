using NetCore.Websites.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.GpsTrackingModule.Models
{
    public class LocationFilterVM : PageFilterVM
    {
        public Guid? GpsDeviceId { get; set; }
        public Guid? GroupId { get; set; }
    }
}