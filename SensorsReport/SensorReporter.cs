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
        private CancellationTokenSource cts = new CancellationTokenSource(300);
        private readonly SensorDataFetcher[] _sensorDataFetchers = new[] {
            new SensorDataFetcher { URL = "http://www.mocky.io/v2/5b23bd852f00003d1ce096df", SensorDataBuilder = new XmlSensorListBuilder() },
            new SensorDataFetcher { URL = "http://www.mocky.io/v2/5b23bcf72f00003215e096dd", SensorDataBuilder = new SimpleJsonSensorListBuider() },
            new SensorDataFetcher { URL = "http://www.mocky.io/v2/5b23be902f00000e00e096e2", SensorDataBuilder = new JsonObjectsSensorListBuilder() }
        };

        public async Task<List<Sensor>> GetSensorList(CancellationToken ct)
        {
            var sensorDataList = new List<Sensor>();
            using (HttpClient client = new HttpClient())
            {
                foreach (var item in _sensorDataFetchers)
                {
                    var response = await client.GetAsync(item.URL, ct);
                    var list = await item.SensorDataBuilder.BuildSensorsAsync(response);
                    if (list.Any()) sensorDataList.AddRange(list);
                }
                return sensorDataList;
            }
        }

        public async Task<string> GetSensorsReport()
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