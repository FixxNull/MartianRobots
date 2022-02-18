using MartianRobots.Domain.Models;

namespace MartianRobots.Infrastructure.Abstractions.Maps
{
    public class PlanetMapConfig
    {
        /// <summary>
        /// Origin coordinates of the map
        /// </summary>
        public Coords Origin { get; set; }

        /// <summary>
        /// Total height of the map
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Total width of the map
        /// </summary>
        public int Width { get; set; }
    }
}