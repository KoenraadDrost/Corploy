using CorployGame.behaviour.steering;
using CorployGame.entity;
using CorployGame.util;
using CorployGame.world;
using CorployGame.world.navigation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace CorployGame
{
    class World
    {
        // TODO: Remove when level takes this over.
        public List<Vehicle> entities = new List<Vehicle>();
        public List<Obstacle> obstacles = new List<Obstacle>();

        public PlayerAgent PlayerEntity { get; set; }
        public Dictionary<string, Node> AllNodes { get; set; }
        public Level CurrentLevel { get; set; }
        public Vehicle Target { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public GraphicsDevice GD { get; set; }

        public double LastUpdateTime { get; set; }

        public World(int w, int h, GraphicsDevice gd)
        {
            AllNodes = new Dictionary<string, Node>();

            //CurrentLevel = LevelGenerator.GenerateLevel(AllNodes, this);

            Width = w;
            Height = h;
            GD = gd;
            LastUpdateTime = 0;
        }

        public void Initialize()
        {
            GenerateAllNodes();
        }

        public void Populate()
        {
            // Entities
            Target = new Vehicle(new Vector2D(200, 100), this, new Texture2D(GD, 10, 10));
            Target.VColor = Color.Red;
            Target.UpdateTexture();

            PlayerEntity = new PlayerAgent(new Vector2D(100, 100), this, new Texture2D(GD, 16, 16));
            PlayerEntity.InitializeAvatar();

            Vehicle v = new Vehicle(new Vector2D(50, 50), this, new Texture2D(GD, 16, 16));
            v.SBS.SeekOn();
            //v.SBS.ArriveON();
            v.SBS.ObstacleAvoidanceON();
            v.SBS.SetTarget(Target.Pos);
            v.VColor = Color.Blue;
            v.UpdateTexture();
            entities.Add(v);

            // Obstacles
            Obstacle o1 = new Obstacle(new Vector2D(300, 300), this, new Texture2D(GD, 40, 40));
            o1.VColor = Color.Gray;
            o1.UpdateTexture();
            obstacles.Add(o1);

            //TODO: remove tests
            List<string> bNodes = o1.GetBlockedNodes();

            foreach (string bnode in bNodes)
            {
                AllNodes[bnode].Blocked = true;
            }

            Graph g = new Graph();
            GenerateBidirectionalEdges(g);
            List<Node> DijkstraNodes = g.AStar(     start: AllNodes["8_16"],
                                                    end: AllNodes["22_14"]);

            List<Vector2D> path = new List<Vector2D>();

            foreach(Node n in DijkstraNodes)
            {
                path.Add(n.Pos);
            }

            Vehicle v2 = new Vehicle(new Vector2D(50, 50), this, new Texture2D(GD, 16, 16));
            //v.SBS.SeekOn();
            //v.SBS.ArriveON();
            v2.SBS.ObstacleAvoidanceON();
            v2.SBS.PathFollowingON();
            v2.SBS.SetPath(path);
            v2.VColor = Color.Purple;
            v2.UpdateTexture();
            entities.Add(v2);


            //Console.WriteLine("Last node in dijsktraList: " + DijkstraNodes[DijkstraNodes.Count - 1].Pos);
            //Console.WriteLine("missing node is known?: " + AllNodes["22_13"].Known); // Note: Last node before end always seems to be missing. No clue why yet.

            //foreach (Node n in DijkstraNodes)
            //{
            //    string s = "";
            //    s += $"Node ID: {n.iIndex} edgecount: {n.Adj.Count} ";
            //    //foreach(Edge e in n.Adj)
            //    //{
            //    //    s += $" => ( E: {e.iFrom} -> {e.iTo} )";
            //    //}
            //    Console.WriteLine(s);

            //    Obstacle ob = new Obstacle(n.Pos, this, new Texture2D(GD, 4, 4));
            //    ob.VColor = Color.Green;
            //    ob.UpdateTexture();
            //    obstacles.Add(ob);
            //}
        }



        public void Update (GameTime gameTime)
        {
            double ElapsedTime = gameTime.ElapsedGameTime.TotalSeconds;

            PlayerEntity.Update((float)ElapsedTime);

            for (int i = 0; i < entities.Count; i++)
            {
                // TODO: might want to simplify this later.
                entities[i].Update((float)ElapsedTime);
            }

            // TODO: change the way target works so we don't get this possibility.
            Target.Update((float)ElapsedTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            PlayerEntity.Avatar.Draw(spriteBatch, gameTime);

            Target.Draw(spriteBatch, gameTime);

            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].Draw(spriteBatch, gameTime);
            }

            for (int i = 0; i < obstacles.Count; i++)
            {
                obstacles[i].Draw(spriteBatch, gameTime);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="me"></param>
        /// <param name="dBoxLength"></param>
        /// <returns></returns>
        public List<Square> TagObstaclesInCollisionRange(Vector2D pos, double dBoxLength)
        {
            List<Square> taggedObstacles = new List<Square>();

            for(int i = 0; i < obstacles.Count; i++)
            {
                // Measure distance between obstacle and moving entity and check if distance is shorter than detection box length.
                double distance = (obstacles[i].Pos - pos).Length();
                if (distance <= dBoxLength) taggedObstacles.Add( new Square(obstacles[i].Pos, Height) ); // Add a copy of obstacle to list, to avoid altering original during calculations.                
            }

            // Return null if no obstacle in collisionbox
            return taggedObstacles.Count < 1 ? null : taggedObstacles;
        }

        public List<Obstacle> TagObstaclesInCollisionRange(MovingEntity me, double dBoxLength)
        {
            List<Obstacle> taggedObstacles = new List<Obstacle>();

            for (int i = 0; i < obstacles.Count; i++)
            {
                // Measure distance between obstacle and moving entity and check if distance is shorter than detection box length.
                double distance = (obstacles[i].Pos - me.Pos).Length();
                if (distance <= dBoxLength) taggedObstacles.Add(new Obstacle(obstacles[i]) ); // Add a copy of obstacle to list, to avoid altering original during calculations.                
            }

            // Return null if no obstacle in collisionbox
            return taggedObstacles.Count < 1 ? null : taggedObstacles;
        }

        // TODO: Move all node related stuff to Level class.
        private void GenerateAllNodes()
        {
            for (int iHor = 1; iHor < StaticParameters.MaxHorizontalNodes; iHor++)
            {
                for (int iVer = 1; iVer < StaticParameters.MaxVerticalNodes; iVer++)
                {
                    string key = $"{iHor}_{iVer}";

                    AllNodes.Add(   key, 
                                    new Node(
                                        key,
                                        new Vector2D(
                                            iHor * StaticParameters.DefaultNodeDistance,    // Pos.X
                                            iVer * StaticParameters.DefaultNodeDistance)    // Pos.Y
                                        )
                    );
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graph"></param>
        private void GenerateBidirectionalEdges(Graph graph)
        {
            // Looping in this way to make neighbour selection more convenient.
            for (int iHor = 1; iHor < StaticParameters.MaxHorizontalNodes; iHor++)
            {
                for (int iVer = 1; iVer < StaticParameters.MaxVerticalNodes; iVer++)
                {
                    Vector2D tempV = new Vector2D(iHor-1, iVer-1);
                    string nodeKey = $"{iHor}_{iVer}";

                    if (AllNodes[nodeKey].Blocked) continue;

                    // Node is not blocked, so node is usable in the graph.
                    graph.AddNode(nodeKey, AllNodes[nodeKey]);

                    /// Default neighbours(B), of current Node(N):
                    /// X X X X X X
                    /// X B B B X X
                    /// X B N B X X
                    /// X B B B X X
                    /// X X X X X X

                    for(int iH = 0; iH < 2; iH++)
                    {
                        for (int iV = 0; iV < 2; iV++)
                        {
                            string bKey = $"{tempV.X}_{tempV.Y}";

                            if (bKey == nodeKey)
                            {
                                tempV.Y++;
                                continue; // We don't need an edge that loops back to the same node.
                            }

                            // If the Node is valid, add new edge.
                            if (AllNodes.ContainsKey(bKey))
                            {
                                Edge e = new Edge(  from: nodeKey,
                                                    to: bKey,
                                                    cost: graph.CalcEdgeCost(   AllNodes[nodeKey],
                                                                                AllNodes[bKey]  )
                                                    );
                                bool succesA = AllNodes[nodeKey].AddAdjacentEdge(e);
                                bool succesB = AllNodes[bKey].AddAdjacentEdge(e);
                                if (!succesA && !succesB) e = null; // To help out the garbage collector a bit.(Though it may not be referenced, it does reference 2 Nodes) Edges that already exist in reverse order are not needed.
                            }

                            tempV.Y++;
                        }

                        tempV.X++;
                    }
                    
                }
            }
        }

        /// <summary>
        /// Smart find of NodeID making use of the constant 'DefaultNodeDistance' and the AllNodes key standard of X_Y.
        /// Using the rounding function, nodes that are less than half the 'DefaultNodeDistance' removed from the search area, are also added.
        /// This also makes sure the double values are whole numbers, safe to be used as the dictionary key-string.
        /// </summary>
        /// <param name="tl">TopLeft corner of search area</param>
        /// <param name="br">BottomRight corner of search area</param>
        /// <returns> List<string> with NodeIDs </returns>
        public List<string> FindNodeIDsInArea(Vector2D tl, Vector2D br)
        {
            List<string> nodesIDs = new List<string>();

            // This gives us the minimum 'X_Y' dictionary key.
            Vector2D minNodeV = new Vector2D( x: Math.Round(tl.X / StaticParameters.DefaultNodeDistance),
                                              y: Math.Round(tl.Y / StaticParameters.DefaultNodeDistance)
                                             );
            // This gives us the maximum 'X_Y' dictionary key.
            Vector2D maxNodeV = new Vector2D( x: Math.Round(br.X / StaticParameters.DefaultNodeDistance),
                                              y: Math.Round(br.Y / StaticParameters.DefaultNodeDistance)
                                             );
            
            // Now loop between min/max X and min/max Y.
            for(int iX = (int)minNodeV.X; iX <= (int)maxNodeV.X; iX++)
            {
                for(int iY = (int)minNodeV.Y; iY <= (int)maxNodeV.Y; iY++)
                {
                    nodesIDs.Add($"{iX}_{iY}");
                }
            }

            return nodesIDs;
        }
    }
}
