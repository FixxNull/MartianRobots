using System.Collections.Generic;
using MartianRobots.Domain.Enums;
using MartianRobots.Domain.Models;

namespace MartianRobots.Infrastructure.Abstractions.Maps
{
    public interface IPlanetMap
    {
        /// <summary>
        /// Configures the planet map.
        /// </summary>
        public void Configure(PlanetMapConfig config);

        /// <summary>
        /// Adds a landmark to the planet.
        /// </summary>
        public void AddMapPoint(Coords coordinates, MapPoint mapPoint);

        /// <summary>
        /// Retrieves the planet landmarks.
        /// </summary>
        public IDictionary<Coords, IEnumerable<MapPoint>> GetMapPoints();

        /// <summary>
        /// Calculates the next coordinates increment from current position and orientation.
        /// </summary>
        public Coords GetNextCoords(Coords coordinates, OrientationEnum orientation);

        /// <summary>
        /// Calculates next orientation 90º to left.
        /// </summary>
        public OrientationEnum TurnLeft(OrientationEnum orientation);

        /// <summary>
        /// Calculates next orientation 90º to right.
        /// </summary>
        public OrientationEnum TurnRight(OrientationEnum orientation);

        /// <summary>
        /// Returns if given coordinates are out of bounds.
        /// </summary>
        public bool IsOutOfMapCoords(Coords coords);
    }
}