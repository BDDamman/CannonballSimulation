using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Cannon_Simulation
{
    class Main : INotifyPropertyChanged
    {
        public Main(ref Canvas BombRange)
        {
            InitializeClass(ref BombRange);
        }

        private void InitializeClass(ref Canvas BombRange)
        {
            ///Instantiates various objects

            // Lists
            CannonBallDensities = new List<string>();
            BallisticTrajectories = new List<TrajectoryData>();

            // Integer Variables
            CanvasHeight = BombRange.Height;
            CanvasWidth = BombRange.Width;

            // Doubles
            ConvertedCannonBarrelLength = CannonBarrelLength = 0.5;
            ConvertedCannonAngle = CannonAngle = 9;
            MinimumPowder = ConvertedPowderAmount = PowderAmount = 30;

            // Other
            IsThereOldData = new Visibility();

            // Hides the old data strings for tidyness
            IsThereOldData = Visibility.Hidden;

            /*//
					Density values found on:
					http://bit.ly/wikidensities

					Unit Measurement: 
					killogram/metre^3 (kg/m^3)            
			//*/

            // Adding Cannonball Densities to list.
            CannonBallDensities.Add("Oak (710)");
            CannonBallDensities.Add("Concrete (2000)");
            CannonBallDensities.Add("Iron (7870)");
            CannonBallDensities.Add("Lead (11340)");

            CannonballTextDensity = cannonBallDensities[0];
            SetDensity();

            ///\
        }

        /// /// /// /// /// /// ///

        enum ConvertableVariables
        {
            Angle,
            BarrelLength,
            PowderAmount
        }

        private List<string> cannonBallDensities = new List<string>();
        public List<string> CannonBallDensities
        {
            get { return cannonBallDensities; }

            set
            {
                if (value != cannonBallDensities)
                {
                    cannonBallDensities = value;
                    NotifyPropertyChanged("CannonBallDensities");
                }
            }
        }

        private Visibility isThereOldData;
        public Visibility IsThereOldData
        {
            get { return isThereOldData; }

            set
            {
                if (value != isThereOldData)
                {
                    isThereOldData = value;
                    NotifyPropertyChanged("IsThereOldData");
                }
            }
        }

        private double canvasWidth;
        public double CanvasWidth
        {
            get { return canvasWidth; }

            set
            {
                if (value != canvasWidth)
                {
                    canvasWidth = value;
                    NotifyPropertyChanged("CanvasWidth");
                }
            }
        }

        private double canvasHeight;
        public double CanvasHeight
        {
            get { return canvasHeight; }

            set
            {
                if (value != canvasHeight)
                {
                    canvasHeight = value;
                    NotifyPropertyChanged("CanvasHeight");
                }
            }
        }

        private double minimumPowder;
        public double MinimumPowder
        {
            get { return minimumPowder; }

            set
            {
                if (value != minimumPowder)
                {
                    minimumPowder = value;
                    NotifyPropertyChanged("MinimumPowder");
                }
            }
        }

        // Current point list of trajectory, will be used
        // later for creation of visual trajectory
        List<Point> currentPathOfCannonball;

        //   For previous trajectories, including current
        // PointList and Visual Trajectory [Limit 5]
        private List<TrajectoryData> ballisticTrajectories = new List<TrajectoryData>();
        public List<TrajectoryData> BallisticTrajectories
        {
            get { return ballisticTrajectories; }

            set
            {
                if (value != ballisticTrajectories)
                {
                    ballisticTrajectories = value;
                    NotifyPropertyChanged("BallisticTrajectories");
                }
            }
        }

        Point currentCannonballPosition;

        double maximumY;
        double maximumX;
        double oldMaximumY;
        double oldMaximumX;

        string nameOfCannonballType;

        #region Constants

        /* Summary
				Fyrodex, or in proper terms Pyrodex,
				is identical to Pyrodex aside from 
				blasting force.

				Fyrodex is a much smaller explosion
				than real Pyrodex, due to the fact
				that I needed to figure out how much
				force was given off during a gun test.

				Information was taken from the table
				on: http://bit.ly/hgpdfiretest,
				using the Hornady 300-gr XTP Sabot.
		*/
        // Unit measurement: Newtons/gram (N/g)
        public const double Fyrodex = 361;

        // UM: meters/second^2 (m/s^2)
        public const double accGravity = -9.8;

        // Rho Atmosphere  |  Assuming field is room temp.
        // UM: killogram/metre^2 (kg/m^2)
        public const double rhoatm = 1.2;

        // UM: No unit measurement known
        public const double cDrag = 0.47;

        // Powder Volume Constant
        // UM: metre^3/g
        public const double PVC = 0.00000081633;
        #endregion

        #region CurrentData
        private string cannonballTextDensity;
        public string CannonballTextDensity
        {
            get { return cannonballTextDensity; }

            set
            {
                if (value != cannonballTextDensity)
                {
                    cannonballTextDensity = value;
                    NotifyPropertyChanged("CannonballTextDensity");
                }
            }
        }

        private int cannonballIntDensity;
        public int CannonballIntDensity
        {
            get { return cannonballIntDensity; }

            set
            {
                if (value != cannonballIntDensity)
                {
                    cannonballIntDensity = value;
                    NotifyPropertyChanged("CannonballIntDensity");
                }
            }
        }

        private double cannonBarrelLength;
        public double CannonBarrelLength
        {
            get { return cannonBarrelLength; }

            set
            {
                if (value != cannonBarrelLength)
                {
                    cannonBarrelLength = value;
                    NotifyPropertyChanged("CannonBarrelLength");
                    ConvertValue(CannonBarrelLength, ConvertableVariables.BarrelLength);
                }
            }
        }

        private double convertedCannonBarrelLength;
        public double ConvertedCannonBarrelLength
        {
            get { return convertedCannonBarrelLength; }

            set
            {
                if (value != convertedCannonBarrelLength)
                {
                    convertedCannonBarrelLength = value;
                    NotifyPropertyChanged("ConvertedCannonBarrelLength");
                }
            }
        }

        private double cannonAngle;
        public double CannonAngle
        {
            get { return cannonAngle; }

            set
            {
                if (value != cannonAngle)
                {
                    cannonAngle = value;
                    NotifyPropertyChanged("CannonAngle");
                    ConvertValue(CannonAngle, ConvertableVariables.Angle);
                }
            }
        }

        private double displayableCannonAngle;
        public double DisplayableCannonAngle
        {
            get { return displayableCannonAngle; }

            set
            {
                if (value != displayableCannonAngle)
                {
                    displayableCannonAngle = value;
                    NotifyPropertyChanged("DisplayableCannonAngle");
                }
            }
        }

        private double convertedCannonAngle;
        public double ConvertedCannonAngle
        {
            get { return convertedCannonAngle; }

            set
            {
                if (value != convertedCannonAngle)
                {
                    convertedCannonAngle = value;
                    NotifyPropertyChanged("ConvertedCannonAngle");
                }
            }
        }

        private double powderAmount;
        public double PowderAmount
        {
            get { return powderAmount; }

            set
            {
                if (value != powderAmount)
                {
                    powderAmount = value;
                    NotifyPropertyChanged("PowderAmount");
                    ConvertValue(PowderAmount, ConvertableVariables.PowderAmount);
                }
            }
        }

        private double convertedPowderAmount;
        public double ConvertedPowderAmount
        {
            get { return convertedPowderAmount; }

            set
            {
                if (value != convertedPowderAmount)
                {
                    convertedPowderAmount = value;
                    NotifyPropertyChanged("ConvertedPowderAmount");
                }
            }
        }
        #endregion

        #region PreviousData
        private string previousCannonballDensity;
        public string PreviousCannonballDensity
        {
            get { return previousCannonballDensity; }

            set
            {
                if (value != previousCannonballDensity)
                {
                    previousCannonballDensity = value;
                    NotifyPropertyChanged("PreviousCannonballDensity");
                }
            }
        }

        private double previousCannonBarrelLength;
        public double PreviousCannonBarrelLength
        {
            get { return previousCannonBarrelLength; }

            set
            {
                if (value != previousCannonBarrelLength)
                {
                    previousCannonBarrelLength = value;
                    NotifyPropertyChanged("PreviousCannonBarrelLength");
                }
            }
        }

        private double previousCannonAngle;
        public double PreviousCannonAngle
        {
            get { return previousCannonAngle; }

            set
            {
                if (value != previousCannonAngle)
                {
                    previousCannonAngle = value;
                    NotifyPropertyChanged("PreviousCannonAngle");
                }
            }
        }

        private double previousPowderAmount;
        public double PreviousPowderAmount
        {
            get { return previousPowderAmount; }

            set
            {
                if (value != previousPowderAmount)
                {
                    previousPowderAmount = value;
                    NotifyPropertyChanged("PreviousPowderAmount");
                }
            }
        }
        #endregion

        public void SetDensity()
        {
            //Converting String to Int by removing everything but digits
            string resultString = Regex.Replace(CannonballTextDensity,
                    @"[\D]", "", RegexOptions.None);

            //Parsing String to Int
            int output;
            Int32.TryParse(resultString, out output);
            CannonballIntDensity = output;

            switch (CannonballIntDensity)
            {
                case 710:
                    MinimumPowder = 30;
                    nameOfCannonballType = "Oak";
                    break;
                case 2000:
                    MinimumPowder = 60;
                    nameOfCannonballType = "Concrete";
                    break;
                case 7870:
                    MinimumPowder = 200;
                    nameOfCannonballType = "Iron";
                    break;
                case 11340:
                    MinimumPowder = 270;
                    nameOfCannonballType = "Lead";
                    break;
            }

            if (ConvertedPowderAmount < MinimumPowder || PowderAmount < MinimumPowder)
            { ConvertedPowderAmount = PowderAmount = MinimumPowder; }

        }

        public bool FireCannonWithInput(ref Canvas BombRange)
        {
            // Checks if Density has been selected
            if (String.IsNullOrWhiteSpace(CannonballTextDensity))
            {
                return false;
            }// Data is good, continuing function

            // Setting up PreviousData
            PreviousCannonballDensity = CannonballTextDensity;
            PreviousCannonBarrelLength = ConvertedCannonBarrelLength;
            PreviousCannonAngle = ConvertedCannonAngle;
            PreviousPowderAmount = ConvertedPowderAmount;

            //   Shows previous data panel, and removes extra
            // paths if count is high than [5]
            if (IsThereOldData != Visibility.Visible)
            { IsThereOldData = Visibility.Visible; }
            if (BallisticTrajectories.Count >= 5)
            {
                BombRange.Children.Remove(BallisticTrajectories[0].VisualTrajectory);
                BallisticTrajectories.RemoveAt(0);
            }

            /// ||
            /// \/ Pre-Calculation Instantiation

            // Settting eqaution variables to real value
            double barrelLength = ConvertedCannonBarrelLength;
            double angleOfCannon = ConvertedCannonAngle;

            // Integer for counting which point number we are on
            int timeSegmentNumber = 1;

            // Radius of Cannonball; in meters 
            double cannonballRadius = 0.065;

            double Time = 0.01;//in seconds

            bool IsTouchingGround = false;

            // Cannonball object, starts in "cannon"
            EllipseGeometry cannonball =
                            new EllipseGeometry(new Point(0, 0),
                            cannonballRadius, cannonballRadius);

            // Other Declaration
            currentPathOfCannonball = new List<Point>();//x & y values
            List<VelocityComponent> velocitiesOfCannonball =
                    new List<VelocityComponent>();//V, Vx, Vy
            List<double> arcAngles = new List<double>();//Rho value

            /// /\
            /// ||
            /// \/ Discovering location of the lost cannon
            /// 
            ///   Any and all equations with Math.Cos/Sin/Atan, etc
            /// and angleOfCannon/Theta will require conversion 
            /// to radians due to our input being in degrees.
            /// 
            /// xπ/180 | x180/π

            // Calculate starting location of cannonball
            double yStart = canvasHeight - 10.5 - Math.Sin(angleOfCannon * Math.PI / 180) * 5.5;
            double xStart = Math.Cos(angleOfCannon * Math.PI / 180) * 5.5 + 5.5;

            // Assigning starting coords to Point
            currentCannonballPosition = new Point(xStart, yStart);

            //   Y-pos of ground; cannonball calculations stop when 
            // currentY is equal to or greater than groundIdentifier
            double groundIdentifier = -10.5 - (Math.Sin(angleOfCannon * Math.PI / 180) * 5.5);

            /// /\
            /// ||
            /// \/Equations to calculate muzzle/starting Velocity (V)
            ///
            ///   In other words, going from bottom of cannon,
            /// to the end of the barrel and calculating
            /// the velocity it is traveling at.

            // Obtaining Area (A) of cannonball
            double Area = Math.PI * Math.Pow(cannonballRadius, 2);

            // Calculate Force (F) of Fyrodex (Assumed Pyrodex)
            double Force = CalculateAverageForce(Area, cannonballRadius);

            // Constructing volume (v) based upon radius of cannonball
            double volume = (4.0 / 3.0) * Math.PI * Math.Pow(cannonballRadius, 3);

            //   Calculating mass (m) of cannonball;
            // since we know it's density and volume
            double mass = CannonballIntDensity * volume;

            //   Now that mass and Force are calculated, we can now 
            // calculate the acceleration (a) of the cannonball
            double muzzleAcceleration = Force / mass;

            //   Figuring out how long it takes to reach the end of 
            // the barrel in seconds (s)
            double barrelTime = Math.Sqrt(
                            ((2 * barrelLength) / muzzleAcceleration)
                            );

            // Finally, the muzzle/starting Velocity (V)
            double MuzzleVelocity = (muzzleAcceleration * barrelTime);

            // Adding base values to lists to begin final calculations
            velocitiesOfCannonball.Add(
                            new VelocityComponent(MuzzleVelocity, new Point(MuzzleVelocity *
                            Math.Cos(angleOfCannon * Math.PI / 180.0), MuzzleVelocity *
                            Math.Sin(angleOfCannon * Math.PI / 180.0))));

            arcAngles.Add(angleOfCannon);
            currentPathOfCannonball.Add(currentCannonballPosition);

            /// /\
            /// ||
            /// \/ Equations to produce a list of Points for cannonball

            double ForceDrag;                       //Fd
            double Theta;                           //Theta
            double accelDrag;                       //a
            double currentVelocity;                 //V
            Point velocity;                         //Vx & Vy
            Point acceleration;                     //Adx & Ady
                                                    ///currentCannonballPosition            //x & y   

            while (!IsTouchingGround)
            {
                // Calculate the air drag the cannonball will receive (Fd)
                ForceDrag = 0.5 * rhoatm * cDrag *
                                Math.Pow(velocitiesOfCannonball[timeSegmentNumber - 1].Velocity, 2) * Area;

                // Taking ForceDrag by CannonMass to get acceleration (a)
                accelDrag = (ForceDrag / mass) * -1;//To insure it is negative

                // Clensing a point.
                acceleration = new Point();

                // Calculating components for AccelerationDrag (Adx & Ady)
                acceleration.X =
                        Math.Cos(arcAngles[timeSegmentNumber - 1] * Math.PI / 180.0) * accelDrag;

                acceleration.Y =
                        Math.Sin(arcAngles[timeSegmentNumber - 1] * Math.PI / 180.0) * accelDrag;

                // Clensing Points, making anew
                velocity = new Point();
                currentCannonballPosition = new Point();

                // Obtaining location of Velocity (X-pos)
                velocity.X = velocitiesOfCannonball[timeSegmentNumber - 1].velocityLocation.X
                        + acceleration.X * Time;

                // Same thing, but for (Y-pos)
                velocity.Y = velocitiesOfCannonball[timeSegmentNumber - 1].velocityLocation.Y
                        + (acceleration.Y + accGravity) * Time;

                // Velocity (X-pos) must not be less than 0
                if (velocity.X <= 0)
                { velocity.X = 0; }

                // Brand new Velocity coming in
                currentVelocity = Math.Sqrt(Math.Pow(velocity.X, 2)
                        + Math.Pow(velocity.Y, 2));

                // Calculate new position, first X, then Y
                // With Y-pos, calculate with gravity (accGravity)
                currentCannonballPosition.X = currentPathOfCannonball[timeSegmentNumber - 1].X
                                + velocitiesOfCannonball[timeSegmentNumber - 1].velocityLocation.X * Time
                                + 0.5 * acceleration.X
                                * Math.Pow(Time, 2);

                currentCannonballPosition.Y = currentPathOfCannonball[timeSegmentNumber - 1].Y
                                + velocitiesOfCannonball[timeSegmentNumber - 1].velocityLocation.Y * Time
                                + 0.5 * (acceleration.Y + accGravity)
                                * Math.Pow(Time, 2);

                currentPathOfCannonball.Add(currentCannonballPosition);

                // Figure out new angle/Theta with new X & Y
                Theta = Math.Atan(
                                ((currentPathOfCannonball[timeSegmentNumber].Y
                                - currentPathOfCannonball[timeSegmentNumber - 1].Y)
                                / (currentPathOfCannonball[timeSegmentNumber].X
                                - currentPathOfCannonball[timeSegmentNumber - 1].X)));

                // Add all new items to list
                velocitiesOfCannonball.Add(new VelocityComponent(currentVelocity, velocity));
                arcAngles.Add(Theta * 180 / Math.PI);

                if (currentCannonballPosition.Y <= groundIdentifier)
                {
                    IsTouchingGround = true;
                }

                // Next time segment please
                timeSegmentNumber++;
            }

            /// /\
            /// ||
            /// \/ Finalizing cannonball trajectory; Calculating canvas dimensions

            FindHighestValue(currentPathOfCannonball);

            BombRange.Height = maximumY + (maximumY * 0.01);
            BombRange.Width = maximumX + (maximumX * 0.01);

            //if (maximumY > (oldMaximumY + 500) || maximumX > (oldMaximumX + 500))
            //{
            //    RedrawAllTrajectories(ref BombRange);
            //}


            oldMaximumX = maximumX;
            oldMaximumY = maximumY;

            //   Cannonball trajectory is now complete, now we need 
            // create a visual trajectory and display it.

            /// /\
            /// ||
            /// \/ Final segment of code; Drawing the Trajectory


            LineSegment lineseg;
            PathFigure pf = new PathFigure();
            Path pth = new Path();
            PathGeometry gm = new PathGeometry();

            pf.StartPoint = currentPathOfCannonball[0];
            for (int i = 1; i < currentPathOfCannonball.Count; i++)
            {
                lineseg = new LineSegment(currentPathOfCannonball[i], true);
                pf.Segments.Add(lineseg);
            }

            gm.Figures.Add(pf);
            pth.Data = gm;

            TrajectoryData td = new TrajectoryData(currentPathOfCannonball, pth);

            td.DistanceTraveled = Math.Round(currentPathOfCannonball[currentPathOfCannonball.Count - 1].X, 3);

            double mY = -1;
            foreach (Point pt in td.PathOfCannonball)
            {
                if (pt.Y > mY)
                {
                    mY = Math.Round(pt.Y, 3);
                }
            }

            td.HighestAltitudeAchieved = mY;
            td.VelocityOfImpact = Math.Round(velocitiesOfCannonball[velocitiesOfCannonball.Count - 1].Velocity, 3);
            td.MuzzleVelocity = Math.Round(MuzzleVelocity, 3);
            td.CannonballType = nameOfCannonballType;
            //td.IndexNumber = BallisticTrajectories.Count - 1;

            BallisticTrajectories.Add(td);

            ////   Applying thickness at just the correct size depending
            //// upon trajectory along with correct color
            //pth.StrokeThickness = GetReasonableStrokeThickness(ref BombRange);
            //pth.Stroke = GetReasonablePathColor(BallisticTrajectories.Count - 1);

            ScaleTransform st = new ScaleTransform(1.0, -1.0);
            BombRange.RenderTransformOrigin = new Point(0, 0.5);
            BombRange.Children[0].Visibility = Visibility.Hidden;

            BombRange.RenderTransform = st;

            BombRange.Children.Add(pth);
            RedrawAllTrajectories(ref BombRange);

            /// /\
            /// \/ Beam us back, trueward!

            return true;
        }

        public void ClearOldData(ref Canvas BombRange)
        {
            //Dropping all old data
            BombRange.Children[0].Visibility = Visibility.Visible;
            PreviousCannonballDensity = string.Empty;
            PreviousCannonBarrelLength = 0.0;
            PreviousCannonAngle = 0.0;
            PreviousPowderAmount = 0.0;
            BombRange.Width = BombRange.Height = 12;

            //Vanishing the PreviousData panel
            if (IsThereOldData != Visibility.Collapsed)
            { IsThereOldData = Visibility.Collapsed; }

            foreach (TrajectoryData td in BallisticTrajectories)
            {
                BombRange.Children.Remove(td.VisualTrajectory);
            }

            BombRange.RenderTransform = null;

            BallisticTrajectories.Clear();
        }

        private void ConvertValue(double convertable, ConvertableVariables cvn)
        {
            //   Converting the value from double, 
            // to double with with/out decimal points

            switch (cvn)
            {
                case ConvertableVariables.Angle:
                    ConvertedCannonAngle = Math.Round(convertable);
                    DisplayableCannonAngle = Math.Round((convertable * -1));
                    break;

                case ConvertableVariables.BarrelLength:
                    ConvertedCannonBarrelLength = Math.Round(convertable, 1);
                    break;

                case ConvertableVariables.PowderAmount:
                    ConvertedPowderAmount = Math.Round(convertable);
                    break;
            }
        }

        private void FindHighestValue(List<Point> cannonballPath)
        {
            maximumX = 0;
            maximumY = 0;

            if (BallisticTrajectories.Count > 0)
            {
                foreach (TrajectoryData td in BallisticTrajectories)
                {
                    foreach (Point pt in td.PathOfCannonball)
                    {
                        if (pt.Y > maximumY)
                        {
                            maximumY = pt.Y;
                        }
                    }

                    if (td.PathOfCannonball[td.PathOfCannonball.Count - 1].X >= maximumX)
                    {
                        maximumX = td.PathOfCannonball[td.PathOfCannonball.Count - 1].X;
                    }
                }
            }

            foreach (Point pt in cannonballPath)
            {
                if (pt.Y > maximumY)
                {
                    maximumY = pt.Y;
                }

                if (pt.X > maximumX)
                {
                    maximumX = pt.X;
                }
            }
        }

        private void RedrawAllTrajectories(ref Canvas BombRange)
        {
            int counter = 0;
            List<TrajectoryData> tempList = new List<TrajectoryData>();

            foreach (TrajectoryData td in BallisticTrajectories)
            {
                BombRange.Children.Remove(td.VisualTrajectory);

                td.VisualTrajectory.StrokeThickness = GetReasonableStrokeThickness(ref BombRange);
                td.VisualTrajectory.Stroke = GetReasonablePathColor(counter);
                td.IndexNumber = counter;

                BombRange.Children.Add(td.VisualTrajectory);
                tempList.Add(td);
                counter++;
            }

            BallisticTrajectories.Clear();
            BallisticTrajectories = tempList;
        }

        private double GetReasonableStrokeThickness(ref Canvas BombRange)
        {
            // Calculates thickness two ways; one using Width, another with Height
            double possibleThicknessY = 3 / App.Current.MainWindow.ActualHeight * BombRange.Height;
            double possibleThicknessX = 3 / App.Current.MainWindow.ActualWidth * BombRange.Width;

            // Determines which is greater, then returns the larger value
            if (possibleThicknessY > possibleThicknessX)
            {
                // Y is greater
                return possibleThicknessY;
            }
            else
            {
                // No, X is
                return possibleThicknessX;
            }
        }

        private SolidColorBrush GetReasonablePathColor(int colorPosition)
        {
            Color clr = new Color();

            switch (colorPosition)
            {
                case 0:
                    clr = (Color)ColorConverter.ConvertFromString("#FFed754e");
                    return new SolidColorBrush(clr);
                case 1:
                    clr = (Color)ColorConverter.ConvertFromString("#FF31af28");
                    return new SolidColorBrush(clr);
                case 2:
                    clr = (Color)ColorConverter.ConvertFromString("#FFed4e75");
                    return new SolidColorBrush(clr);
                case 3:
                    clr = (Color)ColorConverter.ConvertFromString("#FF9b35d6");
                    return new SolidColorBrush(clr);
                default:
                case 4:
                    clr = (Color)ColorConverter.ConvertFromString("#FFbaed4e");
                    return new SolidColorBrush(clr);
            }

            /*
			#FFed754e
			#FF31af28
			#FFed4e75
			#FF9b35d6
			#FFbaed4e
			*/
        }

        private double CalculateAverageForce(double AreaOfCannonball, double cannonballRadius)
        {
            double F1 = ConvertedPowderAmount * Fyrodex;
            double V1 = PVC * ConvertedPowderAmount;
            double V2 = Math.PI * Math.Pow(cannonballRadius, 2) * ConvertedCannonBarrelLength;
            double P1 = F1 / AreaOfCannonball;
            double P2 = P1 * V1 / V2;
            double F2 = P2 * AreaOfCannonball;

            double Favg = (F1 + F2) / 2;

            return Favg;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
