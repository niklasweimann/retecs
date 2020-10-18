using System;
using System.Collections.Generic;
using retecs.ReteCs.Engine;


namespace retecs.ReteCs.core
{
    public class Context<T> : Emitter<T>
    {
        public string Id { get; set; }
        public Dictionary<string, object> Plugins { get; set; }
        public Dictionary<string, Component> Components { get; set; }

        public Context(string id, Dictionary<string, List<Func<object, bool>>> events) : base(events)
        {
            if(Validator.IsValidId(id))
                throw new Exception("Id should be valid to name@0.1.0 format");
            Id = id;
            Plugins = new Dictionary<string, object>();
            Components = new Dictionary<string, Component>();
        }
        
        public void Use<T1, T2>(T1 plugin, T2 options = null) where T1: Plugin where T2: PluginParams<T1>
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
            Trigger("componentregister", component);
        }

        ~Context()
        {
            Trigger("destroy");
        }
    }
}