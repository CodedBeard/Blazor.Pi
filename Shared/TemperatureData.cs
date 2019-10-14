using System;
using System.Collections.Generic;
using System.Text;

namespace Blazor.Pi.Shared
{
    public class TemperatureData
    {
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public bool IsLastReadSuccessful { get; set; }
    }
}