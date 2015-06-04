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
        public Dictionary<int, Match> matches = new Dictionary<int, Match>();
    }
}
