using System;

namespace AwesomeSockets.Domain
{
    internal enum Runtimes
    {
        CLR,
        Mono
    }

    internal class Runtime
    {
        public static Runtimes GetRuntime()
        {
            return Type.GetType("Mono.Runtime") == null ? Runtimes.CLR : Runtimes.Mono;
        }
    }
}
