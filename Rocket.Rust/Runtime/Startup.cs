using System;

using RocketRuntime = Rocket.Runtime;

namespace Rocket.Rust.Runtime
{
    public static class Startup
    {
        public static void Initialize()
        {
            RocketRuntime.Bootstrap();
        }
    }
}
