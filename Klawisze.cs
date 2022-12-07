using System.Runtime.InteropServices;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Klawisze
{
    public partial class Klawisze : Form
    {
        
        public static int W = 1028;
        public static int H = 768;
        public static int padding = 100;
        
        public static Font font = new Font("Stretch Pro V2", 20);
        private System.Windows.Forms.Timer fps;
        
        private menu menu;
        private klawisz[] game;
        public Klawisze()
        {
            InitializeComponent();

            fps = new System.Windows.Forms.Timer();
            fps.Interval = 15; // 67fps
            fps.Enabled = true;
            fps.Tick += new EventHandler(fps_Tick);

            menu = new menu(this);

            game = new klawisz[10];
            for (int i = 0; i<10; i++)
            {
                game[i] = new klawisz(this, 0);
            }
            Application.Idle += gameloop;
            
        }

        private void gameloop(object sender, EventArgs e)
        {
            while (IsApplicationIdle())
            {

            }

        }

        private void fps_Tick(object sender, EventArgs e)
        {
/*            bool stop = false;
            int anim = 0;
            if (!stop)
            {
                for (int i = 0; i < 10; i++)
                {
                    anim = game[i].animacjaklawisza();
                }
            }
            
            if (anim == 10)
            {
                for (int i = 0; i < 10; i++)
                {
                    game[i].klawiszbox.Location = game[i].klawiszboxLocation;
                    stop = true;
                }
            }
  */          
            Invalidate();
        }

        private void Klawisze_KeyPress(object sender, KeyPressEventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                game[i].nowyklawisz();
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
    }
}