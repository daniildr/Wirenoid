using Docker.DotNet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wirenoid.Core.Interfaces
{
     interface IDockerAdapter
    {
        public Task<IList<ContainerListResponse>> GetDockerContainers();

        public Task<bool> StopContainerAsync(string containerId);
    }
}
