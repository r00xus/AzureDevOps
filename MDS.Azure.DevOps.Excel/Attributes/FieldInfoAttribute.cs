using System;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDS.Azure.DevOps.Excel.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FieldInfoAttribute : Attribute
    {
        public string Title { get; set; }

        public int Width { get; set; }

        public HorizontalAlignmentValues HorizontalAlignment { get; set; } = HorizontalAlignmentValues.Left;

        public string Format { get; set; }

        public bool NoDisplay { get; set; } = false;
    }
}
