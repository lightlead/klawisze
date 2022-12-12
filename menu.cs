using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klawisze
{
    internal class menu
    {

        public init init;
        public int wcisniecia;
        private static Image menuimg = Image.FromFile("../../../zasoby/tlomenu.png");
        public PictureBox tlomenu;
        private Label czas, knm, napis;
        private Klawisze form;
        Stopwatch s;
        static public int H = menuimg.Height;
        public menu(Klawisze form)
        {
            this.form = form;
            
            s = new Stopwatch();
            s.Start();
            TimeSpan ts = s.Elapsed;
            string et = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            czas = new Label();
            czas.Text = et;
            czas.Location = new Point(20, 700);
            
            knm = new Label();
            knm.Text = "xd";
            knm.Location = new Point(20, 650);


            tlomenu = new PictureBox();
            tlomenu.Size = new Size(Klawisze.W, menuimg.Height);
            tlomenu.Location = new Point(0, Klawisze.H - menuimg.Height);
            tlomenu.Image = menuimg;
            form.Paint += new PaintEventHandler(paintmenu);
        }
        
        public void update()
        {
            TimeSpan ts = s.Elapsed;
            string et = String.Format("CZAS: {1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            czas.Text = et;

            if (wcisniecia == 0)
            {
                knm.Text = "KNM: 0";
            }
            else
            {
                string knms = String.Format("KNM: {0:00}", wcisniecia / ts.TotalMinutes);
                knm.Text = knms;
            }  

            form.Invalidate();
        }

     
        private void paintmenu(object sender, PaintEventArgs e)
        {
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

            //Rectangle gradient = new Rectangle(0, Klawisze.H - form.menuBox.Height, form.menuBox.Width, form.menuBox.Height);
            e.Graphics.DrawImage(tlomenu.Image, tlomenu.Left, tlomenu.Top, tlomenu.Width, tlomenu.Height);

            Rectangle gradient = new Rectangle(tlomenu.Left, tlomenu.Top, tlomenu.Width, tlomenu.Height);
            LinearGradientBrush pedzel = new LinearGradientBrush(
                new Point(0, 0),
                new Point(0, tlomenu.Top),
                Color.FromArgb(0, 255, 255, 255),
                Color.FromArgb(255, 0, 255, 229)
            );

           e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
           e.Graphics.FillRectangle(pedzel, gradient);

            SolidBrush pedzel2 = new SolidBrush(Color.Black);
            if (czas.Visible == true)
            {
                e.Graphics.DrawString(czas.Text, Klawisze.font, pedzel2, czas.Location);
                e.Graphics.DrawString(knm.Text, Klawisze.font, pedzel2, knm.Location);


            }

        }



    }
    
}
