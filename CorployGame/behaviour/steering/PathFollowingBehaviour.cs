using CorployGame.entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorployGame.behaviour.steering
{
    class PathFollowingBehaviour : SteeringBehaviour
    {
        List<Vector2D> Path;
        int CurrentPoint; // Index of current point in path list.
        int LastPoint; // Index of last point in path list.
        SeekBehaviour Seek;
        ArriveBehaviour Arrive;
        public bool IsPatroling; // Note: Patroling repeats path from starting point. It does not reverse Path order.

        public PathFollowingBehaviour(Vehicle me) : this(me, new List<Vector2D>()) { }

        public PathFollowingBehaviour(Vehicle me, List<Vector2D> path) : base(me)
        {
            SetPath(path);
            IsPatroling = false;
        }

        public override Vector2D Calculate()
        {
            // Pathfollowing works through Seek or Arrive and should either not put out a force of it's own.
            // Or not allow neither Seek nor arrive behaviour to be active and use internal Seek/Arrive behaviours.
            // Otherwise it will likely apply the force twice when calculating cumilative steeringforce.
            // I opted for the first option.
            Vector2D steeringForce = new Vector2D(0, 0);

            // Skip if no path is set.
            if (Path == default || Path.Count < 1) return steeringForce;

            double distance = (Path[CurrentPoint] - ME.Pos).Length();

            // If Moving Entity is already on top of their current target.
            if(distance < (ME.GetRadius() / 2))
            {
                // Note: might turn into switch case.
                // If last point in path is reached, check for patroling.
                if(CurrentPoint == LastPoint)
                {
                    if(IsPatroling)
                    {
                        Seek.UpdateTargetPos(Path[0]); // Set Seek back to first point to repeat path.
                        CurrentPoint = 0;
                    }
                    else
                    {
                        ME.SBS.SeekOFF();
                        Seek = null;
                        ME.SBS.ArriveOFF();
                        Arrive = null;
                        // Note: Yet to decide if this behaviour should disable itself at end of Path or leave that to the class using this Behaviour.
                    }
                }
                // If last point is next target.
                else if(CurrentPoint == LastPoint-1)
                {
                    if(IsPatroling)
                    {
                        Seek.UpdateTargetPos(Path[LastPoint]);
                    }
                    else
                    {
                        // Switch to Arrive.
                        ME.SBS.SeekOFF();
                        Seek = null;
                        Arrive = ME.SBS.ArriveON();
                        Arrive.UpdateTargetPos(Path[LastPoint]);
                    }
                    CurrentPoint = LastPoint;
                }
                // All other cases
                else
                {
                    Seek.UpdateTargetPos(Path[CurrentPoint + 1]);
                    CurrentPoint++;
                }
            }

            return steeringForce;
        }

        public void SetPath (List<Vector2D> newPath)
        {
            Path = newPath;
            CurrentPoint = 0;
            LastPoint = Path.Count - 1;
            Seek = ME.SBS.SeekON();
            if (Path.Count > 0) Seek.UpdateTargetPos(Path[CurrentPoint]);
        }
    }
}
