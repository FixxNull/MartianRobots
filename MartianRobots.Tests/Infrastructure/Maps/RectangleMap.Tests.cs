using FluentAssertions;
using MartianRobots.Domain.Enums;
using MartianRobots.Domain.Models;
using MartianRobots.Infrastructure.Abstractions.Maps;
using MartianRobots.Infrastructure.Maps;
using Xunit;

namespace MartianRobots.Tests.Infrastructure.Maps
{
    public class RectangleMapTests
    {
        public RectangleMap Map { get; set; }

        /// <summary>
        /// Initialize parameters in constructor for using 
        /// them before each test
        /// </summary>
        public RectangleMapTests()
        {
            Map = new RectangleMap();
            var config = new PlanetMapConfig
            {
                Origin = Coords.InitialCoords,
                Height = 5,
                Width = 5,
            };
            Map.Configure(config);
        }

        /// <summary>
        /// Test: SetTopRightCoords
        /// If grid is 5x5 size, origin is 0,0 and top right is 4,4
        /// </summary>
        [Fact]
        public void SetTopRightCoords()
            => Map.TopRight.Should().Be(new Coords(4, 4));

        /// <summary>
        /// Test: MoveNorthIncrementYAxis The direction North corresponds to the 
        /// direction from grid point (x, y) to grid point (x, y+1)
        /// </summary>
        [Fact]
        public void MoveNorthIncrementYAxis()
        {
            // Arrange
            var initialCoords = new Coords(1, 1);

            // Act
            var result = Map.GetNextCoords(initialCoords, OrientationEnum.North);

            // Assert
            result.Should().Be(new Coords(1, 2));
        }

        /// <summary>
        /// Test: ShouldGetNextCoords Forward: the robot moves forward one grid point 
        /// in the direction of the current orientation and maintains the same orientation
        /// </summary>
        /// <param name="orientation"></param>
        /// <param name="xExpected"></param>
        /// <param name="yExpected"></param>
        [Theory]
        [InlineData(OrientationEnum.North, 1, 2)]
        [InlineData(OrientationEnum.South, 1, 0)]
        [InlineData(OrientationEnum.East, 2, 1)]
        [InlineData(OrientationEnum.West, 0, 1)]
        public void ShouldGetNextCoords(OrientationEnum orientation, int xExpected, int yExpected)
        {
            var initialCoords = new Coords(1, 1);
            var result = Map.GetNextCoords(initialCoords, orientation);

            result.Should().Be(new Coords(xExpected, yExpected));
        }

        /// <summary>
        /// Test: ShouldTurnLeft Left direction: robot turns left 90 
        /// degrees and remains on the current grid point
        /// </summary>
        /// <param name="orientation"></param>
        /// <param name="expected"></param>
        [Theory]
        [InlineData(OrientationEnum.North, OrientationEnum.West)]
        [InlineData(OrientationEnum.South, OrientationEnum.East)]
        [InlineData(OrientationEnum.East, OrientationEnum.North)]
        [InlineData(OrientationEnum.West, OrientationEnum.South)]
        public void ShouldTurnLeft(OrientationEnum orientation, OrientationEnum expected)
        {
            var result = Map.TurnLeft(orientation);

            result.Should().Be(expected);
        }

        /// <summary>
        /// Test: ShouldTurnRight Right direction: robot turns right 90 degrees 
        /// and remains on the current grid point
        /// </summary>
        /// <param name="orientation"></param>
        /// <param name="expected"></param>
        [Theory]
        [InlineData(OrientationEnum.North, OrientationEnum.East)]
        [InlineData(OrientationEnum.South, OrientationEnum.West)]
        [InlineData(OrientationEnum.East, OrientationEnum.South)]
        [InlineData(OrientationEnum.West, OrientationEnum.North)]
        public void ShouldTurnRight(OrientationEnum orientation, OrientationEnum expected)
        {
            var result = Map.TurnRight(orientation);

            result.Should().Be(expected);
        }

        /// <summary>
        /// Test: ShouldFindOutOfMapCoords Direction North corresponds to the direction 
        /// from grid point (x, y) to grid point (x, y+1)
        /// </summary>
        [Fact]
        public void ShouldFindOutOfMapCoords()
        {
            var outOfBounds = Map.TopRight + new Coords(1, 1);
            var result = Map.IsOutOfMapCoords(outOfBounds);

            result.Should().BeTrue();
        }

        /// <summary>
        /// Test: ShouldFindAllowedCoords Direction North corresponds to the direction from grid 
        /// point (x, y) to grid point (x, y+1)
        /// </summary>
        [Fact]
        public void ShouldFindAllowedCoords()
        {
            var outOfBounds = Map.Origin + new Coords(1, 1);
            var result = Map.IsOutOfMapCoords(outOfBounds);

            result.Should().BeFalse();
        }
    }
}
