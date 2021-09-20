using Wirenoid.Core.Models;

namespace Wirenoid.Core.Interfaces
{
    internal interface IDockerHandler
    {
        public Launch CreateContainer();

        public string CreateLaunch();

        public void DeleteLaunch();
    }
}
