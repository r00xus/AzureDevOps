using MDS.Azure.DevOps.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MDS.Azure.DevOps.Console
{
    public class Day
    {
        public DateTime Date { get; set; }
        public decimal Hours { get; set; }
        public string Description { get; set; }
    }

    public class Schedual
    {
        public Schedual(string name, DateTime dateFrom, DateTime? dateTo)
        {
            DateTime date = dateFrom;

            DateTime endDate = dateTo ?? DateTime.Now;

            var employee = AppConfig.Instance.Employees.FirstOrDefault(x => x.Name == name);

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
                    var specDay = AppConfig.Instance.SpecialDays.FirstOrDefault(x => date >= x.Start && date <= x.End);
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
    }
}
