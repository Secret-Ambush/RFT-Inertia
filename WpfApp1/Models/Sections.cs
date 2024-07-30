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

        protected double rebarIx = 0;
        protected double rebarIy = 0;
        protected double totalAreaOfRebars = 0;

        public int GetTotalNumberOfRebars(ObservableCollection<Rebars> userInputtedRebars)
        {
            int count = 0;
            foreach (var item in userInputtedRebars)
            {
                count += item.NumOfRebar;
            }

            return count;
        }
        
        public double CalcRebarArea(double diameterOfRebar)
        {

            double radius_Rebar = diameterOfRebar / 2;
            double A_rebar = PI * Math.Pow(radius_Rebar, 2);

            return A_rebar;
        }

        public virtual double[] RebarInertiaCal(double radiusOfRebar, double StirrupThickness, ObservableCollection<Rebars> userInputtedRebars, double cover)
        {
            return [];
        }

        public virtual double[] RebarInertiaCal(double width, double height, double StirrupThickness, ObservableCollection<Rebars> userInputtedRebars, double cover, double sidecover)
        {
            return [];
        }

        public virtual double[] TotalInertiaCal(double diameterOfColumn)
        {
            return [];
        }

        public virtual double[] TotalInertiaCal(double height, double width)
        {
            return [];
        }

        public bool AreThereOverlappingRebars(Coordinates[] rebarCoordintes, int total)
        {
            for (int i = 0; i < total - 1; i++)
            {
                double diff_x = rebarCoordintes[i].x_coordinate - rebarCoordintes[i + 1].x_coordinate;
                double diff_y = rebarCoordintes[i].y_coordinate - rebarCoordintes[i + 1].y_coordinate;
                double sum_radius = rebarCoordintes[i].radius + rebarCoordintes[i + 1].radius;
                if ((Math.Pow(diff_x, 2) + Math.Pow(diff_y, 2)) < Math.Pow(sum_radius, 2))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
