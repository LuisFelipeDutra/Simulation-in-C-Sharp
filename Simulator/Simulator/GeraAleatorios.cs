using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator
{
    public class GeraAleatorios
    {
        static Random r = new Random();
        public GeraAleatorios() { }
        public double receiveRandomIn(double low, double high)
        {
            double result = r.NextDouble() * (high - low) + low;
            return result;
        }
    }
}
