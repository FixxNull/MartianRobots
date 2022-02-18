using System;
using MartianRobots.Domain.Enums;

namespace MartianRobots.App.Extensions
{
    public static class OrientationExtensions
    {
        /// <summary>
        /// Transforms an orientation to a key code.
        /// </summary>
        public static string GetKeyCode(this OrientationEnum orientation) => orientation 
            switch
            {
                OrientationEnum.North => "N",
                OrientationEnum.South => "S",
                OrientationEnum.East => "E",
                OrientationEnum.West => "W",
                _ => throw new ArgumentException(message: "Invalid enum value", paramName: nameof(orientation))
            };

        /// <summary>
        /// Transforms a key code to an orientation.
        /// </summary>
        public static OrientationEnum GetOrientationFromKey(this string code) => (code.ToUpper()) 
            switch
            {
                "N" => OrientationEnum.North,
                "S" => OrientationEnum.South,
                "E" => OrientationEnum.East,
                "W" => OrientationEnum.West,
                _ => throw new ArgumentException(message: "Invalid orientation code value", paramName: nameof(code))
            };
    }
}