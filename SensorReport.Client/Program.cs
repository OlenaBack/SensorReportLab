using System;
using SensorsReport;

namespace SensorReport.Client
{
    class Program
    {
        static void Main(string[] args)
        {            
            var fetchBuilder = new SensorDataFetchBuilder();
            var _sensorDataFetchers = new[] { fetchBuilder.CreateFetcher("http://www.mocky.io/v2/5b23bd852f00003d1ce096df", "xmlsensorlistbuilder"),
            fetchBuilder.CreateFetcher("http://www.mocky.io/v2/5b23bcf72f00003215e096dd", "simplejsonsensorlistbuider"),
            fetchBuilder.CreateFetcher("http://www.mocky.io/v2/5b23be902f00000e00e096e2", "jsonobjectssensorlistbuilder")
            };
            var rep = new SensorReporter(_sensorDataFetchers);
            var result= rep.Report().Result;
            Console.WriteLine(result);
            Console.ReadLine();
        }
    }
}
