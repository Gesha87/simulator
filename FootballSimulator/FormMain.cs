using FootballSimulator.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootballSimulator
{
    public partial class FormMain : Form
    {
        System.Data.SqlClient.SqlConnection connection;

        public FormMain()
        {
            InitializeComponent();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            dataGridViewSpain.Hide();
        }

        private void buttonSimulate_Click(object sender, EventArgs e)
        {
            dataGridViewSpain.Columns.Clear();
            dataGridViewSpain.Rows.Clear();
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * from Team WHERE country_id = 1";
            SqlDataReader reader = command.ExecuteReader();
            dataGridViewSpain.Columns.Add("Team", "Команда");
            List<Team> teams = new List<Team>();
            while (reader.Read())
            {
                Team team = new Team();
                team.id = reader.GetInt32(reader.GetOrdinal("id"));
                team.name = reader.GetString(reader.GetOrdinal("name"));
                team.rating = reader.GetDouble(reader.GetOrdinal("rating"));
                teams.Add(team);
                //dataGridViewSpain.Columns.Add(reader["id"].ToString(), reader["id"].ToString());
            }
            reader.Close();
            dataGridViewSpain.Columns.Add("Points", "Очки");
            int count = teams.Count;
            dataGridViewSpain.Rows.Add(count);
            for (int i = 0; i < count; i++)
            {
                Team home = teams.ElementAt(i);
                for (int j = 0; j < count; j++)
                {
                    if (i == j) continue;
                    Team guest = teams.ElementAt(j);
                    string score = Match.getScore(home.rating, guest.rating);
                    char[] delimiterChars = { ':' };
                    string[] words = score.Split(delimiterChars);
                    int homeScore = int.Parse((string)words.GetValue(0));
                    int guestScore = int.Parse((string)words.GetValue(1));
                    if (homeScore > guestScore)
                    {
                        home.points += 3;
                    }
                    else if (homeScore < guestScore)
                    {
                        guest.points += 3;
                    }
                    else
                    {
                        home.points += 1;
                        guest.points += 1;
                    }
                }
            }
            teams.Sort(delegate(Team x, Team y)
            {
                return -x.points.CompareTo(y.points);
            });
            for (int i = 0; i < count; i++)
            {
                Team team = teams.ElementAt(i);
                dataGridViewSpain.Rows[i].Cells["Team"].Value = team.name;
                dataGridViewSpain.Rows[i].Cells["Points"].Value = team.points;
            }
            dataGridViewSpain.Show();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            string path = Directory.GetCurrentDirectory();
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = @"(LocalDB)\v11.0";
            builder.AttachDBFilename = path + @"\data.mdf";
            builder.IntegratedSecurity = false;
            connection = new System.Data.SqlClient.SqlConnection(builder.ConnectionString);
            connection.Open();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            connection.Close();
        }
    }
}
