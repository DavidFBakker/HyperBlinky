using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using HyperBlinky.Annotations;

namespace HyperBlinky
{
    public class ConfigItem : INotifyPropertyChanged
    {
        private string _colorsIni;
        private string _controlsIni;
        private string _emulatorName;
        private string _ledBlinkyControls;
        private string _ledBlinkyInputMap;
        private string _ledBlinkyTemplateFile;
        private string _mameXML;
        private string _keyMappingFile;
        private string _inputLabelAliases;

        public string LedBlinkyTemplateFile
        {
            get { return _ledBlinkyTemplateFile; }
            set
            {
                _ledBlinkyTemplateFile = value;
                OnPropertyChanged();
            }
        }

        public string LEDBlinkyInputMap
        {
            get { return _ledBlinkyInputMap; }
            set
            {
                _ledBlinkyInputMap = value;
                OnPropertyChanged();
            }
        }

        public string LEDBlinkyControls
        {
            get { return _ledBlinkyControls; }
            set
            {
                _ledBlinkyControls = value;
                OnPropertyChanged();
            }
        }

        public string ColorsINI
        {
            get { return _colorsIni; }
            set
            {
                _colorsIni = value;
                OnPropertyChanged();
            }
        }

        public string ControlsINI
        {
            get { return _controlsIni; }
            set
            {
                _controlsIni = value;
                OnPropertyChanged();
            }
        }

        public string InputLabelAliases
        {
            get { return _inputLabelAliases; }
            set { _inputLabelAliases = value; OnPropertyChanged(); }
        }

        public string KeyMappingFile
        {
            get { return _keyMappingFile; }
            set { _keyMappingFile = value; OnPropertyChanged(); }
        }

        public string EmulatorName
        {
            get { return _emulatorName; }
            set
            {
                _emulatorName = value;
                OnPropertyChanged();
            }
        }

        public string MameXML
        {
            get { return _mameXML; }
            set
            {
                _mameXML = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public static class ConfigItems
    {
        private const string configname = "configitems.xml";

        private static readonly List<ConfigItem> Configs = new List<ConfigItem>();

        private static bool _isLoading;

        static ConfigItems()
        {
            ReadConfig();
        }

        public static ConfigItem GetConfig(string emulatorName = "GLOBAL")
        {
            var config = Configs.FirstOrDefault(a => a.EmulatorName.Equals(emulatorName));

            if (config != null) return config;

            config = new ConfigItem
            {
                EmulatorName = emulatorName
            };
            config.PropertyChanged += Config_PropertyChanged;

            Configs.Add(config);
            WriteConfig();
            return config;
        }

        private static void Config_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            WriteConfig();
        }

        private static void ReadConfig()
        {
            if (!File.Exists(configname))
                return;

            lock (new object())
            {
                _isLoading = true;
                Configs.Clear();
                var ser = new XmlSerializer(typeof (List<ConfigItem>));
                var fs = new FileStream(configname, FileMode.Open);
                var ret = (List<ConfigItem>) ser.Deserialize(fs);
                fs.Close();

                foreach (var a in ret)
                {
                    a.PropertyChanged += Config_PropertyChanged;
                    Configs.Add(a);
                }
                WriteConfig();
                _isLoading = false;
            }
        }


        private static void WriteConfig()
        {
            if (_isLoading)
                return;

            var ser = new XmlSerializer(typeof (List<ConfigItem>));

            TextWriter writer = new StreamWriter(configname);
            ser.Serialize(writer, Configs);
            writer.Close();
        }
    }
}