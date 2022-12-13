using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Forms;
using System.Drawing.Text;
using System.Threading;
namespace Klawisze
{
    internal class keys
    {

        private Klawisze main;

        public PictureBox keybox;
        public Label keychar;
        public bool animate = false;
        private bool left = true;
        private bool cycle = false;
        public bool fail = false;


        private Point AnimPoint;
        public Point keyboxlocation;

        private int distance = 10;
        public keys(Klawisze form, int mode)
        {
            this.main = form;
            
            keybox = new PictureBox();
            keybox.Location = new Point(init.rndint(Klawisze.padding, Klawisze.W - Klawisze.padding), init.rndint(Klawisze.padding, Klawisze.H - Klawisze.padding - menu.H));
            keybox.Image = Klawisze.keyimg;
            keybox.Size = new Size(keybox.Image.Width / Klawisze.keyscale, keybox.Image.Height / Klawisze.keyscale);

            AnimPoint = keybox.Location;
            keychar = new Label();
            keychar.ForeColor = Color.White;
            keychar.Text = init.rndchar();
            keychar.Location = charxy();

            form.Paint += new PaintEventHandler(paintklawisz);
            if (mode == 0)
            {
                
            }
            
        }
        
        private void paintklawisz(object sender, PaintEventArgs e)
        {

            e.Graphics.DrawImage(keybox.Image, keybox.Left, keybox.Top, keybox.Width, keybox.Height);

            using (SolidBrush brush = new SolidBrush(keychar.ForeColor))
            {
                e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                e.Graphics.DrawString(keychar.Text, Klawisze.font, brush, keychar.Location);
            }
            
        }
        
        public void newkey(keys[] Game)
        {
            keybox.Location = new Point(init.rndint(Klawisze.padding, Klawisze.W - Klawisze.padding), init.rndint(Klawisze.padding, Klawisze.H - Klawisze.padding - menu.H));
            AnimPoint = keybox.Location;
            keychar.Location = charxy();
        }

        public void animacjaklawisza()
        {
            fail = false;
            keybox.Location = new Point(keybox.Location.X, keybox.Location.Y + Klawisze.animspeed);
            keychar.Location = charxy();
            
            if (keybox.Location.Y >= AnimPoint.Y + distance)
            {
                animate = false;
            }

            main.Invalidate();
        }

        public void animacjabledu()
        {
            if (left)
                keybox.Location = new Point(keybox.Location.X - Klawisze.animspeed * 4, keybox.Location.Y);
            else
                keybox.Location = new Point(keybox.Location.X + Klawisze.animspeed * 4, keybox.Location.Y);
            
            keychar.Location = charxy();
            keychar.ForeColor = Color.FromArgb(255, 0, 255, 229);

            if (keybox.Location.X < AnimPoint.X - distance && !cycle)
            {
                left = false;
            }
            
            else if (keybox.Location.X > AnimPoint.X + distance*2 && !cycle)
            {
                left = true;
                cycle = true;
            }
            
            else if (keybox.Location.X == AnimPoint.X && cycle)
            {
                cycle = false;
                left = true;
                animate = false;
                fail = false;
                keychar.ForeColor = Color.White;
            }

            main.Invalidate();
        }

        private Point charxy()
        {
            int x = keybox.Location.X + (keybox.Size.Width / 2) - TextRenderer.MeasureText(keychar.Text, keychar.Font).Width;
            int y = keybox.Location.Y + (keybox.Size.Height / 2) - TextRenderer.MeasureText(keychar.Text, keychar.Font).Height;
            return new Point(x,y);
        }
       

    }
    
}
