using FootballSimulator.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
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
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dataGridViewResults, new object[] { true });
        }

        void dataGridViewSpain_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewResults.SelectedRows.Count == 1 && dataGridViewResults.SelectedRows[0].Index == 0)
            {
                dataGridViewResults.ClearSelection();
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            dataGridViewResults.Hide();
        }

        private void buttonSimulate_Click(object sender, EventArgs e)
        {
            List<Team> teams = DB.getInstance().getTeams((int)comboBoxCountry.SelectedValue);
            int count = teams.Count;
            if (count == 0)
            {
                dataGridViewResults.Hide();
                return;
            }

            dataGridViewResults.Rows.Clear();
            dataGridViewResults.Columns.Clear();
            dataGridViewResults.Columns.Add("Position", "М");
            dataGridViewResults.Columns["Position"].Width = 32;
            dataGridViewResults.Columns.Add("Team", "Команда");
            dataGridViewResults.Columns["Team"].Width = 128;
            dataGridViewResults.Columns["Team"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            for (int position = 1; position <= teams.Count; position++)
            {
                dataGridViewResults.Columns.Add("Team" + position, position.ToString());
                dataGridViewResults.Columns["Team" + position].Width = 32;
            }
            dataGridViewResults.Columns.Add("Games", "Иг");
            dataGridViewResults.Columns["Games"].Width = 32;
            dataGridViewResults.Columns.Add("Won", "В");
            dataGridViewResults.Columns["Won"].Width = 32;
            dataGridViewResults.Columns.Add("Tied", "Н");
            dataGridViewResults.Columns["Tied"].Width = 32;
            dataGridViewResults.Columns.Add("Lost", "П");
            dataGridViewResults.Columns["Lost"].Width = 32;
            dataGridViewResults.Columns.Add("Scores", "Мячи");
            dataGridViewResults.Columns["Scores"].Width = 46;
            dataGridViewResults.Columns.Add("Diff", "Разн.");
            dataGridViewResults.Columns["Diff"].Width = 46;
            dataGridViewResults.Columns.Add("Points", "Оч");
            dataGridViewResults.Columns["Points"].Width = 32;
            int index = dataGridViewResults.Rows.Add();
            DataGridViewRow header = dataGridViewResults.Rows[index];
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
            
            dataGridViewResults.Rows.Add(count);

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
                dataGridViewResults.Rows[i].Height = 32;
                dataGridViewResults.Rows[i].Cells["Position"].Value = i;
                dataGridViewResults.Rows[i].Cells["Team"].Value = home.name;
                dataGridViewResults.Rows[i].Cells["Points"].Value = home.points;
                dataGridViewResults.Rows[i].Cells["Games"].Value = home.games;
                dataGridViewResults.Rows[i].Cells["Won"].Value = home.won;
                dataGridViewResults.Rows[i].Cells["Tied"].Value = home.tied;
                dataGridViewResults.Rows[i].Cells["Lost"].Value = home.lost;
                dataGridViewResults.Rows[i].Cells["Scores"].Value = home.scored + "-" + home.missed;
                int diff = home.scored - home.missed;
                dataGridViewResults.Rows[i].Cells["Diff"].Value = diff > 0 ? "+" + diff : diff.ToString();
                for (int j = 1; j <= count; j++)
                {
                    if (i == j)
                    {
                        dataGridViewResults.Rows[i].Cells["Team" + i].Value = i;
                        dataGridViewResults.Rows[i].Cells["Team" + i].Style.BackColor = ColorTranslator.FromHtml("#777");
                        dataGridViewResults.Rows[i].Cells["Team" + i].Style.SelectionBackColor = ColorTranslator.FromHtml("#777");
                        dataGridViewResults.Rows[i].Cells["Team" + i].Style.ForeColor = Color.White;
                        dataGridViewResults.Rows[i].Cells["Team" + i].Style.SelectionForeColor = Color.White;
                        continue;
                    }
                    Team guest = teams.ElementAt(j-1);
                    Match match = home.matches[guest.id];

                    string scoreString = match.homeScore + ":" + match.guestScore;
                    object value = dataGridViewResults.Rows[i].Cells["Team" + j].Value;
                    if (value != null)
                    {
                        value = scoreString + "\n" + value;
                    }
                    else
                    {
                        value = scoreString;
                    }
                    dataGridViewResults.Rows[i].Cells["Team" + j].Value = value;
                    dataGridViewResults.Rows[i].Cells["Team" + j].Tag = true;

                    scoreString = match.guestScore + ":" + match.homeScore;
                    value = dataGridViewResults.Rows[j].Cells["Team" + i].Value;
                    if (value != null)
                    {
                        value = value + "\n" + scoreString;
                    }
                    else
                    {
                        value = scoreString;
                    }
                    dataGridViewResults.Rows[j].Cells["Team" + i].Value = value;
                }
            }
            dataGridViewResults.Show();
            int gridHeight = dataGridViewResults.Rows.GetRowsHeight(DataGridViewElementStates.None);
            int gridWidth = dataGridViewResults.Columns.GetColumnsWidth(DataGridViewElementStates.None);
            dataGridViewResults.ClientSize = new Size(gridWidth + 1, gridHeight + 1);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataDataSet.Country' table. You can move, or remove it, as needed.
            this.countryTableAdapter.Fill(this.dataDataSet.Country);
            ScoreManager.getInstance().loadScores();
        }

        private void dataGridViewResults_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DataGridViewCell cell = dataGridViewResults.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (cell.Value != null && cell.Tag != null)
            {
                if (!e.Handled)
                {
                    e.Handled = true;
                    e.PaintBackground(e.CellBounds, dataGridViewResults.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected);
                }
                if ((e.PaintParts & DataGridViewPaintParts.ContentForeground) != DataGridViewPaintParts.None)
                {
                    string text = e.Value.ToString();
                    Rectangle rect = new Rectangle(e.CellBounds.Location, e.CellBounds.Size);
                    TextFormatFlags flags = TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
                    string[] cellParts = text.Split(new char[] { '\n' });
                    if (cellParts.Length == 2)
                    {
                        Color color = getColor(cellParts[0], e.CellStyle.ForeColor);
                        TextRenderer.DrawText(e.Graphics, cellParts[0] + "\n", e.CellStyle.Font, rect, color, flags);
                        color = getColor(cellParts[1], e.CellStyle.ForeColor);
                        TextRenderer.DrawText(e.Graphics, "\n" + cellParts[1], e.CellStyle.Font, rect, color, flags);
                    }
                }
            }
        }

        private Color getColor(string text, Color defaultColor)
        {
            string[] scoreParts = text.Split(new char[] { ':' });
            int first = int.Parse(scoreParts[0]);
            int second = int.Parse(scoreParts[1]);
            Color color = defaultColor;
            if (first > second)
            {
                color = Color.Green;
            }
            else if (first == second)
            {
                color = Color.Blue;
            }

            return color;
        }

        private void comboBoxCountry_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.State == DrawItemState.Focus)
                e.DrawFocusRectangle();
            var index = e.Index;
            if (index < 0 || index >= comboBoxCountry.Items.Count) return;
            var item = (DataRowView)comboBoxCountry.Items[index];
            string text = (item == null) ? "(null)" : item.Row["name"].ToString();
            using (var brush = new SolidBrush(e.ForeColor))
            {
                StringFormat format = new StringFormat();
                format.LineAlignment = StringAlignment.Center;
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                e.Graphics.DrawString(text, e.Font, brush, e.Bounds, format);
            }
        }
    }
}
