namespace CheetNET.Wpf
{
    using CheetNET.Core;
    using System.Windows.Input;

    public interface ICheet : ICheet<Key>
    {
        /// <summary>
        ///     KeyEventHandler to be used to subscribe to user keyboard events from WPF elements.
        /// </summary>
        void OnKeyDown(object sender, KeyEventArgs e);
    }
}