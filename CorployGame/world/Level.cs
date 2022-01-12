using CorployGame.entity;
using CorployGame.world.navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorployGame.world
{
    class Level
    {
        public Dictionary<string, Node> AllNodes { get; set; }
        public List<Obstacle> Obstacles { get; set; }
        public BaseGameEntity PlayerObjective { get; set; }
        public BaseGameEntity EnemyObjective { get; set; }
        public Graph PathToPlObj { get; set; }
        public Graph PathToEnObj { get; set; }

        public Level(Dictionary<string, Node> allNodes)
        {
            AllNodes = allNodes;
            Obstacles = new List<Obstacle>();
            PathToPlObj = new Graph();
            PathToEnObj = new Graph();            
        }
    }
}
