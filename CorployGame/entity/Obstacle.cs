using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorployGame.entity
{
    class Obstacle : BaseGameEntity
    {
        public Obstacle(Vector2D pos, World w, Texture2D t) : base(pos, w, t)
        {
        }

        // copy Constructor
        public Obstacle(Obstacle o) : base(o.Pos, o.MyWorld, o.Texture)
        {
        }

        public override void Update(float delta)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Vector2 position = new Vector2((float)Pos.X, (float)Pos.Y);

            //_spriteBatch.Draw(
            //    Texture2D texture,
            //    Vector2 position,
            //    Color,
            //);
            spriteBatch.Draw(
                Texture,
                position,
                Color.White
            );
        }
    }
}
