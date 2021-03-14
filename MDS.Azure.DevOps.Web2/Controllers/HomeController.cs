using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MDS.Azure.DevOps.Core;
using MDS.Azure.DevOps.Core.Models;
using MDS.Azure.DevOps.Excel;
using MDS.Azure.DevOps.Web.Controllers;
using MDS.Azure.DevOps.Web2.ActionResults;
using MDS.Azure.DevOps.Web2.Models;
using Newtonsoft.Json;

namespace MDS.Azure.DevOps.Web2.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        private List<WICustomField> GetCustomFieldsFile()
        {
            var fileName = Server.MapPath("~/appdata/workItemCustomFields.json");

            if (!System.IO.File.Exists(fileName)) return new List<WICustomField>();

            var json = System.IO.File.ReadAllText(fileName);

            return JsonConvert.DeserializeObject<List<WICustomField>>(json);
        }

        public ActionResult DevOpsReport(DevOpsReportParams @params)
        {
            try
            {
                var report = new DevOpsReport(GetConfig(), GetCustomFieldsFile());

                report.ExecMainReport(@params);

                return new JsonNetResult
                {
                    Data = new
                    {
                        success = true,
                        activity = report.ActivityReport,
                        task = report.TaskReport,
                        diff = report.WorkingTimeDiffReport,
                        time = report.WorkingTimeReport
                    }
                };
            }
            catch (Exception e)
            {
                return new ExceptionResult(e);
            }
        }

        public ActionResult CreateExcel(DevOpsReportParams @params)
        {
            try
            {

                var config = GetConfig();

                var report = new DevOpsReport(config, GetCustomFieldsFile());

                report.ExecMainReport(@params);

                var excel = new ExcelReport();

                var criteria = new Dictionary<string, string>
                {
                    { "Период", $"{@params.Start:dd.MM.yyyy} - {(@params.End ?? DateTime.Now.Date):dd.MM.yyyy}" },
                    { "Сотрудники", string.Join(", ", config.Employees.Where(x=> @params.Employees.Contains(x.Name)).Select(x=>x.NameShort).OrderBy(x=>x)) },
                };

                excel.AddList(report.ActivityReport, criteria);
                excel.AddList(report.TaskReport, criteria);
                excel.AddList(report.WorkingTimeReport, criteria);
                excel.AddList(report.WorkingTimeDiffReport, criteria);

                var bytes = excel.GetXlsFile();

                var key = $"Execl{Guid.NewGuid().ToString()}";

                TempData[key] = new TmpFile
                {
                    Content = bytes,
                    FileName = "Аналитический отчет.xlsx"
                };

                return new JsonNetResult
                {
                    Data = new
                    {
                        key = key
                    }
                };
            }
            catch (Exception e)
            {
                return new ExceptionResult(e);
            }
        }

        public ActionResult GetExcel(string key)
        {
            var tmpFile = TempData[key] as TmpFile;

            if (tmpFile == null) throw new Exception($"Excel файл с ключом {key} не найден!");

            TempData[key] = null;

            return File(tmpFile.Content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", tmpFile.FileName);
        }
    }
}