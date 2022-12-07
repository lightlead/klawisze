using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klawisze
{
    internal class menu
    {

        public init init;

        public static int H = 300;
        private PictureBox tlomenu;
        private Klawisze form;
        public menu(Klawisze form)
        {
            this.form = form;
            tlomenu = new PictureBox();
            tlomenu.Size = new Size(Klawisze.W, H);
            tlomenu.Location = new Point(0, Klawisze.H - H);
            tlomenu.BackColor = Color.Yellow;

            form.Paint += new PaintEventHandler(paintmenu);
        }
        
        private void paintmenu(object sender, PaintEventArgs e)
        {
            //e.Graphics.DrawImage(klawisz)
        }



    }
    
}
