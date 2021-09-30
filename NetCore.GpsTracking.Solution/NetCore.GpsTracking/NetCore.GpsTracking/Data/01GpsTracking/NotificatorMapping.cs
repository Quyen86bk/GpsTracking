using NetCore.Websites.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCore.GpsTrackingModule.Data
{
    public class NotificatorMapping : RecordBaseGuid
    {
        [ForeignKey("Notification")]
        public Guid? NotificationId { get; set; }
        [JsonIgnore]
        public Notification Notification { get; set; }

        [ForeignKey("Notificator")]
        public Guid? NotificatorId { get; set; }
        [JsonIgnore]
        public Notificator Notificator { get; set; }
    }
}
