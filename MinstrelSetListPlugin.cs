namespace ElvenMinstrelSetList
{
    using Hearthstone_Deck_Tracker.Plugins;
    using System;
    using System.Reflection;
    using System.Windows.Controls;

    /// <summary>
    /// Creates an instance of The Elven Minstrel Set List <see cref="IPlugin">Plug-in</see>
    /// </summary>
    /// <seealso cref="Hearthstone_Deck_Tracker.Plugins.IPlugin"/>
    public class MinstrelSetListPlugin : IPlugin
    {
        /// <summary>
        /// The elven minstrel set list instance
        /// </summary>
        public static MinstrelSetList SetList;

        /// <summary>
        /// The author.
        /// </summary>
        /// <value>The author.</value>
        public string Author => "VeX Harbinger";

        /// <summary>
        /// The button text.
        /// </summary>
        /// <value>The button text.</value>
        public string ButtonText => "Settings";

        /// <summary>
        /// The description.
        /// </summary>
        /// <value>The description.</value>
        public string Description => @"Shows Draw Chance Percentages for the Elven Minstrel";

        /// <summary>
        /// Gets the menu item.
        /// </summary>
        /// <value>The menu item.</value>
        public MenuItem MenuItem => null;

        /// <summary>
        /// The name.
        /// </summary>
        /// <value>The name.</value>
        public string Name => "Elven Minstrel Set List";

        /// <summary>
        /// The version.
        /// </summary>
        /// <value>The version.</value>
        public Version Version => Assembly.GetExecutingAssembly().GetName().Version;

        /// <summary>
        /// Called when [button press].
        /// </summary>
        public void OnButtonPress()
        {
            SettingsView.Flyout.IsOpen = true;
        }

        /// <summary>
        /// Called when [load].
        /// </summary>
        public void OnLoad()
        {
            SetList = new MinstrelSetList();
        }

        /// <summary>
        /// Called when [unload].
        /// </summary>
        public void OnUnload()
        {
            SetList.Dispose();
            SetList = null;
        }

        /// <summary>
        /// Called when [update].
        /// </summary>
        public void OnUpdate()
        {
        }
    }
}