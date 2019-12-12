namespace Mastermind.PerformanceTestRunner
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    internal static class TypeResolver
    {
        internal static IReadOnlyList<Type> GetTypeInAssemblies(string assemblySearchString, Type type)
        {
            var rootDirectory = GetMastermindDirectory();
            var assemblies = GetAssembies(rootDirectory, assemblySearchString);
            var types = assemblies.Select(a => GetSingleOrNoType(a, type)).Where(t => !(t is null)).ToList();
            if (!types.Any())
            {
                var message = $"Did not find type {type.Name} in any of the assemblies.";
                var pathList = string.Join(Environment.NewLine, assemblies.Select(a => a.Location));
                throw new Exception($"{message}{Environment.NewLine}{pathList}");
            }
            return types;
        }

        internal static IReadOnlyList<Type> GetAllTypes(Type type)
        {
            var rootDirectory = GetMastermindDirectory();
            return
                GetAllAssemblyPaths(rootDirectory)
                .Select(p => Assembly.LoadFile(p))
                .Select(a => GetSingleOrNoType(a, type))
                .Where(t => !(t is null))
                .ToList();
        }

        private static IReadOnlyList<string> GetAllAssemblyPaths(string rootDirectory)
        {
            var searchPattern = $"*.dll";
            return Directory.GetFiles(rootDirectory, searchPattern, SearchOption.AllDirectories)
                .GroupBy(p => Path.GetFileName(p))
                .Select(g => g.Count() == 1 ? g.Single() : SelectTheDllInTheBinDirectory(g))
                .Where(p => !(p is null))
                .Where(p => !p.EndsWith(".Tests.dll") && !p.EndsWith(".Test.dll"))
                .ToList();
        }

        private static string SelectTheDllInTheBinDirectory(IEnumerable<string> differentPathsToTheSameFile)
        {
            // use the one that matches */assemblyName/bin/*
            var assemblyName = Path.GetFileNameWithoutExtension(differentPathsToTheSameFile.First());
            var pathToLookFor = $"{Path.DirectorySeparatorChar}{assemblyName}{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}";
            return differentPathsToTheSameFile.FirstOrDefault(f => f.Contains(pathToLookFor));
        }

        private static Type GetSingleOrNoType(Assembly assembly, Type type)
        {
            var types = assembly
                .ExportedTypes
                .Where(t =>
                    t.IsClass &&
                    !t.IsAbstract &&
                    !t.IsGenericType &&
                    type.IsAssignableFrom(t) &&
                    !(t.GetConstructor(new Type[0]) is null))
                .ToList();
            if (types.Count == 0)
            {
                return null;
            }
            else if (types.Count == 1)
            {
                return types[0];
            }
            else
            {
                var message = $"Found multiple types in {assembly.FullName} that is assignable to {type.Name}.";
                var typeList = string.Join(Environment.NewLine, types.Select(t => t.FullName));
                throw new Exception($"{message}{Environment.NewLine}{typeList}");
            }
        }

        private static IReadOnlyCollection<Assembly> GetAssembies(string rootDirectory, string assemblySearchName)
        {
            IEnumerable<string> assemblyFilePaths;
            if (File.Exists(assemblySearchName))
            {
                assemblyFilePaths = new[] { assemblySearchName };
            }
            else
            {
                var paths = GetAllAssemblyPaths(rootDirectory)
                .Where(path => Path.GetFileName(path).Contains(assemblySearchName, StringComparison.OrdinalIgnoreCase))
                .ToList();
                if (paths.Count == 0)
                {
                    throw new Exception($"Could not find a dll file with a name containing {assemblySearchName} in {rootDirectory} (including subdirectories)");
                }
                assemblyFilePaths = paths;
            }
            return assemblyFilePaths.Select(Assembly.LoadFile).ToList();
        }

        private static string GetMastermindDirectory()
        {
            var mastermindDirectory = string.Join(
                            Path.DirectorySeparatorChar,
                            Directory
                                .GetCurrentDirectory()
                                .Split(Path.DirectorySeparatorChar)
                                .Reverse()
                                .SkipWhile(directoryName => !"Mastermind".Equals(directoryName, StringComparison.OrdinalIgnoreCase))
                                .Reverse());
            if (string.IsNullOrWhiteSpace(mastermindDirectory))
            {
                mastermindDirectory = Directory.GetCurrentDirectory();
            }
            return mastermindDirectory;
        }
    }
}