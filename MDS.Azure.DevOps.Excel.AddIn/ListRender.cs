using MDS.Azure.DevOps.Excel.Attributes;
using MDS.Azure.DevOps.Excel.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MsExcel = Microsoft.Office.Interop.Excel;

namespace MDS.Azure.DevOps.Excel.AddIn
{
    public class ListInfo
    {
        public List<object> List { get; set; }

        public string Title { get; set; }

        public Dictionary<string, FieldInfoAttribute> FieldInfo { get; set; } = new Dictionary<string, FieldInfoAttribute>();

        public Dictionary<string, string> Criteria { get; set; } = new Dictionary<string, string>();
    }


    public class ListRender
    {
        public ListRender(MsExcel.Workbook workbook)
        {
            _workbook = workbook;
        }

        public const int FontSize = 10;

        public const string FontName = "Calibri";

        private MsExcel.Workbook _workbook { get; set; }

        private List<ListInfo> _listInfo { get; set; } = new List<ListInfo>();

        protected int _lineNum = 0;

        protected Dictionary<string, FieldInfoAttribute> CreateFieldInfo(Type type)
        {
            var result = new Dictionary<string, FieldInfoAttribute>();

            var properties = type.GetProperties();

            foreach (var propery in properties)
            {
                var attr = type.GetAttributeFromType<FieldInfoAttribute>(propery.Name);

                if (attr == null) attr = new FieldInfoAttribute();

                if (attr.Title == null)
                    attr.Title = propery.Name;

                result[propery.Name] = attr;
            }

            return result;
        }

        protected string GetTitle(Type type)
        {
            var attr = (ListInfoAttribute)type.GetCustomAttributes(typeof(ListInfoAttribute), true).FirstOrDefault();

            if (attr == null) return type.Name;

            return attr.Title;
        }

        public void AddList<T>(List<T> list, Dictionary<string, string> criteria)
        {
            var item = new ListInfo();

            item.List = list.Cast<object>().ToList();
            item.Criteria = criteria;
            item.Title = GetTitle(typeof(T));
            item.FieldInfo = CreateFieldInfo(typeof(T));

            _listInfo.Add(item);
        }

        protected void CreateTitle(MsExcel.Worksheet worksheet, string title, int width)
        {
            MsExcel.Range start = worksheet.Cells[_lineNum, 1];
            MsExcel.Range end = worksheet.Cells[_lineNum, width];

            MsExcel.Range range = worksheet.Range[start, end];
            range.Merge();

            range.Font.Name = FontName;
            range.Font.Size = FontSize + 5;
            range.Font.Bold = true;

            range.Value2 = title;

            _lineNum++;
        }

        protected void CreateCaption(MsExcel.Worksheet worksheet, Dictionary<string, FieldInfoAttribute> fieldInfo)
        {
            int i = 1;

            foreach (var name in fieldInfo.Keys)
            {
                MsExcel.Range cell = worksheet.Cells[_lineNum, i];

                cell.Value2 = fieldInfo[name].Title;

                cell.Font.Name = FontName;
                cell.Font.Size = FontSize;
                cell.Font.Bold = true;

                cell.HorizontalAlignment = MsExcel.XlHAlign.xlHAlignCenter;
                cell.VerticalAlignment = MsExcel.XlVAlign.xlVAlignCenter;

                cell.Borders[MsExcel.XlBordersIndex.xlEdgeLeft].Color = System.Drawing.Color.Black;
                cell.Borders[MsExcel.XlBordersIndex.xlEdgeRight].Color = System.Drawing.Color.Black;
                cell.Borders[MsExcel.XlBordersIndex.xlEdgeTop].Color = System.Drawing.Color.Black;
                cell.Borders[MsExcel.XlBordersIndex.xlEdgeBottom].Color = System.Drawing.Color.Black;

                i++;
            }

            _lineNum++;
        }

        protected string GetUniqWorksheetName(string name)
        {
            int i = 1;

            var result = name;

            List<MsExcel.Worksheet> sheets = _workbook.Worksheets.Cast<MsExcel.Worksheet>().ToList();

            while (true)
            {
                if (!sheets.Any(x => x.Name == result))
                    return result;

                result = name + i.ToString();

                i++;
            }
        }

        private void CreateDataList(MsExcel.Worksheet worksheet, List<object> dataList,
            Dictionary<string, FieldInfoAttribute> fieldsInfo)
        {
            foreach (var item in dataList)
            {
                int i = 1;
                foreach (string name in fieldsInfo.Keys)
                {
                    object fieldValue = item.GetType().GetProperty(name).GetValue(item);

                    MsExcel.Range cell = worksheet.Cells[_lineNum, i];

                    cell.Value = fieldValue;

                    cell.Font.Name = FontName;
                    cell.Font.Size = FontSize;

                    cell.Borders[MsExcel.XlBordersIndex.xlEdgeLeft].Color = System.Drawing.Color.Black;
                    cell.Borders[MsExcel.XlBordersIndex.xlEdgeRight].Color = System.Drawing.Color.Black;
                    cell.Borders[MsExcel.XlBordersIndex.xlEdgeTop].Color = System.Drawing.Color.Black;
                    cell.Borders[MsExcel.XlBordersIndex.xlEdgeBottom].Color = System.Drawing.Color.Black;

                    i++;
                }
                _lineNum++;
            }
        }

        public void Render()
        {
            foreach (var item in _listInfo)
            {
                _lineNum = 1;

                MsExcel.Worksheet sheet = _workbook.Worksheets.Add();

                sheet.Name = GetUniqWorksheetName(item.Title);

                CreateTitle(sheet, item.Title, item.FieldInfo.Count);

                CreateCaption(sheet, item.FieldInfo);

                CreateDataList(sheet, item.List, item.FieldInfo);
            }
        }
    }
}
