using CorployGame.entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorployGame.behaviour.steering
{
    class SeekBehaviour : SteeringBehaviour
    {
        public SeekBehaviour(MovingEntity me) : base(me) { }

        public override Vector2D Calculate()
        {
            Vector2D targetPos = ME.MyWorld.Target.Pos;
            Vector2D desiredVelocity = (targetPos - ME.Pos).Normalize() * ME.MaxSpeed;

            return desiredVelocity - ME.Velocity;

        }
    }
}
