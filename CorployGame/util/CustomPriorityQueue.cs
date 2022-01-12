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
        private SortedList<double, Queue<Node>> QueueList = new SortedList<double, Queue<Node>>();

        public bool IsEmpty()
        {
            if (QueueList.Count > 0) return false;
            return true;
        }

        public Node GetFirst()
        {
            if (IsEmpty()) return null;

            Node n = QueueList.Values.First().Peek();

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
                QueueList[node.Distance].Enqueue(node);
            }            
            else
            {
                Queue<Node> q = new Queue<Node>();
                q.Enqueue(node);
                QueueList.Add(node.Distance, q);
            }
        }

        public Node Dequeue()
        {
            if (IsEmpty()) return null;
            double key = QueueList.Keys.First();
            Node node = QueueList.Values.First().Dequeue();

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

            List<Node> searchList = QueueList[n.Distance].ToList();

            int nodeIndex = searchList.IndexOf(n);

            kvp = new KeyValuePair<double, int>(n.Distance, nodeIndex);

            return kvp;
        }

        public void Clear()
        {
            QueueList.Clear();
        }
    }
}
