using System;
using Xunit;
using FluentAssertions;
using FunctionalExtensions.Maybe;

namespace fnxs.facts.Maybe
{
    public class LiftAndComposeFacts
    {
        private int AddOne(int value)
            => value + 1;
        
        private string ToString(int value)
            => value.ToString();

        [Fact]
        public void LiftFacts()
        {
            var actual = 1.Lift(AddOne)
                          .Bind(num => num.Lift(ToString));

            actual.As<Just<string>>().Value.Should().Be("2");
        }

        [Fact]
        public void ComposeFacts()
        {
            var actual = 1.Return()
                          .Compose("2".Return());

            actual.As<Just<string>>().Value.Should().Be("2");
        }

        [Fact]
        public void NothingBreaksComposeChain()
        {
            var actual = 1.Return()
                          .Compose(new Nothing<string>())
                          .Compose(2.Return());

            actual.Should().BeOfType<Nothing<int>>();
        }
    }
}