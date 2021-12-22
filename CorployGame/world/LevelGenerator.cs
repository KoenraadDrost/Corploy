using CorployGame.entity;
using CorployGame.world.navigation;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorployGame.world
{
    static class LevelGenerator
    {
        public static Level GenerateLevel(List<Node> allNodes, World w)
        {
            Level level = new Level(allNodes);



            return level;
        }

        static Agent GenerateCustomer(Vector2D spawnPos, World w)
        {
            Texture2D t = new Texture2D(w.GD, 10,10);
            return new Agent(spawnPos, w, t);
        }
    }
}
