using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Security.Cryptography.Xml;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Klawisze
{
    public partial class Klawisze : Form
    {
        
        
        public static int W = 1028;
        public static int H = 768;
        public static int padding = 100;
        private int tryb = 1;
        private bool gameover = false;
        private bool start = true;
        public bool blad = false;

        private static int iloscklawiszy = 10;
        public static int szybkoscanimacji = 5;

        public static Image klawiszimg = Image.FromFile("../../../zasoby/klawisz2.png");
        public static Image backgroundimg = Image.FromFile("../../../zasoby/tlo.png");
        public static Font font = new Font("Stretch Pro V2", 20);
        
        private menu menu;
        private klawisz[] game;
        public Klawisze()
        {
            InitializeComponent();
            this.BackgroundImage = backgroundimg;

            menu = new menu(this);
            game = new klawisz[iloscklawiszy];
            
            for (int i = 0; i< iloscklawiszy; i++)
            {
                game[i] = new klawisz(this, 0);
            }
            for (int i = 0; i < iloscklawiszy; i++)
            {
                resetklawisza(i);
            }

            Application.Idle += gameloop;
            
        }

        private void gameloop(object sender, EventArgs e)
        {
            while (IsApplicationIdle())
            {
                if (!gameover && start)
                {                    
                    menu.update();

                    if (tryb == 1) {
                        
                        // anim handler
                        for (int i = 0; i < iloscklawiszy; i++)
                        {
                            if (game[i].animate && !game[i].blad)
                                game[i].animacjaklawisza();
                            else if (game[i].animate && game[i].blad)
                                game[i].animacjabledu();
                        }


                    }

                }
                
            }

        }

        private void resetklawisza(int i)
        {
            int licznik = iloscklawiszy - 1;
            int rst = licznik;
            game[i].znak.Text = init.rndchar();
            while (licznik != -1)
            {
                if (licznik != i)
                {
                    if (game[i].znak.Text == game[licznik].znak.Text)
                    {
                        game[i].znak.Text = init.rndchar();
                        licznik = iloscklawiszy - 1;
                    }
                    else
                    {
                        licznik--;
                    }
                }
                else licznik--;
            }


            game[i].nowyklawisz(game);
            licznik = rst;
            while (licznik != -1)
            {
                    if (licznik != i)
                    {
                        if (game[i].klawiszbox.Bounds.IntersectsWith(game[licznik].klawiszbox.Bounds))
                        {
                            game[i].nowyklawisz(game);
                            licznik = iloscklawiszy - 1;
                        }
                        else
                        {
                            licznik--;
                        }
                    }
                    else licznik--;
            }

            game[i].animate = true;

        }

        private void Klawisze_KeyPress(object sender, KeyPressEventArgs e)
        {
            int licznik = 0;
            for (int i = 0; i < iloscklawiszy; i++)
            {
                if (e.KeyChar.ToString().ToLower() == game[i].znak.Text || e.KeyChar.ToString().ToUpper() == game[i].znak.Text)
                {
                    menu.wcisniecia++;
                    licznik++;
                    resetklawisza(i);
                }

            }
            
            if (licznik == 0)
            {
                for (int i = 0; i < iloscklawiszy; i++)
                {
                    game[i].animate = true;
                    game[i].blad = true;
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