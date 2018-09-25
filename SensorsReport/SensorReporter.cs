using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SensorsReport
{
    public class SensorReporter
    {
        private CancellationTokenSource cts = new CancellationTokenSource(3000);
        private readonly SensorDataFetcher[] _sensorDataFetchers;

        public SensorReporter(SensorDataFetcher[] sensorDataFetchers)
        {
            _sensorDataFetchers = sensorDataFetchers;
        }
        internal protected async Task<List<Sensor>> GetSensorList(CancellationToken ct)
        {
            var sensorDataList = new List<Sensor>();
            using (HttpClient client = new HttpClient())
            {
                var tasks = _sensorDataFetchers.Select(async item =>
                {
                    var response = await client.GetAsync(item.URL, ct);
                    var list = await item.SensorDataBuilder.BuildSensorsAsync(response);
                    if (list.Any()) sensorDataList.AddRange(list);

                });
                await Task.WhenAll(tasks);
                return sensorDataList;
            }
        }

        public async Task<string> Report()
        {
            var sensorList = new List<Sensor>();
            try
            {
                sensorList = await GetSensorList(cts.Token);
            }
            catch (TaskCanceledException)
            {
                if (!cts.Token.IsCancellationRequested)
                {
                    throw;
                }
                else
                {
                    throw new TimeoutException("Performance issue....");
                }
            }
            var report = new SensorReport();
            if (sensorList.Any())
            {
                report = new SensorReport { Min = sensorList.Min(), Max = sensorList.Max(), Ts = DateTime.UtcNow.Ticks };

            }
            return JsonConvert.SerializeObject(report, new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        }
    }
}