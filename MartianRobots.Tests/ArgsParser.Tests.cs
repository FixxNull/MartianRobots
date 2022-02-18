using FluentAssertions;
using MartianRobots.Domain.Enums;
using MartianRobots.Domain.Models;
using MartianRobots.Infrastructure.Robots.Commands;
using System;
using MartianRobots.App;
using Xunit;

namespace MartianRobots.Tests
{
    public class ArgsParserTest
    {
        [Theory]
        [InlineData("1 2 N", 1, 2, OrientationEnum.North)]
        [InlineData("2 3 S", 2, 3, OrientationEnum.South)]
        [InlineData("3 4 E", 3, 4, OrientationEnum.East)]
        [InlineData("4 5 W", 4, 5, OrientationEnum.West)]
        public void ShouldParseDeployInstructions(string input, int x, int y, OrientationEnum orientation)
        {
            var (coords, orientationEnum) = ArgsParser.ParseDeployment(input);

            coords.Should().Be(new Coords(x, y));
            orientationEnum.Should().Be(orientation);
        }

        [Theory]
        [InlineData("")]
        [InlineData("46gt s")]
        [InlineData("1 1")]
        [InlineData("11 N")]
        [InlineData("1111v")]
        public void ShouldThrowOnInvalidInput(string input)
        {
            Action act = () => ArgsParser.ParseDeployment(input);

            act.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData("L", typeof(TurnLeftCommand))]
        [InlineData("R", typeof(TurnRightCommand))]
        [InlineData("F", typeof(MoveForwardCommand))]
        public void ShouldParseRobotInstructions(string input, Type type)
        {
            var result = ArgsParser.ParseInstructions(input);

            result.Should().ContainSingle(i => i.GetType() == type);
        }

        /// <summary>
        /// All instruction strings will be less than 100 characters in length
        /// </summary>
        [Fact]
        public void ShouldThrowIfInstructionLengthOverMax()
        {
            var input = "".PadLeft(ArgsParser.MaxInstructionLength + 1, 'X');
            Action act = () => ArgsParser.ParseInstructions(input);

            act.Should().Throw<ArgumentException>();
        }
    }
}
