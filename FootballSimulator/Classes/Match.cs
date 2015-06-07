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

        private double chanceWinHome = 0.456918219;
        private double chanceDraw = 0.256815106;

        public Match(Team homeTeam, Team guestTeam, bool countHome = true)
        {
            home = homeTeam;
            guest = guestTeam;

            double val = RandomGenerator.getInstance().getDouble();         
            ScoreCollection collection = ScoreCollection.getInstance();
            double diff = (homeTeam.rating - guestTeam.rating) / 1.4;
            Score score = collection.getScore(homeTeam.rating - guestTeam.rating);
            /*double change = Math.Atan(diff) * 2 / Math.PI;
            if (diff < 0)
            {
                chanceDraw += change * (chanceDraw - chanceWinHome * (1 + change));
                chanceWinHome += change * chanceWinHome;
            }
            else
            {
                double chanceWinGuest = 1 - chanceWinHome - chanceDraw;
                chanceDraw -= change * (chanceDraw - chanceWinGuest * (1 - change));
                chanceWinGuest -= change * chanceWinGuest;
                chanceWinHome = 1 - chanceDraw - chanceWinGuest;
            }
            
            int[] balls;
            if (val < chanceWinHome)
            {
                balls = score.homeWin(diff);   
            }
            else if (val < chanceWinHome + chanceDraw)
            {
                balls = score.draw(diff);
            }
            else
            {
                balls = score.guestWin(diff);
            }*/
            homeScore = score.home;
            guestScore = score.guest;
        }
    }
}
