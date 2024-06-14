/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - model for date picker
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Raja Uthanda            Created for POS UI Redesign 
 ********************************************************************************************/
using System;
using System.Windows.Input;

namespace Semnox.Parafait.CommonUI
{
    class DayDetails : ViewModelBase
    {
        #region Members
        private DatePickerVM _datePickerVM;
        private ICommand _selectDateCommand;
        private bool _isChecked;
        private Days _day;
        private string _date;
        private string _month;
        private int _year;
        private bool _isEnabled;
        #endregion

        #region Constructors & Finalizers
        public DayDetails(Days day, string date, string month, int year, bool isEnabled, bool isChecked, DatePickerVM datePickerViewModel)
        {
            _day = day;
            _date = date;
            _month = month;
            _year = year;
            _isEnabled = isEnabled;
            _isChecked = isChecked;
            _datePickerVM = datePickerViewModel;
        }
        #endregion

        #region Properties
        public DatePickerVM DatePickerVM
        {
            get
            {
                return _datePickerVM;
            }
            set
            {
                _datePickerVM = value;
            }
        }

        public bool Checked
        {
            get
            {
                return _isChecked;
            }
            set
            {
                SetProperty(ref _isChecked, value);
            }
        }

        public Days Day
        {
            get
            {
                return _day;
            }
            set
            {
                _day = value;
            }
        }

        public string Date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
            }
        }

        public string Month
        {
            get
            {
                return _month;
            }
            set
            {
                _month = value;
            }
        }

        public int Year
        {
            get
            {
                return _year;
            }
            set
            {
                _year = value;
            }
        }

        public bool Enabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                _isEnabled = value;
            }
        }

        public ICommand SelectDateCommand
        {
            get
            {
                if (_selectDateCommand == null)
                    _selectDateCommand = new DelegateCommand(OnDateSelected);
                return _selectDateCommand;
            }
            set
            {
                _selectDateCommand = value;

            }
        }
        #endregion

        #region Methods
        private void OnDateSelected(object obj)
        {
            if (obj != null)
            {
                DayDetails dateAndDay = obj as DayDetails;
                if (dateAndDay != null && dateAndDay.DatePickerVM != null)
                {
                    dateAndDay.DatePickerVM.IsFromDateChange = true;
                    dateAndDay.DatePickerVM.SelectedDate = Int32.Parse(dateAndDay.Date);
                    dateAndDay.DatePickerVM.IsFromDateChange = false;
                }
            }
        }
        #endregion
    }
}
