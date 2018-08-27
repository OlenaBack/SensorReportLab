using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SensorsReport
{
    public class SimpleJsonSensorListBuider : ISensorListBuilder
    {
        public async Task<IList<Sensor>> BuildSensorsAsync(HttpResponseMessage response)
        {
            return JsonConvert.DeserializeObject<List<Sensor>>(await response.Content.ReadAsStringAsync());
        }
    }
}
