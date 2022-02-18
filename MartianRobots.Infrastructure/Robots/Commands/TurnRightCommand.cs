using MartianRobots.Domain.Models;
using MartianRobots.Infrastructure.Abstractions.Maps;
using MartianRobots.Infrastructure.Abstractions.Robots;
using MartianRobots.Infrastructure.Extensions;

namespace MartianRobots.Infrastructure.Robots.Commands
{
    public sealed class TurnRightCommand : IRobotCommand
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
            robot.Orientation = planetMap.TurnRight(robot.Orientation);
        }

        #region Singleton implementation

        private static TurnRightCommand? CurrentInstance { get; set; }

        private TurnRightCommand()
        {
        }

        /// <summary>
        /// Gets an instance.
        /// </summary>
        public static TurnRightCommand Instance => CurrentInstance ??= new TurnRightCommand();

        #endregion
    }
}