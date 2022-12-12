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
    internal class klawisz
    {

        private Klawisze form;

        public PictureBox klawiszbox;
        public Label znak;
        public bool animate = false;
        private bool lewo = true;
        private bool cykl = false;
        public bool blad = false;


        private Point doAnimacji;
        public Point klawiszboxLocation;

        private int distance = 10;
        private int speed = 3;
        private int skala = 4;
        public klawisz(Klawisze form, int tryb)
        {
            this.form = form;
            klawiszbox = new PictureBox();
            klawiszbox.Location = new Point(init.rndint(Klawisze.padding, Klawisze.W - Klawisze.padding), init.rndint(Klawisze.padding, Klawisze.H - Klawisze.padding - menu.H));
            klawiszbox.Image = Klawisze.klawiszimg;
            klawiszbox.Size = new Size(klawiszbox.Image.Width / skala, klawiszbox.Image.Height / skala);

            doAnimacji = klawiszbox.Location;
            znak = new Label();
            znak.ForeColor = Color.White;
            znak.Text = init.rndchar();
            znak.Location = znakxy();

            form.Paint += new PaintEventHandler(paintklawisz);
            if (tryb == 0)
            {
                
            }
            
        }
        
        private void paintklawisz(object sender, PaintEventArgs e)
        {

            e.Graphics.DrawImage(klawiszbox.Image, klawiszbox.Left, klawiszbox.Top, klawiszbox.Width, klawiszbox.Height);

            using (SolidBrush pedzel = new SolidBrush(znak.ForeColor))
            {
                e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                e.Graphics.DrawString(znak.Text, Klawisze.font, pedzel, znak.Location);
            }
            
        }
        
        public void nowyklawisz(klawisz[] Game)
        {
            klawiszbox.Location = new Point(init.rndint(Klawisze.padding, Klawisze.W - Klawisze.padding), init.rndint(Klawisze.padding, Klawisze.H - Klawisze.padding - menu.H));
            doAnimacji = klawiszbox.Location;
            znak.Location = znakxy();
        }

        public void animacjaklawisza()
        {
            blad = false;
            klawiszbox.Location = new Point(klawiszbox.Location.X, klawiszbox.Location.Y + speed);
            znak.Location = znakxy();
            
            if (klawiszbox.Location.Y >= doAnimacji.Y + distance)
            {
                animate = false;
            }

            form.Invalidate();
        }

        public void animacjabledu()
        {
            if (lewo)
                klawiszbox.Location = new Point(klawiszbox.Location.X - speed*4, klawiszbox.Location.Y);
            else
                klawiszbox.Location = new Point(klawiszbox.Location.X + speed*4, klawiszbox.Location.Y);
            
            znak.Location = znakxy();
            znak.ForeColor = Color.FromArgb(255, 0, 255, 229);

            if (klawiszbox.Location.X < doAnimacji.X - distance && !cykl)
            {
                lewo = false;
            }
            
            else if (klawiszbox.Location.X > doAnimacji.X + distance*2 && !cykl)
            {
                lewo = true;
                cykl = true;
            }
            
            else if (klawiszbox.Location.X == doAnimacji.X && cykl)
            {
                cykl = false;
                lewo = true;
                animate = false;
                blad = false;
                znak.ForeColor = Color.White;
            }

            form.Invalidate();
        }

        private void wait(int ms)
        {
            var t = new System.Windows.Forms.Timer();
            if (ms == 0 || ms < 0)
                return;
            t.Interval = ms;
            t.Enabled = true;
            t.Start();
            t.Tick += (s, e) =>
            {
                t.Enabled = false;
                t.Stop();
            };

            while (t.Enabled)
            {
                Application.DoEvents();
            }

        }

        private Point znakxy()
        {
            int x = klawiszbox.Location.X + (klawiszbox.Size.Width / 2) - TextRenderer.MeasureText(znak.Text, znak.Font).Width;
            int y = klawiszbox.Location.Y + (klawiszbox.Size.Height / 2) - TextRenderer.MeasureText(znak.Text, znak.Font).Height;
            return new Point(x,y);
        }
       

    }
    
}
