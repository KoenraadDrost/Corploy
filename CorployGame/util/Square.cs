using System;
using System.Collections.Generic;
using System.Text;

namespace CorployGame.util
{
    class Square
    {
        Vector2D CenterPos { get; set; }
        double Size { get; set; }
        List<Vector2D> Corners { get; set; }

        public Square(double size) : this(new Vector2D(size / 2, size / 2), size) { }

        public Square(Vector2D centerPos, double size)
        {
            Size = size;
            CenterPos = centerPos;

            Corners = new List<Vector2D>
            {
                new Vector2D( (CenterPos.X - Size/2) , (CenterPos.Y - Size/2) ), // Top left
                new Vector2D( (CenterPos.X + Size/2) , (CenterPos.Y - Size/2) ), // Top right
                new Vector2D( (CenterPos.X - Size/2) , (CenterPos.Y + Size/2) ), // Bottom left
                new Vector2D( (CenterPos.X + Size/2) , (CenterPos.Y + Size/2) ), // Bottom right
            };
        }

        public void SetNewSize(double size)
        {
            // Note that this method does NOT change CenterPos.

            double sizeDif = size - Size;

            // Top left
            Corners[0].X -= sizeDif;
            Corners[0].Y -= sizeDif;
            // Top right
            Corners[1].X += sizeDif;
            Corners[1].Y -= sizeDif;
            // Bottom left
            Corners[2].X -= sizeDif;
            Corners[2].Y += sizeDif;
            // Bottom right
            Corners[3].X += sizeDif;
            Corners[3].Y += sizeDif;

            Size = size;
        }

    }
}
