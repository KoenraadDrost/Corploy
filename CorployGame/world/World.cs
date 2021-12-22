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
        public List<Vehicle> entities = new List<Vehicle>();
        public List<Obstacle> obstacles = new List<Obstacle>();

        public PlayerAgent PlayerEntity { get; set; }
        public List<Node> AllNodes { get; set; }
        public int DefaultNodeDistance { get; set; }
        public Level CurrentLevel { get; set; }
        public Vehicle Target { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public GraphicsDevice GD { get; set; }

        public double LastUpdateTime { get; set; }

        public World(int w, int h, GraphicsDevice gd)
        {
            DefaultNodeDistance = 20;
            AllNodes = new List<Node>();
            GenerateAllNodes();

            CurrentLevel = LevelGenerator.GenerateLevel(AllNodes, this);

            Width = w;
            Height = h;
            GD = gd;
            LastUpdateTime = 0;
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

        public void Populate()
        {
            // Entities
            Target = new Vehicle( new Vector2D(200, 100), this, new Texture2D(GD, 10, 10) );
            Target.VColor = Color.Red;
            Target.UpdateTexture();

            PlayerEntity = new PlayerAgent(new Vector2D(100, 100), this, new Texture2D(GD, 16, 16));
            PlayerEntity.InitializeAvatar();

            Vehicle v = new Vehicle( new Vector2D(50, 50), this, new Texture2D(GD, 16, 16) );
            v.SBS.SeekOn();
            //v.SBS.ArriveON();
            v.SBS.ObstacleAvoidanceON();
            v.SBS.SetTarget(Target.Pos);
            v.VColor = Color.Blue;
            v.UpdateTexture();
            entities.Add(v);

            // Obstacles
            Obstacle o1 = new Obstacle( new Vector2D(300, 300), this, new Texture2D(GD, 30, 30) );
            o1.VColor = Color.Gray;
            o1.UpdateTexture();
            obstacles.Add(o1);
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

        private void GenerateAllNodes()
        {
            int MaxHorizontalNodes = Width / DefaultNodeDistance - 1; // No nodes on border of the level, with counter starting at 1 this results in -2, for borders on both sides.
            int MaxVerticalNodes = Height / DefaultNodeDistance - 1;
            for (int iVer = 1; iVer < MaxVerticalNodes; iVer++)
            {
                for (int iHor = 1; iHor < MaxHorizontalNodes; iHor++)
                {
                    AllNodes.Add( new Node(
                        0,
                        new Vector2D(
                            iHor * DefaultNodeDistance,
                            iVer * DefaultNodeDistance)
                        )
                    );
                }
            }
        }
    }
}
