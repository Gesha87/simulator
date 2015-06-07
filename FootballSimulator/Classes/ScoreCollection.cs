﻿using System;
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

        public Dictionary<int, int> outcomeRatio = new Dictionary<int, int>();
        public Dictionary<int, Dictionary<int, int>> differenceRatio = new Dictionary<int, Dictionary<int, int>>();
        public Dictionary<int, List<Score>> scoreLists = new Dictionary<int, List<Score>>();
        public int outcomeCount = 0;
        public Dictionary<int, int> diffCount = new Dictionary<int, int>();
        public Dictionary<int, int> scoreCounts = new Dictionary<int, int>();

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
        }

        public Score getScore(double diff)
        {
            int outcome = getRandom(outcomeRatio, diff);
            int difference = getRandom(differenceRatio[outcome], diff);
            Score score = selectScore(scoreLists[difference], scoreCounts[difference]);

            return score;
        }

        public int getRandom(Dictionary<int, int> dict, double diff)
        {
            Dictionary<int, double> temp = new Dictionary<int, double>();
            int count = dict.Values.Sum();
            int outcome = dict.Keys.First();
            double val = RandomGenerator.getInstance().getDouble() * count;
            double change = Math.Atan(diff / 1.4) * 2 / Math.PI;

            double cur = 0;
            foreach (int key in temp.Keys)
            {
                cur += temp[key];
                if (val <= cur)
                {
                    outcome = key;
                    break;
                }
            }

            return outcome;
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
