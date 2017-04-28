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

            for (int i = 0; i < Closed.Count; i++)
                bufferg.FillRectangle(Brushes.Red,
                    ox + Closed[i].x * cs + 1,
                    oy + Closed[i].y * cs + 1,
                    cs - 2,
                    cs - 2);

            for (int i = 0; i < Open.Count; i++)
                bufferg.FillRectangle(Brushes.Green,
                    ox + Open[i].x * cs + 1,
                    oy + Open[i].y * cs + 1,
                    cs - 2,
                    cs - 2);

            e.Graphics.DrawImage(buffer, ClientRectangle.Left, ClientRectangle.Top);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
        }

        int gridWidth = 50;
        int gridHeight = 50;

        public void SetSizes(int gridWidth, int gridHeight)
        {
            this.gridWidth = gridWidth;
            this.gridHeight = gridHeight;
            Refresh();
        }

        List<Node> Open = new List<Node>();
        List<Node> Closed = new List<Node>();

        int destinationX;
        int destinationY;

        private int heuCompute(int ax, int ay, int bx, int by)
        {
            return Math.Abs(ax - bx) + Math.Abs(ay - by);
        }

        public void AstarInit(int sx, int sy, int dx, int dy)
        {
            destinationX = dx;
            destinationY = dy;

            Open.Clear();
            Closed.Clear();

            Open.Add(new Node()
            {
                x = sx,
                y = sy,
                cost = 0,
                heu = heuCompute(sx, sy, dx, dy)
            });
        }

        public Node isInList(List<Node> list, int x, int y)
        {
            foreach (Node n in list)
                if (n.x == x && n.y == y)
                    return n;

            return null;
        }

        private void AddNode(int x, int y, int cost)
        {
            if (x < 0 || x >= gridWidth || y < 0 || y >= gridHeight)
                return;

            if (isInList(Closed, x, y) != null)
                return;

            Node n = isInList(Open, x, y);

            if (n != null)
            {
                if (n.cost > cost)
                    n.cost = cost;
            }
            else
            {
                Open.Add(new Node()
                {
                    x = x,
                    y = y,
                    cost = cost,
                    heu = heuCompute(x, y, destinationX, destinationY)
                });
            }
        }

        int r = 0;

        public void AstarStep()
        {
            int total = Open[0].total;
            int idx = 0;

            for (int i = 0; i < Open.Count; i++)
                if (total > Open[i].total)
                {
                    total = Open[i].total;
                    idx = i;
                }

            Node n = Open[idx];
            Open.RemoveAt(idx);
            Closed.Add(n);

            AddNode(n.x + 1, n.y, n.cost + 1);
            AddNode(n.x, n.y + 1, n.cost + 1);
            AddNode(n.x - 1, n.y, n.cost + 1);
            AddNode(n.x, n.y - 1, n.cost + 1);

            //if (r++ % 1000 == 0)
                Invalidate();
        }
    }
}
