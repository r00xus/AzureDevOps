using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MDS.Azure.DevOps.Core
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

    public class SpecialDay
    {
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public decimal Hours { get; set; }
    }

    public class CustomDay
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public DayOfWeek DayOfWeek { get; set; }
        public decimal Hours { get; set; }
    }

    public class AppConfig
    {
        public List<Employee> Employees { get; set; } = new List<Employee>();

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public List<SpecialDay> SpecialDays { get; set; } = new List<SpecialDay>();

        private static AppConfig _instance { get; set; }

        private static string Path
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\MDS.AzureDevOps";
            }
        }

        private const string FileName = "config.json";

        public void Save()
        {
            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);

            var fileName = Path + "\\" + FileName;

            Save(fileName);
        }

        public void Save(string fileName)
        {
            var json = JsonConvert.SerializeObject(this, Formatting.Indented);

            File.WriteAllText(fileName, json);
        }

        public static AppConfig Load()
        {
            var fileName = Path + "\\" + FileName;

            return Load(fileName);
        }

        public static AppConfig Load(string fileName)
        {
            if (!File.Exists(fileName))
                throw new Exception($"Файл конфигурации {fileName} не найден. Его нужно создать");

            var json = File.ReadAllText(fileName);

            return (AppConfig)JsonConvert.DeserializeObject(json, typeof(AppConfig));
        }

        public static AppConfig Instance
        {
            get
            {
                if (_instance == null) _instance = Load();
                return _instance;
            }
        }
    }
}
