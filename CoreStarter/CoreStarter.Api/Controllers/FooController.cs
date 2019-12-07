using CoreStarter.Api.Models;
using CoreStarter.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CoreStarter.Api.Controllers
{
    /// <summary>
    /// Foo controller.
    /// </summary>
    [Route("api/[controller]")]
    public class FooController : ControllerBase
    {
        private readonly IFooService _service;
        private readonly ILogger _logger;

        /// <summary>
        /// Creates new instance of <see cref="FooController"/>.
        /// </summary>
        /// <param name="service">Instance of <see cref="IFooService"/></param>
        /// <param name="logger"></param>
        public FooController(IFooService service, ILogger<FooController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Tries to create a new Foo.
        /// </summary>
        /// <param name="foo">Instance of <see cref="Foo"/>.</param>
        /// <response code="200">Foo created.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost]
        [ProducesResponseType(typeof(int), 201)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Post([FromBody] Foo foo)
        {
            var response = await _service.Create(foo);

            return CreatedAtRoute("getById", new { id = response }, response);
        }

        /// <summary>
        /// Tries to create a new foo file.
        /// </summary>
        /// <param name="foo">Instance of <see cref="Foo"/>.</param>
        /// <param name="file">A file content</param>
        /// <returns></returns>
        [HttpPost("content")]
        [ProducesResponseType(typeof(int), 201)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostFile([FromForm] FooFile foo)
        {
            using (var memoryStream = new MemoryStream())
            {
                await foo.File.OpenReadStream().CopyToAsync(memoryStream);
                var fileName = Guid.NewGuid().ToString("N");
                var path = Path.Combine(Path.GetTempPath(), fileName);
                await System.IO.File.WriteAllBytesAsync(path, memoryStream.ToArray());
            }

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
            var response = await _service.Get();

            return Ok(response);
        }


        [HttpPost("throw")]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Throw([FromBody] Foo foo)
        {
            _ = foo ?? throw new ArgumentNullException(nameof(foo));

            _service.Throw();

            return Ok(foo);
        }

        /// <summary>
        /// Tries to retrieve specified Foo.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <response code="200">Foo successfully retrieved.</response>
        /// <response code="404">Specified Foo doesn't exist.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("{id:int:min(1)}", Name = "getById")]
        [ProducesResponseType(typeof(Foo), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _service.Get(id);

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
        [HttpPut]
        public async Task<IActionResult> Patch([FromBody] Foo foo)
        {
            await _service.Update(foo);

            return Ok();
        }

        /// <summary>
        /// Tires to delete specified Foo.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <response code="200">Foo deleted successfully.</response>
        /// <response code="500">Internal server error.</response>
        [HttpDelete("{id:int:min(1)}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.Delete(id);

            return Ok();
        }
    }
}