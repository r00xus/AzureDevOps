using MDS.Azure.DevOps.Excel.Attributes;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Runtime.Serialization;

namespace MDS.Azure.DevOps.Models
{

    [ListInfo(Title = "Отработанное время")]
    [DataContract]
    public class RIWorkingTime
    {
        [FieldInfo(Title = "Сотрудник")]
        [DataMember(Name = "employeeName")]
        public string EmployeeName { get; set; }

        [FieldInfo(Title = "Часы по DevOps", Format = "#,##0.00", HorizontalAlignment = HorizontalAlignmentValues.Right)]
        [DataMember(Name = "devOpsHours")]
        public decimal DevOpsHours { get; set; }

        [FieldInfo(Title = "Часы по графику", Format = "#,##0.00", HorizontalAlignment = HorizontalAlignmentValues.Right)]
        [DataMember(Name = "schedualeHours")]
        public decimal SchedualeHours { get; set; }

        [FieldInfo(Title = "Не хватает", Format = "#,##0.00", HorizontalAlignment = HorizontalAlignmentValues.Right)]
        [DataMember(Name = "diff")]
        public decimal Diff { get; set; }
    }
}
