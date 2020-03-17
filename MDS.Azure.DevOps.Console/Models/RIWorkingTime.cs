using MDS.Azure.DevOps.Excel.Attributes;
using DocumentFormat.OpenXml.Spreadsheet;

namespace MDS.Azure.DevOps.Console.Models
{

    [ListInfo(Title = "Отработанное время")]
    public class RIWorkingTime
    {
        [FieldInfo(Title = "Сотрудник")]
        public string EmployeeName { get; set; }

        [FieldInfo(Title = "Часы по DevOps", Format = "#,##0.00", HorizontalAlignment = HorizontalAlignmentValues.Right)]
        public decimal DevOpsHours { get; set; }

        [FieldInfo(Title = "Часы по графику", Format = "#,##0.00", HorizontalAlignment = HorizontalAlignmentValues.Right)]
        public decimal SchedualeHours { get; set; }

        [FieldInfo(Title = "Не хватает", Format = "#,##0.00", HorizontalAlignment = HorizontalAlignmentValues.Right)]
        public decimal Diff { get; set; }
    }
}
