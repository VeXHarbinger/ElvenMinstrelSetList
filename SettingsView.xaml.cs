using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using Hearthstone_Deck_Tracker;

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

        private void BtnUnlock_Click(object sender, RoutedEventArgs e)
        {
            ElvenMinstrelSetList.Input.ToggleVisibility();

            BtnUnlock.Content = ElvenMinstrelSetList.Input.Toggle() ?
                "Lock Minstrel Set List" :
                "Unlock Minstrel Set List";
        }
    }
}