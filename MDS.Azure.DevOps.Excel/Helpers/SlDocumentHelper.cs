using SpreadsheetLight;
using System;

namespace MDS.Azure.DevOps.Excel.Helpers
{
    public static class SlDocumentHelper
    {
        public static void SetCellValueObject(this SLDocument doc, int rowIndex, int columnIndex, object data)
        {
            if (data == null) return;

            Type filedType = data.GetType();

            if (filedType == typeof(string))
                doc.SetCellValue(rowIndex, columnIndex, Convert.ToString(data));
            else if (filedType == typeof(int))
                doc.SetCellValue(rowIndex, columnIndex, Convert.ToInt32(data));
            else if (filedType == typeof(DateTime))
                doc.SetCellValue(rowIndex, columnIndex, Convert.ToDateTime(data));
            else if (filedType == typeof(decimal))
                doc.SetCellValue(rowIndex, columnIndex, Convert.ToDecimal(data));
            else if (filedType == typeof(bool))
                doc.SetCellValue(rowIndex, columnIndex, (bool)data == true ? "Да" : "Нет");
        }
    }
}
