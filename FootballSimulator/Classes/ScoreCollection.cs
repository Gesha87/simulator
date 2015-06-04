using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballSimulator.Classes
{
    class ScoreCollection
    {
        private static ScoreCollection instance;

        public static ScoreCollection getInstance()
        {
            if (instance == null)
            {
                instance = new ScoreCollection();
            }

            return instance;
        }

        private ScoreCollection()
        {
            DB.getInstance().loadScores();
        }

        public int[] homeWin(double diff)
        {
            int[] balls = {};

            return balls;
        }

        public int[] draw(double diff)
        {
            int[] balls = { };

            return balls;
        }

        public int[] guestWin(double diff)
        {
            int[] balls = { };

            return balls;
        }
    }
}
