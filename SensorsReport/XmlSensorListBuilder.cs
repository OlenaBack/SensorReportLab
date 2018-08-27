using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SensorsReport
{
    public class SensorStrings
    {
        [XmlElement("name")]
        public string NamesString { get; set; }

        [XmlElement("intensity")]
        public string IntensitiesString { get; set; }
    }
    public class XmlSensorListBuilder : ISensorListBuilder
    {
        public async Task<IList<Sensor>> BuildSensorsAsync(HttpResponseMessage response)
        {
            var deserializer = new XmlSerializer(typeof(SensorStrings), new XmlRootAttribute { ElementName = "sensors", IsNullable = true });
            var desirialized = (SensorStrings)deserializer.Deserialize(await response.Content.ReadAsStreamAsync());

            var sensors = new List<Sensor>();
            var names = desirialized.NamesString.Split(',').Select(x => x.Trim()).ToArray();
            var intensities = desirialized.IntensitiesString.Split(',').Select(x => Convert.ToInt32(x.Trim())).ToArray();

            for (int i = 0; i < Math.Min(names.Length, intensities.Length); i++)
            {
                sensors.Add(new Sensor { Name = names[i], Intensity = intensities[i] });
            }
            return sensors;
        }
    }
}
