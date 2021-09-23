using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Docker.DotNet.Models;
using Wirenoid.Core.Abstracts;
using Wirenoid.Core.Interfaces;
using Wirenoid.Core.Models;

namespace Wirenoid.Core
{
    public class WirenoidCore : AbstractWirenoidCore, IDockerManager
    {
        public WirenoidCore(IOptions<DockerSettings> dockerSettings,
            IOptions<DockerHubSettings> dockerHubSettings,
            IOptions<DockerImageSettings> dockerImageSettings)
            : base(dockerSettings, dockerHubSettings, dockerImageSettings) { }

        #region Container Manager
        /// <summary>
        /// Async methode for getting list of containers (<code>IList<ContainerListResponse></code>) in docker
        /// </summary>
        /// <returns><code>IList<ContainerListResponse></code></returns>
        public async Task<IList<ContainerListResponse>> GetContainersListAsync() =>
            await GetContainersListAsync(10);

        /// <summary>
        /// Async methode for geting list of containers (<code>IList<ContainerListResponse></code>) in docker
        /// </summary>
        /// <param name="limit">Limit of number for getting</param>
        /// <returns><code>IList<ContainerListResponse></code></returns>
        public async Task<IList<ContainerListResponse>> GetContainersListAsync(int limit) =>
            await client.Containers.ListContainersAsync(new ContainersListParameters() { Limit = limit });

        public Task<string> CreateContainerAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> DeleteContainerAsync(string conatinerId)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> StartContainerAsync(string containerId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> StopContainerAsync(string containerId) =>
            await client.Containers.StopContainerAsync(
                containerId,
                new ContainerStopParameters
                {
                    WaitBeforeKillSeconds = 30
                },
                CancellationToken.None);
        #endregion

        public Task<bool> SetDefualtContainerAsync()
        {
            throw new System.NotImplementedException();
        }

        #region Images Manager
        /// <summary>
        /// Async methode for getting list of images (<code>IList<ImagesListResponse></code>) in docker
        /// </summary>
        /// <returns><code>IList<ImagesListResponse></code></returns>
        public async Task<IList<ImagesListResponse>> GetImagesListAsync() =>
            await client.Images.ListImagesAsync(new ImagesListParameters() { All = true });

        /// <summary>
        /// Async method for creating docker image with image data frome configs (IOptions) 
        /// </summary>
        /// <returns><code>string</code>ID of new image</returns>
        public async Task<string> CreateImageAsync() =>
            await CreateImageAsync(ImageSettings.ImageName, ImageSettings.ImageTag);

        /// <summary>
        /// Async method for creating docker image
        /// </summary>
        /// <param name="image">Image name</param>
        /// <param name="tag">Image tag</param>
        /// <param name="useDockerHub"><code>bool</code>Flag of using Docker Hub</param>
        /// <returns><code>string</code>ID of new image</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<string> CreateImageAsync(string image, string tag, bool useDockerHub = false)
        {
            AuthConfig authConfig = null;
            if (useDockerHub)
            {
                if (string.IsNullOrEmpty(DockerHubSettings.Email) ||
                    string.IsNullOrEmpty(DockerHubSettings.Username) ||
                    string.IsNullOrEmpty(DockerHubSettings.Password))
                    throw new ArgumentException($"Can't use DockerHub, check config {nameof(DockerHubSettings)}", nameof(useDockerHub));

                authConfig = new AuthConfig
                {
                    Email = DockerHubSettings.Email,
                    Username = DockerHubSettings.Username,
                    Password = DockerHubSettings.Password
                };
            }                

            await client.Images.CreateImageAsync(
                new ImagesCreateParameters
                {
                    FromImage = image,
                    Tag = tag,
                },
                authConfig,
                new Progress<JSONMessage>());

            var imagesList = await Task.FromResult(client.Images.ListImagesAsync(new ImagesListParameters() { All = true }));

            var newimage = ((List<ImagesListResponse>)imagesList.Result).Find(x => x.Labels.Values.Contains(image));

            return newimage.ID;
        }

        public Task<bool> DeleteImageAsync(string imageId)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}
