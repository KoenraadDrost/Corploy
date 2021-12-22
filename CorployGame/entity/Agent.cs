using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorployGame.entity
{
    class Agent
    {
        public Vehicle Avatar { get; set; }
        public bool ShouldSpawn;
        public bool MovementEnabled;
        protected Vector2D SpawnPos;
        protected World World;
        protected Texture2D AvatarTexture;


        public Agent(Vector2D spawnPos, World w, Texture2D t2d)
        {
            ShouldSpawn = false;
            MovementEnabled = false;

            SpawnPos = spawnPos;
            World = w;
            AvatarTexture = t2d;
        }

        public virtual void InitializeAvatar()
        {
            Avatar = new Vehicle(SpawnPos, World, AvatarTexture);
            ShouldSpawn = false;
        }

        public virtual void Update(float timeElapsed)
        {
            if (Avatar == default && ShouldSpawn) InitializeAvatar();
            if (Avatar != default && MovementEnabled) Avatar.Update(timeElapsed);
        }
    }
}
