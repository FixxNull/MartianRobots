using System.Linq;
using System.Text;
using MartianRobots.Infrastructure;

namespace MartianRobots.App.Extensions
{
    public static class ControlExtensions
    {
        /// <summary>
        /// Gets a status report for the deployed robots.
        /// </summary>
        public static string GetStatusReport(this Control control)
        {
            var builder = new StringBuilder();

            foreach (var robot in control.Robots)
            {
                builder
                    .Append(robot.Coords.PosX)
                    .Append(' ')
                    .Append(robot.Coords.PosY)
                    .Append(' ')
                    .Append(robot.Orientation.GetKeyCode());

                var statuses = robot.Statuses
                    .Select(s => s.GetStatusCode())
                    .ToArray();

                if (statuses.Any())
                {
                    builder.Append(' ');
                    builder.AppendJoin(' ', statuses);
                }

                builder.AppendLine();
            }

            return builder.ToString();
        }
    }
}