namespace SensorsReport
{
    public class SensorDataFetcher
    {
        public string URL { get; set; }
        public ISensorListBuilder SensorDataBuilder { get; set; }

        public SensorDataFetcher(string url, ISensorListBuilder sensorDataBuilder)
        {
            URL = url;
            SensorDataBuilder = sensorDataBuilder;
        }

        public SensorDataFetcher()
        {
        }
    }
}
