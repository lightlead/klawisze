using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klawisze
{

    internal class words
    {

        private Klawisze form;
        private static Font wordfont = new Font("Stretch Pro V2", 20);
        public Label container;
        public bool animate = false;
        public bool render = false;
        private int vertfall = 10;
        public SolidBrush wordbrush;

        public words(Klawisze form)
        {
            this.form = form;
            container = new Label();
            container.Text = "randomtext";
            wordbrush = new SolidBrush(Color.White);
            container.Location = new Point(init.rndint(Klawisze.padding, Klawisze.W - Klawisze.padding), init.rndint(Klawisze.padding, Klawisze.H - Klawisze.padding - gameui.H));

            form.Paint += new PaintEventHandler(paintword);
        }

        private void paintword(object sender, PaintEventArgs e)
        {

            if (render == true)
            {
                e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                e.Graphics.DrawString(container.Text, wordfont, wordbrush, container.Location);
            }

        }

    }
}
