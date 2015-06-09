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

        void dataGridViewSpain_SelectionChanged(object sender, EventArgs e)
        {
            dataGridViewSpain.ClearSelection();
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
            dataGridViewSpain.Columns["Position"].Width = 32;
            dataGridViewSpain.Columns.Add("Team", "Команда");
            dataGridViewSpain.Columns["Team"].Width = 128;
            dataGridViewSpain.Columns["Team"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            for (int position = 1; position <= teams.Count; position++)
            {
                dataGridViewSpain.Columns.Add("Team" + position, position.ToString());
                dataGridViewSpain.Columns["Team" + position].Width = 32;
                dataGridViewSpain.Columns["Team" + position].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            dataGridViewSpain.Columns.Add("Games", "Иг");
            dataGridViewSpain.Columns["Games"].Width = 32;
            dataGridViewSpain.Columns.Add("Won", "В");
            dataGridViewSpain.Columns["Won"].Width = 32;
            dataGridViewSpain.Columns.Add("Tied", "Н");
            dataGridViewSpain.Columns["Tied"].Width = 32;
            dataGridViewSpain.Columns.Add("Lost", "П");
            dataGridViewSpain.Columns["Lost"].Width = 32;
            dataGridViewSpain.Columns.Add("Scores", "Мячи");
            dataGridViewSpain.Columns["Scores"].Width = 46;
            dataGridViewSpain.Columns.Add("Diff", "Разн.");
            dataGridViewSpain.Columns["Diff"].Width = 46;
            dataGridViewSpain.Columns.Add("Points", "Оч");
            dataGridViewSpain.Columns["Points"].Width = 32;
            int index = dataGridViewSpain.Rows.Add();
            DataGridViewRow header = dataGridViewSpain.Rows[index];
            header.DefaultCellStyle.Font = new Font("Microsoft San Serif", 8.25f, FontStyle.Bold);
            header.DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#E0E5E5");
            header.Height = 32;
            header.Cells["Position"].Value = "М";
            header.Cells["Team"].Value = "Команда";
            for (int position = 1; position <= teams.Count; position++)
            {
                header.Cells["Team" + position].Value = position;
            }
            header.Cells["Points"].Value = "Оч";
            header.Cells["Games"].Value = "Иг";
            header.Cells["Won"].Value = "В";
            header.Cells["Tied"].Value = "Н";
            header.Cells["Lost"].Value = "П";
            header.Cells["Scores"].Value = "Мячи";
            header.Cells["Diff"].Value = "Разн.";
            
            dataGridViewSpain.Rows.Add(count);

            for (int i = 0; i < count; i++)
            {
                Team home = teams.ElementAt(i);
                for (int j = 0; j < count; j++)
                {
                    if (i == j) continue;
                    Team guest = teams.ElementAt(j);
                    Match match = new Match(home, guest);
                    home.games += 1; guest.games += 1;
                    home.matches.Add(guest.id, match);
                    int homeScore = match.homeScore;
                    int guestScore = match.guestScore;
                    home.scored += homeScore; home.missed += guestScore;
                    guest.scored += guestScore; guest.missed += homeScore;
                    if (homeScore > guestScore)
                    {
                        home.points += 3;
                        home.won += 1;
                        guest.lost += 1;
                    }
                    else if (homeScore < guestScore)
                    {
                        guest.points += 3;
                        home.lost += 1;
                        guest.won += 1;
                    }
                    else
                    {
                        home.points += 1;
                        guest.points += 1;
                        home.tied += 1;
                        guest.tied += 1;
                    }
                }
            }

            teams.Sort(delegate(Team x, Team y)
            {
                if (x.points > y.points)
                {
                    return -1;
                }
                else if (x.points < y.points)
                {
                    return 1;
                }
                else
                {
                    int xDiff = x.scored - x.missed;
                    int yDiff = y.scored - y.missed;
                    if (xDiff > yDiff)
                    {
                        return -1;
                    }
                    else if (xDiff < yDiff)
                    {
                        return 1;
                    }
                    else
                    {
                        if (x.scored > y.scored)
                        {
                            return -1;
                        }
                        else if (x.scored < y.scored)
                        {
                            return 1;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
            });

            for (int i = 1; i <= count; i++)
            {
                Team home = teams.ElementAt(i-1);
                dataGridViewSpain.Rows[i].Height = 32;
                dataGridViewSpain.Rows[i].Cells["Position"].Value = i;
                dataGridViewSpain.Rows[i].Cells["Team"].Value = home.name;
                dataGridViewSpain.Rows[i].Cells["Points"].Value = home.points;
                dataGridViewSpain.Rows[i].Cells["Games"].Value = home.games;
                dataGridViewSpain.Rows[i].Cells["Won"].Value = home.won;
                dataGridViewSpain.Rows[i].Cells["Tied"].Value = home.tied;
                dataGridViewSpain.Rows[i].Cells["Lost"].Value = home.lost;
                dataGridViewSpain.Rows[i].Cells["Scores"].Value = home.scored + "-" + home.missed;
                int diff = home.scored - home.missed;
                dataGridViewSpain.Rows[i].Cells["Diff"].Value = diff > 0 ? "+" + diff : diff.ToString();
                for (int j = 1; j <= count; j++)
                {
                    if (i == j)
                    {
                        dataGridViewSpain.Rows[i].Cells["Team" + i].Value = i;
                        dataGridViewSpain.Rows[i].Cells["Team" + i].Style.BackColor = ColorTranslator.FromHtml("#777");
                        dataGridViewSpain.Rows[i].Cells["Team" + i].Style.ForeColor = Color.White;
                        continue;
                    }
                    Team guest = teams.ElementAt(j-1);
                    Match match = home.matches[guest.id];

                    string scoreString = match.homeScore + ":" + match.guestScore;
                    object value = dataGridViewSpain.Rows[i].Cells["Team" + j].Value;
                    if (value != null)
                    {
                        value = scoreString + "\n" + value;
                    }
                    else
                    {
                        value = scoreString;
                    }
                    dataGridViewSpain.Rows[i].Cells["Team" + j].Value = value;

                    scoreString = match.guestScore + ":" + match.homeScore;
                    value = dataGridViewSpain.Rows[j].Cells["Team" + i].Value;
                    if (value != null)
                    {
                        value = value + "\n" + scoreString;
                    }
                    else
                    {
                        value = scoreString;
                    }
                    dataGridViewSpain.Rows[j].Cells["Team" + i].Value = value;
                }
            }
            dataGridViewSpain.Show();
            int gridHeight = dataGridViewSpain.Rows.GetRowsHeight(DataGridViewElementStates.None);
            int gridWidth = dataGridViewSpain.Columns.GetColumnsWidth(DataGridViewElementStates.None);
            dataGridViewSpain.ClientSize = new Size(gridWidth + 1, gridHeight + 1);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            ScoreManager.getInstance().loadScores();
        }
    }
}
