using FluentAssertions;
using FunctionalExtensions.Task;
using System.Threading.Tasks;
using Xunit;
using System;

namespace fnxs.facts.Task
{
    public class BindFacts
    {
        [Fact]
        public async void BindShouldBeComposable()
            => (await 1.ReturnTask()
                       .Bind(num => num.ToString().ReturnTask())
                ).Should().Be("1");
    }
}