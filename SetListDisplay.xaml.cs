namespace ElvenMinstrelSetList
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using Card = Hearthstone_Deck_Tracker.Hearthstone.Card;
    using Core = Hearthstone_Deck_Tracker.API.Core;
    using Helper = Hearthstone_Deck_Tracker.Helper;

    /// <summary>
    /// Interaction logic for SetListDisplay.xaml
    /// </summary>
    public partial class SetListDisplay
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetListDisplay"/> class.
        /// </summary>
        public SetListDisplay()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the list of <see cref="Card">Cards</see>
        /// </summary>
        /// <value>The list of <see cref="Card">Cards</see>.</value>
        public List<Card> Cards { get; set; } = new List<Card>();

        /// <summary>
        /// Checks the current deck against our stored copy.
        /// </summary>
        /// <param name="currentCards">The current cards.</param>
        public void CheckDeckAndShow(List<Card> currentCards)
        {
            if (Cards == null || currentCards.Count() != Cards.Count())
            {
                Reset();
                Update(currentCards);
            }
            Show();
        }

        /// <summary>
        /// Does the math.
        /// </summary>
        public void DoMath()
        {
            // First, figure out our remaining card mix
            DeckMixLabel.Text = $"{Cards.Count()}/{Core.Game.Player.DeckCount}";
            // Next, figure out our odds
            ProbabilityLabel.Text = $"{DrawProbability(1)}% / {DrawProbability(2)}%";
            // Finally see if we have any large card counts
            var match = Cards.OrderByDescending(c => c.Count).FirstOrDefault();
            if (match.Count > 2)
            {
                ProbabilityLabel.Text += $" / ({match.Count}) : {DrawProbability(match.Count)}%";
            }
        }

        /// <summary>
        /// Hides the control.
        /// </summary>
        public void Hide()
        {
            Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Resets the Control and empties the card list.
        /// </summary>
        public void Reset()
        {
            DrawPoolCardList.Update(new List<Card>(), true);
            ProbabilityLabel.Text = "";
            DeckMixLabel.Text = "";
            Hide();
        }

        /// <summary>
        /// Shows this display instance.
        /// </summary>
        public void Show()
        {
            if (Cards.Count >= 1)
            {
                Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Starts the configuration mode for setting the layout.
        /// </summary>
        public void StartConfigMode()
        {
            var card = new Card(HearthDb.Cards.Collectible.Values.FirstOrDefault(c => c.Id == Settings.Default.MinstrelCardId));
            if (card != null)
            {
                TitleBarLabel.Visibility = Visibility.Hidden;
                Cards = new List<Card>();
                Cards.Add(card);
                DrawPoolCardList.Update(Cards, true);
                ProbabilityLabel.Text = $"X% / Y";
                DeckMixLabel.Text = $"M/D";
                Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Toggles the visibility and indicates if it is Visible.
        /// </summary>
        /// <returns><c>True</c> If it is Visible, else False</returns>
        public bool Toggle()
        {
            if (IsVisible)
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
        /// Calculates the Draw the probability.
        /// </summary>
        /// <param name="copies">The number of copies to draw from.</param>
        /// <returns>The Draw the probability.</returns>
        internal Double DrawProbability(int copies = 1)
        {
            return Math.Round(Helper.DrawProbability(copies, Core.Game.Player.DeckCount, 2) * 100, 2);
        }

        /// <summary>
        /// Updates the specified list of <see cref="Card">Cards</see>.
        /// </summary>
        /// <param name="cards">The list of <see cref="Card">Cards</see>.</param>
        internal void Update(List<Card> cards)
        {
            foreach (var card in cards)
            {
                if (!Update(card))
                {
                    break;
                }
            }
            DoMath();
        }

        /// <summary>
        /// Adds the specified card to the display.
        /// </summary>
        /// <param name="card">The <see cref="Card"/>.</param>
        internal bool Update(Card card)
        {
            if (card == null || card.Type != "Minion")
            {
                return false;
            }

            // Account for duplicates
            var match = Cards.FirstOrDefault(c => c.Name == card.Name);

            if (match != null)
            {
                card.Count++;
            }
            else
            {
                Cards.Add(card);
            }

            // Update View Cards.Add(card);
            DrawPoolCardList.Update(Cards, false);

            return true;
        }
    }
}