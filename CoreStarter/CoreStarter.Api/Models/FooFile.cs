using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace CoreStarter.Api.Models
{
    public class FooFile : Foo
    {
        /// <summary>
        /// Gets or set the file content.
        /// </summary>
        public IFormFile File { get; set; }
    }
}