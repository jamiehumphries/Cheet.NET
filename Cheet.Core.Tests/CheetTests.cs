namespace Cheet.Core.Tests
{
    using FakeItEasy;
    using NUnit.Framework;
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public interface ITestCallbacks<T>
    {
        void ParamterlessDone();
        void Done(string str, T[] seq);
        void Next(string arg1, TestKey arg2, int arg3, TestKey[] arg4);
        void Fail(string arg1, TestKey[] arg2);
    }

    [TestFixture]
    public class CheetTests
    {
        private Cheet cheet;
        private ITestCallbacks<TestKey> callbacks;

        [SetUp]
        public void SetUp()
        {
            cheet = new Cheet();
            callbacks = A.Fake<ITestCallbacks<TestKey>>();
        }

        [Test]
        public void Sequences_can_be_registered_without_specifying_callbacks()
        {
            // Given
            cheet.Register("a b c");

            // When
            cheet.SendSequence("a b c");

            // Then
            // Nothing bad happens.
        }

        [Test]
        public void Sequences_can_be_registered_with_parameterless_done_callback()
        {
            // Given
            cheet.Register("a b c", callbacks.ParamterlessDone);

            // When
            cheet.SendSequence("a b c");

            // Then
            A.CallTo(() => callbacks.ParamterlessDone()).MustHaveHappened();
        }

        [TestCase("a b c")]
        [TestCase("x y a b c")]
        [TestCase("a b c 1 2")]
        [TestCase("x y a b c 1 2")]
        public void Done_callback_invoked_when_sequence_done(string sequence)
        {
            // Given
            cheet.Register("a b c", callbacks.Done);

            // When
            cheet.SendSequence(sequence);

            // Then
            A.CallTo(DoneCallbackFor("a b c")).MustHaveHappened();
        }

        [TestCase("a b")]
        [TestCase("a b d")]
        [TestCase("a a b b c c")]
        public void Done_callback_not_invoked_if_sequence_incomplete_or_missed(string sequence)
        {
            // Given
            cheet.Register("a b c", callbacks.Done);

            // When
            cheet.SendSequence(sequence);

            // Then
            A.CallTo(DoneCallbackFor("a b c")).WithAnyArguments().MustNotHaveHappened();
        }

        [TestCase("a b c a b c", 2)]
        [TestCase("1 2 a b c 3 4 a b c 5 6", 2)]
        [TestCase("a b c a b c a b c", 3)]
        public void Can_complete_sequences_multiple_times(string sequence, int numberOfCompletions)
        {
            // Given
            cheet.Register("a b c", callbacks.Done);

            // When
            cheet.SendSequence(sequence);

            // Then
            A.CallTo(DoneCallbackFor("a b c")).MustHaveHappened(Repeated.Exactly.Times(numberOfCompletions));
        }

        [Test]
        public void Completions_do_not_overlap()
        {
            // Given
            cheet.Register("a a a", callbacks.Done);

            // When
            cheet.SendSequence("a a a a a");

            // Then
            A.CallTo(DoneCallbackFor("a a a")).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void Can_register_multiple_done_callbacks_to_sequence()
        {
            // Given
            cheet.Register("a b c", callbacks.Done);
            cheet.Register("a b c", callbacks.ParamterlessDone);

            // When
            cheet.SendSequence("a b c");

            // Then
            A.CallTo(DoneCallbackFor("a b c")).MustHaveHappened();
            A.CallTo(() => callbacks.ParamterlessDone()).MustHaveHappened();
        }

        [Test]
        public void Next_callback_invoked_for_each_match_along_sequence()
        {
            // Given
            cheet.Register("a b c", new TestCheetCallbacks { Next = callbacks.Next });

            // When
            cheet.SendSequence("a b c");

            // Then
            A.CallTo(NextCallbackFor("a b c", "a", 0)).MustHaveHappened();
            A.CallTo(NextCallbackFor("a b c", "b", 1)).MustHaveHappened();
            A.CallTo(NextCallbackFor("a b c", "c", 2)).MustHaveHappened();
        }

        [Test]
        public void Next_callbacks_reset_when_sequence_fails()
        {
            // Given
            cheet.Register("a b c", new TestCheetCallbacks { Next = callbacks.Next });

            // When
            cheet.SendSequence("a x a");

            // Then
            A.CallTo(NextCallbackFor("a b c", "a", 0)).MustHaveHappened(Repeated.Exactly.Twice);
        }

        [TestCase("a x")]
        [TestCase("a b x")]
        public void Fail_callback_called_when_in_progress_sequence_fails(string sequence)
        {
            // Given
            cheet.Register("a b c", new TestCheetCallbacks { Fail = callbacks.Fail });

            // When
            cheet.SendSequence(sequence);

            // Then
            A.CallTo(FailCallbackFor("a b c")).MustHaveHappened();
        }

        [Test]
        public void Fail_callback_not_invoked_if_sequence_not_in_progress()
        {
            // Given
            cheet.Register("a b c", new TestCheetCallbacks { Fail = callbacks.Fail });

            // When
            cheet.SendSequence("x");

            // Then
            A.CallTo(FailCallbackFor("a b c")).WithAnyArguments().MustNotHaveHappened();
        }

        [Test]
        public void Fail_callback_invoked_for_multiple_failures()
        {
            // Given
            cheet.Register("a b c", new TestCheetCallbacks { Fail = callbacks.Fail });

            // When
            cheet.SendSequence("a b x a x");

            // Then
            A.CallTo(FailCallbackFor("a b c")).MustHaveHappened(Repeated.Exactly.Twice);
        }

        [Test]
        public void Can_subscribe_to_all_done_callbacks()
        {
            // Given
            cheet.Register("a b c");
            cheet.Register("x y z");

            // When
            cheet.Done(callbacks.Done);
            cheet.SendSequence("a b c x y z");

            // Then
            A.CallTo(DoneCallbackFor("a b c")).MustHaveHappened();
            A.CallTo(DoneCallbackFor("x y z")).MustHaveHappened();
        }

        [Test]
        public void Can_subscribe_to_all_next_callbacks()
        {
            // Given
            cheet.Register("a b c");
            cheet.Register("x y z");

            // When
            cheet.Next(callbacks.Next);
            cheet.SendSequence("a x");

            // Then
            A.CallTo(NextCallbackFor("a b c", "a", 0)).MustHaveHappened();
            A.CallTo(NextCallbackFor("x y z", "x", 0)).MustHaveHappened();
        }

        [Test]
        public void Can_subscribe_to_all_fail_callbacks()
        {
            // Given
            cheet.Register("a b c");
            cheet.Register("x y z");

            // When
            cheet.Fail(callbacks.Fail);
            cheet.SendSequence("a b 1 x y 2");

            // Then
            A.CallTo(FailCallbackFor("a b c")).MustHaveHappened();
            A.CallTo(FailCallbackFor("x y z")).MustHaveHappened();
        }

        [Test]
        public void Separate_sequences_can_overlap()
        {
            // Given
            cheet.Register("a b c");
            cheet.Register("b c d");

            // When
            cheet.Done(callbacks.Done);
            cheet.SendSequence("a b c d");

            // Then
            A.CallTo(DoneCallbackFor("a b c")).MustHaveHappened();
            A.CallTo(DoneCallbackFor("b c d")).MustHaveHappened();
        }

        [Test]
        public void Can_register_for_all_callbacks_on_a_sequence()
        {
            // Given
            cheet.Register("a b c", new TestCheetCallbacks { Done = callbacks.Done, Next = callbacks.Next, Fail = callbacks.Fail });

            // When
            cheet.SendSequence("a b c a b x");

            // Then
            A.CallTo(NextCallbackFor("a b c", "a", 0)).MustHaveHappened();
            A.CallTo(NextCallbackFor("a b c", "b", 1)).MustHaveHappened();
            A.CallTo(NextCallbackFor("a b c", "c", 2)).MustHaveHappened();
            A.CallTo(DoneCallbackFor("a b c")).MustHaveHappened();
            A.CallTo(FailCallbackFor("a b c")).MustHaveHappened();
        }

        [Test]
        public void Disabled_sequences_do_not_fire_further_callbacks()
        {
            // Given
            cheet.Register("a b c", new TestCheetCallbacks { Done = callbacks.Done, Next = callbacks.Next, Fail = callbacks.Fail });

            // When
            cheet.SendSequence("a");
            cheet.Disable("a b c");
            cheet.SendSequence("b c a b x");

            // Then
            A.CallTo(NextCallbackFor("a b c", "a", 0)).MustHaveHappened();
            A.CallTo(NextCallbackFor("a b c", "b", 1)).MustNotHaveHappened();
            A.CallTo(NextCallbackFor("a b c", "c", 2)).MustNotHaveHappened();
            A.CallTo(DoneCallbackFor("a b c")).MustNotHaveHappened();
            A.CallTo(FailCallbackFor("a b c")).MustNotHaveHappened();
        }

        [Test]
        public void Disabled_sequences_can_be_reenabled()
        {
            // Given
            cheet.Register("a b c", callbacks.Done);
            cheet.Disable("a b c");

            // When
            cheet.Register("a b c", callbacks.Done);
            cheet.SendSequence("a b c");

            // Then
            A.CallTo(DoneCallbackFor("a b c")).MustHaveHappened();
        }

        [Test]
        public void Done_callback_not_fired_if_sequence_reset_during_input()
        {
            // Given
            cheet.Register("a b c");
            cheet.Done(callbacks.Done);

            // When
            cheet.SendSequence("a b");
            cheet.Reset("a b c");
            cheet.SendSequence("c");

            // Then
            A.CallTo(DoneCallbackFor("a b c")).MustNotHaveHappened();
        }

        [Test]
        public void Resetting_prevents_fail_callbacks()
        {
            // Given
            cheet.Register("a b c");
            cheet.Fail(callbacks.Fail);

            // When
            cheet.SendSequence("a b");
            cheet.Reset("a b c");
            cheet.SendSequence("x");

            // Then
            A.CallTo(FailCallbackFor("a b c")).MustNotHaveHappened();
        }

        [Test]
        public void Only_requested_sequence_is_reset()
        {
            // Given
            cheet.Register("a b c");
            cheet.Register("a b d");
            cheet.Done(callbacks.Done);

            // When
            cheet.SendSequence("a b");
            cheet.Reset("a b d");
            cheet.SendSequence("c");

            // Then
            A.CallTo(DoneCallbackFor("a b c")).MustHaveHappened();
            A.CallTo(FailCallbackFor("a b d")).MustNotHaveHappened();
        }

        [TestCase("")]
        [TestCase("   ")]
        [TestCase(null)]
        public void Null_or_empty_sequences_are_ignored(string sequence)
        {
            // Given
            cheet.Register(sequence);

            // When
            cheet.SendSequence("a");

            // Then
            // Nothing bad happens.
        }

        [Test]
        public void Resetting_null_is_not_problematic()
        {
            // When
            cheet.Reset(null);

            // Then
            // Nothing bad happens.
        }

        [Test]
        public void Disabling_null_sequence_is_not_problematic()
        {
            // When
            cheet.Disable(null);

            // Then
            // Nothing bad happens.
        }

        [Test]
        public void Resetting_unregistered_sequence_is_not_problematic()
        {
            // When
            cheet.Reset("a b c");

            // Then
            // Nothing bad happens.
        }

        [Test]
        public void Disabling_unregistered_sequence_is_not_problematic()
        {
            // When
            cheet.Disable("a b c");

            // Then
            // Nothing bad happens.
        }

        private Expression<Action> DoneCallbackFor(string sequence)
        {
            return () => callbacks.Done(sequence, A<TestKey[]>.That.Matches(Keys.For(sequence)));
        }

        private Expression<Action> NextCallbackFor(string sequence, string keyName, int number)
        {
            return () => callbacks.Next(sequence, new TestKey(keyName), number, A<TestKey[]>.That.Matches(Keys.For(sequence)));
        }

        private Expression<Action> FailCallbackFor(string sequence)
        {
            return () => callbacks.Fail(sequence, A<TestKey[]>.That.Matches(Keys.For(sequence)));
        }
    }

    public class TestKey
    {
        private readonly string keyName;

        public TestKey(string keyName)
        {
            this.keyName = keyName;
        }

        // ReSharper disable once CSharpWarnings::CS0659
        public override bool Equals(object obj)
        {
            var otherKey = obj as TestKey;
            return otherKey != null && keyName.Equals(otherKey.keyName);
        }
    }

    internal static class Keys
    {
        internal static Expression<Func<TestKey[], bool>> For(string sequence)
        {
            var expectedKeys = sequence.Split(' ').Select(k => new TestKey(k));
            return actualKeys => expectedKeys.SequenceEqual(actualKeys);
        }
    }

    internal class TestCheetCallbacks : CheetCallbacks<TestKey> {}

    internal class Cheet : Cheet<TestKey>
    {
        internal void SendSequence(string sequence)
        {
            var keyNames = sequence.Split(' ');
            foreach (var keyName in keyNames)
            {
                OnKeyDown(GetKey(keyName));
            }
        }

        protected override TestKey GetKey(string keyName)
        {
            return new TestKey(keyName);
        }
    }
}