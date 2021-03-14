using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Spreadsheet;
using MDS.Azure.DevOps.Excel.Attributes;
using MDS.Azure.DevOps.Excel.Helpers;
using SpreadsheetLight;

namespace MDS.Azure.DevOps.Excel
{

    /// <summary> базовый класс для отчетов в xls </summary>
    public class ExcelReport
    {
        public const int FontSize = 10;
        public const string FontName = "Calibri";

        // текущий номер строки в документе
        protected int LineNum = 1;
        protected int FilterStartLineNum = 0;
        protected int FilterEndLineNum = 0;
        protected int DataIndex = 0;

        /// <summary> документ excel </summary>
        protected SLDocument Document;

        /// <summary> стиль для титула отчета </summary>
        protected SLStyle TitleStyle;
        /// <summary> стил для критерие выбора</summary>
        protected SLStyle CriteriaStyle;
        /// <summary> стил для заголовка полей таблиц</summary>
        protected SLStyle CaptionStyle;

        protected List<Dictionary<string, string>> _criteria { get; set; } = new List<Dictionary<string, string>>();

        /// <summary> создание стилей </summary>
        protected void CreateDefaultStyles()
        {
            // стиль для титула отчета
            TitleStyle = Document.CreateStyle();
            TitleStyle.SetHorizontalAlignment(HorizontalAlignmentValues.Left);
            TitleStyle.SetVerticalAlignment(VerticalAlignmentValues.Center);

            TitleStyle.Font.FontSize = FontSize + 5;
            TitleStyle.Font.FontName = FontName;
            TitleStyle.Font.Bold = true;

            // стиль для заголовка полей таблицы
            CaptionStyle = Document.CreateStyle();
            CaptionStyle.SetLeftBorder(BorderStyleValues.Thin, System.Drawing.Color.Black);
            CaptionStyle.SetTopBorder(BorderStyleValues.Thin, System.Drawing.Color.Black);
            CaptionStyle.SetRightBorder(BorderStyleValues.Thin, System.Drawing.Color.Black);
            CaptionStyle.SetBottomBorder(BorderStyleValues.Thin, System.Drawing.Color.Black);
            CaptionStyle.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.Silver, System.Drawing.Color.Silver);

            CaptionStyle.SetHorizontalAlignment(HorizontalAlignmentValues.Center);
            CaptionStyle.SetVerticalAlignment(VerticalAlignmentValues.Center);

            CaptionStyle.Font.FontSize = FontSize;
            CaptionStyle.Font.FontName = FontName;
            CaptionStyle.Font.Bold = true;

            CaptionStyle.Alignment.WrapText = true;
        }

        protected Dictionary<string, SLStyle> CreateFieldStyles(Dictionary<string, FieldInfoAttribute> fieldInfo)
        {
            var result = new Dictionary<string, SLStyle>();

            // стиль для ячейки таблицы
            foreach (string name in fieldInfo.Keys)
            {
                SLStyle style = Document.CreateStyle();

                style.Font.FontSize = FontSize;
                style.Font.FontName = FontName;

                style.SetLeftBorder(BorderStyleValues.Thin, System.Drawing.Color.Black);
                style.SetTopBorder(BorderStyleValues.Thin, System.Drawing.Color.Black);
                style.SetRightBorder(BorderStyleValues.Thin, System.Drawing.Color.Black);
                style.SetBottomBorder(BorderStyleValues.Thin, System.Drawing.Color.Black);
                style.FormatCode = fieldInfo[name].Format ?? string.Empty;
                style.Alignment.WrapText = true;

                style.SetVerticalAlignment(VerticalAlignmentValues.Center);
                style.SetHorizontalAlignment(fieldInfo[name].HorizontalAlignment);

                result[name] = style;
            }

            return result;
        }

        /// <summary> создание заголовка отчета </summary>
        protected void CreateTitle(string title, int width)
        {
            Document.SetCellValue(LineNum, 1, title);
            Document.SetCellStyle(LineNum, 1, TitleStyle);
            Document.SetRowHeight(LineNum, 30);
            Document.MergeWorksheetCells(LineNum, 1, LineNum, width);
            LineNum++;
        }
        /// <summary> создание строки отчета </summary>
        protected void CreateDataLine(object Data, Dictionary<string, FieldInfoAttribute> fieldInfo, Dictionary<string, SLStyle> styles)
        {
            int i = 1;
            foreach (var name in fieldInfo.Keys)
            {
                object fieldValue = Data.GetType().GetProperty(name).GetValue(Data);

                Document.SetCellValueObject(LineNum, i, fieldValue);

                Document.SetCellStyle(LineNum, i, styles[name]);

                i++;
            }
            LineNum++;
        }

        /// <summary> создание заголовка таблицы </summary>
        protected void CreateCaption(Dictionary<string, FieldInfoAttribute> fieldInfo)
        {
            int i = 1;
            foreach (var name in fieldInfo.Keys)
            {
                Document.SetCellStyle(LineNum, i, CaptionStyle);
                Document.SetCellStyle(LineNum + 1, i, CaptionStyle);

                // заголовок
                Document.SetCellValue(LineNum, i, fieldInfo[name].Title);
                // номер столбца
                Document.SetCellValue(LineNum + 1, i, i);

                i++;
            }
            Document.SetRowHeight(LineNum, 40.0);
            LineNum += 2;
        }

        /// <summary> создание документа excel </summary>
        public SLDocument CreateDocument()
        {
            Document = new SLDocument();

            CreateDefaultStyles();

            for (var i = 0; i < _dataLists.Count; i++)
            {
                var dataList = _dataLists[i];
                var fieldsInfo = _fieldsInfo[i];
                var fieldStyles = CreateFieldStyles(fieldsInfo);
                var criteria = _criteria[i];
                var title = _titles[i];

                var worksheetName = GetUniqWorksheetName(title);

                Document.AddWorksheet(worksheetName);

                Document.SelectWorksheet(worksheetName);

                LineNum = 1;

                CreateTitle(worksheetName, fieldsInfo.Count);

                CreateCriteriaList(criteria, fieldsInfo.Count);

                CreateCaption(fieldsInfo);

                Document.FreezePanes(LineNum - 1, 0);

                FilterStartLineNum = LineNum - 1;
                CreateDataList(dataList, fieldsInfo, fieldStyles);
                FilterEndLineNum = LineNum - 1;

                Document.Filter(FilterStartLineNum, 1, FilterEndLineNum, fieldsInfo.Count);

                var j = 1;
                foreach (string name in fieldsInfo.Keys)
                {
                    if (fieldsInfo[name].Width != 0)
                        Document.SetColumnWidth(j, fieldsInfo[name].Width);
                    else
                        Document.AutoFitColumn(j);

                    j++;
                }
                Document.AutoFitRow(1, LineNum);
            }
            return Document;
        }

        private void CreateDataList(List<object> dataList, Dictionary<string, FieldInfoAttribute> fieldsInfo,
            Dictionary<string, SLStyle> fieldStyles)
        {
            foreach (var item in dataList)
            {
                int i = 1;
                foreach (string name in fieldsInfo.Keys)
                {
                    object fieldValue = item.GetType().GetProperty(name).GetValue(item);

                    Document.SetCellValueObject(LineNum, i, fieldValue);

                    Document.SetCellStyle(LineNum, i, fieldStyles[name]);

                    i++;
                }
                LineNum++;
            }
        }

        protected List<Dictionary<string, FieldInfoAttribute>> _fieldsInfo { get; set; } = new List<Dictionary<string, FieldInfoAttribute>>();

        protected List<List<object>> _dataLists { get; set; } = new List<List<object>>();

        protected List<string> _titles { get; set; } = new List<string>();

        public void AddList<T>(List<T> list, Dictionary<string, string> criteria)
        {
            var fieldInfo = CreateFieldInfo(typeof(T));

            _fieldsInfo.Add(fieldInfo);

            var dataList = list.Cast<object>().ToList();

            _dataLists.Add(dataList);

            var title = GetTitle(typeof(T));

            _titles.Add(title);

            if (criteria != null)
                _criteria.Add(criteria);
        }

        /// <summary> создаение списка критериев </summary>
        protected void CreateCriteriaList(Dictionary<string, string> criteria, int width)
        {
            foreach (var key in criteria.Keys)
            {
                SLFont captionFont = Document.CreateFont();
                captionFont.FontName = FontName;
                captionFont.FontSize = FontSize;
                captionFont.Bold = true;

                SLFont valueFont = Document.CreateFont();
                valueFont.FontName = FontName;
                valueFont.FontSize = FontSize;

                SLRstType rstType = Document.CreateRstType();

                rstType.AppendText(key + ": ", captionFont);
                rstType.AppendText(criteria[key], valueFont);

                Document.SetCellValue(LineNum, 1, rstType.ToInlineString());

                Document.MergeWorksheetCells(LineNum, 1, LineNum, width);
                LineNum++;
            }
        }

        protected Dictionary<string, FieldInfoAttribute> CreateFieldInfo(Type type)
        {
            var result = new Dictionary<string, FieldInfoAttribute>();

            var properties = type.GetProperties();

            foreach (var propery in properties)
            {
                var attr = type.GetAttributeFromType<FieldInfoAttribute>(propery.Name);

                if (attr == null) attr = new FieldInfoAttribute();

                if (attr.NoDisplay) continue;

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

        protected string GetUniqWorksheetName(string name)
        {
            int i = 1;

            var result = name;

            while (true)
            {
                if (!Document.GetWorksheetNames().Any(x => x == result))
                    return result;

                result = name + i.ToString();

                i++;
            }
        }

        /// <summary> создание документа excel </summary>
        public byte[] GetXlsFile()
        {
            SLDocument doc = CreateDocument();
            MemoryStream stream = new MemoryStream();
            doc.SaveAs(stream);
            stream.Position = 0;
            return stream.ToArray();
        }
    }
}
