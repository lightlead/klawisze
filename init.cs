using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Klawisze
{
    internal class init
    {
        public init()
        {
        }

        public static int rndint(int min, int max)
        {
            int losowa;
            Random rnd = new Random();
            losowa = rnd.Next(min, max);
            return losowa;
        }
        public static string rndchar(int mode)
        {
            int losowa = 0;
            Random rnd = new Random();
            if (mode == 1)
            {
                losowa = rnd.Next(33, 126); //ascii
            }
            else if (mode == 3)
            {
                losowa = rnd.Next(65, 90);
            }
            char znak = Convert.ToChar(losowa);
            string znakstring = Convert.ToString(znak);
            znakstring = znakstring.ToUpper();
            return znakstring;
        }


    }
}
