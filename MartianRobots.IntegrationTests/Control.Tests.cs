using MartianRobots.Domain.Enums;
using MartianRobots.Domain.Models;
using MartianRobots.Infrastructure;
using MartianRobots.Infrastructure.Abstractions.Maps;
using MartianRobots.Infrastructure.Abstractions.Robots;
using MartianRobots.Infrastructure.Maps;
using MartianRobots.Infrastructure.Robots.Commands;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;
using MartianRobots.Domain.Statuses;
using System.Linq;

namespace MartianRobots.IntegrationTests
{
    public class ControlTests
    {
        /// <summary>
        /// Test execution by include several operations of changing robot position
        /// Test input uses next strategy for robot:
        /// INPUT
        /// 5 3
        /// 1 1 E
        /// RFRFRFRF
        /// 3 2 N
        /// FRRFLLFFRRFLL
        /// 0 3 W
        /// LLFFFLFLFL
        /// OUTPUT
        /// 1 1 E
        /// 3 3 N LOST
        /// 2 3 S
        /// </summary>
        [Fact]
        public void ShouldComputeInstructions()
        {
            var map = new RectangleMap();
            map.Configure(new PlanetMapConfig { Height = 5 + 1, Width = 3 + 1, Origin = Coords.InitialCoords });

            var pairs = new Dictionary<Robot, IRobotCommand[]>
            {
                {
                    new Robot(new Coords(1, 1), OrientationEnum.East),
                    new IRobotCommand[] {
                        TurnRightCommand.Instance,
                        MoveForwardCommand.Instance,
                        TurnRightCommand.Instance,
                        MoveForwardCommand.Instance,
                        TurnRightCommand.Instance,
                        MoveForwardCommand.Instance,
                        TurnRightCommand.Instance,
                        MoveForwardCommand.Instance,
                    }
                },
                {
                    new Robot(new Coords(3, 2), OrientationEnum.North),
                    new IRobotCommand[] {
                        MoveForwardCommand.Instance,
                        TurnRightCommand.Instance,
                        TurnRightCommand.Instance,
                        MoveForwardCommand.Instance,
                        TurnLeftCommand.Instance,
                        TurnLeftCommand.Instance,
                        MoveForwardCommand.Instance,
                        MoveForwardCommand.Instance,
                        TurnRightCommand.Instance,
                        TurnRightCommand.Instance,
                        MoveForwardCommand.Instance,
                        TurnLeftCommand.Instance,
                        TurnLeftCommand.Instance,
                    }
                },
                {
                    new Robot(new Coords(0, 3), OrientationEnum.West),
                    new IRobotCommand[] {
                        TurnLeftCommand.Instance,
                        TurnLeftCommand.Instance,
                        MoveForwardCommand.Instance,
                        MoveForwardCommand.Instance,
                        MoveForwardCommand.Instance,
                        TurnLeftCommand.Instance,
                        MoveForwardCommand.Instance,
                        TurnLeftCommand.Instance,
                        MoveForwardCommand.Instance,
                        TurnLeftCommand.Instance}
                },
            };

            var control = new Control();
            control.InitPlanetMap(map);

            foreach (var pair in pairs)
            {
                control.DeployRobot(pair.Key);
                foreach (var instruction in pair.Value)
                {
                    control.CommandRobot(instruction);
                }
            }

            control.Robots.Should().HaveCount(3);

            control.Robots.Should().ContainSingle(r => r.Coords == new Coords(1, 1, 0) && 
                r.Orientation == OrientationEnum.East);

            control.Robots.Should().ContainSingle(r => r.Coords == new Coords(3, 3, 0) && 
                r.Orientation == OrientationEnum.North && r.Statuses.OfType<RobotStatusLost>().Any());

            control.Robots.Should().ContainSingle(r => r.Coords == new Coords(2, 3, 0) && 
                r.Orientation == OrientationEnum.South);
        }
    }
}
