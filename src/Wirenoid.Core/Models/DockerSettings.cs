using Docker.DotNet;

namespace Wirenoid.Core.Models
{
    public class DockerSettings
    {
        /// <summary>
        /// Path to the docker daemon
        /// </summary>
        /// <example>
        /// To connect to your local Docker for Windows daemon using named pipes or your local Docker for Mac daemon using Unix sockets:
        /// npipe://./pipe/docker_engine - default Docker Engine on Windows
        /// unix:///var/run/docker.sock - default Docker Engine on Linux
        /// http://xxx.cloud.net:4243 - network Docker Engine
        /// </example>
        public string DockerDaemonPath { get; set; }
        
        /// <summary>
        /// Path to the certificate file for creating credentials if you are running Docker with TLS (HTTPS)
        /// </summary>
        public string CertFile { get; set; } = null;

        /// <summary>
        /// User name for Basic HTTP Authentication
        /// </summary>
        public string Username { get; set; } = null;

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; } = null; 
    }
}
