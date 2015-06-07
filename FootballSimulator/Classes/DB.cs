using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballSimulator.Classes
{
    class DB
    {
        public SqlConnection connection;

        private static DB instance;

        public static DB getInstance()
        {
            if (instance == null)
            {
                instance = new DB();
            }

            return instance;
        }

        private DB()
        {
            string path = Directory.GetCurrentDirectory();
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = @"(LocalDB)\v11.0";
            builder.AttachDBFilename = path + @"\data.mdf";
            builder.IntegratedSecurity = false;
            connection = new SqlConnection(builder.ConnectionString);
            connection.Open();
        }

        public void Dispose()
        {
            connection.Close();
        }

        public List<Team> getTeams(int countryId)
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * from Team WHERE country_id = " + countryId;
            SqlDataReader reader = command.ExecuteReader();
            List<Team> teams = new List<Team>();
            while (reader.Read())
            {
                Team team = new Team();
                team.id = reader.GetInt32(reader.GetOrdinal("id"));
                team.name = reader.GetString(reader.GetOrdinal("name"));
                team.rating = reader.GetDouble(reader.GetOrdinal("rating"));
                teams.Add(team);
            }
            reader.Close();

            return teams;
        }

        public void loadScores(ScoreCollection collection)
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * from Score";
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int outcome = reader.GetInt32(reader.GetOrdinal("outcome"));
                int id = reader.GetInt32(reader.GetOrdinal("id"));
                int count = reader.GetInt32(reader.GetOrdinal("count"));
                int scoreHome = reader.GetInt32(reader.GetOrdinal("home"));
                int scoreGuest = reader.GetInt32(reader.GetOrdinal("guest"));
                Score score = new Score() { id = id, count = count, guest = scoreGuest, home = scoreHome };
                collection.outcomeRatio[Math.Sign(outcome)] += count;
                collection.differenceRatio[Math.Sign(outcome)][outcome] += count;
                collection.scoreCounts[outcome] += count;
                collection.scoreLists[outcome].Add(score);
                if (outcome > 0)
                {
                    collection.homeWinScores.Add(score);
                    collection.homeWinCount += count;
                }
                else if (outcome < 0)
                {
                    collection.guestWinScores.Add(score);
                    collection.guestWinCount += count;
                }
                else
                {
                    collection.drawScores.Add(score);
                    collection.drawCount += count;
                }
                collection.outcomeCount += count;
            }
            collection.homeWinScores.Sort(delegate(Score one, Score two)
            {
                int outcomeOne = one.home - one.guest;
                int outcomeGuest = two.home - two.guest;
                if (outcomeOne > outcomeGuest)
                {
                    return 1;
                }
                else if (outcomeOne < outcomeGuest)
                {
                    return -1;
                }
                else
                {
                    return one.home.CompareTo(two.home);
                }
            });
            collection.guestWinScores.Sort(delegate(Score one, Score two)
            {
                int outcomeOne = one.guest - one.home;
                int outcomeGuest = two.guest - two.home;
                if (outcomeOne > outcomeGuest)
                {
                    return 1;
                }
                else if (outcomeOne < outcomeGuest)
                {
                    return -1;
                }
                else
                {
                    return one.guest.CompareTo(two.guest);
                }
            });
            reader.Close();
        }
    }
}
