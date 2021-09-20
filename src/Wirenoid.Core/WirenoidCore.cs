using Docker.DotNet.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Wirenoid.Core.Abstracts;
using Wirenoid.Core.Interfaces;
using Wirenoid.Core.Models;

namespace Wirenoid.Core
{
    public class WirenoidCore : AbstractWirenoidCore, IDockerHandler
    {
        public WirenoidCore(IOptions<CoreSettings> settings) 
            : base(settings.Value.DockerdeamonPath)
        {

        }

        public Launch CreateContainer()
        {
            throw new System.NotImplementedException();
        }

        public string CreateLaunch()
        {
            throw new System.NotImplementedException();
        }

        public void DeleteLaunch()
        {
            throw new System.NotImplementedException();
        }

        public async Task<IList<ContainerListResponse>> GetDockerContainers()
        {
            return await client.Containers.ListContainersAsync(new ContainersListParameters()
            {
                Limit = 10,
            });
        }

        public async Task<bool> StopContainerAsync(string containerId)
        {
            return await client.Containers.StopContainerAsync(containerId,
                new ContainerStopParameters
                {
                    WaitBeforeKillSeconds = 30
                },
                CancellationToken.None);
        }
    }
}
