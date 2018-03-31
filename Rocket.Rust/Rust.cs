using System;
using System.Collections.Generic;

using Rocket.API;
using Rocket.API.DependencyInjection;
using Rocket.API.Logging;

namespace Rocket.Rust
{
    public class Rust : IImplementation
    {
        public IEnumerable<string> Capabilities => new string[] { "NADA" };
        public string InstanceId => ConVar.Server.identity;

        public Rust(IDependencyContainer container, IDependencyResolver resolver, ILogger logger)
        {
            logger.Info("Looks like Rocket.Rust is here...");
        }

        public void Reload()
        {

        }

        public void Shutdown()
        {

        }
    }
}
