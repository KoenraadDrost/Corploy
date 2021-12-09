using CorployGame;
using NUnit.Framework;
using System;

namespace CorployGameTests
{
    [TestFixture]
    class UtilVectorTests
    {
        [TestCase(0, 0, 0)]
        [TestCase(1, 0, 0)]
        [TestCase(1, 1, 45)]
        [TestCase(2, 2, 45)]
        [TestCase(2, 4, 63)]
        [TestCase(0, 1, 90)]
        [TestCase(-1, 1, 135)]
        [TestCase(-1, 0, 180)] // Technically could also be -180, but positive angle takes priority.
        [TestCase(-1, -1, -135)]
        [TestCase(0, -1, -90)]
        [TestCase(1, -1, -45)]
        public void GetAngleDegreesFromVector(double x, double y, float expected)
        {
            float actual;

            // Arrange
            Vector2D vector = new Vector2D(x, y);

            // Act
            actual = (float)Math.Round( vector.GetAngleDegrees(), 0);

            // Assert
            Assert.AreEqual(expected, actual);
        }

    }
}
