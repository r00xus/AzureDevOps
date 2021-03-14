using MDS.Azure.DevOps.Excel.Attributes;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Runtime.Serialization;

namespace MDS.Azure.DevOps.Models
{
    [ListInfo(Title = "Расхождения с графиком")]
    [DataContract]
    public class RIWorkingTimeDiff
    {
        [FieldInfo(Title = "Сотрудник")]
        [DataMember(Name = "employeeName")]
        public string EmployeeName { get; set; }

        [FieldInfo(Title = "Дата", Format = "dd.MM.yyyy")]
        [DataMember(Name = "day")]
        public DateTime Day { get; set; }

        [FieldInfo(Title = "День недели")]
        [DataMember(Name = "dayOfWeek")]
        public string DayOfWeek { get; set; }

        [FieldInfo(Title = "Тип дня")]
        [DataMember(Name = "description")]
        public string Description { get; set; }

        [FieldInfo(Title = "Часы по DevOps", Format = "#,##0.00", HorizontalAlignment = HorizontalAlignmentValues.Right)]
        [DataMember(Name = "devOpsHours")]
        public decimal DevOpsHours { get; set; }

        [FieldInfo(Title = "Часы по графику", Format = "#,##0.00", HorizontalAlignment = HorizontalAlignmentValues.Right)]
        [DataMember(Name = "schedualeHours")]
        public decimal SchedualeHours { get; set; }

        [FieldInfo(Title = "Разница", Format = "#,##0.00", HorizontalAlignment = HorizontalAlignmentValues.Right)]
        [DataMember(Name = "diff")]
        public decimal Diff { get; set; }
    }
}
