using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using EasyStorage;
using Karl.Storage;

namespace Karl.Settings
{
    public abstract class GameSettings
    {

        private const string FileExtension = ".ini";

        private readonly Dictionary<string, Setting> _settings = new Dictionary<string, Setting>();

        protected abstract string SettingsName { get; }

        private string FilePath
        {
            get
            {
                var filePath = SettingsName.ToLower() + FileExtension;
                return filePath;
            }
        }

        public void Register(string name, object defaultValue, ValidatorDelegate validator,
            SaveFormatDelegate saveFormat = null)
        {
            var setting = new Setting
            {
                Name = name,
                Validate = validator,
                SaveFormat = saveFormat ?? SaveFormats.SaveFormatStd,
                Default = defaultValue
            };

            _settings.Add(name, setting);
        }



        public void Persist(IStorageProvider storageProvider)
        {
            storageProvider.RequestSharedDevice()
                .Ready(saveDevice => saveDevice.Save("settings", FilePath, stream => new IniFile(_settings).Save(stream)));
        }

        public void Load(IStorageProvider storageProvider, Action callback)
        {
            storageProvider.RequestSharedDevice()
                .Ready(saveDevice => saveDevice.Load("settings", FilePath, stream =>
                {
                    new IniFile(_settings).Load(stream);
                    if( callback != null) callback();
                }));
        }

        public string GetString(string key)
        {
            return Get(key).AsString();
        }

        public int GetInt(string key)
        {
            return Get(key).AsInt();
        }

        public float GetFloat(string key)
        {
            return Get(key).AsFloat();
        }

        public bool GetBool(string key)
        {
            return Get(key).AsBool();
        }

        private Setting Get(string key)
        {
            return _settings[key];
        }
    }
}
