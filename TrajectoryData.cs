using System.Collections.Generic;
using System.Windows;
using System.Windows.Shapes;

namespace Cannon_Simulation
{
    class TrajectoryData
    {
        /// <summary>
        /// Container for the Path o' Points and visual UIElement.
        /// </summary>
        /// <param name="poc">The current path of points for cannonball.</param>
        /// <param name="vt">Path with full trajectory data.</param>
        public TrajectoryData(List<Point> poc, Path vt)
        {
            PathOfCannonball = new List<Point>();
            PathOfCannonball = poc;

            VisualTrajectory = vt;
        }

        public List<Point> PathOfCannonball { get; set; }

        public Path VisualTrajectory { get; set; }

        public double DistanceTraveled { get; set; }

        public double HighestAltitudeAchieved { get; set; }

        public double VelocityOfImpact { get; set; }

        public double MuzzleVelocity { get; set; }

        public int IndexNumber { get; set; }

        public string CannonballType { get; set; }
    }
}
