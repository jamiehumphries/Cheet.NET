namespace Cheet.Core.Tests
{
    using FakeItEasy;
    using NUnit.Framework;
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public interface ITestCallbacks<T>
    {
        void Done();
        void Done(string str, T[] seq);
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
            cheet.Register("a b c", () => callbacks.Done());

            // When
            cheet.SendSequence("a b c");

            // Then
            A.CallTo(() => callbacks.Done()).MustHaveHappened();
        }

        [TestCase("a b c")]
        [TestCase("x y a b c")]
        [TestCase("a b c 1 2")]
        [TestCase("x y a b c 1 2")]
        public void Done_callback_invoked_when_sequence_done(string sequence)
        {
            // Given
            cheet.Register("a b c", (str, seq) => callbacks.Done(str, seq));

            // When
            cheet.SendSequence(sequence);

            // Then
            A.CallTo(() => callbacks.Done("a b c", A<TestKey[]>.That.Matches(Keys.For("a b c")))).MustHaveHappened();
        }

        [TestCase("a b")]
        [TestCase("a b d")]
        [TestCase("a a b b c c")]
        public void Done_callback_not_invoked_if_sequence_incomplete_or_missed(string sequence)
        {
            // Given
            cheet.Register("a b c", (str, seq) => callbacks.Done(str, seq));

            // When
            cheet.SendSequence(sequence);

            // Then
            A.CallTo(() => callbacks.Done(null, null)).WithAnyArguments().MustNotHaveHappened();
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