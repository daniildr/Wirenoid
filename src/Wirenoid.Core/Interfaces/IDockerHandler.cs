using System.Collections.Generic;
using System.Threading.Tasks;
using Docker.DotNet.Models;
using Wirenoid.Core.Models;

namespace Wirenoid.Core.Interfaces
{
    internal interface IDockerHandler
    {
        public Task<IList<ContainerListResponse>> GetDockerContainers();

        public Task<bool> StopContainerAsync(string containerId);

        public Launch CreateContainer();

        public string CreateLaunch();

        public void DeleteLaunch();
    }
}
