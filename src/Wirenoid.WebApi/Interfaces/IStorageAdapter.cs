using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wirenoid.WebApi.DbItems.Models;
using Wirenoid.WebApi.Exceptions;

namespace Wirenoid.WebApi.Interfaces
{
    public interface IStorageAdapter : IDisposable
    {
        /// <summary>
        /// Async method for getting list of launched containers in docker
        /// </summary>
        /// <returns><code>List<LaunchedContainer></code></returns>
        public Task<List<LaunchedContainer>> GetLaunchesAsync();

        /// <summary>
        /// Async method for getting specific launch by ID
        /// </summary>
        /// <param name="id">Launch ID</param>
        /// <returns><code></code></returns>
        /// <exception cref="LaunchNotFoundException"></exception>
        public Task<LaunchedContainer> GetLaunchAsync(Guid id);

        /// <summary>
        /// Async method for deleting specific launch by ID
        /// </summary>
        /// <param name="id">Launch ID</param>
        /// <exception cref="LaunchNotFoundException"></exception>
        public Task DeleteLaunchAsync(Guid id);

        /// <summary>
        /// Async method for adding launch in storage
        /// </summary>
        /// <param name="containerId">ID of docker container</param>
        /// <param name="uri">Full URL to container</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DockerContainerNotFound"></exception>
        public Task AddLaunchAsync(string containerId, Uri uri);
    }
}