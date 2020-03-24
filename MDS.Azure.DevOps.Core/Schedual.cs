using MDS.Azure.DevOps.Core;
using MDS.Azure.DevOps.Core.Models.Config;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MDS.Azure.DevOps.Core
{
    public class Day
    {
        public DateTime Date { get; set; }

        public decimal Hours { get; set; }

        public string Description { get; set; }
    }

    public class SchedualParams
    {
        public string Name { get; set; }

        public DateTime Start { get; set; }

        public DateTime? End { get; set; }
    }

    public class Schedual
    {
        public Schedual(SchedualParams @params, ConfigBase config)
        {
            _params = @params;
            _config = config;

            DateTime date = _params.Start;

            DateTime endDate = _params.End ?? DateTime.Now;

            var employee = _config.Employees.FirstOrDefault(x => x.Name == _params.Name);

            while (date <= endDate)
            {
                var day = new Day();

                day.Date = date;

                var emplSpecDay = employee.SpecialDays.FirstOrDefault(x => date >= x.Start && date <= x.End);

                if (emplSpecDay != null)
                {
                    day.Hours = emplSpecDay.Hours;
                    day.Description = emplSpecDay.Description;
                }
                else
                {
                    var specDay = _config.SpecialDays.FirstOrDefault(x => date >= x.Start && date <= x.End);
                    if (specDay != null)
                    {
                        day.Hours = specDay.Hours;
                        day.Description = specDay.Description;
                    }
                    else if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                    {
                        day.Hours = 0;
                        day.Description = "Выходной";
                    }
                    else
                    {
                        var customDay = employee.CustomDays.FirstOrDefault(x => x.DayOfWeek == day.Date.DayOfWeek);
                        if (customDay != null)
                        {
                            day.Hours = customDay.Hours;
                            day.Description = "Рабочий день";

                        }
                        else
                        {
                            day.Hours = 8;
                            day.Description = "Рабочий день";
                        }
                    }
                }

                Hours += day.Hours;

                Days.Add(day);

                date = date.AddDays(1);
            }
        }

        public decimal Hours { get; private set; } = 0;

        public List<Day> Days { get; set; } = new List<Day>();

        private SchedualParams _params { get; set; } = new SchedualParams();

        private ConfigBase _config { get; set; }
    }
}
