using System;
using NUnit.Framework;
using Microsoft.Extensions.Options;
using FluentAssertions;
using FakeItEasy;
using Wirenoid.Core.Models;

namespace Wirenoid.UnitTest.WirenoidCore
{
    public class WirenoidCoreUnitTest : AbstractBaseTest
    {
        [TestCase("", "", "", "")]
        [TestCase("test", "npipe://./pipe/docker_engine", "", "test")]
        [TestCase("", "npipe://./pipe/docker_engine", "", "test")]
        public void CheckCreateCoreExceptions(
            string certFile,
            string dockerDaemonPath,
            string password,
            string username)
        {
            DockerSettings = Options.Create(new DockerSettings()
            {
                CertFile = certFile,
                DockerDaemonPath = dockerDaemonPath,
                Password = password,
                Username = username,
            });
            DockerHubSettings = A.Fake<IOptions<DockerHubSettings>>();
            DockerImageSettings = A.Fake<IOptions<DockerImageSettings>>();

            Func<Core.WirenoidCore> createCore =
                () => new Core.WirenoidCore(DockerSettings, DockerHubSettings, DockerImageSettings);

            createCore.Should().Throw<OptionsValidationException>();
        }
    }
}
