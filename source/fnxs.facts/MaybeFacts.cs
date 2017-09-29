using System;
using Xunit;
using FluentAssertions;
using FunctionalExtensions.Maybe;

namespace fnxs.facts
{
    public class MaybeFacts
    {
        [Fact]
        public void RandomTests()
        {
            1
                .Return()
                .Bind(val => "a".Return())
                .Compose(true.Return())
                .Bind(value => value.Should().BeTrue().Return());
        }
    }
}
