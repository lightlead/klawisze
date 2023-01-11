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
        
        private gameui gameui;
        private keys[] key;
        private words[] word;
        private scoreboard scoreboard;
        public Klawisze()
        {
            InitializeComponent();
            gameui = new gameui(this);
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

            for (int i = 0; i < maxwordamount; i++)
            {
                word[i] = new words(this);
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
                    gameui.statupdate(s, points, clicks);

                    // KOÑCZY KA¯DY TRYB PO MINUCIE
                    if (ts.Minutes == 1)
                    {
                        addtoscoreboard();
                        showscoreboard();
                        stopgame();
                    }

                    // TRYB GRY 1
                    if (mode == 1 || mode == 3 && gamerunning) {
                        animkeys();
                        if (streak > 30)
                        {
                            gameui.comboval.Text = "8X!";
                            for (int i = 0; i < keyamount; i++)
                            {
                                key[i].rainbow();
                            }
                            combo = 8;
                        }
                        else if (streak > 20)
                        {
                            gameui.comboval.Text = "4X!";
                            gameui.colorbg("darken", 2);
                            combo = 4;
                        }
                        else if (streak > 10)
                        {
                            gameui.comboval.Text = "2X!";
                            gameui.colorbg("darken", 1);
                            combo = 2;
                            gameui.combotxt.Visible = true;
                        }
                        else if (streak == 0)
                        {
                            combo = 1;
                            gameui.combotxt.Visible = false;
                            gameui.colorbg("lighten", 0);
                        }
                    }

                }
                
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
            if (e.X > gameui.exit.Location.X && e.X < gameui.exit.Location.X + gameui.exit.Width && e.Y > gameui.exit.Location.Y && e.Y < gameui.exit.Location.Y + 75)
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

            if (e.X > gameui.menubtn.Location.X && e.X < gameui.menubtn.Location.X + gameui.menubtn.Width + 40 && e.Y > gameui.menubtn.Location.Y && e.Y < gameui.menubtn.Location.Y + 40)
            {
                stopgame();
                showpausemenu();
            }

        }

        private void backtomainmenu()
        {
            click.Play();
            nickbox.Visible = true;
            startbtn.Visible = true;
            scoreboardbtn.Visible = true;
            exitgame.Visible = true;
            title.Visible = true;
            restartgamesettings();
        }

        private void hidemainmenu()
        {
            click.Play();
            nickbox.Visible = false;
            startbtn.Visible = false;
            scoreboardbtn.Visible = false;
            exitgame.Visible = false;
            title.Visible = false;
        }

        private void showpausemenu()
        {
            click.Play();
            title.Visible = true;
            exitmode.Visible = true;
            backtogame.Visible = true;
        }

        private void hidepausemenu()
        {
            click.Play();
            title.Visible = false;
            exitmode.Visible = false;
            backtogame.Visible = false;
        }

        private void hidescoreboard()
        {
            click.Play();
            scoreboardleft.Visible = false;
            scoreboardright.Visible = false;
            scoreboardmode.Visible = false;

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
            nickbox.ReadOnly = false;
            gamerunning = false;
            gameui.game = false;
            if (mode == 1 || mode == 3)
            {
                for (int i = 0; i < maxkeyamount; i++)
                {
                    key[i].render = false;
                }
            }
            
            if (mode == 2 || mode == 4)
            {
                wordbox.Visible = false;
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

        private void wordbox_TextChanged(object sender, EventArgs e)
        {
            if (mode == 2 && gamerunning)
            {

            }
            else if (mode == 4 && gamerunning)
            {

            }

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
                        keyapproved(i);
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
                        keyapproved(i);
                        cnt++;
                    }
                }
                keysfailanim(cnt);
            }
        }

        private void keyapproved(int i)
        {
            click.Play();
            clicks++;
            key[i].keyrst(key, i, mode);
            streak++;
            points += 100 + (int)gameui.knmVal * combo;
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
            checkmode();
            nickbox.ReadOnly = false;
            gamerunning = true;
            gameui.game = true;
            s.Start();
        }

        private void checkmode()
        {
            if (mode == 1)
            {
                keyamount = 10;
                thememodecolor = Color.Cyan;
                gameui.setgradient(thememodecolor);
                for (int i = 0; i < keyamount; i++)
                {
                    key[i].keyrst(key, i, mode);
                    key[i].render = true;
                }
            }
            else if (mode == 3)
            {
                keyamount = 5;
                thememodecolor = Color.Red;
                gameui.setgradient(thememodecolor);
                for (int i = 0; i < keyamount; i++)
                {
                    key[i].keyrst(key, i, mode);
                    key[i].render = true;
                }
            }
            else if (mode == 2)
            {
                wordbox.BackColor = Color.Gold;
                gameui.setgradient(Color.Gold);
                wordbox.Visible = true;
            }
            else if (mode == 4)
            {
                wordbox.BackColor = Color.Chartreuse;
                gameui.setgradient(Color.Chartreuse);
                wordbox.Visible = true;
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

        private void addtoscoreboard()
        {
            if (mode == 1)
            {
                scoreboard.loadedscoreboard.Add(new scoreboard.userdata { score1 = points, nick = nick });
            }
            else if (mode == 3)
            {
                scoreboard.loadedscoreboard.Add(new scoreboard.userdata { score3 = points, nick = nick });
            }
            else if (mode == 2)
            {
                scoreboard.loadedscoreboard.Add(new scoreboard.userdata { score2 = points, nick = nick });
            }
            else if (mode == 4)
            {
                scoreboard.loadedscoreboard.Add(new scoreboard.userdata { score4 = points, nick = nick });
            }
        }

        private void showscoreboard()
        {
            click.Play();
            scoreboardleft.Visible = true;
            scoreboardright.Visible = true;
            scoreboardmode.Visible = true;
            exitmode.Visible = true;
            title.Visible = true;

            if (mode == 1)
            {
                scoreboard.loadedscoreboard = scoreboard.loadedscoreboard.OrderByDescending(o => o.score1).ToList();
                scoreboardmode.Text = "ZNAKI";
                scoreboardmode.ForeColor = Color.Cyan;

                if (scoreboard.loadedscoreboard.Count >= 5 && scoreboard.loadedscoreboard[4].score1 != 0)
                {
                    top1.Text = scoreboard.loadedscoreboard[0].nick;
                    top1score.Text = scoreboard.loadedscoreboard[0].score1.ToString();
                    top1.Visible = true;
                    top1score.Visible = true;

                    top2.Text = scoreboard.loadedscoreboard[1].nick;
                    top2score.Text = scoreboard.loadedscoreboard[1].score1.ToString();
                    top2.Visible = true;
                    top2score.Visible = true;

                    top3.Text = scoreboard.loadedscoreboard[2].nick;
                    top3score.Text = scoreboard.loadedscoreboard[2].score1.ToString();
                    top3.Visible = true;
                    top3score.Visible = true;

                    top4.Text = scoreboard.loadedscoreboard[3].nick;
                    top4score.Text = scoreboard.loadedscoreboard[3].score1.ToString();
                    top4.Visible = true;
                    top4score.Visible = true;

                    top5.Text = scoreboard.loadedscoreboard[4].nick;
                    top5score.Text = scoreboard.loadedscoreboard[4].score1.ToString();
                    top5.Visible = true;
                    top5score.Visible = true;
                }
                else if (scoreboard.loadedscoreboard.Count >= 4 && scoreboard.loadedscoreboard[3].score1 != 0)
                {
                    top1.Text = scoreboard.loadedscoreboard[0].nick;
                    top1score.Text = scoreboard.loadedscoreboard[0].score1.ToString();
                    top1.Visible = true;
                    top1score.Visible = true;

                    top2.Text = scoreboard.loadedscoreboard[1].nick;
                    top2score.Text = scoreboard.loadedscoreboard[1].score1.ToString();
                    top2.Visible = true;
                    top2score.Visible = true;

                    top3.Text = scoreboard.loadedscoreboard[2].nick;
                    top3score.Text = scoreboard.loadedscoreboard[2].score1.ToString();
                    top3.Visible = true;
                    top3score.Visible = true;

                    top4.Text = scoreboard.loadedscoreboard[3].nick;
                    top4score.Text = scoreboard.loadedscoreboard[3].score1.ToString();
                    top4.Visible = true;
                    top4score.Visible = true;
                }
                else if (scoreboard.loadedscoreboard.Count >= 3 && scoreboard.loadedscoreboard[2].score1 != 0)
                {
                    top1.Text = scoreboard.loadedscoreboard[0].nick;
                    top1score.Text = scoreboard.loadedscoreboard[0].score1.ToString();
                    top1.Visible = true;
                    top1score.Visible = true;

                    top2.Text = scoreboard.loadedscoreboard[1].nick;
                    top2score.Text = scoreboard.loadedscoreboard[1].score1.ToString();
                    top2.Visible = true;
                    top2score.Visible = true;

                    top3.Text = scoreboard.loadedscoreboard[2].nick;
                    top3score.Text = scoreboard.loadedscoreboard[2].score1.ToString();
                    top3.Visible = true;
                    top3score.Visible = true;
                }
                else if (scoreboard.loadedscoreboard.Count >= 2 && scoreboard.loadedscoreboard[1].score1 != 0)
                {
                    top1.Text = scoreboard.loadedscoreboard[0].nick;
                    top1score.Text = scoreboard.loadedscoreboard[0].score1.ToString();
                    top1.Visible = true;
                    top1score.Visible = true;

                    top2.Text = scoreboard.loadedscoreboard[1].nick;
                    top2score.Text = scoreboard.loadedscoreboard[1].score1.ToString();
                    top2.Visible = true;
                    top2score.Visible = true;
                }
                else if (scoreboard.loadedscoreboard.Count >= 1 && scoreboard.loadedscoreboard[0].score1 != 0)
                {
                    top1.Text = scoreboard.loadedscoreboard[0].nick;
                    top1score.Text = scoreboard.loadedscoreboard[0].score1.ToString();
                    top1.Visible = true;
                    top1score.Visible = true;
                }
            }
            else if (mode == 3)
            {
                scoreboard.loadedscoreboard = scoreboard.loadedscoreboard.OrderByDescending(o => o.score3).ToList();
                scoreboardmode.Text = "ZNAKI + MYSZKA";
                scoreboardmode.ForeColor = Color.Red;

                if (scoreboard.loadedscoreboard.Count >= 5 && scoreboard.loadedscoreboard[4].score3 != 0)
                {
                    top1.Text = scoreboard.loadedscoreboard[0].nick;
                    top1score.Text = scoreboard.loadedscoreboard[0].score3.ToString();
                    top1.Visible = true;
                    top1score.Visible = true;

                    top2.Text = scoreboard.loadedscoreboard[1].nick;
                    top2score.Text = scoreboard.loadedscoreboard[1].score3.ToString();
                    top2.Visible = true;
                    top2score.Visible = true;

                    top3.Text = scoreboard.loadedscoreboard[2].nick;
                    top3score.Text = scoreboard.loadedscoreboard[2].score3.ToString();
                    top3.Visible = true;
                    top3score.Visible = true;

                    top4.Text = scoreboard.loadedscoreboard[3].nick;
                    top4score.Text = scoreboard.loadedscoreboard[3].score3.ToString();
                    top4.Visible = true;
                    top4score.Visible = true;

                    top5.Text = scoreboard.loadedscoreboard[4].nick;
                    top5score.Text = scoreboard.loadedscoreboard[4].score3.ToString();
                    top5.Visible = true;
                    top5score.Visible = true;
                }
                else if (scoreboard.loadedscoreboard.Count >= 4 && scoreboard.loadedscoreboard[3].score3 != 0)
                {
                    top1.Text = scoreboard.loadedscoreboard[0].nick;
                    top1score.Text = scoreboard.loadedscoreboard[0].score3.ToString();
                    top1.Visible = true;
                    top1score.Visible = true;

                    top2.Text = scoreboard.loadedscoreboard[1].nick;
                    top2score.Text = scoreboard.loadedscoreboard[1].score3.ToString();
                    top2.Visible = true;
                    top2score.Visible = true;

                    top3.Text = scoreboard.loadedscoreboard[2].nick;
                    top3score.Text = scoreboard.loadedscoreboard[2].score3.ToString();
                    top3.Visible = true;
                    top3score.Visible = true;

                    top4.Text = scoreboard.loadedscoreboard[3].nick;
                    top4score.Text = scoreboard.loadedscoreboard[3].score3.ToString();
                    top4.Visible = true;
                    top4score.Visible = true;
                }
                else if (scoreboard.loadedscoreboard.Count >= 3 && scoreboard.loadedscoreboard[2].score3 != 0)
                {
                    top1.Text = scoreboard.loadedscoreboard[0].nick;
                    top1score.Text = scoreboard.loadedscoreboard[0].score3.ToString();
                    top1.Visible = true;
                    top1score.Visible = true;

                    top2.Text = scoreboard.loadedscoreboard[1].nick;
                    top2score.Text = scoreboard.loadedscoreboard[1].score3.ToString();
                    top2.Visible = true;
                    top2score.Visible = true;

                    top3.Text = scoreboard.loadedscoreboard[2].nick;
                    top3score.Text = scoreboard.loadedscoreboard[2].score3.ToString();
                    top3.Visible = true;
                    top3score.Visible = true;
                }
                else if (scoreboard.loadedscoreboard.Count >= 2 && scoreboard.loadedscoreboard[1].score3 != 0)
                {
                    top1.Text = scoreboard.loadedscoreboard[0].nick;
                    top1score.Text = scoreboard.loadedscoreboard[0].score3.ToString();
                    top1.Visible = true;
                    top1score.Visible = true;

                    top2.Text = scoreboard.loadedscoreboard[1].nick;
                    top2score.Text = scoreboard.loadedscoreboard[1].score3.ToString();
                    top2.Visible = true;
                    top2score.Visible = true;
                }
                else if (scoreboard.loadedscoreboard.Count >= 1 && scoreboard.loadedscoreboard[0].score3 != 0)
                {
                    top1.Text = scoreboard.loadedscoreboard[0].nick;
                    top1score.Text = scoreboard.loadedscoreboard[0].score3.ToString();
                    top1.Visible = true;
                    top1score.Visible = true;
                }
            }
            else if (mode == 2)
            {
                scoreboard.loadedscoreboard = scoreboard.loadedscoreboard.OrderByDescending(o => o.score2).ToList();
                scoreboardmode.Text = "S£OWA";
                scoreboardmode.ForeColor = Color.Gold;

                if (scoreboard.loadedscoreboard.Count >= 5 && scoreboard.loadedscoreboard[4].score2 != 0)
                {
                    top1.Text = scoreboard.loadedscoreboard[0].nick;
                    top1score.Text = scoreboard.loadedscoreboard[0].score2.ToString();
                    top1.Visible = true;
                    top1score.Visible = true;

                    top2.Text = scoreboard.loadedscoreboard[2].nick;
                    top2score.Text = scoreboard.loadedscoreboard[2].score2.ToString();
                    top2.Visible = true;
                    top2score.Visible = true;

                    top3.Text = scoreboard.loadedscoreboard[2].nick;
                    top3score.Text = scoreboard.loadedscoreboard[2].score2.ToString();
                    top3.Visible = true;
                    top3score.Visible = true;

                    top4.Text = scoreboard.loadedscoreboard[2].nick;
                    top4score.Text = scoreboard.loadedscoreboard[2].score2.ToString();
                    top4.Visible = true;
                    top4score.Visible = true;

                    top5.Text = scoreboard.loadedscoreboard[2].nick;
                    top5score.Text = scoreboard.loadedscoreboard[2].score2.ToString();
                    top5.Visible = true;
                    top5score.Visible = true;
                }
                else if (scoreboard.loadedscoreboard.Count >= 4 && scoreboard.loadedscoreboard[3].score2 != 0)
                {
                    top1.Text = scoreboard.loadedscoreboard[0].nick;
                    top1score.Text = scoreboard.loadedscoreboard[0].score2.ToString();
                    top1.Visible = true;
                    top1score.Visible = true;

                    top2.Text = scoreboard.loadedscoreboard[2].nick;
                    top2score.Text = scoreboard.loadedscoreboard[2].score2.ToString();
                    top2.Visible = true;
                    top2score.Visible = true;

                    top3.Text = scoreboard.loadedscoreboard[2].nick;
                    top3score.Text = scoreboard.loadedscoreboard[2].score2.ToString();
                    top3.Visible = true;
                    top3score.Visible = true;

                    top4.Text = scoreboard.loadedscoreboard[2].nick;
                    top4score.Text = scoreboard.loadedscoreboard[2].score2.ToString();
                    top4.Visible = true;
                    top4score.Visible = true;
                }
                else if (scoreboard.loadedscoreboard.Count >= 3 && scoreboard.loadedscoreboard[2].score2 != 0)
                {
                    top1.Text = scoreboard.loadedscoreboard[0].nick;
                    top1score.Text = scoreboard.loadedscoreboard[0].score2.ToString();
                    top1.Visible = true;
                    top1score.Visible = true;

                    top2.Text = scoreboard.loadedscoreboard[2].nick;
                    top2score.Text = scoreboard.loadedscoreboard[2].score2.ToString();
                    top2.Visible = true;
                    top2score.Visible = true;

                    top3.Text = scoreboard.loadedscoreboard[2].nick;
                    top3score.Text = scoreboard.loadedscoreboard[2].score2.ToString();
                    top3.Visible = true;
                    top3score.Visible = true;
                }
                else if (scoreboard.loadedscoreboard.Count >= 2 && scoreboard.loadedscoreboard[1].score2 != 0)
                {
                    top1.Text = scoreboard.loadedscoreboard[0].nick;
                    top1score.Text = scoreboard.loadedscoreboard[0].score2.ToString();
                    top1.Visible = true;
                    top1score.Visible = true;

                    top2.Text = scoreboard.loadedscoreboard[2].nick;
                    top2score.Text = scoreboard.loadedscoreboard[2].score2.ToString();
                    top2.Visible = true;
                    top2score.Visible = true;
                }
                else if (scoreboard.loadedscoreboard.Count >= 1 && scoreboard.loadedscoreboard[0].score2 != 0)
                {
                    top1.Text = scoreboard.loadedscoreboard[0].nick;
                    top1score.Text = scoreboard.loadedscoreboard[0].score2.ToString();
                    top1.Visible = true;
                    top1score.Visible = true;
                }
            }
            else if (mode == 4)
            {
                scoreboard.loadedscoreboard = scoreboard.loadedscoreboard.OrderByDescending(o => o.score4).ToList();
                scoreboardmode.Text = "S£OWA + MYSZKA";
                scoreboardmode.ForeColor = Color.Chartreuse;

                if (scoreboard.loadedscoreboard.Count >= 2 && scoreboard.loadedscoreboard[1].score4 != 0)
                {
                    top1.Text = scoreboard.loadedscoreboard[0].nick;
                    top1score.Text = scoreboard.loadedscoreboard[0].score4.ToString();
                    top1.Visible = true;
                    top1score.Visible = true;

                    top2.Text = scoreboard.loadedscoreboard[4].nick;
                    top2score.Text = scoreboard.loadedscoreboard[4].score4.ToString();
                    top2.Visible = true;
                    top2score.Visible = true;
                }
                else if (scoreboard.loadedscoreboard.Count >= 3 && scoreboard.loadedscoreboard[2].score4 != 0)
                {
                    top1.Text = scoreboard.loadedscoreboard[0].nick;
                    top1score.Text = scoreboard.loadedscoreboard[0].score4.ToString();
                    top1.Visible = true;
                    top1score.Visible = true;

                    top2.Text = scoreboard.loadedscoreboard[4].nick;
                    top2score.Text = scoreboard.loadedscoreboard[4].score4.ToString();
                    top2.Visible = true;
                    top2score.Visible = true;

                    top3.Text = scoreboard.loadedscoreboard[4].nick;
                    top3score.Text = scoreboard.loadedscoreboard[4].score4.ToString();
                    top3.Visible = true;
                    top3score.Visible = true;
                }
                else if (scoreboard.loadedscoreboard.Count >= 4 && scoreboard.loadedscoreboard[3].score4 != 0)
                {
                    top1.Text = scoreboard.loadedscoreboard[0].nick;
                    top1score.Text = scoreboard.loadedscoreboard[0].score4.ToString();
                    top1.Visible = true;
                    top1score.Visible = true;

                    top2.Text = scoreboard.loadedscoreboard[4].nick;
                    top2score.Text = scoreboard.loadedscoreboard[4].score4.ToString();
                    top2.Visible = true;
                    top2score.Visible = true;

                    top3.Text = scoreboard.loadedscoreboard[4].nick;
                    top3score.Text = scoreboard.loadedscoreboard[4].score4.ToString();
                    top3.Visible = true;
                    top3score.Visible = true;

                    top4.Text = scoreboard.loadedscoreboard[4].nick;
                    top4score.Text = scoreboard.loadedscoreboard[4].score4.ToString();
                    top4.Visible = true;
                    top4score.Visible = true;
                }
                else if (scoreboard.loadedscoreboard.Count >= 5 && scoreboard.loadedscoreboard[4].score4 != 0)
                {
                    top1.Text = scoreboard.loadedscoreboard[0].nick;
                    top1score.Text = scoreboard.loadedscoreboard[0].score4.ToString();
                    top1.Visible = true;
                    top1score.Visible = true;

                    top2.Text = scoreboard.loadedscoreboard[4].nick;
                    top2score.Text = scoreboard.loadedscoreboard[4].score4.ToString();
                    top2.Visible = true;
                    top2score.Visible = true;

                    top3.Text = scoreboard.loadedscoreboard[4].nick;
                    top3score.Text = scoreboard.loadedscoreboard[4].score4.ToString();
                    top3.Visible = true;
                    top3score.Visible = true;

                    top4.Text = scoreboard.loadedscoreboard[4].nick;
                    top4score.Text = scoreboard.loadedscoreboard[4].score4.ToString();
                    top4.Visible = true;
                    top4score.Visible = true;

                    top5.Text = scoreboard.loadedscoreboard[4].nick;
                    top5score.Text = scoreboard.loadedscoreboard[4].score4.ToString();
                    top5.Visible = true;
                    top5score.Visible = true;
                }
                else if (scoreboard.loadedscoreboard.Count >= 1 && scoreboard.loadedscoreboard[0].score4 != 0)
                {
                    top1.Text = scoreboard.loadedscoreboard[0].nick;
                    top1score.Text = scoreboard.loadedscoreboard[0].score4.ToString();
                    top1.Visible = true;
                    top1score.Visible = true;
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

        private void scoreboardleft_Click(object sender, EventArgs e)
        {
            mode--;
            if (mode == 0)
                mode = 4;
            hidescoreboard();
            showscoreboard();
        }

        private void scoreboardright_Click(object sender, EventArgs e)
        {
            mode++;
            if (mode == 5)
                mode = 1;
            hidescoreboard();
            showscoreboard();
        }

        bool IsApplicationIdle()
        {
            NativeMessage result;
            return PeekMessage(out result, IntPtr.Zero, (uint)0, (uint)0, (uint)0) == 0;
        }
        #endregion


    }
}