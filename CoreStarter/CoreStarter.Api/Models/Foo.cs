using System;

namespace CoreStarter.Api.Models
{
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