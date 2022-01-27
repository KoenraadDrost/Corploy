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

        Vector2D TargetPos; // Ease of reference

        public ArriveBehaviour(Vehicle me) : this(me, DecelerationSpeed.normal, me.Pos) { }
        public ArriveBehaviour(Vehicle me, DecelerationSpeed ds, Vector2D targetPos) : base(me)
        {
            DecelerationSpd = ds;
            TargetPos = targetPos;
        }

        public override Vector2D Calculate()
        {
            // Get relative position to target
            Vector2D toTarget = TargetPos - ME.Pos;

            // Calculate distance to target
            double dist = toTarget.Length();

            if(dist > 0)
            {
                // Calculate the speed required to reach the target given the desired deceleration. 40 / 0.3 
                double speed = dist / ( (double)DecelerationSpd * DecelerationFactor);

                // Limit speed by moving entity 's maxspeed.
                speed = Math.Min(speed, ME.MaxSpeed);

                Vector2D desiredVelocity = toTarget * speed / dist;

                return (desiredVelocity - ME.Velocity);
            }

            // If target has been reached, set velocity to 0;
            return new Vector2D(0, 0);
        }

        public void UpdateTargetPos(Vector2D targetPos)
        {
            TargetPos = targetPos;
        }
    }
}
