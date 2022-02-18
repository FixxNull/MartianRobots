using System;

namespace MartianRobots.Domain.Models
{
    public struct Coords : IEquatable<Coords>
    {
        public static Coords InitialCoords => new(0, 0, 0);

        public const int MaxPos = 50;
        public int PosX { get; set; }
        public int PosY { get; set; }
        public int PosZ { get; set; }

        public Coords(int posX, int posY, int posZ = 0)
        {
            ValidateMaxPosCoord(posX);
            ValidateMaxPosCoord(posY);
            ValidateMaxPosCoord(posZ);

            PosX = posX;
            PosY = posY;
            PosZ = posZ;
        }

        public bool Equals(Coords other) =>
            PosX.Equals(other.PosX) && PosY.Equals(other.PosY) && PosX.Equals(other.PosX);

        public override bool Equals(object? obj) => 
            obj is Coords other && Equals(other);

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode() => 
            PosX.GetHashCode() ^ PosY.GetHashCode() ^ PosZ.GetHashCode();

        /// <summary>
        /// Sum operator.
        /// </summary>
        public static Coords operator + (Coords begin, Coords end) =>
            new(begin.PosX + end.PosX, begin.PosY + end.PosY, begin.PosZ + end.PosZ);

        /// <summary>
        /// Equals operator.
        /// </summary>
        public static bool operator == (Coords begin, Coords end) => begin.Equals(end);

        /// <summary>
        /// Not equals operator.
        /// </summary>
        public static bool operator != (Coords begin, Coords end) => !begin.Equals(end);

        private static void ValidateMaxPosCoord(int coord)
        {
            if (coord > MaxPos) 
                throw new ArgumentException($"Coord {coord} exceeds the limit {MaxPos}");
        }
    }
}