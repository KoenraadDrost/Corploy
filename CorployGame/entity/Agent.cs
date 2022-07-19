using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorployGame.entity
{
    class Agent// :IDisposable
    {
        public Vehicle Avatar { get; set; }

        private bool _shouldSpawn;
        public bool ShouldSpawn 
        {
            get => _shouldSpawn;
            set { if (!HasSpawned) _shouldSpawn = value; } // Prevent spawning again if already active.
        }

        private bool _shouldDespawn;
        public bool ShouldDespawn
        {
            get => _shouldDespawn;
            set { if (HasSpawned) _shouldDespawn = value; } // Only Despawn if currently active.
        }
        public bool HasSpawned;
        public bool MovementEnabled;
        protected Vector2D SpawnPos;
        protected World World;
        protected Texture2D AvatarTexture;


        public Agent(Vector2D spawnPos, World w, Texture2D t2d)
        {
            ShouldSpawn = false;
            ShouldDespawn = false;
            HasSpawned = false;
            MovementEnabled = false;

            SpawnPos = spawnPos;
            World = w;
            AvatarTexture = t2d;
        }

        public virtual void InitializeAvatar()
        {
            Avatar = new Vehicle(SpawnPos, World, AvatarTexture);
            ShouldSpawn = false;

            MovementEnabled = true;
            HasSpawned = true;
        }

        public virtual void Update(float timeElapsed)
        {
            if (Avatar == default && ShouldSpawn) InitializeAvatar();
            if (Avatar != default && MovementEnabled) Avatar.Update(timeElapsed);
            if (Avatar != default && ShouldDespawn) Despawn();
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (HasSpawned) Avatar.Draw(spriteBatch, gameTime);
        }

        public void Despawn()
        {
            Avatar = null;
            ShouldSpawn = false; // Just to make sure. Under normal circumstances this should already be false at this point.
            MovementEnabled = false;
            HasSpawned = false;
            ShouldDespawn = false;
        }

        //TODO: Implement for managing active agents in the game via waves.
        //public void Dispose()
        //{
        //    Dispose(true);
        //    //the SuppressFinalize method prevents the garbage collector from running the finalizer.
        //    GC.SuppressFinalize(this);
        //}
    }
}
