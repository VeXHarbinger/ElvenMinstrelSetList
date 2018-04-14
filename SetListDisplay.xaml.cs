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
using Card = Hearthstone_Deck_Tracker.Hearthstone.Card;
using Hearthstone_Deck_Tracker.Controls;
using Hearthstone_Deck_Tracker;
using Helper = Hearthstone_Deck_Tracker.Helper;
using System.ComponentModel;
using Hearthstone_Deck_Tracker.Annotations;
using System.Runtime.CompilerServices;
using System.Activities.Expressions;

namespace ElvenMinstrelSetList
{
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
        public List<Card> Cards { get; set; }

        /// <summary>
        /// Does the math.
        /// </summary>
        public void DoMath()
        {
            StatsLabel.Text = $"{DrawProbability(1)} / {DrawProbability(2)}";
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
            Cards = new List<Card>();
            DrawPoolCardList.Update(Cards, true);
            StatsLabel.Text = "";
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
                Cards = new List<Card>();
                Cards.Add(card);
                DrawPoolCardList.Update(Cards, true);
                DoMath();
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
        /// Updates the specified list of <see cref="Card">Cards</see>.
        /// </summary>
        /// <param name="cards">The list of <see cref="Card">Cards</see>.</param>
        public void Update(List<Card> cards)
        {
            foreach (var card in cards)
            {
                if (!iUpdate(card))
                {
                    break;
                }
            }
            DoMath();
        }

        /// <summary>
        /// Calculates the Draw the probability.
        /// </summary>
        /// <param name="drawing">The number of cards to draw.</param>
        /// <returns>The Draw the probability.</returns>
        internal Double DrawProbability(int drawing = 1)
        {
            return Math.Round(Helper.DrawProbability(drawing, Cards.Count(), 2), 2);
        }

        /// <summary>
        /// Adds the specified card to the display.
        /// </summary>
        /// <param name="card">The <see cref="Card"/>.</param>
        internal bool iUpdate(Card card)
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