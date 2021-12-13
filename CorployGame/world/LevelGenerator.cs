using CorployGame.entity;
using CorployGame.world.navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorployGame.world
{
    static class LevelGenerator
    {
        public static Level GenerateLevel(List<Node> allNodes)
        {
            Level level = new Level(allNodes);




            return level;
        }

        static Agent GenerateCustomer()
        {
            return new Agent();
        }
    }
}
