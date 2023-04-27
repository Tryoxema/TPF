using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TPF.Demo.Net461.Controls
{
    public static class SearchManager
    {
        static SearchManager()
        {
            Initialize();
        }

        private static readonly Dictionary<string, List<SearchMetadataAttribute>> _allSearchableWindows = new Dictionary<string, List<SearchMetadataAttribute>>();

        private static void Initialize()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            for (var i = 0; i < assemblies.Length; i++)
            {
                var assembly = assemblies[i];

                if (assembly.IsDynamic) continue;

                var types = assembly.GetExportedTypes();

                for (var j = 0; j < types.Length; j++)
                {
                    var type = types[j];

                    var windowAttribute = type.GetCustomAttribute<WindowMetadataAttribute>();

                    if (windowAttribute != null)
                    {
                        var searchMetadata = type.GetCustomAttributes<SearchMetadataAttribute>();

                        var count = searchMetadata.Count();

                        if (count > 0 && !_allSearchableWindows.TryGetValue(windowAttribute.Name, out var attributes))
                        {
                            _allSearchableWindows.Add(windowAttribute.Name, new List<SearchMetadataAttribute>(searchMetadata));
                        }
                    }
                }
            }
        }

        public static List<SearchResult> Search(string searchTerm)
        {
            var result = new List<SearchResult>();

            var regex = new Regex($"({searchTerm})", RegexOptions.IgnoreCase);

            foreach (var item in _allSearchableWindows.Where(x => x.Value.Any(y => regex.Match(y.Name).Success)))
            {
                var values = item.Value.Where(x => regex.Match(x.Name).Success);

                foreach (var value in values)
                {
                    result.Add(new SearchResult(item.Key, value.Name));
                }
            }

            return result;
        }

        /// <summary>
        /// Diese Methode existiert nur um die Statische Instanz zu erschaffen, um Zeit bei der ersten Suche zu sparen
        /// </summary>
        public static void Create() { }
    }

    public class SearchResult
    {
        public string InternalName { get; set; }

        public string DisplayName { get; set; }

        public SearchResult(string internalName, string displayName)
        {
            InternalName = internalName;
            DisplayName = displayName;
        }
    }
}