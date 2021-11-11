using CorployGame.entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorployGame.behaviour.steering
{
    public enum DecelerationSpeed
    {
        slow = 3,
        normal = 2,
        fast = 1
    }
    class ArriveBehaviour : SteeringBehaviour
    {
        // How rough or smooth the deceleration should be.
        DecelerationSpeed DecelerationSpd;
        // Universal constant factor to calculate all deceleration with.
        double DecelerationFactor = 0.3;

        public ArriveBehaviour(MovingEntity me) : this(me, DecelerationSpeed.normal)
        {

        }

        public ArriveBehaviour(MovingEntity me, DecelerationSpeed ds) : base(me)
        {
            DecelerationSpd = ds;
        }

        public override Vector2D Calculate()
        {
            // Get relative position to target
            Vector2D toTarget = ME.MyWorld.Target.Pos - ME.Pos;

            // Calculate distance to target
            double dist = toTarget.Length();

            if(dist > 0)
            {
                // Calculate the speed required to reach the target given the desired deceleration. 40 / 0.3 
                double speed = dist / ( (double)DecelerationSpd * DecelerationFactor);

                // Limit speed by moving entity 's maxspeed.
                speed %= ME.MaxSpeed;

                Vector2D desiredVelocity = toTarget * speed / dist;

                return (desiredVelocity - ME.Velocity);
            }

            // If target has been reached, set velocity to 0;
            return new Vector2D(0, 0);
        }
    }
}
