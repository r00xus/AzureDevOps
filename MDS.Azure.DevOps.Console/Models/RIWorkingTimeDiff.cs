using MDS.Azure.DevOps.Excel.Attributes;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDS.Azure.DevOps.Console.Models
{
    [ListInfo(Title = "Расхождения с графиком")]
    public class RIWorkingTimeDiff
    {
        [FieldInfo(Title = "Сотрудник")]
        public string EmployeeName { get; set; }

        [FieldInfo(Title = "Дата", Format = "dd.MM.yyyy")]
        public DateTime Day { get; set; }

        [FieldInfo(Title = "День недели")]
        public string DayOfWeek { get; set; }

        [FieldInfo(Title = "Тип дня")]
        public string Description { get; set; }

        [FieldInfo(Title = "Часы по DevOps", Format = "#,##0.00", HorizontalAlignment = HorizontalAlignmentValues.Right)]
        public decimal DevOpsHours { get; set; }

        [FieldInfo(Title = "Часы по графику", Format = "#,##0.00", HorizontalAlignment = HorizontalAlignmentValues.Right)]
        public decimal SchedualeHours { get; set; }

        [FieldInfo(Title = "Разница", Format = "#,##0.00", HorizontalAlignment = HorizontalAlignmentValues.Right)]
        public decimal Diff { get; set; }
    }
}
