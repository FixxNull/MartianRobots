using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MartianRobots.App.Extensions;
using MartianRobots.Domain.Enums;
using MartianRobots.Domain.Models;
using MartianRobots.Infrastructure.Abstractions.Robots;
using MartianRobots.Infrastructure.Robots.Commands;

namespace MartianRobots.App
{
    public static class ArgsParser
    {
        /// <summary>
        /// Max allowed instruction length.
        /// </summary>
        public const int MaxInstructionLength = 100;

        /// <summary>
        /// Parses deployment instructions into coordinates and orientarion.
        /// </summary>
        public static (Coords coords, OrientationEnum orientation) ParseDeployment(string input)
        {
            // Check patter
            // N* N* X
            const string pattern = @"^(\d*)\s(\d*)\s(\w)$";
            // Should be 3 positions
            var match = Regex.Match(input, pattern, RegexOptions.IgnoreCase);

            if (!match.Success || match.Groups.Count != 4)
                throw new ArgumentException($"The value '{input}' is not valid.");

            var orientation = match.Groups[3].Value.GetOrientationFromKey();
            var coords = new Coords
            {
                PosX = int.Parse(match.Groups[1].Value),
                PosY = int.Parse(match.Groups[2].Value)
            };

            return (coords, orientation);
        }

        /// <summary>
        /// Parses robot instructions.
        /// </summary>
        public static IEnumerable<IRobotCommand> ParseInstructions(string input)
        {
            // MAX ALLOWED LENGTH 100
            if (input.Length > MaxInstructionLength)
                throw new ArgumentException($"Robot instructions cannot exceed length of {MaxInstructionLength}.");

            var instructions = new List<IRobotCommand>();

            foreach (var c in input.ToUpper())
            {
                switch (c)
                {
                    case 'L':
                        instructions.Add(TurnLeftCommand.Instance);
                        break;

                    case 'R':
                        instructions.Add(TurnRightCommand.Instance);
                        break;

                    case 'F':
                        instructions.Add(MoveForwardCommand.Instance);
                        break;
                }
            }

            return instructions;
        }
    }
}