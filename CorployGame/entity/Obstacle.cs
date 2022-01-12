using CorployGame.world.navigation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorployGame.entity
{
    class Obstacle : BaseGameEntity
    {
        public bool BlocksNodes;
        public Obstacle(Vector2D pos, World w, Texture2D t) : base(pos, w, t)
        {
            BlocksNodes = false;
        }

        // copy Constructor
        public Obstacle(Obstacle o) : this(o.Pos, o.MyWorld, o.Texture)
        {
        }

        public override void Update(float delta)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Vector2 position = new Vector2((float)Pos.X, (float)Pos.Y);

            //_spriteBatch.Draw(
            //    Texture2D, --texture
            //    Vector2, --position
            //    Square, --(is nullable)
            //    Color,
            //    Rotation,
            //    Origin, --(Define an origin point relative to the texture. Origin wil snap to the given 'position'. If not used, or set to(0,0), Drawing will start with 'position' as top-left corner of texture.)
            //              (This is normaly used to set the rotation axis) 
            //    Vector2, --scale
            //    SpriteEffects, --effects
            //    0f
            //);
            spriteBatch.Draw(
                Texture,
                position,
                null,
                VColor,
                0,
                GetTextureCenter(),
                Vector2.One,
                SpriteEffects.None,
                0f
            );
        }    

        public List<string> GetBlockedNodes()
        {
            Vector2 tc = GetTextureCenter();
            // TopLeft corner
            Vector2D tl = new Vector2D(    x: Pos.X - tc.X,
                                           y: Pos.Y - tc.Y
                                        );
            // BottomRight corner
            Vector2D br = new Vector2D(    x: Pos.X + tc.X,
                                           y: Pos.Y + tc.Y
                                        );

            List<string> BlockedNodeIDs = new List<string>();
            BlockedNodeIDs = MyWorld.FindNodeIDsInArea(tl, br);

            return BlockedNodeIDs;
        }

        public void SetBlockedNodes()
        {
            List<string> nodeIDs = GetBlockedNodes();
            if (nodeIDs != null || nodeIDs.Count < 1) return;

            BlocksNodes = true;
            foreach(string id in nodeIDs)
            {
                MyWorld.AllNodes[id].Blocked = true;
            }
        }
    }
}
