using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using FootballSimulator.Classes;
using System.Drawing.Imaging;

namespace FootballSimulator.Controls
{
    public partial class TransparentListBox : ListBox
    {
        private int hoveredIndex = -1;

        public TransparentListBox()
        {
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint, true);  
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            Invalidate();

            base.OnSelectedIndexChanged(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            Invalidate();
            Point point = PointToClient(Cursor.Position);
            hoveredIndex = IndexFromPoint(point);

            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            Invalidate();
            hoveredIndex = -1;

            base.OnMouseLeave(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            for (int i = 0; i < Items.Count; i++)
            {
                StringFormat strFmt = new System.Drawing.StringFormat();
                strFmt.LineAlignment = StringAlignment.Center;
                Rectangle rect = GetItemRectangle(i);
                LinearGradientBrush brush = new LinearGradientBrush(new Point(rect.X, rect.Y), new Point(rect.X + rect.Width, rect.Y + rect.Width), Color.FromArgb(255, 136, 162, 99), Color.FromArgb(0, 136, 162, 99));
                float[] relativeIntensities = { 0.0f, 1.0f, 1.0f };
                float[] relativePositions = { 0.0f, 0.5f, 1.0f };
                if (i == SelectedIndex)
                {
                    relativeIntensities = new float[] { 1.0f, 0.0f, 0.0f };
                    relativePositions = new float[] { 0.0f, 0.5f, 1.0f };
                }
                Blend blend = new Blend();
                blend.Factors = relativeIntensities;
                blend.Positions = relativePositions;
                brush.Blend = blend;
                GraphicsPath path = RoundedRectangle.Create(rect.X + 1, rect.Y + 1, rect.Width - 2, rect.Height - 2, 5);
                e.Graphics.FillPath(brush, path);
                Rectangle flagRect = new Rectangle(rect.X, rect.Y, 28, rect.Height);
                DataRow dataRow = ((DataRowView)(Items[i])).Row;
                Bitmap flag = (Bitmap)FootballSimulator.Properties.Resources.ResourceManager.GetObject(dataRow["flag"].ToString());
                e.Graphics.DrawImage(flag, flagRect.X + (flagRect.Width - flag.Width) / 2, flagRect.Y + (flagRect.Height - flag.Height) / 2, flag.Width, flag.Height);
                Rectangle nameRect = new Rectangle(rect.X + 24, rect.Y, rect.Width - 42, rect.Height);
                e.Graphics.DrawString(GetItemText(Items[i]), Font, new SolidBrush(ForeColor), nameRect, strFmt);
                if (i == hoveredIndex)
                {
                    path = RoundedRectangle.Create(rect.X + 1, rect.Y + 1, rect.Width - 2, rect.Height - 2, 7);
                    e.Graphics.DrawPath(Pens.White, path);
                }
                if (i == SelectedIndex || i == hoveredIndex)
                {
                    Rectangle arrowRect = new Rectangle(rect.X + rect.Width - 22, rect.Y, 22, rect.Height);
                    e.Graphics.DrawString(">", Font, new SolidBrush(ForeColor), arrowRect, strFmt);
                }
            }

            base.OnPaint(e);  
        }
    }
}
