using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Syriaca.Client.Utils;

namespace Syriaca.Client.Plugins
{
    public class PluginStore : IDisposable
    {
        private const string plugin_library_prefix = "Syriaca.Plugin";

        private readonly Dictionary<Assembly, Type> loadedAssemblies = new();

        public PluginStore()
        {
            loadFromDisk();
            addMissingPlugins();
        }
        
        public IEnumerable<Plugin> AvailablePlugins { get; private set; }

        private void addMissingPlugins()
        {
            var instances = loadedAssemblies.Values.Select(p => (Plugin) Activator.CreateInstance(p)).ToList();
            var index = 0;

            foreach (var plugin in instances)
                plugin.Id = index++;

            AvailablePlugins = instances;
        }

        private void loadFromDisk()
        {
            try
            {
                var files = Directory.GetFiles(Environment.CurrentDirectory, $"{plugin_library_prefix}.*dll");

                foreach (var file in files)
                    loadPluginFromFile(file);
            }
            catch (Exception e)
            {
                Logger.Error($"Could not load plugins from directory {Environment.CurrentDirectory}");
                Logger.Error(e);
            }
        }

        private void loadPluginFromFile(string file)
        {
            var fileName = Path.GetFileNameWithoutExtension(file);
            
            if (loadedAssemblies.Values.Any(t => t.Namespace == fileName))
                return;

            try
            {
                addPlugin(Assembly.LoadFrom(file));
            }
            catch (Exception e)
            {
                Logger.Error($"Failed to load plugin {fileName}");
                Logger.Error(e);
            }
        }

        private void addPlugin(Assembly assembly)
        {
            if (loadedAssemblies.ContainsKey(assembly))
                return;

            try
            {
                loadedAssemblies[assembly] =
                    assembly.GetTypes().First(t => t.IsPublic && t.IsSubclassOf(typeof(Plugin)));
            }
            catch (Exception e)
            {
                Logger.Error($"Failed to add plugin {assembly}");
                Logger.Error(e);
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}