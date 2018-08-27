using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SensorsReport
{
    public interface ISensorListBuilder
    {
        Task<IList<Sensor>> BuildSensorsAsync(HttpResponseMessage response);
    }
}