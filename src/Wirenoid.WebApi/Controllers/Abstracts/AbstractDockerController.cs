using Microsoft.AspNetCore.Mvc;
using Wirenoid.Core.Interfaces;

namespace Wirenoid.WebApi.Controllers.Abstracts
{
    public abstract class AbstractDockerController : ControllerBase
    {
        protected readonly IDockerManager _dockerManager;

        public AbstractDockerController(IDockerManager dockerManager)
        {
            _dockerManager = dockerManager;
        }
    }
}
