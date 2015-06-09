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

        public Match(Team homeTeam, Team guestTeam, bool countHome = true)
        {
            home = homeTeam;
            guest = guestTeam;
            double val = RandomGenerator.getInstance().getDouble();         
            ScoreManager manager = ScoreManager.getInstance();
            Score score = manager.getScore(homeTeam.rating - guestTeam.rating);
            homeScore = score.home;
            guestScore = score.guest;
        }
    }
}
