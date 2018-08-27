namespace SensorsReport
{
    public class SensorDataFetcher
    {
        public string URL { get; set; }
        public ISensorListBuilder SensorDataBuilder { get; set; }
    }
}
