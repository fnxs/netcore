using FluentAssertions;
using FunctionalExtensions.TaskMonad;
using Xunit;

namespace fnxs.facts.Task
{
    public class ReturnFacts
    {
        [Fact]
        public async void ReturnGivesJust()
            => (await 1.ReturnTask()).Should().Be(1);

        [Fact]
        public async void DoubleReturnGivesTask()
            => (await 1.ReturnTask().ReturnTask()).Should().Be(1);
    }
}