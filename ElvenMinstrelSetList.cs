namespace ElvenMinstrelSetList
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Linq;
    using Hearthstone_Deck_Tracker;
    using Hearthstone_Deck_Tracker.API;
    using Core = Hearthstone_Deck_Tracker.API.Core;
    using Card = Hearthstone_Deck_Tracker.Hearthstone.Card;
    using Hearthstone_Deck_Tracker.Enums;

    /// <summary>
    /// Elven Minstrel Set List
    /// </summary>
    public class ElvenMinstrelSetList
    {
        /// <summary>
        /// The <see cref="InputManager"/> object reference
        /// </summary>
        public static InputManager Input;

        /// <summary>
        /// The <see cref="SetListView"/> object reference
        /// </summary>
        public SetListView setListView;

        /// <summary>
        /// Gets the minstrel card object reference.
        /// </summary>
        /// <value>The minstrel card object reference.</value>
        private static string minstrelCardId = "LOOT_211";

        /// <summary>
        /// Initializes a new instance of the <see cref="ElvenMinstrelSetList"/> class.
        /// </summary>
        public ElvenMinstrelSetList()
        {
            // Create set List container
            setListView = new SetListView();
            setListView.Orientation = Orientation.Vertical;
            ResetSetList();
            Core.OverlayCanvas.Children.Add(setListView);
            Canvas.SetTop(setListView, Settings.Default.SetlistTop);
            Canvas.SetLeft(setListView, Settings.Default.SetlistLeft);
            Input = new InputManager(setListView);

            if (Config.Instance.HideInMenu && Core.Game.IsInMenu)
                setListView.Visibility = Visibility.Hidden;

            Settings.Default.PropertyChanged += SettingsChanged;
            SettingsChanged(null, null);

            // Game events
            GameEvents.OnGameStart.Add(ResetSetList);
            GameEvents.OnGameEnd.Add(ResetSetList);
            GameEvents.OnTurnStart.Add(ResetSetList);
            GameEvents.OnPlayerDraw.Add(ResetSetList);

            // Mouse
            GameEvents.OnPlayerHandMouseOver.Add(PlayerHandMouseOver);
            GameEvents.OnMouseOverOff.Add(OnMouseOff);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            Core.OverlayCanvas.Children.Remove(setListView);
            Input.Dispose();
        }

        /// <summary>
        /// Generates the percents for the current minions.
        /// </summary>
        private void GeneratePercents()
        {
            var playerDeck = Core.Game.Player.PlayerCardList
                .Where(c => c.Type == "Minion" && (c.Count - c.InHandCount) > 0);

            if (playerDeck.Count() != setListView.Cards.Count())
            {
                setListView.Reset();
                foreach (Card card in playerDeck)
                {
                    setListView.Update(card);
                }
                setListView.StatsLabel.Text = $"2/{playerDeck.Count()}";
            }
        }

        /// <summary>
        /// Called when [mouse off].
        /// </summary>
        private void OnMouseOff()
        {
            setListView.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Called when [mouse over].
        /// </summary>
        /// <param name="card">The card.</param>
        private void PlayerHandMouseOver(Card card)
        {
            if (card.Id == minstrelCardId)
            {
                GeneratePercents();
                setListView.Visibility = Visibility.Visible;
                setListView.BringIntoView();
            }
        }

        /// <summary>
        /// Resets the set list.
        /// </summary>
        /// <param name="obj">The object.</param>
        private void ResetSetList(ActivePlayer obj)
        {
            ResetSetList();
        }

        /// <summary>
        /// Resets the set list.
        /// </summary>
        /// <param name="obj">The object.</param>
        private void ResetSetList(Card obj)
        {
            ResetSetList();
        }

        /// <summary>
        /// Resets the card set list.
        /// </summary>
        private void ResetSetList()
        {
            setListView.Reset();
        }

        /// <summary>
        /// Apply Setting Changes
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">
        /// The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the
        /// event data.
        /// </param>
        private void SettingsChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            setListView.RenderTransform = new ScaleTransform(Settings.Default.Scale / 100, Settings.Default.Scale / 100);
            setListView.Opacity = Settings.Default.Opacity / 100;
        }
    }
}