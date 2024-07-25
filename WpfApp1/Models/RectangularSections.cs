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
            double rebarIx = 0;
            double rebarIy = 0;
            double totalAreaOfRebars = 0;
            int countOfOfRebarsInTheWholeSection = 0;

            foreach (var item in userInputtedRebars)
            {
                countOfOfRebarsInTheWholeSection += item.NumOfRebar;
            }

            Coordinates[] rebarsCoordinates = new Coordinates[countOfOfRebarsInTheWholeSection];

            if (width != 0 && height !=0 && userInputtedRebars is not null)
            {
                int j = 0;
                foreach (var item in userInputtedRebars)
                {
                    double spacing = (width + 2 * sidecover) / item.NumOfRebar;
                    double areaOfRebar = CalcRebarArea(item.RebarDia);
                    
                    for (int i = 0; i < item.NumOfRebar; i++)
                    {
                        double x = sidecover + (i * spacing);
                        double y = cover + item.DeltaY;
                        rebarIx += ((PI * Math.Pow(item.RebarDia, 4)) / 64) + (areaOfRebar * Math.Pow(x, 2));
                        rebarIy += ((PI * Math.Pow(item.RebarDia, 4)) / 64) + (areaOfRebar * Math.Pow(y, 2));
                        rebarsCoordinates[j] = new Coordinates(x, y, item.RebarDia);
                        j++;
                    }

                    totalAreaOfRebars += areaOfRebar;

                }

                for (int i = 0; i < countOfOfRebarsInTheWholeSection - 1; i++)
                {
                    double diff_x = rebarsCoordinates[i].x_coordinate - rebarsCoordinates[i + 1].x_coordinate;
                    double diff_y = rebarsCoordinates[i].y_coordinate - rebarsCoordinates[i + 1].y_coordinate;
                    double sum_radius = rebarsCoordinates[i].radius + rebarsCoordinates[i + 1].radius;
                    if ((Math.Pow(diff_x, 2) + Math.Pow(diff_y, 2)) < Math.Pow(sum_radius, 2))
                    {
                        return [-1, -1, -1];
                    }
                }

            }

            return [Math.Round(rebarIx, 6), Math.Round(rebarIy, 6), Math.Round(totalAreaOfRebars, 6)];
        }

        public override double[] TotalInertiaCal(double breadth, double height)
        {
            double totalIx = (breadth * Math.Pow(height, 3)) / 12;
            double totalIy = (breadth * Math.Pow(height, 3)) / 12;

            return [Math.Round(totalIx, 6), Math.Round(totalIy, 6) ];
        }
    }
}
