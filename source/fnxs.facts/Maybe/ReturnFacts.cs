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
            if(1.ReturnMaybe() is Just<int> value)
                value.Value.Should().Be(1);
            else
                Assert.True(false, "Return wraps value in Just<T>.");
        }

        [Fact]
        public void DoubleReturnGivesMaybe()
        {
            if(1.ReturnMaybe().ReturnMaybe() is Just<int> value)
                value.Value.Should().Be(1);
            else
                Assert.True(false, "Double return does nothing.");
        }

        [Fact]
        public void ReturnNothingIsNothing()
        {
            if (!(new Nothing<int>().ReturnMaybe() is Nothing<int>))
            {
                Assert.True(false, "ReturnMaybe nothing returns nothing.");
            }
        }

        [Fact]
        public void ReturnJustIsJust()
        {
            var just = new Just<int>(1);

            if(just.ReturnMaybe() is Just<int> value)
                value.Value.Should().Be(1);
            else
                Assert.True(false, "Double return does nothing.");
        }
    }
}
