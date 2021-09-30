using NetCore.Websites.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCore.GpsTrackingModule.Data
{
    public class Notification : RecordBaseGuid
    {
        public List<NotificationMapping> NotificationMappings { get; set; }

        public string Name { get; set; }
        public bool Distribution { get; set; }

        public List<NotificatorMapping> NotificatorMappings { get; set; }

        [NotMapped]
        public List<string> Notificators { get; set; }
        [NotMapped]
        public string ListNotificators { get; set; }
    }
}
