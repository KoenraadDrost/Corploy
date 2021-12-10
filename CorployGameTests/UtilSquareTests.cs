using CorployGame;
using CorployGame.util;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CorployGameTests
{
    [TestFixture]
    class UtilSquareTests
    {
        public static IEnumerable<TestCaseData> SquareConstructorTestCases
        {
            get
            {
                yield return new TestCaseData( new Vector2D(0,0), 10,
                    new List<Vector2D>{
                        new Vector2D(-5,-5),    //TopLeft
                        new Vector2D(5,-5),     //TopRight
                        new Vector2D(5,5),      //BottomRight
                        new Vector2D(-5,5),     //BottomLeft
                    }, 10, new Vector2D(0,0) );
                yield return new TestCaseData(null, 10,
                    new List<Vector2D>{
                        new Vector2D(0,0),      //TopLeft
                        new Vector2D(10,0),     //TopRight
                        new Vector2D(10,10),    //BottomRight
                        new Vector2D(0,10),     //BottomLeft
                    }, 10, new Vector2D(5, 5));
            }
        }

        // Param Order: CenterPosX, CenterPosY, size, expected(X,Y) of each corner
        [TestCaseSource("SquareConstructorTestCases")]
        public void SquareConstructor(Vector2D centerPos, double size, List<Vector2D> expectedCorners, double expectedSize, Vector2D expectedCenterPos)
        {
            // Arrange
            Square actual = centerPos == null ? new Square(size) : new Square(centerPos, size);

            // Act

            // Assert
            // Note: AreEqual comparisons with complex objects like Vector2D were inconsistent. Using direct values instead.
            // List[x] comparisons also check if the same object is being referenced. In this case we only want to compare the values.
            for (int i = 0; i < expectedCorners.Count; i++)
            {
                Assert.AreEqual(expectedCorners[i].X, actual.Corners[i].X);
                Assert.AreEqual(expectedCorners[i].Y, actual.Corners[i].Y);
            }
            Assert.AreEqual(expectedSize, actual.Size);
            Assert.AreEqual(expectedCenterPos.X, actual.CenterPos.X);
            Assert.AreEqual(expectedCenterPos.Y, actual.CenterPos.Y);
        }

        public static IEnumerable<TestCaseData> SquareSetNewSizeTestCases {
            get {
                yield return new TestCaseData(new Vector2D(0, 0), 10, 20,
                    new List<Vector2D>{
                        new Vector2D(-10,-10),    //TopLeft
                        new Vector2D(10,-10),     //TopRight
                        new Vector2D(10,10),      //BottomRight
                        new Vector2D(-10,10),     //BottomLeft
                    }, 20, new Vector2D(0, 0));
                yield return new TestCaseData(null, 10, 10,
                    new List<Vector2D>{
                        new Vector2D(0,0),      //TopLeft
                        new Vector2D(10,0),     //TopRight
                        new Vector2D(10,10),    //BottomRight
                        new Vector2D(0,10),     //BottomLeft
                    }, 10, new Vector2D(5, 5));
                yield return new TestCaseData(null, 10, 5,
                    new List<Vector2D>{
                        new Vector2D(2.5,2.5),      //TopLeft
                        new Vector2D(7.5,2.5),      //TopRight
                        new Vector2D(7.5,7.5),      //BottomRight
                        new Vector2D(2.5,7.5),      //BottomLeft
                    }, 5, new Vector2D(5, 5));
            }
        }

        [TestCaseSource("SquareSetNewSizeTestCases")]
        public void SquareSetNewSize(Vector2D centerPos, double size, double newSize, List<Vector2D> expectedCorners, double expectedSize, Vector2D expectedCenterPos)
        {
            // Arrange
            Square actual = centerPos == null ? new Square(size) : new Square(centerPos, size);

            // Act
            actual.SetNewSize(newSize);

            // Assert
            for (int i = 0; i < expectedCorners.Count; i++)
            {
                Assert.AreEqual(expectedCorners[i].X, actual.Corners[i].X);
                Assert.AreEqual(expectedCorners[i].Y, actual.Corners[i].Y);
            }
            Assert.AreEqual(expectedSize, actual.Size);
            Assert.AreEqual(expectedCenterPos.X, actual.CenterPos.X);
            Assert.AreEqual(expectedCenterPos.Y, actual.CenterPos.Y);
        }

        public static IEnumerable<TestCaseData> SquareRotateCornersTestCases {
            get {
                yield return new TestCaseData(new Vector2D(0, 0), 10, 0,
                    new List<Vector2D>{
                        new Vector2D(-5,-5),    //TopLeft
                        new Vector2D(5,-5),     //TopRight
                        new Vector2D(5,5),      //BottomRight
                        new Vector2D(-5,5),     //BottomLeft
                    }, new Vector2D(0, 0));
                yield return new TestCaseData(null, 10, 0,
                    new List<Vector2D>{
                        new Vector2D(0,0),      //TopLeft
                        new Vector2D(10,0),     //TopRight
                        new Vector2D(10,10),    //BottomRight
                        new Vector2D(0,10),     //BottomLeft
                    }, new Vector2D(5, 5));
                yield return new TestCaseData(new Vector2D(0, 0), 10, 45,
                    new List<Vector2D>{
                        new Vector2D( 0, -Math.Sqrt(50) ),     //TopLeft y = square root of (-5^2 + -5^2)
                        new Vector2D( Math.Sqrt(50), 0 ),      //TopRight
                        new Vector2D( 0, Math.Sqrt(50) ),      //BottomRight
                        new Vector2D( -Math.Sqrt(50), 0 ),     //BottomLeft
                    }, new Vector2D(0, 0));
                yield return new TestCaseData(new Vector2D(0, 0), 10, -45,
                    new List<Vector2D>{
                        new Vector2D( -(Math.Sqrt(50)), 0 ),     //TopLeft y = square root of (-5^2 + -5^2)
                        new Vector2D( 0, -Math.Sqrt(50) ),     //TopRight
                        new Vector2D( Math.Sqrt(50), 0 ),      //BottomRight
                        new Vector2D( 0, Math.Sqrt(50) ),      //BottomLeft
                    }, new Vector2D(0, 0));
            }
        }

        [TestCaseSource("SquareRotateCornersTestCases")]
        public void SquareRotateCorners(Vector2D centerPos, double size, float degrees, List<Vector2D> expectedCorners, Vector2D expectedCenterPos)
        {
            // Arrange
            Square actual = centerPos == null ? new Square(size) : new Square(centerPos, size);

            // Act
            actual.RotateCorners(degrees);

            // Assert
            for (int i = 0; i < expectedCorners.Count; i++)
            {
                Assert.AreEqual( Math.Round(expectedCorners[i].X, 4), Math.Round(actual.Corners[i].X, 4) );
                Assert.AreEqual( Math.Round(expectedCorners[i].Y, 4), Math.Round(actual.Corners[i].Y, 4) );
            }
            Assert.AreEqual(expectedCenterPos.X, actual.CenterPos.X);
            Assert.AreEqual(expectedCenterPos.Y, actual.CenterPos.Y);
        }
    }
}
