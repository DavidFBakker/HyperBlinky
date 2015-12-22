using System.IO;
using System.Linq;

namespace HyperBlinky
{
    internal static class Extensions
    {
        public static string ToInputCode(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            if (str == "P1B7")
            {
                var saaaaaa = 1;
            }


            if (Util.Aliases != null && Util.Ports.FirstOrDefault(a => a.label == str) == null)
            {
                if (Util.Aliases.ContainsKey(str))
                {
                    str = Util.Aliases[str];
                }
                else if (Util.Aliases.ContainsValue(str))
                {
                    str = Util.Aliases.First(a => a.Value == str).Key;
                }
            }


            if (str.Contains("JOYSTICK"))
            {
                var joy = "JOYSTICK" + str.Substring(1, str.IndexOf("_")-1);
                var dir = str.Substring(str.LastIndexOf("_")+1);

                var control = Util.Ports.FirstOrDefault(a => a.label == joy);
                if (control != null && control.inputCodes != null && control.inputCodes.Contains("|"))
                {
                    var dirs = control.inputCodes.Split('|');
                    switch (dir)
                    {
                        case "UP":
                            return dirs[0];
                        case "DOWN":
                            return dirs[1];
                        case "LEFT":
                            return dirs[2];
                        case "RIGHT":
                            return dirs[3];

                    }
                }

            }
            var code = Util.Ports.FirstOrDefault(a => a.label == str);
            if (code == null)
                return "";
            return code.inputCodes;
        }


        public static string ToBlinkyInputCode(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            var ret = "";
            if (str.Contains("JOYSTICK"))
            {
                ret = str;
            }
            else if (str.StartsWith("P") && str.Contains("_"))
            {
                if (str.Contains("BUTTON"))
                {
                    ret = str.Replace("_BUTTON", "B");
                }

                if (str.Contains("START"))
                {
                    ret = str.Replace("_", "");
                }


                if (str.EndsWith("EXIT"))
                {
                    ret = "EXIT";
                }
            }
            else if (str.StartsWith("COIN"))
            {
                ret = "P" + str.Replace("COIN", "") + "COIN";
            }
            else if (str.StartsWith("START"))
            {
                ret = "P" + str.Replace("START", "") + "START";
            }
            else if (str.StartsWith("START"))
            {
                ret = "P" + str.Replace("START", "") + "START";
            }
            else
            {
                if (Util.Mappings != null && Util.Mappings.ContainsKey(str))
                {
                    ret = Util.Mappings[str];
                }
            }


            return ret;
        }
    }
}