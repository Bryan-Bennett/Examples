using System;
using System.IO;
using System.Reflection;

namespace Examples.ElementaryTypes
{
    /// <summary>
    /// The AssemblyExtensions class adds extension methods to the <see cref="Assembly"/> type.
    /// </summary>
    public static class AssemblyExtensions
    {

        /// <summary>
        /// Gets the <see cref="AssemblyInformation"/> of this assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static AssemblyInformation GetAssemblyInfo(this Assembly assembly)
        {
            return new AssemblyInformation(assembly);
        }

        /// <summary>
        /// Gets the filepath to this assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static string GetAssemblyPath(this Assembly assembly)
        {
            string codeBase = assembly.CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            return Uri.UnescapeDataString(uri.Path);
        }

        /// <summary>
        /// Gets the directory that this assembly resides in.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static string GetAssemblyDirectory(this Assembly assembly)
        {
            return Path.GetDirectoryName(assembly.GetAssemblyPath());
        }
    }
}
