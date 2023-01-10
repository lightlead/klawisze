using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Forms;
using System.Drawing.Text;
using System.Threading;
using System.Drawing;

namespace Klawisze
{
    internal class keys
    {

        private Klawisze main;
        private static Font keyfont = new Font("Stretch Pro V2", 20);

        public PictureBox keybox;
        public Label keychar;
        public bool animate = false;
        public bool render = true;
        private bool left = true;
        private bool cycle = false;
        public bool fail = false;


        private Point AnimPoint;
        private int vertfall = 10;
        private int horizmove = 5;

        private int keycolorHZ = 20;
        public SolidBrush keybrush;

        public keys(Klawisze form)
        {
            this.main = form;
            keybrush = new SolidBrush(Color.White);
            keybox = new PictureBox();
            keybox.Location = new Point(init.rndint(Klawisze.padding, Klawisze.W - Klawisze.padding), init.rndint(Klawisze.padding, Klawisze.H - Klawisze.padding - menu.H));
            AnimPoint = keybox.Location;
            keybox.Image = Klawisze.keyimg;
            keybox.Size = new Size(keybox.Image.Width / Klawisze.keyscale, keybox.Image.Height / Klawisze.keyscale);
            keychar = new Label();
            keychar.ForeColor = Color.White;
            keychar.Text = init.rndchar();
            keychar.Location = charxy();

            form.Paint += new PaintEventHandler(paintkey);
        }
        
        public void rainbow()
        {
            int alpha = keybrush.Color.A;
            int red = keybrush.Color.R;
            int green = keybrush.Color.G;
            int blue = keybrush.Color.B;

            if (red == 255 && green > 255 && green == blue )
            {
                green -= keycolorHZ;
                blue -= keycolorHZ;
            }

            // 255, 0->255, 0
            if (red >= 255 && green < 255 && blue == 0)
                green += keycolorHZ;
            // 255->0, 255, 0
            else if (red > 0 && green >= 255)
                red -= keycolorHZ;
            // 0, 255, 0->255
            else if (green >= 255 && blue < 255)
                blue += keycolorHZ;
            // 0, 255->0, 255
            else if (blue >= 255 && green > 0)
                green -= keycolorHZ;
            // 0->255, 0, 255
            else if (blue >= 255 && red < 255)
                red += keycolorHZ;
            // 255, 0, 255->0
            else if (red >= 255 && blue > 0)
                blue -= keycolorHZ;

            if (red > 255)
                red = 255;
            if (green > 255)
                green = 255;
            if (blue > 255)
                blue = 255;
            if (alpha > 255)
                alpha = 255;

            if (red < 0)
                red = 0;
            if (green < 0)
                green = 0;
            if (blue < 0)
                blue = 0;
            if (alpha < 0)
                alpha = 0;

            keybrush = new SolidBrush(Color.FromArgb(alpha, red, green, blue));
        }

        private void paintkey(object sender, PaintEventArgs e)
        {

            if (render == true)
            {
                e.Graphics.DrawImage(keybox.Image, keybox.Left, keybox.Top, keybox.Width, keybox.Height);

                e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                    e.Graphics.DrawString(keychar.Text, keyfont, keybrush, keychar.Location);
 
            }
            
        }
        
        public void newkey()
        {
            keybox.Location = new Point(init.rndint(Klawisze.padding, Klawisze.W - Klawisze.padding), init.rndint(Klawisze.padding, Klawisze.H - Klawisze.padding - menu.H));
            AnimPoint = keybox.Location;
            keychar.Location = charxy();
        }

        public void keyrst(keys[] key, int i)
        {
            // losuje znak dopóki nie znajdzie takiego co nie ma na ekranie
            int cnt = key.Length - 1;
            int rst = cnt;
            keychar.Text = init.rndchar();
            while (cnt != -1)
            {
                if (cnt != i)
                {
                    if (keychar.Text == key[cnt].keychar.Text)
                    {
                        keychar.Text = init.rndchar();
                        cnt = rst;
                    }
                    else
                    {
                        cnt--;
                    }
                }
                else cnt--;
            }

            //losuje pozycje
            newkey();
            cnt = rst;
            while (cnt != -1)
            {
                if (cnt != i)
                {
                    if (key[i].keybox.Bounds.IntersectsWith(key[cnt].keybox.Bounds))
                    {
                        newkey();
                        cnt = rst;
                    }
                    else
                    {
                        cnt--;
                    }
                }
                else cnt--;
            }


            animate = true;
            fail = false;
        }

        public void akeyspawn()
        {
            fail = false;
            keybox.Location = new Point(keybox.Location.X, keybox.Location.Y + Klawisze.animspeed);
            keychar.Location = charxy();
            
            if (keybox.Location.Y >= AnimPoint.Y + vertfall)
            {
                animate = false;
            }
        }

        public void aerror()
        {
            if (left)
                keybox.Location = new Point(keybox.Location.X - Klawisze.animspeed * 4, keybox.Location.Y);
            else
                keybox.Location = new Point(keybox.Location.X + Klawisze.animspeed * 4, keybox.Location.Y);
            
            keychar.Location = charxy();
            keybrush = new SolidBrush(Color.FromArgb(255, 0, 255, 229));

            if (keybox.Location.X < AnimPoint.X - horizmove && !cycle)
            {
                left = false;
            }
            
            else if (keybox.Location.X > AnimPoint.X + horizmove * 2 && !cycle)
            {
                left = true;
                cycle = true;
            }
            
            else if (keybox.Location.X == AnimPoint.X && cycle || keybox.Location.X > AnimPoint.X + horizmove * 2 || keybox.Location.X < AnimPoint.X - horizmove)
            {
                cycle = false;
                left = true;
                animate = false;
                fail = false;
                keybrush = new SolidBrush(Color.White);
            }
        }

        private Point charxy()
        {
            int x = keybox.Location.X + (keybox.Size.Width / 2) - TextRenderer.MeasureText(keychar.Text, keychar.Font).Width;
            int y = keybox.Location.Y + (keybox.Size.Height / 2) - TextRenderer.MeasureText(keychar.Text, keychar.Font).Height;
            return new Point(x,y);
        }
       

    }
    
}
