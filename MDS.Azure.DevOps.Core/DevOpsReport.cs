using MDS.Azure.DevOps.Core.Models.Config;
using MDS.Azure.DevOps.Models;
using MDS.Azure.DevOps.Reader;
using MDS.Azure.DevOps.Reader.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MDS.Azure.DevOps.Core
{
    public class DevOpsReportParams
    {
        public DateTime Start { get; set; }

        public DateTime? End { get; set; }

        public List<string> Employees { get; set; } = new List<string>();
    }

    public class DevOpsReport
    {
        public DevOpsReport(DevOpsReportParams @params, ConfigBase config)
        {
            _params = @params;
            _config = config;

            GetDataFromDevOps();
            CreateActivityReport();
            CreateWorkingTimeDiffReport();
        }

        private DevOpsReportParams _params { get; set; }

        private ConfigBase _config { get; set; }

        public List<WIActivityDto> Activities { get; set; }

        public List<RIActivity> ActivityReport { get; set; }

        public List<RIWorkingTime> WorkingTimeReport { get; set; }

        public List<RIWorkingTimeDiff> WorkingTimeDiffReport { get; set; }

        public List<RITask> TaskReport { get; set; }

        const string AzureUrl = "https://dev.azure.com/mihvsts/";

        public void GetDataFromDevOps()
        {
            var reader = new DevOpsReader(AzureUrl);

            Activities = reader.GetActivities(_params.Employees, _params.Start, _params.End);
        }

        private void CreateActivityReport()
        {
            ActivityReport = new List<RIActivity>();

            foreach (var item in Activities)
            {
                var reportItem = new RIActivity();

                reportItem.AssigndTo = item.AssigndTo;
                reportItem.Position = _config.Employees.FirstOrDefault(x => x.Name == reportItem.AssigndTo)?.Position;
                reportItem.ActivityId = item.Id;
                reportItem.ActivityName = item.Name;
                reportItem.TaskId = item.Task.Id;
                reportItem.TaskName = item.Task.Name;
                reportItem.StartDate = item.Task.StartDate;
                reportItem.FinishDate = item.Task.FinishDate;
                reportItem.TargetDate = item.TargetDate;
                if (!string.IsNullOrWhiteSpace(item.Task.mdsTaskDescription1))
                    reportItem.ServiceName = Regex.Replace(HttpUtility.HtmlDecode(item.Task.mdsTaskDescription1), @"<[^>]+>|&nbsp;", "").Trim();
                reportItem.Technology = item.Task.mdsTaskDescription2;
                reportItem.WorkType = item.Task.mdsTaskWorkType;
                reportItem.Company = item.Task.mdsTaskActive;
                reportItem.CompletedWork = item.CompletedWork;
                if (reportItem.TargetDate != null)
                    reportItem.Month = ((DateTime)item.TargetDate).ToString("MMMM", CultureInfo.CreateSpecificCulture("ru-RU"));
                reportItem.TaskState = item.Task.State;
                reportItem.AreaPath = item.AreaPath;

                ActivityReport.Add(reportItem);
            }

            ActivityReport = ActivityReport.OrderByDescending(x => x.ActivityId).ToList();
        }

        private void CreateWorkingTimeDiffReport()
        {
            WorkingTimeDiffReport = new List<RIWorkingTimeDiff>();

            foreach (var employee in _params.Employees)
            {
                var schedual = new Schedual(new SchedualParams
                {
                    Name = employee,
                    Start = _params.Start,
                    End = _params.End
                }, _config);

                foreach (var day in schedual.Days)
                {
                    var item = new RIWorkingTimeDiff();

                    item.EmployeeName = employee;
                    item.Day = day.Date;
                    item.SchedualeHours = day.Hours;
                    item.Description = day.Description;
                    item.DevOpsHours = Activities
                        .Where(x => x.AssigndTo == employee && x.TargetDate?.Date == item.Day).Sum(x => x.CompletedWork);
                    item.Diff = item.DevOpsHours - item.SchedualeHours;

                    var culture = new CultureInfo("ru-RU");
                    item.DayOfWeek = culture.DateTimeFormat.GetDayName(item.Day.DayOfWeek);

                    if (item.Diff != 0)
                    {
                        WorkingTimeDiffReport.Add(item);
                    }
                }
            }

            WorkingTimeDiffReport = WorkingTimeDiffReport
                .OrderBy(x => x.EmployeeName)
                .ThenByDescending(x => x.Day).ToList();
        }
    }
}

