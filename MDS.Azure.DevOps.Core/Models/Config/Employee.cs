using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDS.Azure.DevOps.Core.Models.Config
{
    public class Employee
    {
        public string Name { get; set; }

        public string NameShort
        {
            get
            {
                var names = Name.Split(' ');

                var surname = names.ElementAtOrDefault(0);
                var name = names.ElementAtOrDefault(1);
                var midname = names.ElementAtOrDefault(2);

                var init = string.Empty;

                if (name != null) init += name[0] + ".";
                if (midname != null) init += midname[0] + ".";

                return $"{surname} {init}";

            }
        }

        public string Position { get; set; }

        public List<SpecialDay> SpecialDays { get; set; } = new List<SpecialDay>();

        public List<CustomDay> CustomDays { get; set; } = new List<CustomDay>();
    }
}
