using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDS.Azure.DevOps.Excel.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ListInfoAttribute : Attribute
    {
        public string Title { get; set; }
    }
}
