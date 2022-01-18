using CorployGame.util;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorployGame.world.navigation
{
    class Graph
    {
        // --- Public ---
        public Dictionary<string, Node> Nodes { get; set; } //"the nodes that comprise this graph"
        public List<Edge> EdgeList { get; set; } // Limited List of edges.
        public List<List<Edge>> Edges { get; set; } // (All edges, grouped by node) "a vector of adjacency edge lists. (each node index keys into the list of edges associated with that node)"
        public bool IsDirectional { get; set; } //"is this a directed graph?"

        // --- Private ---
        //private int iNextNodeIndex;

        public Graph ()
        {
            Nodes = new Dictionary<string, Node>();
            //iNextNodeIndex = 1;
        }

        //public int AddNode(Node node)
        //{
        //    Nodes.Add(node);
        //    node.iIndex = iNextNodeIndex;
        //    iNextNodeIndex++;
        //    return node.iIndex;
        //}

        public void AddNode(string key, Node value)
        {
            Nodes.Add(key, value);
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

        // --- Graph Algorythms ---
        public List<Node> Dijkstra(Node start, Node end)
        {
            // Reset nodes before use.
            ResetNodeDistance();

            // Varriables
            Dictionary<string, Node> visited = new Dictionary<string, Node>();
            Dictionary<string, List<Node>> nodePaths = new Dictionary<string, List<Node>>(); // Keeps track of shortest path taken to reach each node from starting node.(Start node should not be in this list.)
            CustomPriorityQueue pQueue = new CustomPriorityQueue();

            // Dijkstra Algorithm
            // Preperation
            start.Distance = 0;
            start.Known = true;
            DijkstraRecursion(start);

            // Recursive loop
            void DijkstraRecursion(Node cN) // cN  = Current Node
            {
                // Add neighbours to queue if they're not in queue.
                for(int i = 0; i < cN.Adj.Count; i++)
                {
                    string neighInd;

                    // Bidirectional edges, select whichever node is NOT Current Node(cN)
                    if (cN.Adj[i].iFrom == cN.iIndex) neighInd = cN.Adj[i].iTo;
                    else neighInd = cN.Adj[i].iFrom;

                    if (visited.ContainsKey(neighInd) || !Nodes.ContainsKey(neighInd)) continue; // If neighbour was already resolved, skip. Or skip if node is blocked and not in dict.

                    // Retrieve node from dictionary
                    Node neighbour = Nodes[neighInd];

                    KeyValuePair<double, int> res = pQueue.FindNode(neighbour);

                    // Update distance if pathing(from start-node) through current node is shorter than the known path to the neighbour.
                    if ((cN.Distance + cN.Adj[i].Cost) < neighbour.Distance)
                    {
                        // Assemble new shorter path.
                        List<Node> eList = new List<Node>();
                        if (nodePaths.ContainsKey(cN.iIndex)) eList.AddRange(nodePaths[cN.iIndex]); // Normally only fails for start node, prevents nullpointer errors.
                        eList.Add(cN); // add current node.

                        if (nodePaths.ContainsKey(neighInd)) nodePaths[neighInd] = eList;
                        else nodePaths.Add(neighInd, eList);

                        if (res.Key > -1 && res.Value > -1)
                        {
                            pQueue.RemoveNode(res);
                            res = new KeyValuePair<double, int>(-1, -1);
                        }

                        neighbour.Distance = cN.Distance + cN.Adj[i].Cost;
                    }

                    // If neighbour is not already in queue, add to queue.
                    if (res.Key < 0 || res.Value < 0) pQueue.Enqueue(neighbour);
                    neighbour.Known = true;
                }

                // Before moving on to next in queue.
                visited.TryAdd(cN.iIndex, cN);

                // Continue recursion until end-node is reached. at which point no further path searching is needed.
                if (cN.iIndex != end.iIndex && pQueue.Count() > 0)
                    DijkstraRecursion(pQueue.Dequeue());
            }

            // Dijkstra finished, end-node path is now Shortest Path Tree(SPT)
            return nodePaths[end.iIndex];
        }

        public List<Node> AStar(Node start, Node end)
        {
            // Reset nodes before use.
            ResetNodeDistance();

            // Varriables
            Dictionary<string, Node> visited = new Dictionary<string, Node>();
            Dictionary<string, List<Node>> nodePaths = new Dictionary<string, List<Node>>(); // Keeps track of shortest path taken to reach each node from starting node.(Start node should not be in this list.)
            CustomPriorityQueue pQueue = new CustomPriorityQueue();

            // A* Algorithm
            // Preperation
            start.Distance = 0;
            start.Known = true;
            AStarRecursion(start);

            // Recursive loop
            void AStarRecursion(Node cN) // cN  = Current Node
            {
                Vector2D relNodeV = cN.Pos - end.Pos; // Relative position of neighbour to end point.
                double heurNodeEuclidean = relNodeV.Length();
                double heurNodeDist = cN.Distance + heurNodeEuclidean;

                // Add neighbours to queue if they're not in queue.
                for (int i = 0; i < cN.Adj.Count; i++)
                {
                    // Bidirectional edges, select whichever node is NOT Current Node(cN)
                    string neighInd = cN.Adj[i].iFrom == cN.iIndex ? cN.Adj[i].iTo : cN.Adj[i].iFrom;

                    if (visited.ContainsKey(neighInd) || !Nodes.ContainsKey(neighInd)) continue; // If neighbour was already resolved, skip. Or skip if node is blocked and not in dict.

                    // Retrieve node from dictionary
                    Node neighbour = Nodes[neighInd];

                    // A* Heuristic
                    Vector2D relNeiV = neighbour.Pos - end.Pos; // Relative position of neighbour to end point.
                    double heurNeiEuclidean = relNeiV.Length();
                    double heurNeiDist = neighbour.Distance + heurNeiEuclidean;

                    KeyValuePair<double, int> res = pQueue.FindNode(neighbour, heurNeiDist);

                    // Update distance if pathing(from start-node) through current node is shorter than the known path to the neighbour.
                    if ((heurNodeDist + cN.Adj[i].Cost) < heurNeiDist)
                    {
                        // Assemble new shorter path.
                        List<Node> eList = new List<Node>();
                        if (nodePaths.ContainsKey(cN.iIndex)) eList.AddRange(nodePaths[cN.iIndex]); // Normally only fails for start node, prevents nullpointer errors.
                        eList.Add(cN); // add current node.

                        if (nodePaths.ContainsKey(neighInd)) nodePaths[neighInd] = eList;
                        else nodePaths.Add(neighInd, eList);

                        if (res.Key > -1 && res.Value > -1)
                        {
                            pQueue.RemoveNode(res);
                            res = new KeyValuePair<double, int>(-1, -1);
                        }

                        neighbour.Distance = cN.Distance + cN.Adj[i].Cost;
                        heurNeiDist = neighbour.Distance + heurNeiEuclidean; // Update if new distance is shorter for propper placement in queue.
                    }

                    // If neighbour is not already in queue, add to queue.
                    if (res.Key < 0 || res.Value < 0) pQueue.Enqueue(neighbour, heurNeiDist);
                    neighbour.Known = true;
                }

                // Before moving on to next in queue.
                visited.TryAdd(cN.iIndex, cN);

                // Continue recursion until end-node is reached. at which point no further path searching is needed.
                if (cN.iIndex != end.iIndex && pQueue.Count() > 0)
                    AStarRecursion(pQueue.Dequeue());
            }

            // Dijkstra finished, end-node path is now Shortest Path Tree(SPT)
            return nodePaths[end.iIndex];
        }

        // Graph Algorythm Helper Functions
        public void ResetNodeDistance()
        {
            for (int iHor = 1; iHor < StaticParameters.MaxHorizontalNodes; iHor++)
            {
                for (int iVer = 1; iVer < StaticParameters.MaxVerticalNodes; iVer++)
                {
                    string key = $"{iHor}_{iVer}";

                    if (!Nodes.ContainsKey(key)) continue; // Skip if node does not exist in list.
                    Nodes[key].Distance = StaticParameters.InfinityD;
                }
            }
        }


    }
}
