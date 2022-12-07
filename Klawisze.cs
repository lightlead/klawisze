namespace Klawisze
{
    public partial class Klawisze : Form
    {
        
        public static int W = 1028;
        public static int H = 768;
        public static int padding = 100;

        public static Font font = new Font("Stretch Pro V2", 20);

        private menu menu;
        private klawisz game;
        public Klawisze()
        {
            InitializeComponent();
            menu = new menu(this);
            game = new klawisz(this, 0);
        }

        private void Klawisze_KeyPress(object sender, KeyPressEventArgs e)
        {
            game.resetklawisza();
        }
        
    }
}