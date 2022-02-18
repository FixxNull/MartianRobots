using System;
using System.Collections.Generic;
using System.Linq;
using MartianRobots.Domain.Enums;
using MartianRobots.Domain.Models;
using MartianRobots.Infrastructure.Abstractions.Maps;

namespace MartianRobots.Infrastructure.Maps
{
    public class RectangleMap : IPlanetMap
    {
        #region Public fields

        public Coords Origin { get; private set; }
        public Coords TopRight { get; private set; }

        #endregion

        #region Private fields

        private readonly IDictionary<Coords, List<MapPoint>> _mapPoints = new Dictionary<Coords, List<MapPoint>>();
        private bool Configured { get; set; }

        #endregion

        #region Public methods
        /// <summary>
        /// Setup configurations
        /// </summary>
        /// <param name="planetMapConfig">Configurations of planet map</param>
        public void Configure(PlanetMapConfig planetMapConfig)
        {
            TopRight = new Coords(planetMapConfig.Height - 1, planetMapConfig.Width - 1);
            Origin = planetMapConfig.Origin;
            Configured = true;
        }

        /// <summary>
        /// Add point to planet map
        /// </summary>
        /// <param name="coordinates"></param>
        /// <param name="mapPoint"></param>
        public void AddMapPoint(Coords coordinates, MapPoint mapPoint)
        {
            if (!_mapPoints.ContainsKey(coordinates))
                _mapPoints.Add(coordinates, new List<MapPoint>());
            
            _mapPoints[coordinates].Add(mapPoint);
        }

        /// <summary>
        /// Gets map coordinate points
        /// </summary>
        public IDictionary<Coords, IEnumerable<MapPoint>> GetMapPoints() => 
            _mapPoints.ToDictionary(p => p.Key, p => p.Value.AsEnumerable());

        /// <summary>
        /// Get next coordinates from position and orientation.
        /// North is Y+1, not X+1
        /// X axis for W-E
        /// Y axis for N-S
        /// </summary>
        public Coords GetNextCoords(Coords coordinates, OrientationEnum orientation)
        {
            IsConfigured();

            const int chunkAdvances = 1;

            var xAmmount = orientation is OrientationEnum.East or OrientationEnum.West 
                ? chunkAdvances 
                : 0;

            var yAmmount = orientation is OrientationEnum.North or OrientationEnum.South 
                ? chunkAdvances 
                : 0;

            switch (orientation)
            {
                // South or West are decrements in coordinate
                case OrientationEnum.South:
                    yAmmount *= -1;
                    break;

                case OrientationEnum.West:
                    xAmmount *= -1;
                    break;
            }

            return coordinates + new Coords(xAmmount, yAmmount);
        }

        /// <summary>
        /// Turn left by input orientation
        /// </summary>
        /// <param name="orientation"></param>
        /// <returns></returns>
        public OrientationEnum TurnLeft(OrientationEnum orientation) 
            => Turn(orientation, -1);

        /// <summary>
        /// Turn right by input orientation
        /// </summary>
        /// <param name="orientation"></param>
        /// <returns></returns>
        public OrientationEnum TurnRight(OrientationEnum orientation) 
            => Turn(orientation, 1);

        /// <summary>
        /// Validate coordinates for limited bounds of map
        /// </summary>
        /// <param name="coords"></param>
        /// <returns></returns>
        public bool IsOutOfMapCoords(Coords coords)
        {
            IsConfigured();

            if (coords.PosX < Origin.PosX || coords.PosX > TopRight.PosX) 
                return true;

            if (coords.PosY < Origin.PosY || coords.PosY > TopRight.PosY) 
                return true;

            return false;
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Turn n times, clockwise
        /// </summary>
        /// <param name="orientation">Current orientation instance</param>
        /// <param name="times">Times</param>
        /// <returns></returns>
        private OrientationEnum Turn(OrientationEnum orientation, int times)
        {
            IsConfigured();

            var min = (int)Enum.GetValues(typeof(OrientationEnum)).Cast<OrientationEnum>().Min();
            var max = (int)Enum.GetValues(typeof(OrientationEnum)).Cast<OrientationEnum>().Max();

            var next = (int)orientation + times;
            if (next > max) 
                next = min;

            if (next < min) 
                next = max;

            return (OrientationEnum)next;
        }

        private void IsConfigured()
        {
            if (!Configured)
                throw new InvalidOperationException("Map is not configured yet. You should call Configure first.");
        }
        #endregion
    }
}