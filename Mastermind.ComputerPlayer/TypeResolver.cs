namespace Mastermind.ComputerPlayer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    internal static class TypeResolver
    {
        internal static Type GetTypeInAssembly(string assemblySearchString, Type type)
        {
            var rootDirectory = GetMastermindDirectory();
            var assembly = GetSingleAssembly(rootDirectory, assemblySearchString);
            return GetSingleType(assembly, type);
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
                .GroupBy(f => Path.GetFileName(f))
                .Select(g => g.Count() == 1 ? g.Single() : SelectTheDllInTheBinDirectory(g))
                .Where(f => !(f is null))
                .ToList();
        }

        private static string SelectTheDllInTheBinDirectory(IEnumerable<string> differentPathsToTheSameFile)
        {
            // use the one that matches */assemblyName/bin/*
            var assemblyName = Path.GetFileNameWithoutExtension(differentPathsToTheSameFile.First());
            var pathToLookFor = $"{Path.DirectorySeparatorChar}{assemblyName}{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}";
            return differentPathsToTheSameFile.FirstOrDefault(f => f.Contains(pathToLookFor));
        }

        private static Type GetSingleType(Assembly assembly, Type type)
        {
            var t = GetSingleOrNoType(assembly, type);
            if (t is null)
            {
                throw new Exception($"No type found in {assembly.FullName} that is assignable to {type.Name} and has a public default constructor.");
            }
            return t;
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

        private static Assembly GetSingleAssembly(string rootDirectory, string assemblySearchName)
        {
            string assemblyFilePath;
            if (File.Exists(assemblySearchName))
            {
                assemblyFilePath = assemblySearchName;
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
                else if (paths.Count == 1)
                {
                    assemblyFilePath = paths[0];
                }
                else
                {
                    var message = $"Found multiple dll files with a name containing {assemblySearchName} in {rootDirectory} (including subdirectories).";
                    var pathList = string.Join(Environment.NewLine, paths);
                    throw new Exception($"{message}{Environment.NewLine}{pathList}");
                }
            }
            return Assembly.LoadFile(assemblyFilePath);
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