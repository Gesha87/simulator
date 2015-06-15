using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootballSimulator.Controls
{
    public partial class TransparentDataGridView : DataGridView
    {
        public TransparentDataGridView()
        {
            InitializeComponent();
        }

        protected override void PaintBackground(Graphics graphics, Rectangle clipBounds, Rectangle gridBounds)
        {
            Rectangle rectSource = new Rectangle(Location.X, Location.Y, Width, Height);
            Rectangle rectDest = new Rectangle(0, 0, rectSource.Width, rectSource.Height);

            graphics.DrawImage(Parent.BackgroundImage, rectDest, rectSource, GraphicsUnit.Pixel);
        }
    }
}
