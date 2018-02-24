using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using StructureMap;

namespace Sweesh.Core.Configuration
{
    public class Setup
    {
        private List<Type> _allTypesOf = new List<Type>();
        private List<Action<ConfigurationExpression>> _configs = new List<Action<ConfigurationExpression>>();

        private Container _cont;

        public Setup(IConfigurationRoot config)
        {
            _configs.Add(t => {
                t.For<IConfiguration>().Use(x => config);
                t.For<IConfigurationRoot>().Use(x => config);
            });
        }

        public Container Build()
        {
            if (_cont != null)
                return _cont;

            _cont = new Container();

            _cont.Configure(t => {
                t.Scan(c =>
                {
                    c.AssembliesAndExecutablesFromApplicationBaseDirectory();
                    c.TheCallingAssembly();
                    c.WithDefaultConventions();

                    foreach (var type in _allTypesOf)
                    {
                        c.AddAllTypesOf(type);
                    }

                });

                foreach (var config in _configs)
                {
                    config?.Invoke(t);
                }
            });

            return _cont;
        }

        public Setup AddTypesOf<T>()
        {
            _allTypesOf.Add(typeof(T));
            return this;
        }

        public Setup AddTypesOf(Type type)
        {
            _allTypesOf.Add(type);
            return this;
        }

        public Setup Configure(params Action<ConfigurationExpression>[] confis)
        {
            foreach (var thing in confis)
                _configs.Add(thing);
            return this;
        }
    }
}
