using FluentAssertions;
using FunctionalExtensions.Maybe;
using Xunit;

namespace fnxs.facts.Maybe
{
    public class LiftFacts
    {
        [Fact]
        public void LiftBasicUsage()
        {
            // Given
            var add1 = MaybeExtensions.Lift<int, int>(num => num + 1);

            // When
            var result = add1.Invoke(1.ReturnMaybe());

            // Then
            result.As<Just<int>>().Value.Should().Be(2);
        }
    }
}