using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CoreStarter.Api.Binders;
using CoreStarter.Api.Configuration;
using CoreStarter.Api.Models;
using CoreStarter.Api.Services;
using CoreStarter.Api.SwaggerExamples;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Filters;

namespace CoreStarter.Api.Controllers
{
    /// <summary>
    /// Foo controller.
    /// </summary>
    [Route("api/[controller]")]
    public class FooController : ControllerBase
    {
        private readonly ConnectionStrings _connectionStrings;
        private readonly IFooService _service;
        private readonly ILogger _logger;

        /// <summary>
        /// Creates new instance of <see cref="FooController"/>.
        /// </summary>
        /// <param name="connectionStrings">
        /// Instance of <see cref="IOptionsSnapshot{ConnectionStrings}"/> object that contains connection string.
        /// More information: https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.options.ioptionssnapshot-1?view=aspnetcore-2.1
        /// TODO: https://www.strathweb.com/2016/09/strongly-typed-configuration-in-asp-net-core-without-ioptionst/
        /// </param>
        /// <param name="service">Instance of <see cref="IFooService"/></param>
        /// <param name="logger"></param>
        public FooController(IOptionsSnapshot<ConnectionStrings> connectionStrings, IFooService service, ILogger<FooController> logger)
        {
            _connectionStrings = connectionStrings.Value ?? throw new ArgumentNullException(nameof(connectionStrings));
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
        [SwaggerRequestExample(typeof(Foo), typeof(FooRequestExample))]
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
        [AddSwaggerFileUploadButton]
        // Since we are using custom model provider this post method doesn't support swagger request examples
        // I guess this can be coded to be supported, but I feel it will go beyond this template's boundaries.
        public async Task<IActionResult> PostFile([ModelBinder(BinderType = typeof(JsonModelBinder))] Foo foo, IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.OpenReadStream().CopyToAsync(memoryStream);

                var path = Path.Combine(Path.GetTempPath(), file.FileName);
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
        [SwaggerResponseExample(200, typeof(FooResponseExample))]
        public async Task<IActionResult> Get()
        {
            var response = await _service.Get().ConfigureAwait(false);
            // Example on how to get configuration values
            var connections = new List<string> { _connectionStrings.ApiDb, _connectionStrings.Api2Db };

            return Ok(connections);
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
        [SwaggerResponseExample(200, typeof(FooListResponseExample))]
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
        [SwaggerRequestExample(typeof(Foo), typeof(FooRequestExample))]
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
        [HttpDelete("{id:int:min(1)}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.Delete(id).ConfigureAwait(false);

            return Ok();
        }
    }
}