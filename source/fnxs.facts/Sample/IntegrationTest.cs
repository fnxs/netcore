using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using FunctionalExtensions.MaybeMonad;
using FunctionalExtensions.TaskMonad;
using Xunit;

namespace fnxs.facts.Sample
{
    public class Data
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class IntegrationTest
    {
        private List<Data> _data;

        public IntegrationTest()
        {
            _data = new List<Data> 
            {
                new Data { Id = 1, Name = "one" },
                new Data { Id = 2, Name = "two" },
                new Data { Id = 3, Name = "two" }
            };
        }

        public Task<Maybe<Data>> GetDataByName(string name)
        {
            var result = _data.SingleOrDefault(d => d.Name == name);
            if (result != null)
                return result.ReturnMaybe().ReturnTask();
            else
                return ((Maybe<Data>)new Nothing<Data>()).ReturnTask();
        }

        public Maybe<int> Numberify(string number)
        {
            if(number == "one")
                return 1.ReturnMaybe();
            else
                return new Nothing<int>();
        }
    }

    public class IntegrationFacts
    {
        [Fact]
        public async void Test()
        {
            var repository = new IntegrationTest();

            var result = await repository
                .GetDataByName("one")
                .Bind(m => m.Bind(d => repository.Numberify(d.Name)).ReturnTask());
            
            result.As<Just<int>>().Value.Should().Be(1);
        }
    }
}