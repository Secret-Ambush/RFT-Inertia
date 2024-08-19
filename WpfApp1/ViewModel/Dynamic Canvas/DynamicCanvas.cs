using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApp1
{
    public class DynamicCanvas
    {
        public static void DrawRebars(WindowViewModel viewModelObject)
        {
            // Clear existing elements in RebarsForCanvas
            viewModelObject.RebarsForCanvas.Clear();

            double canvasSize = 350;
            double canvasCenter = canvasSize / 2;
            double scale = 1;

            if (viewModelObject.SelectedSection == "Circular Column")
            {
                viewModelObject.DisplayCircleGuideIllustration = false;
                viewModelObject.DisplaySmallCircleGuide = true;

                double columnDiameter = viewModelObject.Diameter;

                if (columnDiameter > 300)
                {
                    scale = 300 / columnDiameter;
                }

                double margin = canvasCenter - viewModelObject.Radius * scale;

                //Adding Circular Column
                Ellipse Circularcolumn = new Ellipse
                {
                    Width = viewModelObject.Diameter * scale,
                    Height = viewModelObject.Diameter * scale,
                    Stroke = System.Windows.Media.Brushes.Green,
                    StrokeThickness = 2,
                    Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(73, 0, 180, 104)),
                    Margin = new Thickness(margin, margin, 0, 0)
                };

                viewModelObject.RebarsForCanvas.Add(Circularcolumn);

                //Adding Rebars
                foreach (var item in viewModelObject.UserRebarEntries)
                {
                    int numberOfRebars = item.NumOfRebar;
                    double rebarRadius = item.RebarDia / 2;
                    double angleStep = 2 * Math.PI / numberOfRebars;

                    for (int i = 0; i < item.NumOfRebar; i++)
                    {
                        double angle = angleStep * i;
                        double rebarX = ((viewModelObject.Radius - (viewModelObject.Cover + item.DeltaY + viewModelObject.SelectedStirrupThickness + item.RebarDia / 2)) * Math.Cos(angle)) * scale;
                        double rebarY = ((viewModelObject.Radius - (viewModelObject.Cover + item.DeltaY + viewModelObject.SelectedStirrupThickness + item.RebarDia / 2)) * Math.Sin(angle)) * scale;

                        double margin_left = canvasCenter - (rebarRadius * scale) - rebarX;
                        double margin_top = canvasCenter - (rebarRadius * scale) - rebarY;

                        Ellipse rebar_columns = new Ellipse
                        {
                            Width = item.RebarDia * scale,
                            Height = item.RebarDia * scale,
                            Stroke = System.Windows.Media.Brushes.Black,
                            StrokeThickness = 2,
                            Margin = new Thickness(margin_left, margin_top, 0, 0)
                        };

                        viewModelObject.RebarsForCanvas.Add(rebar_columns);
                    }

                    //Adding Stirrups

                    double StirrupRadius = (viewModelObject.Radius - (viewModelObject.Cover + item.DeltaY));
                    double StirrupDia = StirrupRadius * 2;

                    double stirrupMargin = canvasCenter - StirrupRadius * scale;

                    Ellipse Stirrup = new Ellipse
                    {
                        Width = StirrupDia * scale,
                        Height = StirrupDia * scale,
                        Stroke = System.Windows.Media.Brushes.DarkRed,
                        StrokeThickness = viewModelObject.SelectedStirrupThickness * scale,
                        Margin = new Thickness(stirrupMargin, stirrupMargin, 0, 0)
                    };

                    viewModelObject.RebarsForCanvas.Add(Stirrup);
                }
            }

            else
            {
                viewModelObject.DisplayRectangleGuideIllustration = false;
                viewModelObject.DisplaySmallRectangleGuide = true;
                double columnBreadth = viewModelObject.Breadth;
                double columnHeight = viewModelObject.Height;


                if (columnBreadth > columnHeight && columnBreadth > 300)
                {
                    scale = 300 / columnBreadth;
                }

                else if (columnBreadth < columnHeight && columnHeight > 300)
                {
                    scale = 300 / columnHeight;
                }

                double margin_left = canvasCenter - (columnBreadth / 2) * scale;
                double margin_top = canvasCenter - (columnHeight / 2) * scale;

                System.Windows.Shapes.Rectangle RectangularColumn = new System.Windows.Shapes.Rectangle
                {
                    Width = columnBreadth * scale,
                    Height = columnHeight * scale,
                    Stroke = System.Windows.Media.Brushes.Green,
                    StrokeThickness = 2,
                    Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(73, 0, 180, 104)),
                    Margin = new Thickness(margin_left, margin_top, 0, 0)
                };

                viewModelObject.RebarsForCanvas.Add(RectangularColumn);

                //Starting corner of rectangle column
                double corner_x = margin_left;
                double corner_y = margin_top;

                //Adding Rebars + Stirrups
                foreach (var item in viewModelObject.UserRebarEntries)
                {
                    int numberOfRebars = item.NumOfRebar;
                    double rebarRadius = item.RebarDia / 2;
                    double spaceForRebarPlacement = viewModelObject.Breadth - 2 * (viewModelObject.SideCover + viewModelObject.SelectedStirrupThickness);
                    double spacing = (spaceForRebarPlacement - 2 * rebarRadius) / (item.NumOfRebar - 1);

                    for (int i = 0; i < item.NumOfRebar; i++)
                    {
                        double rebarX = (viewModelObject.SideCover + viewModelObject.SelectedStirrupThickness + (i * spacing)) * scale;
                        double rebarY = (viewModelObject.Cover + viewModelObject.SelectedStirrupThickness + item.DeltaY) * scale;

                        double margin_left_rebar = corner_x + rebarX;
                        double margin_top_rebar = corner_y + rebarY;

                        Ellipse rebarColumn = new Ellipse
                        {
                            Width = item.RebarDia * scale,
                            Height = item.RebarDia * scale,
                            Stroke = System.Windows.Media.Brushes.Black,
                            StrokeThickness = 2,
                            Margin = new Thickness(margin_left_rebar, margin_top_rebar, 0, 0)
                        };

                        viewModelObject.RebarsForCanvas.Add(rebarColumn);
                    }

                    //Adding Stirrups

                    double StirrupHeight = viewModelObject.Height - 2 * viewModelObject.Cover;
                    double StirrupWidth = viewModelObject.Breadth - 2 * viewModelObject.SideCover;

                    double stirrupXMargin = corner_x + viewModelObject.SideCover * scale;
                    double stirrupYMargin = corner_y + viewModelObject.Cover * scale;

                    System.Windows.Shapes.Rectangle Stirrup = new System.Windows.Shapes.Rectangle
                    {
                        Width = StirrupWidth * scale,
                        Height = StirrupHeight * scale,
                        Stroke = System.Windows.Media.Brushes.DarkRed,
                        StrokeThickness = viewModelObject.SelectedStirrupThickness * scale,
                        Margin = new Thickness(stirrupXMargin, stirrupYMargin, 0, 0),
                        RadiusX = rebarRadius * scale,
                        RadiusY = rebarRadius * scale

                    };


                    viewModelObject.RebarsForCanvas.Add(Stirrup);
                }

            }
        }

    }
}
