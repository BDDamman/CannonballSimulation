using System.Windows;
using System.Windows.Controls;

namespace Cannon_Simulation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            vmMain = new Main(ref BombRange);
            this.DataContext = vmMain;
        }

        Main vmMain;

        private void SelectedDensity(object sender, SelectionChangedEventArgs e)
        {
            //   After selecting object-density, convert densityText to int
            // for proper calculations.
            vmMain.SetDensity();
        }

        private void FireCannonWithInput(object sender, RoutedEventArgs e)
        {
            // Method will return early if data is missing
            bool doWeHaveData = vmMain.FireCannonWithInput(ref this.BombRange);

            if (doWeHaveData != true)
            {
                // Run code to indicate error(s) with inputted data
            }// Proceed with standard program.
        }

        private void ClearOldData(object sender, RoutedEventArgs e)
        {
            // Null-ifies previousData strings/doubles, then hides previousPanel
            vmMain.ClearOldData(ref BombRange);
        }
    }
}
