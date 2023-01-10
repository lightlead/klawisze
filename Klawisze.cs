using Microsoft.VisualBasic.Devices;
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
        
        // USTAWIENIA OGÓLNE PROGRAMU
        public static int W = 1026;
        public static int H = 768;
        public static int padding = 100;

        //TIMERY
        private Stopwatch s, cntdwn;
        private TimeSpan ts;


        // DANE NA TEMAT GRACZA
        private string nick = "MISTRZ";
        private int combo = 0;
        private int streak = 0;
        private int points = 0;
        private int clicks = 0;

        // USTAWIENIA GRY
        private static int maxkeyamount = 15;
        private int keyamount;
        private Color thememodecolor;
        private int mode = 1;
        private bool startcountdown = false;
        private bool gamerunning = false;
        public bool fail = false;

        // USTAWIENIA RENDERU
        public static int keyscale = 4;
        public static int animspeed = 5;

        //tryb slow
        private static int maxwordamount = 15;
        private int wordamount;

        private static SoundPlayer click = new SoundPlayer("../../../zasoby/klik.wav");
        public static Image keyimg = Image.FromFile("../../../zasoby/klawisz.png");
        
        private menu menu;
        private keys[] key;
        private words[] word;
        private scoreboard scoreboard;
        public Klawisze()
        {
            InitializeComponent();
            menu = new menu(this);
            scoreboard = new scoreboard();
            word = new words[maxwordamount];
            key = new keys[maxkeyamount];
            s = new Stopwatch();
            cntdwn = new Stopwatch();
            initarrays();

            Application.Idle += gameloop;
            
        }

        private void initarrays()
        {
            for (int i = 0; i < maxkeyamount; i++)
            {
                key[i] = new keys(this);
            }

            for (int i = 0; i < maxkeyamount; i++)
            {
                key[i] = new keys(this);
            }

        }

        private void gameloop(object sender, EventArgs e)
        {
            while (IsApplicationIdle())
            {
                Invalidate();

                if (startcountdown)
                {
                    countdown.Visible = true;
                    TimeSpan cntdwnts = cntdwn.Elapsed;
                    countdown.Text = "ROZGRYWKA ROZPOCZNIE SIÊ ZA " + (3 - cntdwnts.Seconds).ToString() + "...";
                    if (cntdwnts.Seconds == 3)
                    {
                        cntdwn.Stop();
                        cntdwn.Reset();
                        countdown.Visible = false;
                        startgame();
                        startcountdown = false;
                    }
                }


                if (gamerunning)
                {

                    // AKTUALIZUJE CZAS, KNM, PUNKTY
                    ts = s.Elapsed;
                    menu.statupdate(s, points, clicks);

                    // KOÑCZY KA¯DY TRYB PO MINUCIE
                    if (ts.Minutes == 1)
                    {
                        showscoreboard();
                        stopgame();
                    }

                    // TRYB GRY 1
                    if (mode == 1 || mode == 3 && gamerunning) {
                        animkeys();
                        if (streak > 30)
                        {
                            menu.comboval.Text = "8X!";
                            for (int i = 0; i < keyamount; i++)
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

        private void showscoreboard()
        {
            exitmode.Visible = true;
            scoreboard.loadedscoreboard.Add(new scoreboard.userdata { score = points, nick = nick });
            scoreboard.loadedscoreboard = scoreboard.loadedscoreboard.OrderByDescending(o => o.score).ToList();

            title.Visible = true;
            top1.Text = scoreboard.loadedscoreboard[0].nick;
            top1score.Text = scoreboard.loadedscoreboard[0].score.ToString();
            top1.Visible = true;
            top1score.Visible = true;

            if (scoreboard.loadedscoreboard.Count == 2 )
            {
                top2.Text = scoreboard.loadedscoreboard[1].nick;
                top2score.Text = scoreboard.loadedscoreboard[1].score.ToString();
                top2.Visible = true;
                top2score.Visible = true;
            }
            else if (scoreboard.loadedscoreboard.Count == 3)
            {
                top2.Text = scoreboard.loadedscoreboard[1].nick;
                top2score.Text = scoreboard.loadedscoreboard[1].score.ToString();
                top2.Visible = true;
                top2score.Visible = true;

                top3.Text = scoreboard.loadedscoreboard[2].nick;
                top3score.Text = scoreboard.loadedscoreboard[2].score.ToString();
                top3.Visible = true;
                top3score.Visible = true;
            }
            else if (scoreboard.loadedscoreboard.Count == 4)
            {
                top2.Text = scoreboard.loadedscoreboard[1].nick;
                top2score.Text = scoreboard.loadedscoreboard[1].score.ToString();
                top2.Visible = true;
                top2score.Visible = true;

                top3.Text = scoreboard.loadedscoreboard[2].nick;
                top3score.Text = scoreboard.loadedscoreboard[2].score.ToString();
                top3.Visible = true;
                top3score.Visible = true;

                top4.Text = scoreboard.loadedscoreboard[3].nick;
                top4score.Text = scoreboard.loadedscoreboard[3].score.ToString();
                top4.Visible = true;
                top4score.Visible = true;
            }
            else if (scoreboard.loadedscoreboard.Count >= 5 )
            {
                top2.Text = scoreboard.loadedscoreboard[1].nick;
                top2score.Text = scoreboard.loadedscoreboard[1].score.ToString();
                top2.Visible = true;
                top2score.Visible = true;

                top3.Text = scoreboard.loadedscoreboard[2].nick;
                top3score.Text = scoreboard.loadedscoreboard[2].score.ToString();
                top3.Visible = true;
                top3score.Visible = true;

                top4.Text = scoreboard.loadedscoreboard[3].nick;
                top4score.Text = scoreboard.loadedscoreboard[3].score.ToString();
                top4.Visible = true;
                top4score.Visible = true;

                top5.Text = scoreboard.loadedscoreboard[4].nick;
                top5score.Text = scoreboard.loadedscoreboard[4].score.ToString();
                top5.Visible = true;
                top5score.Visible = true;
            }

        }

        private void animkeys()
        {
            for (int i = 0; i < keyamount; i++)
            {
                if (key[i].animate && !key[i].fail)
                    key[i].akeyspawn();
                else if (key[i].animate && key[i].fail)
                    key[i].aerror(thememodecolor);
            }
        }

        private void Klawisze_MouseClick(object sender, MouseEventArgs e)
        {
            // wyjscie
            if (e.X > menu.exit.Location.X && e.X < menu.exit.Location.X + menu.exit.Width && e.Y > menu.exit.Location.Y && e.Y < menu.exit.Location.Y + 75)
            {
                freezegame();
                DialogResult oknowyjscia = MessageBox.Show("Czy na pewno chcesz wyjœc z gry?", "[K]lawisze", MessageBoxButtons.YesNo);
                if (oknowyjscia == DialogResult.Yes)
                {
                    scoreboard.update();
                    Application.Exit();
                }
                unfreezegame();
            }

            // menu

            if (e.X > menu.menubtn.Location.X && e.X < menu.menubtn.Location.X + menu.menubtn.Width + 40 && e.Y > menu.menubtn.Location.Y && e.Y < menu.menubtn.Location.Y + 40)
            {
                stopgame();
                showpausemenu();
            }

        }

        private void backtomainmenu()
        {
            nickbox.Visible = true;
            startbtn.Visible = true;
            scoreboardbtn.Visible = true;
            exitgame.Visible = true;
            title.Visible = true;
            restartgamesettings();
        }

        private void hidemainmenu()
        {
            nickbox.Visible = false;
            startbtn.Visible = false;
            scoreboardbtn.Visible = false;
            exitgame.Visible = false;
            title.Visible = false;
        }

        private void showpausemenu()
        {
            title.Visible = true;
            exitmode.Visible = true;
            backtogame.Visible = true;
        }

        private void hidepausemenu()
        {
            title.Visible = false;
            exitmode.Visible = false;
            backtogame.Visible = false;
        }

        private void hidescoreboard()
        {
            top1.Visible = false;
            top1score.Visible = false;

            top2.Visible = false;
            top2score.Visible = false;

            top3.Visible = false;
            top3score.Visible = false;

            top4.Visible = false;
            top4score.Visible = false;

            top5.Visible = false;
            top5score.Visible = false;
        }

        private void restartgamesettings()
        {
            s.Reset();
            streak = 0;
            combo = 0;
            points = 0;
            clicks = 0;
            gamerunning = false;
        }

        private void freezegame()
        {
            s.Stop();
            gamerunning = false;
        }

        private void unfreezegame()
        {
            s.Start();
            gamerunning = true;
        }

        private void stopgame()
        {
            s.Stop();
            gamerunning = false;
            menu.game = false;
            for (int i = 0; i < maxkeyamount; i++)
            {
                key[i].render = false;
            }
        }

        private void showgamemodes()
        {
            title.Visible = true;
            mode1btn.Visible = true;
            mode2btn.Visible = true;
            mode3btn.Visible = true;
            mode4btn.Visible = true;
        }

        private void hidegamemodes()
        {
            mode1btn.Visible = false;
            mode2btn.Visible = false;
            mode3btn.Visible = false;
            mode4btn.Visible = false;
        }

        private void startgamecountdown()
        {
            hidegamemodes();
            hidemainmenu();
            hidepausemenu();
            hidescoreboard();
            countdown.Visible = true;
            cntdwn.Start();
            startcountdown = true;
        }

        private void Klawisze_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (mode == 1 && gamerunning)
            {
                int cnt = 0;
                for (int i = 0; i < keyamount; i++)
                {
                    if (e.KeyChar.ToString().ToLower() == key[i].keychar.Text || e.KeyChar.ToString().ToUpper() == key[i].keychar.Text)
                    {
                        click.Play();
                        clicks++;
                        key[i].keyrst(key, i);
                        streak++;
                        points += 100 + (int)menu.knmVal * combo;
                        cnt++;
                    }

                }
                keysfailanim(cnt);

            }
            else if (mode == 3 && gamerunning)
            {
                Point cursor = this.PointToClient(Cursor.Position);
                int cnt = 0;
                for (int i = 0; i < keyamount; i++)
                {
                    if (e.KeyChar.ToString().ToLower() == key[i].keychar.Text || e.KeyChar.ToString().ToUpper() == key[i].keychar.Text
                        && cursor.X > key[i].keybox.Location.X && cursor.X < key[i].keybox.Location.X + key[i].keybox.Width
                        && cursor.Y > key[i].keybox.Location.Y && cursor.Y < key[i].keybox.Location.Y + key[i].keybox.Height
                        )
                    {
                        click.Play();
                        clicks++;
                        key[i].keyrst(key, i);
                        streak++;
                        points += 100 + (int)menu.knmVal * combo;
                        cnt++;
                    }
                }
                keysfailanim(cnt);
            }

        }

        private void keysfailanim(int cnt)
        {
            if (cnt == 0)
            {
                for (int i = 0; i < keyamount; i++)
                {
                    key[i].animate = true;
                    key[i].fail = true;
                    streak = 0;
                }
            }
        }

        private void startbtn_Click(object sender, EventArgs e)
        {
            hidemainmenu();
            if (nickbox.Text == "WPROWAD SWÓJ NICK..")
                nick = "MISTRZ";
            else
                nick = nickbox.Text;

            showgamemodes();
            exitmode.Visible = true;
        }

        private void startgame()
        {
            hidegamemodes();
            for (int i = 0; i < maxkeyamount; i++)
            {
                key[i].keyrst(key, i);
            }

            checkmode();
            gamerunning = true;
            menu.game = true;
            s.Start();
        }

        private void checkmode()
        {
            if (mode == 1)
            {
                keyamount = 10;
                thememodecolor = Color.Cyan;
                menu.setgradient(thememodecolor);
                for (int i = 0; i < keyamount; i++)
                {
                    key[i].keyrst(key, i);
                    key[i].render = true;
                }
            }
            else if (mode == 3)
            {
                keyamount = 5;
                thememodecolor = Color.Red;
                menu.setgradient(thememodecolor);
                for (int i = 0; i < keyamount; i++)
                {
                    key[i].keyrst(key, i);
                    key[i].render = true;
                }
            }
            else if (mode == 2)
            {
                menu.setgradient(Color.Gold);
            }
            else if (mode == 4)
            {
                menu.setgradient(Color.Green);
            }
        }

        private void mode1btn_Click(object sender, EventArgs e)
        {
            mode = 1;
            startgamecountdown();
        }

        private void mode2btn_Click(object sender, EventArgs e)
        {
            mode = 2;
            startgamecountdown();
        }

        private void mode3btn_Click(object sender, EventArgs e)
        {
            mode = 3;
            startgamecountdown();
        }

        private void mode4btn_Click(object sender, EventArgs e)
        {
            mode = 4;
            startgamecountdown();
        }

        private void scoreboardbtn_Click(object sender, EventArgs e)
        {
            hidemainmenu();
            showscoreboard();
        }

        private void backtogame_Click(object sender, EventArgs e)
        {
            hidepausemenu();
            startgamecountdown();
        }

        private void exitmode_Click(object sender, EventArgs e)
        {
            hidescoreboard();
            hidepausemenu();
            hidegamemodes();
            backtomainmenu();
            restartgamesettings();
        }

        private void exitgame_Click(object sender, EventArgs e)
        {
            scoreboard.update();
            Application.Exit();
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


    }
}