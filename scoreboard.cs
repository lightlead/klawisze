using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klawisze
{
    internal class scoreboard
    {
        public List<userdata> loadedscoreboard;
        public class userdata
        {
            public string nick;
            public int score;
        }

        public string nick;
        public int score;


        private List<string> updatedscoreboard;
        private string[] importedscoreboard;
        private string[] splitteddata = new string[2];
        private static string path = "../../../zasoby/scoreboard.txt";

        public scoreboard()
        {
            nick = "MISTRZ";
            score = 0;

            loadedscoreboard = new List<userdata>();
            updatedscoreboard = new List<string>();
            import();
        }

        private void import()
        {
            if (File.Exists(path))
            {
                importedscoreboard = System.IO.File.ReadAllLines(path);
                if (importedscoreboard.Length > 0)
                {
                    for (int i = 0; i < importedscoreboard.Length; i++)
                    {
                        splitteddata = importedscoreboard[i].Split(";");
                        loadedscoreboard.Add(new userdata { score = int.Parse(splitteddata[1]), nick = splitteddata[0] });
                    }
                }

            }
            else
            {
                File.Create("../../../zasoby/scoreboard.txt");
            }
        }

        public void update()
        {
            for (int i = 0; i < loadedscoreboard.Count; i++)
            {
                updatedscoreboard.Add(loadedscoreboard[i].nick + ";" + loadedscoreboard[i].score);
            }
            File.WriteAllLines(path, updatedscoreboard);

        }

    }
}
