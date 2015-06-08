using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballSimulator.Classes
{
    class ScoreManager
    {
        //public List<Score> homeWinScores = new List<Score>();
        //public List<Score> guestWinScores = new List<Score>();
        //public List<Score> drawScores = new List<Score>();
        //public int homeWinCount = 0;
        //public int guestWinCount = 0;
        //public int drawCount = 0;

        public Dictionary<int, int> outcomeRatio = new Dictionary<int, int>();
        public Dictionary<int, Dictionary<int, int>> differenceRatio = new Dictionary<int, Dictionary<int, int>>();
        public Dictionary<int, List<Score>> scoreLists = new Dictionary<int, List<Score>>();
        //public int outcomeCount = 0;
        //public Dictionary<int, int> diffCount = new Dictionary<int, int>();
        public Dictionary<int, int> scoreCounts = new Dictionary<int, int>();

        private static ScoreManager instance;
        private double chanceWinHome = 0;
        private double chanceDraw = 0;

        public static ScoreManager getInstance()
        {
            if (instance == null)
            {
                instance = new ScoreManager();
            }

            return instance;
        }

        private ScoreManager()
        {
            for (int i = -1; i <= 1; i++)
            {
                outcomeRatio.Add(i , 0);
                differenceRatio.Add(i, new Dictionary<int, int>());
            }
            for (int i = -7; i <= 7; i++)
            {
                differenceRatio[Math.Sign(i)].Add(i, 0);
                scoreCounts.Add(i, 0);
                scoreLists.Add(i, new List<Score>());
            }
            DB.getInstance().loadScores(this);
            int outcomeCount = outcomeRatio.Values.Sum();
            if (outcomeCount > 0)
            {
                chanceWinHome = (double)outcomeRatio[1] / outcomeCount;
                chanceDraw = (double)outcomeRatio[0] / outcomeCount;
            }
        }

        public Score getScore(double diff)
        {
            int outcome = getOutcome(diff);
            int difference = getDifference(differenceRatio[outcome], diff);
            Score score = selectScore(scoreLists[difference], scoreCounts[difference]);

            return score;
        }

        private int getOutcome(double diff)
        {
            double localChanceWinHome = chanceWinHome;
            double localChanceDraw = chanceDraw;
            int outcome = 0;
            double val = RandomGenerator.getInstance().getDouble();
            double change = Math.Atan(diff / 1.4) * 2 / Math.PI;
            if (diff < 0)
            {
                localChanceDraw += change * (chanceDraw - chanceWinHome * (1 + change));
                localChanceWinHome += change * chanceWinHome;
            }
            else
            {
                double localChanceWinGuest = 1 - chanceWinHome - chanceDraw;
                localChanceDraw -= change * (chanceDraw - localChanceWinGuest * (1 - change));
                localChanceWinGuest -= change * localChanceWinGuest;
                localChanceWinHome = 1 - chanceDraw - localChanceWinGuest;
            }

            if (val < chanceWinHome)
            {
                outcome = 1;
            }
            else if (val < chanceWinHome + chanceDraw)
            {
                outcome = 0;
            }
            else
            {
                outcome = -1;
            }

            return outcome;
        }

        private int getDifference(Dictionary<int, int> dict, double diff)
        {
            return dict.Keys.First();
        }

        private Score selectScore(List<Score> list, int count)
        {
            double val = RandomGenerator.getInstance().getDouble() * count;
            int cur = 0;
            foreach (Score score in list)
            {
                cur += score.count;
                if (val <= cur)
                {
                    return score;
                }
            }

            return list.First();
        }

        /*private double getRandom(double diff)
        {
            double val = RandomGenerator.getInstance().getDouble();
            double change = Math.Atan(diff) * 2 / Math.PI;
            if (diff > 0)
            {
                val += change * (1 - val);
            }
            else
            {
                val += change * val;
            }

            return val;
        }*/
    }
}
