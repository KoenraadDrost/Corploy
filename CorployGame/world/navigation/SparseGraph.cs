using System;
using System.Collections.Generic;
using System.Text;

namespace CorployGame.world.navigation
{
    class SparseGraph
    {
        public List<Node> Nodes { get; set; } //"the nodes that comprise this graph"
        public List<Edge> EdgeList { get; set; } // Limited List of edges.
        public List<List<Edge>> Edges { get; set; } // (All edges, grouped by node) "a vector of adjacency edge lists. (each node index keys into the list of edges associated with that node)"
        public bool IsDirectional { get; set; } //"is this a directed graph?"

        public SparseGraph ()
        {

        }

        public int AddNode(Node node)
        {
            Nodes.Add(node);
            return node.iIndex;
        }

        public void RemoveNode()
        {

        }

        public void AddEdge()
        {

        }

        public void RemoveEdge()
        {

        }

        public double CalcEdgeCost(Node nFrom, Node nTo)
        {
            return Math.Abs( (nFrom.Pos - nTo.Pos).Length() );
        }
    }
}
