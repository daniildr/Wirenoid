using Docker.DotNet.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wirenoid.Core;
using Wirenoid.Core.Interfaces;

namespace Wirenoid.WebApi.Controllers
{
    [Route("api/docker/[controller]")]
    [ApiController]
    public class ContainerManagerController : ControllerBase
    {
        private readonly IDockerManager _dockerManager;

        public ContainerManagerController(WirenoidCore dockerManager)
        {
            _dockerManager = dockerManager;
        }

        [HttpGet("containers")]
        public async Task<IList<ContainerListResponse>> GetListOfContainers(int limit = 10) => 
            await _dockerManager.GetContainersListAsync(limit);

        [HttpGet("images")]
        public async Task<IList<ImagesListResponse>> GetListOfImages() => 
            await _dockerManager.GetImagesListAsync();

        [HttpPut("images")]
        public async Task<string> CreateImage() => 
            await _dockerManager.CreateImageAsync();

        [HttpPost("imahes")]
        public async Task<string> CreateCustomImage(string image, string tag, bool useDockerHub = false) =>
            await _dockerManager.CreateImageAsync(image, tag, useDockerHub);
    }
}
