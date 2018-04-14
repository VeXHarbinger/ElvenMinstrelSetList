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
    using System.Windows.Input;
    using System.Collections.Generic;
    using System;

    /// <summary>
    /// Elven Minstrel Set List
    /// </summary>
    public class MinstrelSetList
    {
        /// <summary>
        /// The <see cref="SetListWindow"/> display object reference
        /// </summary>
        public SetListDisplay setListDisplay;

        /// <summary>
        /// Gets the minstrel card object reference.
        /// </summary>
        /// <value>The minstrel card object reference.</value>
        private static string minstrelCardId = "LOOT_211";

        /// <summary>
        /// Initializes a new instance of the <see cref="global::ElvenMinstrelSetList"/> class.
        /// </summary>
        public MinstrelSetList()
        {
            // Create set List container
            setListDisplay = new SetListDisplay();
            ResetSetList();

            Core.OverlayCanvas.Children.Add(setListDisplay);
            //Core.OverlayWindow

            Canvas.SetTop(setListDisplay, Settings.Default.SetlistTop);
            Canvas.SetRight(setListDisplay, Settings.Default.SetlistLeft);

            if (Config.Instance.HideInMenu && Core.Game.IsInMenu)
                setListDisplay.Visibility = Visibility.Hidden;

            //Settings.Default.PropertyChanged += SettingsChanged;
            //SettingsChanged(null, null);

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
            Core.OverlayCanvas.Children.Remove(setListDisplay);
            //Input.Dispose();
        }

        /// <summary>
        /// Resets the card set list.
        /// </summary>
        public void ResetSetList()
        {
            setListDisplay.Reset();
        }

        /// <summary>
        /// Checks the card counts to see if we need to change the draw odds.
        /// </summary>
        private void CheckCounts()
        {
            var playerDeck = Core.Game.Player.PlayerCardList
                  .Where(c => c.Type == "Minion" && (c.Count - c.InHandCount) > 0).ToList();

            if (playerDeck.Count() != setListDisplay.Cards.Count())
            {
                setListDisplay.Reset();
                setListDisplay.Update(playerDeck);
            }
        }

        /// <summary>
        /// Called when [mouse off].
        /// </summary>
        private void OnMouseOff()
        {
            setListDisplay.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Called when [mouse over].
        /// </summary>
        /// <param name="card">The card.</param>
        private void PlayerHandMouseOver(Card card)
        {
            if (card.Id == minstrelCardId)
            {
                CheckCounts();
                setListDisplay.Show();
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
        /// Apply Setting Changes
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">
        /// The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the
        /// event data.
        /// </param>
        private void SettingsChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            setListDisplay.RenderTransform = new ScaleTransform(Settings.Default.Scale / 100, Settings.Default.Scale / 100);
            setListDisplay.Opacity = Settings.Default.Opacity / 100;
        }
    }
}