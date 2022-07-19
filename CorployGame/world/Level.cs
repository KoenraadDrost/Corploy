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
    class Level
    {
        // Public
        public Dictionary<string, Node> AllNodes { get; set; }
        public List<AiAgent> ActiveAgents { get; set; }
        public List<Obstacle> Obstacles { get; set; }
        public List<NpcWave> NpcWaves { get; set; }
        public Obstacle PlayerObjective { get; set; }
        public Obstacle EnemyObjective { get; set; }
        public Graph LevelGraph { get; set; }
        public List<Node> PathToPlObj { get; set; }
        public List<Node> PathToEnObj { get; set; }

        public bool levelComplete;

        // Private
        private int CurrentWave;

        public Level(Dictionary<string, Node> allNodes, Obstacle playerObj, Obstacle enemyObj)
        {
            // Passed parameters.
            AllNodes = allNodes;
            PlayerObjective = playerObj;
            EnemyObjective = enemyObj;

            // Defaults
            levelComplete = false;
            ActiveAgents = new List<AiAgent>();

            Obstacles = new List<Obstacle>();
            Obstacles.Add(PlayerObjective);
            Obstacles.Add(EnemyObjective);

            NpcWaves = new List<NpcWave>();

            LevelGraph = new Graph();
            PathToPlObj = new List<Node>();
            PathToEnObj = new List<Node>();
            CurrentWave = 0;
        }

        public void Initialize()
        {
            LevelGraph.GenerateBidirectionalEdges(AllNodes);

            // Setting up goals.
            // Note: Currently requires objectives to not block the nearest node.
            // Therefore Player and Enemy objective should always be set to "traversable"
            string PlObjNodeId = FindclosestNodeID(PlayerObjective.Pos);
            string EnObjNodeID = FindclosestNodeID(EnemyObjective.Pos);

            string NorthStart = FindclosestNodeID(StaticParameters.NorthSpawn);
            string SouthStart = FindclosestNodeID(StaticParameters.SouthSpawn);

            // TODO: account for alternative starting position.
            PathToPlObj = LevelGraph.AStar(  start: AllNodes[SouthStart],
                                                end: AllNodes[PlObjNodeId]);
            PathToEnObj = LevelGraph.AStar(  start: AllNodes[NorthStart],
                                                end: AllNodes[EnObjNodeID]);
        }

        public void Update(GameTime gameTime)
        {
            double ElapsedTime = gameTime.ElapsedGameTime.TotalSeconds;

            // Update wave
            if (NpcWaves != default && NpcWaves.Count > 0)
            {
                AiAgent agent = NpcWaves[CurrentWave].SpawnCheck(gameTime.ElapsedGameTime); // Note: ElapsedGameTime might not give timespan I need.
                if (agent != default)
                {
                    ActiveAgents.Add(agent); // Might add check to ensure no Agent gets added to list twice. For now: should not occur from 'SpawnCheck()'.

                    // Note: This step is not possible in LevelGenerator, as Edges and Paths can not be generated prior to level Innitialization.
                    List<Vector2D> vPath;
                    if (agent.Goal == PlayerObjective.Pos)
                    {
                        vPath = ConvertToVectorPath(PathToPlObj);
                    }
                    else vPath = ConvertToVectorPath(PathToEnObj);

                    agent.Path = vPath;
                }

                // Remove Agents that no longer have a role to play in the level.
                if (ActiveAgents.Count > 0)
                {
                    for(int i = ActiveAgents.Count-1; i >= 0; i--)
                    {
                        if (ActiveAgents[i].ShouldDespawn && ActiveAgents[i].Avatar == null)
                        {
                            ActiveAgents.Remove(ActiveAgents[i]);
                        }
                    }
                }

                // Next wave check
                if (ActiveAgents.Count < 1 && NpcWaves[CurrentWave].WaveEnded)
                {
                    if (NpcWaves.Count > CurrentWave + 1) CurrentWave++;
                    else levelComplete = true; // TODO: Later in development might not want this here.
                }
            }

            // Update entities
            for (int i = 0; i < ActiveAgents.Count; i++)
            {
                // TODO: might want to simplify this later.
                ActiveAgents[i].Update((float)ElapsedTime);
            }

            // Level checks

        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            for (int i = 0; i < ActiveAgents.Count; i++)
            {
                ActiveAgents[i].Draw(spriteBatch, gameTime);
            }

            for (int i = 0; i < Obstacles.Count; i++)
            {
                Obstacles[i].Draw(spriteBatch, gameTime);
            }
        }

        // Helper functions

        public string FindclosestNodeID(Vector2D pos)
        {
            string nodeID = "";

            double posX = pos.X;
            double posY = pos.Y;

            // Rounded down innitialy.
            int nodeIndX = (int)(posX / StaticParameters.DefaultNodeDistance);
            int nodeIndY = (int)(posY / StaticParameters.DefaultNodeDistance);

            // Determine if closest node index X and Y should be rounded up.
            if (posX % StaticParameters.DefaultNodeDistance > ( StaticParameters.DefaultNodeDistance / 2 ) )
                nodeIndX++;
            if (posY % StaticParameters.DefaultNodeDistance > (StaticParameters.DefaultNodeDistance / 2))
                nodeIndY++;

            // No nodes on the border of screen, results in -2 nodes in both X and Y axis.
            // example :: screenwidth: 1200, node-to-node-distance: 20, results in range: ( X = 1 to 59 ), instead of range ( X = 0 to 60 ).
            int maxXid = (StaticParameters.ScreenWidth / StaticParameters.DefaultNodeDistance) - 2;
            int maxYid = (StaticParameters.ScreenHeight / StaticParameters.DefaultNodeDistance) - 2;

            nodeIndX = Clamp(nodeIndX, 1, maxXid);
            nodeIndY = Clamp(nodeIndY, 1, maxYid);

            nodeID = $"{nodeIndX}_{nodeIndY}";

            if (AllNodes.ContainsKey(nodeID)) return nodeID;

            return null;
        }

        public List<string> FindNodeIDsInArea(Vector2D tl, Vector2D br)
        {
            List<string> nodesIDs = new List<string>();

            // This gives us the minimum 'X_Y' dictionary key.
            Vector2D minNodeV = new Vector2D(x: Math.Round(tl.X / StaticParameters.DefaultNodeDistance),
                                              y: Math.Round(tl.Y / StaticParameters.DefaultNodeDistance)
                                             );
            // This gives us the maximum 'X_Y' dictionary key.
            Vector2D maxNodeV = new Vector2D(x: Math.Round(br.X / StaticParameters.DefaultNodeDistance),
                                              y: Math.Round(br.Y / StaticParameters.DefaultNodeDistance)
                                             );

            // Now loop between min/max X and min/max Y.
            for (int iX = (int)minNodeV.X; iX <= (int)maxNodeV.X; iX++)
            {
                for (int iY = (int)minNodeV.Y; iY <= (int)maxNodeV.Y; iY++)
                {
                    nodesIDs.Add($"{iX}_{iY}");
                }
            }

            return nodesIDs;
        }

        public List<Vector2D> ConvertToVectorPath(List<Node> nodePath)
        {
            List<Vector2D> vectorPath = new List<Vector2D>();

            for(int i = 0; i < nodePath.Count; i++)
            {
                vectorPath.Add(nodePath[i].Pos);
            }

            return vectorPath;
        }

        // Might want to put this in larger scope later.
        public int Clamp(int value, int min, int max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }
    }
}
