using CorployGame.behaviour.steering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorployGame.entity
{
    class Vehicle : MovingEntity
    {
        public SteeringBehaviours SBS{ get; set; }
        public Color VColor { get; set; }

        public Vehicle(Vector2D pos, World w, Texture2D t) : base(pos, w, t)
        {
            SBS = new SteeringBehaviours(this);
            Velocity = new Vector2D(0, 0);
            Scale = 5;

            VColor = Color.White;

            UpdateTexture();
        }

        public override void Update(float timeElapsed)
        {
            SteeringForce = SBS.Calculate();
            base.Update(timeElapsed);
        }

        public void UpdateTexture()
        {
            Color[] data = new Color[Texture.Width * Texture.Height];
            for (int i = 0; i < (Texture.Width * Texture.Height); ++i) data[i] = VColor;
            Texture.SetData<Color>(data);
        }
    }
}
