using System;
using System.Collections.Generic;

namespace retecs.ReteCs.core
{
    public class Context
    {
        public Emitter Emitter { get; set; }
        public string Id { get; set; }
        public Dictionary<string, object> Plugins { get; set; }
        public Dictionary<string, Component> Components { get; set; }

        public Context(string id, Emitter emitter)
        {
            Emitter = emitter;
            if(Validator.IsValidId(id))
                throw new Exception("Id should be valid to name@0.1.0 format");
            Id = id;
            Plugins = new Dictionary<string, object>();
            Components = new Dictionary<string, Component>();
        }
        
        public void Use<T1>(T1 plugin, PluginParams<T1> options = null) where T1: Plugin
        {
            if (Plugins.ContainsKey(plugin.Name))
            {
                throw new Exception($"Plugin {plugin.Name} already in use`");
            }

            plugin.Install(this, options ?? new PluginParams<T1>());
            Plugins.Add(plugin.Name, options);
        }

        public void Register(Component component)
        {
            if (Components.ContainsKey(component.Name))
            {
                throw new Exception($"Component {component.Name} already registered");
            }
            
            Components.Add(component.Name, component);
            Emitter.OnComponentRegister(component);
        }

        ~Context()
        {
            Emitter.OnDestroy();
        }
    }
}