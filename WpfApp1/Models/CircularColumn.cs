using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class CircularSections : Sections
    {
        public override double[] RebarInertiaCal(double radiusOfColumn, double StirrupThickness, ObservableCollection<Rebars> userInputtedRebars, double cover)
        {
            int countOfOfRebarsInTheWholeSection = GetTotalNumberOfRebars(userInputtedRebars);

            Coordinates[] rebarsCoordinates = new Coordinates[countOfOfRebarsInTheWholeSection];

            if (radiusOfColumn != 0 && userInputtedRebars is not null)
            {
                int j = 0;
                foreach (var item in userInputtedRebars)
                {

                    double areaOfRebar = CalcRebarArea(item.RebarDia);
                    double slice = (2 * Math.PI) / item.NumOfRebar; //radians

                    for (int i = 0; i < item.NumOfRebar; i++)
                    {
                        double angle = slice * i;
                        double x = (radiusOfColumn - (cover + item.DeltaY + StirrupThickness + item.RebarDia / 2)) * Math.Cos(angle);
                        double y = (radiusOfColumn - (cover + item.DeltaY + StirrupThickness + item.RebarDia / 2)) * Math.Sin(angle);
                        rebarsCoordinates[j] = new Coordinates(x, y, item.RebarDia);
                        rebarIx += ((PI * Math.Pow(item.RebarDia, 4)) / 64) + (areaOfRebar * Math.Pow(x, 2));
                        rebarIy += ((PI * Math.Pow(item.RebarDia, 4)) / 64) + (areaOfRebar * Math.Pow(y, 2));
                        j++;
                    }

                    totalAreaOfRebars += areaOfRebar;

                }

                if (AreThereOverlappingRebars(rebarsCoordinates, countOfOfRebarsInTheWholeSection))
                    return [-1, -1, -1];

            }

            return [Math.Round(rebarIx, 6), Math.Round(rebarIy, 6), Math.Round(totalAreaOfRebars, 6)];
        }

        public override double[] TotalInertiaCal(double diameterOfColumn)
        {
            double totalIx = PI * Math.Pow(diameterOfColumn, 4) / 64;
            double totalIy = PI * Math.Pow(diameterOfColumn, 4) / 64;

            return [Math.Round(totalIx, 6), Math.Round(totalIy, 6)];
        }
    }

}
