using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class RectangularSections : Sections
    {
        public override double[] RebarInertiaCal(double width, double height, double StirrupThickness, ObservableCollection<Rebars> userInputtedRebars, double cover, double sidecover)
        {

            int countOfOfRebarsInTheWholeSection = GetTotalNumberOfRebars(userInputtedRebars);

            Coordinates[] rebarsCoordinates = new Coordinates[countOfOfRebarsInTheWholeSection];

            if (width != 0 && height != 0 && userInputtedRebars is not null)
            {
                int j = 0;
                foreach (var item in userInputtedRebars)
                {
                    double rebarRadius = item.RebarDia / 2;
                    double spaceForRebarPlacement = width - 2 * (sidecover + StirrupThickness);
                    double spacing = (spaceForRebarPlacement - 2 * rebarRadius) / (item.NumOfRebar - 1);
                    double areaOfRebar = CalcRebarArea(item.RebarDia);

                    for (int i = 0; i < item.NumOfRebar; i++)
                    {
                        double x = sidecover + StirrupThickness + (i * spacing);
                        double y = cover + StirrupThickness + item.DeltaY;
                        rebarIx += ((PI * Math.Pow(item.RebarDia, 4)) / 64) + (areaOfRebar * Math.Pow(x, 2));
                        rebarIy += ((PI * Math.Pow(item.RebarDia, 4)) / 64) + (areaOfRebar * Math.Pow(y, 2));
                        rebarsCoordinates[j] = new Coordinates(x, y, item.RebarDia);
                        j++;
                    }

                    totalAreaOfRebars += areaOfRebar;

                }

                if (AreThereOverlappingRebars(rebarsCoordinates, countOfOfRebarsInTheWholeSection))
                    return [-1, -1, -1];

            }

            return [Math.Round(rebarIx, 6), Math.Round(rebarIy, 6), Math.Round(totalAreaOfRebars, 6)];
        }

        public override double[] TotalInertiaCal(double breadth, double height)
        {
            double totalIx = (breadth * Math.Pow(height, 3)) / 12;
            double totalIy = (breadth * Math.Pow(height, 3)) / 12;

            return [Math.Round(totalIx, 6), Math.Round(totalIy, 6)];
        }
    }
}
