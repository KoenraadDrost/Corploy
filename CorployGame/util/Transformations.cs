using System;
using System.Collections.Generic;
using System.Text;

namespace CorployGame.util
{
    public class Transformations
    {
        public Transformations() { }
        public Vector2D VectorToWorldSpace(Vector2D vec, Vector2D heading, Vector2D side)
        {
            //make a copy of the point
            Vector2D TransVec = vec;

            //create a transformation matrix
            Matrix2D matTransform = new Matrix2D();
            matTransform = matTransform.RotateMatrix(heading, side);

            //now transform the vertices
            //matTransform.TransformVector2Ds(TransVec);

            return matTransform * TransVec;
        }

        public Vector2D VectorToWorldSpace(Vector2D vec, float degree)
        {
            //make a copy of the point
            Vector2D TransVec = vec;

            //create a transformation matrix
            Matrix2D matTransform = new Matrix2D();
            matTransform = Matrix2D.RotateMatrix(degree);

            //now transform the vertices
            //matTransform.TransformVector2Ds(TransVec);

            return matTransform * TransVec;
        }
    }
}
