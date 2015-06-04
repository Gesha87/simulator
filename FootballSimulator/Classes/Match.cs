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
            RandomNumberGenerator generator = RandomNumberGenerator.Create();
            byte[] bytes = new byte[4];
            generator.GetBytes(bytes);
            double val = (double)BitConverter.ToUInt32(bytes, 0) / UInt32.MaxValue;
            if (val < 0.45)
            {
                homeScore = 1;
            }
            else if (val < 0.7)
            {
                homeScore = 1;
                guestScore = 1;
            }
            else
            {
                guestScore = 1;
            }
        }
    }
}
