using System;
using FluentAssertions;
using MartianRobots.Domain.Models;
using Xunit;

namespace MartianRobots.Tests.Domain
{
    public class CoordsTests
    {
        /// <summary>
        /// Test: ShouldImplementEqualsOperator check work of overrided Equal method
        /// </summary>
        [Fact]
        public void ShouldImplementEquals()
        {
            var arrange = new Coords(1, 2, 3);
            var expected = new Coords(1, 2, 3);

            arrange.Equals(expected).Should().BeTrue();
        }

        /// <summary>
        /// Test: ShouldImplementSumOperator check calculation by override (+) operator for properties 
        /// in every instance of coordinates
        /// </summary>
        [Theory]
        [InlineData(1, 2, 3, 1, 1, 1, 2, 3, 4)]
        [InlineData(1, 0, 0, 1, 0, 0, 2, 0, 0)]
        [InlineData(0, 1, 0, 0, 1, 0, 0, 2, 0)]
        [InlineData(0, 0, 1, 0, 0, 1, 0, 0, 2)]
        public void ShouldImplementSumOperator(int xA, int yA, int zA, int xB, int yB, int zB, int xR, int yR, int zR)
        {
            var result = new Coords(xA, yA, zA) + new Coords(xB, yB, zB);

            result.Should().Be(new Coords(xR, yR, zR));
        }

        /// <summary>
        /// Test: ShouldImplementEqualsOperator check work of overrided Equal operator
        /// </summary>
        [Fact]
        public void ShouldImplementEqualsOperator()
        {
            var arrange = new Coords(1, 2, 3);
            var expected = new Coords(1, 2, 3);

            var result = arrange == expected;

            result.Should().BeTrue();
        }

        /// <summary>
        /// Test: ShouldImplementEqualsOperator check work of overrided NotEqual operator
        /// </summary>
        [Fact]
        public void ShouldImplementNotEqualsOperator()
        {
            var arrange = new Coords(1, 2, 3);
            var expected = new Coords(3, 2, 1);

            var result = arrange != expected;

            result.Should().BeTrue();
        }

        /// <summary>
        /// Test: ShouldNotAllowOverMax check pos value that should be fewer than 50
        /// The maximum value for any coordinate is 50
        /// </summary>
        [Fact]
        public void ShouldNotAllowOverMax()
        {
            Action act = () => new Coords(Coords.MaxPos + 1, 0, 0);

            act.Should().Throw<ArgumentException>();

            act = () => new Coords(0, Coords.MaxPos + 1, 0);

            act.Should().Throw<ArgumentException>();

            act = () => new Coords(0, 0, Coords.MaxPos + 1);

            act.Should().Throw<ArgumentException>();
        }
    }
}