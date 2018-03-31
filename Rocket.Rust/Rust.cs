using System;
using System.Collections.Generic;

using Rocket.API;
using Rocket.API.DependencyInjection;
using ILogger = Rocket.API.Logging.ILogger;

using UnityEngine;

namespace Rocket.Rust
{
    public class Rust : MonoBehaviour, IImplementation
    {
        public IEnumerable<string> Capabilities => new string[] { "NADA" };
        public string InstanceId => ConVar.Server.identity;

        public Rust(IDependencyContainer container, IDependencyResolver resolver, ILogger logger)
        {
            logger.Info("Looks like Rocket.Rust is here...");
        }

        public void Load(IRuntime runtime)
        {
            
        }

        public void Reload()
        {

        }

        public void Shutdown()
        {

        }
    }
}
