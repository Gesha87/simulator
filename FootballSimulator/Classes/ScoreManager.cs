using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballSimulator.Classes
{
    class ScoreManager
    {
        public Dictionary<int, int> outcomeRatio = new Dictionary<int, int>();
        public Dictionary<int, Dictionary<int, int>> differenceRatio = new Dictionary<int, Dictionary<int, int>>();
        public Dictionary<int, List<Score>> scoreLists = new Dictionary<int, List<Score>>();
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

        public void loadScores()
        {
            for (int i = -1; i <= 1; i++)
            {
                outcomeRatio.Add(i, 0);
                differenceRatio.Add(i, new Dictionary<int, int>());
            }
            for (int i = -9; i <= 9; i++)
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
            int difference = 0;
            switch (outcome)
            {
                case 1:
                    difference = getDifferenceHomeWin(diff);
                    break;
                case -1:
                    difference = getDifferenceGuestWin(diff);
                    break;
            }
            Score score = selectScore(scoreLists[difference], scoreCounts[difference]);

            return score;
        }

        private int getOutcome(double diff)
        {
            double localChanceWinHome = chanceWinHome;
            double localChanceDraw = chanceDraw;
            int outcome = 0;
            double val = RandomGenerator.getInstance().getDouble();
            double change = Math.Atan(diff) * 2 / Math.PI;
            if (diff < 0)
            {
                localChanceDraw += change * (chanceDraw - chanceWinHome * (1 + change));
                localChanceWinHome += change * chanceWinHome;
            }
            else
            {
                double localChanceWinGuest = 1 - localChanceWinHome - localChanceDraw;
                localChanceDraw -= change * (localChanceDraw - localChanceWinGuest * (1 - change));
                localChanceWinGuest -= change * localChanceWinGuest;
                localChanceWinHome = 1 - localChanceDraw - localChanceWinGuest;
            }

            if (val < localChanceWinHome)
            {
                outcome = 1;
            }
            else if (val < localChanceWinHome + localChanceDraw)
            {
                outcome = 0;
            }
            else
            {
                outcome = -1;
            }

            return outcome;
        }

        private int getDifferenceHomeWin(double diff)
        {
            Dictionary<int, int> temp = new Dictionary<int, int>(differenceRatio[1]);
            double val = RandomGenerator.getInstance().getDouble();
            if (diff >= 1)
            {
                for (int i = 0; i >= -7; i--)
                {
                    temp.Add(i, temp[2 - i]);
                }
                int d = (int)diff;
                if (d > 8) d = 8;
                for (int i = 9; i >= 1; i--)
                {
                    temp[i] = temp[i - d];
                }
            }
            if (diff < 0)
            {
                double change = -Math.Atan(diff) * 2 / Math.PI;
                for (int i = 9; i >= 2; i--)
                {
                    temp[i] -= (int)Math.Floor(temp[i] * change);
                }
            }
            int count = 0;
            for (int i = 1; i <= 9; i++) 
            {
                count += temp[i];
            }
            val *= count;

            double cur = 0;
            foreach (int key in differenceRatio[1].Keys)
            {
                cur += temp[key];
                if (val <= cur)
                {
                    return key;
                }
            }

            return 1;
        }

        private int getDifferenceGuestWin(double diff)
        {
            Dictionary<int, int> temp = new Dictionary<int, int>(differenceRatio[-1]);
            double val = RandomGenerator.getInstance().getDouble();
            if (diff <= -1)
            {
                for (int i = 0; i <= 7; i++)
                {
                    temp.Add(i, temp[i - 2]);
                }
                int d = -(int)diff;
                if (d > 8) d = 8;
                for (int i = -9; i <= -1; i++)
                {
                    temp[i] = temp[i + d];
                }
            }
            if (diff > 0)
            {
                double change = Math.Atan(diff) * 2 / Math.PI;
                for (int i = -9; i <= -2; i++)
                {
                    temp[i] -= (int)Math.Floor(temp[i] * change);
                }
            }
            int count = 0;
            for (int i = -1; i >= -9; i--)
            {
                count += temp[i];
            }
            val *= count;

            double cur = 0;
            foreach (int key in differenceRatio[-1].Keys)
            {
                cur += temp[key];
                if (val <= cur)
                {
                    return key;
                }
            }

            return -1;
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
    }
}
