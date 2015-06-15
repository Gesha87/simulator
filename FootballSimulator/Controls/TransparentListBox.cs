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
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            for (int i = 0; i < Items.Count; i++)
            {
                StringFormat strFmt = new System.Drawing.StringFormat();
                strFmt.LineAlignment = StringAlignment.Center;
                Rectangle rect = GetItemRectangle(i);
                if (i == SelectedIndex)
                {
                    //rect.X += 4;
                    //rect.Y += 2;
                }
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
                GraphicsPath path = RoundedRectangle.Create(rect.X + 1, rect.Y + 1, rect.Width - 2, rect.Height - 2, 7);
                e.Graphics.FillPath(brush, path);
                e.Graphics.DrawString(GetItemText(Items[i]), Font, new SolidBrush(ForeColor), rect, strFmt);
                if (i == hoveredIndex)
                {
                    path = RoundedRectangle.Create(rect.X + 1, rect.Y + 1, rect.Width - 2, rect.Height - 2, 7);
                    e.Graphics.DrawPath(Pens.White, path);
                }
                if (i == SelectedIndex || i == hoveredIndex)
                {
                    rect.X = rect.X + rect.Width - 22;
                    rect.Width = 22;
                    e.Graphics.DrawString(">", Font, new SolidBrush(ForeColor), rect, strFmt);
                }
            }

            base.OnPaint(e);  
        }
    }
}
