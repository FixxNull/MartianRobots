using MartianRobots.Domain.Models;
using MartianRobots.Infrastructure.Abstractions.Maps;
using MartianRobots.Infrastructure.Abstractions.Robots;
using MartianRobots.Infrastructure.Extensions;

namespace MartianRobots.Infrastructure.Robots.Commands
{
    public sealed class TurnLeftCommand : IRobotCommand
    {
        /// <summary>
        /// Executes the robor action
        /// </summary>
        public void ExecuteCommand(Robot robot, IPlanetMap planetMap)
        {
            // Troubled robots can't move
            if (robot.IsStatusLost()) 
                return;

            // Get current orientation
            // Change orientation
            robot.Orientation = planetMap.TurnLeft(robot.Orientation);
        }

        #region Singleton implementation

        private static TurnLeftCommand? CurrentInstance { get; set; }

        private TurnLeftCommand()
        {
        }

        /// <summary>
        /// Gets an instance.
        /// </summary>
        public static TurnLeftCommand Instance => CurrentInstance ??= new TurnLeftCommand();

        #endregion
    }
}