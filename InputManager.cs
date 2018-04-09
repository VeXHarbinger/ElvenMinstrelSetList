namespace ElvenMinstrelSetList
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Collections.Generic;
    using Hearthstone_Deck_Tracker;
    using Core = Hearthstone_Deck_Tracker.API.Core;
    using Card = Hearthstone_Deck_Tracker.Hearthstone.Card;
    using System.Windows.Input;

    /// <summary>
    /// Input Manager
    /// </summary>
    public class InputManager
    {
        /// <summary>
        /// The mouse input
        /// </summary>
        private User32.MouseInput mouseInput;

        /// <summary>
        /// The set list panel
        /// </summary>
        private SetListView setListPanel;

        /// <summary>
        /// Initializes a new instance of the <see cref="InputManager"/> class.
        /// </summary>
        /// <param name="setList">The set list.</param>
        public InputManager(SetListView setList)
        {
            setListPanel = setList;
            setListPanel.IsManipulationEnabled = true;
            setListPanel.MouseDown += MouseDown;

            setListPanel.MouseLeftButtonDown += MouseLeftButtonUp;
            setListPanel.MouseLeftButtonUp += MouseLeftButtonUp;

            setListPanel.ManipulationStarted += Mouse_ManipulationEnded;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            mouseInput.Dispose();
        }

        /// <summary>
        /// Hides the Set List panel.
        /// </summary>
        public void Hide()
        {
            setListPanel.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Determines whether this panel instance is visible.
        /// </summary>
        /// <returns><c>true</c> if this panel instance is visible; otherwise, <c>false</c>.</returns>
        public bool IsVisible()
        {
            return (setListPanel.Visibility == Visibility.Visible);
        }

        /// <summary>
        /// Shows Set List Panel.
        /// </summary>
        public void Show()
        {
            setListPanel.Visibility = Visibility.Visible;
            setListPanel.BringIntoView();
        }

        /// <summary>
        /// Adds a card to test the panel position.
        /// </summary>
        /// <returns></returns>
        public bool TestDisplay(bool nowMovable)
        {
            // So we can click on it
            setListPanel.Focusable = nowMovable;

            //setListView.Background.IsFrozen
            //setListView.CommandBindings
            if (Core.Game.CurrentGameMode == Hearthstone_Deck_Tracker.Enums.GameMode.None && setListPanel.Cards.Count == 0)
            {
                var crd = HearthDb.Cards.Collectible.Values.FirstOrDefault(c => c.Id == "LOOT_211");
                setListPanel.Cards.Add(new Card(crd));
            }

            return Toggle();
        }

        /// <summary>
        /// Toggles the visibility.
        /// </summary>
        public bool Toggle()
        {
            if (IsVisible())
            {
                Hide();
                return false;
            }
            else
            {
                Show();

                return true;
            }
        }

        /// <summary>
        /// Handles the <see cref="StackPanel"/> Manipulation Ended event of the Mouse control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">
        /// The <see cref="ManipulationStartedEventArgs"/> instance containing the event data.
        /// </param>
        private void Mouse_ManipulationEnded(object sender, ManipulationStartedEventArgs e)
        {
            updateSettings();
        }

        private void MouseDown(object sender, MouseButtonEventArgs e)
        {
            updateSettings();
        }

        private void MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            updateSettings();
        }

        private void updateSettings()
        {
            Settings.Default.SetlistTop = Canvas.GetTop(setListPanel);
            Settings.Default.SetlistLeft = Canvas.GetLeft(setListPanel);
        }
    }
}