using FluentAssertions;
using FunctionalExtensions.TaskMonad;
using Xunit;

namespace fnxs.facts.Task
{
    public class ComposeFacts
    {
        [Fact]
        public async void ComposeBasic()
        {
            var actual = await 1.ReturnTask()
                          .Compose("2".ReturnTask());

            actual.Should().Be("2");
        }
    }
}