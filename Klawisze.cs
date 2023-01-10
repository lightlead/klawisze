using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Media;
using System.Runtime.InteropServices;
using System.Security.Cryptography.Xml;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Klawisze
{
    public partial class Klawisze : Form
    {
        
        
        public static int W = 1026;
        public static int H = 768;
        public static int padding = 100;
        private int mode = 1;
        private Stopwatch s;
        private TimeSpan ts;

        private int streak = 0;
        private int combo = 0;
        private int points = 0;
        private bool gameover = false;
        private bool start = true;
        public bool fail = false;

        // Maksymalnie 30 poniewa¿ wiêcej siê nie zmieœci na ekranie
        private static int maxkeyamount = 10;
        private int keyamount;
        public static int keyscale = 4;
        public static int animspeed = 5;

        private static SoundPlayer click = new SoundPlayer("../../../zasoby/klik.wav");
        public static Image keyimg = Image.FromFile("../../../zasoby/klawisz.png");
        
        private menu menu;
        private keys[] key;
        private init init;
        public Klawisze()
        {
            InitializeComponent();
            menu = new menu(this);
            key = new keys[maxkeyamount];
            s = new Stopwatch();
            s.Start();
            
            // tworzymy wszystkie klawisze
            for (int i = 0; i < maxkeyamount; i++)
            {
                key[i] = new keys(this);

            }

            // resetujemy klawisze zeby sie nie powtarza³y znaki
            for (int i = 0; i < maxkeyamount; i++)
            {
                key[i].keyrst(key, i);
            }

            Application.Idle += animloop;
            
        }

        private void animloop(object sender, EventArgs e)
        {
            while (IsApplicationIdle())
            {
                Invalidate();
                if (!gameover && start)
                {
                    // AKTUALIZUJE CZAS, KNM, 
                    ts = s.Elapsed;
                    menu.statupdate(s);
                    if (ts.Seconds == 10)
                    {
                        //s.Stop();
                    }

                    if (mode == 1) {
                        animknm();
                        if (streak > 30)
                        {
                            menu.comboval.Text = "8X!";
                            for (int i = 0; i < maxkeyamount; i++)
                            {
                                key[i].rainbow();
                            }
                            combo = 8;
                        }
                        else if (streak > 20)
                        {
                            menu.comboval.Text = "4X!";
                            menu.colorbg("darken", 2);
                            combo = 4;
                        }
                        else if (streak > 10)
                        {
                            menu.comboval.Text = "2X!";
                            menu.colorbg("darken", 1);
                            combo = 2;
                            menu.combotxt.Visible = true;
                        }
                        else if (streak == 0)
                        {
                            combo = 1;
                            menu.combotxt.Visible = false;
                            menu.colorbg("lighten", 0);
                        }
                    }

                }
                
            }

        }

        private void animknm()
        {
            for (int i = 0; i < maxkeyamount; i++)
            {
                if (key[i].animate && !key[i].fail)
                    key[i].akeyspawn();
                else if (key[i].animate && key[i].fail)
                    key[i].aerror();
            }
        }

        private void Klawisze_MouseClick(object sender, MouseEventArgs e)
        {
            //MessageBox.Show("");

            if (e.X > menu.exit.Location.X && e.X < menu.exit.Location.X + menu.exit.Width && e.Y > menu.exit.Location.Y && e.Y < menu.exit.Location.Y + 75)
            {
                DialogResult oknowyjscia = MessageBox.Show("Czy na pewno chcesz wyjœc z gry?", "[K]lawisze", MessageBoxButtons.YesNo);
                if (oknowyjscia == DialogResult.Yes)
                {
                    Application.Exit();
                }
            }
        }

        private void Klawisze_KeyPress(object sender, KeyPressEventArgs e)
        {
            int licznik = 0;
            for (int i = 0; i < maxkeyamount; i++)
            {
                if (e.KeyChar.ToString().ToLower() == key[i].keychar.Text || e.KeyChar.ToString().ToUpper() == key[i].keychar.Text)
                {
                    click.Play();
                    menu.clicks++;
                    key[i].keyrst(key, i);
                    streak++;
                    points += 100 + (int)menu.knmVal * combo;
                    menu.points.Text = "PUNKTY :: " + points;
                    licznik++;
                }

            }
            
            if (licznik == 0)
            {
                for (int i = 0; i < maxkeyamount; i++)
                {
                    key[i].animate = true;
                    key[i].fail = true;
                    streak = 0;
                }
            }

        }

        
        #region struktura potrzebna do gameloopa
        [StructLayout(LayoutKind.Sequential)]
        public struct NativeMessage
        {
            public IntPtr Handle;
            public uint Message;
            public IntPtr WParameter;
            public IntPtr LParameter;
            public uint Time;
            public Point Location;
        }
        [DllImport("user32.dll")]
        public static extern int PeekMessage(out NativeMessage message, IntPtr window, uint filterMin, uint filterMax, uint remove);
        bool IsApplicationIdle()
        {
            NativeMessage result;
            return PeekMessage(out result, IntPtr.Zero, (uint)0, (uint)0, (uint)0) == 0;
        }
        #endregion

        private void menuBox_Click(object sender, EventArgs e)
        {

        }

    }
}