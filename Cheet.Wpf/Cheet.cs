namespace Cheet.Wpf
{
    using global::Cheet.Core;
    using System.Windows.Input;

    public class Cheet : Cheet<Key>
    {
        protected override Key GetKey(string keyName)
        {
            return Key.A;
        }
    }
}