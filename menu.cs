using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Klawisze
{
    internal class menu
    {
        
        public init init;
        Stopwatch s;

        private Font combotxtfont = new Font("Stretch Pro V2", 20);
        private Font combovalfont = new Font("Stretch Pro V2", 40);
        private Font bottomfont = new Font("Stretch Pro V2", 20);

        public int clicks;
        public bool game = true;
        private static int statoffset = 110;
        private static int padding = 25;
        private static Image menuimg = Image.FromFile("../../../zasoby/tlomenu.png");
        public PictureBox menubackground;
        private Label time, knm, points, combotxt, comboval;
        private Klawisze form;
        static public int H = menuimg.Height;

        private PictureBox bg;
        public static Image backgroundimg = Image.FromFile("../../../zasoby/tlo.png");

        public SolidBrush bgbrush;
        private Rectangle bgcolor;
        private int bgcolorHZ = 4;
        public menu(Klawisze form)
        {
            this.form = form;

            //gorna czesc
            combotxt = new Label();
            comboval = new Label();
            combotxt.Text = "COMBO";
            comboval.Text = "2X!";
            comboval.Location = new Point(combotxt.Location.X + TextRenderer.MeasureText(e.Graphics, combotxt.Text, combotxtfont).Width, combotxt.Location.Y - 40);
            combotxt.Location = new Point(padding, padding - combotxtfont.Height / 4);

            // bg
            bg = new PictureBox();
            bg.Width = Klawisze.W;
            bg.Height = Klawisze.H;
            bg.Image = backgroundimg;
            bg.Location = new Point(0, 0);

            // operacje tła
            bgcolor = new Rectangle();
            bgcolor.Width = form.Width;
            bgcolor.Height = form.Height;
            bgcolor.Location = new Point(0, 0);
            bgbrush = new SolidBrush(Color.FromArgb(0, 0, 0, 0));

            // ==================================================================================================== << DOLNY PANEL
            // tło dolnego panelu
            menubackground = new PictureBox();
            menubackground.Size = new Size(Klawisze.W, menuimg.Height);
            menubackground.Location = new Point(0, Klawisze.H - menuimg.Height);
            menubackground.Image = menuimg;

            s = new Stopwatch();
            s.Start();
            TimeSpan ts = s.Elapsed;
            string et = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            time = new Label();
            time.Text = et;

            knm = new Label();
            knm.Text = "xd";

            // punkty
            points = new Label();
            points.Text = "PUNKTY :: 4020";
            txtlocationupdate();

            form.Paint += new PaintEventHandler(paintmenu);
        }
        
        public void statupdate()
        {
            TimeSpan ts = s.Elapsed;
            string et = String.Format("CZAS :: {1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            time.Text = et;

            string knms = String.Format("KNM :: {0:00}", clicks / ts.TotalMinutes);
            knm.Text = knms;
        }

        private void txtlocationupdate()
        {
            time.Location = new Point(20, menubackground.Location.Y + statoffset);
            knm.Location = new Point(20, time.Location.Y - time.Font.Height * 4);
            points.Location = new Point(20, time.Location.Y - time.Font.Height * 2);
        }

        public void aMenu()
        {
            menubackground.Location = new Point(menubackground.Location.X, menubackground.Location.Y - 1);
            txtlocationupdate();
        }

        public void colorbg(string func)
        {
            int alpha = bgbrush.Color.A;
            int red = bgbrush.Color.R;
            int green = bgbrush.Color.G;
            int blue = bgbrush.Color.B;

            if (func == "darken")
            {
                if (alpha < 150)
                    alpha += bgcolorHZ;
            }

            if (func == "lighten")
            {
                if (alpha != 0)
                    alpha -= bgcolorHZ;
            }


            bgbrush = new SolidBrush(Color.FromArgb(alpha, red, green, blue));
        }

        private void paintmenu(object sender, PaintEventArgs e)
        {
            
            ////////// ustawienia renderu
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            SolidBrush black = new SolidBrush(Color.Black);
            LinearGradientBrush bluegrad = new LinearGradientBrush(
                new Point(0, 0),
                new Point(0, menubackground.Top),
                Color.FromArgb(0, 255, 255, 255),
                Color.FromArgb(255, 0, 255, 229)
            );

            ////////////// tlo
            e.Graphics.DrawImage(bg.Image, bg.Left, bg.Top, bg.Width, bg.Height);

            // przyciemnianie tła
            if (game == true)
            {
                e.Graphics.FillRectangle(bgbrush, bgcolor);
            }

            //////////////// gorna czesc panelu
            e.Graphics.DrawString(combotxt.Text, combotxtfont, black, combotxt.Location);
            e.Graphics.DrawString(comboval.Text, combovalfont, black, comboval.Location);


            // ==================================================================================================== << DOLNY PANEL
            e.Graphics.DrawImage(menubackground.Image, menubackground.Left, menubackground.Top, menubackground.Width, menubackground.Height);
            Rectangle gradient = new Rectangle(menubackground.Left, menubackground.Top, menubackground.Width, menubackground.Height);
            e.Graphics.FillRectangle(bluegrad, gradient);

            if (time.Visible == true)
            {
                e.Graphics.DrawString(time.Text, bottomfont, black, time.Location);
                e.Graphics.DrawString(knm.Text, bottomfont, black, knm.Location);
                e.Graphics.DrawString(points.Text, bottomfont, black, points.Location);
            }

        }



    }
    
}
