using CorployGame.behaviour.steering;
using CorployGame.world;
using CorployGame.world.navigation;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorployGame.entity
{
    class AiAgent : Agent
    {
        public Vector2D Goal;
        public STEERINGBEHAVIOUR DefaultSteeringBehaviour;
        public List<Vector2D> Path = new List<Vector2D>();

        public AiAgent(Vector2D spawnPos, World w, Texture2D t2d, Vector2D goal, STEERINGBEHAVIOUR defSteering) : base(spawnPos, w, t2d)
        {
            Goal = goal;
            DefaultSteeringBehaviour = defSteering;
        }
        public AiAgent(Vector2D spawnPos, World w, Texture2D t2d, Vector2D goal) : this(spawnPos, w, t2d, goal, STEERINGBEHAVIOUR.PathFollowing)
        {
            Goal = goal;
        }
        public AiAgent(Vector2D spawnPos, World w, Texture2D t2d) : this(spawnPos, w, t2d, StaticParameters.BottomRightNodePos)
        {

        }

        public override void InitializeAvatar()
        {
            base.InitializeAvatar();

            Avatar.SBS.ObstacleAvoidanceON();
            Avatar.SBS.SetTarget(Goal);

            switch(DefaultSteeringBehaviour)
            {
                case STEERINGBEHAVIOUR.Seek:
                    Avatar.SBS.SeekON();
                    Avatar.SBS.SetTarget(Goal);
                    break;
                case STEERINGBEHAVIOUR.Arrive:
                    Avatar.SBS.ArriveON();
                    break;
                case STEERINGBEHAVIOUR.PathFollowing:
                    Avatar.SBS.PathFollowingON();
                    Avatar.SBS.SetPath(Path);
                    break;
                default:
                    break;
            }
        }

        public override void Update(float timeElapsed)
        {
            // TODO: will likely need to change later.
            // Note: redundant check? See base.InitializeAvater().
            if (!MovementEnabled && Avatar != default) MovementEnabled = true;

            // TODO: Replace with behaviour logic later.
            if (Avatar != null)
            {
                //Console.WriteLine("avatar speed:" + Avatar.Speed);
                if (Avatar.SBS.ArriveIsOn) ShouldDespawn = true;
            }

            base.Update(timeElapsed);
        }
    }
}
