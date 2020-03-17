using MDS.Azure.DevOps.Excel.Attributes;
using DocumentFormat.OpenXml.Spreadsheet;
using System;

namespace MDS.Azure.DevOps.Models
{
    [ListInfo(Title = "Активности")]
    public class RIActivity
    {
        [FieldInfo]
        public int TaskId { get; set; }

        [FieldInfo(Width = 40)]
        public string TaskName { get; set; }

        public string TaskState { get; set; }

        [FieldInfo]
        public int ActivityId { get; set; }

        [FieldInfo(Width = 40)]
        public string ActivityName { get; set; }

        [FieldInfo(Format = "dd.MM.yyyy")]
        public DateTime? StartDate { get; set; }

        [FieldInfo(Format = "dd.MM.yyyy")]
        public DateTime? FinishDate { get; set; }

        public string AssigndTo { get; set; }

        [FieldInfo(Title = "Должность")]
        public string Position { get; set; }

        [FieldInfo(Format = "dd.MM.yyyy")]
        public DateTime? TargetDate { get; set; }

        [FieldInfo(Title = "Месяц")]
        public string Month { get; set; }

        [FieldInfo(Format = "#,##0.00", HorizontalAlignment = HorizontalAlignmentValues.Right)]
        public decimal CompletedWork { get; set; }

        [FieldInfo(Title = "Услуга", Width = 40)]
        public string ServiceName { get; set; }

        [FieldInfo(Title = "Технология")]
        public string Technology { get; set; }

        [FieldInfo(Title = "Вид работ")]
        public string WorkType { get; set; }

        [FieldInfo(Title = "Актив")]
        public string Company { get; set; }

        public string AreaPath { get; set; }
    }
}
