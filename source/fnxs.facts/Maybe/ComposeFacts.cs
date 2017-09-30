using System;
using Xunit;
using FluentAssertions;
using FunctionalExtensions.Maybe;

namespace fnxs.facts.Maybe
{
    public class ComposeFacts
    {
        private int AddOne(int value)
            => value + 1;
        
        private string ToString(int value)
            => value.ToString();

        [Fact]
        public void ComposeBasic()
        {
            var actual = 1.ReturnMaybe()
                          .Compose("2".ReturnMaybe());

            actual.As<Just<string>>().Value.Should().Be("2");
        }

        [Fact]
        public void NothingBreaksComposeChain()
        {
            var actual = 1.ReturnMaybe()
                          .Compose(new Nothing<string>())
                          .Compose(2.ReturnMaybe());

            actual.Should().BeOfType<Nothing<int>>();
        }
    }
}