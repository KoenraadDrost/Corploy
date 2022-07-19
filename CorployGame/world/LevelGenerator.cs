using CorployGame.entity;
using CorployGame.util;
using CorployGame.world.navigation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorployGame.world
{
    class LevelGenerator
    {
        static World Wrld;
        static Vector2D SpawnPos;
        static Vector2D PlObjPos;
        static Vector2D EnObjPos;
        public Level GenerateLevel(World w)
        {
            // Prep
            Level level;
            Wrld = w;

            Obstacle playerObjective = new Obstacle(StaticParameters.TopLeftNodePos, w, new Texture2D(w.GD, 16, 24));
            playerObjective.VColor = Color.Blue;
            playerObjective.UpdateTexture();

            Obstacle enemyObjective = new Obstacle(StaticParameters.BottomRightNodePos, w, new Texture2D(w.GD, 16, 24));
            enemyObjective.VColor = Color.Red;
            enemyObjective.UpdateTexture();

            PlObjPos = playerObjective.Pos;
            EnObjPos = enemyObjective.Pos;

            // Core
            level = new Level(w.AllNodes, playerObjective, enemyObjective);

            level.Obstacles.AddRange(GenerateObstacles());

            TimeSpan interval = TimeSpan.FromSeconds(5.0);

            level.NpcWaves.Add( GenerateWave(interval, 5) );
            level.NpcWaves.Add( GenerateWave(interval, 6) );
            level.NpcWaves.Add( GenerateWave(interval, 7) );

            return level;
        }

        private AiAgent GenerateCustomer(Vector2D spawnPos, Vector2D goal)
        {
            Texture2D t = new Texture2D(Wrld.GD, 10,10);

            return new AiAgent(spawnPos, Wrld, t, goal);
        }

        private NpcWave GenerateWave(TimeSpan spawnInterval, int npcAmount)
        {
            List<AiAgent> agentList = new List<AiAgent>();

            for(int i = 0; i < npcAmount; i++)
            {
                // TODO: Replace goal with Ai decision logic. Possibly randomize spawn.
                Vector2D spawn = StaticParameters.NorthSpawn;
                Vector2D goal = PlObjPos;

                if (i % 2 > 0)
                {
                    goal = EnObjPos;
                    spawn = StaticParameters.SouthSpawn;
                }

                AiAgent agent = GenerateCustomer(spawn, goal);

                agentList.Add(agent);
            }


            return new NpcWave(agentList, spawnInterval);
        }

        private List<Obstacle> GenerateObstacles()
        {
            // TODO: Find a better way for this, for multiple levels later on.

            List<Obstacle> obstacles = new List<Obstacle>();

            Vector2D ObstPos = new Vector2D(StaticParameters.ScreenWidth / 3, 120);

            // Note: These elongated obstacles will cause added trouble for obstacle avoidance.
            Obstacle o1 = new Obstacle(ObstPos, Wrld, new Texture2D(Wrld.GD, 30, 240));
            o1.VColor = Color.Gray;
            o1.UpdateTexture();
            o1.SetBlockedNodes();
            obstacles.Add(o1);

            ObstPos.X = (StaticParameters.ScreenWidth / 3) * 2;
            ObstPos.Y = StaticParameters.ScreenHeight - 150;
            o1 = new Obstacle(ObstPos, Wrld, new Texture2D(Wrld.GD, 30, 300));
            o1.VColor = Color.Gray;
            o1.UpdateTexture();
            o1.SetBlockedNodes();
            obstacles.Add(o1);

            return obstacles;
        }
    }
}
