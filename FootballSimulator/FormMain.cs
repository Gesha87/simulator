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
            List<Team> teams = DB.getInstance().getTeams(1);
            int count = teams.Count;

            dataGridViewSpain.Rows.Clear();
            dataGridViewSpain.Columns.Clear();
            dataGridViewSpain.Columns.Add("Position", "М");
            dataGridViewSpain.Columns.Add("Team", "Команда");
            foreach (Team team in teams)
            {
                dataGridViewSpain.Columns.Add("Team" + team.id, team.id.ToString());
                dataGridViewSpain.Columns["Team" + team.id].HeaderCell.Style.Font = new Font("Microsoft Sans Serif", (float)8.25);
            }
            dataGridViewSpain.Columns.Add("Points", "Очки");
            dataGridViewSpain.Rows.Add(count);
            
            for (int i = 0; i < count; i++)
            {
                Team home = teams.ElementAt(i);
                for (int j = 0; j < count; j++)
                {
                    if (i == j) continue;
                    Team guest = teams.ElementAt(j);
                    Match match = new Match(home, guest);
                    home.matches.Add(guest.id, match);
                    int homeScore = match.homeScore;
                    int guestScore = match.guestScore;
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
                Team home = teams.ElementAt(i);
                dataGridViewSpain.Rows[i].Cells["Position"].Value = i + 1;
                dataGridViewSpain.Rows[i].Cells["Team"].Value = home.name;
                dataGridViewSpain.Rows[i].Cells["Points"].Value = home.points;
                for (int j = 0; j < count; j++)
                {
                    if (i == j) continue;
                    Team guest = teams.ElementAt(j);
                    Match match = home.matches[guest.id];

                    string scoreString = match.homeScore + ":" + match.guestScore;
                    object value = dataGridViewSpain.Rows[i].Cells["Team" + (j + 1)].Value;
                    if (value != null)
                    {
                        value = scoreString + "\n" + value;
                    }
                    else
                    {
                        value = scoreString;
                    }
                    dataGridViewSpain.Rows[i].Cells["Team" + (j + 1)].Value = value;

                    scoreString = match.guestScore + ":" + match.homeScore;
                    value = dataGridViewSpain.Rows[j].Cells["Team" + (i + 1)].Value;
                    if (value != null)
                    {
                        value = value + "\n" + scoreString;
                    }
                    else
                    {
                        value = scoreString;
                    }
                    dataGridViewSpain.Rows[j].Cells["Team" + (i + 1)].Value = value;
                }
            }
            dataGridViewSpain.Show();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}
