/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - Model for combobox group
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Raja Uthanda            Created for POS UI Redesign 
 ********************************************************************************************/
using System.Collections.ObjectModel;


namespace Semnox.Parafait.CommonUI
{
    public class ComboBoxField : ViewModelBase
    {
        #region Members
        private string propertyName;
        private string _header;
        private string _selectedItem;
        private ObservableCollection<string> _items;
        private bool _isReadOnly;
        private bool _isEditable;
        private Size _size;
        private bool _isEnabled;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public string PropertyName
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(propertyName);
                return propertyName;
            }
            set
            {
                log.LogMethodEntry(propertyName, value);
                SetProperty(ref propertyName, value);
                log.LogMethodExit(propertyName);
            }
        }

        public string Header
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(_header);
                return _header;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref _header, value);
            }
        }

        public string SelectedItem
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(_selectedItem);
                return _selectedItem;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref _selectedItem, value);
            }
        }

        public ObservableCollection<string> Items
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(_items);
                return _items;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref _items, value);
            }
        }

        public bool IsReadOnly
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(_isReadOnly);
                return _isReadOnly;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref _isReadOnly, value);
            }
        }

        public bool IsEditable
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(_isEditable);
                return _isEditable;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref _isEditable, value);
            }
        }

        public Size Size
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(_size);
                return _size;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref _size, value);
            }
        }

        public bool IsEnabled
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(_isEnabled);
                return _isEnabled;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref _isEnabled, value);
            }
        }
        #endregion

        #region Contructors
        public ComboBoxField()
        {
            log.LogMethodEntry();
            _header = "";
            _selectedItem = null;
            _items = new ObservableCollection<string>();
            _isReadOnly = false;
            _isEditable = false;
            _size = Size.Small;
            _isEnabled = true;
            log.LogMethodExit();
        }
        #endregion
    }
}
