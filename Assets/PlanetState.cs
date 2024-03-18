using UnityEngine;

namespace DefaultNamespace
{
    public class PlanetState
    {
        public string Name { get; }
        public Vector3d Velocity { get; }
        public PlanetState Orbiting { get; }
        public const double G = 6.67430E-11f;

        private double _mu;
        /// <summary>
        /// Standard gravitation parameter
        /// Units are .1km^2 * .1km/s^2
        ///
        /// Mars is 6776km in diameter, so the surface is ~ 3000 km from the center. At the surface of an equivalent planet
        /// I want the acceleration to be .050 km/s2.
        /// Ex: Earth is 3.986E5
        /// </summary>
        public double Mu => _mu; 
    
        public double Mass => _mu / G;

        public Vector3d Position;

        public PlanetState(string name, Vector3d position, Vector3d velocity, PlanetState orbiting)
        {
            Position = position;
            Name = name;
            Velocity = velocity;
            Orbiting = orbiting;
        }
    }
}