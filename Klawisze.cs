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
        private bool gameover = false;
        private bool start = true;
        public bool fail = false;

        // Maksymalnie 30 poniewa¿ wiêcej siê nie zmieœci na ekranie
        private static int keyamount = 10;
        public static int keyscale = 4;
        public static int animspeed = 5;

        private static SoundPlayer click = new SoundPlayer("../../../zasoby/klik.wav");
        public static Image keyimg = Image.FromFile("../../../zasoby/klawisz.png");
        
        private menu menu;
        private keys[] key;
        public Klawisze()
        {
            InitializeComponent();
            menu = new menu(this);
            key = new keys[keyamount];
            
            for (int i = 0; i< keyamount; i++)
            {
                key[i] = new keys(this, 0);
            }
            for (int i = 0; i < keyamount; i++)
            {
                key[i].keyrst(key, i);
            }

            Application.Idle += gameloop;
            
        }

        private void gameloop(object sender, EventArgs e)
        {
            while (IsApplicationIdle())
            {
                Invalidate();
                if (!gameover && start)
                {                   
                    // AKTUALIZUJE CZAS, KNM, 
                    menu.statupdate();

                    if (mode == 1) {
                        animknm();
                        menu.colorbg("darken");
                    }

                }
                
            }

        }

        private void animknm()
        {
            for (int i = 0; i < keyamount; i++)
            {
                if (key[i].animate && !key[i].fail)
                    key[i].akeyspawn();
                else if (key[i].animate && key[i].fail)
                    key[i].aerror();
            }
        }

        private void Klawisze_KeyPress(object sender, KeyPressEventArgs e)
        {
            int licznik = 0;
            for (int i = 0; i < keyamount; i++)
            {
                if (e.KeyChar.ToString().ToLower() == key[i].keychar.Text || e.KeyChar.ToString().ToUpper() == key[i].keychar.Text)
                {
                    click.Play();
                    menu.clicks++;
                    licznik++;
                    key[i].keyrst(key, i);
                }

            }
            
            if (licznik == 0)
            {
                for (int i = 0; i < keyamount; i++)
                {
                    key[i].animate = true;
                    key[i].fail = true;
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