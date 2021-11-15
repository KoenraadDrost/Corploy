using CorployGame.behaviour.steering;
using CorployGame.entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorployGame
{
    class World
    {
        public List<Vehicle> entities = new List<Vehicle>();
        public Vehicle Target { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public GraphicsDevice GD { get; set; }

        public World(int w, int h, GraphicsDevice gd)
        {
            Width = w;
            Height = h;
            GD = gd;
        }

        public void Populate()
        {
            Target = new Vehicle(new Vector2D(100, 60), this, new Texture2D(GD, 10, 10));
            Target.VColor = Color.Red;
            Target.UpdateTexture();
            Target.Pos = new Vector2D(200, 200);

            Vehicle v = new Vehicle(new Vector2D(100, 100), this, new Texture2D(GD, 20, 20));
            v.SBS.SeekOn();
            v.SBS.SetTarget(Target.Pos);
            v.VColor = Color.Blue;
            v.UpdateTexture();
            entities.Add(v);
        }

        public void Update (GameTime gameTime)
        {
            //TODO: Remove later
            //Console.WriteLine(gameTime.ElapsedGameTime.TotalSeconds);

            for (int i = 0; i < entities.Count; i++)
            {
                // TODO: might want to simplify this later.
                entities[i].Update((float)gameTime.ElapsedGameTime.TotalSeconds * 50);
            }

            // TODO: change the way target works so we don't get this possibility.
            //Target.Update((float)gameTime.ElapsedGameTime.TotalSeconds * 50);
        }
    }
}
