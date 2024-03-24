using System.Collections.Concurrent;
using System.Reflection;

namespace Chat.Framework;

public sealed class AssemblyCache
{
    private static readonly object LockObj = new();
    private static AssemblyCache? _instance;

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

    private readonly ConcurrentDictionary<string, Assembly> _assemblyLists;
    
    private AssemblyCache()
    {
        _assemblyLists = new();
    }

    public AssemblyCache AddAllAssemblies(string assemblyPrefix)
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

        return this;
    }

    public AssemblyCache AddAllAssemblies(string location, string assemblyPrefix)
    {
        if (string.IsNullOrEmpty(location)) return this;

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

        return this;
    }

    public AssemblyCache AddAssemblies(params Assembly[] assemblies)
    {
        AddAssemblies(assemblies.ToList());
        return this;
    }

    public AssemblyCache AddAssemblies(List<Assembly> assemblies)
    {
        assemblies.ForEach(assembly => AddAssembly(assembly));
        return this;
    }

    public AssemblyCache AddAssembly(Assembly assembly)
    {
        if (string.IsNullOrEmpty(assembly.FullName) ||
            _assemblyLists.ContainsKey(assembly.FullName))
        {
            Console.WriteLine($"{assembly.FullName} already added\n");
            return this;
        }

        _assemblyLists.TryAdd(assembly.FullName, assembly);
        Console.WriteLine($"Added Assembly {assembly.FullName}\n");
        
        return this;
    }

    public List<Assembly> GetAddedAssemblies()
    {
        return _assemblyLists.Values.ToList();
    }
}