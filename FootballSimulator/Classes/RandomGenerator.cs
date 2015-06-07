using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FootballSimulator.Classes
{
    class RandomGenerator
    {
        private static RandomGenerator instance;
        private RandomNumberGenerator generator;

        public static RandomGenerator getInstance()
        {
            if (instance == null)
            {
                instance = new RandomGenerator();
            }

            return instance;
        }

        private RandomGenerator()
        {
            generator = RandomNumberGenerator.Create();
        }

        public double getDouble()
        {
            byte[] bytes = new byte[4];
            generator.GetBytes(bytes);
            double val = (double)BitConverter.ToUInt32(bytes, 0) / UInt32.MaxValue;

            return val;
        }
    }
}
