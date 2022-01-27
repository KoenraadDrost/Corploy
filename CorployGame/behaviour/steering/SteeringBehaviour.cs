using CorployGame.entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorployGame.behaviour.steering
{
    abstract class SteeringBehaviour
    {
        public Vehicle ME { get; set; }
        public abstract Vector2D Calculate();

        public SteeringBehaviour(Vehicle me)
        {
            ME = me;
        }
    }
}
