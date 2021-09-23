using Docker.DotNet;
using Wirenoid.Core.Models;

namespace Wirenoid.Core.Abstracts
{
    public abstract class AbstractWirenoidCoreBase
    {
        protected DockerClient client;

        #region Settings
        protected DockerSettings DockerSettings { get; private set; }

        protected DockerHubSettings DockerHubSettings { get; private set; }

        protected DockerImageSettings ImageSettings { get; private set; }

        //protected NetworkSettings NetworkSettings { get; private set; }
        #endregion

        public void Dispose()
        {
            DockerSettings = null;
            DockerHubSettings = null;
            ImageSettings = null;

            client.Dispose();
        }
    }
}