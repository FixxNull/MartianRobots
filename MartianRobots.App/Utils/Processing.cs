using MartianRobots.Domain.Models;
using MartianRobots.Infrastructure;
using MartianRobots.Infrastructure.Abstractions.Maps;
using MartianRobots.Infrastructure.Maps;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MartianRobots.App.Utils
{
    class Processing
    {
        #region Private fields
        /// <summary>
        /// Default name of instructions file
        /// </summary>
        private const string _instructionsFile = "instructions.txt";
        /// <summary>
        /// Dictionary of instructions that readed from file
        /// </summary>
        private static Dictionary<string, string>? _parsedInstructions;
        /// <summary>
        /// Flag says has data been read from file and configured
        /// </summary>
        private static bool _isReadAndConfigure;

        #endregion

        #region Public fields
        /// <summary>
        /// Height of map rectangle
        /// </summary>
        public static int Height { get; set; }
       
        /// <summary>
        /// Width of map rectangle
        /// </summary>
        public static int Width { get; set; }

        /// <summary>
        /// Instance of control that allows work with robot instance => deploy robot, 
        /// execute command robot and init planet map
        /// </summary>
        public static Control? Control { get; set; }

        #endregion

        #region Public methods
        /// <summary>
        /// Read configurations from file and create dictionary of instructions
        /// </summary>
        public static void ReadFileAndConfigure() 
        {
            _isReadAndConfigure = false;

            var data = File.ReadAllLines(_instructionsFile, Encoding.UTF8);
            if (!data.Any())
                throw new ApplicationException("No instructions in the file");

            var planetMapDistance = data[0].Split(' ');
            Height = int.Parse(planetMapDistance[0]);
            Width = int.Parse(planetMapDistance[1]);

            _parsedInstructions = new Dictionary<string, string>();

            for (var i = 1; i < data.Length; i += 2)
            {
                if (i + 1 < data.Length)
                    _parsedInstructions.Add(data[i], data[i + 1]);
            }

            var planetConfig = new PlanetMapConfig
            {
                Height = Height + 1,
                Width = Width + 1,
                Origin = Coords.InitialCoords
            };

            var planetMap = new RectangleMap();
            planetMap.Configure(planetConfig);

            Control = new Control();
            Control.InitPlanetMap(planetMap);

            _isReadAndConfigure = true;
        }

        /// <summary>
        /// Deploy instructions to robot instance
        /// </summary>
        public static void Deploy() 
        {
            if (!_isReadAndConfigure)
                throw new ApplicationException("Deploy failed! Robot instructions has not been readed and processing...");

            foreach (var (key, value) in _parsedInstructions)
            {
                Console.WriteLine($"{key}, {value}");

                var (coords, orientation) = ArgsParser.ParseDeployment(key);
                var instructions = ArgsParser.ParseInstructions(value);

                Control?.DeployRobot(new Robot(coords, orientation));
                foreach (var instruction in instructions)
                    Control?.CommandRobot(instruction);
            }
        }
        #endregion
    }
}
