using System;
using System.Collections.Generic;

namespace retecs.ReteCs.core
{
    public class Emitter<T>
    {
        private Dictionary<string, List<Func<object, bool>>> Events { get; }

        public Emitter(Dictionary<string, List<Func<object, bool>>> events)
        {
            Events = events;
        }

        public Emitter<T> On(List<string> names, Func<object, bool> handler)
        {
            foreach (var @event in names)
            {
                if (!Events.ContainsKey(@event))
                {
                    throw new ArgumentException($"{@event} does not exist!");
                }

                Events[@event].Add(handler);
            }

            return this;
        }

        public bool Trigger(string name, params object[] data)
        {
            if (!Events.ContainsKey(name))
            {
                throw new ArgumentException($"{name} does not exist!");
            }

            var res = true;
            foreach (var func in Events[name])
            {
                if (!func.Invoke(data))
                {
                    res = false;
                }
            }

            return res;
        }

        public void Bind(string name)
        {
            if (!Events.ContainsKey(name))
            {
                throw new ArgumentException($"{name} does not exist!");
            }

            Events.Add(name, new List<Func<object, bool>>());
        }

        public bool Exist(string name)
        {
            return Events.ContainsKey(name);
        }
    }
}