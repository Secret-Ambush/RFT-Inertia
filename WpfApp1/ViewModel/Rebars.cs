using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1;

namespace WpfApp1
{
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
        public override string this[string propName]
        {
            get
            {
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
                HasErrors[6] = true; 
                UpdateErrorList(nonPositiveError, true);
                return "Error";
            }
            else if (NumOfRebar < 2)
            {
                HasErrors[6] = true;
                UpdateErrorList(lessThanTwoError, true);
                return "Error";
            }
            else
            {
                HasErrors[6] = false;
                UpdateErrorList(nonPositiveError, false);
                UpdateErrorList(lessThanTwoError, false);
                return string.Empty;
            }
        }
        private string ValidateDeltaY()
        {
            var radius = GetMinimumDimension?.Invoke() ?? 0; // holds the radius value
            var height_RectangularColumn = GetHeightDimension?.Invoke() ?? 0; // holds the height 
            var maxValue = isRectangularSection ? height_RectangularColumn : radius;
            string dim = "0";
            string exceediverError = $"Delta Y can't be greater than {dim}";
            string nonPositiveError = "Delta Y should have a positive value only.";

            if (isRectangularSection)
                maxValue -= (2 * Cover + RebarDia); // max delta y considers cover, rebar diameter
            else
                maxValue -= (Cover + RebarDia);

            if (DeltaY < 0)
            {
                HasErrors[7] = true;
                UpdateErrorList(nonPositiveError, true);
                return "Error";
            }
            else if ((DeltaY != 0) && (DeltaY > maxValue))
            {
                dim = maxValue.ToString();
                HasErrors[7] = true;
                UpdateErrorList(exceediverError, true);
                return "Error";
            }
            else
            {
                HasErrors[7] = false;
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
                HasErrors[8] = true;
                UpdateErrorList(nonPositiveError, true);
                return "Error";
            }
            else if (RebarDia > minValue)
            {
                HasErrors[8] = true;
                UpdateErrorList(exceediverError, true);
                return "Error";
            }
            else
            {
                HasErrors[8] = false;
                UpdateErrorList(nonPositiveError, false);
                UpdateErrorList(exceediverError, false);
                return string.Empty;
            }
        }

        #endregion

        private Func<double> GetMinimumDimension { get; set; }
        private Func<double> GetHeightDimension { get; set; }
        private Action<string, bool> UpdateErrorList { get; set; }

        public void SetCallBcks(Func<double> getMinimumDimension, Func<double> getHeightDimension, Action<string, bool> updateErrorList)
        {
            GetMinimumDimension = getMinimumDimension;
            GetHeightDimension = getHeightDimension;
            UpdateErrorList = updateErrorList;
        }
        public Rebars()
        {

        }
    }
}
