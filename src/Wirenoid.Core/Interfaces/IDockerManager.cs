using System;
using Docker.DotNet.Models;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Docker.DotNet;

namespace Wirenoid.Core.Interfaces
{
    public interface IDockerManager : IDisposable
    {
        #region Container Manager 
        /// <summary>
        /// Async method for getting list of containers (<code>IList<ContainerListResponse></code>) in docker
        /// </summary>
        /// <param name="limit">Limit of number for getting. Can be null.</param>
        /// <param name="all">Flag of full getting</param>
        /// <returns><code>List<ContainerListResponse></code></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<List<ContainerListResponse>> GetContainersListAsync(int? limit, bool all);

        /// <summary>
        /// Async method for getting container info (ContainerListResponse) in docker
        /// </summary>
        /// <param name="id">ID of container</param>
        /// <returns><code>ContainerListResponse</code></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DockerContainerNotFoundException"></exception>
        public Task<ContainerListResponse> GetContainerAsync(string id);

        /// <summary>
        /// Async method for creating new container
        /// </summary>
        /// <param name="name">Custom name of container</param>
        /// <param name="image">Image name</param>
        /// <param name="tag">Image tag</param>
        /// <param name="privatePort">Internal (private) port of container</param>
        /// <param name="publicPort">Public port of container</param>
        /// <returns>ID of new container</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="Exception"></exception>
        public Task<string> CreateContainerAsync(
            string name, string image, string tag, string privatePort, string publicPort);

        /// <summary>
        /// Async method to delete container by ID
        /// </summary>
        /// <param name="id">Container ID</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DockerContainerNotFoundException"></exception>
        public Task DeleteContainerAsync(string id);

        /// <summary>
        /// Async method to start container by ID
        /// </summary>
        /// <param name="id">Container ID</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DockerContainerNotFoundException"></exception>
        public Task StartContainerAsync(string id);

        /// <summary>
        /// Async method to stop container by ID
        /// </summary>
        /// <param name="id">Container ID</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DockerContainerNotFoundException"></exception>
        public Task StopContainerAsync(string id);
        #endregion

        #region Image Manager
        /// <summary>
        /// Async method for getting list of images (<code>IList<ImagesListResponse></code>) in docker
        /// </summary>
        /// <returns><code>IList<ImagesListResponse></code></returns>
        public Task<List<ImagesListResponse>> GetImagesListAsync();

        /// <summary>
        /// Async method for getting image info (<code>ImagesListResponse</code>) in docker by ID
        /// </summary>
        /// <param name="id">ID of image</param>
        /// <returns><code>ImagesListResponse</code></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DockerImageNotFoundException"></exception>
        public Task<ImagesListResponse> GetImageByIdAsync([NotNull] string id);

        /// <summary>
        /// Async method for getting image info (<code>ImagesListResponse</code>) in docker by name
        /// </summary>
        /// <param name="name">ID of image</param>
        /// <returns><code>ImagesListResponse</code></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DockerImageNotFoundException"></exception>
        public Task<ImagesListResponse> GetImageByNameAsync([NotNull] string name);

        /// <summary>
        /// Async method for creating docker image with image data frome configs (IOptions) 
        /// </summary>
        /// <returns>ID of new image</returns>
        public Task<string> CreateImageAsync();

        /// <summary>
        /// Async method for creating docker image
        /// </summary>
        /// <param name="image">Image name</param>
        /// <param name="tag">Image tag</param>
        /// <param name="useDockerHub">Flag of using Docker Hub</param>
        /// <returns>ID of new image</returns>
        /// <exception cref="ArgumentException"></exception>
        public Task<string> CreateImageAsync([NotNull] string image, [NotNull] string tag);

        /// <summary>
        /// Async method for deleting image
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task DeleteImageAsync(string id);
        #endregion
    }
}
