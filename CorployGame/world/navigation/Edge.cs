using System;
using System.Collections.Generic;
using System.Text;

namespace CorployGame.world.navigation
{
    class Edge
    {
        public double Cost { get; set; }
        public int iFrom { get; set; }
        public int iTo { get; set; }

        public Edge(int from, int to, double cost)
        {
            iFrom = from;
            iTo = to;
            Cost = cost;
        }

        public Edge(double cost) : this(0,0, cost)
        {
        }

        public bool IsTraversable()
        {
            return iFrom >= 1 && iTo >= 1;
        }
    }
}
