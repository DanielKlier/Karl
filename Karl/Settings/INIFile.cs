using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Karl.Settings
{
    class IniFile
    {
        private struct Taxonomy
        {
            public string Section;
            public string Key;
        }

        private static readonly char[] TaxnonomySeparator = { '.' };

        private readonly Dictionary<string, Setting> _settings;


        public IniFile(Dictionary<string, Setting> settings)
        {
            _settings = settings;
        }

        public void Save(Stream stream)
        {
            var settingsBySection = new Dictionary<string, List<Setting>>();
            var writer = new StreamWriter(stream);

            foreach (var setting in _settings.Values)
            {
                var taxonomy = GetTaxonomy(setting.Name);
                
                if(settingsBySection.ContainsKey(taxonomy.Section) == false)
                    settingsBySection[taxonomy.Section] = new List<Setting>();

                settingsBySection[taxonomy.Section].Add(setting);
            }

            foreach (var sectionNameSettingsPair in settingsBySection)
            {
                writer.WriteLine(GetSectionString(sectionNameSettingsPair.Key));

                foreach (var setting in sectionNameSettingsPair.Value)
                {
                    if(setting.Value != null)
                        writer.WriteLine(GetSettingString(setting));
                }
            }

            writer.Close();
        }

        private string GetSettingString(Setting setting)
        {
            var taxonomy = GetTaxonomy(setting.Name);
            return taxonomy.Key + " = " + setting.SaveFormat(setting.Value);
        }

        private string GetSectionString(string section)
        {
            return "[" + section + "]";
        }

        public void Load(Stream stream)
        {
            var iniParser = new IniParser();

            var result = new Dictionary<string, string>();
            iniParser.Parse(stream, (section, key, value) => result[section + TaxnonomySeparator[0] + key] = value);

            foreach (var kvp in result)
            {
                if (_settings.ContainsKey(kvp.Key))
                {
                    var setting = _settings[kvp.Key];
                    setting.Value = setting.Validate(kvp.Value);
                    _settings[kvp.Key] = setting;
                }
            }
        }

        private static Taxonomy GetTaxonomy(string name)
        {
            var t = name.Split(TaxnonomySeparator, 2);

            Debug.Assert(t.Length == 2, "Taxonomy must include at least one dot character [.]");

            return new Taxonomy()
            {
                Section = t[0],
                Key = t[1]
            };
        }
    }
}