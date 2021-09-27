using System;

namespace Wirenoid.WebApi.Exceptions
{
    public class LaunchNotFoundException : Exception
    {
        public LaunchNotFoundException() { }

        public LaunchNotFoundException(Guid launchId) 
            : base($"Launch id {launchId} not found") { }

        public LaunchNotFoundException(Guid launchId, Exception innerException) 
            : base($"Launch id {launchId} not found", innerException) { }
    }
}