namespace ElvenMinstrelSetList
{
    using MahApps.Metro.Controls;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for SetListWindow.xaml
    /// </summary>
    public partial class SetListWindow : MetroWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetListWindow"/> class.
        /// </summary>
        public SetListWindow()
        {
            InitializeComponent();
            ConfigSetListDisplay.StartConfigMode();
            Visibility = Visibility.Visible;
            Canvas.SetTop(this, Settings.Default.SetlistTop);
            Canvas.SetLeft(this, Settings.Default.SetlistLeft);
        }
    }
}