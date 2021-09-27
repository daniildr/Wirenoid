using Docker.DotNet.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wirenoid.Core.Interfaces;
using Wirenoid.WebApi.Controllers.Abstracts;

namespace Wirenoid.WebApi.Controllers
{
    [Route("api/docker/[controller]")]
    [ApiController]
    public class ContainersController : AbstractDockerController
    {
        public ContainersController(IDockerManager dockerManager)
            : base(dockerManager) { }

        [HttpGet(Order = 0)]
        public async Task<List<ContainerListResponse>> GetContainersListAsync() =>
            await DockerManager.GetContainersListAsync(null, true);
        
        [HttpGet("{id}", Order = 1)]
        public async Task<ContainerListResponse> GetContainerAsync(string id) =>
            await DockerManager.GetContainerAsync(id);

        [HttpDelete("{id}", Order = 2)]
        public async Task DeleteContainerAsync(string id) =>
            await DockerManager.DeleteContainerAsync(id);

        [HttpGet("start/{id}", Order = 3)]
        public async Task StartContainerAsync(string id) =>
            await DockerManager.StartContainerAsync(id);

        [HttpGet("stop/{id}", Order = 3)]
        public async Task StopContainerAsync(string id) =>
            await DockerManager.StopContainerAsync(id);

        [HttpPost("create/")]
        public async Task<string> CreateContainerAsync(
            string name = "wiremock",
            string image = "rodolpheche/wiremock",
            string tag = "latest",
            string privatePort = "8080/tcp",
            string publicPort = "9999") =>
            await DockerManager.CreateContainerAsync(name, image, tag, privatePort, publicPort);


    }
}
