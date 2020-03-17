using MDS.Azure.DevOps.Console.Models;
using MDS.Azure.DevOps.Core;
using MDS.Azure.DevOps.Excel;
using MDS.Azure.DevOps.Reader;
using MDS.Azure.DevOps.Reader.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MDS.Azure.DevOps.Console
{
    class Program
    {
        static List<WIActivityDto> Activities { get; set; }

        static List<RIActivity> ActivityReport { get; set; }

        static List<RIWorkingTime> WorkingTimeReport { get; set; }

        static List<RIWorkingTimeDiff> WorkingTimeDiffReport { get; set; }

        static List<RITask> TaskReport { get; set; }


        static void Main(string[] args)
        {
            //System.Console.WriteLine($"Загрузка данных из DevOps URL: {AzureUrl}");

            //GetDataFromDevOps();

            //System.Console.WriteLine($"Формирование отчетов");

            //CreateActivityReport();
            //CreateTaskReport();
            //CreateTimeReport();
            //CreateWorkingTimeDiffReport();

            //System.Console.WriteLine($"Создание документа");

            //CreateDocument();
        }

    //    static void CreateDocument()
    //    {
    //        var report = new ExcelReport();

    //        var criteria = new Dictionary<string, string>
    //        {
    //            { "Период", $"{AppConfig.Instance.StartDate:dd.MM.yyyy} - {(AppConfig.Instance.EndDate ?? DateTime.Now.Date):dd.MM.yyyy}" },
    //            { "Сотрудники", string.Join(", ", AppConfig.Instance.Employees.Select(x=>x.NameShort).OrderBy(x=>x)) },
    //        };

    //        report.AddList(ActivityReport, criteria);
    //        report.AddList(TaskReport, criteria);
    //        report.AddList(WorkingTimeReport, criteria);
    //        report.AddList(WorkingTimeDiffReport, criteria);

    //        var doc = report.CreateDocument();

    //        var fileName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AzureDevOpsReport.xlsx";

    //        System.Console.WriteLine($"Сохранение документа {fileName}");

    //        doc.SaveAs(fileName);

    //        Process.Start(fileName);
    //    }

    //    const string AzureUrl = "https://dev.azure.com/mihvsts/";

    //    static void GetDataFromDevOps()
    //    {
    //        var reader = new DevOpsReader(AzureUrl);

    //        var employee = AppConfig.Instance.Employees.Select(x => x.Name).ToList();

    //        Activities = reader.GetActivities(employee, AppConfig.Instance.StartDate, AppConfig.Instance.EndDate);
    //    }

    //    static void CreateActivityReport()
    //    {
    //        ActivityReport = new List<RIActivity>();

    //        foreach (var item in Activities)
    //        {
    //            var reportItem = new RIActivity();

    //            reportItem.AssigndTo = item.AssigndTo;
    //            reportItem.Position = AppConfig.Instance.Employees.FirstOrDefault(x => x.Name == reportItem.AssigndTo)?.Position;
    //            reportItem.ActivityId = item.Id;
    //            reportItem.ActivityName = item.Name;
    //            reportItem.TaskId = item.Task.Id;
    //            reportItem.TaskName = item.Task.Name;
    //            reportItem.StartDate = item.Task.StartDate;
    //            reportItem.FinishDate = item.Task.FinishDate;
    //            reportItem.TargetDate = item.TargetDate;
    //            if (!string.IsNullOrWhiteSpace(item.Task.mdsTaskDescription1))
    //                reportItem.ServiceName = Regex.Replace(HttpUtility.HtmlDecode(item.Task.mdsTaskDescription1), @"<[^>]+>|&nbsp;", "").Trim();
    //            reportItem.Technology = item.Task.mdsTaskDescription2;
    //            reportItem.WorkType = item.Task.mdsTaskWorkType;
    //            reportItem.Company = item.Task.mdsTaskActive;
    //            reportItem.CompletedWork = item.CompletedWork;
    //            if (reportItem.TargetDate != null)
    //                reportItem.Month = ((DateTime)item.TargetDate).ToString("MMMM", CultureInfo.CreateSpecificCulture("ru-RU"));
    //            reportItem.TaskState = item.Task.State;
    //            reportItem.AreaPath = item.AreaPath;

    //            ActivityReport.Add(reportItem);
    //        }

    //        ActivityReport = ActivityReport.OrderByDescending(x => x.ActivityId).ToList();
    //    }

    //    static void CreateTimeReport()
    //    {
    //        WorkingTimeReport = new List<RIWorkingTime>();

    //        foreach (var employee in AppConfig.Instance.Employees)
    //        {
    //            var schedual = new Schedual(employee.Name, AppConfig.Instance.StartDate, AppConfig.Instance.EndDate);

    //            var item = new RIWorkingTime();

    //            item.EmployeeName = employee.Name;

    //            item.SchedualeHours = schedual.Hours;

    //            item.DevOpsHours = ActivityReport.Where(x => x.AssigndTo == employee.Name).Sum(x => x.CompletedWork);

    //            item.Diff = item.DevOpsHours - item.SchedualeHours;

    //            WorkingTimeReport.Add(item);
    //        }
    //    }

    //    static void CreateTaskReport()
    //    {
    //        TaskReport = new List<RITask>();

    //        var tasks = ActivityReport.GroupBy(x => new
    //        {
    //            x.TaskId,
    //            x.TaskName,
    //            x.AssigndTo,
    //            x.Position,
    //            x.StartDate,
    //            x.FinishDate,
    //            x.ServiceName,
    //            x.Technology,
    //            x.TaskState,
    //            x.Company,
    //            x.WorkType,
    //            x.AreaPath,
    //            x.Month

    //        });

    //        foreach (var task in tasks)
    //        {
    //            var item = new RITask();

    //            item.TaskId = task.Key.TaskId;
    //            item.TaskName = task.Key.TaskName;
    //            item.AssigndTo = task.Key.AssigndTo;
    //            item.Position = task.Key.Position;
    //            item.StartDate = task.Key.StartDate;
    //            item.FinishDate = task.Key.FinishDate;
    //            item.ServiceName = task.Key.ServiceName;
    //            item.Technology = task.Key.Technology;
    //            item.CompletedWork = task.Sum(x => x.CompletedWork);
    //            item.TaskState = task.Key.TaskState;
    //            item.WorkType = task.Key.WorkType;
    //            item.AreaPath = task.Key.AreaPath;
    //            item.Month = task.Key.Month;
    //            item.Company = task.Key.Company;

    //            TaskReport.Add(item);
    //        }
    //    }

    //    static void CreateWorkingTimeDiffReport()
    //    {
    //        WorkingTimeDiffReport = new List<RIWorkingTimeDiff>();

    //        foreach (var employee in AppConfig.Instance.Employees)
    //        {
    //            var schedual = new Schedual(employee.Name, AppConfig.Instance.StartDate, AppConfig.Instance.EndDate);

    //            foreach (var day in schedual.Days)
    //            {
    //                var item = new RIWorkingTimeDiff();

    //                item.EmployeeName = employee.Name;
    //                item.Day = day.Date;
    //                item.SchedualeHours = day.Hours;
    //                item.Description = day.Description;
    //                item.DevOpsHours = Activities
    //                    .Where(x => x.AssigndTo == employee.Name && x.TargetDate?.Date == item.Day).Sum(x => x.CompletedWork);
    //                item.Diff = item.DevOpsHours - item.SchedualeHours;

    //                var culture = new CultureInfo("ru-RU");
    //                item.DayOfWeek = culture.DateTimeFormat.GetDayName(item.Day.DayOfWeek);

    //                if (item.Diff != 0)
    //                {
    //                    WorkingTimeDiffReport.Add(item);
    //                }
    //            }
    //        }

    //        WorkingTimeDiffReport = WorkingTimeDiffReport
    //            .OrderBy(x => x.EmployeeName)
    //            .ThenByDescending(x => x.Day).ToList();
    //    }
    }
}
