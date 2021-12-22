using CorployGame.world;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CorployGame.entity
{
    class PlayerAgent : Agent
    {
        public bool PlayerControlsEnabled;
        public bool ActiveKeyboardMove;
        private MouseState oldMState;
        private KeyboardState oldKState;

        public PlayerAgent(Vector2D spawnPos, World w, Texture2D t2d) : base(spawnPos, w, t2d)
        {
            PlayerControlsEnabled = false;
            ActiveKeyboardMove = false;
        }

        public override void InitializeAvatar()
        {
            base.InitializeAvatar();

            Avatar.SBS.SeekOn();
            //v.SBS.ArriveON();
            Avatar.SBS.ObstacleAvoidanceON();
            Avatar.SBS.SetTarget(World.Target.Pos);
            Avatar.VColor = Color.Blue;
            Avatar.UpdateTexture();

            // Load Texture of player
            FileStream fileStream = new FileStream(StaticParameters.PlayerSpritePath, FileMode.Open);
            Avatar.Texture = Texture2D.FromStream(World.GD, fileStream);
            fileStream.Dispose();
        }

        public override void Update(float timeElapsed)
        {
            var kstate = Keyboard.GetState();
            var mstate = Mouse.GetState();

            // Enable player controls on agent if Avatar has been innitialzed.
            // TODO: change when game pauzing is added.
            if (!PlayerControlsEnabled && Avatar != default)
            {
                PlayerControlsEnabled = true;
                MovementEnabled = true;
            }

            if(PlayerControlsEnabled)
            {
                if(ActiveKeyboardMove)
                {
                    // If the player is moving using their keyboard, all automized movement is overriden.
                    Avatar.SBS.AllOff();
                    // Steering force calcualted based on keyboard controls.
                    Avatar.SteeringForce = CalculateManual();
                }
                else
                {
                    // Otherwise turn the automatic movent back on if needed


                    // Update target position on mouse click. Account for lingering "Pressed"state.
                    if (mstate.LeftButton == ButtonState.Pressed && oldMState.LeftButton == ButtonState.Released)
                    {
                        World.Target.Pos = new Vector2D(mstate.X, mstate.Y);

                        Avatar.SBS.SetTarget(World.Target.Pos);
                    }

                    Avatar.SBS.SeekOn();
                    Avatar.SBS.ObstacleAvoidanceON();
                }

                // TODO: Add player actions somewhere here.
            }

            // Save latest state for reference.
            oldMState = mstate;
            oldKState = kstate;

            base.Update(timeElapsed);
        }

        public Vector2D CalculateManual()
        {
            Vector2D desiredVelocity = new Vector2D(0, 0);
            // Calculate manual movement force based on keyboard presses.
            // A = -X , D = +X , W = -Y , S = +Y

            // Calculate keyboardpress forces

            // Check to see if force is steering too close to obstacles or edge of map.
            double detectRange = Avatar.Texture.Height + Avatar.Texture.Height * (Avatar.Speed / Avatar.MaxSpeed);
            World.TagObstaclesInCollisionRange(Avatar, detectRange);

            return desiredVelocity;
        }
    }
}
