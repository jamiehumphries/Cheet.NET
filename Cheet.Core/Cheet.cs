namespace CheetNET.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public abstract class Cheet<T> : ICheet<T>
    {
        private readonly Dictionary<string, CheetSequence<T>> cheetSequences = new Dictionary<string, CheetSequence<T>>();

        private delegate void SequenceEventHandler(object sender, SequenceEventArgs e);

        private delegate void SequenceNextEventHandler(object sender, SequenceNextEventArgs e);

        private event SequenceEventHandler SequenceDone;

        private event SequenceNextEventHandler SequenceNext;

        private event SequenceEventHandler SequenceFail;

        public virtual void Map(string sequence)
        {
            Map(sequence, new CheetCallbacks<T>());
        }

        public virtual void Map(string sequence, Action done)
        {
            Map(sequence, (str, seq) => done());
        }

        public virtual void Map(string sequence, Action<string, T[]> done)
        {
            Map(sequence, new CheetCallbacks<T> { Done = done });
        }

        public virtual void Map(string sequence, CheetCallbacks<T> callbacks)
        {
            sequence = NormalizeSequence(sequence);
            TrackSequence(sequence);

            if (callbacks.Done != null)
            {
                Done((str, seq) =>
                {
                    if (str == sequence)
                    {
                        callbacks.Done(str, seq);
                    }
                });
            }

            if (callbacks.Next != null)
            {
                Next((str, key, num, seq) =>
                {
                    if (str == sequence)
                    {
                        callbacks.Next(str, key, num, seq);
                    }
                });
            }

            if (callbacks.Fail != null)
            {
                Fail((str, seq) =>
                {
                    if (str == sequence)
                    {
                        callbacks.Fail(str, seq);
                    }
                });
            }
        }

        public virtual void Done(Action<string, T[]> callback)
        {
            SequenceDone += (sender, e) => callback(e.StringSequence, e.KeySequence);
        }

        public virtual void Next(Action<string, T, int, T[]> callback)
        {
            SequenceNext += (sender, e) => callback(e.StringSequence, e.Key, e.Number, e.KeySequence);
        }

        public virtual void Fail(Action<string, T[]> callback)
        {
            SequenceFail += (sender, e) => callback(e.StringSequence, e.KeySequence);
        }

        public virtual void Disable(string sequence)
        {
            sequence = NormalizeSequence(sequence);
            cheetSequences.Remove(sequence);
        }

        public virtual void Reset(string sequence)
        {
            sequence = NormalizeSequence(sequence);
            CheetSequence<T> cheetSequence;
            cheetSequences.TryGetValue(sequence, out cheetSequence);
            if (cheetSequence != null)
            {
                cheetSequence.Reset();
            }
        }

        /// <summary>
        ///     Implementing classes should call this method when a key has been pressed.
        /// </summary>
        protected virtual void OnKeyDown(T key)
        {
            foreach (var cheetSequence in cheetSequences.Values.ToList())
            {
                cheetSequence.OnKeyDown(key);
            }
        }

        /// <summary>
        ///     Returns the key associated with a given key name.
        /// </summary>
        protected abstract T GetKey(string keyName);

        private string NormalizeSequence(string sequence)
        {
            return sequence == null ? "" : Regex.Replace(sequence, " +", " ").Trim(' ');
        }

        private void TrackSequence(string sequence)
        {
            if (String.IsNullOrEmpty(sequence) || cheetSequences.ContainsKey(sequence))
            {
                return;
            }

            var keySequence = ParseSequence(sequence);
            var cheetSequence = new CheetSequence<T>(keySequence);
            cheetSequences.Add(sequence, cheetSequence);

            cheetSequence.Done += (sender, e) => OnSequenceDone(
                new SequenceEventArgs { StringSequence = sequence, KeySequence = keySequence });
            cheetSequence.Next += (sender, e) => OnSequenceNext(
                new SequenceNextEventArgs { StringSequence = sequence, Key = e.Key, Number = e.Number, KeySequence = keySequence });
            cheetSequence.Fail += (sender, e) => OnSequenceFail(
                new SequenceEventArgs { StringSequence = sequence, KeySequence = keySequence });
        }

        private T[] ParseSequence(string sequence)
        {
            var keyNames = sequence.Split(' ');
            return keyNames.Select(TryGetKey).ToArray();
        }

        private T TryGetKey(string keyName)
        {
            var key = GetKey(keyName);
            // ReSharper disable once CompareNonConstrainedGenericWithNull
            if (key == (null))
            {
                throw new ArgumentException(String.Format("Could not map key named '{0}'.", keyName));
            }
            return key;
        }

        private void OnSequenceDone(SequenceEventArgs e)
        {
            if (SequenceDone != null)
            {
                SequenceDone(this, e);
            }
        }

        private void OnSequenceNext(SequenceNextEventArgs e)
        {
            if (SequenceNext != null)
            {
                SequenceNext(this, e);
            }
        }

        private void OnSequenceFail(SequenceEventArgs e)
        {
            if (SequenceFail != null)
            {
                SequenceFail(this, e);
            }
        }

        private class SequenceEventArgs
        {
            public string StringSequence { get; set; }
            public T[] KeySequence { get; set; }
        }

        private class SequenceNextEventArgs : SequenceEventArgs
        {
            public T Key { get; set; }
            public int Number { get; set; }
        }
    }
}