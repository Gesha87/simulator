using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballSimulator.Classes
{
    static class Match
    {
        public static string getScore(double homeRating, double guestRating, bool countHome = true)
        {
            Random rand = new Random();
            double val = rand.NextDouble();
            if (val < 0.45 + homeRating - guestRating)
            {
                return "1:0";
            }
            else if (val < 0.7 + (homeRating - guestRating) / 2) 
            {
                return "1:1";
            }
            else
            {
                return "0:1";
            }
        }
    }
}
