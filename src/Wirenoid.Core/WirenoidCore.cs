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

        public async Task<IList<ContainerListResponse>> GetDockerContainersAsync() =>
            await GetDockerContainersAsync(10);

        public async Task<IList<ContainerListResponse>> GetDockerContainersAsync(int limit) =>
            await client.Containers.ListContainersAsync(new ContainersListParameters() { Limit = limit });

        public async Task<string> CreateImageAsync() =>
            await CreateImageAsync(ImageSettings.ImageName, ImageSettings.ImageTag);

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

        public Task<string> CreateContainerAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> DeleteContainerAsync(string conatinerId)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> SetDefualtContainerAsync()
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
    }
}
