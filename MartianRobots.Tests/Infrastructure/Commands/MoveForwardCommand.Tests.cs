using FluentAssertions;
using MartianRobots.Domain.Enums;
using MartianRobots.Domain.Models;
using MartianRobots.Domain.Statuses;
using MartianRobots.Infrastructure.Abstractions.Maps;
using MartianRobots.Infrastructure.Maps.MapPoints;
using MartianRobots.Infrastructure.Robots.Commands;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MartianRobots.Tests.Infrastructure.Commands
{
    public class MoveForwardCommandTests
    {
        /// <summary>
        /// Test: IgnoreIfRobotIsLost
        /// </summary>
        [Fact]
        public void IgnoreIfRobotIsLost()
        {
            var map = new Mock<IPlanetMap>();
            var robot = new Robot(Coords.InitialCoords, OrientationEnum.North);

            robot.Statuses.Add(new RobotStatusLost());
            MoveForwardCommand.Instance.ExecuteCommand(robot, map.Object);

            map.Verify(mock => mock.TurnLeft(It.IsAny<OrientationEnum>()), Times.Never());
        }

        /// <summary>
        /// Test ShouldMoveRobot Check robot movement
        /// </summary>
        [Fact]
        public void MoveRobot()
        {
            var map = new Mock<IPlanetMap>();
            var initialPosition = Coords.InitialCoords;
            var robot = new Robot(initialPosition, OrientationEnum.North);
            var destination = new Coords(0, 1);
            var lostLandmark = new LostMapPoint(destination);

            map.Setup(m => m.GetNextCoords(It.IsAny<Coords>(), It.IsAny<OrientationEnum>()))
                .Returns(() => destination);

            map.Setup(m => m.GetMapPoints())
                .Returns(() => new Dictionary<Coords, IEnumerable<MapPoint>> { { robot.Coords, new MapPoint[0] } });

            MoveForwardCommand.Instance.ExecuteCommand(robot, map.Object);

            robot.Coords.Should().Be(destination);
        }

        /// <summary>
        /// Test: IgnoreIfRobotWillBeLost An instruction to move off the world from a grid point from which a robot 
        /// has been previously lost is simply ignored by the current robot.
        /// </summary>
        [Fact]
        public void IgnoreIfRobotWillBeLost()
        {
            var map = new Mock<IPlanetMap>();
            var initialPosition = Coords.InitialCoords;
            var robot = new Robot(initialPosition, OrientationEnum.North);
            var destination = new Coords(0, 1);
            var lostLandmark = new LostMapPoint(destination);

            map.Setup(m => m.GetNextCoords(It.IsAny<Coords>(), It.IsAny<OrientationEnum>()))
                .Returns(() => destination);

            map.Setup(m => m.GetMapPoints())
                .Returns(() => new Dictionary<Coords, IEnumerable<MapPoint>> { { robot.Coords, new[] { lostLandmark } } });

            MoveForwardCommand.Instance.ExecuteCommand(robot, map.Object);
            robot.Coords.Should().Be(initialPosition);
        }

        /// <summary>
        /// Test: ShouldSetLostMapPoint Robot should have problem. MapPoint should be added
        /// </summary>
        [Fact]
        public void SetLostMapPoint()
        {
            var map = new Mock<IPlanetMap>();
            var initialPosition = Coords.InitialCoords;
            var robot = new Robot(initialPosition, OrientationEnum.North);
            var destination = new Coords(0, 1);
            var lostLandmark = new LostMapPoint(destination);

            map.Setup(m => m.GetNextCoords(It.IsAny<Coords>(), It.IsAny<OrientationEnum>()))
                .Returns(() => destination);

            map.Setup(m => m.GetMapPoints())
                .Returns(() => new Dictionary<Coords, IEnumerable<MapPoint>> { { robot.Coords, new MapPoint[0] } });

            map.Setup(m => m.IsOutOfMapCoords(It.IsAny<Coords>()))
                .Returns(() => true);
            
            MoveForwardCommand.Instance.ExecuteCommand(robot, map.Object); 
            robot.Statuses.OfType<RobotStatusLost>().Should().HaveCount(1);

            map.Verify(mock => mock.AddMapPoint(It.IsAny<Coords>(), It.IsAny<MapPoint>()), Times.Once());
        }
    }
}
