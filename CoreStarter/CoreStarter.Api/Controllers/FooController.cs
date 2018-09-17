using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CoreStarter.Api.Controllers
{
    /// <summary>
    /// Foo controller.
    /// </summary>
    [Route("api/[controller]")]
    public class FooController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IFooService _service;

        /// <summary>
        /// Creates new instance of <see cref="FooController"/>.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="service"></param>
        public FooController(IConfiguration configuration, IFooService service)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        /// <summary>
        /// Tries to create a new Foo.
        /// </summary>
        /// <param name="foo">Instance of <see cref="Foo"/>.</param>
        /// <response code="200">Foo created.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Post([FromBody] Foo foo)
        {
            // Example on how to get configuration values
            var connection = _configuration.GetSection("connectionStrings")["shopisticaApi"];

            var response = await _service.Create(foo);

            return CreatedAtRoute("getById", new { id = response }, response);
        }

        /// <summary>
        /// Tries to retrieve all Foo objects.
        /// </summary>
        /// <response code="200">All available Foo objects retrieved.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet, ResponseCache(CacheProfileName = "default")]
        [ProducesResponseType(typeof(IEnumerable<Foo>), 200)]
        public async Task<IActionResult> Get()
        {
            var response = await _service.Get().ConfigureAwait(false);

            return Ok(response);
        }

        /// <summary>
        /// Tries to retrieve specified Foo.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <response code="200">Foo successfully retrieved.</response>
        /// <response code="404">Specified Foo doesn't exist.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("{id}", Name = "getById")]
        [ProducesResponseType(typeof(Foo), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _service.Get(id).ConfigureAwait(false);

            if (response == null)
                return NotFound(id);

            return Ok(response);
        }

        /// <summary>
        /// Tries to update the Foo.
        /// </summary>
        /// <param name="foo">Instance of <see cref="Foo"/> that holds values that we want updated.</param>
        /// <response code="200">Foo updated successfully.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPatch]
        public async Task<IActionResult> Patch([FromBody] Foo foo)
        {
            await _service.Update(foo).ConfigureAwait(false);

            return Ok();
        }

        /// <summary>
        /// Tires to delete specified Foo.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <response code="200">Foo deleted successfully.</response>
        /// <response code="500">Internal server error.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.Delete(id).ConfigureAwait(false);

            return Ok();
        }
    }

    /// <summary>
    /// Represents the set of methods for Foo manipulation.
    /// </summary>
    public interface IFooService
    {
        /// <summary>
        /// Tries to create new Foo.
        /// </summary>
        /// <param name="foo">Instance of <see cref="Foo"/></param>
        /// <returns>Unique identifier.</returns>
        Task<int> Create(Foo foo);

        /// <summary>
        /// Tries to retrieve all Foo objects.
        /// </summary>
        /// <returns>A collection of Foo objects (collection might be empty, but never null).</returns>
        Task<IEnumerable<Foo>> Get();

        /// <summary>
        /// Tries to retrieve specified Foo object if exists.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns>A <see cref="Foo"/> object, or null.</returns>
        Task<Foo> Get(int id);

        /// <summary>
        /// Tries to perform update.
        /// </summary>
        /// <param name="foo">Instance of <see cref="Foo"/> that holds values that we want updated.</param>
        /// <returns>An awaitable task.</returns>
        Task Update(Foo foo);

        /// <summary>
        /// Tries to delete specified Foo.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns>An awaitable task.</returns>
        Task Delete(int id);
    }

    internal class FooService : IFooService
    {
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
    }

    /// <summary>
    /// The Foo
    /// </summary>
    public class Foo
    {
        /// <summary>
        /// Gets the creation time.
        /// </summary>
        public DateTime CreateAd => DateTime.UtcNow;

        /// <summary>
        /// Gets or sets unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Foo value.
        /// </summary>
        public string Value { get; set; }
    }
}