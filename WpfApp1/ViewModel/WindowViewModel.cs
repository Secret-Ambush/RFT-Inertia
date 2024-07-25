using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Xml.Linq;
using JacobsApp.ViewModel;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Data.Common;

namespace WpfApp1
{
    public class WindowViewModel : BaseViewModel
    {
        #region Private Properties

        private Window mWindow;
        private ObservableCollection<Shape> _rebarsForCanvas;

        private ObservableCollection<Rebars>? _userint;
        private ObservableCollection<string> _sectionTypeOptions; 
        private ObservableCollection<double> _stirrupThicknessOptions;
        private string _selectedSectionType;
        private double _selectedStirrupThickness;

        private static bool _isRectangularSection = true;
        
        private string _image = "Images/full.jpg";
        private double _radius;
        private double _diameter;
        private double _breadth;
        private double _height;
        private double _sidecover;
        private static double _cover = 0;

        private static bool _calcCanExecute;
        const double PI = 3.14f;

        private double _totalAreaOfSection;
        private static double _areaOfRebars = 0;
        private static double _rebarIx = 0;
        private static double _rebarIy = 0;
        private static double _rebarRx = 0;
        private static double _rebarRy = 0;
        private static double _totalIx = 0;
        private static double _totalIy = 0;
        private static double _totalRx = 0;
        private static double _totalRy = 0;

        #endregion

        #region Public Properties/Methods

        //Image for fullscreen, normal screen button
        public string Image
        {
            get => _image;
            set { _image = value; OnPropertyChanged(nameof(Image)); }
        }

        //Obs Collection for Combobox options
        public ObservableCollection<string>? SectionTypeOptions
        {
            get { return _sectionTypeOptions; }
            set
            {
                _sectionTypeOptions = value;
                OnPropertyChanged(nameof(SectionTypeOptions));
            }
        }
        public ObservableCollection<double>? StirrupThicknessOptions
        {
            get { return _stirrupThicknessOptions; }
            set
            {
                _stirrupThicknessOptions = value;
                OnPropertyChanged(nameof(Entries));
            }
        }

        // List of Validation Errors
        public static string[] errorList = new string[11];
        
        //Commands
        public ICommand MinimizeCommand { get; set; }
        public ICommand MaximizeCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public ICommand CalculateInertia { get; set; }
        public ICommand ClearAll { get; set; }

        // User Inputs
        public double Radius
        {
            get => _radius;
            set
            {
                _radius = value; OnPropertyChanged(nameof(Radius));
                Diameter = _radius * 2; OnPropertyChanged(nameof(Diameter));
                // ValidateCover();
            }
        }
        public double Diameter
        {
            get => _diameter;
            set
            {
                _diameter = value; OnPropertyChanged(nameof(Diameter));
                _radius = _diameter / 2; OnPropertyChanged(nameof(Radius));
                // ValidateCover();
            }
        }
        public double Height
        {
            get => _height;
            set
            {
                _height = value; OnPropertyChanged(nameof(Height));
                // ValidateCover();
            }
        }
        public double Breadth
        {
            get => _breadth;
            set
            {
                _breadth = value; OnPropertyChanged(nameof(Breadth));
                // ValidateSideCover();
            }
        }
        public double SideCover
        {
            get => _sidecover;
            set
            {
                _sidecover = value; OnPropertyChanged(nameof(SideCover));
            }
        }
        public double Cover
        {
            get => _cover;
            set
            {
                _cover = value; OnPropertyChanged(nameof(Cover));
                _sidecover = value; OnPropertyChanged(nameof(SideCover));
            }
        }
        public string SelectedSection
        {
            get { return _selectedSectionType; }
            set
            {
                ClearContentUponSelection();

                _selectedSectionType = value;
                switch (_selectedSectionType)
                {
                    case ("Rectangular Column"):
                    case ("Rectangular Beam"):
                        { isRectangularSection = true; break; }
                    case ("Circular Column"):
                        { isRectangularSection = false; break; }
                }

            }
        }
        public double SelectedStirrupThickness
        {
            get { return _selectedStirrupThickness; }
            set
            {
                _selectedStirrupThickness = value;
                OnPropertyChanged(nameof(SelectedStirrupThickness));
            }
        }

        // Obs Collection for Datagrid
        public ObservableCollection<Rebars>? Entries
        {
            get { return _userint; }
            set
            {
                _userint = value;
                OnPropertyChanged(nameof(Entries));
            }
        }

        //Helpers
        public bool isRectangularSection
        {
            get { return _isRectangularSection; }
            set
            {
                _isRectangularSection = value;
                OnPropertyChanged(nameof(_isRectangularSection));
            }
        }
        public bool calCanExecute
        {
            get => _calcCanExecute;
            set
            {
                _calcCanExecute = value;
                OnPropertyChanged(nameof(calCanExecute));
            }
        }
        public static bool[] HasErrors = new bool[10];

        // Calculated Values
        public double RebarIx
        {
            get => _rebarIx;
            set
            {
                _rebarIx = value; OnPropertyChanged(nameof(RebarIx));
            }
        }
        public double RebarIy
        {
            get => _rebarIy;
            set
            {
                _rebarIy = value; OnPropertyChanged(nameof(RebarIy));
            }
        }
        public double RebarRadiusX
        {
            get => _rebarRx;
            set
            {
                _rebarRx = value; OnPropertyChanged(nameof(RebarRadiusX));
            }
        }
        public double RebarRadiusY
        {
            get => _rebarRy;
            set
            {
                _rebarRy = value; OnPropertyChanged(nameof(RebarRadiusY));
            }
        }
        public double AreaOfRebars
        {
            get => _areaOfRebars;
            set
            {
                _areaOfRebars = value; OnPropertyChanged(nameof(AreaOfRebars));
            }
        }
        public double TotalArea
        {
            get => _totalAreaOfSection;
            set
            {
                _totalAreaOfSection = value; OnPropertyChanged(nameof(TotalArea));
            }
        }
        public double TotalIx
        {
            get => _totalIx;
            set
            {
                _totalIx = value; OnPropertyChanged(nameof(TotalIx));
            }
        }
        public double TotalIy
        {
            get => _totalIy;
            set
            {
                _totalIy = value; OnPropertyChanged(nameof(TotalIy));
            }
        }
        public double TotalRx
        {
            get => _totalRx;
            set
            {
                _totalRx = value; OnPropertyChanged(nameof(TotalRx));
            }
        }
        public double TotalRy
        {
            get => _totalRy;
            set
            {
                _totalRy = value; OnPropertyChanged(nameof(TotalRy));
            }
        }

        // Canvas
        public ObservableCollection<Shape> RebarsForCanvas
        {
            get => _rebarsForCanvas;
            private set
            {
                _rebarsForCanvas = value;
                OnPropertyChanged(nameof(Rebars));
            }
        }
        private void DrawRebars()
        {
            // Clear existing elements in RebarsForCanvas
            RebarsForCanvas.Clear();

            double canvasSize = 350;
            double canvasCenter = canvasSize / 2; 
            double scale = 1;

            if (SelectedSection == "Circular Column")
            {
                double columnDiameter = Diameter;

                if (columnDiameter > 300)
                {
                    scale = 300 / columnDiameter;
                }

                double margin = canvasCenter - Radius * scale;

                //Adding Circular Column
                Ellipse Circularcolumn = new Ellipse
                {
                    Width = Diameter * scale,
                    Height = Diameter * scale,
                    Stroke = Brushes.Green,
                    StrokeThickness = 2,
                    Fill = new SolidColorBrush(Color.FromArgb(73, 0, 180, 104)),
                    Margin = new Thickness(margin, margin, 0, 0)
                };

                RebarsForCanvas.Add(Circularcolumn);

                //Adding Rebars
                foreach (var item in Entries)
                {
                    int numberOfRebars = item.NumOfRebar;
                    double rebarRadius = item.RebarDia / 2;
                    double angleStep = 2 * Math.PI / numberOfRebars;

                    for (int i = 0; i < item.NumOfRebar; i++)
                    {
                        double angle = angleStep * i;
                        double rebarX = ((Radius - (Cover + item.DeltaY + SelectedStirrupThickness + item.RebarDia / 2)) * Math.Cos(angle)) * scale;
                        double rebarY = ((Radius - (Cover + item.DeltaY + SelectedStirrupThickness + item.RebarDia / 2)) * Math.Sin(angle)) * scale;

                        double margin_left = canvasCenter - (rebarRadius * scale) - rebarX;
                        double margin_top = canvasCenter - (rebarRadius * scale) - rebarY;

                        Ellipse rebar_columns = new Ellipse
                        {
                            Width = item.RebarDia * scale,
                            Height = item.RebarDia * scale,
                            Stroke = Brushes.Black,
                            StrokeThickness = 2,
                            Margin = new Thickness(margin_left, margin_top, 0, 0)
                        };

                        RebarsForCanvas.Add(rebar_columns);
                    }

                    //Adding Stirrups

                    double StirrupRadius = (Radius - (Cover + item.DeltaY));
                    double StirrupDia = StirrupRadius * 2;

                    double stirrupMargin = canvasCenter - StirrupRadius * scale;

                    Ellipse Stirrup = new Ellipse
                    {
                        Width = StirrupDia * scale,
                        Height = StirrupDia * scale,
                        Stroke = Brushes.DarkRed,
                        StrokeThickness = SelectedStirrupThickness * scale,
                        Margin = new Thickness(stirrupMargin, stirrupMargin, 0, 0)
                    };

                    RebarsForCanvas.Add(Stirrup);
                }
            }

            else
            {
                double columnBreadth = Breadth;
                double columnHeight = Height;


                if (columnBreadth > columnHeight && columnBreadth > 300)
                {
                    scale = 300 / columnBreadth;
                }

                else if (columnBreadth < columnHeight && columnBreadth > 300)
                {
                    scale = 300 / columnHeight;
                }

                double margin_left = canvasCenter - (columnBreadth/2) * scale;
                double margin_top = canvasCenter - (columnHeight / 2) * scale;

                Rectangle RectangularColumn = new Rectangle
                {
                    Width = columnBreadth * scale,
                    Height = columnHeight *scale,
                    Stroke = Brushes.Green,
                    StrokeThickness = 2,
                    Fill = new SolidColorBrush(Color.FromArgb(73, 0, 180, 104)),
                    Margin = new Thickness(margin_left, margin_top, 0, 0)
                };

                RebarsForCanvas.Add(RectangularColumn);

                //Starting corner of rectangle column
                double corner_x = margin_left;
                double corner_y = margin_top;

                //Adding Rebars
                foreach (var item in Entries)
                {
                    int numberOfRebars = item.NumOfRebar;
                    double rebarRadius = item.RebarDia / 2;
                    double spaceForRebarPlacement = Breadth - 2 * (SideCover + SelectedStirrupThickness);
                    double spacing = (spaceForRebarPlacement - 2 * rebarRadius) / (item.NumOfRebar-1); 

                    for (int i = 0; i < item.NumOfRebar; i++)
                    {
                        double rebarX = (SideCover + SelectedStirrupThickness + (i * spacing)) * scale;
                        double rebarY = (Cover + SelectedStirrupThickness + item.DeltaY) * scale;

                        double margin_left_rebar = corner_x + rebarX;
                        double margin_top_rebar = corner_y + rebarY;

                        Ellipse rebarColumn = new Ellipse
                        {
                            Width = item.RebarDia * scale,
                            Height = item.RebarDia * scale,
                            Stroke = Brushes.Black,
                            StrokeThickness = 2,
                            Margin = new Thickness(margin_left_rebar, margin_top_rebar, 0, 0)
                        };

                        RebarsForCanvas.Add(rebarColumn);
                    }

                    //Adding Stirrups

                    double StirrupHeight = Height - 2 * Cover;
                    double StirrupWidth = Breadth - 2 * SideCover ;

                    double stirrupXMargin = corner_x + SideCover * scale;
                    double stirrupYMargin = corner_y + Cover * scale;

                    Rectangle Stirrup = new Rectangle
                    {
                        Width = StirrupWidth * scale,
                        Height = StirrupHeight * scale,
                        Stroke = Brushes.DarkRed,
                        StrokeThickness = SelectedStirrupThickness * scale,
                        Margin = new Thickness(stirrupXMargin, stirrupYMargin, 0, 0)
                    };

                    RebarsForCanvas.Add(Stirrup);
                }

            }
        }

        // Public methods
        public void CalculateInertia_Execute()
        {
            if (!isRectangularSection)
            {
                CircularSections c = new CircularSections();
                double[] inertia = c.RebarInertiaCal(Radius, SelectedStirrupThickness, Entries, Cover);

                if (inertia[0] == -1)
                { 
                    errorList[10] = "Rebars are overlapping. Please check";
                }

                else
                {
                    RebarIx = inertia[0];
                    RebarIy = inertia[1];

                    AreaOfRebars = inertia[2];

                    RebarRadiusX = Math.Round(Math.Sqrt(RebarIx / AreaOfRebars), 6);
                    RebarRadiusY = Math.Round(Math.Sqrt(RebarIy / AreaOfRebars), 6);

                    double[] total = c.TotalInertiaCal(Diameter);
                    TotalIx = TotalIy = total[0];

                    TotalArea = PI * Math.Pow(Radius, 2);

                    TotalRx = Math.Sqrt(TotalIx / TotalArea);
                    TotalRy = Math.Sqrt(TotalIy / TotalArea); 

                    errorList[10] = "String.Empty";
                    DrawRebars();
                }

            }

            else
            {
                RectangularSections r = new RectangularSections();

                double[] inertia = r.RebarInertiaCal(Breadth, Height, SelectedStirrupThickness, Entries, Cover, SideCover);

                if (inertia[0] == -1)
                {
                    errorList[10] = "Rebars are overlapping. Please check";
                }

                else
                {
                    RebarIx = inertia[0];
                    RebarIy = inertia[1];

                    AreaOfRebars = inertia[2];

                    RebarRadiusX = Math.Round(Math.Sqrt(RebarIx / AreaOfRebars), 6);
                    RebarRadiusY = Math.Round(Math.Sqrt(RebarIy / AreaOfRebars), 6);

                    double[] total = r.TotalInertiaCal(Breadth, Height);
                    TotalIx = TotalIy = total[0];

                    TotalArea = Breadth * Height;

                    TotalRx = Math.Sqrt(TotalIx / TotalArea);
                    TotalRy = Math.Sqrt(TotalIy / TotalArea);

                    errorList[10] = String.Empty;
                    DrawRebars();
                }

            }
        }
        public bool CalculateInertia_CanExecute()
        {
            if (Entries.Count != 0)
            {
                foreach (bool item in HasErrors)
                {
                    if (item)
                    { calCanExecute = false; return false; }
                }

                calCanExecute = true;
                return true;
            }

            calCanExecute = false;
            return false;
            
        }
        public void ClearContentUponSelection()
        {
            RebarIx = RebarIy = RebarRadiusX = RebarRadiusY = Radius = Diameter = 0;
            Height = Breadth = Cover = SideCover = AreaOfRebars = 0;
            TotalArea = TotalIx = TotalIy = TotalRx = TotalRy = 0;
            Entries = new ObservableCollection<Rebars>();
            var initial = new Rebars();
            initial.RowCount = 1;
            initial.GetMinimumDimension = GetMinimumDimension;
            initial.GetHeightDimension = GetHeightDimension;
            Entries.Add(initial);
            Entries.CollectionChanged += OnEntriesCollectionChanged;
            HasErrors = new bool[10]; 
            RebarsForCanvas = new ObservableCollection<Shape>();
            errorList = new string[11];
        }

        #endregion

        #region Constructors
        public WindowViewModel(Window window)
        {
            mWindow = window;

            MinimizeCommand = new RelayCommand(() => mWindow.WindowState = WindowState.Minimized);
            MaximizeCommand = new RelayCommand(() =>
            {
                if (mWindow.WindowState != WindowState.Maximized)
                {
                    Image = "Images/norm.jpg";
                    mWindow.WindowState = WindowState.Maximized;
                }
                else
                {
                    Image = "Images/full.jpg";
                    mWindow.WindowState = WindowState.Normal;
                }
            });
            CloseCommand = new RelayCommand(() => mWindow.Close());

            CalculateInertia = new RelayCommand(() => CalculateInertia_Execute(), CalculateInertia_CanExecute);

            SectionTypeOptions = new ObservableCollection<string>
            {
                "Circular Column",
                "Rectangular Beam",
                "Rectangular Column"
            };

            StirrupThicknessOptions = new ObservableCollection<double>
            {
                4,
                6,
                8,
                10,
                12
            };

            ClearAll = new RelayCommand(() =>
            {
                ClearContentUponSelection();
            });

            Entries = new ObservableCollection<Rebars>();
            RebarsForCanvas = new ObservableCollection<Shape>();
            Entries.CollectionChanged += OnEntriesCollectionChanged;
            errorList = new string[11];
            for (int i = 0; i < 11; i++)
            {
                errorList[i]="";
            }
        }

        public WindowViewModel()
        {
        }

        #endregion

        #region Validation

        public override string Error
        {
            get { return String.Empty; } // return the error message
        }

        public override string this[string propertyName]
        {
            get
            {
                return propertyName switch
                {
                    nameof(Radius) => ValidateRadius(),
                    nameof(Diameter) => ValidateDiameter(),
                    nameof(Height) => ValidateHeight(),
                    nameof(Breadth) => ValidateBreadth(),
                    nameof(Cover) => ValidateCover(),
                    nameof(SideCover) => ValidateSideCover(),
                    _ => String.Empty
                };
            }

        }

        #endregion

        #region Public Helpers
        public string ValidateCover()
        {
            if (!isRectangularSection)
            {

                if (Cover < 0) { HasErrors[0] = true; errorList[0] = "Positive value only."; return errorList[0]; }
                else if (Cover >= Radius) { HasErrors[0] = true; errorList[0] = "Cover can't be greater than or equal to radius"; return errorList[0]; }
                else { HasErrors[0] = false; errorList[0] = String.Empty; return errorList[0];  }
            }
            else
            {
                if (Cover < 0) { HasErrors[0] = true; errorList[0] =  "Positive value only."; return errorList[0]; }
                else if (Cover >= Height) { HasErrors[0] = true; errorList[0] = "Cover can't be greater than or equal to height"; return errorList[0]; }
                else { HasErrors[0] = false; errorList[0] = String.Empty; return errorList[0]; }
            }
        }
        public string ValidateSideCover()
        {
            if (!isRectangularSection) { HasErrors[1] = false; errorList[1] = String.Empty; return errorList[1]; }
            else
            {
                if (SideCover >= Breadth) { HasErrors[1] = true; errorList[1] = "Side Cover can't be greater than or equal to width"; return errorList[1]; }
                if (SideCover <= 0) { HasErrors[1] = true; errorList[1] = "Non-zero positive value only."; return errorList[1]; }
                else { HasErrors[1] = false; errorList[1] = String.Empty; return errorList[1]; }
            }
        }
        public string ValidateHeight()
        {
            if (!isRectangularSection) { HasErrors[1] = false; errorList[1] = String.Empty; return errorList[1]; }
            else
            {
                if (Height <= 0) { HasErrors[2] = true; errorList[2] = "Non-zero positive value only."; return errorList[2]; }
                else
                {
                    HasErrors[2] = false; errorList[2] = String.Empty;
                    HasErrors[4] = false; errorList[4] = String.Empty; //radius
                    HasErrors[5] = false; errorList[5] = String.Empty; //diameter
                    return String.Empty;
                }
            }
        }
        public string ValidateBreadth()
        {
            if (!isRectangularSection) { HasErrors[1] = false; errorList[1] = String.Empty; return errorList[1]; }
            else
            {
                if (Breadth <= 0) { HasErrors[3] = true; errorList[3] = "Non-zero positive value only."; return errorList[3]; }
                else { HasErrors[3] = false; return String.Empty; }
            }
        }
        public string ValidateRadius()
        {
            if (isRectangularSection) { HasErrors[1] = false; errorList[1] = String.Empty; return errorList[1]; }
            else
            {
                if (Radius <= 0) { HasErrors[4] = true; errorList[4] = "Non-zero positive value only."; return errorList[4]; }
                else
                {
                    HasErrors[4] = false; errorList[4] = String.Empty;
                    HasErrors[1] = false; errorList[1] = String.Empty; // Side Cover
                    HasErrors[3] = false; errorList[3] = String.Empty; // Breadth
                    HasErrors[2] = false; errorList[2] = String.Empty; // Height
                    return String.Empty;
                }
            }
        }
        public string ValidateDiameter()
        {
            if (isRectangularSection) { HasErrors[1] = false; errorList[1] = String.Empty; return errorList[1]; }
            else
            {
                if (Diameter <= 0) { HasErrors[5] = true; errorList[5] = "Non-zero positive value only."; return errorList[5]; }
                else { HasErrors[5] = false; errorList[5] = String.Empty; return errorList[5]; }
            }
        }
        #endregion

        #region Private Helpers

        private double GetMinimumDimension()
        {
            if (isRectangularSection) return Math.Min(Breadth, Height);
            else return Radius;
        }
        private double GetHeightDimension()
        {
            return Height;
        }

        private void OnEntriesCollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (var item in Entries)
            {
                item.RowCount = Entries.IndexOf(item) + 1;
                item.GetMinimumDimension = GetMinimumDimension;
                item.GetHeightDimension = GetHeightDimension;
            }
        }
        #endregion

    }

    public class Rebars : WindowViewModel
    {

        #region Private members
        private double _dia;
        private int _num;
        private double _delta;
        private int _count;
        #endregion

        #region Public Properties
        public double RebarDia
        {
            get { return _dia; }
            set { _dia = value; }
        }
        public int NumOfRebar
        {
            get { return _num; }
            set { _num = value; }
        }
        public int RowCount
        {
            get => _count;
            set
            {
                _count = value;
                OnPropertyChanged(nameof(RowCount));
            }
        }
        public double DeltaY
        {
            get { return _delta; }
            set
            {
                if (RowCount == 1) { _delta = 0; }
                else { _delta = value; }
            }
        }

        #endregion

        #region Validation
        override public string Error
        {
            get { return String.Empty; }
        }

        public override string this[string propName]
        {
            get
            {
                var minValue = GetMinimumDimension?.Invoke() ?? 0;
                var height_RectangularColumn = GetHeightDimension?.Invoke() ?? 0;
                if (propName == "NumOfRebar")
                {
                    if (NumOfRebar <= 0) { HasErrors[6] = true; errorList[6] = "Non-zero positive value only."; return errorList[6]; }
                    else if (NumOfRebar < 2) { HasErrors[6] = true; errorList[6] = "Number of rebars must be a minimum of 2"; return errorList[6]; }
                    else { HasErrors[6] = false; errorList[6] = String.Empty; return errorList[6]; }
                }

                if (propName == "DeltaY")
                {
                    if (!isRectangularSection)
                    {
                        if (DeltaY < 0) { HasErrors[7] = true; errorList[7] = "Positive value only."; return errorList[7]; }

                        if (DeltaY >= minValue) { HasErrors[7] = true; errorList[7] = "Delta Y can't be greater or equal to radius of column"; return errorList[7]; }
                        else { HasErrors[7] = false; errorList[7] = String.Empty; return errorList[7]; }
                    }
                    else 
                    {
                        if (DeltaY < 0) { HasErrors[7] = true; errorList[7] = "Positive value only."; return errorList[7]; }

                        if (DeltaY >= height_RectangularColumn) { HasErrors[7] = true; errorList[7] = "Delta Y can't be greater or equal to height"; return errorList[7]; }
                        else { HasErrors[7] = false; errorList[7] = String.Empty; return errorList[7]; }
                    }
                }

                if (propName == "RebarDia")
                {
                    if (!isRectangularSection)
                    {
                        if (RebarDia <= 0) { HasErrors[8] = true; errorList[8] = "Non-zero positive value only."; return errorList[8]; }
                        else if (RebarDia > minValue) { HasErrors[8] = true; errorList[8] = "Diamter of rebar can't be greater or equal to radius of column"; return errorList[8]; }
                        else { HasErrors[8] = false; errorList[8] = String.Empty; return errorList[8];}
                    }
                    else
                    {
                        if (RebarDia <= 0) { HasErrors[9] = true; errorList[9] = "Non-zero positive value only."; return errorList[9]; }
                        else if (RebarDia > minValue) { HasErrors[9] = true; errorList[9] = "Diamter of rebar can't be greater or equal to boundary of column"; return errorList[9]; }
                        else { HasErrors[9] = false; errorList[9] = String.Empty; return errorList[8]; }
                    }
                }

                return String.Empty;
            }
        }
        #endregion

        public Func<double> GetMinimumDimension { get; set; }
        public Func<double> GetHeightDimension { get; set; }
        public Rebars()
        {
        }
    }


}
