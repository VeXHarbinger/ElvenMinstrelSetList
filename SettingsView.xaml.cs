using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Hearthstone;
using Core = Hearthstone_Deck_Tracker.API.Core;
using System.Windows.Controls.Primitives;
using System.Linq;
using System.Windows.Media;
using System.Windows.Forms;

namespace ElvenMinstrelSetList
{
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
                    Canvas.SetRight(setListDisplay, Settings.Default.SetlistLeft);
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