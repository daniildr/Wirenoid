using NUnit.Framework;
using Microsoft.Extensions.Options;
using Wirenoid.Core.Models;

namespace Wirenoid.UnitTest.WirenoidCore
{
    [TestFixture]
    public abstract class AbstractBaseTest
    {
        protected Core.WirenoidCore wirenoidCore;

        protected IOptions<DockerSettings> DockerSettings { get; set; }

        protected IOptions<DockerHubSettings> DockerHubSettings { get; set; }

        protected IOptions<DockerImageSettings> DockerImageSettings { get; set; }
    }
}
