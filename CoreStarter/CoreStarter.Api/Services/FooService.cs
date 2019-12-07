using CoreStarter.Api.Configuration;
using CoreStarter.Api.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreStarter.Api.Services
{
    public sealed class FooService : IFooService
    {
        private readonly ConnectionStrings _connectionStrings;

        public FooService(IOptionsMonitor<ConnectionStrings> connectionStrings)
        {
            _connectionStrings = connectionStrings.CurrentValue;
        }

        public Task<int> Create(Foo foo)
        {
            return Task.FromResult(new Random().Next());
        }

        public Task<IEnumerable<Foo>> Get()
        {
            return Task.FromResult<IEnumerable<Foo>>(new List<Foo>
            {
                new Foo {Id = 1, Value = Guid.NewGuid().ToString().Remove(5)},
                new Foo {Id = 3, Value = Guid.NewGuid().ToString().Remove(5)}
            });
        }

        public Task<Foo> Get(int id)
        {
            if (id == 0)
                return Task.FromResult<Foo>(null);

            return Task.FromResult(new Foo { Id = id, Value = Guid.NewGuid().ToString().Remove(5) });
        }

        public Task Update(Foo foo)
        {
            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            return Task.CompletedTask;
        }

        public void Throw()
        {
            throw new ApplicationException("Here is an error for you");
        }
    }
}