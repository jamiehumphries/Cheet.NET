namespace Cheet.Wpf.Tests
{
    using FluentAssertions;
    using NUnit.Framework;
    using System;
    using System.Collections;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;

    [TestFixture]
    public class CheetTests
    {
        private TestCheet cheet;

        [SetUp]
        public void SetUp()
        {
            cheet = new TestCheet();
        }

        [TestCaseSource("KeyNameMappings")]
        public void All_key_names_mapped_correctly(string keyName, Key expectedKey)
        {
            cheet.GetKey(keyName).Should().Be(expectedKey);
        }

        [Test]
        public void Exception_is_thrown_if_key_name_cannot_be_mapped()
        {
            // When
            Action mappingBadKeyName = () => cheet.GetKey("bad_name");

            // Then
            mappingBadKeyName.ShouldThrow<ArgumentException>().WithMessage("Could not map key named 'bad_name'.");
        }

        [Test]
        [RequiresSTA]
        public void Key_down_events_trigger_cheet_sequences()
        {
            // Given
            var uiElement = new UIElement();
            uiElement.PreviewKeyDown += cheet.OnKeyDown;
            var sequenceDone = false;
            cheet.Map("a b c", () => { sequenceDone = true; });

            // When
            uiElement.SimulateKeyPresses(Key.A, Key.B, Key.C);

            // Then
            sequenceDone.Should().BeTrue();
        }

        public IEnumerable KeyNameMappings()
        {
            yield return new TestCaseData("left", Key.Left);
            yield return new TestCaseData("L", Key.Left);
            yield return new TestCaseData("←", Key.Left);
            yield return new TestCaseData("up", Key.Up);
            yield return new TestCaseData("U", Key.Up);
            yield return new TestCaseData("↑", Key.Up);
            yield return new TestCaseData("right", Key.Right);
            yield return new TestCaseData("R", Key.Right);
            yield return new TestCaseData("→", Key.Right);
            yield return new TestCaseData("down", Key.Down);
            yield return new TestCaseData("D", Key.Down);
            yield return new TestCaseData("↓", Key.Down);
            yield return new TestCaseData("0", Key.D0);
            yield return new TestCaseData("1", Key.D1);
            yield return new TestCaseData("2", Key.D2);
            yield return new TestCaseData("3", Key.D3);
            yield return new TestCaseData("4", Key.D4);
            yield return new TestCaseData("5", Key.D5);
            yield return new TestCaseData("6", Key.D6);
            yield return new TestCaseData("7", Key.D7);
            yield return new TestCaseData("8", Key.D8);
            yield return new TestCaseData("9", Key.D9);
            yield return new TestCaseData("a", Key.A);
            yield return new TestCaseData("b", Key.B);
            yield return new TestCaseData("c", Key.C);
            yield return new TestCaseData("d", Key.D);
            yield return new TestCaseData("e", Key.E);
            yield return new TestCaseData("f", Key.F);
            yield return new TestCaseData("g", Key.G);
            yield return new TestCaseData("h", Key.H);
            yield return new TestCaseData("i", Key.I);
            yield return new TestCaseData("j", Key.J);
            yield return new TestCaseData("k", Key.K);
            yield return new TestCaseData("l", Key.L);
            yield return new TestCaseData("m", Key.M);
            yield return new TestCaseData("n", Key.N);
            yield return new TestCaseData("o", Key.O);
            yield return new TestCaseData("p", Key.P);
            yield return new TestCaseData("q", Key.Q);
            yield return new TestCaseData("r", Key.R);
            yield return new TestCaseData("s", Key.S);
            yield return new TestCaseData("t", Key.T);
            yield return new TestCaseData("u", Key.U);
            yield return new TestCaseData("v", Key.V);
            yield return new TestCaseData("w", Key.W);
            yield return new TestCaseData("x", Key.X);
            yield return new TestCaseData("y", Key.Y);
            yield return new TestCaseData("z", Key.Z);
            yield return new TestCaseData("backspace", Key.Back);
            yield return new TestCaseData("tab", Key.Tab);
            yield return new TestCaseData("enter", Key.Enter);
            yield return new TestCaseData("return", Key.Return);
            yield return new TestCaseData("shift", Key.LeftShift);
            yield return new TestCaseData("⇧", Key.LeftShift);
            yield return new TestCaseData("control", Key.LeftCtrl);
            yield return new TestCaseData("ctrl", Key.LeftCtrl);
            yield return new TestCaseData("^", Key.LeftCtrl);
            yield return new TestCaseData("alt", Key.LeftAlt);
            yield return new TestCaseData("option", Key.LeftAlt);
            yield return new TestCaseData("⌥", Key.LeftAlt);
            yield return new TestCaseData("command", Key.LWin);
            yield return new TestCaseData("⌘", Key.LWin);
            yield return new TestCaseData("pause", Key.Pause);
            yield return new TestCaseData("capslock", Key.CapsLock);
            yield return new TestCaseData("esc", Key.Escape);
            yield return new TestCaseData("space", Key.Space);
            yield return new TestCaseData("pageup", Key.PageUp);
            yield return new TestCaseData("pagedown", Key.PageDown);
            yield return new TestCaseData("end", Key.End);
            yield return new TestCaseData("home", Key.Home);
            yield return new TestCaseData("insert", Key.Insert);
            yield return new TestCaseData("delete", Key.Delete);
            yield return new TestCaseData("equal", Key.OemPlus);
            yield return new TestCaseData("comma", Key.OemComma);
            yield return new TestCaseData(",", Key.OemComma);
            yield return new TestCaseData("minus", Key.OemMinus);
            yield return new TestCaseData("-", Key.OemMinus);
            yield return new TestCaseData("period", Key.OemPeriod);
            yield return new TestCaseData(".", Key.OemPeriod);
            yield return new TestCaseData("kp_0", Key.NumPad0);
            yield return new TestCaseData("kp_1", Key.NumPad1);
            yield return new TestCaseData("kp_2", Key.NumPad2);
            yield return new TestCaseData("kp_3", Key.NumPad3);
            yield return new TestCaseData("kp_4", Key.NumPad4);
            yield return new TestCaseData("kp_5", Key.NumPad5);
            yield return new TestCaseData("kp_6", Key.NumPad6);
            yield return new TestCaseData("kp_7", Key.NumPad7);
            yield return new TestCaseData("kp_8", Key.NumPad8);
            yield return new TestCaseData("kp_9", Key.NumPad9);
            yield return new TestCaseData("kp_multiply", Key.Multiply);
            yield return new TestCaseData("kp_plus", Key.Add);
            yield return new TestCaseData("kp_minus", Key.Subtract);
            yield return new TestCaseData("kp_decimal", Key.Decimal);
            yield return new TestCaseData("kp_divide", Key.Divide);
            yield return new TestCaseData("f1", Key.F1);
            yield return new TestCaseData("f2", Key.F2);
            yield return new TestCaseData("f3", Key.F3);
            yield return new TestCaseData("f4", Key.F4);
            yield return new TestCaseData("f5", Key.F5);
            yield return new TestCaseData("f6", Key.F6);
            yield return new TestCaseData("f7", Key.F7);
            yield return new TestCaseData("f8", Key.F8);
            yield return new TestCaseData("f9", Key.F9);
            yield return new TestCaseData("f10", Key.F10);
            yield return new TestCaseData("f11", Key.F11);
            yield return new TestCaseData("f12", Key.F12);
        }
    }

    internal static class UIElementExtensions
    {
        internal static void SimulateKeyPresses(this UIElement uiElement, params Key[] keys)
        {
            foreach (var key in keys)
            {
                var keyArgs = new KeyEventArgs(Keyboard.PrimaryDevice, new FakePresentationSource(), Environment.TickCount, key) { RoutedEvent = UIElement.PreviewKeyDownEvent };
                uiElement.RaiseEvent(keyArgs);
            }
        }
    }

    internal class TestCheet : Cheet
    {
        public new Key GetKey(string keyName)
        {
            return base.GetKey(keyName);
        }
    }

    internal class FakePresentationSource : PresentationSource
    {
        public override Visual RootVisual { get; set; }

        public override bool IsDisposed
        {
            get { return false; }
        }

        protected override CompositionTarget GetCompositionTargetCore()
        {
            return null;
        }
    }
}