using System.Windows;
namespace Cannon_Simulation
{
    /// <summary>
    /// Holder for various velocity properties.
    /// </summary>
    class VelocityComponent
    {
        /// <summary>
        /// Contains both Velocity and Velocity Location
        /// </summary>
        /// <param name="velocity">Newly calculated velocity</param>
        /// <param name="vLocation">Location of new velocity</param>
        public VelocityComponent(double velocity, Point vLocation)
        {
            Velocity = velocity;
            velocityLocation = vLocation;
        }

        /// <summary>
        /// The value of Velocity
        /// </summary>
        public double Velocity { get; set; }

        /// <summary>
        /// Current position of Velocity
        /// </summary>
        public Point velocityLocation { get; set; }
    }
}
