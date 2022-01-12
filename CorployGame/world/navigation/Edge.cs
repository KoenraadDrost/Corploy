using System;
using System.Collections.Generic;
using System.Text;

namespace CorployGame.world.navigation
{
    class Edge
    {
        public double Cost { get; set; }
        public string iFrom { get; set; }
        public string iTo { get; set; }

        public Edge(string from, string to, double cost)
        {
            iFrom = from;
            iTo = to;
            Cost = cost;
        }

        // Not sure if I need this and if I want to give it acces to Graph to check Nodes for Blocked state.
        //public bool IsTraversable()
        //{
        //    return iFrom >= 1 && iTo >= 1;
        //}
    }
}
