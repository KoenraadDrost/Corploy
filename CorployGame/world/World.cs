using CorployGame.behaviour.steering;
using CorployGame.entity;
using CorployGame.util;
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
        public List<MovingEntity> entities = new List<MovingEntity>();
        public List<Obstacle> obstacles = new List<Obstacle>();
        public Vehicle Target { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public GraphicsDevice GD { get; set; }

        public double LastUpdateTime { get; set; }

        public World(int w, int h, GraphicsDevice gd)
        {
            Width = w;
            Height = h;
            GD = gd;
            LastUpdateTime = 0;
        }

        public void Update (GameTime gameTime)
        {
            double ElapsedTime = gameTime.ElapsedGameTime.TotalSeconds;
            //TODO: Remove debug line later
            //Console.WriteLine(ElapsedTime);

            for (int i = 0; i < entities.Count; i++)
            {
                // TODO: might want to simplify this later.
                entities[i].Update((float)ElapsedTime);

                //TODO: Remove debug line later.
                //Console.WriteLine($"Speed = {entities[i].Speed}");
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
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].Draw(spriteBatch, gameTime);
            }

            for (int i = 0; i < obstacles.Count; i++)
            {
                obstacles[i].Draw(spriteBatch, gameTime);
            }

            Target.Draw(spriteBatch, gameTime);
        }

        public void Populate()
        {
            // Entities
            Target = new Vehicle( new Vector2D(200, 100), this, new Texture2D(GD, 10, 10) );
            Target.VColor = Color.Red;
            Target.UpdateTexture();

            Vehicle v = new Vehicle( new Vector2D(100, 100), this, new Texture2D(GD, 16, 16) );
            v.SBS.SeekOn();
            //v.SBS.ArriveON();
            v.SBS.ObstacleAvoidanceON();
            v.SBS.SetTarget(Target.Pos);
            v.VColor = Color.Blue;
            v.UpdateTexture();
            entities.Add(v);

            // Load Texture of player
            FileStream fileStream = new FileStream("D:/GitHub_Repos/Corploy/CorployGame/Content/PlayerAgent 12-02-2021 04-44-13.png", FileMode.Open);            
            v.Texture = Texture2D.FromStream(GD, fileStream);
            fileStream.Dispose();

            // Obstacles
            Obstacle o1 = new Obstacle( new Vector2D(300, 300), this, new Texture2D(GD, 30, 30) );
            o1.VColor = Color.Gray;
            o1.UpdateTexture();
            obstacles.Add(o1);

            // TODO: Remove test
            Square s1 = new Square(20);
            Console.WriteLine($"Square Write Test: {s1}");

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="me"></param>
        /// <param name="dBoxLength"></param>
        /// <returns></returns>
        public List<Square> TagObstaclesInCollisionRange(MovingEntity me, double dBoxLength)
        {
            List<Square> taggedObstacles = new List<Square>();

            for(int i = 0; i < obstacles.Count; i++)
            {
                // Measure distance between obstacle and moving entity and check if distance is shorter than detection box length.
                double distance = (obstacles[i].Pos - me.Pos).Length();
                if (distance <= dBoxLength) taggedObstacles.Add( new Square(obstacles[i].Pos, Height) ); // Add a copy of obstacle to list, to avoid altering original during calculations.                
            }

            // Return null if no obstacle in collisionbox
            return taggedObstacles.Count < 1 ? null : taggedObstacles;
        }
    }
}
