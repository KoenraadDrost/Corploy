using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CorployGame.entity
{
    abstract class BaseGameEntity
    {
        public Vector2D Pos { get; set; }
        public float Scale { get; set; }
        public World MyWorld { get; set; }
        public Texture2D Texture { get; set; }
        public Color VColor { get; set; }

        double Radius; // Not always set.

        public BaseGameEntity(Vector2D pos, World w, Texture2D t)
        {
            Pos = pos;
            MyWorld = w;
            Texture = t;
            VColor = Color.White;

            UpdateTexture();
        }

        public BaseGameEntity(Vector2D pos, World w, Texture2D t, double rad) : this(pos, w, t)
        {
            Radius = rad;
        }

        public abstract void Update(float delta);

        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);

        public virtual void Render(GraphicsDevice g)
        {

        }
        public void UpdateTexture()
        {
            Color[] data = new Color[Texture.Width * Texture.Height];
            for (int i = 0; i < (Texture.Width * Texture.Height); ++i) data[i] = VColor;
            Texture.SetData<Color>(data);
        }

        public Vector2 GetDrawCoordinate()
        {
            float x = (float)Pos.X - (Texture.Width / 2);
            float y = (float)Pos.Y - (Texture.Height / 2);
            return new Vector2(x, y);
        }

        public Vector2 GetTextureCenter()
        {
            return new Vector2(Texture.Width / 2, Texture.Height / 2);
        }

        public double GetRadius()
        {
            if (Radius != default) return Radius;
            if (Texture.Height >= Texture.Width)
            {
                return Texture.Width;
            }
            else return Texture.Height;
        }
    }
}
