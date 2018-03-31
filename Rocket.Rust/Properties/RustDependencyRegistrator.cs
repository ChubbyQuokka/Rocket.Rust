using System;

using Rocket.API;
using Rocket.API.DependencyInjection;
using Rocket.API.Player;
using Rocket.Rust.Logging;
using Rocket.Rust.Player;
using ILogger = Rocket.API.Logging.ILogger;

using UnityEngine;

namespace Rocket.Rust
{
    public class RustDependencyRegistrator : IDependencyRegistrator
    {
        public void Register(IDependencyContainer container, IDependencyResolver resolver)
        {
            GameObject obj = new GameObject("Rocket.Rust");
            UnityEngine.Object.DontDestroyOnLoad(obj);

            container.RegisterSingletonInstance<IImplementation>(obj.AddComponent<Rust>());
            container.RegisterType<ILogger, UnityLogger>();
            container.RegisterType<IPlayerManager, RustPlayerManager>();
        }
    }
}
