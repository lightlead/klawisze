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

        public int i = 0;
        public PictureBox klawiszbox;
        private Label znak;
        private static System.Threading.Timer t;
        private Klawisze form;
        private static Image klawiszimg = Image.FromFile("../../../zasoby/klawisz.png");
        private Point doAnimacji;
        public Point klawiszboxLocation;
        private int skala = 4;
        public klawisz(Klawisze form, int tryb)
        {
            this.form = form;
            klawiszbox = new PictureBox();
            klawiszboxLocation = new Point(init.rndint(Klawisze.padding, Klawisze.W - Klawisze.padding), init.rndint(Klawisze.padding, Klawisze.H - Klawisze.padding - menu.H));
            klawiszbox.Location = klawiszboxLocation;
            klawiszbox.Image = klawiszimg;
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

        public void nowyklawisz()
        {
            klawiszbox.Location = new Point(init.rndint(Klawisze.padding, Klawisze.W - Klawisze.padding), init.rndint(Klawisze.padding, Klawisze.H - Klawisze.padding - menu.H));
            znak.Text = init.rndchar();
            znak.Location = znakxy();
        }

        public int animacjaklawisza()
        {
            i++;
            klawiszbox.Location = new Point(doAnimacji.X, doAnimacji.Y + i);
            znak.Location = new Point(doAnimacji.X, doAnimacji.Y + i);
            return i;
        }

        private Point znakxy()
        {
            int x = klawiszbox.Location.X + (klawiszbox.Size.Width / 2) - TextRenderer.MeasureText(znak.Text, znak.Font).Width;
            int y = klawiszbox.Location.Y + (klawiszbox.Size.Height / 2) - TextRenderer.MeasureText(znak.Text, znak.Font).Height;
            return new Point(x,y);
        }
       

    }
    
}
