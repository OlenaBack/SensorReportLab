using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SensorsReport
{
    public class JsonObjectsSensorListBuilder : ISensorListBuilder
    {
        public async Task<IList<Sensor>> BuildSensorsAsync(HttpResponseMessage response)
        {
            var objectDictionary= JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(await response.Content.ReadAsStringAsync());
            return objectDictionary.Keys.Select(k=> new Sensor { Name= k, Intensity = (int)(long)objectDictionary[k]["intensity"]}).ToList();
        }
    }
}
