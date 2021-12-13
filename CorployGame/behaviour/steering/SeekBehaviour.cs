using CorployGame.entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorployGame.behaviour.steering
{
    class SeekBehaviour : SteeringBehaviour
    {
        MovingEntity Target; // For actively moving targets.
        Vector2D TargetPos; // Usefull for waypoints independend of world.
        public SeekBehaviour(MovingEntity me) : this( me, new Vector2D(me.MyWorld.Height /2, me.MyWorld.Width /2) ) { }

        public SeekBehaviour(MovingEntity me, Vector2D targetPos) : base(me)
        {
            TargetPos = targetPos;
        }

        public SeekBehaviour(MovingEntity me, MovingEntity target) : base(me)
        {
            Target = target;
            UpdateTargetPos(Target.Pos);
        }

        public override Vector2D Calculate()
        {
            //Vector2D targetPos = ME.MyWorld.Target.Pos;

            if (Target != null) UpdateTargetPos(Target.Pos); // Get latest position of active target, if a moving target is set.

            Vector2D desiredVelocity = (TargetPos - ME.Pos).Normalize() * ME.MaxSpeed;

            //TODO: Remove later
            Console.WriteLine($"desiredvelocity Seek: {desiredVelocity - ME.Velocity}");

            return desiredVelocity - ME.Velocity;
        }

        public void UpdateTargetPos (Vector2D targetPos)
        {
            TargetPos = targetPos;
        }
    }
}
