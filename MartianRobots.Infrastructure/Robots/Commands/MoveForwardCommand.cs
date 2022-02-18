using System.Linq;
using MartianRobots.Domain.Models;
using MartianRobots.Domain.Statuses;
using MartianRobots.Infrastructure.Abstractions.Maps;
using MartianRobots.Infrastructure.Abstractions.Robots;
using MartianRobots.Infrastructure.Extensions;
using MartianRobots.Infrastructure.Maps.MapPoints;

namespace MartianRobots.Infrastructure.Robots.Commands
{
    public sealed class MoveForwardCommand : IRobotCommand
    {
        /// <summary>
        /// Execute command for robot moving
        /// </summary>
        /// <param name="robot">Instance of robot</param>
        /// <param name="planet">Instance of planet</param>
        public void ExecuteCommand(Robot robot, IPlanetMap planet)
        {
            // Robots with status LOST can't move
            if (robot.IsStatusLost()) 
                return;

            var nextCoords = planet.GetNextCoords(robot.Coords, robot.Orientation);

            // If any lost map points in position
            // Get next position to check if next coord it's setuped as lost
            // Already recorded as lost map points will be avoided
            var lostMapPoints = planet.GetLostMapPoints();
            var nextIsLost = lostMapPoints.TryGetValue(robot.Coords, out var marks)
                             && marks.Any(m => m.LostCoords == nextCoords);

            if (nextIsLost) 
                return;

            if (planet.IsOutOfMapCoords(nextCoords))
            {
                // If next position is out of map points, robot will be lost
                // and a lost map points should be setuped at last seen coordinates
                robot.Statuses.Add(new RobotStatusLost());
                planet.AddMapPoint(robot.Coords, new LostMapPoint(nextCoords));
            }
            else
            {
                robot.Coords = nextCoords;
            }
        }

        #region Singleton implementation

        private static MoveForwardCommand? CurrentInstance { get; set; }

        private MoveForwardCommand()
        {
        }

        public static MoveForwardCommand Instance => CurrentInstance ??= new MoveForwardCommand();

        #endregion
    }
}