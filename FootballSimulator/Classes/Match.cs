using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FootballSimulator.Classes
{
    class Match
    {
        public Team home;
        public Team guest;
        public int homeScore = 0;
        public int guestScore = 0;

        private double percentWinHome = 0.456918219;
        private double percentDraw = 0.256815106;

        public Match(Team homeTeam, Team guestTeam, bool countHome = true)
        {
            home = homeTeam;
            guest = guestTeam;
            double val = RandomGenerator.getInstance().getDouble();         
            ScoreCollection score = ScoreCollection.getInstance();
            double diff = homeTeam.rating - guestTeam.rating;
            double change = diff / 4;
            percentDraw -= change * percentDraw / (1 - percentWinHome);
            percentWinHome += change;
            int[] balls;
            if (val < percentWinHome)
            {
                balls = score.homeWin(diff);   
            }
            else if (val < percentWinHome + percentDraw)
            {
                balls = score.draw(diff);
            }
            else
            {
                balls = score.guestWin(diff);
            }
            homeScore = balls[0];
            guestScore = balls[1];
        }
    }
}
