using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FootballSimulator.Classes
{
    static class Match
    {
        public static string getScore(double homeRating, double guestRating, bool countHome = true)
        {
            RandomNumberGenerator generator = RandomNumberGenerator.Create();
            byte[] bytes = new byte[4];
            generator.GetBytes(bytes);
            double val = (double)BitConverter.ToUInt32(bytes, 0) / UInt32.MaxValue;
            if (val < 0.45)
            {
                return "1:0";
            }
            else if (val < 0.7)
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
