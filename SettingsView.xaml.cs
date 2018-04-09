using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Hearthstone;
using Core = Hearthstone_Deck_Tracker.API.Core;
using System.Windows.Controls.Primitives;

namespace ElvenMinstrelSetList
{
    public partial class SettingsView : ScrollViewer
    {
        private static Flyout _flyout;

        public SettingsView()
        {
            InitializeComponent();
            Settings.Default.PropertyChanged += (sender, e) => Settings.Default.Save();
        }

        public static Flyout Flyout
        {
            get
            {
                if (_flyout == null)
                {
                    _flyout = CreateSettingsFlyout();
                }
                return _flyout;
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
            Panel.SetZIndex(settings, 100);
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
            ElvenMinstrelSetList.Input.TestDisplay(((ToggleButton)sender).IsChecked.Value);
        }
    }
}