using MDS.Azure.DevOps.Core.Models.Config;
using System.IO;
using System.Web.Hosting;

namespace MDS.Azure.DevOps.Web.Models
{
    public class Config : ConfigBase
    {
        protected override string FilePath
        {
            get
            {
                return HostingEnvironment.MapPath("~/appdata/");
            }
        }
    }
}