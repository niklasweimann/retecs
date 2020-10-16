using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace retecs.ReteCs.core
{
    public class Context<T> : Emitter<EventTypes>
    {
        public string Id { get; set; }
        public Dictionary<string, object> Plugins { get; set; }
        public Dictionary<string, Component> Components { get; set; }

        public Context(string id, Events events) : base(events)
        {
            if(Validator.isValidId(id))
                throw new Exception("Id should be valid to name@0.1.0 format");
            Id = id;
            Plugins = new Dictionary<string, object>();
            Components = new Dictionary<string, Component>();
        }
        
        public void Use<T1, T2>(T1 plugin, T2 options = null) where T1: Plugin, T2: PluginParams<T1>
        {
            if (Plugins.TryGetValue(plugin.Name))
            {
                throw new Exception($"Plugin {plugin.name} already in use`");
            }

            plugin.install(this, options ?? new PluginParams<T1>());
            Plugins.Add(plugin.name, options);
        }

        public void Register(Component component)
        {
            if (Components.TryGetValue(component.Name))
            {
                throw new Exception($"Component {component.name} already registered");
            }
            
            Components.Add(component.name, component);
            Trigger("componentregister", component);
        }

        ~Context()
        {
            Trigger("destroy");
        }
    }
}