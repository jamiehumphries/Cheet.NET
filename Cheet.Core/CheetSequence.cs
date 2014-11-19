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

        internal event EventHandler Done;

        internal void OnKeyDown(T key)
        {
            if (sequence[completedIndex + 1].Equals(key))
            {
                MoveNext();
            }
        }

        private void MoveNext()
        {
            completedIndex++;
            if (completedIndex == sequence.Length - 1)
            {
                OnDone();
            }
        }

        private void OnDone()
        {
            if (Done != null)
            {
                Done(this, new EventArgs());
            }
        }
    }
}