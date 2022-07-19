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

        // TODO: Clean op test variables
        // temp test variables
        AiAgent TestAgent { get; set; }

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
            LevelGenerator lGen = new LevelGenerator();
            CurrentLevel = lGen.GenerateLevel(this);

            // Entities
            // Current player target
            Target = new Vehicle(new Vector2D(200, 100), this, new Texture2D(GD, 10, 10));
            Target.VColor = Color.Red;
            Target.UpdateTexture();

            Vector2D playerSpawn = StaticParameters.TopLeftNodePos;
            playerSpawn.X += StaticParameters.DefaultNodeDistance * 2;
            PlayerEntity = new PlayerAgent(new Vector2D(100, 100), this, new Texture2D(GD, 16, 16));
            PlayerEntity.InitializeAvatar();

            CurrentLevel.Initialize();

            // TODO: Remove test stuff.
            // Temp test
            //Vector2D agentSpawn = new Vector2D(600, 400);
            //Texture2D agentTexture = new Texture2D(GD, 16, 16);
            //Color aColor = Color.BlanchedAlmond;
            //Color[] data = new Color[agentTexture.Width * agentTexture.Height];
            //for (int i = 0; i < (agentTexture.Width * agentTexture.Height); ++i) data[i] = aColor;
            //agentTexture.SetData<Color>(data);

            //TestAgent = new AiAgent(agentSpawn, this, agentTexture);
            //TestAgent.Goal = new Vector2D(300, 350);
            //TestAgent.DefaultSteeringBehaviour = STEERINGBEHAVIOUR.Seek;
            //TestAgent.ShouldSpawn = true;


            //Vehicle v = new Vehicle(new Vector2D(50, 50), this, new Texture2D(GD, 16, 16));
            //v.SBS.SeekON();
            ////v.SBS.ArriveON();
            //v.SBS.ObstacleAvoidanceON();
            //v.SBS.SetTarget(Target.Pos);
            //v.VColor = Color.Blue;
            //v.UpdateTexture();
            //entities.Add(v);

            // Obstacles
            //Obstacle o1 = new Obstacle(new Vector2D(300, 300), this, new Texture2D(GD, 40, 40));
            //o1.VColor = Color.Gray;
            //o1.UpdateTexture();
            //obstacles.Add(o1);

            //TODO: remove tests
            //List<string> bNodes = o1.GetCoveredNodes();

            //foreach (string bnode in bNodes)
            //{
            //    AllNodes[bnode].Blocked = true;
            //}

            //Graph g = new Graph();
            //g.GenerateBidirectionalEdges(AllNodes);
            //List<Node> DijkstraNodes = g.AStar(     start: AllNodes["8_16"],
            //                                        end: AllNodes["22_14"]);

            //List<Vector2D> path = new List<Vector2D>();

            //foreach(Node n in DijkstraNodes)
            //{
            //    path.Add(n.Pos);
            //}

            //Vehicle v2 = new Vehicle(new Vector2D(50, 50), this, new Texture2D(GD, 16, 16));
            ////v.SBS.SeekOn();
            ////v.SBS.ArriveON();
            //v2.SBS.ObstacleAvoidanceON();
            //v2.SBS.PathFollowingON();
            //v2.SBS.SetPath(path);
            //v2.VColor = Color.Purple;
            //v2.UpdateTexture();
            //entities.Add(v2);


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

            CurrentLevel.Update(gameTime);

            // TODO: change the way target works so we don't get this possibility.
            Target.Update((float)ElapsedTime);

            //TODO:  remove test
            //TestAgent.Update((float)ElapsedTime);
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

            CurrentLevel.Draw(spriteBatch, gameTime);

            //TODO:  remove test
            //Console.WriteLine("TestAgent testprint = " + TestAgent.HasSpawned);
            //if (TestAgent.HasSpawned) TestAgent.Draw(spriteBatch, gameTime);
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
