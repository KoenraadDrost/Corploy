using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorployGame.entity
{
    class Vehicle : MovingEntity
    {
        public Color VColor { get; set; }

        public Vehicle(Vector2D pos, World w, Texture2D t) : base(pos, w, t)
        {
            Velocity = new Vector2D(0, 0);
            Scale = 5;

            Color[] data = new Color[20 * 20];
            for (int i = 0; i < (20 * 20); ++i) data[i] = Color.Chocolate;
            Texture.SetData<Color>(data);

        }
    }
}
