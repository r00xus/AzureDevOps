using MDS.Azure.DevOps.Excel.Attributes;
using DocumentFormat.OpenXml.Spreadsheet;
using System;

namespace MDS.Azure.DevOps.Models
{
    [ListInfo(Title = "Задачи")]
    public class RITask
    {
        [FieldInfo(Title = "ID")]
        public int TaskId { get; set; }

        public string WorksItem { get { return "Task"; } }

        [FieldInfo(Width = 40, Title = "Title")]
        public string TaskName { get; set; }

        public string AssigndTo { get; set; }

        [FieldInfo(Title = "State")]
        public string TaskState { get; set; }

        [FieldInfo(Format = "dd.MM.yyyy", Title = "Start Date")]
        public DateTime? StartDate { get; set; }

        [FieldInfo(Format = "dd.MM.yyyy", Title = "Finish Date")]
        public DateTime? FinishDate { get; set; }

        [FieldInfo(Format = "#,##0.00", HorizontalAlignment = HorizontalAlignmentValues.Right, Title = "Completed Work")]
        public decimal CompletedWork { get; set; }

        [FieldInfo(Title = "Area Path")]
        public string AreaPath { get; set; }

        [FieldInfo(Title = "mdsTaskDescription1", Width = 40)]
        public string ServiceName { get; set; }

        [FieldInfo(Title = "mdsTaskDescription2")]
        public string Technology { get; set; }

        [FieldInfo(Title = "mdsTaskWorkType")]
        public string WorkType { get; set; }

        [FieldInfo(Title = "mdsTaskActive")]
        public string Company { get; set; }

        [FieldInfo(Title = "Месяц")]
        public string Month { get; set; }

        [FieldInfo(Title = "Должность")]
        public string Position { get; set; }

        [FieldInfo(Title = "Тип услуги")]
        public string ServiceType { get; set; }
    }
}
