namespace CheetNET.Core
{
    using System;

    public class CheetCallbacks<T>
    {
        /// <summary>
        ///     A callback to execute each time the sequence is correctly pressed.
        ///     <para></para>
        ///     The first parameter is the string representation of the sequence that completed.
        ///     <para></para>
        ///     The second parameter is an array of keys representing the sequence that completed.
        /// </summary>
        public virtual Action<string, T[]> Done { get; set; }

        /// <summary>
        ///     A callback to execute each time a correct key in the sequence is pressed in order.
        ///     <para></para>
        ///     The first parameter is the string representation of the sequence that is in progress.
        ///     <para></para>
        ///     The second parameter is the name of the key that was pressed.
        ///     <para></para>
        ///     The third parameter is a number representing the current progress of the sequence. (starts at 0)
        ///     <para></para>
        ///     The fourth parameter is an array of keys representing the sequence that is in progress.
        /// </summary>
        public virtual Action<string, T, int, T[]> Next { get; set; }

        /// <summary>
        ///     A callback to execute each time a sequence's progress is broken.
        ///     <para></para>
        ///     The first parameter is the string representation of the sequence that failed.
        ///     <para></para>
        ///     The second parameter is an array of keys representing the sequence that was pressed.
        /// </summary>
        public virtual Action<string, T[]> Fail { get; set; }
    }
}