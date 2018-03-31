using System;
using System.IO;
using System.Reflection;

using RocketRuntime = Rocket.Runtime;

namespace Rocket.Rust.Runtime
{
    public static class Startup
    {
        static string RocketDir => Path.Combine(Directory.GetCurrentDirectory(), @"Rocket\Binaries");
        static string RustDir => Path.Combine(Directory.GetCurrentDirectory(), @"RustDedicated_Data\Managed");

        private static void Initialize()
        {
            AppDomain.CurrentDomain.AssemblyResolve += delegate (object sender, ResolveEventArgs args)
            {
                AssemblyName assemblyName = new AssemblyName(args.Name);
                Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

                foreach (Assembly assembly in assemblies)
                {
                    AssemblyName name2 = assembly.GetName();
                    if (string.Equals(name2.Name, assemblyName.Name, StringComparison.InvariantCultureIgnoreCase) && string.Equals(name2.CultureInfo.Name ?? "", assemblyName.CultureInfo.Name ?? "", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return assembly;
                    }
                }

                string file = Path.Combine(RocketDir, assemblyName.Name + ".dll");
                string fileSecondary = Path.Combine(RustDir, assemblyName.Name + ".dll");

                if (File.Exists(file))
                {
                    return Assembly.LoadFile(file);
                }

                if (File.Exists(fileSecondary))
                {
                    return Assembly.LoadFile(fileSecondary);
                }

                return null;
            };
        }
    }
}
