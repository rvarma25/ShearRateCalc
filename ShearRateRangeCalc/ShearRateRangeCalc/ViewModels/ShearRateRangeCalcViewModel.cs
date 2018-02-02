using System.Windows.Input;
using ShearRateRangeCalc.Helpers;
using ShearRateRangeCalc.Models;
using System.Collections.Generic;
using System;
using System.Windows;

namespace ShearRateRangeCalc.ViewModels
{
    public class ShearRateRangeCalcViewModel : ObservableObject
    {
        #region Fields

        const int SIGNIFICANT_DIGITS = 4;
        const string SYRINGE = " Syringe";
        const double ACCEPTABLE_FLOW_RATE_OVERLAP = 0.70;
        const string NO_SUGGESTION_COMMENT = "No suggestion available for the chosen inputs.";
        const string BEST_SELECTION_SUGGESTION_COMMENT = "{0} syringe provides from {1} μl/min to {2} μl/min, which is the best selection and colored in Green.";

        private double _FactorK;

        public bool _SuggestBtnClicked = false;


        double _max_possible_flow_rate = 1.0;
        double _min_possible_flow_rate = 1.0;

        public bool IsWindowLoaded { get; set; }

        public double CanvasWidth { get; set; }

        public bool SuggestBtnClicked { get; set; }

        public double FlowRateToScreenUnitsFactor { get; set; }

        private Chip _currentChip;
        public Chip CurrentChip
        {
            get
            {
                return _currentChip;
            }

            set
            {
                _currentChip = value;
                ShearStressMin = _currentChip.TauMin;
                ShearStressMax = _currentChip.TauMax;
                _FactorK = _currentChip.K;
                OnPropertyChanged("CurrentChip");
                CalculateShearAndFlowRates();
                FindSyringe();
            }
        }

        private double _shearStressMin;
        public double ShearStressMin
        {

            get
            {
                return _shearStressMin;
            }

            set
            {
                _shearStressMin = value;
                OnPropertyChanged("ShearStressMin");
                CalculateShearAndFlowRates();
                FindSyringe();
            }
        }

        private double _shearStressMax;
        public double ShearStressMax
        {

            get
            {
                return _shearStressMax;
            }

            set
            {
                _shearStressMax = value;
                OnPropertyChanged("ShearStressMax");
                CalculateShearAndFlowRates();
                FindSyringe();
            }
        }

        private double? _estimatedViscosity;
        public double? EstimatedViscosity
        {

            get
            {
                return _estimatedViscosity;
            }

            set
            {
                _estimatedViscosity = value;;
                CalculateShearAndFlowRates();
                FindSyringe();
            }
        }

        public double ShearRateMin { get; set; }

        public double ShearRateMax { get; set; }
        
        public double FlowRateMin { get; set; }

        public double FlowRateMax { get; set; }

        private string _shearRateMinForView;
        public string ShearRateMinForView
        {
            get
            {
                return _shearRateMinForView;
            }

            set
            {
                _shearRateMinForView = value;
                OnPropertyChanged("ShearRateMinForView");
            }
        }

        private string _shearRateMaxForView;
        public string ShearRateMaxForView
        {
            get
            {
                return _shearRateMaxForView;
            }

            set
            {
                _shearRateMaxForView = value;
                OnPropertyChanged("ShearRateMaxForView");
            }
        }

        private string _flowRateMinForView;
        public string FlowRateMinForView
        {
            get
            {
                return _flowRateMinForView;
            }

            set
            {
                _flowRateMinForView = value;
                OnPropertyChanged("FlowRateMinForView");
            }
        }

        private string _flowRateMaxForView;
        public string FlowRateMaxForView
        {
            get
            {
                return _flowRateMaxForView;
            }

            set
            {
                _flowRateMaxForView = value;
                OnPropertyChanged("FlowRateMaxForView");
            }
        }

        private Syringe _currentSyringe;
        public Syringe CurrentSyringe
        {
            get
            {
                return _currentSyringe;
            }

            set
            {
                _currentSyringe = value;
                UpdateReferenceSyringeBar();
            }
        }

        private double _fluidBarLeft;
        public double FluidBarLeft
        {
            get
            {
                return _fluidBarLeft;
            }
            set
            {
                _fluidBarLeft = value;
                OnPropertyChanged("FluidBarLeft");
            }
        }

        private double _fluidBarWidth;
        public double FluidBarWidth
        {
            get
            {
                return _fluidBarWidth;
            }
            set
            {
                _fluidBarWidth = value;
                OnPropertyChanged("FluidBarWidth");
            }
        }

        private string _fluidBarText;
        public string FluidBarText
        {
            get
            {
                return _fluidBarText;
            }
            set
            {
                _fluidBarText = value;
                OnPropertyChanged("FluidBarText");
            }
        }

        private Visibility _fluidBarVisible;
        public Visibility FluidBarVisible
        {
            get
            {
                return _fluidBarVisible;
            }

            set
            {
                _fluidBarVisible = value;
                OnPropertyChanged("FluidBarVisible");
            }
        }

        private double _refBarLeft;
        public double RefBarLeft
        {
            get
            {
                return _refBarLeft;
            }
            set
            {
                _refBarLeft = value;
                OnPropertyChanged("RefBarLeft");
            }
        }

        private double _refBarWidth;
        public double RefBarWidth
        {
            get
            {
                return _refBarWidth;
            }
            set
            {
                _refBarWidth = value;
                OnPropertyChanged("RefBarWidth");
            }
        }

        private string _refBarText;
        public string RefBarText
        {
            get
            {
                return _refBarText;
            }
            set
            {
                _refBarText = value;
                OnPropertyChanged("RefBarText");
            }
        }

        private string _refBarLabel;
        public string RefBarLabel
        {
            get
            {
                return _refBarLabel;
            }
            set
            {
                _refBarLabel = value;
                OnPropertyChanged("RefBarLabel");
            }
        }

        private double _suggestBarLeft;
        public double SuggestBarLeft
        {
            get
            {
                return _suggestBarLeft;
            }
            set
            {
                _suggestBarLeft = value;
                OnPropertyChanged("SuggestBarLeft");
            }
        }

        private double _suggestBarWidth;
        public double SuggestBarWidth
        {
            get
            {
                return _suggestBarWidth;
            }
            set
            {
                _suggestBarWidth = value;
                OnPropertyChanged("SuggestBarWidth");
            }
        }

        private string _suggestBarText;
        public string SuggestBarText
        {
            get
            {
                return _suggestBarText;
            }
            set
            {
                _suggestBarText = value;
                OnPropertyChanged("SuggestBarText");
            }
        }

        private Visibility _suggestBarVisible;
        public Visibility SuggestBarVisible
        {
            get
            {
                return _suggestBarVisible;
            }

            set
            {
                _suggestBarVisible = value;
                OnPropertyChanged("SuggestBarVisible");
            }
        }

        #endregion

        #region Public Properties/Commands

        private Pump _currentPumpType;
        public Pump CurrentPump
        {
            get
            {
                return _currentPumpType;
            }
            set
            {
                _currentPumpType = value;
                OnPumpSelectionChanged();
            }
        }

        private string _comments;
        public string Comments
        {
            get
            {
                return _comments;
            }

            set
            {
                _comments = value;
                OnPropertyChanged("Comments");
            }
        }

        public List<Pump> PumpList { get; private set; }

        public List<Chip> ChipList { get; private set; }

        public List<Syringe> SyringeList { get; set; }

        public Syringe SuggestedSyringe { get; set; }

        public void UpdateFlowRateToScreenUnitsFactor()
        {
            if (!IsWindowLoaded)
                return;

            double pumpFlowrange = CurrentPump.Type == Pump.PumpType.Regular ? Math.Log(Pump.REGULAR_PUMP_FLOW_RATE_MAX) : Math.Log(Pump.FAST_PUMP_FLOW_RATE_MAX) - Math.Log(2);
            FlowRateToScreenUnitsFactor = CanvasWidth / pumpFlowrange;
        }

        private void CalculateShearAndFlowRates()
        {
            double shear_rate_min, shear_rate_max;

            if (EstimatedViscosity == null ||
                ShearStressMin <= 0 ||
                ShearStressMax <= 0 ||
                ShearStressMax < ShearStressMin
                )
                return;

            shear_rate_min = ((ShearStressMin * 1000) / EstimatedViscosity.Value);
            shear_rate_max = (ShearStressMax * 1000) / EstimatedViscosity.Value;

            FlowRateMin = shear_rate_min / _FactorK;
            FlowRateMax = shear_rate_max / _FactorK;

            FlowRateMin = Math.Min(Math.Max(FlowRateMin, _min_possible_flow_rate), _max_possible_flow_rate);
            FlowRateMax = Math.Max(Math.Min(FlowRateMax, _max_possible_flow_rate), _min_possible_flow_rate);

            shear_rate_min = FlowRateMin * _FactorK;
            shear_rate_max = FlowRateMax * _FactorK;

            ShearRateMinForView = To4SignificanDigits(shear_rate_min, SIGNIFICANT_DIGITS);

            ShearRateMaxForView = To4SignificanDigits(shear_rate_max, SIGNIFICANT_DIGITS);

            FlowRateMinForView = To4SignificanDigits(FlowRateMin, SIGNIFICANT_DIGITS);

            FlowRateMaxForView = To4SignificanDigits(FlowRateMax, SIGNIFICANT_DIGITS);

            FluidBarLeft = FlowRateToScreenUnitsFactor * Math.Log(FlowRateMin);
            FluidBarWidth = FlowRateToScreenUnitsFactor * (Math.Log(FlowRateMax) - Math.Log(FlowRateMin));
            FluidBarText = string.Format("({0} ~ {1} µl/min)", FlowRateMinForView, FlowRateMaxForView);
            FluidBarVisible = Visibility.Visible;

        }

        public void UpdateReferenceSyringeBar()
        {
            double minFlow = 2, maxFlow = 100;

            if (!IsWindowLoaded)
                return;

            if (CurrentPump.Type == Pump.PumpType.Regular)
            {
                minFlow = CurrentSyringe.RegularPumpMinFlowRate;
                maxFlow = CurrentSyringe.RegularPumpMaxFlowRate;
            }
            else
            {
                minFlow = CurrentSyringe.FastPumpMinFlowRate;
                maxFlow = CurrentSyringe.FastPumpMaxFlowRate;
            }

            RefBarLeft = FlowRateToScreenUnitsFactor * Math.Log(minFlow);
            RefBarWidth = FlowRateToScreenUnitsFactor * (Math.Log(maxFlow) - Math.Log(minFlow));
            RefBarText = string.Format("{0} ({1} ~ {2} µl/min)", CurrentSyringe.Size, To4SignificanDigits(minFlow, 4), To4SignificanDigits(maxFlow, 4));
            RefBarLabel = CurrentSyringe.Size + SYRINGE;
        }

        private void UpdateSuggestedSyringeBar()
        {
            double minFlow, maxFlow;

            if (!IsWindowLoaded)
                return;

            if (SuggestedSyringe == null)
            {
                SuggestBarVisible = Visibility.Hidden;
                return;
            }


            if (CurrentPump.Type == Pump.PumpType.Regular)
            {
                minFlow = SuggestedSyringe.RegularPumpMinFlowRate;
                maxFlow = SuggestedSyringe.RegularPumpMaxFlowRate;
            }
            else
            {
                minFlow = SuggestedSyringe.FastPumpMinFlowRate;
                maxFlow = SuggestedSyringe.FastPumpMaxFlowRate;
            }

            SuggestBarLeft = FlowRateToScreenUnitsFactor * Math.Log(minFlow);
            SuggestBarWidth = FlowRateToScreenUnitsFactor * (Math.Log(maxFlow) - Math.Log(minFlow));
            SuggestBarText = string.Format("{0} ({1} ~ {2} µl/min)", SuggestedSyringe.Size, To4SignificanDigits(minFlow, 4), To4SignificanDigits(maxFlow, 4)); ;

            SuggestBarVisible = Visibility.Visible;
        }

        private ICommand _suggestSyringeCommand;
        public ICommand SuggestSyringeCommand
        {
            get
            {
                if (_suggestSyringeCommand == null)
                {
                    _suggestSyringeCommand = new RelayCommand(param => FindSyringe());
                }
                return _suggestSyringeCommand;
            }
        }

        //SetStressesToDefaultCommand
        private ICommand _setStressesToDefaultCommand;
        public ICommand SetStressesToDefaultCommand
        {
            get
            {
                if (_setStressesToDefaultCommand == null)
                {
                    _setStressesToDefaultCommand = new RelayCommand(param => SetToDefaults());
                }
                return _setStressesToDefaultCommand;
            }
        }

        #endregion

        #region Private Helpers

        private void SetToDefaults()
        {
            ShearStressMin = _currentChip.TauMin;
            ShearStressMax = _currentChip.TauMax;
        }

        private void FindSyringe()
        {
            Comments = "";
            if (ShearStressMin <= 0.0 ||
                ShearStressMax <= 0.0 ||
                EstimatedViscosity <= 0.0 ||
                !SuggestBtnClicked)
                return;

            double overlap = 0.0;
            double flowrateRange = FlowRateMax - FlowRateMin;

            // Algorithm to find max overlap between 2 collinear lines
            // if (pb - pc >= 0 and pd - pa >=0 ) { // overlap
            // OverlapInterval = [max(pa, pc), min(pb, pd)] // it could be a point [x, x]
            SuggestedSyringe = null;
            for (int i= 0; i < SyringeList.Count; ++i)
            {
                double tmpOverlap;
                double beg, end;
                Syringe s = SyringeList[i];
                if (CurrentPump.Type == Pump.PumpType.Regular)
                {
                    beg = s.RegularPumpMinFlowRate;
                    end = s.RegularPumpMaxFlowRate;
                }
                else
                {
                    beg = s.FastPumpMinFlowRate;
                    end = s.FastPumpMaxFlowRate;
                }
                if (FlowRateMax - beg >= 0.0 && end - FlowRateMin >= 0.0)
                {
                    tmpOverlap = Math.Abs(Math.Max(FlowRateMin, beg) - Math.Min(FlowRateMax, end));
                    if (tmpOverlap > 0.0)
                    {
                        SuggestedSyringe = s;
                    }
                    if (tmpOverlap > overlap)
                    {
                        overlap = tmpOverlap;
                        if (tmpOverlap / flowrateRange >= ACCEPTABLE_FLOW_RATE_OVERLAP)
                            break;
                    }
                }
            }

            if (SuggestedSyringe != null)
            {
                //UpdateSuggestedSyringeBar();

                if (CurrentPump.Type == Pump.PumpType.Regular)
                    Comments = string.Format(BEST_SELECTION_SUGGESTION_COMMENT, SuggestedSyringe.Size, SuggestedSyringe.RegularPumpMinFlowRate, SuggestedSyringe.RegularPumpMaxFlowRate);
                else
                    Comments = string.Format(BEST_SELECTION_SUGGESTION_COMMENT, SuggestedSyringe.Size, SuggestedSyringe.FastPumpMinFlowRate, SuggestedSyringe.FastPumpMaxFlowRate);
            }
            else
            {
                Comments = NO_SUGGESTION_COMMENT;
            }
            UpdateSuggestedSyringeBar();
        }


        public ShearRateRangeCalcViewModel()
        {
            // Create Pump List
            PumpList = new List<Pump>()
            {
                new Pump() {Name = "Regular", Type=Pump.PumpType.Regular},
                new Pump() {Name = "Fast", Type=Pump.PumpType.Fast},
            };

            // Create Chip List
            ChipList = new List<Chip>()
            { 
                new Chip() {PressureSensorType = "A", ChannelDepth = 2,  TauMin =  0.45, TauMax = 11.29,  K = 83.33 },
                new Chip() {PressureSensorType = "A", ChannelDepth = 5,  TauMin =  1.12, TauMax = 27.96,  K = 13.33 },
                new Chip() {PressureSensorType = "A", ChannelDepth = 10, TauMin =  2.20, TauMax = 55.04,  K = 3.333 },
                new Chip() {PressureSensorType = "A", ChannelDepth = 20, TauMin =  4.27, TauMax = 106.7,  K = 0.8333 },
                new Chip() {PressureSensorType = "A", ChannelDepth = 30, TauMin =  5.93, TauMax = 148.2,  K = 0.3704 },
                new Chip() {PressureSensorType = "B", ChannelDepth = 5,  TauMin =  4.47, TauMax = 111.8,  K = 13.33 },
                new Chip() {PressureSensorType = "B", ChannelDepth = 10, TauMin =  8.81, TauMax = 220.1,  K = 3.333 },
                new Chip() {PressureSensorType = "B", ChannelDepth = 20, TauMin =  17.08, TauMax = 427.0,  K = 0.8333 },
                new Chip() {PressureSensorType = "B", ChannelDepth = 30, TauMin =  23.72, TauMax = 592.8,  K = 0.3704 },
                new Chip() {PressureSensorType = "C", ChannelDepth = 5,  TauMin =  22.37, TauMax = 559.1, K = 13.33 },
                new Chip() {PressureSensorType = "C", ChannelDepth = 10, TauMin =  44.03, TauMax = 1100,  K = 3.333 },
                new Chip() {PressureSensorType = "C", ChannelDepth = 20, TauMin =  85.40, TauMax = 2134,  K = 0.8333 },
                new Chip() {PressureSensorType = "E", ChannelDepth = 2,  TauMin =  81.01, TauMax = 2025,  K = 83.33},
                new Chip() {PressureSensorType = "E", ChannelDepth = 5,  TauMin =  199.6, TauMax = 4988,  K = 13.33 }
            };

            // Create Syringe List
            SyringeList = new List<Syringe>()
            {
                new Syringe() {Size = "100 μl", RegularPumpMinFlowRate = 1,   RegularPumpMaxFlowRate = 200,   FastPumpMinFlowRate = 2,   FastPumpMaxFlowRate = 400  },
                new Syringe() {Size = "250 μl", RegularPumpMinFlowRate = 2.5, RegularPumpMaxFlowRate = 500,   FastPumpMinFlowRate = 5,   FastPumpMaxFlowRate = 1000  },
                new Syringe() {Size = "500 μl", RegularPumpMinFlowRate = 5,   RegularPumpMaxFlowRate = 1000,  FastPumpMinFlowRate = 10,  FastPumpMaxFlowRate = 2000  },
                new Syringe() {Size = "1 mL", RegularPumpMinFlowRate =   10,  RegularPumpMaxFlowRate = 2000,  FastPumpMinFlowRate = 20,  FastPumpMaxFlowRate = 4000  },
                new Syringe() {Size = "10 mL", RegularPumpMinFlowRate =  100, RegularPumpMaxFlowRate = 20000, FastPumpMinFlowRate = 200, FastPumpMaxFlowRate = 40000  }
            };

            CurrentPump = PumpList[0];

            CurrentChip = ChipList[0];

            CurrentSyringe = SyringeList[0];

            IsWindowLoaded = false;

            FluidBarLeft = 0;

            FluidBarWidth = 200;

            FluidBarVisible = Visibility.Hidden;
            SuggestBarVisible = Visibility.Hidden;

            SuggestedSyringe = SyringeList[0];

        }

        private string To4SignificanDigits(double value, int significant_digits)
        {
            // Use G format to get significant digits.
            // Then convert to double and use F format.
            string format1 = "{0:G" + significant_digits.ToString() + "}";
            string result = Convert.ToDouble(String.Format(format1, value)).ToString("F99");

            // Rmove trailing 0s.
            result = result.TrimEnd('0');

            return result;
        }

        private void OnPumpSelectionChanged()
        {
            _min_possible_flow_rate = (CurrentPump.Type == Pump.PumpType.Regular) ? Pump.REGULAR_PUMP_FLOW_RATE_MIN : Pump.FAST_PUMP_FLOW_RATE_MIN;
            _max_possible_flow_rate = (CurrentPump.Type == Pump.PumpType.Regular) ? Pump.REGULAR_PUMP_FLOW_RATE_MAX : Pump.FAST_PUMP_FLOW_RATE_MAX;

            UpdateFlowRateToScreenUnitsFactor();

            CalculateShearAndFlowRates();

            UpdateReferenceSyringeBar();

            FindSyringe();
        }

        #endregion
    }
}
