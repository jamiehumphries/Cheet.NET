namespace Cheet.Core
{
    using System;

    internal class CheetSequence<T>
    {
        private readonly T[] sequence;
        private int completedIndex;

        internal CheetSequence(T[] sequence)
        {
            this.sequence = sequence;
            completedIndex = -1;
        }

        internal delegate void NextEventHandler(object sender, NextEventArgs e);

        internal event EventHandler Done;
        internal event NextEventHandler Next;
        internal event EventHandler Fail;

        internal void OnKeyDown(T key)
        {
            if (sequence[completedIndex + 1].Equals(key))
            {
                OnNext();
            }
            else if (completedIndex != -1)
            {
                OnFail();
            }
        }

        private void Reset()
        {
            completedIndex = -1;
        }

        private void OnNext()
        {
            completedIndex++;
            if (Next != null)
            {
                Next(this, new NextEventArgs { Key = sequence[completedIndex], Number = completedIndex });
            }
            if (completedIndex == sequence.Length - 1)
            {
                OnDone();
            }
        }

        private void OnDone()
        {
            Reset();
            if (Done != null)
            {
                Done(this, new EventArgs());
            }
        }

        private void OnFail()
        {
            Reset();
            if (Fail != null)
            {
                Fail(this, new EventArgs());
            }
        }

        internal class NextEventArgs
        {
            public T Key { get; set; }
            public int Number { get; set; }
        }
    }
}