using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApp1
{
    public class WindowViewModel : BaseViewModel
    {
        #region Private Properties
        private readonly string rebarInfoEmpty = "Fill in rebar details";

        private ObservableCollection<Rebars>? _userRebarInput;

        private readonly List<string> _sectionTypeOptions = ["Circular Column", "Rectangular Beam", "Rectangular Column"];
        private string _selectedSectionType;

        private List<double> _stirrupThicknessOptions = [8, 10, 12, 16, 18, 20];
        private double _selectedStirrupThickness;

        private ObservableCollection<string> _errorList = new ObservableCollection<string>();

        private ObservableCollection<Shape> _rebarsForCanvas;
        private static bool _isRectangularSection = true;
        private bool _displayRectangleGuideIllustration = true;
        private bool _displaySmallRectangleGuide = false;
        private bool _displayCircleGuideIllustration = false;
        private bool _displaySmallCircleGuide = false;

        private string _image = "Images/full.jpg";
        private double _radius;
        private double _diameter;
        private double _breadth;
        private double _height;
        private double _sidecover;
        private static double _cover = 0;

        private static double _maxDeltaYValue;
        private static string _maxDeltaYString = string.Empty;

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

        //Obs Collection for Combobox and Radio Button options
        public List<string> SectionTypeOptions
        {
            get { return _sectionTypeOptions; }
        }
        public List<double> StirrupThicknessOptions
        {
            get { return _stirrupThicknessOptions; }
        }

        //Commands
        public ICommand CalculateInertia { get; set; }
        public ICommand ClearAll { get; set; }

        #region User Inputs
        // User Inputs
        public double Radius
        {
            get => _radius;
            set
            {
                _radius = value; OnPropertyChanged(nameof(Radius));
                Diameter = _radius * 2; OnPropertyChanged(nameof(Diameter));
            }
        }
        public double Diameter
        {
            get => _diameter;
            set
            {
                _diameter = value; OnPropertyChanged(nameof(Diameter));
                _radius = _diameter / 2; OnPropertyChanged(nameof(Radius));
            }
        }
        public double Height
        {
            get => _height;
            set
            {
                _height = value; OnPropertyChanged(nameof(Height));
            }
        }
        public double Breadth
        {
            get => _breadth;
            set
            {
                _breadth = value; OnPropertyChanged(nameof(Breadth));
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
                _selectedSectionType = value;
                switch (_selectedSectionType)
                {
                    case ("Rectangular Column"):
                    case ("Rectangular Beam"):
                        {
                            isRectangularSection = true;
                            DisplayRectangleGuideIllustration = true;
                            DisplayCircleGuideIllustration = false;
                            DisplaySmallRectangleGuide = false;
                            DisplaySmallCircleGuide = false;
                            ClearContentUponSelection();
                            GetDefaultValuesRectangle();

                            break;
                        }
                    case ("Circular Column"):
                        {
                            isRectangularSection = false;
                            DisplayRectangleGuideIllustration = false;
                            DisplayCircleGuideIllustration = true;
                            DisplaySmallRectangleGuide = false;
                            DisplaySmallCircleGuide = false;
                            ClearContentUponSelection();
                            GetDefaultValuesCircle();

                            break;
                        }
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
        #endregion

        // Obs Collection for Datagrid
        public ObservableCollection<Rebars>? UserRebarEntries
        {
            get { return _userRebarInput; }
            set
            {
                _userRebarInput = value;
                OnPropertyChanged(nameof(UserRebarEntries));

            }
        }

        // List of Validation Errors + Error List
        private Visibility _errorsVisibility = Visibility.Visible;
        public Visibility ErrorsVisibility
        {
            get => _errorsVisibility;
            set
            {
                _errorsVisibility = value;
                OnPropertyChanged(nameof(ErrorsVisibility));
            }
        }
        public ObservableCollection<string> ErrorList
        {
            get { return _errorList; }
            set
            {
                _errorList = value;
                OnPropertyChanged(nameof(ErrorList));
            }
        }
        private void UpdateErrorListBasedOnRebars(string error, bool add = true)
        {

            if (add)
            {
                if (!(ErrorList.Contains(error)))
                    ErrorList.Add(error);
            }

            else
            {
                if (ErrorList.FirstOrDefault(e => e.Equals(error)) is string error1)
                    ErrorList.Remove(error1);
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
        public bool CalCanExecute
        {
            get => _calcCanExecute;
            set
            {
                _calcCanExecute = value;
                OnPropertyChanged(nameof(CalCanExecute));
            }
        }
        public double MaxDeltaYValue
        {
            get => _maxDeltaYValue;
            set
            { _maxDeltaYValue = value; OnPropertyChanged(nameof(MaxDeltaYValue)); }
        }
        public string MaxDeltaYMessage
        {
            get => _maxDeltaYString;
            set
            {
                _maxDeltaYString = value; OnPropertyChanged(nameof(MaxDeltaYMessage));
            }
        }

        #region Guide Images
        // Guide Images
        public bool DisplayRectangleGuideIllustration
        {
            get { return _displayRectangleGuideIllustration; }
            set
            {
                _displayRectangleGuideIllustration = value;
                OnPropertyChanged(nameof(DisplayRectangleGuideIllustration));
            }
        }

        public bool DisplayCircleGuideIllustration
        {
            get { return _displayCircleGuideIllustration; }
            set
            {
                _displayCircleGuideIllustration = value;
                OnPropertyChanged(nameof(DisplayCircleGuideIllustration));
            }
        }

        public bool DisplaySmallCircleGuide
        {
            get { return _displaySmallCircleGuide; }
            set
            {
                _displaySmallCircleGuide = value;
                OnPropertyChanged(nameof(DisplaySmallCircleGuide));
            }
        }

        public bool DisplaySmallRectangleGuide
        {
            get { return _displaySmallRectangleGuide; }
            set
            {
                _displaySmallRectangleGuide = value;
                OnPropertyChanged(nameof(DisplaySmallRectangleGuide));
            }
        }

        #endregion

        #region Calculated Values
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
        #endregion

        #region Dynamic Canvas
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
                DisplayCircleGuideIllustration = false;
                DisplaySmallCircleGuide = true;

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
                foreach (var item in UserRebarEntries)
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
                DisplayRectangleGuideIllustration = false;
                DisplaySmallRectangleGuide = true;
                double columnBreadth = Breadth;
                double columnHeight = Height;


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

                Rectangle RectangularColumn = new Rectangle
                {
                    Width = columnBreadth * scale,
                    Height = columnHeight * scale,
                    Stroke = Brushes.Green,
                    StrokeThickness = 2,
                    Fill = new SolidColorBrush(Color.FromArgb(73, 0, 180, 104)),
                    Margin = new Thickness(margin_left, margin_top, 0, 0)
                };

                RebarsForCanvas.Add(RectangularColumn);

                //Starting corner of rectangle column
                double corner_x = margin_left;
                double corner_y = margin_top;

                //Adding Rebars + Stirrups
                foreach (var item in UserRebarEntries)
                {
                    int numberOfRebars = item.NumOfRebar;
                    double rebarRadius = item.RebarDia / 2;
                    double spaceForRebarPlacement = Breadth - 2 * (SideCover + SelectedStirrupThickness);
                    double spacing = (spaceForRebarPlacement - 2 * rebarRadius) / (item.NumOfRebar - 1);

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
                    double StirrupWidth = Breadth - 2 * SideCover;

                    double stirrupXMargin = corner_x + SideCover * scale;
                    double stirrupYMargin = corner_y + Cover * scale;

                    Rectangle Stirrup = new Rectangle
                    {
                        Width = StirrupWidth * scale,
                        Height = StirrupHeight * scale,
                        Stroke = Brushes.DarkRed,
                        StrokeThickness = SelectedStirrupThickness * scale,
                        Margin = new Thickness(stirrupXMargin, stirrupYMargin, 0, 0),
                        RadiusX = rebarRadius * scale,
                        RadiusY = rebarRadius * scale

                    };


                    RebarsForCanvas.Add(Stirrup);
                }

            }
        }

        #endregion

        // Public methods
        public void CalculateInertia_Execute()
        {
            string rebarOverlapError = "Rebars are overlapping. Please check";
            double[] inertia = [];
            double[] total = [];

            // Max Delta Y Value
            foreach (var item in UserRebarEntries)
            {
                if (item.RowCount == 1)
                {
                    var maxValue = isRectangularSection ? Height : Radius;

                    if (isRectangularSection)
                        maxValue -= (2 * Cover + item.RebarDia + 2 * SelectedStirrupThickness);
                    else
                        maxValue -= (Cover + item.RebarDia + SelectedStirrupThickness);

                    MaxDeltaYValue = maxValue;
                    SetMaxDeltaYMessage(item.RebarDia);

                    break;
                }
            }

            if (!isRectangularSection)
            {
                CircularSections c = new CircularSections();
                inertia = c.RebarInertiaCal(Radius, SelectedStirrupThickness, UserRebarEntries, Cover);
                total = c.TotalInertiaCal(Diameter);
                TotalArea = Math.Round(PI * Math.Pow(Radius, 2),1);
            }

            else
            {
                RectangularSections r = new RectangularSections();
                inertia = r.RebarInertiaCal(Breadth, Height, SelectedStirrupThickness, UserRebarEntries, Cover, SideCover);
                total = r.TotalInertiaCal(Breadth, Height);
                TotalArea = Math.Round(Height * Breadth,1);
            }

            if (inertia[0] == -1)
            {
                UpdateErrorListBasedOnRebars(rebarOverlapError, true);
                return;
            }

            RebarIx = Math.Round(inertia[0],1);
            RebarIy = Math.Round(inertia[1],1);
            AreaOfRebars = Math.Round(inertia[2],1);

            RebarRadiusX = Math.Round(Math.Sqrt(RebarIx / AreaOfRebars), 1);
            RebarRadiusY = Math.Round(Math.Sqrt(RebarIy / AreaOfRebars), 1);

            
            TotalIx = TotalIy = Math.Round(total[0],1);
            
            TotalRx = Math.Round(Math.Sqrt(TotalIx / TotalArea),1);
            TotalRy = Math.Round(Math.Sqrt(TotalIy / TotalArea),1);

            UpdateErrorListBasedOnRebars(rebarOverlapError, false);

            DrawRebars();
            
        }
        public bool CalculateInertia_CanExecute()
        {
            if (ErrorsVisibility == Visibility.Visible && !(ErrorList.Contains("Rebars are overlapping. Please check")))
            {
                CalCanExecute = false;
                return false;
            }

            if (UserRebarEntries.Count == 0)
            {
                UpdateErrorListBasedOnRebars(rebarInfoEmpty, true);
                CalCanExecute = false;
                return false;
            }

            CalCanExecute = true;
            return true;
        }
        public void ClearContentUponSelection()
        {
            ErrorList = new ObservableCollection<string>();
            ErrorList.CollectionChanged += OnErrorListCollectionChanged;

            RebarIx = RebarIy = RebarRadiusX = RebarRadiusY = Radius = Diameter = 0;
            Height = Breadth = Cover = SideCover = AreaOfRebars = 0;
            TotalArea = TotalIx = TotalIy = TotalRx = TotalRy = 0;

            UserRebarEntries = new ObservableCollection<Rebars>();
            var initial = new Rebars();
            initial.RowCount = 1;
            initial.SetCallBcks(GetMinimumDimension, GetHeightDimension, GetIfRectangularSection, GetCover, GetStirrupDiameter, UpdateErrorListBasedOnRebars);
            UserRebarEntries.Add(initial);
            UserRebarEntries.CollectionChanged += OnUserRebarEntriesCollectionChanged;
            RebarsForCanvas = new ObservableCollection<Shape>();

            MaxDeltaYMessage = string.Empty;

            if (isRectangularSection)
            {
                ValidateBreadth(); ValidateHeight(); ValidateCover(); ValidateSideCover(); 
                DisplayRectangleGuideIllustration = true;
                DisplayCircleGuideIllustration = false;
                DisplaySmallRectangleGuide = false;
                DisplaySmallCircleGuide = false;
            }
            else
            {
                ValidateRadius(); ValidateDiameter(); ValidateCover();
                DisplayRectangleGuideIllustration = false;
                DisplayCircleGuideIllustration = true;
                DisplaySmallRectangleGuide = false;
                DisplaySmallCircleGuide = false;
            }
        }

        #endregion

        #region Constructor
        public WindowViewModel(Window window)
        {
            CalculateInertia = new RelayCommand(() => CalculateInertia_Execute(), CalculateInertia_CanExecute);

            MaxDeltaYMessage = string.Empty;

            ClearAll = new RelayCommand(() =>
            {
                ClearContentUponSelection();
            });

            ErrorList = new ObservableCollection<string>();
            ErrorList.CollectionChanged += OnErrorListCollectionChanged;

            UserRebarEntries = new ObservableCollection<Rebars>();
            UserRebarEntries.CollectionChanged += OnUserRebarEntriesCollectionChanged;

            RebarsForCanvas = new ObservableCollection<Shape>();
            
            SelectedStirrupThickness = StirrupThicknessOptions[0];
            SelectedSection = SectionTypeOptions[1];

        }

        #endregion

        #region Validation

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
        private string ValidateRadius()
        {
            if (isRectangularSection) { return string.Empty; }
            else
            {
                string nonPositiveError = "Radius should have a non-zero positive value.";

                if (Radius <= 0)
                {
                    UpdateErrorListBasedOnRebars(nonPositiveError, true);
                    return "Error";
                }
                else
                {
                    UpdateErrorListBasedOnRebars(nonPositiveError, false);
                    return String.Empty;
                }
            }
        }
        private string ValidateDiameter()
        {
            if (isRectangularSection) { return string.Empty; }
            else
            {
                string nonPositiveError = "Diameter should have a non-zero positive value.";

                if (Diameter <= 0)
                {
                    UpdateErrorListBasedOnRebars(nonPositiveError, true);
                    return "Error";
                }
                else
                {
                    UpdateErrorListBasedOnRebars(nonPositiveError, false);
                    return String.Empty;
                }
            }
        }
        private string ValidateHeight()
        {
            string nonPositiveError = "Height should have a non-zero positive value.";

            if (!isRectangularSection) { return string.Empty; }
            else
            {
                if (Height <= 0)
                {
                    UpdateErrorListBasedOnRebars(nonPositiveError, true);
                    return "Error";
                }
                else
                {
                    UpdateErrorListBasedOnRebars(nonPositiveError, false);

                    return String.Empty;
                }
            }
        }
        private string ValidateBreadth()
        {
            string nonPositiveError = "Breadth should have a non-zero positive value.";

            if (!isRectangularSection) { return string.Empty; }
            else
            {
                if (Breadth <= 0)
                {
                    UpdateErrorListBasedOnRebars(nonPositiveError, true);
                    return "Error";
                }
                else
                {
                    UpdateErrorListBasedOnRebars(nonPositiveError, false);
                    return String.Empty;
                }
            }
        }
        private string ValidateCover()
        {
            string dim = isRectangularSection ? "height" : "radius";
            double value = isRectangularSection ? Height : Radius;
            string exceediverError = $"Cover can't be greater than or equal to {dim}";
            string nonPositiveError = "Cover should have a positive value only.";

            if (Cover < 0)
            {
                UpdateErrorListBasedOnRebars(nonPositiveError, true);
                return "Error";
            }

            else if (Cover >= value)
            {
                UpdateErrorListBasedOnRebars(exceediverError, true);
                return "Error";
            }

            else
            {
                UpdateErrorListBasedOnRebars(nonPositiveError, false);
                UpdateErrorListBasedOnRebars(exceediverError, false);
                return string.Empty;
            }


        }
        private string ValidateSideCover()
        {
            string exceedBreadthError = "Side Cover can't be greater than or equal to Breadth";
            string nonPositiveError = "Side Cover should have a non-zero positive value.";

            if (!isRectangularSection) { return string.Empty; }
            else
            {
                if (SideCover <= 0)
                {
                    UpdateErrorListBasedOnRebars(nonPositiveError, true);
                    return "Error";
                }

                else if (SideCover >= Breadth)
                {
                    UpdateErrorListBasedOnRebars(exceedBreadthError, true);
                    return "Error";
                }

                else
                {
                    UpdateErrorListBasedOnRebars(nonPositiveError, false);
                    UpdateErrorListBasedOnRebars(exceedBreadthError, false);
                    return string.Empty;
                }
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
        private double GetStirrupDiameter()
        {
            return SelectedStirrupThickness;
        }
        private bool GetIfRectangularSection()
        {
            return isRectangularSection;
        }
        private double GetCover()
        {
            return Cover;
        }
        private void SetMaxDeltaYMessage(double rebarDia)
        {
            if (MaxDeltaYValue == 0)
                MaxDeltaYMessage = string.Empty;
            else
                MaxDeltaYMessage = $"Max Delta Y value: {MaxDeltaYValue}mm \nIf Rebars of Diameter {rebarDia}mm are used throughout";
        }
        private void OnUserRebarEntriesCollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateErrorListBasedOnRebars(rebarInfoEmpty, false);

            foreach (var item in UserRebarEntries)
            {
                item.RowCount = UserRebarEntries.IndexOf(item) + 1;
                item.SetCallBcks(GetMinimumDimension, GetHeightDimension, GetIfRectangularSection, GetCover, GetStirrupDiameter, UpdateErrorListBasedOnRebars);
            }
        }
        private void OnErrorListCollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (ErrorList.Any())
                ErrorsVisibility = Visibility.Visible;
            else
                ErrorsVisibility = Visibility.Collapsed;
        }
        private void GetDefaultValuesRectangle()
        {
            Height = 300;
            Breadth = 400;
            Cover = 20;
            SideCover = 30;
            SelectedStirrupThickness = StirrupThicknessOptions[0];

            UserRebarEntries.RemoveAt(0);
            UserRebarEntries.Add(new Rebars(20, 2, 0));
            UserRebarEntries.Add(new Rebars(20, 2, 224));

            CalculateInertia_Execute();
        }

        private void GetDefaultValuesCircle()
        {
            Radius = 100;
            Diameter = 200;
            Cover = 10;
            SelectedStirrupThickness = StirrupThicknessOptions[0];

            UserRebarEntries.RemoveAt(0);
            UserRebarEntries.Add(new Rebars(10, 4, 0));

            CalculateInertia_Execute();
        }

        #endregion

    }
}
