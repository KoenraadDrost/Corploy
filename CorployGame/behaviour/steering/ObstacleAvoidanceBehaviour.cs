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

        BaseGameEntity ClosestIntersectingObject;
        double DistanceToClosestIP; // Closest Intersectingpoint distance.
        Vector2D LocalPosOfClosestObstacle;

        // TODO: change later
        Transformations transformations = new Transformations();

        public ObstacleAvoidanceBehaviour(MovingEntity me) : base(me)
        {
            DBoxWidth = me.Texture.Width;
            Margin = DBoxWidth / 2;
            UpdateDBoxLength();
        }

        private void UpdateDBoxLength ()
        {
            // Split up to avoid divide by zero error.
            DBoxLength = ME.Texture.Height;
            if (ME.MaxSpeed > 0.001)
            {
                DBoxLength += (ME.Speed / ME.MaxSpeed) * ME.Texture.Height;
            }
        }

        private void UpdateDBoxWidth()
        {
            DBoxWidth = ME.GetRadius();
            Margin = DBoxWidth / 2;
        }

        public override Vector2D Calculate()
        {
            Vector2D steeringForce = new Vector2D(0, 0);
            UpdateDBoxLength();
            UpdateDBoxWidth();
            ClosestIntersectingObject = null;
            DistanceToClosestIP = double.MaxValue;

            List<Obstacle> localObstacles = ME.MyWorld.TagObstaclesInCollisionRange(ME, DBoxLength);
            // If there are no local objects to collide with, return blank vector to avoid calculation errors with "NULL".
            if(localObstacles == null || localObstacles.Count < 1) return new Vector2D(0, 0);

            Vector2D MePos = GenerateLocalUniverse(ref localObstacles);

            // Determine closest obstacle.
            for(int i = 0; i < localObstacles.Count; i++)
            {
                CheckObstacleCollision(localObstacles[i]);
            }

            // Calculating the steering force
            if(ClosestIntersectingObject != null)
            {
                // "the closer the agent is to an object, the stronger the steering force should be"
                double multiplier = 1.0 + (DBoxLength - LocalPosOfClosestObstacle.X) / DBoxLength;
                steeringForce.Y = (ClosestIntersectingObject.GetRadius() - LocalPosOfClosestObstacle.Y) * multiplier;

                // "apply a braking force proportional to the obstacle's distance from the vehicle"
                const double BreakingWeight = 0.2;

                steeringForce.X = (ClosestIntersectingObject.GetRadius() - LocalPosOfClosestObstacle.X) * BreakingWeight;
            }
            //Book uses heading and side, version. Which is currently not working on my end.
            //return transformations.VectorToWorldSpace(steeringForce, ME.Heading, ME.Side);
            Vector2D worldSteeringForce = transformations.VectorToWorldSpace(steeringForce, ME.Orientation) + new Vector2D(ME.Pos.X, 0); // Add X back to the force, as we set X to 0 during localSpace calculation.

            return worldSteeringForce;
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

        /// <summary>
        /// Makes obstacle positions relative to Moving Entity.
        /// </summary>
        /// <param name="obstList"></param>
        private Vector2D GenerateLocalUniverse(ref List<Obstacle> obstList)
        {
            Vector2D tempMEpos = new Vector2D(ME.Pos);

            // Rotate all objects in the opposite direction of Moving Entity orientation, so that the orientation is now 0 degrees.
            Matrix2D rMat = Matrix2D.RotateMatrix(-ME.Orientation);
            tempMEpos = rMat * tempMEpos;

            for (int i = 0; i < obstList.Count; i++)
            {
                obstList[i].Pos = rMat * obstList[i].Pos;
            }
            // Move temporary ME position to (x=0, y=0) and move all obstacles with same amount.
            // As y is already 0, we only need to update all the x-values.
            for (int i = 0; i < obstList.Count; i++)
            {
                obstList[i].Pos.X -= tempMEpos.X;
            }

            return tempMEpos;
        }

        private void CheckObstacleCollision (BaseGameEntity obst)
        {
            Vector2D obstPos = obst.Pos;

            // If obstacle is behind Moving Entity, ignore.
            if (obstPos.X <= 0) return;

            // If obstacle is way outside of collisionbox Y-axis, ignore.
            // 2* margin because it's for both the Moving entity's width and the margin in which it can collide with edge of obstacle.
            // Check for both positive and negative Y-axis values.(Below horizon, above horizon)
            // Simplified by forcing positive Y-axis value. Math.Abs()
            double maxPossibleCollision = obst.GetRadius() + Margin * 2;
            if (obst.Pos.Y >= 0 && Math.Abs(obstPos.Y) > maxPossibleCollision) return;

            // Obstacle is now almost certainly within the detection box.
            double cX = obst.Pos.X;
            double cY = obst.Pos.Y;

            //"we only need to calculate the sqrt part of the above equation once"
            double expandedRadius = obst.GetRadius() + ME.GetRadius();

            double SqrtPart = Math.Sqrt(expandedRadius * expandedRadius + cX * cY);

            double ip = cX - SqrtPart;

            if(ip <= 0)
            {
                ip = cX + SqrtPart;
            }

            //"test to see if this is the closest so far. If it is, keep a record of hte obstacle and its coördinates"
            if(ip < DistanceToClosestIP)
            {
                DistanceToClosestIP = ip;
                ClosestIntersectingObject = obst;
                LocalPosOfClosestObstacle = obst.Pos;
            }

        }

    }
}
