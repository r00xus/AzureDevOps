using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MDS.Azure.DevOps.Core;
using MDS.Azure.DevOps.Core.Models.Config;
using MDS.Azure.DevOps.Excel;
using MDS.Azure.DevOps.Web.Controllers;
using MDS.Azure.DevOps.Web2.ActionResults;
using Newtonsoft.Json;

namespace MDS.Azure.DevOps.Web2.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DevOpsReport(DevOpsReportParams @params)
        {
            var report = new DevOpsReport(@params, GetConfig(), GetTaskEstimate());

            return new JsonNetResult
            {
                Data = new
                {
                    activity = report.ActivityReport,
                    diff = report.WorkingTimeDiffReport
                }
            };
        }

        private List<TaskEstimate> GetTaskEstimate()
        {
            var fileName = Server.MapPath("~/appdata/taskEstimate.json");

            if (!System.IO.File.Exists(fileName)) return new List<TaskEstimate>();

            var json = System.IO.File.ReadAllText(fileName);

            return JsonConvert.DeserializeObject<List<TaskEstimate>>(json);
        }

        public ActionResult CreateExcel(DevOpsReportParams @params)
        {
            var config = GetConfig();

            var report = new DevOpsReport(@params, config, GetTaskEstimate());

            var excel = new ExcelReport();

            var criteria = new Dictionary<string, string>
            {
                { "Период", $"{@params.Start:dd.MM.yyyy} - {(@params.End ?? DateTime.Now.Date):dd.MM.yyyy}" },
                { "Сотрудники", string.Join(", ", config.Employees.Where(x=> @params.Employees.Contains(x.Name)).Select(x=>x.NameShort).OrderBy(x=>x)) },
            };

            excel.AddList(report.ActivityReport, criteria);
            excel.AddList(report.TaskReport, criteria);
            excel.AddList(report.WorkingTimeDiffReport, criteria);
            excel.AddList(report.TaskEstimateReport, criteria);

            var bytes = excel.GetXlsFile();

            var key = $"Execl{Guid.NewGuid().ToString()}";

            TempData[key] = bytes;

            return new JsonNetResult
            {
                Data = new
                {
                    key = key
                }
            };
        }

        public ActionResult GetExcel(string key)
        {
            byte[] bytes = TempData[key] as byte[];

            if (bytes == null) throw new Exception($"Excel файл с ключом {key} не найден!");

            TempData[key] = null;            

            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Отчет DevOps.xlsx");
        }
    }
}