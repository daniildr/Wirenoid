using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wirenoid.Core.Interfaces;
using Wirenoid.WebApi.DbItems.Models;
using Wirenoid.WebApi.Exceptions;
using Wirenoid.WebApi.Interfaces;

namespace Wirenoid.WebApi.DbItems
{
    public class InMemoryStorage : IStorageAdapter
    {
        private readonly CacheDbContext _context;
        private readonly IDockerManager _dockerManager;

        public InMemoryStorage(CacheDbContext context, IDockerManager dockerManager)
        {
            _context = context;
            _dockerManager = dockerManager;
        }

        /// <summary>
        /// Async method for getting list of launched containers in docker
        /// </summary>
        /// <returns><code>List<LaunchedContainer></code></returns>
        public async Task<List<LaunchedContainer>> GetLaunchesAsync() =>
            await _context.LaunchedContainers.ToListAsync() ?? new List<LaunchedContainer>();

        /// <summary>
        /// Async method for getting specific launch by ID
        /// </summary>
        /// <param name="id">Launch ID</param>
        /// <returns><code></code></returns>
        /// <exception cref="LaunchNotFoundException"></exception>
        public async Task<LaunchedContainer> GetLaunchAsync(Guid id) =>
            await _context.LaunchedContainers.FirstOrDefaultAsync(launch => launch.Id == id) ??
            throw new LaunchNotFoundException(id);

        /// <summary>
        /// Async method for deleting specific launch by ID
        /// </summary>
        /// <param name="id">Launch ID</param>
        /// <exception cref="LaunchNotFoundException"></exception>
        public async Task DeleteLaunchAsync(Guid id)
        {
            var item = await GetLaunchAsync(id);
            _context.Remove(item);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Async method for adding launch in storage
        /// </summary>
        /// <param name="containerId">ID of docker container</param>
        /// <param name="uri">Full URL to container</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DockerContainerNotFound"></exception>
        public async Task AddLaunchAsync(string containerId, Uri uri)
        {
            var containerInfo = await _dockerManager.GetContainerAsync(containerId);
            _context.Add(new LaunchedContainer()
            {
                ContainerId = containerInfo.ID,
                ImageFullName = containerInfo.Image,
                Url = uri
            });
        }

        public void Dispose()
        {
            _dockerManager?.Dispose();
            _context?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}