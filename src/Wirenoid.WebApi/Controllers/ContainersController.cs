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
            await _dockerManager.GetContainersListAsync(null, true);
        
        [HttpGet("{id}", Order = 1)]
        public async Task<ContainerListResponse> GetContainerAsync(string id) =>
            await _dockerManager.GetContainerAsync(id);

        [HttpDelete("{id}", Order = 2)]
        public async Task DeleteContainerAsync(string id) =>
            await _dockerManager.DeleteContainerAsync(id);

        [HttpGet("start/{id}", Order = 3)]
        public async Task StartContainerAsync(string id) =>
            await _dockerManager.StartContainerAsync(id);

        [HttpGet("stop/{id}", Order = 3)]
        public async Task StopContainerAsync(string id) =>
            await _dockerManager.StopContainerAsync(id);

        [HttpPost("create/")]
        public async Task<string> CreateContainerAsync(
            string name = "wiremock",
            string image = "rodolpheche/wiremock",
            string tag = "latest",
            string privatePort = "8080/tcp",
            string publicPort = "9999") =>
            await _dockerManager.CreateContainerAsync(name, image, tag, privatePort, publicPort);


    }
}
