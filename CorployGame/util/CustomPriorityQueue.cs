using CorployGame.world;
using CorployGame.world.navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CorployGame.util
{
    class CustomPriorityQueue
    {
        // This class class was created to avoid duplicate key('Node.Distance') issues.
        private SortedList<double, List<Node>> QueueList = new SortedList<double, List<Node>>();

        public bool IsEmpty()
        {
            if (QueueList.Count > 0) return false;
            return true;
        }

        public Node GetFirst()
        {
            if (IsEmpty()) return null;

            Node n = QueueList.Values.First()[0];

            return n;
        }

        public int Count()
        {
            return QueueList.Count; // Not perfect count, but not crucial for implementation.
        }

        public void Enqueue(Node node)
        {
            if (QueueList.ContainsKey(node.Distance) && QueueList[node.Distance] != null)
            {
                QueueList[node.Distance].Add(node);
            }            
            else
            {
                List<Node> q = new List<Node>();
                q.Add(node);
                QueueList.Add(node.Distance, q);
            }
        }

        public void Enqueue(Node node, double heurDist)
        {
            if (QueueList.ContainsKey(heurDist) && QueueList[heurDist] != null)
            {
                QueueList[heurDist].Add(node);
            }
            else
            {
                List<Node> q = new List<Node>();
                q.Add(node);
                QueueList.Add(heurDist, q);
            }
        }

        public Node Dequeue()
        {
            if (IsEmpty()) return null;
            double key = QueueList.Keys.First();
            Node node = QueueList.Values.First()[0];
            QueueList.Values.First().RemoveAt(0);

            if(QueueList[key] == null || QueueList[key].Count < 1)
            {
                QueueList.Remove(key);
            }

            return node;
        }

        // Returns <OrderList[Key](double) , Queue[Index](int)>
        public KeyValuePair<double, int> FindNode(Node n)
        {
            KeyValuePair<double, int> kvp = new KeyValuePair<double, int>(-1, -1); // Indexes can't both be negative, must send back KeyValuePair(return NULL not allowed) so this is the default.

            if (!QueueList.ContainsKey(n.Distance)) return kvp;

            List<Node> searchList = QueueList[n.Distance];

            int nodeIndex = searchList.IndexOf(n);

            kvp = new KeyValuePair<double, int>(n.Distance, nodeIndex);

            return kvp;
        }

        public KeyValuePair<double, int> FindNode(Node n, double heurDist)
        {
            KeyValuePair<double, int> kvp = new KeyValuePair<double, int>(-1, -1); // Indexes can't both be negative, must send back KeyValuePair(return NULL not allowed) so this is the default.

            if (!QueueList.ContainsKey(heurDist)) return kvp;

            List<Node> searchList = QueueList[heurDist];

            int nodeIndex = searchList.IndexOf(n);

            kvp = new KeyValuePair<double, int>(heurDist, nodeIndex);

            return kvp;
        }

        public void RemoveNode(KeyValuePair<double, int> location)
        {
            if (!QueueList.ContainsKey(location.Key)) return;

            QueueList[location.Key].RemoveAt(location.Value);

            // If after removing node, the sub-list is empty, remove whole entry form main list.
            if(QueueList[location.Key] == null || QueueList[location.Key].Count < 1)
            {
                QueueList.Remove(location.Key);
            }
        }

        public void Clear()
        {
            QueueList.Clear();
        }
    }
}
