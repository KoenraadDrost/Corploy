using CorployGame.entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorployGame.behaviour.steering
{
    class SeekBehaviour : SteeringBehaviour
    {
        Vector2D TargetPos; // Ease of reference
        public SeekBehaviour(MovingEntity me) : this( me, new Vector2D(me.MyWorld.Height /2, me.MyWorld.Width /2) ) { }

        public SeekBehaviour(MovingEntity me, Vector2D targetPos) : base(me)
        {
            TargetPos = targetPos;
        }

        public override Vector2D Calculate()
        {
            Vector2D desiredVelocity = (TargetPos - ME.Pos).Normalize() * ME.MaxSpeed;

            return desiredVelocity - ME.Velocity;
        }

        public void UpdateTargetPos (Vector2D targetPos)
        {
            TargetPos = targetPos;
        }
    }
}
