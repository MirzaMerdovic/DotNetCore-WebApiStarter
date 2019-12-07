using System.Collections.Generic;
using System.Threading.Tasks;
using CoreStarter.Api.Models;

namespace CoreStarter.Api.Services
{
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

        /// <summary>
        /// Throws an exception to demonstrate exception handling.
        /// </summary>
        void Throw();
    }
}