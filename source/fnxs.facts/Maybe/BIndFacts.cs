using System;
using Xunit;
using FluentAssertions;
using FunctionalExtensions.Maybe;

namespace fnxs.facts.Maybe
{
    public class BindFacts
    {
        [Fact]
        public void BindShouldBeComposable()
        {
            var actual = 1.ReturnMaybe()
                          .Bind(num => num.ToString().ReturnMaybe());

            actual.Should().BeOfType<Just<string>>();
            actual.As<Just<string>>().Value.Should().Be("1");
        }

        [Fact]
        public void BindShouldSkipWhenArgumentIsNothing()
        {
            var actual = 1.ReturnMaybe()
                          .Bind(num => new Nothing<int>())
                          .Bind(str => "1".ReturnMaybe());

            actual.Should().BeOfType<Nothing<string>>();
        }
    }
}