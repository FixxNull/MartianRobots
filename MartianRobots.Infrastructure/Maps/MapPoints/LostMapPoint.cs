using MartianRobots.Domain.Models;
using MartianRobots.Infrastructure.Abstractions.Maps;

namespace MartianRobots.Infrastructure.Maps.MapPoints
{
    public record LostMapPoint(Coords LostCoords) : MapPoint
    {
        /// <summary>
        /// Coordinates to where the lost robot was headed
        /// </summary>
        public Coords LostCoords { get; } = LostCoords;
    }
}