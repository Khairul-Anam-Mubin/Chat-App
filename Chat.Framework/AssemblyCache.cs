using System.Collections.Concurrent;
using System.Reflection;

namespace Chat.Framework;

public sealed class AssemblyCache
{
    private static readonly object LockObj = new();
    private static AssemblyCache? _instance;

    private readonly ConcurrentDictionary<string, Assembly> _assemblyLists;

    public static AssemblyCache Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (LockObj)
                {
                    _instance ??= new AssemblyCache();
                }
            }
            return _instance;
        }
    }

    private AssemblyCache()
    {
        _assemblyLists = new();
    }

    public void AddAllAssemblies(string assemblyPrefix)
    {
        var entryAssemblyLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);

        if (!string.IsNullOrEmpty(entryAssemblyLocation))
        {
            AddAllAssemblies(entryAssemblyLocation, assemblyPrefix);
        }

        var executingAssemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        if (!string.IsNullOrEmpty(executingAssemblyLocation))
        {
            AddAllAssemblies(executingAssemblyLocation, assemblyPrefix);
        }
    }

    public void AddAllAssemblies(string location, string assemblyPrefix)
    {
        if (string.IsNullOrEmpty(location)) return;

        var files = Directory.GetFiles(location);

        foreach (var file in files)
        {
            try
            {
                var fileInfo = new FileInfo(file);

                if (!fileInfo.Name.StartsWith(assemblyPrefix, StringComparison.InvariantCultureIgnoreCase) ||
                    fileInfo.Extension != ".dll") continue;

                var assemblyName = Path.GetFileNameWithoutExtension(file);
                if (string.IsNullOrEmpty(assemblyName)) continue;

                var assembly = Assembly.Load(assemblyName);
                AddAssembly(assembly);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    public void AddAssembly(Assembly assembly)
    {
        if (string.IsNullOrEmpty(assembly.FullName) ||
            _assemblyLists.ContainsKey(assembly.FullName))
        {
            Console.WriteLine($"{assembly.FullName} already added\n");
            return;
        }

        _assemblyLists.TryAdd(assembly.FullName, assembly);
        Console.WriteLine($"Added Assembly {assembly.FullName}\n");
    }

    public List<Assembly> GetAddedAssemblies()
    {
        return _assemblyLists.Values.ToList();
    }
}