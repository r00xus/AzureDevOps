using MDS.Azure.DevOps.Excel.Attributes;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Runtime.Serialization;

namespace MDS.Azure.DevOps.Models
{
    [ListInfo(Title = "Задачи c активностями")]
    [DataContract]
    public class RITask
    {
        [FieldInfo(Title = "ID")]
        [DataMember(Name = "id")]
        public int TaskId { get; set; }

        [DataMember(Name = "worksItem")]
        public string WorksItem { get { return "Task"; } }

        [FieldInfo(Width = 40, Title = "Title")]
        [DataMember(Name = "taskName")]
        public string TaskName { get; set; }

        [DataMember(Name = "assigndTo")]
        public string AssigndTo { get; set; }

        [FieldInfo(Title = "State")]
        [DataMember(Name = "taskState")]
        public string TaskState { get; set; }

        [FieldInfo(Format = "dd.MM.yyyy", Title = "Start Date")]
        [DataMember(Name = "startDate")]
        public DateTime? StartDate { get; set; }

        [FieldInfo(Format = "dd.MM.yyyy", Title = "Finish Date")]
        [DataMember(Name = "finishDate")]
        public DateTime? FinishDate { get; set; }

        [FieldInfo(Format = "#,##0.00", HorizontalAlignment = HorizontalAlignmentValues.Right, Title = "Completed Work")]
        [DataMember(Name = "completedWork")]
        public decimal CompletedWork { get; set; }

        [FieldInfo(Title = "Area Path")]
        [DataMember(Name = "areaPath")]
        public string AreaPath { get; set; }

        [FieldInfo(Title = "mdsTaskDescription1", Width = 40)]
        [DataMember(Name = "serviceName")]
        public string ServiceName { get; set; }

        [FieldInfo(Title = "mdsTaskDescription2")]
        [DataMember(Name = "technology")]
        public string Technology { get; set; }

        [FieldInfo(Title = "mdsTaskWorkType")]
        [DataMember(Name = "workType")]
        public string WorkType { get; set; }

        [FieldInfo(Title = "mdsTaskActive")]
        [DataMember(Name = "company")]
        public string Company { get; set; }

        [FieldInfo(Title = "Месяц")]
        [DataMember(Name = "month")]
        public string Month { get; set; }

        [FieldInfo(Title = "Должность")]
        [DataMember(Name = "position")]
        public string Position { get; set; }

        [FieldInfo(Title = "Тип услуги")]
        [DataMember(Name = "serviceType")]
        public string ServiceType { get; set; }

        [FieldInfo(Format = "#,##0.00", HorizontalAlignment = HorizontalAlignmentValues.Right, Title = "Original Estimate")]
        [DataMember(Name = "originalEstimate")]
        public decimal OriginalEstimate { get; set; }

        [FieldInfo(Title = "Проект Project Online")]
        [DataMember(Name = "projectOnlineName")]
        public string ProjectOnlineName { get; set; }
    }
}
