namespace ElvenMinstrelSetList
{
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Collections.Generic;
    using Hearthstone_Deck_Tracker;
    using Hearthstone_Deck_Tracker.Controls;
    using Card = Hearthstone_Deck_Tracker.Hearthstone.Card;
    using System.Runtime.Remoting.Messaging;

    /// <summary>
    /// Our default <see cref="StackPanel"/> object
    /// </summary>
    /// <seealso cref="System.Windows.Controls.StackPanel"/>
    public class SetListView : StackPanel
    {
        /// <summary>
        /// The List of <see cref="Card">Cards</see>
        /// </summary>
        public List<Card> Cards;

        /// <summary>
        /// The <see cref="HearthstoneTextBlock"/> label
        /// </summary>
        public HearthstoneTextBlock Label;

        /// <summary>
        /// The draw stats label
        /// </summary>
        public HearthstoneTextBlock StatsLabel;

        /// <summary>
        /// The <see cref="AnimatedCardList"/> view
        /// </summary>
        public AnimatedCardList View;

        /// <summary>
        /// Initializes a new instance of the <see cref="SetListView"/> class.
        /// </summary>
        public SetListView()
        {
            Orientation = Orientation.Vertical;

            // Section Label
            Label = hstb("Minstrel Set List");
            var margin = Label.Margin;
            margin.Top = 2;
            Label.Margin = margin;
            Children.Add(Label);
            Label.Visibility = Visibility.Visible;

            // Card View
            View = new AnimatedCardList();
            Children.Add(View);
            Cards = new List<Card>();

            // Stats Label
            StatsLabel = hstb("");
            Children.Add(StatsLabel);
        }

        /// <summary>
        /// Resets the card list and Stats label.
        /// </summary>
        public void Reset()
        {
            Cards = new List<Card>();
            StatsLabel.Text = "";
        }

        /// <summary>
        /// Updates the specified card.
        /// </summary>
        /// <param name="card">The card.</param>
        /// <returns>If the label is</returns>
        public bool Update(Card card)
        {
            if (card == null || card.Type != "Minion")
            {
                return false;
            }

            // Account for duplicates
            var match = Cards.FirstOrDefault(c => c.Name == card.Name);

            if (match != null)
            {
                Cards.Remove(match);
                card = match.Clone() as Card;
                card.Count++;
            }

            // Update View
            Cards.Add(card);
            View.Update(Cards, false);
            Label.Visibility = Visibility.Visible;

            return true;
        }

        /// <summary>
        /// Creates a new <see cref="HearthstoneTextBlock"/>
        /// </summary>
        /// <param name="labelText">The label text.</param>
        /// <returns>A <see cref="HearthstoneTextBlock"/></returns>
        private HearthstoneTextBlock hstb(string labelText)
        {
            return new HearthstoneTextBlock()
            {
                FontSize = 16,
                TextAlignment = TextAlignment.Center,
                Text = labelText,
                Visibility = Visibility.Visible
            };
        }
    }
}