using System;
using System.Collections.Generic;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using HyperBlinky.Classes;
using LEDBlinkyControls;
using LEDBlinkyInputMap;
using mame;
using dat = LEDBlinkyControls.dat;

namespace HyperBlinky
{
    internal static class Util
    {
        private static mame.mame _mame;

        internal static LEDBlinkyInputMap.dat LEDBlinkyInputMaps
        {
            get
            {
                if (_ledBlinkyInputMap == null)
                {
                    LoadInputMap();
                }
                return _ledBlinkyInputMap;
            }
            set { _ledBlinkyInputMap = value; }
        }

        private static dat _LEDBlinkyControls;

        private static List<MameControl> mameControls;
        private static List<ColorClass> colorClasses;
        private static LEDBlinkyInputMap.dat _ledBlinkyInputMap;

        public static mame.mame Mame
        {
            get
            {
                if (_mame == null)
                {
                    LoadMame();
                }

                return _mame;
            }
            set { _mame = value; }
        }

        //private static void ReadControlsINI()
        //{
        //    var configItem = ConfigItems.GetConfig();

        //    mameControls = new List<MameControl>();
        //    configItem.ControlsINI = @"C:\HyperSpin\ROMLister_031.2\controls.ini";
        //    if (string.IsNullOrEmpty(configItem.ControlsINI))
        //        configItem.ControlsINI = @"C:\HyperSpin\ROMLister_031.2\controls.ini";

        //    if (!File.Exists(configItem.ControlsINI))
        //        return;

        //    string line;

        //    MameControl mameControl = null;

        //    var file =
        //        new StreamReader(configItem.ControlsINI);
        //    while ((line = file.ReadLine()) != null)
        //    {
        //        if (line.StartsWith(@"["))
        //        {
        //            if (mameControl != null)
        //            {
        //                mameControls.Add(mameControl);
        //            }
        //            mameControl = new MameControl();

        //            var game = line.Remove(0, 1);
        //            game = game.Remove(game.Length - 1);
        //            mameControl.name = game;
        //            continue;
        //        }

        //        if (!line.Contains(@"=") || mameControl == null)
        //            continue;

        //        var split = line.Split('=');

        //        if (split[0].StartsWith("P"))
        //        {
        //            var color = colorClasses.FirstOrDefault(a => a.name == mameControl.name && a.controlName == split[0]);
        //            mameControl.Controls.Add(new control
        //            {
        //                name = split[0],
        //                voice = split[1],
        //                color = color == null ? "white":color.Color
        //            });
        //        }
        //        else
        //        {
        //            switch (split[0])
        //            {
        //                case "gamename":
        //                    mameControl.gamename = split[1];
        //                    break;
        //                case "numPlayers":
        //                    mameControl.numPlayers = split[1];
        //                    break;
        //                case "alternating":
        //                    mameControl.alternating = split[1];
        //                    break;
        //                case "mirrored":
        //                    mameControl.mirrored = split[1];
        //                    break;
        //                case "tilt":
        //                    mameControl.tilt = split[1];
        //                    break;
        //                case "cocktail":
        //                    mameControl.cocktail = split[1];
        //                    break;
        //                case "usesService":
        //                    mameControl.usesService = split[1];
        //                    break;
        //                case "miscDetails":
        //                    mameControl.miscDetails = split[1];
        //                    break;
        //            }
        //        }
        //    }
        //}

        private static void ReadColorsINI()
        {
            var configItem = ConfigItems.GetConfig();

            colorClasses = new List<ColorClass>();

            if (string.IsNullOrEmpty(configItem.ColorsINI))
                configItem.ColorsINI = @"C:\HyperSpin\ROMLister_031.2\colors.ini";

            if (!File.Exists(configItem.ColorsINI))
                return;

            string line;

        
            string game = null;
            var file =
                new StreamReader(configItem.ControlsINI);
            while ((line = file.ReadLine()) != null)
            {
                if (line.StartsWith(@"["))
                {
                    
                    game = line.Remove(0, 1);
                    game = game.Remove(game.Length - 1);
                   
                    continue;
                }

                if (!line.Contains(@"=") || String.IsNullOrEmpty(game))
                    continue;

                var split = line.Split('=');

                if (split[0].StartsWith("P"))
                {                    
                    colorClasses.Add(new ColorClass
                    {
                        name = game,controlName = split[0],Color = split[1]
                    });                    
                }
               
            }
        }

        private static Dictionary<string, string> _mappings;

        public static Dictionary<string, string> Mappings
        {
            get
            {
                if (_mappings == null)
                {
                    ReloadMappings();
                }
                return _mappings;
            }

        }

        private static Dictionary<string, string> _aliases;

        public static Dictionary<string, string> Aliases
        {
            get
            {
                if (_aliases == null)
                {
                    ReloadAliases();
                }
                return _aliases;
            }

        }

        public static void ReloadAliases()
        {
            _aliases = null;
            var configItem = ConfigItems.GetConfig();

            if (!File.Exists(configItem.InputLabelAliases))
                configItem.InputLabelAliases = @"C:\HyperSpin\LEDBlinky\InputLabelAliases.txt";

            if (!File.Exists(configItem.InputLabelAliases))
                return;

            string line;
            _aliases = new Dictionary<string, string>();
            var rfile =
                new StreamReader(configItem.InputLabelAliases);
            while ((line = rfile.ReadLine()) != null)
            {
                if (line.Contains("="))
                {
                    var lines = line.Split('=');
                    _aliases[lines[0]] = lines[1];
                }
            }
        }

        public static void ReloadMappings()
        {
            _mappings = null;
            var configItem = ConfigItems.GetConfig();

            if (!File.Exists(configItem.KeyMappingFile))
                configItem.KeyMappingFile = @"C:\HyperSpin\LEDBlinky\KeyMapping.txt";

            if (!File.Exists(configItem.KeyMappingFile))
                return;

            string line;
            _mappings = new Dictionary<string, string>();
            var rfile =
                new StreamReader(configItem.KeyMappingFile);
            while ((line = rfile.ReadLine()) != null)
            {
                if (line.Contains("="))
                {
                    var lines = line.Split('=');
                    _mappings[lines[0]] = lines[1];
                }
            }
        }

        internal static List<datLedControllerPort> Ports;

        public static void LoadInputMap()
        {

            try
            {
              
                var configItem = ConfigItems.GetConfig();

                if (string.IsNullOrEmpty(configItem.LEDBlinkyInputMap))
                    configItem.LEDBlinkyInputMap = @"C:\HyperSpin\LEDBlinky\LEDBlinky\LEDBlinkyInputMap.xml";


                if (!File.Exists(configItem.LEDBlinkyInputMap))
                    return;

                var ser = new XmlSerializer(typeof(LEDBlinkyInputMap.dat));
                var fs = new FileStream(configItem.LEDBlinkyInputMap, FileMode.Open);
                LEDBlinkyInputMaps = (LEDBlinkyInputMap.dat)ser.Deserialize(fs);
                fs.Close();

                var portd = Util.LEDBlinkyInputMaps.Items.Where(a => a.GetType() == typeof(datLedController)).ToList();
                if (!portd.Any())
                    return;

                Ports = new List<datLedControllerPort>();
                foreach (datLedController p in portd)
                {
                    Ports.AddRange(p.port);
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
       
        }

        public static void LoadControllerMap()
        {
            _LEDBlinkyControls = ReadControllerMap();

         
        }

        public static LEDBlinkyControls.dat ReadControllerMap(string fileName="")
        {
            LEDBlinkyControls.dat ret = new dat();
            try
            {

                var configItem = ConfigItems.GetConfig();

                if (string.IsNullOrEmpty(configItem.LEDBlinkyControls))
                    configItem.LEDBlinkyControls = @"C:\HyperSpin\LEDBlinky\LEDBlinky\LEDBlinkyControls.xml";

                if (String.IsNullOrEmpty(fileName))
                {
                    fileName= configItem.LEDBlinkyControls;
                }               

                if (!File.Exists(fileName))
                    return ret;

                var ser = new XmlSerializer(typeof(LEDBlinkyControls.dat));
                var fs = new FileStream(fileName, FileMode.Open);
                ret = (LEDBlinkyControls.dat)ser.Deserialize(fs);
                fs.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ret;

        }

        public static void LoadMame()
        {
           
            try
            {
                ReadColorsINI();
              //  ReadControlsINI();

                var configItem = ConfigItems.GetConfig();

                if (string.IsNullOrEmpty(configItem.MameXML))
                    configItem.MameXML = @"C:\HyperSpin\ROMLister_031.2\list.xml";


                if (!File.Exists(configItem.MameXML))
                    return;

                var ser = new XmlSerializer(typeof (mame.mame));
                var fs = new FileStream(configItem.MameXML, FileMode.Open);
                _mame = (mame.mame) ser.Deserialize(fs);
                fs.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            var asteroid = _mame.machine.FirstOrDefault(a => a.name == "asteroid");
            var ass = asteroid.ToControlGroup();
        }

        public static controlGroup ToControlGroup(this mameMachine mameGame)
        {
            var service = PluralizationService.CreateService(CultureInfo.CurrentCulture);

            var pl = service.Pluralize(mameGame.name);
            var numplayers = 1;
            int.TryParse(mameGame.input[0].players, out numplayers);

            var buttons = 0;
            int.TryParse(mameGame.input[0].buttons, out buttons);

            var controlGroup = new controlGroup
            {
                groupName = mameGame.name,
                voice = pl,
                numPlayers = mameGame.input[0].players
            };

            var controlGroupPlayers = new List<controlGroupPlayer>();

            for (var pcount = 1; pcount <= numplayers; ++pcount)
            {
                var cg = new controlGroupPlayer {number = pcount.ToString()};

                var controls = new List<control>();
                for (var ccount = 0; ccount < buttons; ++ccount)
                {
                    var cc = new control
                    {
                        name = "P" + pcount + "_BUTTON" + ccount,
                        color = "white"
                    };
                    controls.Add(cc);
                }
                cg.control = controls.ToArray();
                controlGroupPlayers.Add(cg);
            }
            controlGroup.player = controlGroupPlayers.ToArray();

            return controlGroup;
        }
    }
}