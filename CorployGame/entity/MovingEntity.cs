using CorployGame.behaviour.steering;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorployGame.entity
{
    abstract class MovingEntity : BaseGameEntity
    {
        public Vector2D Velocity { get; set; } // Current speed and direction.
        public Vector2D Heading { get; set; }
        public Vector2D Side { get; set; }
        public float Mass { get; set; }
        public float MaxSpeed { get; set; }
        public double MaxForce { get; set; }
        public double MaxTurnRate { get; set; }

        public Vector2D SteeringForce { get; set; }

        public MovingEntity(Vector2D pos, World w, Texture2D t) : base(pos, w, t)
        {
            Mass = 30;
            MaxSpeed = 150;
            MaxForce = 300;
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
            Pos = Pos.Add(Velocity.Multiply(timeElapsed));

            // update the heading if velocity is greater than 0.
            if (Velocity.LengthSquared() > 0.001)
            {
                Heading = Velocity.Normalize();

                Side = Heading.Perpendicular();
            }

            // TODO: Remove print statement later.
            //Console.WriteLine($"speed:{acceleration}");
        }

        public override string ToString()
        {
            return String.Format("{0}", Velocity);
        }
    }
}
