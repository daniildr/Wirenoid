using Docker.DotNet.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wirenoid.Core;

namespace Wirenoid.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DockerController : ControllerBase
    {
        private readonly WirenoidCore _wirenoidCore;

        public DockerController(WirenoidCore core)
        {
            _wirenoidCore = core;
        }

        [HttpGet("containersList")]
        public async Task<IList<ContainerListResponse>> GetContainersList() => await _wirenoidCore.GetDockerContainers();

        [HttpPost("containerStop")]
        public async Task<bool> StopContainer(string dockerId) => await _wirenoidCore.StopContainerAsync(dockerId);
    }
}
