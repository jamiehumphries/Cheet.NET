namespace CheetNET.Core
{
    using System;

    public class CheetCallbacks<T>
    {
        public virtual Action<string, T[]> Done { get; set; }
        public virtual Action<string, T, int, T[]> Next { get; set; }
        public virtual Action<string, T[]> Fail { get; set; }
    }
}