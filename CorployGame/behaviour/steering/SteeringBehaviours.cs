using CorployGame.entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorployGame.behaviour.steering
{
    public enum STEERINGBEHAVIOUR
    {
        Seek = 1,
        Arrive = 2,
        ObstacleAvoidance = 3,
        PathFollowing = 4
    }

    class SteeringBehaviours
    {
        // Don't confuse this class with the singular 'SeekBehaviour' abstract class.
        // This class serves to consolidate all the steringbehaviours the entity has available and calculate the combined movement force.
        Vehicle Vehicle;
        Vector2D SteeringForce;

        // Behaviours
        SeekBehaviour Seek;
        ArriveBehaviour Arrive;
        ObstacleAvoidanceBehaviour ObstacleAvoidance;
        PathFollowingBehaviour PathFollowing;

        // Booleans for active behaviours
        public bool SeekIsOn;
        public bool ArriveIsOn;
        public bool ObstacleAvoidanceIsOn;
        public bool PathFollowingIsOn;

        public SteeringBehaviours(Vehicle vehicle)
        {
            Vehicle = vehicle;

            SeekIsOn = false;
            ArriveIsOn = false;
            ObstacleAvoidanceIsOn = false;
            PathFollowingIsOn = false;
        }

        public Vector2D Calculate()
        {
            // Reset the force.
            SteeringForce = new Vector2D(0,0);

            Vector2D force = new Vector2D(0, 0);

            if (ObstacleAvoidanceIsOn)
            {
                force = ObstacleAvoidance.Calculate();

                if (!AccumilatedForce(force)) return SteeringForce; // Max Force already reached, no need to try and add more.
            }

            if (PathFollowingIsOn)
            {
                force = PathFollowing.Calculate();

                if (!AccumilatedForce(force)) return SteeringForce; // Max Force already reached, no need to try and add more.
            }

            if (SeekIsOn)
            {
                force = Seek.Calculate();

                if (!AccumilatedForce(force)) return SteeringForce; // Max Force already reached, no need to try and add more.
            }

            if (ArriveIsOn)
            {
                force = Arrive.Calculate();

                if (!AccumilatedForce(force)) return SteeringForce; // Max Force already reached, no need to try and add more.
            }

            // Return sum of all forces.
            return SteeringForce;
        }

        public bool AccumilatedForce(Vector2D forceToAdd)
        {
            //calculate how much steering force the vehicle has used so far.
            double MagnitudeSoFar = SteeringForce.Length();

            //calculate how much steering force remains to be used by the vehicle.
            double magnitudeRemaining = Vehicle.MaxSpeed - MagnitudeSoFar;

            // return false if there is no more force left.
            if (magnitudeRemaining <= 0.0) return false;

            //calculate the magnitude of the force we want to add
            double magnitudeToAdd = forceToAdd.Length();

            //if the magnitude of the sum of forceToAdd and the running total
            //does not exceed the maximum force available to this vehicle, just
            //add together. Otherwise add as much of the ForceToAdd vector as
            //possible without going over the max.
            if(magnitudeToAdd < magnitudeRemaining)
            {
                SteeringForce += forceToAdd;
            }
            else
            {
                SteeringForce += (forceToAdd.Normalize() * magnitudeRemaining);
            }

            return true;
        }

        public void SetTarget(Vector2D target)
        {
            if (Seek != null && SeekIsOn) Seek.UpdateTargetPos(target);
            if (Arrive != null && ArriveIsOn) Arrive.UpdateTargetPos(target);
        }

        public void SetPath(List<Vector2D> path)
        {
            if (PathFollowing != null && PathFollowingIsOn) PathFollowing.SetPath(path);
        }

        // Add and activate behaviours.
        public SeekBehaviour SeekON()
        {
            if (SeekIsOn) return Seek;
            Seek = new SeekBehaviour(Vehicle);
            SeekIsOn = true;
            return Seek;
        }

        public ArriveBehaviour ArriveON()
        {
            if (ArriveIsOn) return Arrive;
            Arrive = new ArriveBehaviour(Vehicle);
            ArriveIsOn = true;
            return Arrive;
        }

        public ObstacleAvoidanceBehaviour ObstacleAvoidanceON()
        {
            if (ObstacleAvoidanceIsOn) return ObstacleAvoidance;
            ObstacleAvoidance = new ObstacleAvoidanceBehaviour(Vehicle);
            ObstacleAvoidanceIsOn = true;
            return ObstacleAvoidance;
        }

        public PathFollowingBehaviour PathFollowingON()
        {
            if (PathFollowingIsOn) return PathFollowing;
            PathFollowing = new PathFollowingBehaviour(Vehicle);
            PathFollowingIsOn = true;
            return PathFollowing;
        }

        // Remove and deactivate behaviours.
        public void AllOFF()
        {
            SeekOFF();
            ArriveOFF();
            ObstacleAvoidanceOFF();
            PathFollowingOFF();
        }

        public void SeekOFF()
        {
            Seek = null;
            SeekIsOn = false;
        }

        public void ArriveOFF()
        {
            Arrive = null;
            ArriveIsOn = false;
        }

        public void ObstacleAvoidanceOFF()
        {
            ObstacleAvoidance = null;
            ObstacleAvoidanceIsOn = false;
        }

        public void PathFollowingOFF()
        {
            PathFollowing = null;
            PathFollowingIsOn = false;
        }
    }
}
