using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MDS.Azure.DevOps.Core.Models.Config
{
    [DataContract]
    public class Employee
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "nameShort")]
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

        [DataMember(Name = "position")]
        public string Position { get; set; }

        [DataMember(Name = "specialDays")]
        public List<SpecialDay> SpecialDays { get; set; } = new List<SpecialDay>();

        [DataMember(Name = "customDays")]
        public List<CustomDay> CustomDays { get; set; } = new List<CustomDay>();
    }
}
