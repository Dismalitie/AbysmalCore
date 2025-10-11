using System.Numerics;
using System.Runtime.InteropServices;

namespace AbysmalCore
{
    /// this isnt really necessary as it
    /// was to fix mem access violations
    /// by making sure c# structs match
    /// up with cpp bind structs
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector2Int
    {
        public int X;
        public int Y;
        public Vector2Int(int x, int y) { X = x; Y = y; }
        public Vector2Int(float x, float y) { X = (int)x; Y = (int)y; }
        public Vector2Int(int n) { X = n; Y = n; }
        public Vector2Int(float n) { X = (int)n; Y = (int)n; }

        /// raylib uses Sys numerics vectors a lot
        /// so this is useful
        public Vector2 ToSys() => new(X, Y);
        public static Vector2Int FromSys(Vector2 vec) => new((int)vec.X, (int)vec.Y);
    }
}