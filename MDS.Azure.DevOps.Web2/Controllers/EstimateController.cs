using MDS.Azure.DevOps.Core;
using MDS.Azure.DevOps.Core.Models;
using MDS.Azure.DevOps.Core.Models.Config;
using MDS.Azure.DevOps.Excel;
using MDS.Azure.DevOps.Web.Controllers;
using MDS.Azure.DevOps.Web2.ActionResults;
using MDS.Azure.DevOps.Web2.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MDS.Azure.DevOps.Web2.Controllers
{
    public class EstimateController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Save(List<RITaskEstimate> items)
        {
            var json = JsonConvert.SerializeObject(items, Formatting.Indented);

            System.IO.File.WriteAllText(Server.MapPath("~/appdata/taskEstimate.json"), json);

            return new HttpStatusCodeResult(200);
        }

        private List<TaskEstimate> GetEstimateFile()
        {
            var fileName = Server.MapPath("~/appdata/taskEstimate.json");

            if (!System.IO.File.Exists(fileName)) return new List<TaskEstimate>();

            var json = System.IO.File.ReadAllText(fileName);

            return JsonConvert.DeserializeObject<List<TaskEstimate>>(json);
        }

        public ActionResult EstimateReport()
        {
            var report = new DevOpsReport(GetConfig());

            report.ExecEstimateReport(GetEstimateFile());

            return new JsonNetResult
            {
                Data = new
                {
                    rows = report.TaskEstimateReport
                }
            };
        }

        public ActionResult CreateExcel()
        {
            var config = GetConfig();

            var report = new DevOpsReport(config);

            report.ExecEstimateReport(GetEstimateFile());

            var excel = new ExcelReport();

            excel.AddList(report.TaskEstimateReport, new Dictionary<string, string>());

            var bytes = excel.GetXlsFile();

            var key = $"Execl{Guid.NewGuid().ToString()}";

            TempData[key] = new TmpFile
            {
                FileName = "Оценка задач.xlsx",
                Content = bytes
            };

            return new JsonNetResult
            {
                Data = new
                {
                    key = key
                }
            };
        }
    }
}