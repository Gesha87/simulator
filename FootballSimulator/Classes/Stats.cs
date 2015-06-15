using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballSimulator.Classes
{
    class Stats
    {
        public int count = 0;

        public int homeWin = 0;
        public int draw = 0;
        public int guestWin = 0;

        public Dictionary<string, int> scores = new Dictionary<string,int>();

        private static Stats instance;

        public static Stats getInstance()
        {
            if (instance == null)
            {
                instance = new Stats();
            }

            return instance;
        }

        private Stats()
        {
            for (int i = 0; i <= 4; i++)
            {
                for (int j = i; j <= 9 - i; j++)
                {
                    scores.Add(i + ":" + j, 0);
                    if (i != j)
                    {
                        scores.Add(j + ":" + i, 0);
                    }
                }
            }
        }

        public void Reset()
        {
            instance = null;
        }
    }
}
