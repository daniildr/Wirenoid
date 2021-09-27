using Microsoft.AspNetCore.Mvc;
using Wirenoid.Core.Interfaces;

namespace Wirenoid.WebApi.Controllers.Abstracts
{
    public abstract class AbstractDockerController : ControllerBase
    {
        protected readonly IDockerManager DockerManager;

        public AbstractDockerController(IDockerManager dockerManager)
        {
            DockerManager = dockerManager;
        }
    }
}
