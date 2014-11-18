namespace Cheet.Core.Tests
{
    using FakeItEasy;
    using NUnit.Framework;

    public interface ITestCallbacks
    {
        void Done();
    }

    [TestFixture]
    public class CheetTests
    {
        private Cheet cheet;
        private ITestCallbacks callbacks;

        [SetUp]
        public void SetUp()
        {
            cheet = new Cheet();
            callbacks = A.Fake<ITestCallbacks>();
        }

        [Test]
        public void Parameterless_callback_invoked_when_sequence_done()
        {
            // Given
            cheet.Register("a b c", () => callbacks.Done());

            // When
            cheet.SendSequence("a b c");

            // Then
            A.CallTo(() => callbacks.Done()).MustHaveHappened();
        }

        [TestCase("a b")]
        [TestCase("a b d")]
        public void Paramterless_callback_not_invoked_if_sequence_incomplete_or_missed(string failedSequence)
        {
            // Given
            cheet.Register("a b c", () => callbacks.Done());

            // When
            cheet.SendSequence(failedSequence);

            // Then
            A.CallTo(() => callbacks.Done()).MustNotHaveHappened();
        }
    }

    public class Cheet : Cheet<int>
    {
        public void SendSequence(string sequence)
        {
            var keyNames = sequence.Split(' ');
            foreach (var keyName in keyNames)
            {
                OnKeyDown(GetKey(keyName));
            }
        }

        protected override int GetKey(string keyName)
        {
            return keyName.GetHashCode();
        }
    }
}