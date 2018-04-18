namespace ElvenMinstrelSetList
{
    using Hearthstone_Deck_Tracker.Controls;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
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
        /// Checks the current deck against our stored copy.
        /// </summary>
        /// <param name="currentCards">The current cards.</param>
        public void CheckDeck(List<Card> currentCards)
        {
            if (currentCards.Count() != CurrentCardCount())
            {
                CleanCards(currentCards);
                DrawPoolCardList.Update(currentCards, true);
                DoMath();
            }
        }

        /// <summary>
        /// The current minion card count.
        /// </summary>
        /// <returns></returns>
        public int CurrentCardCount()
        {
            return DrawPoolCardList.Items.Count;
        }

        /// <summary>
        /// Does the math.
        /// </summary>
        public void DoMath()
        {
            // First, figure out our remaining card mix
            DeckMixLabel.Text = $"{CurrentCardCount()}/{Core.Game.Player.DeckCount}";
            // Next, figure out our odds
            ProbabilityLabel.Text = $"{DrawProbability(1)}% / {DrawProbability(2)}%";
            // Finally see if we have any large card counts
            var match = DrawPoolCardList.Items.Cast<AnimatedCard>().OrderByDescending(c => c.Card.Count).FirstOrDefault().Card;
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
            if (CurrentCardCount() >= 1)
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
                var Cards = new List<Card>();
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
        /// Cleans the list of <see cref="Card">Cards</see>, by combining copies
        /// </summary>
        /// <param name="cards">The List of <see cref="Card">Cards</see>.</param>
        internal void CleanCards(List<Card> cards)
        {
            var dups = cards.GroupBy(c => c.Id).Where(d => d.Count() > 1).ToList();

            if (dups.Count() >= 1)
            {
                foreach (var d in dups.ToList())
                {
                    var count = 0;
                    Card first = d.First();
                    foreach (var i in d)
                    {
                        i.IsCreated = false;
                        count += i.Count;
                        i.Count = 0;
                    }
                    first.Count = count;
                }
            }
            cards.RemoveAll(c => c.Count == 0);
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
    }
}