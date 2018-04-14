using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Core = Hearthstone_Deck_Tracker.API.Core;

namespace ElvenMinstrelSetList
{
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
            Height = 200;
            Width = 230;
            Visibility = Visibility.Visible;
            Canvas.SetTop(this, Settings.Default.SetlistTop);
            Canvas.SetLeft(this, Settings.Default.SetlistLeft);
        }
    }
}