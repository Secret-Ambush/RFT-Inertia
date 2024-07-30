using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1;

namespace WpfApp1
{
    public class Rebars : BaseViewModel
    {

        #region Private members
        private double _dia;
        private int _num;
        private double _delta;
        private int _count;
        #endregion

        #region Public Properties
        public bool isRectangularSection { get; set; }
        public double cover { get; set; }
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
        public override string this[string propName]
        {
            get
            {
                isRectangularSection = IsRectangularSection?.Invoke() ?? true;
                cover = GetCover?.Invoke() ?? 0;

                return propName switch
                { 
                    nameof(NumOfRebar) => ValidateNumOfRebar(),
                    nameof(DeltaY) => ValidateDeltaY(),
                    nameof(RebarDia) => ValidateRebarDia(),
                    _ => String.Empty
                };
            }
        }
        private string ValidateNumOfRebar()
        {
            string nonPositiveError = "Number of rebars should have a non-zero positive value.";
            string lessThanTwoError = "Number of rebars should be more than or equal to two.";


            if (NumOfRebar <= 0)
            {
                UpdateErrorList(nonPositiveError, true);
                return "Error";
            }
            else if (NumOfRebar < 2)
            {
                UpdateErrorList(lessThanTwoError, true);
                return "Error";
            }
            else
            {
                UpdateErrorList(nonPositiveError, false);
                UpdateErrorList(lessThanTwoError, false);
                return string.Empty;
            }
        }
        private string ValidateDeltaY()
        {
            var radius = GetMinimumDimension?.Invoke() ?? 0; // holds the radius
            var heightRectangularColumn = GetHeightDimension?.Invoke() ?? 0; // holds the height 
            var stirrupDiameter = GetStirrupDiameter?.Invoke() ?? 0; // holds the Stirrup Diameter 
            var maxValue = isRectangularSection ? heightRectangularColumn : radius;
            string exceediverError = "Delta Y can't be greater than boundary";
            string nonPositiveError = "Delta Y should have a positive value only.";

            if (isRectangularSection)
                maxValue -= (2 * cover + RebarDia + 2 * stirrupDiameter); // max delta y considers cover, rebar diameter
            else
                maxValue -= (cover + RebarDia + stirrupDiameter);

            if (DeltaY < 0)
            {
                UpdateErrorList(nonPositiveError, true);
                return "Error";
            }
            else if ((DeltaY != 0) && (DeltaY > maxValue))
            {
                UpdateErrorList(exceediverError, true);
                return "Error";
            }
            else
            {
                UpdateErrorList(nonPositiveError, false);
                UpdateErrorList(exceediverError, false);
                return string.Empty;
            }


        }
        private string ValidateRebarDia()
        {
            var minValue = GetMinimumDimension?.Invoke() ?? 0;
            var dim = isRectangularSection ? "boundary" : "radius";
            string exceediverError = $"Rebar Dia can't be greater than {dim} of section";
            string nonPositiveError = "Rebar Dia should have a non-zero positive value only.";

            if (RebarDia <= 0)
            {
                UpdateErrorList(nonPositiveError, true);
                return "Error";
            }
            else if (RebarDia > minValue)
            {
                UpdateErrorList(exceediverError, true);
                return "Error";
            }
            else
            {
                UpdateErrorList(nonPositiveError, false);
                UpdateErrorList(exceediverError, false);
                return string.Empty;
            }
        }

        #endregion

        private Func<double> GetMinimumDimension { get; set; }
        private Func<bool> IsRectangularSection { get; set; }
        private Func<double> GetHeightDimension { get; set; }
        private Func<double> GetStirrupDiameter { get; set; }
        private Action<string, bool> UpdateErrorList { get; set; }
        private Func<double> GetCover {  get; set; }

        public void SetCallBcks(Func<double> getMinimumDimension, Func<double> getHeightDimension, Func<bool> GetIfRectangularSection, Func<double> getCover, Func<double> getStirrupDiameter, Action<string, bool> updateErrorList)
        {
            GetMinimumDimension = getMinimumDimension;
            GetHeightDimension = getHeightDimension;
            GetStirrupDiameter = getStirrupDiameter;
            IsRectangularSection = GetIfRectangularSection;
            GetCover = getCover;
            UpdateErrorList = updateErrorList;
        }
        public Rebars()
        {

        }
    }
}
