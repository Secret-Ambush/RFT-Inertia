using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Coordinates
    {
        public double x_coordinate;
        public double y_coordinate;
        public double radius;

        public Coordinates(double x, double y, double diameter)
        {
            x_coordinate = x;
            y_coordinate = y;
            radius = diameter / 2;
        }
    }
}
