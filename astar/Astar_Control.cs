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

        int cs;
        int ox;
        int oy;

        protected override void OnPaint(PaintEventArgs e)
        {
            cs = Math.Min(
                (int)(buffer.Width * 0.9 / gridWidth),
                (int)(buffer.Height * 0.9 / gridHeight)
                );

            ox = (buffer.Width - cs * gridWidth) / 2;
            oy = (buffer.Height - cs * gridHeight) / 2;

            for (int y = 0; y < gridHeight + 1; y++)
                bufferg.DrawLine(Pens.Black, ox, oy + y * cs, ox + cs * gridWidth, oy + y * cs);

            for (int x = 0; x < gridWidth + 1; x++)
                bufferg.DrawLine(Pens.Black, ox + x * cs, oy, ox + x * cs, oy + gridHeight * cs);

            for (int y = 0; y < gridHeight; y++)
                for (int x = 0; x < gridWidth; x++)
                    if (walls[x, y])
                        bufferg.FillRectangle(Brushes.Black,
                            ox + x * cs + 1,
                            oy + y * cs + 1,
                            cs - 2,
                            cs - 2);

            for (int i = 0; i < Closed.Count; i++)
            {
                int l = ox + Closed[i].x * cs + 1;
                int t = oy + Closed[i].y * cs + 1;

                bufferg.FillRectangle(Brushes.Red,
                    l,
                    t,
                    cs - 2,
                    cs - 2);

                /*bufferg.DrawString("" + Closed[i].cost,
                    this.Font,
                    Brushes.Black, l, t);*/
            }

            for (int i = 0; i < Open.Count; i++)
            {
                int l = ox + Open[i].x * cs + 1;
                int t = oy + Open[i].y * cs + 1;

                bufferg.FillRectangle(Brushes.Green,
                    l,
                    t,
                    cs - 2,
                    cs - 2);

                /*bufferg.DrawString("" + Open[i].cost,
                    this.Font,
                    Brushes.Black, l, t);

                bufferg.DrawString("" + Open[i].heu,
                    this.Font,
                    Brushes.Black, l + cs / 2, t + cs / 2);*/
            }

            e.Graphics.DrawImage(buffer, ClientRectangle.Left, ClientRectangle.Top);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
        }

        int gridWidth = 50;
        int gridHeight = 50;

        List<Node> Open = new List<Node>();
        List<Node> Closed = new List<Node>();

        int destinationX;
        int destinationY;

        bool[,] walls = new bool[50, 50];

        private int heuCompute(int ax, int ay, int bx, int by)
        {
            return (int)
                Math.Sqrt(
                    Math.Pow(ax - bx, 2)
                    +
                    Math.Pow(ay - by, 2)
                );

            //return Math.Abs(ax - bx) + Math.Abs(ax - bx);
        }

        public void AstarInit(int sx, int sy, int dx, int dy, int gridWidth, int gridHeight)
        {
            destinationX = dx;
            destinationY = dy;

            walls = new bool[gridWidth, gridHeight];

            this.gridWidth = gridWidth;
            this.gridHeight = gridHeight;

            Open.Clear();
            Closed.Clear();

            Open.Add(new Node()
            {
                x = sx,
                y = sy,
                cost = 0,
                heu = heuCompute(sx, sy, dx, dy)
            });

            Invalidate();
        }

        public Node isInList(List<Node> list, int x, int y)
        {
            foreach (Node n in list)
                if (n.x == x && n.y == y)
                    return n;

            return null;
        }

        List<Node> Path = new List<Node>();

        private void AddNode(int x, int y, int cost, int parentx, int parenty)
        {
            if (x < 0 || x >= gridWidth || y < 0 || y >= gridHeight)
                return;

            if (walls[x, y])
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
                    heu = heuCompute(x, y, destinationX, destinationY),
                    parentx = parentx,
                    parenty = parenty
                });
            }
        }

        int r = 0;

        public void AstarStep()
        {
            if (Open.Count == 0)
                return;

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

            if (n.x == destinationX && n.y == destinationY)
            {
                Open.Clear();




                return;
            }

            AddNode(n.x + 1, n.y, n.cost + 1, n.x, n.y);
            AddNode(n.x, n.y + 1, n.cost + 1, n.x, n.y);
            AddNode(n.x - 1, n.y, n.cost + 1, n.x, n.y);
            AddNode(n.x, n.y - 1, n.cost + 1, n.x, n.y);

            if (r++ % 10 == 0)
                Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.Button == MouseButtons.Right)
            {
                int cx = (e.X - ox) / cs;
                int cy = (e.Y - oy) / cs;

                if (!walls[cx, cy])
                {
                    walls[cx, cy] = true;
                    Invalidate();
                }
            }
        }
    }
}
