using MartianRobots.Domain.Models;
using MartianRobots.Infrastructure.Abstractions.Maps;

namespace MartianRobots.Infrastructure.Abstractions.Robots
{
    public interface IRobotCommand
    {
        void ExecuteCommand(Robot robot, IPlanetMap planet);
    }
}