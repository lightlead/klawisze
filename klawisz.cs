using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Forms;
using System.Drawing.Text;

namespace Klawisze
{
    internal class klawisz
    {

        private PictureBox klawiszbox;
        private Label znak;

        private Klawisze form;
        private static Image klawiszimg = Image.FromFile("../../../zasoby/klawisz.png");

        private int skala = 4;
        public klawisz(Klawisze form, int tryb)
        {
            this.form = form;
            
            klawiszbox = new PictureBox();
            klawiszbox.Location = new Point(init.rndint(Klawisze.padding, Klawisze.W-Klawisze.padding), init.rndint(Klawisze.padding, Klawisze.H - Klawisze.padding - menu.H));
            klawiszbox.Image = klawiszimg;
            klawiszbox.Size = new Size(klawiszbox.Image.Width / skala, klawiszbox.Image.Height / skala);

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

        public void resetklawisza()
        {
            klawiszbox.Location = new Point(init.rndint(Klawisze.padding, Klawisze.W - Klawisze.padding), init.rndint(Klawisze.padding, Klawisze.H - Klawisze.padding - menu.H));
            znak.Text = init.rndchar();
            znak.Location = znakxy();
            form.Invalidate();
        }

        private Point znakxy()
        {
            int x = klawiszbox.Location.X + (klawiszbox.Size.Width / 2) - TextRenderer.MeasureText(znak.Text, znak.Font).Width;
            int y = klawiszbox.Location.Y + (klawiszbox.Size.Height / 2) - TextRenderer.MeasureText(znak.Text, znak.Font).Height;
            return new Point(x,y);
        }

    }
    
}
