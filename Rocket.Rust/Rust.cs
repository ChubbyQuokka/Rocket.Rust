using System;
using System.Collections.Generic;

using Rocket.API;
using ILogger = Rocket.API.Logging.ILogger;

using UnityEngine;

namespace Rocket.Rust
{
    public class Rust : MonoBehaviour, IImplementation
    {
        public IEnumerable<string> Capabilities => new string[] { "NADA" };
        public string InstanceId => ConVar.Server.identity;
        
        public void Load(IRuntime runtime)
        {
            runtime.Container.Get<ILogger>().Info("Rocket.Eco has been intialize.");
        }

        public void Reload()
        {

        }

        public void Shutdown()
        {

        }
    }
}
