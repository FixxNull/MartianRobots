using System.Collections.Generic;
using MartianRobots.Domain.Enums;
using MartianRobots.Domain.Statuses.Interfaces;

namespace MartianRobots.Domain.Models
{
    public class Robot
    {
        /// <summary>
        /// Coordinates <see cref="Coords"/>
        /// </summary>
        public Coords Coords { get; set; }

        /// <summary>
        /// Orientation <see cref="OrientationEnum"/>
        /// </summary>
        public OrientationEnum Orientation { get; set; }

        /// <summary>
        /// Reported status list <see cref="IRobotStatus"/>.
        /// </summary>
        public IList<IRobotStatus> Statuses { get; } = new List<IRobotStatus>();

        public Robot(Coords coordinates, OrientationEnum orientation)
        {
            Coords = coordinates;
            Orientation = orientation;
        }
    }
}