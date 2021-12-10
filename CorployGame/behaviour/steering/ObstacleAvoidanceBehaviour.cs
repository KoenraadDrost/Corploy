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
        public double Margin { get; set; }

        public ObstacleAvoidanceBehaviour(MovingEntity me) : base(me)
        {
            DBoxWidth = me.Texture.Width;
            Margin = DBoxWidth / 2;
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

            Console.WriteLine("detection test1");

            GenerateLocalUniverse(ref localObstacles);


            //TODO: Replace with propper vector later.
            return new Vector2D(0, 0);
        }

        private void GenerateLocalUniverse(ref List<Square> obstList)
        {
            Vector2D tempMEpos = new Vector2D(ME.Pos);

            // Rotate all objects in the opposite direction of Moving Entity orientation, so that the orientation is now 0 degrees.
            Matrix2D rMat = Matrix2D.RotateMatrix(-ME.Orientation);
            tempMEpos = rMat * tempMEpos;

            for(int i = 0; i < obstList.Count; i++)
            {
                obstList[i].RotateSquare(-ME.Orientation);
            }

            // TODO: Might be safe to use TempMEpos directly with flipped X.
            // Move temporary ME position to (x=0, y=0) and move all obstacles with same amount.
            // As y is already 0, we only need to update all the x-values.
            Vector2D changeV = new Vector2D(-ME.Pos.X, 0);
            tempMEpos.X = 0;

            for (int i = 0; i < obstList.Count; i++)
            {
                obstList[i].MoveSquare(changeV);
            }

            // Then check list for: Square Center Y - 1/2 * Heigth - Margin > 0, then ignore (outside of collision box Y)
            // Then go through remaining squares and check each corner, monitoring which corner is closest.
            // When closest corner is determined, calculate vector push to avoid collision from that corner preferable least deviating from current heading.

            // Calculate clockwwise rotation angle of ME

            // Use angle as counter-clockwise transformation on objects.

        }


    }
}
