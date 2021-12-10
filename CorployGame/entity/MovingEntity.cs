using CorployGame.behaviour.steering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorployGame.entity
{
    abstract class MovingEntity : BaseGameEntity
    {
        public Vector2D Velocity { get; set; } // Current length of travel and direction represented by a x.y coördinate relative to entity position. Value is dependend on elapsed time.
        public Vector2D Heading { get; set; }
        public Vector2D Side { get; set; }
        public float Orientation { get; set; } // Angle of heading relative to horizon
        public float Mass { get; set; }
        public float Speed { get; set; } // Velocity length per second since last update. ( pixels/second )
        public float MaxSpeed { get; set; }
        public double MaxForce { get; set; }
        public double MaxTurnRate { get; set; }
        public Vector2D SteeringForce { get; set; }

        public MovingEntity(Vector2D pos, World w, Texture2D t) : base(pos, w, t)
        {
            int speedfactor = 200;

            Orientation = 0f;
            Mass = 3;
            Speed = 0;
            MaxSpeed = 150f * speedfactor;
            MaxForce = 300.0 * speedfactor;
            Velocity = new Vector2D();
            Heading = new Vector2D();
            Side = new Vector2D();
        }

        public override void Update(float timeElapsed)
        {
            // acceleration = force / mass
            Vector2D acceleration = SteeringForce.divide(Mass);

            // update velocity
            Velocity += acceleration * timeElapsed;

            // make sure vehicle does not exceed maximum velocity
            Velocity.truncate(MaxSpeed);

            // update the position
            Pos += Velocity * timeElapsed;

            // update the heading if velocity is greater than 0.
            if (Velocity.LengthSquared() > 0.001)
            {
                Heading = Velocity.Normalize();
                Side = Heading.PerpendicularClockwise();
                Orientation = Velocity.GetAngleDegrees();
            }

            // Update Speed (Velocity.Length/second)
            Speed = (float)Velocity.Length() * (1 / timeElapsed);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Vector2 position = new Vector2((float)Pos.X, (float)Pos.Y);

            float rotation = Orientation * ((float)Math.PI / 180);
            //_spriteBatch.Draw(
            //    Texture2D texture,
            //    Vector2 position,
            //    Square (is nullable),
            //    Color,
            //    Rotation,
            //    Origin(Define an origin point relative to the texture. Origin wil snap to the given 'position'. If not used, or set to(0,0), Drawing will start with 'position' as top-left corner of texture.) ,
            //    Vector2 scale,
            //    SpriteEffects effects,
            //    0f
            //);
            spriteBatch.Draw(
                Texture,
                position,
                null,
                Color.White,
                rotation,
                GetTextureOrigin(),
                Vector2.One,
                SpriteEffects.None,
                0f
            );
        }

        public override string ToString()
        {
            return String.Format("{0}", Velocity);
        }
    }
}
