using System;
using System.Net;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using Wirenoid.Core.Abstracts;
using Wirenoid.Core.Interfaces;
using Wirenoid.Core.Models;
using Docker.DotNet;
using Docker.DotNet.Models;

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
        /// Async method for getting list of containers (<code>IList<ContainerListResponse></code>) in docker
        /// </summary>
        /// <param name="limit">Limit of number for getting. Can be null.</param>
        /// <param name="all">Flag of full getting</param>
        /// <returns><code>List<ContainerListResponse></code></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<List<ContainerListResponse>> GetContainersListAsync(int? limit = 10, bool all = false)
        {
            if ((limit == null || limit == 0) && !all)
                throw new ArgumentNullException(nameof(limit), $"The limit mustn't be null or 0");            

            if (limit != null)
            {
                if (limit != 0 && all)
                    throw new ArgumentException($"You must choose of getting mechanism - use limit or get all containers.", nameof(all));
            }                
            else
            {
                if ((limit != null && all))
                    throw new ArgumentException($"You must choose of getting mechanism - use limit or get all containers.", nameof(all));
            }
                

            return all
                ? (await Client.Containers.ListContainersAsync(new ContainersListParameters() { All = all })).ToList()
                : (await Client.Containers.ListContainersAsync(new ContainersListParameters() { Limit = limit })).ToList();
        }

        /// <summary>
        /// Async method for getting container info (ContainerListResponse) in docker
        /// </summary>
        /// <param name="id">ID of container</param>
        /// <returns><code>ContainerListResponse</code></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DockerContainerNotFoundException"></exception>
        public async Task<ContainerListResponse> GetContainerAsync([NotNull] string id)
        {
            _ = id ??
                throw new ArgumentNullException(nameof(id));

            return (await GetContainersListAsync(null, true)).Find(x => x.ID == id) ??
                throw new DockerContainerNotFoundException(HttpStatusCode.NotFound, id);
        }

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
        public async Task<string> CreateContainerAsync(
            [NotNull] string name, [NotNull] string image, [NotNull] string tag, [NotNull] string privatePort, [NotNull] string publicPort)
        {
            _ = name ?? throw new ArgumentNullException(nameof(name));
            _ = image ?? throw new ArgumentNullException(nameof(image));
            _ = tag ?? throw new ArgumentNullException(nameof(tag));
            _ = privatePort ?? throw new ArgumentNullException(nameof(privatePort));
            _ = publicPort ?? throw new ArgumentNullException(nameof(publicPort));

            var fullImageName = $"{image}:{tag}";
            var portBindings = new Dictionary<string, IList<PortBinding>>
            {
                {
                    privatePort,
                    (IList<PortBinding>)new List<PortBinding>()
                {
                    new PortBinding()
                    {
                        HostPort = publicPort
                    }
                }
                }
            };

            await Client.Containers.CreateContainerAsync(new CreateContainerParameters()
            {
                Name = name,
                Image = fullImageName,
                HostConfig = new HostConfig()
                {
                    PortBindings = portBindings
                }
            });

            var containers = await GetContainersListAsync();
            try
            {
                var container = containers.Where(x => x.Names.Any(s => s.Contains(name)) && x.Image == fullImageName).First();
                return container.ID;
            }
            catch (ArgumentNullException argNullEx)
            {
                throw new ArgumentNullException(
                    $"One or more of arguments ({nameof(name)}, {nameof(fullImageName)}) for linq-expression \"Where\" are null.",
                    argNullEx);
            }
            catch (InvalidOperationException invalidOperationEx)
            {
                throw new InvalidOperationException(
                    $"Container with parameters (name: {name}, image: {image}, tag: {tag}) not created.",
                    invalidOperationEx);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Async method to delete container by ID
        /// </summary>
        /// <param name="id">Container ID</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DockerContainerNotFoundException"></exception>
        public async Task DeleteContainerAsync([NotNull] string id)
        {
            _ = id ??
                throw new ArgumentNullException(nameof(id));

            try
            {
                await StopContainerAsync(id);                
            } 
            catch (DockerContainerNotFoundException)
            {
                throw new DockerContainerNotFoundException(HttpStatusCode.NotFound,
                    $"method {nameof(StopContainerAsync)} threw {nameof(DockerContainerNotFoundException)} exception");
            }

            await Client.Containers.RemoveContainerAsync(id, new ContainerRemoveParameters()
            {
                Force = true
            });
        }

        /// <summary>
        /// Async method to start container by ID
        /// </summary>
        /// <param name="id">Container ID</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DockerContainerNotFoundException"></exception>
        public async Task StartContainerAsync([NotNull] string id)
        {
            _ = id ??
                throw new ArgumentNullException(nameof(id));

            try
            {
                await Client.Containers.StartContainerAsync(id, new ContainerStartParameters());
            } 
            catch (Exception ex)
            {
                try
                {
                    var container = await GetContainerAsync(id);
                    throw new Exception($"Container found. Unexpected Error.", ex);
                } catch (DockerContainerNotFoundException)
                {
                    throw new DockerContainerNotFoundException(HttpStatusCode.NotFound, id);
                }
            }
        }

        /// <summary>
        /// Async method to stop container by ID
        /// </summary>
        /// <param name="id">Container ID</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DockerContainerNotFoundException"></exception>
        public async Task StopContainerAsync([NotNull] string id)
        {
            _ = id ??
                throw new ArgumentNullException(nameof(id));

            try
            {
                await Client.Containers.StopContainerAsync(id,
                new ContainerStopParameters
                {
                    WaitBeforeKillSeconds = 30
                },
                CancellationToken.None);
            }
            catch (Exception ex)
            {
                try
                {
                    var container = await GetContainerAsync(id);
                    throw new Exception($"Container found. Unexpected Error.", ex);
                }
                catch (DockerContainerNotFoundException)
                {
                    throw new DockerContainerNotFoundException(HttpStatusCode.NotFound, id);
                }
            }
        }            
        #endregion

        #region Images Manager
        /// <summary>
        /// Async method for getting list of images (<code>IList<ImagesListResponse></code>) in docker
        /// </summary>
        /// <returns><code>IList<ImagesListResponse></code></returns>
        public async Task<List<ImagesListResponse>> GetImagesListAsync() =>
            (await Client.Images.ListImagesAsync(new ImagesListParameters() { All = true })).ToList();

        /// <summary>
        /// Async method for getting image info (<code>ImagesListResponse</code>) in docker by ID
        /// </summary>
        /// <param name="id">ID of image</param>
        /// <returns><code>ImagesListResponse</code></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DockerImageNotFoundException"></exception>
        public async Task<ImagesListResponse> GetImageByIdAsync([NotNull] string id)
        {
            _ = id ??
                throw new ArgumentNullException(nameof(id));

            return (await GetImagesListAsync()).Find(x => x.ID == id) ??
                throw new DockerImageNotFoundException(HttpStatusCode.NotFound, id);
        }

        /// <summary>
        /// Async method for getting image info (<code>ImagesListResponse</code>) in docker by name
        /// </summary>
        /// <param name="name">ID of image</param>
        /// <returns><code>ImagesListResponse</code></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DockerImageNotFoundException"></exception>
        public async Task<ImagesListResponse> GetImageByNameAsync([NotNull] string name)
        {
            _ = name ??
                throw new ArgumentNullException(nameof(name));

            return (await GetImagesListAsync()).Find(x => x.RepoTags.Any(s => s.Contains(name))) ??
                throw new DockerImageNotFoundException(HttpStatusCode.NotFound, name);
        }

        /// <summary>
        /// Async method for creating docker image with image data frome configs (IOptions) 
        /// </summary>
        /// <returns>ID of new image</returns>
        public async Task<string> CreateImageAsync() =>
            await CreateImageAsync(ImageSettings.ImageName, ImageSettings.ImageTag);

        /// <summary>
        /// Async method for creating docker image
        /// </summary>
        /// <param name="image">Image name</param>
        /// <param name="tag">Image tag</param>
        /// <returns>ID of new image</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<string> CreateImageAsync([NotNull] string image, [NotNull] string tag)
        {
            AuthConfig authConfig = null;
            if (DockerHubSettings.UseDockerHub)
            {
                if (string.IsNullOrEmpty(DockerHubSettings.Email) ||
                    string.IsNullOrEmpty(DockerHubSettings.Username) ||
                    string.IsNullOrEmpty(DockerHubSettings.Password))
                    throw new ArgumentException($"Can't use DockerHub, check config {nameof(DockerHubSettings)}", new ArgumentException());

                authConfig = new AuthConfig
                {
                    Email = DockerHubSettings.Email,
                    Username = DockerHubSettings.Username,
                    Password = DockerHubSettings.Password
                };
            }                

            await Client.Images.CreateImageAsync(
                new ImagesCreateParameters
                {
                    FromImage = image,
                    Tag = tag,
                },
                authConfig,
                new Progress<JSONMessage>());

            return (await GetImageByNameAsync(image)).ID;
        }

        /// <summary>
        /// Async method for deleting image
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns></returns>
        public Task DeleteImageAsync(string imageId) => 
            Client.Images.DeleteImageAsync(imageId, new ImageDeleteParameters() { Force = true, PruneChildren = true});        
        #endregion
    }
}
