using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballSimulator.Classes
{
    class ScoreCollection
    {
        public List<Score> homeWinScores = new List<Score>();
        public List<Score> guestWinScores = new List<Score>();
        public List<Score> drawScores = new List<Score>();
        public int homeWinCount = 0;
        public int guestWinCount = 0;
        public int drawCount = 0;

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
            DB.getInstance().loadScores(this);
        }

        public int[] homeWin(double diff)
        {
            return getScore(homeWinScores, homeWinCount);
        }

        public int[] draw(double diff)
        {
            return getScore(drawScores, drawCount);
        }

        public int[] guestWin(double diff)
        {
            return getScore(guestWinScores, guestWinCount);
        }

        private int[] getScore(List<Score> list, int count)
        {
            int[] balls = new int[2] { 9, 9 };
            double val = RandomGenerator.getInstance().getDouble() * count;
            int cur = 0;
            foreach (Score score in list)
            {
                cur += score.count;
                if (val <= cur)
                {
                    balls = new int[2] { score.home, score.guest };
                    break;
                }
            }

            return balls;
        }
    }
}
