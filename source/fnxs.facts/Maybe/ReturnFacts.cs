using Xunit;
using FluentAssertions;
using FunctionalExtensions.Maybe;

namespace fnxs.facts.Maybe
{
    public class ReturnFacts
    {
        [Fact]
        public void ReturnGivesJust()
        {
            if(1.Return() is Just<int> value)
                value.Value.Should().Be(1);
            else
                Assert.True(false, "Return wraps value in Just<T>.");
        }

        [Fact]
        public void DoubleReturnGivesMaybe()
        {
            if(1.Return().Return() is Just<int> value)
                value.Value.Should().Be(1);
            else
                Assert.True(false, "Double return does nothing.");
        }

        [Fact]
        public void ReturnNothingIsNothing()
        {
            if (!(new Nothing<int>().Return() is Nothing<int>))
            {
                Assert.True(false, "Return nothing returns nothing.");
            }
        }

        [Fact]
        public void ReturnJustIsJust()
        {
            var just = new Just<int>(1);

            if(just.Return() is Just<int> value)
                value.Value.Should().Be(1);
            else
                Assert.True(false, "Double return does nothing.");
        }
    }
}
