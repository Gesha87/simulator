using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootballSimulator.Classes
{
    public partial class TransparentDataGridView : DataGridView
    {
        public TransparentDataGridView()
        {
            InitializeComponent();
        }

        protected override void PaintBackground(Graphics graphics, Rectangle clipBounds, Rectangle gridBounds)
        {
            base.PaintBackground(graphics, clipBounds, gridBounds);
            Rectangle rectSource = new Rectangle(this.Location.X, this.Location.Y, this.Width, this.Height);
            Rectangle rectDest = new Rectangle(0, 0, rectSource.Width, rectSource.Height);

            Bitmap b = new Bitmap(Parent.ClientRectangle.Width, Parent.ClientRectangle.Height);
            Graphics.FromImage(b).DrawImage(Parent.BackgroundImage, Parent.ClientRectangle);

            graphics.DrawImage(b, rectDest, rectSource, GraphicsUnit.Pixel);
        }
    }
}
