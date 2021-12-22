using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorployGame.entity
{
    class AiAgent : Agent
    {
        public AiAgent(Vector2D spawnPos, World w, Texture2D t2d) : base(spawnPos, w, t2d)
        {
        }

        public override void InitializeAvatar()
        {
            base.InitializeAvatar();
        }

        public override void Update(float timeElapsed)
        {
            // TODO: will likely need to change later
            if (!MovementEnabled && Avatar != default) MovementEnabled = true;

            base.Update(timeElapsed);
        }
    }
}
