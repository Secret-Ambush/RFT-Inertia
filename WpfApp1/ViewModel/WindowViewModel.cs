using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Xml.Linq;

namespace WpfApp1
{
    public class WindowViewModel : BaseViewModel
    {
        #region Private Properties

        private ObservableCollection<Rebars>? _userint;
        private Window mWindow;
        private static bool _isRectangularSection = true;
        private ComboBoxItem _mySelectedItem;
        private ComboBoxItem _selectedCode;
        private string _image = "Images/full.jpg";
        private double _radius;
        private double _diameter;
        private double _sidecover;
        private double _height;
        private double _breadth;
        private double _spacing;
        private double _totalAreaOfSection;
        const double PI = 3.14f;

        private static double _areaOfRebars = 0;
        private static double _rebarIx = 0;
        private static double _rebarIy = 0;
        private static double _rebarRx = 0;
        private static double _rebarRy = 0;
        private static double _cover = 0;
        private static double _totalIx = 0;
        private static double _totalIy = 0;
        private static double _totalRx = 0;
        private static double _totalRy = 0;

        #endregion

        #region Public Properties/Methods

        public string Image
        {
            get => _image;
            set { _image = value; OnPropertyChanged(nameof(Image)); }
        }
        public ObservableCollection<Rebars>? Entries
        {
            get { return _userint; }
            set
            {
                _userint = value;
                OnPropertyChanged(nameof(Entries));
            }
        }
        public ICommand MinimizeCommand { get; set; }
        public ICommand MaximizeCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public ICommand CalculateInertia { get; set; }
        public ICommand ClearAll { get; set; }

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
        public bool isRectangularSection
        {
            get { return _isRectangularSection; }
            set { 
                _isRectangularSection = value; 
                OnPropertyChanged(nameof(_isRectangularSection));
            }
        }
        public ComboBoxItem MySelectedItem
        {
            get { return _mySelectedItem; }
            set
            {
                _mySelectedItem = value;
                switch (_mySelectedItem.Content.ToString())
                {
                    case ("Rectangular Column"):
                    case ("Rectangular Beam"):
                        { isRectangularSection = true; break; }
                    case ("Circular Column"):
                        { isRectangularSection = false; break; }
                }
            }
        }
        private void clearContents()
        {
            RebarIx = RebarIy = RebarRadiusX = RebarRadiusY = Radius = Diameter = TotalRx = TotalRy = 0;
            Height = Breadth = Cover = SideCover = AreaOfRebars = 0;
            Entries = new ObservableCollection<Rebars>();
            HasErrors = new bool[10];
        }
        public ComboBoxItem SelectedCode
        {
            get { return _selectedCode; }
            set
            {
                _mySelectedItem = value;
                switch (_selectedCode.Content.ToString())
                {
                    case ("some code 1"):
                        { Spacing = 5; break; }
                    case ("some code 2"):
                        { Spacing = 10; break; }
                }
            }
        }
        public double Radius
        {
            get => _radius;
            set
            {
                _radius = value; OnPropertyChanged(nameof(Radius));
                Diameter = _radius * 2; OnPropertyChanged(nameof(Diameter));
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
                _height = value; OnPropertyChanged(nameof(Height));
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
        public double Spacing
        {
            get => _spacing;
            set
            {
                _spacing = value; OnPropertyChanged(nameof(Spacing));
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

        public void CalculateInertia_Execute()
        {
            if (!isRectangularSection)
            {
                CircularSections c = new CircularSections();
                double[] inertia = c.RebarInertiaCal(Radius, Entries, Cover);

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
            }

            else
            {
                RectangularSections r = new RectangularSections();

                double[] inertia = r.RebarInertiaCal(Breadth, Height, Entries, Cover, SideCover);

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
            }
        }
        public bool CalculateInertia_CanExecute()
        {
            if (Entries.Count != 0)
            {
                foreach (bool item in HasErrors)
                {
                    if (item)
                    { return false; }
                }

                return true;
            }

            return false;
            
        }

        public static bool[] HasErrors = new bool[10];

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

            ClearAll = new RelayCommand(() =>
            {
                RebarIx = RebarIy = RebarRadiusX = RebarRadiusY = Radius = Diameter = TotalRx = TotalRy = 0;
                Height = Breadth = Cover = SideCover = AreaOfRebars = 0;
                Entries = new ObservableCollection<Rebars>();
                HasErrors = new bool[10];
            });

            Entries = new ObservableCollection<Rebars>();
            Entries.CollectionChanged += OnEntriesCollectionChanged;
        }

        private double GetMinimumDimension()
        {
            if (isRectangularSection) return Math.Min(Breadth, Height);
            else return Radius;
        }

        public WindowViewModel()
        {
        }

        #endregion

        #region Private Helpers
        private void OnEntriesCollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (var item in Entries)
            {
                item.RowCount = Entries.IndexOf(item) + 1;
                item.GetMinimumDimension = GetMinimumDimension;
            }
        }

        #endregion


        public override string Error
        {
            get { return null; } // return the error message
        }

        public override string this[string propertyName]
        {
            get
            {
                if (propertyName == "Cover")
                {

                    if (!isRectangularSection)
                    {
                        
                        if (Cover < 0) { HasErrors[0] = true; return "Positive value only."; }
                        else if (Cover >= Radius) { HasErrors[0] = true; return "Cover can't be greater than or equal to radius"; }
                        else { HasErrors[0] = false; return null;  }
                    }
                    else
                    {
                        if (Cover < 0) { HasErrors[0] = true; return "Positive value only."; }
                        else if (Cover >= Height) { HasErrors[0] = true; return "Cover can't be greater than or equal to height"; }
                        else { HasErrors[0] = false; return null; }
                    }
                }

                if (propertyName == "SideCover")
                {
                    if (!isRectangularSection) { HasErrors[1] = false; return null; }
                    else
                    {
                        if (SideCover >= Breadth) { HasErrors[1] = true; return "Side Cover can't be greater than or equal to width"; }
                        if (SideCover <= 0) { HasErrors[1] = true; return "Non-zero positive value only."; }
                        else { HasErrors[1] = false; return null; }
                    }
                    
                }

                if (propertyName == "Height")
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

                if (propertyName == "Breadth")
                {
                    if (!isRectangularSection) { HasErrors[1] = false; return null; }
                    else
                    {
                        if (Breadth <= 0) { HasErrors[3] = true; return "Non-zero positive value only."; }
                        else { HasErrors[3] = false; return null; }
                    }
                }

                if (propertyName == "Radius")
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

                if (propertyName == "Diameter")
                {
                    if (isRectangularSection) { HasErrors[1] = false; return null; }
                    else
                    {
                        if (Diameter <= 0) { HasErrors[5] = true; return "Non-zero positive value only."; }
                        else { HasErrors[5] = false; return null; }
                    }
                }

                // If there's no error, null gets returned
                return null;
            }

        }

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

        public Func<double> GetMinimumDimension { get; set; }
        public Rebars()
        {
        }
    }


}
