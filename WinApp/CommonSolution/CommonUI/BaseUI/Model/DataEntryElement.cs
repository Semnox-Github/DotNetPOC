/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - model for generic data entry
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Raja Uthanda            Created for POS UI Redesign 
 ********************************************************************************************/
using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Semnox.Parafait.CommonUI
{
    public class DataEntryElement : ViewModelBase
    {
        #region Members
        private int maxLength;
        private int numericUpDownvalue;
        private int numericUpDownMaxvalue;
        private int numericUpDownMinvalue;

        private bool isMasked;
        private bool isChecked;
        private bool isEnabled;
        private bool isEditable;
        private bool isReadOnly;
        private bool errorState;
        private bool isMandatory;
        private bool showTimePicker;
        private bool disablePastDates;

        private string text;
        private string heading;
        private string defaultValue;
        private string errorMessage;
        private string customerFieldName;
        private string customerGroupName;
        private string displayMemberPath;
        private string customerSubGroupName;

        private Size size;
        private DataEntryType type;
        private Visibility errorVisibility;
        private ValidationType validationType;        
        private NumericButtonType numericButtonType;
        private NumberKeyboardType numberKeyboardType;
        private DatePickerValidationType datePickerValidationType;

        private object selectedItem;
        private ObservableCollection<object> options;
        private List<string> displayMemberPathCollection;

        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Properties
        public NumberKeyboardType NumberKeyboardType
        {
            get { return numberKeyboardType; }
            set
            {
                SetProperty(ref numberKeyboardType, value);
            }
        }
        public string CustomerFieldName
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(customerFieldName);
                return customerFieldName;
            }
            set
            {
                log.LogMethodEntry(customerFieldName, value);
                SetProperty(ref customerFieldName, value);
                log.LogMethodExit(customerFieldName);
            }
        }
        public string CustomerGroupName
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(customerGroupName);
                return customerGroupName;
            }
            set
            {
                log.LogMethodEntry(customerGroupName, value);
                SetProperty(ref customerGroupName, value);
                log.LogMethodExit(customerGroupName);
            }
        }
        public string CustomerSubGroupName
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(customerSubGroupName);
                return customerSubGroupName;
            }
            set
            {
                log.LogMethodEntry(customerSubGroupName, value);
                SetProperty(ref customerSubGroupName, value);
                log.LogMethodExit(customerSubGroupName);
            }
        }
        public int MaxLength
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(maxLength);
                return maxLength;
            }
            set
            {
                log.LogMethodEntry(maxLength, value);
                SetProperty(ref maxLength, value);
                log.LogMethodExit(maxLength);
            }
        }
        public int NumericUpDownValue
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(numericUpDownvalue);
                return numericUpDownvalue;
            }
            set
            {
                log.LogMethodEntry(numericUpDownvalue, value);
                SetProperty(ref numericUpDownvalue, value);
                log.LogMethodExit(numericUpDownvalue);
            }
        }

        public int NumericUpDownMaxValue
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(numericUpDownMaxvalue);
                return numericUpDownMaxvalue;
            }
            set
            {
                log.LogMethodEntry(numericUpDownMaxvalue, value);
                SetProperty(ref numericUpDownMaxvalue, value);
                log.LogMethodExit(numericUpDownMaxvalue);
            }
        }

        public int NumericUpDownMinValue
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(numericUpDownMinvalue);
                return numericUpDownMinvalue;
            }
            set
            {
                log.LogMethodEntry(numericUpDownMinvalue, value);
                SetProperty(ref numericUpDownMinvalue, value);
                log.LogMethodExit(numericUpDownMinvalue);
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
        public bool DisablePastDates
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(disablePastDates);
                return disablePastDates;
            }
            set
            {
                log.LogMethodEntry(disablePastDates, value);
                SetProperty(ref disablePastDates, value);
                log.LogMethodExit(disablePastDates);
            }
        }
        public DatePickerValidationType DatePickerValidationType
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(datePickerValidationType);
                return datePickerValidationType;
            }
            set
            {
                log.LogMethodEntry(datePickerValidationType, value);
                SetProperty(ref datePickerValidationType, value);
                log.LogMethodExit(datePickerValidationType);
            }
        }
        public NumericButtonType NumericButtonType
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(numericButtonType);
                return numericButtonType;
            }
            set
            {
                log.LogMethodEntry(numericButtonType, value);
                SetProperty(ref numericButtonType, value);
                log.LogMethodExit(numericButtonType);
            }
        }

        public string DisplayMemberPath
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(displayMemberPath);
                return displayMemberPath;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref displayMemberPath, value);
                log.LogMethodExit(displayMemberPath);
            }
        }

        public bool IsEnabled
        {
            get
            {
                log.LogMethodEntry();
                return isEnabled;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref isEnabled, value);
            }
        }

        public bool IsReadOnly
        {
            get
            {
                log.LogMethodEntry();
                return isReadOnly;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref isReadOnly, value);
            }
        }

        public bool IsMasked
        {
            get
            {
                log.LogMethodEntry();
                return isMasked;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref isMasked, value);
            }
        }

        public string DefaultValue
        {
            get
            {
                log.LogMethodEntry();
                return defaultValue;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref defaultValue, value);
            }
        }

        public Visibility ErrorVisibility
        {
            get
            {
                log.LogMethodEntry();
                return errorVisibility;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref errorVisibility, value);
            }
        }

        public string ErrorMessage
        {
            get
            {
                log.LogMethodEntry();
                return errorMessage;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref errorMessage, value);
            }
        }

        public ValidationType ValidationType
        {
            get
            {
                log.LogMethodEntry();
                return validationType;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref validationType, value);
            }
        }

        public string Heading
        {
            get
            {
                log.LogMethodEntry();
                return heading;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref heading, value);
            }
        }

        public Size Size
        {
            get
            {
                log.LogMethodEntry();
                return size;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref size, value);
            }
        }

        public object SelectedItem
        {
            get
            {
                log.LogMethodEntry();
                return selectedItem;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref selectedItem, value);
            }
        }

        public string Text
        {
            get
            {
                log.LogMethodEntry();
                return text;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref text, value);
            }
        }
        public List<string> DisplayMemberPathCollection
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(displayMemberPathCollection);
                return displayMemberPathCollection;
            }
            set
            {
                log.LogMethodEntry(displayMemberPathCollection, value);
                SetProperty(ref displayMemberPathCollection, value);
                log.LogMethodExit(displayMemberPathCollection);
            }
        }
        public ObservableCollection<object> Options
        {
            get
            {
                log.LogMethodEntry();
                return options;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref options, value);
            }
        }
        public bool IsChecked
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(isChecked);
                return isChecked;
            }
            set
            {
                log.LogMethodEntry(isChecked, value);
                SetProperty(ref isChecked, value);
                log.LogMethodExit(isChecked);
            }
        }
        public bool ErrorState
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(errorState);
                return errorState;
            }
            set
            {
                log.LogMethodEntry(errorState, value);
                SetProperty(ref errorState, value);
                log.LogMethodExit(errorState);
            }
        }
        public bool IsMandatory
        {
            get
            {
                log.LogMethodEntry();
                return isMandatory;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref isMandatory, value);
            }
        }

        public bool IsEditable
        {
            get
            {
                log.LogMethodEntry();
                return isEditable;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref isEditable, value);
            }
        }

        public DataEntryType Type
        {
            get
            {
                log.LogMethodEntry();
                return type;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref type, value);
            }
        }

        #endregion

        #region Constructors and Finalizers
        public DataEntryElement()
        {
            log.LogMethodEntry();

            isEnabled = true;
            isEditable = true;
            isReadOnly = false;
            isMasked = false;
            isMandatory = false;
            isChecked = false;
            errorState = false;
            showTimePicker = false;
            disablePastDates = false;

            displayMemberPath = string.Empty;
            defaultValue = string.Empty;
            errorMessage = string.Empty;
            heading = string.Empty;
            text = string.Empty;
            customerFieldName = string.Empty;

            displayMemberPathCollection = null;

            size = Size.Small;
            type = DataEntryType.TextBox;
            validationType = ValidationType.None;
            errorVisibility = Visibility.Collapsed;
            numberKeyboardType = NumberKeyboardType.Positive;
            datePickerValidationType = DatePickerValidationType.None;

            options = new ObservableCollection<object>();

            numericButtonType = NumericButtonType.None;

            maxLength = int.MaxValue;

            numericUpDownvalue = 0;
            numericUpDownMaxvalue = int.MaxValue;
            numericUpDownMinvalue = int.MinValue;

            log.LogMethodExit();
        }
        #endregion
    }
}
