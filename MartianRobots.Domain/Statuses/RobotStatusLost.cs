using MartianRobots.Domain.Statuses.Interfaces;

namespace MartianRobots.Domain.Statuses
{
    public struct RobotStatusLost : IRobotStatus
    {
        private const string StatusCode = "LOST";
        public string GetStatusCode() => StatusCode;
    }
}