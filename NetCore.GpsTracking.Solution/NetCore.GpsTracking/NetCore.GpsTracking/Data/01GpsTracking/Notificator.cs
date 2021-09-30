using NetCore.Websites.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCore.GpsTrackingModule.Data
{
    public class Notificator : RecordBaseGuid
    {
        public List<NotificatorMapping> NotificatorMappings { get; set; }
        public string Name { get; set; }
    }
}
