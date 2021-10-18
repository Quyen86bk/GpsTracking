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
using NetCore.GpsTrackingModule.Models;
using NetCore.GpsTrackingModule.Library;

namespace NetCore.GpsTrackingModule.Services
{
    public partial interface IReportEventService
    {
        Task GetPage(Pipeline pipeline, ReportEventFilterVM filter);
    }

    public partial class ReportEventService : _ServiceGpsTracking, IReportEventService
    {
        public ReportEventService(IServiceProvider _ServiceProvider)
            : base(_ServiceProvider)
        {
        }

        public async Task GetPage(Pipeline pipeline, ReportEventFilterVM filter)
        {
            //Select
            var ReportEvents = DBs.Event.Query;

            //filter.Date
            if (filter.HaveDate())
                ReportEvents = ReportEvents.Where(x => x.CreatedTime >= filter.FromDate && x.CreatedTime <= filter.ToDate);

            var gpsDeviceName = "All";
            if (lib.Selected(filter.GpsDeviceId))
            {
                var gpsDevice = DBs.GpsDevice.Query.FirstOrDefault(x => x.Id == filter.GpsDeviceId);
                if (gpsDevice != null)
                {
                    gpsDeviceName = gpsDevice.Name;
                    ReportEvents = ReportEvents.Where(x => x.GpsDeviceId == filter.GpsDeviceId);
                }
            }

            var groupName = "All";
            if (lib.Selected(filter.GroupId))
            {
                var group = DBs.Group.Query
                    .Include(x => x.GpsDeviceMappings)
                    .FirstOrDefault(x => x.Id == filter.GroupId);
                if (group != null)
                {
                    groupName = group.Name;
                    ReportEvents = ReportEvents.Where(x => group.GpsDeviceMappings.Any(y => y.GpsDeviceId == x.GpsDeviceId));
                }
            }

            if (lib.Selected(filter.TypeId))
                ReportEvents = ReportEvents.Where(x => x.TypeId == filter.TypeId);

            if (lib.Selected(filter.TimeRange))
            {
                var timeFrom = new DateTime(lib.Time.Year, lib.Time.Month, lib.Time.Day);
                var timeTo = lib.Time;
                int BeginofWeek(DateTime dt)
                {
                    switch (dt.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            return 0;
                        case DayOfWeek.Tuesday:
                            return 1;
                        case DayOfWeek.Wednesday:
                            return 2;
                        case DayOfWeek.Thursday:
                            return 3;
                        case DayOfWeek.Friday:
                            return 4;
                        case DayOfWeek.Saturday:
                            return 5;
                        case DayOfWeek.Sunday:
                            return 6;
                    }
                    throw new Exception("Error!");
                }

                if (filter.TimeRange == 2)
                {
                    timeFrom = new DateTime(lib.Time.Year, lib.Time.Month, lib.Time.Day).AddDays(-1);
                    timeTo = new DateTime(lib.Time.Year, lib.Time.Month, lib.Time.Day).AddTicks(-1);
                }
                else if (filter.TimeRange == 3)
                {
                    timeFrom = lib.Time.AddDays(-1 * BeginofWeek(lib.Time));
                    timeTo = timeFrom.AddDays(6);
                }
                else if (filter.TimeRange == 4)
                {
                    timeFrom = new DateTime(lib.Time.Year, lib.Time.Month, 1).AddDays(-1);
                    timeTo = lib.Time;
                }

                ReportEvents = ReportEvents.Where(x => x.CreatedTime >= timeFrom && x.CreatedTime <= timeTo);
            }

            //Order
            ReportEvents = ReportEvents.OrderByDescending(x => x.CreatedTime);

            //Models
            var Total = ReportEvents.Count();
            var Models = await ReportEvents
                .Include(x => x.GpsDevice)
                .Include(x => x.Geofence)
                .Skip(filter.From).Take(Config.MaxSelectPage(filter.From, filter.To))
                .ToListAsync();

            //write file

            string DownloadUrl = "";
            if (filter.ExportFile == true || filter.SendEmail == true)
            {
                var export = new ReportExport(pipeline, filter, "Event", "Report-Event", false, 9, Models.Count - 1, 1, 5);

                export.Sheet.SetExcelCell(4, 2, gpsDeviceName);
                export.Sheet.SetExcelCell(5, 2, groupName);

                if (filter.HaveDate())
                    export.Sheet.SetExcelCell(6, 2, filter.FromDate + " - " + filter.ToDate);
                else
                    export.Sheet.SetExcelCell(6, 2, "All");

                foreach (var model in Models)
                {
                    export.Sheet.SetExcelCell(export.CurrentRow, 1, export.CurrentRow - export.StartRow + 1);

                    export.Sheet.SetExcelCell(export.CurrentRow, 2, model.GpsDevice.Name);
                    export.Sheet.SetExcelCell(export.CurrentRow, 3, lib.NiceDateTime3(model.CreatedTime));
                    export.Sheet.SetExcelCell(export.CurrentRow, 4, model.TypeName);
                    export.Sheet.SetExcelCell(export.CurrentRow, 5, model.Geofence.Name);

                    export.CurrentRow++;
                }
                await export.SaveAsAsync();

                if (filter.ExportFile == true)
                    DownloadUrl = export.DownloadUrl;

                if (filter.SendEmail == true)
                {
                    var ProfileInfo = DBs.ProfileInfo.Query.FirstOrDefault(x => x.Id == pipeline.UserId);

                    string subject = "Báo cáo trạng thái: " + lib.Time;
                    string content = "Báo cáo trạng thái của phần mềm GPS, tính đến thời điểm " + lib.Time;

                    EmailHelper.Send(new List<string> { ProfileInfo.Email }, subject, content, export.OutputFile);

                    //Result
                    pipeline.Status = ResponseStatus.CustomSuccessful;
                    pipeline.CustomOKMessage = "Đã gửi báo cáo đến địa chỉ email " + ProfileInfo.Email + lib.Line + "Vui lòng kiểm tra hộp thư";
                    pipeline.Result = new
                    {
                        Models,
                        Total,
                    };

                    return;
                }
            }

            //Result
            pipeline.Status = ResponseStatus.Successful;
            pipeline.Result = new
            {
                Models,
                Total,
                DownloadUrl,
            };
        }
    }
}
