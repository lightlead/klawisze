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
        Stopwatch s;

        public int clicks;
        private static Image menuimg = Image.FromFile("../../../zasoby/tlomenu.png");
        public PictureBox menubackground;
        private Label time, knm, points;
        private Klawisze form;
        static public int H = menuimg.Height;
        public menu(Klawisze form)
        {
            this.form = form;
            
            s = new Stopwatch();
            s.Start();
            TimeSpan ts = s.Elapsed;
            string et = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            time = new Label();
            time.Text = et;
            time.Location = new Point(20, 680);
            
            knm = new Label();
            knm.Text = "xd";
            knm.Location = new Point(20, time.Location.Y - time.Font.Height * 4);

            points = new Label();
            points.Text = "PUNKTY :: 4020";
            points.Location = new Point(20, time.Location.Y - time.Font.Height * 2);

            menubackground = new PictureBox();
            menubackground.Size = new Size(Klawisze.W, menuimg.Height);
            menubackground.Location = new Point(0, Klawisze.H - menuimg.Height);
            menubackground.Image = menuimg;
            form.Paint += new PaintEventHandler(paintmenu);
        }
        
        public void update()
        {
            TimeSpan ts = s.Elapsed;
            string et = String.Format("CZAS :: {1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            time.Text = et;


            string knms = String.Format("KNM :: {0:00}", clicks / ts.TotalMinutes);
            knm.Text = knms;
 

            form.Invalidate();
        }

     
        private void paintmenu(object sender, PaintEventArgs e)
        {
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

            e.Graphics.DrawImage(menubackground.Image, menubackground.Left, menubackground.Top, menubackground.Width, menubackground.Height);

            Rectangle gradient = new Rectangle(menubackground.Left, menubackground.Top, menubackground.Width, menubackground.Height);
            LinearGradientBrush brush = new LinearGradientBrush(
                new Point(0, 0),
                new Point(0, menubackground.Top),
                Color.FromArgb(0, 255, 255, 255),
                Color.FromArgb(255, 0, 255, 229)
            );

           e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
           e.Graphics.FillRectangle(brush, gradient);

            SolidBrush brush2 = new SolidBrush(Color.Black);
            if (time.Visible == true)
            {
                e.Graphics.DrawString(time.Text, Klawisze.font, brush2, time.Location);
                e.Graphics.DrawString(knm.Text, Klawisze.font, brush2, knm.Location);
                e.Graphics.DrawString(points.Text, Klawisze.font, brush2, points.Location);
            }

        }



    }
    
}
