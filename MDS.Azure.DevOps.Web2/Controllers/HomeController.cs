using System.Web.Mvc;
using MDS.Azure.DevOps.Core;
using MDS.Azure.DevOps.Web.Controllers;
using MDS.Azure.DevOps.Web2.ActionResults;

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
            var report = new DevOpsReport(@params, GetConfig());

            return new JsonNetResult
            {
                Data = new
                {
                    activity = report.ActivityReport,
                    diff = report.WorkingTimeDiffReport
                }
            };
        }
    }
}