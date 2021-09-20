using System;
using Docker.DotNet;

namespace Wirenoid.Core.Abstracts
{
    public class AbstractWirenoidCore
    {
        protected string dockerdeamonPath;

        protected DockerClient client;

        public AbstractWirenoidCore(string dockerdeamonPath)
        {
            this.dockerdeamonPath = dockerdeamonPath;
            client = new DockerClientConfiguration(
                new Uri(this.dockerdeamonPath))
                .CreateClient();
        }
    }
}
