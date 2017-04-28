using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace astar
{
    class Astar_Control : Control
    {
        public Astar_Control() : base()
        {
        }

        Bitmap buffer;
        Graphics bufferg;

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            buffer = new Bitmap(ClientSize.Width, ClientSize.Height);
            bufferg = Graphics.FromImage(buffer);

            bufferg.Clear(Color.White);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            int cs = Math.Min(
                (int)(buffer.Width * 0.9 / gridWidth),
                (int)(buffer.Height * 0.9 / gridHeight)
                );

            int ox = (buffer.Width - cs * gridWidth) / 2;
            int oy = (buffer.Height - cs * gridHeight) / 2;

            for (int y = 0; y < gridHeight + 1; y++)
                bufferg.DrawLine(Pens.Black, ox, oy + y * cs, ox + cs * gridWidth, oy + y * cs);

            for (int x = 0; x < gridWidth + 1; x++)
                bufferg.DrawLine(Pens.Black, ox + x * cs, oy, ox + x * cs, oy + gridHeight * cs);

            e.Graphics.DrawImage(buffer, ClientRectangle.Left, ClientRectangle.Top);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
        }

        int gridWidth = 10;
        int gridHeight = 10;

        public void SetSizes(int gridWidth, int gridHeight)
        {
            this.gridWidth = gridWidth;
            this.gridHeight = gridHeight;
            Refresh();
        }
    }
}
