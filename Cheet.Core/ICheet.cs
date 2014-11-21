namespace CheetNET.Core
{
    using System;

    public interface ICheet<T>
    {
        /// <summary>
        ///     Map a sequence of keypresses to a callback. This can be called multiple times.
        /// </summary>
        /// <param name="sequence">
        ///     A string representation of a sequence of key names. Each keyname must be separated by a single space.
        /// </param>
        void Map(string sequence);

        /// <summary>
        ///     Map a sequence of keypresses to a callback. This can be called multiple times.
        /// </summary>
        /// <param name="sequence">
        ///     A string representation of a sequence of key names. Each keyname must be separated by a single space.
        /// </param>
        /// <param name="done">
        ///     A callback to execute each time the sequence is correctly pressed.
        /// </param>
        void Map(string sequence, Action done);

        /// <summary>
        ///     Map a sequence of keypresses to a callback. This can be called multiple times.
        /// </summary>
        /// <param name="sequence">
        ///     A string representation of a sequence of key names. Each keyname must be separated by a single space.
        /// </param>
        /// <param name="done">
        ///     A callback to execute each time the sequence is correctly pressed.
        ///     <para></para>
        ///     The first parameter is the string representation of the sequence that completed.
        ///     <para></para>
        ///     The second parameter is an array of keys representing the sequence that completed.
        /// </param>
        void Map(string sequence, Action<string, T[]> done);

        /// <summary>
        ///     Map a sequence of keypresses to a callback. This can be called multiple times.
        /// </summary>
        /// <param name="sequence">
        ///     A string representation of a sequence of key names. Each keyname must be separated by a single space.
        /// </param>
        /// <param name="callbacks">
        ///     The callbacks to execute when the sequence is completed, progressed or broken.
        /// </param>
        void Map(string sequence, CheetCallbacks<T> callbacks);

        /// <summary>
        ///     Set a global callback that executes whenever any mapped sequence is completed successfully.
        /// </summary>
        /// <param name="callback">
        ///     A callback to execute each time any sequence is correctly pressed.
        ///     <para></para>
        ///     The first parameter is the string representation of the sequence that completed.
        ///     <para></para>
        ///     The second parameter is an array of keys representing the sequence that completed.
        /// </param>
        void Done(Action<string, T[]> callback);

        /// <summary>
        ///     Set a global callback that executes whenever any mapped sequence progresses.
        /// </summary>
        /// <param name="callback">
        ///     A callback to execute each time a correct key in any sequence is pressed in order.
        ///     <para></para>
        ///     The first parameter is the string representation of the sequence that is in progress.
        ///     <para></para>
        ///     The second parameter is the name of the key that was pressed.
        ///     <para></para>
        ///     The third parameter is a number representing the current progress of the sequence. (starts at 0)
        ///     <para></para>
        ///     The fourth parameter is an array of keys representing the sequence that is in progress.
        /// </param>
        void Next(Action<string, T, int, T[]> callback);

        /// <summary>
        ///     Set a global callback that executes whenever any in-progress sequence is broken.
        /// </summary>
        /// <param name="callback">
        ///     A callback to execute each time any sequence's progress is broken.
        ///     <para></para>
        ///     The first parameter is the string representation of the sequence that failed.
        ///     <para></para>
        ///     The second parameter is an array of keys representing the sequence that was pressed.
        /// </param>
        void Fail(Action<string, T[]> callback);

        /// <summary>
        ///     Disable a previously-mapped sequence.
        /// </summary>
        /// <param name="sequence">
        ///     The same string you used to map the callback when using cheet.Map(sequence, ...).
        /// </param>
        void Disable(string sequence);

        /// <summary>
        ///     Resets a sequence that may or may not be in progress.
        ///     This will not cause Fail callbacks to fire, but will effectively cancel the sequence.
        /// </summary>
        /// <param name="sequence">
        ///     The same string you used to map the callback when using cheet.Map(sequence, ...).
        /// </param>
        void Reset(string sequence);
    }
}