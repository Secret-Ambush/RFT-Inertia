using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;

namespace WpfApp1
{
    public class WindowViewModel : BaseViewModel, IDataErrorInfo
    {
        #region Private Properties

        private ObservableCollection<Rebars>? _userint;
        private Window mWindow;
        private static bool _option = true;
        private ComboBoxItem _mySelectedItem;
        private string _image = "Images/full.jpg";
        private double _radius;
        private double _diameter;
        private double _sidecover;
        private double _height;
        private double _breadth;
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
        private bool _flag;

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
        public ICommand Calculate { get; set; }
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
        public bool Option
        {
            get { return _option; }
            set { _option = value; OnPropertyChanged(nameof(Option)); }
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
                        { Option = true; break; }
                    case ("Circular Column"):
                        { Option = false; break; }
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

        public bool Flag
        {
            get => _flag;
            set
            {
                foreach (var item in Entries)
                {
                    var d = item as Rebars;
                    if (d != null && !string.IsNullOrEmpty(d.Error)) _flag = false;
                }
                _flag = true;
                OnPropertyChanged(nameof(Flag));
            }

        }
        public void CalculatingInertia()
        {
            if (!Option) //circular
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

            Calculate = new RelayCommand(() => CalculatingInertia());

            ClearAll = new RelayCommand(() =>
            {
                RebarIx = RebarIy = RebarRadiusX = RebarRadiusY = Radius = Diameter = TotalRx = TotalRy = 0;
                Height = Breadth = Cover = SideCover = AreaOfRebars = 0;
                Entries = new ObservableCollection<Rebars>();
            });

            Entries = new ObservableCollection<Rebars>();
            Entries.CollectionChanged += OnEntriesCollectionChanged;

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
                item.Count = Entries.IndexOf(item) + 1;
            }
        }

        #endregion

        string IDataErrorInfo.Error
        {
            get { return null; }
        }

        string IDataErrorInfo.this[string propertyName]
        {
            get
            {
                if (propertyName == "Cover")
                {

                    if (!Option)
                    {
                        if (Cover < 0) { return "Positive value only."; }
                        if (Cover >= Radius) { return "Cover can't be greater than or equal to radius"; }
                    }
                    else
                    {
                        if (Cover < 0) { return "Positive value only."; }
                        if (Cover >= Height) { return "Cover can't be greater than or equal to height"; }
                    }
                }

                if (propertyName == "SideCover")
                {
                    if (SideCover >= Breadth) { return "Side Cover can't be greater than or equal to width"; }
                    if (SideCover <= 0) { return "Non-zero positive value only."; }
                }

                if (propertyName == "Height")
                {
                    if (Height <= 0) { return "Non-zero positive value only."; }
                }

                if (propertyName == "Breadth")
                {
                    if (Breadth <= 0) { return "Non-zero positive value only."; }
                }

                if (propertyName == "Radius")
                {
                    if (Radius <= 0) { return "Non-zero positive value only."; }
                }

                if (propertyName == "Diameter")
                {
                    if (Diameter <= 0) { return "Non-zero positive value only."; }
                }

                // If there's no error, null gets returned
                return null;
            }

        }

    }

    public class Rebars : WindowViewModel, IDataErrorInfo
    {
        private double _dia;
        private int _num;
        private double _delta;
        private int _count;


        public double Dia
        {
            get { return _dia; }
            set { _dia = value; }
        }

        public int Num
        {
            get { return _num; }
            set { _num = value; }
        }

        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                OnPropertyChanged(nameof(Count));
            }
        }

        public double DeltaY
        {
            get { return _delta; }
            set
            {
                if (Count == 1) { _delta = 0; }
                else { _delta = value; }
            }
        }

        public string Error => throw new NotImplementedException();

        public string this[string propName]
        {
            get
            {
                if (propName == "Num")
                {
                    if (Num < 4) { return "Number of Rebars must be a minimum of 4"; }
                    if (Num <= 0) { return "Non-zero positive value only."; }
                }

                if (propName == "DeltaY")
                {
                    if (DeltaY < 0) { return "Positive value only."; }
                }

                if (propName == "Dia")
                {
                    if (!Option)
                    {
                        if (Dia > Radius) { return "Diamter of rebar can't be greater or equal to radius of circle"; }
                        if (Dia <= 0) { return "Non-zero positive value only."; }
                        if (DeltaY >= Radius) { return "Delta Y can't be greater or equal to radius of circle"; }
                    }
                    else
                    {
                        if (Dia >= Breadth || Dia >= Height) { return "Diamter of rebar can't be greater or equal to radius of circle"; }
                        if (Dia <= 0) { return "Non-zero positive value only."; }
                    }
                }

                return null;
            }
        }

        public Rebars()
        {
        }
    }


}
