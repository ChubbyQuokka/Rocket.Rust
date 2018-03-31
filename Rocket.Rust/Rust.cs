using System;
using System.Collections.Generic;

using Rocket.API;

using UnityEngine;

namespace Rocket.Rust
{
    public class Rust : MonoBehaviour, IImplementation
    {
        public IEnumerable<string> Capabilities => new string[] { "NADA" };
        public string InstanceId => ConVar.Server.identity;
        
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
