using System.Numerics;

namespace AbysmalCore
{
    /// <summary>
    /// Represents a 2D vector with integer components
    /// </summary>
    public struct Vector2Int
    {
        /// <summary>
        /// The x axis
        /// </summary>
        public int X;
        /// <summary>
        /// The y axis
        /// </summary>
        public int Y;

        /// <summary>
        /// Creates a new vector from 2 ints
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Vector2Int(int x, int y) { X = x; Y = y; }
        /// <summary>
        /// Creates a new vector from 2 floats
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Vector2Int(float x, float y) { X = (int)x; Y = (int)y; }
        /// <summary>
        /// Creates a new vector from 1 int
        /// </summary>
        /// <param name="n"></param>
        public Vector2Int(int n) { X = n; Y = n; }
        /// <summary>
        /// Creates a new vector from 1 float
        /// </summary>
        /// <param name="n"></param>
        public Vector2Int(float n) { X = (int)n; Y = (int)n; }
        /// <summary>
        /// Creates a new vector from a <see cref="Vector2"/>
        /// </summary>
        /// <param name="v"></param>
        public Vector2Int(Vector2 v) { X = (int)v.X; Y = (int)v.Y; }

        /// raylib uses Sys numerics vectors a lot
        /// so this is useful
        public Vector2 ToSys() => new(X, Y);
    }
}