using System;
using System.Collections.Generic;
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
        public static string rndchar()
        {
            int losowa;
            Random rnd = new Random();
            losowa = rnd.Next(33, 126); //ascii
            char znak = Convert.ToChar(losowa);
            string znakstring = Convert.ToString(znak);
            znakstring = znakstring.ToUpper();
            return znakstring;
        }


    }
}
