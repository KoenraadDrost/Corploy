using CorployGame.entity;
using CorployGame.util;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorployGame.behaviour.steering
{
    class ObstacleAvoidanceBehaviour : SteeringBehaviour
    {
        public double DBoxLength { get; set; }
        public double DBoxWidth { get; set; }

        public ObstacleAvoidanceBehaviour(MovingEntity me) : base(me)
        {
            DBoxWidth = me.Texture.Width;
            UpdateDBoxLength();
        }

        private void UpdateDBoxLength ()
        {
            // Split up to avoid divide by zero error.
            DBoxLength = ME.Texture.Height * 2;
            if (ME.MaxSpeed > 0.001)
            {
                DBoxLength += (ME.Speed / ME.MaxSpeed) * ME.Texture.Height;
            }
        }

        public override Vector2D Calculate()
        {
            UpdateDBoxLength();

            List<Square> localObstacles = ME.MyWorld.TagObstaclesInCollisionRange(ME, DBoxLength);
            // If there are no local objects to collide with, return blank vector to avoid calculation errors with "NULL".
            if(localObstacles == null || localObstacles.Count < 1) return new Vector2D(0, 0);




            //TODO: Replace with propper vector later.
            return new Vector2D(0, 0);
        }

        private void GenerateLocalUniverse(List<Square> obstList)
        {


            // Change obstacles to squares and use matrix.transform on all squares instead of rotating, after ME is set as origin(0,0)
            // Then check list for: Square Center Y - 1/2 * Heigth - Margin > 0, then ignore (outside of collision box Y)
            // Then go through remaining squares and check each corner, monitoring which corner is closest.
            // When closest corner is determined, calculate vector push to avoid collision from that corner preferable least deviating from current heading.

            // Calculate clockwwise rotation angle of ME

            // Use angle as counter-clockwise transformation on objects.

        }


    }
}
