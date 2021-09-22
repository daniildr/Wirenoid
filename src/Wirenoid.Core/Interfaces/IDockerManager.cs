using Docker.DotNet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wirenoid.Core.Interfaces
{
    internal interface IDockerManager
    {
        public Task<IList<ContainerListResponse>> GetDockerContainersAsync();

        public Task<IList<ContainerListResponse>> GetDockerContainersAsync(int limit);

        public Task<string> CreateImageAsync();

        public Task<string> CreateImageAsync(string image, string tag, bool useDockerHub = false);

        public Task<bool> DeleteImageAsync(string imageId);

        public Task<string> CreateContainerAsync();

        public Task<bool> DeleteContainerAsync(string conatinerId);

        public Task<bool> SetDefualtContainerAsync();

        public Task<bool> StartContainerAsync(string containerId);

        public Task<bool> StopContainerAsync(string containerId);
    }
}
