using System;

namespace SensorsReport
{
    public class Sensor:IComparable<Sensor>
    {       
        public string Name { get; set; }
        public Int32 Intensity { get; set; }
        public int CompareTo(Sensor other) { return Intensity.CompareTo(other.Intensity); }
    }
}
