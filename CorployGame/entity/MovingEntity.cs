using CorployGame.behaviour.steering;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorployGame.entity
{
    abstract class MovingEntity : BaseGameEntity
    {
        public Vector2D Velocity { get; set; }
        public Vector2D Heading { get; set; }
        public Vector2D Side { get; set; }
        public float Mass { get; set; }
        public float MaxSpeed { get; set; }

        public SteeringBehaviour SB { get; set; }

        public MovingEntity(Vector2D pos, World w, Texture2D t) : base(pos, w, t)
        {
            Mass = 30;
            MaxSpeed = 150;
            Velocity = new Vector2D();
            Heading = new Vector2D();
            Side = new Vector2D();
        }

        public override void Update(float timeElapsed)
        {
            // calculate the combined force from each steering behavior in the vehicle's list
            Vector2D steeringForce = SB.Calculate();

            // acceleration = force / mass
            Vector2D acceleration = steeringForce.divide(Mass);

            // update velocity
            Velocity.Add(acceleration.Multiply(timeElapsed));

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
        }

        public override string ToString()
        {
            return String.Format("{0}", Velocity);
        }
    }
}
