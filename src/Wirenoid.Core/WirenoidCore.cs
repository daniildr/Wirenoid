using Microsoft.Extensions.Options;
using Wirenoid.Core.Abstracts;
using Wirenoid.Core.Interfaces;
using Wirenoid.Core.Models;

namespace Wirenoid.Core
{
    public class WirenoidCore : AbstractWirenoidCore, IDockerHandler
    {
        public WirenoidCore(IOptions<CoreSettings> settings) 
            : base(settings.Value.DockerdeamonPath)
        {

        }

        public Launch CreateContainer()
        {
            throw new System.NotImplementedException();
        }

        public string CreateLaunch()
        {
            throw new System.NotImplementedException();
        }

        public void DeleteLaunch()
        {
            throw new System.NotImplementedException();
        }
    }
}
