using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDS.Azure.DevOps.Core.Models.Config
{
    public class SpecialDay
    {
        public string Description { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public decimal Hours { get; set; }

    }
}
