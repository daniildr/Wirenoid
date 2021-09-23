using Docker.DotNet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wirenoid.Core.Interfaces
{
    public interface IDockerManager
    {
        #region Container Manager
        /// <summary>
        /// Async methode for getting list of containers (<code>IList<ContainerListResponse></code>) in docker
        /// </summary>
        /// <returns><code>IList<ContainerListResponse></code></returns>
        public Task<IList<ContainerListResponse>> GetContainersListAsync();

        /// <summary>
        /// Async methode for geting list of containers (<code>IList<ContainerListResponse></code>) in docker
        /// </summary>
        /// <param name="limit">Limit of number for getting</param>
        /// <returns><code>IList<ContainerListResponse></code></returns>
        public Task<IList<ContainerListResponse>> GetContainersListAsync(int limit);

        public Task<string> CreateContainerAsync();

        public Task<bool> DeleteContainerAsync(string conatinerId);

        public Task<bool> StartContainerAsync(string containerId);

        public Task<bool> StopContainerAsync(string containerId);
        #endregion

        public Task<bool> SetDefualtContainerAsync();

        #region Image Manager
        /// <summary>
        /// Async methode for getting list of images (<code>IList<ImagesListResponse></code>) in docker
        /// </summary>
        /// <returns><code>IList<ImagesListResponse></code></returns>
        public Task<IList<ImagesListResponse>> GetImagesListAsync();

        /// <summary>
        /// Async method for creating docker image with image data frome configs (IOptions) 
        /// </summary>
        /// <returns><code>string</code>ID of new image</returns>
        public Task<string> CreateImageAsync();

        /// <summary>
        /// Async method for creating docker image
        /// </summary>
        /// <param name="image">Image name</param>
        /// <param name="tag">Image tag</param>
        /// <param name="useDockerHub"><code>bool</code>Flag of using Docker Hub</param>
        /// <returns><code>string</code>ID of new image</returns>
        /// <exception cref="ArgumentException"></exception>
        public Task<string> CreateImageAsync(string image, string tag, bool useDockerHub = false);

        public Task<bool> DeleteImageAsync(string imageId);
        #endregion
    }
}
