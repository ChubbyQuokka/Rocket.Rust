using System;

using Rocket.API;
using Rocket.API.DependencyInjection;
using Rocket.API.Player;
using Rocket.Rust.Player;

namespace Rocket.Rust
{
    public class RustDependencyRegistrator : IDependencyRegistrator
    {
        public void Register(IDependencyContainer container, IDependencyResolver resolver)
        {
            container.RegisterSingletonInstance<IImplementation>(container.Activate<Rust>());
            container.RegisterType<IPlayerManager, RustPlayerManager>();
        }
    }
}
