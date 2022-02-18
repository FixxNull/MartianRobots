using System.Linq;
using MartianRobots.Domain.Models;
using MartianRobots.Domain.Statuses;

namespace MartianRobots.Infrastructure.Extensions
{
    public static class RobotExtensions
    {
        /// <summary>
        /// Get LOST status of current robot instance
        /// </summary>
        /// <param name="robot">Current instance or robot</param>
        /// <returns></returns>
        public static bool IsStatusLost(this Robot robot) => 
            robot.Statuses.OfType<RobotStatusLost>().Any();
    }
}