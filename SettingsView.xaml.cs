namespace ElvenMinstrelSetList
{
    using MahApps.Metro.Controls;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using Core = Hearthstone_Deck_Tracker.API.Core;

    /// <summary>
    /// The HDT Plug-in <see cref="SettingsView">Settings View</see> for user plugin configuration
    /// </summary>
    /// <seealso cref="System.Windows.Controls.ScrollViewer"/>
    /// <seealso cref="System.Windows.Markup.IComponentConnector"/>
    public partial class SettingsView : ScrollViewer
    {
        private static Flyout flyout;

        /// <summary>
        /// The <see cref="SetListDisplay"/> reference
        /// </summary>
        private SetListDisplay setListDisplay;

        /// <summary>
        /// The <see cref="SetListWindow"/> reference
        /// </summary>
        private SetListWindow setListWindow;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsView"/> class.
        /// </summary>
        public SettingsView()
        {
            InitializeComponent();
            Settings.Default.PropertyChanged += (sender, e) => Settings.Default.Save();
        }

        /// <summary>
        /// Gets the <see cref="Flyout"/> control.
        /// </summary>
        /// <value>The <see cref="Flyout"/> control.</value>
        public static Flyout Flyout
        {
            get
            {
                if (flyout == null)
                {
                    flyout = CreateSettingsFlyout();
                }
                return flyout;
            }
        }

        /// <summary>
        /// Updates the panel position.
        /// </summary>
        public void UpdatePosition()
        {
            Canvas.SetTop(this, Core.OverlayWindow.Height * 5 / 100);
            Canvas.SetRight(this, Core.OverlayWindow.Width * 20 / 100);
        }

        /// <summary>
        /// Creates the settings flyout.
        /// </summary>
        /// <returns></returns>
        private static Flyout CreateSettingsFlyout()
        {
            var settings = new Flyout();
            settings.Position = Position.Left;
            System.Windows.Controls.Panel.SetZIndex(settings, 100);
            settings.Header = "Minstrel Set List Settings";
            settings.Content = new SettingsView();
            Core.MainWindow.Flyouts.Items.Add(settings);
            return settings;
        }

        /// <summary>
        /// Handles the button lock/unlock clicks.
        /// </summary>
        /// <param name="sender">The <see cref="Button"/> sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void BtnUnlock_Click(object sender, RoutedEventArgs e)
        {
            if (((ToggleButton)sender).IsChecked.Value)
            {
                setListDisplay = FindSetListDisplay();
                if (setListDisplay != null)
                {
                    setListDisplay.Hide();
                }

                setListWindow = new SetListWindow();
                setListWindow.Activate();
            }
            else
            {
                setListDisplay = FindSetListDisplay();
                if (setListWindow != null && setListDisplay != null)
                {
                    Settings.Default.SetlistTop = setListWindow.Top;
                    Settings.Default.SetlistLeft = setListWindow.Left;
                    Canvas.SetTop(setListDisplay, Settings.Default.SetlistTop);
                    Canvas.SetLeft(setListDisplay, Settings.Default.SetlistLeft);
                    setListWindow.Close();
                }
            }
        }

        /// <summary>
        /// Get's the reference to our panel on the canvas.
        /// </summary>
        /// <returns></returns>
        private SetListDisplay FindSetListDisplay()
        {
            return Core.OverlayCanvas.Children.OfType<SetListDisplay>().FirstOrDefault();
        }
    }
}