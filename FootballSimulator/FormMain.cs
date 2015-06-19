using FootballSimulator.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
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
            typeof(ListBox).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, listBoxCountry, new object[] { true });
            labelStats.BringToFront();            
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            Stats.getInstance().Reset();
            labelStats.Text = "Пусто";
            dataGridViewResults.Hide();
        }

        private void buttonSimulate_Click(object sender, EventArgs e)
        {
            simulate();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            this.countryTableAdapter.Fill(this.dataDataSet.Country);
            ScoreManager.getInstance().loadScores();
            simulate();
        }

        private void dataGridViewResults_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DataGridViewCell cell = dataGridViewResults.Rows[e.RowIndex].Cells[e.ColumnIndex];
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            using (Brush gridBrush = new SolidBrush(dataGridViewResults.GridColor))
            {
                using (Brush backColorBrush = new SolidBrush(Color.FromArgb(212, cell.Selected ? e.CellStyle.SelectionBackColor : e.CellStyle.BackColor)))
                {
                    using (Pen gridLinePen = new Pen(gridBrush))
                    {
                        Rectangle border = e.CellBounds;
                        border.X -= 1;
                        border.Y -= 1;
                        if (e.RowIndex == 0 && e.ColumnIndex == 0)
                        {
                            GraphicsPath path = RoundedRectangle.Create(border, 5, RoundedRectangle.RectangleCorners.TopLeft);
                            e.Graphics.FillPath(backColorBrush, path);
                            e.Graphics.DrawPath(gridLinePen, path);
                        }
                        else if (e.RowIndex == 0 && e.ColumnIndex == dataGridViewResults.ColumnCount - 1)
                        {
                            GraphicsPath path = RoundedRectangle.Create(border, 5, RoundedRectangle.RectangleCorners.TopRight);
                            e.Graphics.FillPath(backColorBrush, path);
                            e.Graphics.DrawPath(gridLinePen, path);
                        }
                        else if (e.RowIndex == dataGridViewResults.RowCount - 1 && e.ColumnIndex == 0)
                        {
                            GraphicsPath path = RoundedRectangle.Create(border, 5, RoundedRectangle.RectangleCorners.BottomLeft);
                            e.Graphics.FillPath(backColorBrush, path);
                            e.Graphics.DrawPath(gridLinePen, path);
                        }
                        else if (e.RowIndex == dataGridViewResults.RowCount - 1 && e.ColumnIndex == dataGridViewResults.ColumnCount - 1)
                        {
                            GraphicsPath path = RoundedRectangle.Create(border, 5, RoundedRectangle.RectangleCorners.BottomRight);
                            e.Graphics.FillPath(backColorBrush, path);
                            e.Graphics.DrawPath(gridLinePen, path);
                        }
                        else
                        {
                            e.Graphics.FillRectangle(backColorBrush, border);
                            e.Graphics.DrawRectangle(gridLinePen, border);
                        }
                    }
                }
            }
                    
            if (cell.Tag == null)
            {
                e.PaintContent(e.CellBounds);
            }
            else
            {
                string text = e.Value.ToString();
                Rectangle rect = new Rectangle(e.CellBounds.Location, e.CellBounds.Size);
                TextFormatFlags flags = TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
                string[] cellParts = text.Split(new char[] { '\n' });
                if (cellParts.Length == 2)
                {
                    Color color = getTextColor(cellParts[0], e.CellStyle.ForeColor);
                    TextRenderer.DrawText(e.Graphics, cellParts[0] + "\n", e.CellStyle.Font, rect, color, flags);
                    color = getTextColor(cellParts[1], e.CellStyle.ForeColor);
                    TextRenderer.DrawText(e.Graphics, "\n" + cellParts[1], e.CellStyle.Font, rect, color, flags);
                }
            }

            e.Handled = true;
        }

        private Color getTextColor(string text, Color defaultColor)
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

        private void checkBoxStats_CheckedChanged(object sender, EventArgs e)
        {
            labelStats.Visible = checkBoxStats.Checked;
        }

        private void listBoxCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            simulate();
        }

        private void simulate()
        {
            List<Team> teams = DB.getInstance().getTeams((int)listBoxCountry.SelectedValue);
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
            dataGridViewResults.Columns["Team"].Width = 118;
            dataGridViewResults.Columns["Team"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            for (int position = 1; position <= teams.Count; position++)
            {
                dataGridViewResults.Columns.Add("Team" + position, position.ToString());
                dataGridViewResults.Columns["Team" + position].Width = 27;
            }
            dataGridViewResults.Columns.Add("Games", "Иг");
            dataGridViewResults.Columns["Games"].Width = 30;
            dataGridViewResults.Columns.Add("Won", "В");
            dataGridViewResults.Columns["Won"].Width = 30;
            dataGridViewResults.Columns.Add("Tied", "Н");
            dataGridViewResults.Columns["Tied"].Width = 30;
            dataGridViewResults.Columns.Add("Lost", "П");
            dataGridViewResults.Columns["Lost"].Width = 30;
            dataGridViewResults.Columns.Add("Scores", "Мячи");
            dataGridViewResults.Columns["Scores"].Width = 44;
            dataGridViewResults.Columns.Add("Diff", "Разн.");
            dataGridViewResults.Columns["Diff"].Width = 40;
            dataGridViewResults.Columns.Add("Points", "Оч");
            dataGridViewResults.Columns["Points"].Width = 30;
            int index = dataGridViewResults.Rows.Add();
            DataGridViewRow header = dataGridViewResults.Rows[index];
            header.DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#E0E5E5");
            header.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#E0E5E5");
            header.Height = 27;
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
                Team home = teams.ElementAt(i - 1);
                dataGridViewResults.Rows[i].Height = 27;
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
                    Team guest = teams.ElementAt(j - 1);
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

            Stats stats = Stats.getInstance();
            labelStats.Text = "Всего матчей: " + stats.count + "\n" +
                "Домашние победы: " + string.Format("{0,0:P1}", (double)stats.homeWin / stats.count) + "\n" +
                "Ничьи: " + string.Format("{0,0:P1}", (double)stats.draw / stats.count) + "\n" +
                "Гостевые победы: " + string.Format("{0,0:P1}", (double)stats.guestWin / stats.count) + "\n";
            int k = 0;
            foreach (string key in stats.scores.Keys)
            {
                k++;
                labelStats.Text += key + ":" + string.Format("{0,5}({1,7:P3})", stats.scores[key], (double)stats.scores[key] / stats.count) + (k % 3 == 0 ? "\n" : " ");
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            listBoxCountry.SelectedIndexChanged -= listBoxCountry_SelectedIndexChanged;
        }
    }
}
