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
        private ObservableCollection<Ellipse> _rebarsForCanvas;

        private ObservableCollection<Rebars>? _userint;
        private ObservableCollection<string> _sectionTypeOptions; 
        private ObservableCollection<double> _stirrupThicknessOptions;
        private string _selectedSectionType;
        private double _selectedStirrupThickness;

        private string _errorMessage;

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

        // Error Message Display
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

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
        public ObservableCollection<Ellipse> RebarsForCanvas
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
            double columnDiameter = Diameter;
            double scale = 1;

            if (columnDiameter > 300)
            {
                scale =  300 / columnDiameter;
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

        // Public methods
        public void CalculateInertia_Execute()
        {
            if (!isRectangularSection)
            {
                CircularSections c = new CircularSections();
                double[] inertia = c.RebarInertiaCal(Radius, SelectedStirrupThickness, Entries, Cover);

                if (inertia[0] == -1)
                {
                    ErrorMessage = "Rebars are overlapping. Please check";
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
                    ErrorMessage = "";

                    DrawRebars();
                }

            }

            else
            {
                RectangularSections r = new RectangularSections();

                double[] inertia = r.RebarInertiaCal(Breadth, Height, SelectedStirrupThickness, Entries, Cover, SideCover);

                if (inertia[0] == -1)
                {
                    ErrorMessage = "Rebars are overlapping. Please check";
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

                    ErrorMessage = "";
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
            Entries.Add(initial);
            Entries.CollectionChanged += OnEntriesCollectionChanged;
            HasErrors = new bool[10];
            ErrorMessage = "";
            RebarsForCanvas = new ObservableCollection<Ellipse>();
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
            RebarsForCanvas = new ObservableCollection<Ellipse>();
            Entries.CollectionChanged += OnEntriesCollectionChanged;
        }

        public WindowViewModel()
        {
        }

        #endregion

        #region Validation

        public override string Error
        {
            get { return null; } // return the error message
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
                    _ => null
                };
            }

        }

        #endregion

        #region Public Helpers
        public string ValidateCover()
        {
            if (!isRectangularSection)
            {

                if (Cover < 0) { HasErrors[0] = true; return "Positive value only."; }
                else if (Cover >= Radius) { HasErrors[0] = true; return "Cover can't be greater than or equal to radius"; }
                else { HasErrors[0] = false; return null; }
            }
            else
            {
                if (Cover < 0) { HasErrors[0] = true; return "Positive value only."; }
                else if (Cover >= Height) { HasErrors[0] = true; return "Cover can't be greater than or equal to height"; }
                else { HasErrors[0] = false; return null; }
            }
        }
        public string ValidateSideCover()
        {
            if (!isRectangularSection) { HasErrors[1] = false; return null; }
            else
            {
                if (SideCover >= Breadth) { HasErrors[1] = true; return "Side Cover can't be greater than or equal to width"; }
                if (SideCover <= 0) { HasErrors[1] = true; return "Non-zero positive value only."; }
                else { HasErrors[1] = false; return null; }
            }
        }
        public string ValidateHeight()
        {
            if (!isRectangularSection) { HasErrors[1] = false; return null; }
            else
            {
                if (Height <= 0) { HasErrors[2] = true; return "Non-zero positive value only."; }
                else
                {
                    HasErrors[2] = false;
                    HasErrors[4] = false; //radius
                    HasErrors[5] = false; //diameter
                    return null;
                }
            }
        }
        public string ValidateBreadth()
        {
            if (!isRectangularSection) { HasErrors[1] = false; return null; }
            else
            {
                if (Breadth <= 0) { HasErrors[3] = true; return "Non-zero positive value only."; }
                else { HasErrors[3] = false; return null; }
            }
        }
        public string ValidateRadius()
        {
            if (isRectangularSection) { HasErrors[1] = false; return null; }
            else
            {
                if (Radius <= 0) { HasErrors[4] = true; return "Non-zero positive value only."; }
                else
                {
                    HasErrors[4] = false;
                    HasErrors[1] = false; // Side Cover
                    HasErrors[3] = false; // Breadth
                    HasErrors[2] = false; // Height
                    return null;
                }
            }
        }
        public string ValidateDiameter()
        {
            if (isRectangularSection) { HasErrors[1] = false; return null; }
            else
            {
                if (Diameter <= 0) { HasErrors[5] = true; return "Non-zero positive value only."; }
                else { HasErrors[5] = false; return null; }
            }
        }
        #endregion

        #region Private Helpers

        private double GetMinimumDimension()
        {
            if (isRectangularSection) return Math.Min(Breadth, Height);
            else return Radius;
        }
        private void OnEntriesCollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (var item in Entries)
            {
                item.RowCount = Entries.IndexOf(item) + 1;
                item.GetMinimumDimension = GetMinimumDimension;
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
            get { return null; }
        }

        public override string this[string propName]
        {
            get
            {
                var minValue = GetMinimumDimension?.Invoke() ?? 0;
                if (propName == "NumOfRebar")
                {
                    if (NumOfRebar <= 0) { HasErrors[6] = true; return "Non-zero positive value only."; }
                    else if (NumOfRebar < 2) { HasErrors[6] = true; return "Number of rebars must be a minimum of 2"; }
                    else { HasErrors[6] = false; return null; }
                }

                if (propName == "DeltaY")
                {
                    if (DeltaY < 0) { HasErrors[7] = true; return "Positive value only."; }
                    else if (DeltaY >= minValue) { HasErrors[7] = true; return "Delta Y can't be greater or equal to radius of column"; }
                    else { HasErrors[7] = false; return null; }
                }

                if (propName == "RebarDia")
                {
                    if (!isRectangularSection)
                    {
                        if (RebarDia <= 0) { HasErrors[8] = true; return "Non-zero positive value only."; }
                        else if (RebarDia > minValue) { HasErrors[8] = true; return "Diamter of rebar can't be greater or equal to radius of column"; }
                        else { HasErrors[8] = false; return null; }
                    }
                    else
                    {
                        if (RebarDia <= 0) { HasErrors[9] = true; return "Non-zero positive value only."; }
                        else if (RebarDia > minValue) { HasErrors[9] = true; return "Diamter of rebar can't be greater or equal to boundary of column"; }
                        else { HasErrors[9] = false; return null; }
                    }
                }

                return null;
            }
        }
        #endregion

        public Func<double> GetMinimumDimension { get; set; }
        public Rebars()
        {
        }
    }


}
