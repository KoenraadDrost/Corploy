using System;
using System.Collections.Generic;
using System.Text;

namespace CorployGame.world.navigation
{
    class Node
    {
        public int iIndex { get; set; }
        public Vector2D Pos { get; set; }

        public Node (int index, Vector2D pos)
        {
            iIndex = index;
            Pos = pos;
        }

        public bool IsTraversable()
        {
            return iIndex >= 1;
        }
    }
}
