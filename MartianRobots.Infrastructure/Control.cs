using System;
using System.Collections.Generic;
using MartianRobots.Domain.Models;
using MartianRobots.Infrastructure.Abstractions.Maps;
using MartianRobots.Infrastructure.Abstractions.Robots;

namespace MartianRobots.Infrastructure
{
    public class Control
    {
        #region Private fields
        /// <summary>
        /// List of deployed robots
        /// </summary>
        private readonly List<Robot> _deployedRobots = new();
        /// <summary>
        /// Current instance of Robot
        /// </summary>
        private Robot? CurrentRobot { get; set; }
        /// <summary>
        /// Instance of Planet Map
        /// </summary>
        private IPlanetMap? PlanetMap { get; set; }

        #endregion

        #region Public methods
        /// <summary>
        /// Collection of robots deployed to the map.
        /// </summary>
        public IReadOnlyCollection<Robot> Robots 
            => _deployedRobots.AsReadOnly();

        /// <summary>
        /// Creates an instance.
        /// </summary>
        public void InitPlanetMap(IPlanetMap planetMap)
            => PlanetMap = planetMap;

        /// <summary>
        /// Deploys a robot on planet surface and sets it as current controlled robot.
        /// </summary>
        /// <param name="robot">Instance of Robot</param>
        public void DeployRobot(Robot robot)
        {
            _deployedRobots.Add(robot);
            CurrentRobot = robot;
        }

        /// <summary>
        /// Commands the current robot to perform actions.
        /// </summary>
        /// <param name="command"></param>
        public void CommandRobot(IRobotCommand command)
        {
            if (CurrentRobot is null)
                throw new InvalidOperationException("There is no robot deployed yet.");
            
            command.ExecuteCommand(CurrentRobot, PlanetMap!);
        }
        #endregion
    }
}