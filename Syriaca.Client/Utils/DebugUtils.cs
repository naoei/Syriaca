using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Syriaca.Client.Utils
{
    public class DebugUtils
    {
        public static bool IsDebugBuild => is_debug_build.Value;

        private static readonly Lazy<bool> is_debug_build = new(() =>
            isDebugAssembly(typeof(DebugUtils).Assembly) || isDebugAssembly(GetEntryAssembly())
        );

        // https://stackoverflow.com/a/2186634
        private static bool isDebugAssembly(Assembly assembly) => assembly?.GetCustomAttributes(false).OfType<DebuggableAttribute>().Any(da => da.IsJITTrackingEnabled) ?? false;

        /// <summary>
        /// Gets the entry assembly, or calling assembly otherwise.
        /// </summary>
        /// <returns>The entry assembly (usually obtained via <see cref="Assembly.GetEntryAssembly()" />.</returns>
        public static Assembly GetEntryAssembly()
            => Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();

        public static string GetCallingClass(int depth = 0)
        {
            var method = new StackTrace().GetFrame(depth + 2)?.GetMethod();

            return method?.ReflectedType?.Name;
        }
    }
}