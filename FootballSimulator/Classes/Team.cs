using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballSimulator.Classes
{
    class Team
    {
        public int id;
        public string name;
        public double rating;

        public int points = 0;

        public int games = 0;
        public int scored = 0;
        public int missed = 0;

        public int won = 0;
        public int tied = 0;
        public int lost = 0;

        public Dictionary<int, Match> matches = new Dictionary<int, Match>();
    }
}
