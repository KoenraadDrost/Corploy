using CorployGame.world;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorployGame.entity
{
    class AiAgent : Agent
    {
        public Vector2D Goal;
        public AiAgent(Vector2D spawnPos, World w, Texture2D t2d) : base(spawnPos, w, t2d)
        {
            Goal = new Vector2D(StaticParameters.BottomRightNodePos);
        }

        public override void InitializeAvatar()
        {
            base.InitializeAvatar();

            Avatar.SBS.SeekOn();
            //Avatar.SBS.ArriveON();
            Avatar.SBS.ObstacleAvoidanceON();
            Avatar.SBS.SetTarget(Goal);
        }

        public override void Update(float timeElapsed)
        {
            // TODO: will likely need to change later
            if (!MovementEnabled && Avatar != default) MovementEnabled = true;

            base.Update(timeElapsed);
        }
    }
}
