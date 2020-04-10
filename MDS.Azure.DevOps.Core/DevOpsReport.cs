using MDS.Azure.DevOps.Core.Models;
using MDS.Azure.DevOps.Core.Models.Config;
using MDS.Azure.DevOps.Models;
using MDS.Azure.DevOps.Reader;
using MDS.Azure.DevOps.Reader.Models;
using Newtonsoft.Json;
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
        public DevOpsReport(ConfigBase config, List<WICustomField> customFields = null)
        {
            _config = config;

            if (customFields != null)
            {
                _customFields = customFields;
            }
        }

        public void ExecMainReport(DevOpsReportParams @params)
        {
            _params = @params;

            GetDataFromDevOps();
            CreateActivityReport();
            CreateTaskReport();
            CreateWorkingTimeDiffReport();
            CreateTimeReport();
        }

        public void ExecEstimateReport(List<TaskEstimate> taskEstimates)
        {
            _taskEstimates = taskEstimates;
            CreateTaskEstimateReport();
        }

        public void FillCustomFields()
        {
            foreach (var activity in Activities)
            {
                var customField = _customFields.FirstOrDefault(x => x.Id == activity.Task?.Id);

                if (customField == null) continue;

                activity.Task.ProjectOnlineName = customField?.ProjectOnlineName;
            }
        }

        private DevOpsReportParams _params { get; set; }

        private ConfigBase _config { get; set; }
        private List<WICustomField> _customFields { get; set; } = new List<WICustomField>();
        private List<TaskEstimate> _taskEstimates { get; set; }

        public List<WIActivityDto> Activities { get; set; }

        public List<RIActivity> ActivityReport { get; set; }

        public List<RIWorkingTime> WorkingTimeReport { get; set; }

        public List<RIWorkingTimeDiff> WorkingTimeDiffReport { get; set; }

        public List<RITask> TaskReport { get; set; }

        public List<RITaskEstimate> TaskEstimateReport { get; set; }

        const string AzureUrl = "https://dev.azure.com/mihvsts/";

        public void GetDataFromDevOps()
        {
            var reader = new DevOpsReader(AzureUrl);

            Activities = reader.GetActivities(_params.Employees, _params.Start, _params.End);

            FillCustomFields();
        }

        private string DeleteHtmlTags(string val)
        {
            return Regex.Replace(HttpUtility.HtmlDecode(val), @"<[^>]+>|&nbsp;", "").Trim();
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
                    reportItem.ServiceName = DeleteHtmlTags(item.Task.mdsTaskDescription1);
                reportItem.Technology = item.Task.mdsTaskDescription2;
                reportItem.WorkType = item.Task.mdsTaskWorkType;
                reportItem.Company = item.Task.mdsTaskActive;
                reportItem.CompletedWork = item.CompletedWork;
                if (reportItem.TargetDate != null)
                    reportItem.Month = ((DateTime)item.TargetDate).ToString("MMMM", CultureInfo.CreateSpecificCulture("ru-RU"));
                reportItem.TaskState = item.Task.State;
                reportItem.AreaPath = item.AreaPath;
                reportItem.OriginalEstimate = item.Task.OriginalEstimate;
                reportItem.ProjectOnlineName = item.Task.ProjectOnlineName;


                ActivityReport.Add(reportItem);
            }

            ActivityReport = ActivityReport.OrderByDescending(x => x.ActivityId).ToList();
        }

        private void CreateTaskReport()
        {
            TaskReport = new List<RITask>();

            var tasks = ActivityReport.GroupBy(x => new
            {
                x.TaskId,
                x.TaskName,
                x.AssigndTo,
                x.Position,
                x.StartDate,
                x.FinishDate,
                x.ServiceName,
                x.Technology,
                x.TaskState,
                x.OriginalEstimate,
                x.Company,
                x.WorkType,
                x.AreaPath,
                x.Month,
                x.ProjectOnlineName

            });

            foreach (var task in tasks)
            {
                var item = new RITask();

                item.TaskId = task.Key.TaskId;
                item.TaskName = task.Key.TaskName;
                item.AssigndTo = task.Key.AssigndTo;
                item.Position = task.Key.Position;
                item.StartDate = task.Key.StartDate;
                item.FinishDate = task.Key.FinishDate;
                item.ServiceName = task.Key.ServiceName;
                item.Technology = task.Key.Technology;
                item.CompletedWork = task.Sum(x => x.CompletedWork);
                item.TaskState = task.Key.TaskState;
                item.WorkType = task.Key.WorkType;
                item.AreaPath = task.Key.AreaPath;
                item.Month = task.Key.Month;
                item.Company = task.Key.Company;
                item.OriginalEstimate = task.Key.OriginalEstimate;
                item.ProjectOnlineName = task.Key.ProjectOnlineName;

                TaskReport.Add(item);
            }
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

        private void CreateTaskEstimateReport()
        {
            var taskIds = _taskEstimates.Where(x => x.TaskId != null).Select(x => (int)x.TaskId).ToList();

            var reader = new DevOpsReader(AzureUrl);

            var tasks = reader.GetTasks(taskIds);

            TaskEstimateReport = new List<RITaskEstimate>();

            foreach (var taskEstimate in _taskEstimates)
            {
                var item = new RITaskEstimate();

                item.SpecName = taskEstimate.SpecName;
                item.Analytic = taskEstimate.Analytic;
                item.Date = taskEstimate.Date;
                item.Developer = taskEstimate.Developer;
                item.Reviewer = taskEstimate.Reviewer;
                item.EstimateDeveloperHours = taskEstimate.EstimateDeveloperHours;
                item.EstimateReviewerHours = taskEstimate.EstimateReviewerHours;

                item.TaskId = taskEstimate.TaskId;

                var task = tasks.FirstOrDefault(x => x.Id == item.TaskId);

                if (task != null)
                {
                    item.TaskName = task.Name;
                    item.Start = task.StartDate;
                    item.End = task.FinishDate;
                    item.EstimateFactHours = task.CompletedWork;
                    item.State = task.State;
                }

                TaskEstimateReport.Add(item);
            }

            TaskEstimateReport = TaskEstimateReport.OrderBy(x => x.Date).ToList();
        }

        private void CreateTimeReport()
        {
            WorkingTimeReport = new List<RIWorkingTime>();

            foreach (var employee in _params.Employees)
            {
                var schedual = new Schedual(new SchedualParams
                {
                    Name = employee,
                    Start = _params.Start,
                    End = _params.End
                }, _config);

                var item = new RIWorkingTime();

                item.EmployeeName = employee;

                item.SchedualeHours = schedual.Hours;

                item.DevOpsHours = ActivityReport.Where(x => x.AssigndTo == employee).Sum(x => x.CompletedWork);

                item.Diff = item.DevOpsHours - item.SchedualeHours;

                WorkingTimeReport.Add(item);
            }
        }
    }
}

