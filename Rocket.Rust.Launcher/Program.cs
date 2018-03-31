﻿using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

using Mono.Cecil;
using Mono.Cecil.Cil;
using Rocket.Rust.Runtime;

namespace Rocket.Rust.Launcher
{
    static class Program
    {
        static string RocketDir => Path.Combine(Directory.GetCurrentDirectory(), "Rocket", "Binaries");
        static string RustDir => Path.Combine(Directory.GetCurrentDirectory(), "RustDedicated_Data", "Managed");
        
        static Program()
        {
            AppDomain.CurrentDomain.AssemblyResolve += delegate (object sender, ResolveEventArgs args)
            {
                AssemblyName assemblyName = new AssemblyName(args.Name);
                Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

                foreach (Assembly assembly in assemblies)
                {
                    AssemblyName name2 = assembly.GetName();
                    if (string.Equals(name2.Name, assemblyName.Name, StringComparison.InvariantCultureIgnoreCase) && string.Equals(CultureToString(name2.CultureInfo), CultureToString(assemblyName.CultureInfo), StringComparison.InvariantCultureIgnoreCase))
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
                else if (File.Exists(fileSecondary))
                {
                    return Assembly.LoadFile(fileSecondary);
                }
                else
                {
                    return null;
                }
            };
        }
        

        static void Main(string[] args)
        {
            byte[] originalRust = File.ReadAllBytes(Path.Combine(RustDir, "Assembly-CSharp.dll"));

            MemoryStream stream = new MemoryStream(originalRust);
            try
            {
                AssemblyDefinition rust = AssemblyDefinition.ReadAssembly(stream);

                MethodDefinition rustInit = rust.MainModule.Types.First(x => x.FullName.Equals("Bootstrap")).Methods.First(x => x.Name.Equals("Init_Systems"));

                if (rustInit.Body.Instructions[0].OpCode != OpCodes.Call)
                {
                    AssemblyDefinition mscorlib = AssemblyDefinition.ReadAssembly(Assembly.GetAssembly(typeof(object)).Location);

                    ILProcessor processor = rustInit.Body.GetILProcessor();

                    //After trying 3,000 different ways to call it, I finally decided on reflection because of a weird AssemblyResolveException I was getting.
                    Instruction[] loadRocketAssembly = new Instruction[]
                    {
                        //Add the CurrentDirectory and the directory for Rocket.Rust.dll to the stack
                        processor.Create(OpCodes.Call, rustInit.Module.ImportReference(typeof(Directory).GetMethod("GetCurrentDirectory"))),
                        processor.Create(OpCodes.Ldstr, @"Rocket\Binaries\Rocket.Rust.dll"),
                        //Run Path.Combine
                        processor.Create(OpCodes.Call, rustInit.Module.ImportReference(typeof(Path).GetMethod("Combine", new Type[] { typeof(string), typeof(string)}))),
                        //Load the Assembly using the combined path
                        processor.Create(OpCodes.Call, rustInit.Module.ImportReference(typeof(Assembly).GetMethod("LoadFile", new Type[] { typeof(string) }))),
                        //Load type for the reflection call
                        processor.Create(OpCodes.Ldstr, "Rocket.Rust.Runtime.Startup"),
                        processor.Create(OpCodes.Callvirt, rustInit.Module.ImportReference(typeof(Assembly).GetMethod("GetType", new Type[]{ typeof(string) }))),
                        //Load the name of the method to call
                        processor.Create(OpCodes.Ldstr, "Initialize"),
                        //Load 24 on the stack (BindingFlags.Static | BindingFlags.Public)
                        processor.Create(OpCodes.Ldc_I4_S, (sbyte)24),
                        //Load the target method onto the stack
                        processor.Create(OpCodes.Callvirt, rustInit.Module.ImportReference(typeof(Type).GetMethod("GetMethod", new Type[] { typeof(string), typeof(BindingFlags) }))),
                        //Load two null values onto the stack for the invoke call Invoke(null, null)
                        processor.Create(OpCodes.Ldnull),
                        processor.Create(OpCodes.Ldnull),
                        //Make this mother fucking reflection call
                        processor.Create(OpCodes.Callvirt, rustInit.Module.ImportReference(typeof(MethodBase).GetMethod("Invoke", new Type[] { typeof(object), typeof(object[]) }))),
                        //Make the stack explode
                        processor.Create(OpCodes.Pop)

                    };

                    for (int i = 0; i < loadRocketAssembly.Length; i++)
                    {
                        processor.InsertBefore(rustInit.Body.Instructions[i], loadRocketAssembly[i]);
                    }

                    string originalDir = Path.Combine(RocketDir, "Rust");
                    Directory.CreateDirectory(originalDir);

                    File.WriteAllBytes(Path.Combine(originalDir, "Assembly-CSharp.dll"), originalRust);
                    rust.Write((Path.Combine(RustDir, "Assembly-CSharp.dll")));
                }
            }
            catch
            {
                File.WriteAllBytes(Path.Combine(RustDir, "Assembly-CSharp.dll"), originalRust);
                throw;
            }
            finally
            {
                stream.Dispose();
            }

            //Process.Start(Path.Combine(Directory.GetCurrentDirectory(), "RustDedicated.exe"), "-batchmode -nographics");
        }

        static string CultureToString(CultureInfo culture)
        {
            if (culture == null)
            {
                return "";
            }

            return culture.Name;
        }
    }
}