namespace CheetDemo
{
    using Cheet.Wpf;
    using System;

    public partial class MainWindow
    {
        public MainWindow()
        {
            var cheet = new Cheet();
            PreviewKeyDown += cheet.OnKeyDown;

            cheet.Map("↑ ↑ ↓ ↓ ← → ← → b a", () => { WriteLine("Voilà!"); });

            cheet.Map("i d d q d", () => {
                WriteLine("god mode enabled");
            });

            cheet.Map("o n e a t a t i m e", new CheetCallbacks {
                Next = (str, key, num, seq) => {
                    WriteLine("key pressed: " + key);
                    WriteLine("progress: " + (double)num / seq.Length);
                    WriteLine("seq: " + String.Join(" ", seq));
                },
                Fail = (str, seq) => {
                    WriteLine("sequence failed");
                },
                Done = (str, seq) => {
                    WriteLine("+30 lives ;)");
                }
            });

            cheet.Map("o n c e", () => {
                WriteLine("This will only fire once.");
                cheet.Disable("o n c e");
            });

            dynamic sequences = new {
                Cross = "up down left right",
                Circle = "left up right down"
            };

            cheet.Map(sequences.Cross);
            cheet.Map(sequences.Circle);

            cheet.Done((str, seq) => {
                if (str == sequences.Cross) {
                    WriteLine("cross!");
                }
                else if (str == sequences.Circle) {
                    WriteLine("circle!");
                }
            });

            InitializeComponent();
        }

        private void WriteLine(string message)
        {
            TextBlock.Text += message + "\r\n";
            ScrollViewer.ScrollToEnd();
        }
    }
}