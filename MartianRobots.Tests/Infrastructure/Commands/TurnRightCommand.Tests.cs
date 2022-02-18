using MartianRobots.Domain.Enums;
using MartianRobots.Domain.Models;
using MartianRobots.Domain.Statuses;
using MartianRobots.Infrastructure.Abstractions.Maps;
using MartianRobots.Infrastructure.Robots.Commands;
using Moq;
using Xunit;

namespace MartianRobots.Tests.Infrastructure.Commands
{
    public class TurnRightCommandTests
    {
        /// <summary>
        /// Test: reIfRobotIsLost A robot that moves off an edge of the grid is lost forever. 
        /// However, lost robots leave a robot scent that prohibits future robots from dropping off the world at the same grid point. 
        /// The scent is left at the last grid position the robot occupied before disappearing over the edge. 
        /// An instruction to move off the world from a grid point from which a robot has been previously 
        /// lost is simply ignored by the current robot.")]
        /// </summary>
        [Fact]
        public void IgnoreIfRobotIsLost()
        {
            var map = new Mock<IPlanetMap>();
            var robot = new Robot(Coords.InitialCoords, OrientationEnum.North);

            robot.Statuses.Add(new RobotStatusLost());

            TurnRightCommand.Instance.ExecuteCommand(robot, map.Object);

            map.Verify(mock => mock.TurnRight(It.IsAny<OrientationEnum>()), Times.Never());
        }

        /// <summary>
        /// Test: MoveRobot
        /// </summary>
        [Fact]
        public void MoveRobot()
        {
            var map = new Mock<IPlanetMap>();
            var robot = new Robot(Coords.InitialCoords, OrientationEnum.North);

            TurnRightCommand.Instance.ExecuteCommand(robot, map.Object);

            map.Verify(mock => mock.TurnRight(It.IsAny<OrientationEnum>()), Times.Once());
        }
    }
}
