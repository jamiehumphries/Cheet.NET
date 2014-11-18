namespace Cheet.Core
{
    using System;

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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        protected abstract T GetKey(string keyName);
    }
}