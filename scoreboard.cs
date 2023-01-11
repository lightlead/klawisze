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
            public int score1;
            public int score2;
            public int score3;
            public int score4;
        }

        public string nick;
        public int score;


        private List<string> updatedscoreboard;
        private string[] importedscoreboard;
        private string[] splitteddata = new string[5];
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
                        loadedscoreboard.Add(new userdata { nick = splitteddata[0],
                            score1 = int.Parse(splitteddata[1]),
                            score2 = int.Parse(splitteddata[2]),
                            score3 = int.Parse(splitteddata[3]),
                            score4 = int.Parse(splitteddata[4])
                        });
                    }
                }

            }
            else
            {
                File.Create("../../../zasoby/scoreboard.txt").Close();
            }
        }

        public void update()
        {
            for (int i = 0; i < loadedscoreboard.Count; i++)
            {
                updatedscoreboard.Add(loadedscoreboard[i].nick + ";"
                    + loadedscoreboard[i].score1 + ";"
                    + loadedscoreboard[i].score2 + ";"
                    + loadedscoreboard[i].score3 + ";"
                    + loadedscoreboard[i].score4 + ";"
                    );
            }
            File.WriteAllLines(path, updatedscoreboard);

        }

    }
}
