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
            var actual = 1.Return()
                          .Bind(num => num.ToString().Return());

            actual.Should().BeOfType<Just<string>>();
            actual.As<Just<string>>().Value.Should().Be("1");
        }

        [Fact]
        public void BindShouldSkipWhenArgumentIsNothing()
        {
            var actual = 1.Return()
                          .Bind(num => new Nothing<int>())
                          .Bind(str => "1".Return());

            actual.Should().BeOfType<Nothing<string>>();
        }
    }
}