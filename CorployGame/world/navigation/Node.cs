using System.Collections.Generic;

namespace CorployGame.world.navigation
{
    class Node
    {
        public string iIndex { get; set; }
        public Vector2D Pos { get; set; }
        public List<Edge> Adj; // Adjacent Edges
        public double Distance; // Shortest distance from Start node.
        public bool Blocked;
        public bool Known;

        public Node (string index, Vector2D pos)
        {
            iIndex = index;
            Pos = pos;
            Adj = new List<Edge>();
            Distance = StaticParameters.InfinityD;
            Blocked = false;
            Known = false;
        }

        public bool IsTraversable()
        {
            return (Adj.Count > 0 && !Blocked);
        }

        public bool AddAdjacentEdge(Edge e)
        {
            if (Adj.Count < 1)
            {
                Adj.Add(e);
                return true;
            }
            
            // Check to avoid duplicate(Bidirectional)
            for(int i = 0; i < Adj.Count; i++)
            {
                if (Adj[i].iFrom == e.iFrom && Adj[i].iTo == e.iTo) return false;
                if (Adj[i].iTo == e.iFrom && Adj[i].iFrom == e.iTo) return false;
            }

            Adj.Add(e);
            return true;
        }
    }
}
