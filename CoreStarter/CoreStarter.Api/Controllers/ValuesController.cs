using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CoreStarter.Api.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        private readonly IValueService _service;

        public ValuesController(IValueService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Post(string value)
        {
            var response = await _service.Create(value);

            return CreatedAtRoute("getById", new { id = response }, response);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await _service.Get().ConfigureAwait(false);

            return Ok(response);
        }

        [HttpGet("{id}", Name = "getById")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _service.Get(id).ConfigureAwait(false);

            if (response == null)
                return NotFound(id);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.Delete(id).ConfigureAwait(false);

            return Ok();
        }
    }

    public interface IValueService
    {
        Task<int> Create(string value);

        Task<IEnumerable<string>> Get();

        Task<string> Get(int id);

        Task Delete(int id);
    }

    internal class ValueService : IValueService
    {
        public Task<int> Create(string value)
        {
            return Task.FromResult(new Random().Next());
        }

        public Task Delete(int id)
        {
            return Task.CompletedTask;
        }

        public Task<IEnumerable<string>> Get()
        {
            return Task.FromResult<IEnumerable<string>>(new List<string> { "value1", "value2" });
        }

        public Task<string> Get(int id)
        {
            if (id == 0)
                return Task.FromResult<string>(null);

            return Task.FromResult($"value{id}");
        }

        public Task Update(string value)
        {
            return Task.CompletedTask;
        }
    }
}