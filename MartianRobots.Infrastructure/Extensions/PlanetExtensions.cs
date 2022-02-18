using System.Collections.Generic;
using System.Linq;
using MartianRobots.Domain.Models;
using MartianRobots.Infrastructure.Abstractions.Maps;
using MartianRobots.Infrastructure.Maps.MapPoints;

namespace MartianRobots.Infrastructure.Extensions
{
    public static class PlanetExtensions
    {
        /// <summary>
        /// Extension method allow get points of map that have status LOST
        /// </summary>
        /// <param name="planetMap">Instance of IPlanetMap</param>
        /// <returns></returns>
        public static IDictionary<Coords, IEnumerable<LostMapPoint>> GetLostMapPoints(this IPlanetMap planetMap) 
            => planetMap.GetMapPoints()
                .Where(p => p.Value.OfType<LostMapPoint>()
                    .Any())
                .ToDictionary(p => p.Key,
                    p => p.Value.OfType<LostMapPoint>());
    }
}