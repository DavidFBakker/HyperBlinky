using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HyperBlinky.Classes
{
    public class MameControl
    {

        public MameControl()
        {
        //    Controls=new List<control>();
        }

        public string name { get; set; }
        public string gamename { get; set; }
        public string numPlayers { get; set; }
        public string alternating { get; set; }
        public string mirrored { get; set; }
        public string tilt { get; set; }
        public string cocktail { get; set; }
        public string usesService { get; set; }
        public string miscDetails { get; set; }
      //  public List<control> Controls { get; set; }
    }

    public class ColorClass
    {
        public string name { get; set; }
        public string controlName { get; set; }
        public string Color { get; set; }

    }
}
