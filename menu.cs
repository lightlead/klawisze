using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Klawisze
{
    internal class menu
    {
        
        public init init;
        private Font combotxtfont = new Font("Stretch Pro V2", 20);
        private Font combovalfont = new Font("Stretch Pro V2", 40);
        private Font bottomfont = new Font("Stretch Pro V2", 20);

        public bool game = false;
        private static int statoffset = 110;
        private static int padding = 25;
        private static Image menuimg = Image.FromFile("../../../zasoby/tlomenu.png");
        public PictureBox menubackground;
        public Label menubtn, time, knm, points, combotxt, comboval;
        public Label exit;
        private Klawisze form;
        static public int H = menuimg.Height;
        public double knmVal;

        private LinearGradientBrush menugrad;
        private PictureBox bg;
        public static Image backgroundimg = Image.FromFile("../../../zasoby/tlo.png");

        //anim

        public SolidBrush bgbrush, combobrush;
        private Rectangle bgcolor;
        private int bgcolorHZ = 10;
        public menu(Klawisze form)
        {
            this.form = form;

            //gorna czesc
            combotxt = new Label();
            combotxt.Visible = false;
            comboval = new Label();
            combotxt.Text = "COMBO";
            comboval.Text = "2X!";
            combobrush = new SolidBrush(Color.FromArgb(255, 0, 0, 0));

            exit = new Label();
            exit.Text = "x";

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

            /////////////////////////
            ///

            // ==================================================================================================== << DOLNY PANEL
            // tło dolnego panelu
            menubackground = new PictureBox();
            menubackground.Size = new Size(Klawisze.W, menuimg.Height);
            menubackground.Location = new Point(0, Klawisze.H - menuimg.Height);
            menubackground.Image = menuimg;

            menubtn = new Label();
            menubtn.Text = "MENU";
            menubtn.Location = new Point(Klawisze.W - 300, Klawisze.H - 120);

            time = new Label();
            knm = new Label();
            knm.Text = "xd";

            // punkty
            points = new Label();
            points.Text = "PUNKTY :: 00";
            txtlocationupdate();

            menugrad = new LinearGradientBrush(
                new Point(0, 0),
                new Point(0, menubackground.Top),
                Color.FromArgb(0, 255, 255, 255),
                Color.FromArgb(255, 0, 255, 229)
            );

            form.Paint += new PaintEventHandler(paintmenu);
        }
        
        public void statupdate(Stopwatch s, int pointsValue, int clicks)
        {
            TimeSpan ts = s.Elapsed;
            string et = String.Format("CZAS :: {1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            time.Text = et;

            knmVal = clicks / ts.TotalMinutes;
            string knms = String.Format("KNM :: {0:00}", knmVal);
            knm.Text = knms;

            points.Text = "PUNKTY :: " + pointsValue.ToString();
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

        public void colorbg(string func, int i)
        {
            int alpha = bgbrush.Color.A;
            int red = bgbrush.Color.R;
            int green = bgbrush.Color.G;
            int blue = bgbrush.Color.B;
            
            if (func == "darken")
            {
                red = 0;
                green = 0;
                blue = 0;

                if (alpha < 100 && i == 1)
                    alpha += bgcolorHZ/2;

                if (alpha < 200 && i == 2)
                    alpha += bgcolorHZ/2;

            }

            if (func == "lighten")
            {
                if (alpha != 0)
                    alpha -= bgcolorHZ;
            }


            //failsafe
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

            bgbrush = new SolidBrush(Color.FromArgb(alpha, red, green, blue));
        }

       public void setgradient(Color color)
        {
            menugrad = new LinearGradientBrush(
                new Point(0, 0),
                new Point(0, menubackground.Top),
                Color.FromArgb(0, 255, 255, 255),
                color
            );
        }

        private void paintmenu(object sender, PaintEventArgs e)
        {
            
            ////////// ustawienia renderu
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            SolidBrush black = new SolidBrush(Color.Black);
            SolidBrush white = new SolidBrush(Color.White);
            SolidBrush red = new SolidBrush(Color.Red);
            SolidBrush cyan = new SolidBrush(Color.FromArgb(255, 0, 255, 229));

            ////////////// tlo
            e.Graphics.DrawImage(bg.Image, bg.Left, bg.Top, bg.Width, bg.Height);

            // przyciemnianie tła
            if (game == true)
            {
                e.Graphics.FillRectangle(bgbrush, bgcolor);
            }

            //////////////// gorna czesc panelu

            int combotxtwidth = TextRenderer.MeasureText(combotxt.Text, combotxtfont).Width;
            comboval.Location = new Point(combotxtwidth + padding, 0);
            combotxt.Location = new Point(comboval.Location.X - combotxtwidth, comboval.Location.Y + 20);

            if (combotxt.Visible && game == true)
            {
                e.Graphics.DrawString(combotxt.Text, combotxtfont, white, combotxt.Location);
                e.Graphics.DrawString(comboval.Text, combovalfont, cyan, comboval.Location);
            }

            exit.Location = new Point(Klawisze.W - TextRenderer.MeasureText(exit.Text, combovalfont).Width, -10);
            e.Graphics.DrawString(exit.Text, combovalfont, red, exit.Location);


            // ==================================================================================================== << DOLNY PANEL
            if (game == true)
            {
                e.Graphics.DrawImage(menubackground.Image, menubackground.Left, menubackground.Top, menubackground.Width, menubackground.Height);
                Rectangle gradient = new Rectangle(menubackground.Left, menubackground.Top, menubackground.Width, menubackground.Height);
                e.Graphics.FillRectangle(menugrad, gradient);

                e.Graphics.DrawString(menubtn.Text, bottomfont, black, menubtn.Location);
                e.Graphics.DrawString(time.Text, bottomfont, black, time.Location);
                e.Graphics.DrawString(knm.Text, bottomfont, black, knm.Location);
                e.Graphics.DrawString(points.Text, bottomfont, black, points.Location);
            }

        }



    }
    
}
