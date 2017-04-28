using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace astar
{
    class Node
    {
        public int x;
        public int y;
        public int cost;
        public int heu;
        public int total { get { return (int)(0.3*cost + 1.7*heu); } }
        public bool inPath;
        public Node Parent;
    }
}
