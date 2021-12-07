using CorployGame.entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace CorployGame
{
    public class Game1 : Game
    {
        Texture2D winLogotexture;
        Vector2 winLogoPos;
        float winLogoSpeed;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private World world;
        private MouseState oldMState;
        private KeyboardState oldKState;

        public Game1()
        {
            IsFixedTimeStep = false;
            TargetElapsedTime = TimeSpan.FromSeconds(0.016666);
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic
            winLogoPos = new Vector2(_graphics.PreferredBackBufferWidth / 2,
                _graphics.PreferredBackBufferHeight / 2);
            winLogoSpeed = 100f;

            int width = 800;
            int height = 480;

            world = new World(width, height, GraphicsDevice);
            world.Populate();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            //winLogotexture = Content.Load<Texture2D>("WindesheimLogo");

            // TODO: Replacement for Content loader from monogame
            //FileStream fileStream = new FileStream("Content/sprites/sprite_atlas.png", FileMode.Open):
            //Texture2D spriteAtlas = Texture2D.FromStream(graphicsDevice, fileStream);
            //fileStream.Dispose();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            var kstate = Keyboard.GetState();
            var mstate = Mouse.GetState();

            if (kstate.IsKeyDown(Keys.Up))
                winLogoPos.Y -= winLogoSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(Keys.Down))
                winLogoPos.Y += winLogoSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(Keys.Left))
                winLogoPos.X -= winLogoSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(Keys.Right))
                winLogoPos.X += winLogoSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Update target position on mouse click. Account for lingering "Pressed"state.
            if (mstate.LeftButton == ButtonState.Pressed && oldMState.LeftButton == ButtonState.Released)
            {
                world.Target.Pos = new Vector2D(mstate.X, mstate.Y);

                // TODO: Find better way for target update.
                if (world.entities[0] is Vehicle)
                {
                    ((Vehicle)world.entities[0] ).SBS.SetTarget(world.Target.Pos);
                }
            }

            // Update World
            world.Update(gameTime);

            // Save latest state for reference.
            oldMState = mstate;
            oldKState = kstate;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            //_spriteBatch.Begin();
            //_spriteBatch.Draw(
            //    winLogotexture,
            //    winLogoPos,
            //    null,
            //    Color.White,
            //    0f,
            //    new Vector2(winLogotexture.Width / 2, winLogotexture.Height / 2),
            //    Vector2.One,
            //    SpriteEffects.None,
            //    0f
            //);
            //_spriteBatch.End();


            // TODO: Remove excesive comments.
            _spriteBatch.Begin();

            //_spriteBatch.Draw(world.Target.Texture, new Vector2((float) world.Target.Pos.X, (float)world.Target.Pos.Y), Color.White);

            //foreach (MovingEntity me in world.entities)
            //{
            //    _spriteBatch.Draw(me.Texture, me.GetDrawCoordinate(), Color.White);
            //}

            //foreach (Obstacle obst in world.obstacles)
            //{
            //    _spriteBatch.Draw(obst.Texture, obst.GetDrawCoordinate(), Color.White);
            //}

            world.Draw(_spriteBatch, gameTime);

            _spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
