using CorployGame.entity;
using CorployGame.world.navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorployGame.world
{
    class Level
    {
        public List<Node> AllNodes { get; set; }
        public List<Obstacle> Obstacles { get; set; }
        public BaseGameEntity PlayerObjective { get; set; }
        public BaseGameEntity EnemyObjective { get; set; }

        public Level(List<Node> allNodes)
        {
            AllNodes = allNodes;
            Obstacles = new List<Obstacle>();
        }
    }
}
