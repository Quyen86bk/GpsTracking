using NetCore.Websites.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCore.GpsTrackingModule.Data
{
    public class NotificationMapping : RecordBaseGuid
    {
        [ForeignKey("ProfileInfo")]
        public Guid? ProfileInfoId { get; set; }
        [JsonIgnore]
        public ProfileInfo ProfileInfo { get; set; }

        [ForeignKey("GpsDevice")]
        public Guid? GpsDeviceId { get; set; }
        [JsonIgnore]
        public GpsDevice GpsDevice { get; set; }

        [ForeignKey("Group")]
        public Guid? GroupId { get; set; }
        [JsonIgnore]
        public Group Group { get; set; }

        [ForeignKey("Notification")]
        public Guid? NotificationId { get; set; }
        [JsonIgnore]
        public Notification Notification { get; set; }
    }
}
