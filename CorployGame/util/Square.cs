using System;
using System.Collections.Generic;
using System.Text;

namespace CorployGame.util
{
    public class Square
    {
        public Vector2D CenterPos { get; set; }
        public double Size { get; set; }
        public List<Vector2D> Corners { get; set; }

        public Square(double size) : this(new Vector2D(size / 2, size / 2), size) { }

        public Square(Vector2D centerPos, double size)
        {
            Size = size;
            CenterPos = centerPos;

            Corners = new List<Vector2D>
            {
                new Vector2D( (CenterPos.X - Size/2) , (CenterPos.Y - Size/2) ), // Top left
                new Vector2D( (CenterPos.X + Size/2) , (CenterPos.Y - Size/2) ), // Top right
                new Vector2D( (CenterPos.X + Size/2) , (CenterPos.Y + Size/2) ), // Bottom right
                new Vector2D( (CenterPos.X - Size/2) , (CenterPos.Y + Size/2) ), // Bottom left
            };
        }

        public void SetNewSize(double size)
        {
            // Note that this method does NOT change CenterPos.

            double sizeDif = (size - Size) / 2;

            // Top left
            Corners[0].X -= sizeDif;
            Corners[0].Y -= sizeDif;
            // Top right
            Corners[1].X += sizeDif;
            Corners[1].Y -= sizeDif;
            // Bottom right
            Corners[2].X += sizeDif;
            Corners[2].Y += sizeDif;
            // Bottom left
            Corners[3].X -= sizeDif;
            Corners[3].Y += sizeDif;

            Size = size;
        }

        public void RotateCorners(float degrees)
        {
            Matrix2D rM = Matrix2D.RotateMatrix(degrees);

            for (int i = 0; i < 4; i++)
            {
                Corners[i] =  rM * Corners[i];
            }
        }

        public void RotateSquare(float degrees)
        {
            Matrix2D rM = Matrix2D.RotateMatrix(degrees);
            CenterPos = rM * CenterPos;
            for (int i = 0; i < 4; i++)
            {
                Corners[i] = rM * Corners[i];
            }
        }

        public void MoveSquare(Vector2D v)
        {
            CenterPos += v;
            for (int i = 0; i < 4; i++)
            {
                Corners[i] += v;
            }
        }

        public override string ToString()
        {
            return string.Format("< C={0} : [TL{1} - TR{2} - BR{3} - BL{4} >", CenterPos, Corners[0], Corners[1], Corners[2], Corners[3]);
        }

    }
}
