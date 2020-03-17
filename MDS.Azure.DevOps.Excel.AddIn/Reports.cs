using MDS.Azure.DevOps.Console.Models;
using Microsoft.Office.Tools.Ribbon;
using System;
using System.Collections.Generic;
using MsExcel = Microsoft.Office.Interop.Excel;

namespace MDS.Azure.DevOps.Excel.AddIn
{
    public partial class Reports
    {
        private MsExcel.Workbook Workbook { get { return Globals.ThisAddIn.Application.ActiveWorkbook; } }


        private void Reports_Load(object sender, RibbonUIEventArgs e)
        {

        }

        private void btnExecute_Click(object sender, RibbonControlEventArgs e)
        {
            var render = new ListRender(Workbook);

            var list = new List<RIWorkingTimeDiff>();

            for (int i = 1; i <= 100; i++)
            {
                list.Add(new RIWorkingTimeDiff
                {
                    EmployeeName = $"Item {i}",
                    Day = DateTime.Now.AddDays(i)
                });
            }

            render.AddList(list, new Dictionary<string, string>());

            render.Render();
        }
    }
}
