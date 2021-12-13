using CorployGame.entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorployGame.behaviour.steering
{
    class SteeringBehaviours
    {
        // This class serves to consolidate all the steringbehaviours the entity has available and calculate the combined movement force.
        Vehicle Vehicle;
        Vector2D SteeringForce;


        // Behaviours
        SeekBehaviour Seek;
        ArriveBehaviour Arrive;
        ObstacleAvoidanceBehaviour ObstacleAvoidance;

        // Booleans for active behaviours
        bool SeekIsOn;
        bool ArriveIsOn;
        bool ObstacleAvoidanceIsOn;

        public SteeringBehaviours(Vehicle vehicle)
        {
            Vehicle = vehicle;

            SeekIsOn = false;
            ArriveIsOn = false;
            ObstacleAvoidanceIsOn = false;
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
            
            if(SeekIsOn)
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
            if (Seek != null) Seek.UpdateTargetPos(target);
        }



        // Add and activate behaviours.
        public void SeekOn()
        {
            Seek = new SeekBehaviour(Vehicle);
            SeekIsOn = true;
        }

        public void ArriveON()
        {
            Arrive = new ArriveBehaviour(Vehicle);
            ArriveIsOn = true;
        }

        public void ObstacleAvoidanceON()
        {
            ObstacleAvoidance = new ObstacleAvoidanceBehaviour(Vehicle);
            ObstacleAvoidanceIsOn = true;
        }

        // Remove and deactivate behaviours.
        public void SeekOff()
        {
            Seek = null;
            SeekIsOn = false;
        }

        public void ArriveOff()
        {
            Arrive = null;
            ArriveIsOn = false;
        }

        public void ObstacleAvoidanceOff()
        {
            ObstacleAvoidance = null;
            ObstacleAvoidanceIsOn = false;
        }
    }
}
