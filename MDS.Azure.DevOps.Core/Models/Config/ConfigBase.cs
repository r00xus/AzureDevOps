using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace MDS.Azure.DevOps.Core.Models.Config
{
    public class ConfigBase
    {
        public List<Employee> Employees { get; set; } = new List<Employee>();

        public List<SpecialDay> SpecialDays { get; set; } = new List<SpecialDay>();

        protected virtual string FilePath { get; }

        private const string FileName = "config.json";

        public void Save()
        {
            if (!Directory.Exists(FilePath))
                Directory.CreateDirectory(FilePath);

            var fileName = FilePath + "\\" + FileName;

            Save(fileName);
        }

        public void Save(string fileName)
        {
            var json = JsonConvert.SerializeObject(this, Formatting.Indented);

            File.WriteAllText(fileName, json);
        }

        public ConfigBase Load()
        {
            var fileName = FilePath + "\\" + FileName;

            return Load(fileName);
        }

        public ConfigBase Load(string fileName)
        {
            if (!File.Exists(fileName))
                throw new Exception($"Файл конфигурации {fileName} не найден. Его нужно создать");

            var json = File.ReadAllText(fileName);

            return (ConfigBase)JsonConvert.DeserializeObject(json, typeof(ConfigBase));
        }
    }
}
