using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SensorsReport;

namespace SensorReport.Client
{
    class SensorDataFetchBuilder
    {
        private IDictionary<string, Type> _fetchers;
        public SensorDataFetchBuilder()
        {
            LoadFetcherTypes();
        }

        private void LoadFetcherTypes()
        {
            var types = Assembly.GetAssembly(typeof(ISensorListBuilder)).GetTypes().ToList();
            _fetchers = types.Where(t => t.GetInterface(typeof(ISensorListBuilder).ToString()) != null).ToDictionary(t => t.Name.ToString().ToLower(), t => t);
        }

        public SensorDataFetcher CreateFetcher(string uri, string fetcherType)
        {
            if (!_fetchers.ContainsKey(fetcherType)) return null;// Report.Null;
            var listBuilder= Activator.CreateInstance(_fetchers[fetcherType]) as ISensorListBuilder;
            return new SensorDataFetcher(uri, listBuilder);
        }

    }
}
