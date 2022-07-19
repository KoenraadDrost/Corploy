using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorployGame
{
   
    public class Vector2D
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Vector2D() : this(0, 0)
        {
        }

        public Vector2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Vector2D(Vector2D v) : this(v.X, v.Y) { }

        public double Length()
        {
            double xPow = Math.Pow(Math.Abs(X), 2);
            double yPow = Math.Pow(Math.Abs(Y), 2);
            //double xPow = Math.Pow(X, 2);
            //double yPow = Math.Pow(Y, 2);
            return Math.Sqrt(xPow + yPow);
        }

        public double LengthSquared()
        {
            return Math.Sqrt(Length());
        }

        /// <summary>
        /// Gets angle in degrees relative to horizon hine pointA( X = 0, Y = 0 ) => pointB( X = this.X , Y = 0)
        /// Points below the horizon line are positive angles.( eg. 0.0 degrees to 180.0 degrees ).
        /// Points above the horizon line are negative angles.( eg. -0.1 degrees to -180.0 degrees ).
        /// </summary>
        /// <returns></returns>
        public float GetAngleDegrees()
        {
            double radianAngle = Math.Atan2( Y, X );
            double degreeAngle = radianAngle * (180 / Math.PI);
            float degrees = (float)degreeAngle;

            return degrees;
        }

        public Vector2D Add(Vector2D v)
        {
            this.X += v.X;
            this.Y += v.Y;
            return this;
        }

        public Vector2D Sub(Vector2D v)
        {
            this.X -= v.X;
            this.Y -= v.Y;
            return this;
        }

        public Vector2D Multiply(double value)
        {
            this.X *= value;
            this.Y *= value;
            return this;
        }

        public Vector2D divide(double value)
        {
            this.X = X / value;
            this.Y = Y / value;
            return this;
        }

        public Vector2D divide(float value)
        {
            this.X = X / value;
            this.Y = Y / value;
            return this;
        }

        public Vector2D PerpendicularClockwise()
        {
            // Vector(4,7) becomes (7,-4)
            return new Vector2D(Y, -X);
        }

        public Vector2D PerpendicularCounterClockwise()
        {
            // Vector(-8, 3) becomes (-3, -8)
            return new Vector2D(-Y, X);
        }

        // Operators
        public static Vector2D operator +(Vector2D v1, Vector2D v2)
        {
            return new Vector2D((v1.X + v2.X), (v1.Y + v2.Y));
        }
        public static Vector2D operator -(Vector2D v1, Vector2D v2)
        {
            return new Vector2D((v1.X - v2.X), (v1.Y - v2.Y));
        }
        public static Vector2D operator *(Vector2D v1, float f1)
        {
            return new Vector2D((v1.X * f1), (v1.Y * f1));
        }

        public static Vector2D operator *(float f1, Vector2D v1)
        {
            return v1 * f1;
        }

        public static Vector2D operator *(Vector2D v1, double f1)
        {
            return new Vector2D((v1.X * f1), (v1.Y * f1));
        }

        public static Vector2D operator *(double f1, Vector2D v1)
        {
            return v1 * f1;
        }

        public static Vector2D operator /(Vector2D v1, double f1)
        {
            return v1.divide(f1);
        }

        public static Vector2D operator /(Vector2D v1, float f1)
        {
            return v1.divide(f1);
        }

        public Vector2D Normalize()
        {
            if(X != 0) X = X / Length(); // prevent divide-by-zero with if-statement.
            if(Y != 0) Y = Y / Length();
            return this;
        }

        public Vector2D truncate(double maX)
        {
            if (Length() > maX)
            {
                Normalize();
                Multiply(maX);
            }
            return this;
        }

        public Vector2D Clone()
        {
            return new Vector2D(this.X, this.Y);
        }

        public override string ToString()
        {
            return String.Format("({0},{1})", X, Y);
        }
    }


}
