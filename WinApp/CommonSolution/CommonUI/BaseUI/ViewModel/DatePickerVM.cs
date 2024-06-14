/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - view model for generic data entry page
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Raja Uthanda            Created for POS UI Redesign 
 ********************************************************************************************/
using System;
using System.Linq;
using System.Reflection;
using System.Globalization;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Semnox.Parafait.CommonUI
{
    internal enum Days
    {
        SUN = 0,
        MON = 1,
        TUE = 2,
        WED = 3,
        THU = 4,
        FRI = 5,
        SAT = 6
    }

    internal enum AMorPM
    {
        None,
        AM,
        PM
    }

    internal class DatePickerVM : ViewModelBase
    {

        #region Members
        private int selectedDate;
        private int selectedYear;
        private int currentMonthWeekLastDate;
        private int currentMonthFirstWeekFirstDate;

        private string selectedHour;
        private string selectedMonth;
        private string selectedMinute;
        
        private bool showTimePicker;
        private bool enableOkButton;
        private bool isFromDateChange;
        private bool editableMonthYear;       
        private bool enableDaySelection;
        private bool enableYearSelection;
        private bool enableMonthSelection;
        private bool nextNavigationEnable;
        private bool previousNavigationEnable;

        private AMorPM aMorPM;

        private ICommand actionsCommand;

        private DateTime disableTill;
        private DatePickerView datePickerView;

        private ObservableCollection<int> years;
        private ObservableCollection<string> hours;
        private ObservableCollection<string> months;
        private ObservableCollection<string> minutes;
        private ObservableCollection<string> daysCollection;
        private ObservableCollection<DayDetails> fifthWeekDates;
        private ObservableCollection<DayDetails> sixthWeekDates;
        private ObservableCollection<DayDetails> firstWeekDates;
        private ObservableCollection<DayDetails> thirdWeekDates;
        private ObservableCollection<DayDetails> secondWeekDates;
        private ObservableCollection<DayDetails> fourthWeekDates;

        private static readonly logging.Logger log = new logging.Logger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public bool EnableOkButton
        {
            get { return enableOkButton; }
            private set { SetProperty(ref enableOkButton, value); }
        }
        public bool EnableDaySelection
        {
            get { return enableDaySelection; }
            set { SetProperty(ref enableDaySelection, value); }
        }
        public bool EnableMonthSelection
        {
            get { return enableMonthSelection; }
            set { SetProperty(ref enableMonthSelection, value); }
        }
        public DateTime DisableTill
        {
            get { return disableTill; }
            set { SetProperty(ref disableTill, value); }
        }
        public bool EditableMonthYear
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(editableMonthYear);
                return editableMonthYear;
            }
            set
            {
                log.LogMethodEntry(editableMonthYear, value);
                SetProperty(ref editableMonthYear, value);
                log.LogMethodExit(editableMonthYear);
            }
        }
        public AMorPM AMorPM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(aMorPM);
                return aMorPM;
            }
            set
            {
                log.LogMethodEntry(aMorPM, value);
                SetProperty(ref aMorPM, value);
                log.LogMethodExit(aMorPM);
            }
        }
        public bool ShowTimePicker
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(showTimePicker);
                return showTimePicker;
            }
            set
            {
                log.LogMethodEntry(showTimePicker, value);
                SetProperty(ref showTimePicker, value);
                log.LogMethodExit(showTimePicker);
            }
        }
        public string SelectedHour
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedHour);
                return selectedHour;
            }
            set
            {
                log.LogMethodEntry(selectedHour, value);
                SetProperty(ref selectedHour, value);
                log.LogMethodExit(selectedHour);
            }
        }
        public string SelectedMinute
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedMinute);
                return selectedMinute;
            }
            set
            {
                log.LogMethodEntry(selectedMinute, value);
                SetProperty(ref selectedMinute, value);
                log.LogMethodExit(selectedMinute);
            }
        }
        internal bool IsFromDateChange
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(isFromDateChange);
                return isFromDateChange;
            }
            set
            {
                log.LogMethodEntry(isFromDateChange);
                SetProperty(ref isFromDateChange, value);
                log.LogMethodExit(isFromDateChange);
            }
        }
        public bool PreviousNavigationEnable
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(previousNavigationEnable);
                return previousNavigationEnable;
            }
            set
            {
                log.LogMethodEntry(previousNavigationEnable, value);
                SetProperty(ref previousNavigationEnable, value);
                log.LogMethodExit(previousNavigationEnable);
            }
        }
        public bool NextNavigationEnable
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(nextNavigationEnable);
                return nextNavigationEnable;
            }
            set
            {
                log.LogMethodEntry(nextNavigationEnable, value);
                SetProperty(ref nextNavigationEnable, value);
                log.LogMethodExit(nextNavigationEnable);
            }
        }
        public int SelectedDate
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedDate);
                return selectedDate;
            }
            set
            {
                log.LogMethodEntry(selectedDate, value);
                SetProperty(ref selectedDate, value);
                log.LogMethodExit(selectedDate);
            }
        }
        public bool EnableYearSelection
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(enableYearSelection);
                return enableYearSelection;
            }
            set
            {
                log.LogMethodEntry(enableYearSelection, value);
                SetProperty(ref enableYearSelection, value);
                log.LogMethodExit(enableYearSelection);
            }
        }
        public int SelectedYear
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedYear);
                return selectedYear;
            }
            set
            {
                log.LogMethodEntry(selectedYear, value);
                if (enableYearSelection)
                {
                    if (DisableTill != DateTime.MinValue && value < DisableTill.Year)
                    {
                        SelectedYear = DisableTill.Year;
                    }
                    else
                    {
                        SetProperty(ref selectedYear, value);
                    }
                }
                log.LogMethodExit(selectedYear);
            }
        }
        public string SelectedMonth
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedMonth);
                return selectedMonth;
            }
            set
            {
                log.LogMethodEntry(selectedMonth, value);
                int monthIndex = months.IndexOf(value);
                if (monthIndex > -1)
                {
                    string disableMonth = GetDisableMonth();
                    if (DisableTill != DateTime.MinValue && DisableTill.Year == selectedYear &&
                        monthIndex < months.IndexOf(disableMonth))
                    {
                        SelectedMonth = disableMonth;
                    }
                    else
                    {
                        SetProperty(ref selectedMonth, value);
                    }
                }
                log.LogMethodExit(selectedMonth);
            }
        }
        public ObservableCollection<DayDetails> FirstWeekDates
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(firstWeekDates);
                return firstWeekDates;
            }
            private set
            {
                log.LogMethodEntry(value);
                SetProperty(ref firstWeekDates, value);
                log.LogMethodExit(firstWeekDates);
            }
        }
        public ObservableCollection<DayDetails> SecondWeekDates
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(secondWeekDates);
                return secondWeekDates;
            }
            private set
            {
                log.LogMethodEntry(secondWeekDates, value);
                SetProperty(ref secondWeekDates, value);
                log.LogMethodExit(secondWeekDates);
            }
        }
        public ObservableCollection<DayDetails> ThirdWeekDates
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(thirdWeekDates);
                return thirdWeekDates;
            }
            private set
            {
                log.LogMethodEntry(thirdWeekDates, value);
                SetProperty(ref thirdWeekDates, value);
                log.LogMethodExit(thirdWeekDates);
            }
        }
        public ObservableCollection<DayDetails> FourthWeekDates
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(fourthWeekDates);
                return fourthWeekDates;
            }
            private set
            {
                log.LogMethodEntry(fourthWeekDates, value);
                SetProperty(ref fourthWeekDates, value);
                log.LogMethodExit(fourthWeekDates);
            }
        }
        public ObservableCollection<DayDetails> FifthWeekDates
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(fifthWeekDates);
                return fifthWeekDates;
            }
            private set
            {
                log.LogMethodEntry(fifthWeekDates, value);
                SetProperty(ref fifthWeekDates, value);
                log.LogMethodExit(fifthWeekDates);
            }
        }
        public ObservableCollection<DayDetails> SixthWeekDates
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(sixthWeekDates);
                return sixthWeekDates;
            }
            private set
            {
                log.LogMethodEntry(sixthWeekDates, value);
                SetProperty(ref sixthWeekDates, value);
                log.LogMethodExit(sixthWeekDates);
            }
        }
        public ObservableCollection<string> DaysCollection
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(daysCollection);
                return daysCollection;
            }
        }
        public ObservableCollection<string> Months
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(months);
                return months;
            }
        }
        public ObservableCollection<int> Years
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(years);
                return years;
            }
        }
        public ObservableCollection<string> Hours
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(hours);
                return hours;
            }
        }
        public ObservableCollection<string> Minutes
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(minutes);
                return minutes;
            }
        }
        public ICommand ActionsCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(actionsCommand);
                return actionsCommand;
            }
        }
        #endregion

        #region Constructors and Finalizers
        internal DatePickerVM()
        {
            log.LogMethodEntry();

            InitializeCommands();
                        
            showTimePicker = false;
            editableMonthYear = false;            

            currentMonthWeekLastDate = 0;
            currentMonthFirstWeekFirstDate = 1;

            DateTime dateTime = DateTime.Now;

            selectedYear = dateTime.Year;
            selectedDate = dateTime.Date.Day;
            selectedMonth = dateTime.ToString("MMMM");

            int yearRange = 500;
            aMorPM = AMorPM.None;

            hours = new ObservableCollection<string>(Enumerable.Range(1, 12).ToList().Select(h => h.ToString("D2")));
            minutes = new ObservableCollection<string>(Enumerable.Range(0, 60).ToList().Select(h => h.ToString("D2")));
            months = new ObservableCollection<string>(DateTimeFormatInfo.CurrentInfo.MonthNames.Where(month => !string.IsNullOrEmpty(month)).ToList());
            years = new ObservableCollection<int>(Enumerable.Range(selectedYear - yearRange, yearRange).ToList());
            firstWeekDates = new ObservableCollection<DayDetails>();
            secondWeekDates = new ObservableCollection<DayDetails>();
            thirdWeekDates = new ObservableCollection<DayDetails>();
            fourthWeekDates = new ObservableCollection<DayDetails>();
            fifthWeekDates = new ObservableCollection<DayDetails>();
            sixthWeekDates = new ObservableCollection<DayDetails>();
            daysCollection = new ObservableCollection<string>(Enum.GetNames(typeof(Days)).ToList().Select(s => s.ToString()));

            years = new ObservableCollection<int>(years.Concat(Enumerable.Range(selectedYear, yearRange).ToList()));
            DateTime dateValue = new DateTime(selectedYear, months.IndexOf(selectedMonth) + 1, currentMonthFirstWeekFirstDate);

            Days firstday = (Days)Enum.Parse(typeof(Days), dateValue.ToString("ddd"), true);

            int monthIndex = months.IndexOf(selectedMonth);
            int previousMonthFinalDay = DateTime.DaysInMonth(selectedYear, monthIndex == 0 ? months.Count : monthIndex);
            EnableYearSelection = true;
            EnableDaySelection = true;
            EnableMonthSelection = true;
            EnableOkButton = true;
            GetMonthWeeks();
            SetNavigationEnable();

            log.LogMethodExit();
        }
        #endregion

        #region Methods
        private void InitializeCommands()
        {
            log.LogMethodEntry();
            actionsCommand = new DelegateCommand(OnActionsClicked);
            PropertyChanged += OnPropertyChanged;
            log.LogMethodExit();
        }
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (!string.IsNullOrWhiteSpace(e.PropertyName))
            {   
                switch (e.PropertyName)
                {
                    case "SelectedDate":
                    case "EnableDaySelection":
                    case "EnableMonthSelection":
                    case "DisablePreviousDates":
                    case "DisableTill":
                        {
                            GetMonthWeeks();
                        }
                        break;
                    case "SelectedYear":
                        {
                            GetMonthWeeks();
                            string disableMonth = GetDisableMonth();
                            if (months.IndexOf(selectedMonth) < months.IndexOf(disableMonth))
                            {
                                SelectedMonth = disableMonth;
                            }
                        }
                        break;
                    case "SelectedMonth":
                        if (selectedMonth != null)
                        {
                            GetMonthWeeks();
                        }
                        break;
                    case "AMorPM":
                        SetRadioButtonValue();
                        break;
                }
            }
            log.LogMethodExit();
        }
        private void SetNavigationEnable()
        {
            log.LogMethodEntry();
            PreviousNavigationEnable = CanPreviousNavigationExecute();
            NextNavigationEnable = CanNextNavigationExecute();
            log.LogMethodExit();
        }
        private void OnActionsClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                string actionText = parameter as string;
                DatePickerView pickerView = parameter as DatePickerView;
                if (!string.IsNullOrWhiteSpace(actionText))
                {
                    switch (actionText.ToLower())
                    {
                        case "ok":
                            {
                                OnOkClicked();
                            }
                            break;
                        case "cancel":
                            {
                                OnCancelClicked();
                            }
                            break;
                        case "prev":
                            {
                                OnPreviousNavigation();
                            }
                            break;
                        case "next":
                            {
                                OnNextNavigation();
                            }
                            break;
                    }
                }
                else if (pickerView != null)
                {
                    datePickerView = pickerView;
                    SetTimePickerValues();
                }
            }
            log.LogMethodExit();
        }
        private void OnNextNavigation()
        {
            log.LogMethodEntry();
            if (selectedMonth.ToLower() != "december")
            {
                SelectedMonth = months[months.IndexOf(this.SelectedMonth) + 1];
            }
            else if (enableYearSelection && years.Contains(this.SelectedYear + 1))
            {
                SelectedYear += 1;
                SelectedMonth = "January";
            }
            log.LogMethodExit();
        }
        private bool CanNextNavigationExecute()
        {
            log.LogMethodEntry();
            bool isEnable = true;
            if (!EnableDaySelection || !EnableMonthSelection || selectedMonth.ToLower() == "december" && (!enableYearSelection || !years.Contains(selectedYear + 1)))
            {
                isEnable = false;
            }
            log.LogMethodExit(isEnable);
            return isEnable;
        }
        private void OnPreviousNavigation()
        {
            log.LogMethodEntry();
            if (selectedMonth.ToLower() != "january")
            {
                SelectedMonth = months[months.IndexOf(selectedMonth) - 1];
            }
            else if (enableYearSelection && years.Contains(this.SelectedYear - 1))
            {
                SelectedYear -= 1;
                SelectedMonth = "December";
            }
            log.LogMethodExit();
        }
        private bool CanPreviousNavigationExecute()
        {
            log.LogMethodEntry();
            bool isEnable = true;
            if (!EnableDaySelection || !EnableMonthSelection || 
                (DisableTill != DateTime.MinValue && GetDisableMonth() == selectedMonth && DisableTill.Year == selectedYear)
                || (selectedMonth.ToLower() == "january" && (!enableYearSelection || !years.Contains(selectedYear - 1))))
            {
                isEnable =  false;
            }
            log.LogMethodExit(isEnable);
            return isEnable;
        }
        private void SetTimePickerValues()
        {
            log.LogMethodEntry();
            DateTime dateTime = DateTime.Now;
            if (aMorPM == AMorPM.None)
            {
                AMorPM = dateTime.ToString("tt", CultureInfo.InvariantCulture).ToLower() == "am".ToLower() ? AMorPM.AM : AMorPM.PM;
            }
            SetRadioButtonValue();
            if (string.IsNullOrEmpty(SelectedHour))
            {
                SelectedHour = dateTime.Hour > 12 ? (dateTime.Hour - 12).ToString("D2") : dateTime.Hour.ToString("D2");
            }
            if (string.IsNullOrEmpty(SelectedMinute))
            {
                SelectedMinute = dateTime.Minute.ToString("D2");
            }
            log.LogMethodExit();
        }        
        private void OnCancelClicked()
        {
            log.LogMethodEntry();
            if (datePickerView != null)
            {
                datePickerView.SelectedDate = string.Empty;
                datePickerView.Close();
            }
            log.LogMethodExit();
        }
        private bool PerformValidation()
        {
            log.LogMethodEntry();
            bool valid = true;
            if (editableMonthYear)
            {
                if (selectedMonth == null || datePickerView.cmbMonth.Text.ToLower() != selectedMonth.ToLower())
                {
                    valid = false;
                    datePickerView.cmbMonth.ErrorState = true;
                }
                if (selectedYear <= 0 || datePickerView.cmbYear.Text.ToLower() != selectedYear.ToString().ToLower())
                {
                    valid = false;
                    datePickerView.cmbYear.ErrorState = true;
                }
            }
            if (showTimePicker)
            {
                if (selectedMinute == null || datePickerView.cmbMins.Text.ToLower() != selectedMinute.ToLower())
                {
                    valid = false;
                    datePickerView.cmbMins.ErrorState = true;
                }
                if (selectedHour == null || datePickerView.cmbHour.Text.ToLower() != selectedHour.ToLower())
                {
                    valid = false;
                    datePickerView.cmbHour.ErrorState = true;
                }
            }
            log.LogMethodExit(valid);
            return valid;
        }
        private void OnOkClicked()
        {
            log.LogMethodEntry();
            if (datePickerView != null)
            {
                if (!PerformValidation())
                {
                    return;
                }
                if (showTimePicker)
                {
                    datePickerView.SelectedDate = string.Format("{0}/{1}/{2} {3}:{4} {5}", selectedDate.ToString(), selectedMonth, selectedYear, selectedHour, selectedMinute,
                        (bool)datePickerView.rdioAM.IsChecked ? "AM" : "PM");
                }
                else
                {
                    datePickerView.SelectedDate = string.Format("{0}/{1}/{2}", selectedDate.ToString(), selectedMonth, selectedYear);
                }
                datePickerView.Close();
            }
            log.LogMethodExit();
        }
        private void GetMonthWeeks()
        {
            log.LogMethodEntry();
            GetFirstWeek();
            GetWeek(currentMonthWeekLastDate, secondWeekDates);
            GetWeek(currentMonthWeekLastDate, thirdWeekDates);
            GetWeek(currentMonthWeekLastDate, fourthWeekDates);
            GetWeek(currentMonthWeekLastDate, fifthWeekDates);
            GetWeek(currentMonthWeekLastDate, sixthWeekDates);
            SetNavigationEnable();
            SetEnableOkButton();
            log.LogMethodExit();
        }
        private void GetFirstWeek()
        {
            log.LogMethodEntry();
            int previousMonthLastWeekFirstDay = 0;
            int previousMonthLastDay = 0;
            DateTime dateValue = new DateTime(selectedYear, months.IndexOf(selectedMonth) + 1, currentMonthFirstWeekFirstDate);
            Days firstday = (Days)Enum.Parse(typeof(Days), dateValue.ToString("ddd"), true);
            previousMonthLastDay = selectedMonth.ToLower() == "January".ToLower() ? DateTime.DaysInMonth(selectedYear - 1, months.IndexOf(selectedMonth) + 12)
                : DateTime.DaysInMonth(selectedYear, months.IndexOf(selectedMonth));
            if (firstWeekDates != null && firstWeekDates.Count > 0)
            {
                firstWeekDates.Clear();
            }
            switch (firstday)
            {
                case Days.SUN:
                    {
                        currentMonthWeekLastDate = 7;
                    }
                    break;
                case Days.MON:
                    {
                        previousMonthLastWeekFirstDay = previousMonthLastDay;
                        currentMonthWeekLastDate = 6;
                    }
                    break;
                case Days.TUE:
                    {
                        previousMonthLastWeekFirstDay = previousMonthLastDay - 1;
                        currentMonthWeekLastDate = 5;
                    }
                    break;
                case Days.WED:
                    {
                        previousMonthLastWeekFirstDay = previousMonthLastDay - 2;
                        currentMonthWeekLastDate = 4;
                    }
                    break;
                case Days.THU:
                    {
                        previousMonthLastWeekFirstDay = previousMonthLastDay - 3;
                        currentMonthWeekLastDate = 3;
                    }
                    break;
                case Days.FRI:
                    {
                        previousMonthLastWeekFirstDay = previousMonthLastDay - 4;
                        currentMonthWeekLastDate = 2;
                    }
                    break;
                case Days.SAT:
                    {
                        previousMonthLastWeekFirstDay = previousMonthLastDay - 5;
                        currentMonthWeekLastDate = 1;
                    }
                    break;
            }
            if (firstday != Days.SUN)
            {
                for (int i = previousMonthLastWeekFirstDay; i <= previousMonthLastDay; i++)
                {
                    string month = GetMonth();
                    firstWeekDates.Add(new DayDetails((Days)i - previousMonthLastWeekFirstDay, i.ToString("D2"), month, selectedYear, false, false, this));
                }
            }
            for (int i = currentMonthFirstWeekFirstDate; i <= currentMonthWeekLastDate; i++)
            {
                bool isEnabled = IsDateEnabled(i);
                firstWeekDates.Add(new DayDetails((Days)i - 1, i.ToString("D2"), selectedMonth, selectedYear, isEnabled, IsSelectedDate(i), this));
            }
            log.LogMethodExit();
        }
        private void GetWeek(int previousWeekLastDate, ObservableCollection<DayDetails> week)
        {
            log.LogMethodEntry();
            int selectedMonthLastDate = DateTime.DaysInMonth(selectedYear, months.IndexOf(selectedMonth) + 1);
            if (week.Any())
            {
                week.Clear();
            }
            for (int i = currentMonthWeekLastDate + 1; i <= currentMonthWeekLastDate + 7; i++)
            {
                if (i <= selectedMonthLastDate)
                {
                    bool isEnabled = IsDateEnabled(i);
                    week.Add(new DayDetails((Days)i - currentMonthWeekLastDate - 1, i.ToString("D2"), selectedMonth, selectedYear, isEnabled, IsSelectedDate(i), this));
                }
                else
                {
                    string month = GetMonth();
                    week.Add(new DayDetails((Days)i - currentMonthWeekLastDate - 1, (i - selectedMonthLastDate).ToString("D2"), month, selectedYear, false, false, this));
                }
            }
            currentMonthWeekLastDate += 7;
            log.LogMethodExit();
        }
        private string GetMonth()
        {
            log.LogMethodEntry();
            string month;
            switch (selectedMonth.ToLower())
            {
                case "january":
                    {
                        month = "December";
                    }
                    break;
                case "december":
                    {
                        month = "January";
                    }
                    break;
                default:
                    {
                        month = months[months.IndexOf(selectedMonth) - 1];
                    }
                    break;
            }
            return month;
        }
        private bool IsSelectedDate(int date)
        {
            log.LogMethodEntry();
            log.LogMethodExit();
            return selectedDate == date ? true : false;
        }
        private string GetDisableMonth()
        {
            log.LogMethodEntry();
            log.LogMethodExit(disableTill);
            return disableTill.ToString("MMMM");
        }
        private void SetRadioButtonValue()
        {
            log.LogMethodEntry();
            if (datePickerView != null)
            {
                switch (aMorPM)
                {
                    case AMorPM.AM:
                        if (datePickerView.rdioAM != null)
                        {
                            datePickerView.rdioAM.IsChecked = true;
                        }
                        break;
                    case AMorPM.PM:
                        if (datePickerView.rdioPM != null)
                        {
                            datePickerView.rdioPM.IsChecked = true;
                        }
                        break;
                    case AMorPM.None:
                        if (datePickerView.rdioAM != null)
                        {
                            datePickerView.rdioAM.IsChecked = false;
                        }
                        if (datePickerView.rdioPM != null)
                        {
                            datePickerView.rdioPM.IsChecked = false;
                        }
                        break;
                }
            }
            log.LogMethodExit();
        }
        private bool IsDateEnabled(int day)
        {
            log.LogMethodEntry(day);
            bool isNotAPastDate = false;
            if (EnableDaySelection)
            {
                isNotAPastDate = DisableTill != DateTime.MinValue &&
                day <= DisableTill.Date.Day && GetDisableMonth() == selectedMonth && DisableTill.Year == selectedYear ? false : true;
            }
            log.LogMethodExit(isNotAPastDate);
            return isNotAPastDate;
        }
        private void SetEnableOkButton()
        {
            log.LogMethodEntry();
            if(DisableTill != DateTime.MinValue && DisableTill.Date.Year <= selectedYear && GetDisableMonth() == selectedMonth
               && selectedDate <= DisableTill.Date.Day)
            {
                EnableOkButton = false;
            }
            else
            {
                EnableOkButton = true;
            }
            log.LogMethodExit();
        }
        #endregion

    }
}
