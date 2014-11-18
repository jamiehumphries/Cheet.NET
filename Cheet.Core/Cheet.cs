namespace Cheet.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface ICheet<T>
    {
        void Register(string sequence);
        void Register(string sequence, Action done);
        void Register(string sequence, Action<string, T[]> done);
        void Register(string sequence, CheetCallbacks<T> callbacks);
        void Done(Action<string, T[]> callback);
        void Next(Action<string, T, int, T[]> callback);
        void Fail(Action<string, T[]> callback);
        void Disable(string sequence);
        void Reset(string sequence);
    }

    public abstract class Cheet<T> : ICheet<T>
    {
        private readonly List<CheetSequence<T>> cheetSequences = new List<CheetSequence<T>>();

        private delegate void SequenceDoneEventHandler(object sender, SequenceDoneEventArgs e);

        private event SequenceDoneEventHandler SequenceDone;

        public virtual void Register(string sequence)
        {
            Register(sequence, () => { });
        }

        public virtual void Register(string sequence, Action done)
        {
            Register(sequence, (str, seq) => done());
        }

        public virtual void Register(string sequence, Action<string, T[]> done)
        {
            Register(sequence, new CheetCallbacks<T> { Done = done });
        }

        public virtual void Register(string sequence, CheetCallbacks<T> callbacks)
        {
            AddSequence(sequence);

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
            throw new NotImplementedException();
        }

        public virtual void Fail(Action<string, T[]> callback)
        {
            throw new NotImplementedException();
        }

        public virtual void Disable(string sequence)
        {
            throw new NotImplementedException();
        }

        public virtual void Reset(string sequence)
        {
            throw new NotImplementedException();
        }

        protected virtual void OnKeyDown(T key)
        {
            foreach (var cheetSequence in cheetSequences)
            {
                cheetSequence.OnKeyDown(key);
            }
        }

        protected abstract T GetKey(string keyName);

        private void AddSequence(string sequence)
        {
            var keySequence = ParseSequence(sequence);
            var cheetSequence = new CheetSequence<T>(keySequence);
            cheetSequences.Add(cheetSequence);

            var doneEventArgs = new SequenceDoneEventArgs { StringSequence = sequence, KeySequence = keySequence };
            cheetSequence.Done += (sender, e) => OnSequenceDone(doneEventArgs);
        }

        private T[] ParseSequence(string sequence)
        {
            var keyNames = sequence.Split(' ');
            return keyNames.Select(GetKey).ToArray();
        }

        private void OnSequenceDone(SequenceDoneEventArgs e)
        {
            if (SequenceDone != null)
            {
                SequenceDone(this, e);
            }
        }

        private class SequenceDoneEventArgs
        {
            public string StringSequence { get; set; }
            public T[] KeySequence { get; set; }
        }
    }
}