using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    abstract class Sections
    {

        protected const double PI = 3.14;

        public double CalcRebarArea( double diameterOfRebar) 
        {

            double radius_Rebar = diameterOfRebar / 2;
            double A_rebar = PI * Math.Pow(radius_Rebar, 2);

            return A_rebar;
        }

        public virtual double[] RebarInertiaCal(double radiusOfRebar, double StirrupThickness, ObservableCollection<Rebars> userInputtedRebars, double cover)
        {
            return new double[0];
        }

        public virtual double[] RebarInertiaCal(double width, double height, double StirrupThickness, ObservableCollection<Rebars> userInputtedRebars, double cover, double sidecover)
        {
            return new double[0];
        }

        public virtual double[] TotalInertiaCal(double diameterOfColumn)
        {
            return new double[0];
        }

        public virtual double[] TotalInertiaCal(double height, double width)
        {
            return new double[0];
        } 
    }
}
