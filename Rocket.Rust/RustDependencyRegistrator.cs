using System;

using Rocket.API;
using Rocket.API.DependencyInjection;

namespace Rocket.Rust
{
    public class RustDependencyRegistrator : IDependencyRegistrator
    {
        public void Register(IDependencyContainer container, IDependencyResolver resolver)
        {
            container.RegisterSingletonInstance<IImplementation>(container.Activate<Rust>());
        }
    }
}
