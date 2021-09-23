using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Options;
using Docker.DotNet;
using Docker.DotNet.BasicAuth;
using Docker.DotNet.X509;
using Wirenoid.Core.Models;

namespace Wirenoid.Core.Abstracts
{
    public abstract class AbstractWirenoidCore : IDisposable
    {
        #region Settings
        protected DockerSettings DockerSettings { get; private set; }

        protected DockerHubSettings DockerHubSettings {  get; private set; }

        protected DockerImageSettings ImageSettings { get; private set; }

        //protected NetworkSettings NetworkSettings { get; private set; }
        #endregion

        protected DockerClient client;

        public AbstractWirenoidCore(
            IOptions<DockerSettings> dockerSettings,
            IOptions<DockerHubSettings> dockerHubSettings,
            IOptions<DockerImageSettings> dockerImageSettings)
        {
            DockerSettings = dockerSettings.Value;
            DockerHubSettings = dockerHubSettings.Value;
            ImageSettings = dockerImageSettings.Value;
            //NetworkSettings = networkSettings.Value;
            Credentials credentials = null;

            if (string.IsNullOrEmpty(DockerSettings.DockerDaemonPath))
                throw new OptionsValidationException(
                    nameof(DockerSettings.DockerDaemonPath),
                    typeof(DockerSettings),
                    new List<string>()
                    {
                        "You need to specify the path to the docker deman.",
                    });

            if (!string.IsNullOrEmpty(DockerSettings.Username) && !string.IsNullOrEmpty(DockerSettings.CertFile))
                throw new OptionsValidationException(
                    nameof(DockerSettings.CertFile), 
                    typeof(DockerSettings), 
                    new List<string>() 
                    {
                        "You need to choose type of Authentication.",
                        $"If the Docker instance is secured with Basic HTTP Authentication - use property {nameof(DockerSettings.Username)}.",
                        "If you are running Docker with TLS (HTTPS), you can authenticate to the Docker instance using the Docker.DotNet.X509 package.",
                        $"And you need to use property {nameof(DockerSettings.CertFile)}.",
                    });

            if (!string.IsNullOrEmpty(DockerSettings.Username) && !string.IsNullOrEmpty(DockerSettings.Password))
                credentials = new BasicAuthCredentials(DockerSettings.Username, DockerSettings.Password);
            else if (!string.IsNullOrEmpty(DockerSettings.CertFile) && !string.IsNullOrEmpty(DockerSettings.Password))
                credentials = new CertificateCredentials(new X509Certificate2(DockerSettings.CertFile, DockerSettings.Password));
            else if ((!string.IsNullOrEmpty(DockerSettings.Username) || !string.IsNullOrEmpty(DockerSettings.CertFile)) && string.IsNullOrEmpty(DockerSettings.Password))
                throw new OptionsValidationException(
                    nameof(DockerSettings.Password),
                    typeof(DockerSettings),
                    new List<string>() { "You need to provide a password if you want to use authentication." });

            switch (credentials)
            {
                case BasicAuthCredentials cred:
                    {
                        var config = new DockerClientConfiguration(new Uri(DockerSettings.DockerDaemonPath), cred);
                        client = config.CreateClient();
                    }
                    break;
                case CertificateCredentials cred:
                    {
                        var config = new DockerClientConfiguration(new Uri(DockerSettings.DockerDaemonPath), cred);
                        client = config.CreateClient();
                    }
                    break;
                case null:
                    client = new DockerClientConfiguration(
                        new Uri(DockerSettings.DockerDaemonPath))
                        .CreateClient();
                    break;
                default:
                    throw new Exception("Unknown error while trying to create a client.");
            }
        }

        public void Dispose()
        {
            DockerSettings = null;
            DockerHubSettings = null;
            ImageSettings = null;

            client.Dispose();
        }
    }
}
