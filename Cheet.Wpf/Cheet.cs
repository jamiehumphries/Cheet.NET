namespace Cheet.Wpf
{
    using global::Cheet.Core;
    using System;
    using System.Text.RegularExpressions;
    using System.Windows.Input;

    public interface ICheet : ICheet<Key>
    {
        void OnKeyDown(object sender, KeyEventArgs e);
    }

    public class Cheet : Cheet<Key>, ICheet
    {
        private static readonly Regex LetterKeyNamePattern = new Regex(@"^[a-z]$");
        private static readonly Regex NumberKeyNamePattern = new Regex(@"^[0-9]$");
        private static readonly Regex KeypadNumberKeyNamePattern = new Regex(@"^kp_[0-9]$");
        private static readonly Regex FunctionKeyNamePattern = new Regex(@"^(?:f[1-9]|f1[0-2])$");

        public virtual void OnKeyDown(object sender, KeyEventArgs e)
        {
            base.OnKeyDown(e.Key);
        }

        protected override Key GetKey(string keyName)
        {
            if (LetterKeyNamePattern.IsMatch(keyName))
            {
                return ParseKey(keyName.ToUpper());
            }
            if (NumberKeyNamePattern.IsMatch(keyName))
            {
                return ParseKey("D" + keyName);
            }
            if (KeypadNumberKeyNamePattern.IsMatch(keyName))
            {
                return ParseKey(keyName.Replace("kp_", "NumPad"));
            }
            if (FunctionKeyNamePattern.IsMatch(keyName))
            {
                return ParseKey(keyName.ToUpper());
            }
            switch (keyName)
            {
                case "left":
                case "L":
                case "←":
                    return Key.Left;
                case "up":
                case "U":
                case "↑":
                    return Key.Up;
                case "right":
                case "R":
                case "→":
                    return Key.Right;
                case "down":
                case "D":
                case "↓":
                    return Key.Down;
                case "backspace":
                    return Key.Back;
                case "tab":
                    return Key.Tab;
                case "enter":
                    return Key.Enter;
                case "return":
                    return Key.Return;
                case "shift":
                case "⇧":
                    return Key.LeftShift;
                case "control":
                case "ctrl":
                case "^":
                    return Key.LeftCtrl;
                case "alt":
                case "option":
                case "⌥":
                    return Key.LeftAlt;
                case "command":
                case "⌘":
                    return Key.LWin;
                case "pause":
                    return Key.Pause;
                case "capslock":
                    return Key.CapsLock;
                case "esc":
                    return Key.Escape;
                case "space":
                    return Key.Space;
                case "pageup":
                    return Key.PageUp;
                case "pagedown":
                    return Key.PageDown;
                case "end":
                    return Key.End;
                case "home":
                    return Key.Home;
                case "insert":
                    return Key.Insert;
                case "delete":
                    return Key.Delete;
                case "equal":
                case "=":
                    return Key.OemPlus;
                case "comma":
                case ",":
                    return Key.OemComma;
                case "minus":
                case "-":
                    return Key.OemMinus;
                case "period":
                case ".":
                    return Key.OemPeriod;
                case "kp_multiply":
                    return Key.Multiply;
                case "kp_plus":
                    return Key.Add;
                case "kp_minus":
                    return Key.Subtract;
                case "kp_decimal":
                    return Key.Decimal;
                case "kp_divide":
                    return Key.Divide;
            }
            throw new ArgumentException(String.Format("Could not map key named '{0}'.", keyName));
        }

        private static Key ParseKey(string keyName)
        {
            return (Key)Enum.Parse(typeof(Key), keyName);
        }
    }
}