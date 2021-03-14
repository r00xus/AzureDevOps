using System.Web;
using System.Web.Mvc;

namespace MDS.Azure.DevOps.Web2
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
