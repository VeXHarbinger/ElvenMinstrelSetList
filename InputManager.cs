namespace ElvenMinstrelSetList
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Collections.Generic;
    using Hearthstone_Deck_Tracker;
    using Core = Hearthstone_Deck_Tracker.API.Core;

    /// <summary>
    /// Input Manager
    /// </summary>
    public class InputManager
    {
        /// <summary>
        /// The mouse input
        /// </summary>
        private User32.MouseInput _mouseInput;

        /// <summary>
        /// The set list panel
        /// </summary>
        private StackPanel setListPanel;

        /// <summary>
        /// Initializes a new instance of the <see cref="InputManager"/> class.
        /// </summary>
        /// <param name="setList">The set list.</param>
        public InputManager(StackPanel setList)
        {
            setListPanel = setList;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            _mouseInput.Dispose();
        }

        /// <summary>
        /// Toggles this instance.
        /// </summary>
        /// <returns></returns>
        public bool Toggle()
        {
            if (_mouseInput == null)
            {
                _mouseInput = new User32.MouseInput();
                _mouseInput.LmbDown += MouseInputOnLmbDown;
                _mouseInput.LmbUp += MouseInputOnLmbUp;
                _mouseInput.MouseMoved += MouseInputOnMouseMoved;
                return true;
            }
            else
            {
                _mouseInput.Dispose();
                _mouseInput = null;
                return false;
            }
        }

        /// <summary>
        /// Toggles the visibility.
        /// </summary>
        public void ToggleVisibility()
        {
            setListPanel.Visibility = setListPanel.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
        }

        /// <summary>
        /// Mouses input, on LMB down.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void MouseInputOnLmbDown(object sender, EventArgs eventArgs)
        {
            var pos = User32.GetMousePos();
            var _mousePos = new Point(pos.X, pos.Y);
        }

        /// <summary>
        /// Mouses input, on LMB up.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void MouseInputOnLmbUp(object sender, EventArgs eventArgs)
        {
            var pos = User32.GetMousePos();
            var p = Core.OverlayCanvas.PointFromScreen(new Point(pos.X, pos.Y));

            Settings.Default.SetlistTop = p.Y;
            Settings.Default.SetlistLeft = p.X;
        }

        /// <summary>
        /// Mouses input on mouse moved.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void MouseInputOnMouseMoved(object sender, EventArgs eventArgs)
        {
            var pos = User32.GetMousePos();
            var p = Core.OverlayCanvas.PointFromScreen(new Point(pos.X, pos.Y));

            Settings.Default.SetlistTop = p.Y;
            Settings.Default.SetlistLeft = p.X;
        }
    }
}