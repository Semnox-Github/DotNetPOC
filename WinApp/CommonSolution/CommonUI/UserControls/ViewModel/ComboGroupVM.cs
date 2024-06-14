/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - Combo group View Model
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Raja Uthanda            Created for POS UI Redesign 
 ********************************************************************************************/
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Semnox.Parafait.CommonUI
{
    public class ComboGroupVM : ViewModelBase
    {
        #region Members
        private ObservableCollection<ComboBoxField> comboList;
        private ICommand selectedItemCommand;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public ICommand SelectedItemCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedItemCommand);
                return selectedItemCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref selectedItemCommand, value);
            }
        }

        public ObservableCollection<ComboBoxField> ComboList
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(comboList);
                return comboList;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref comboList, value);
            }
        }
        #endregion

        #region Constructors
        public ComboGroupVM()
        {
            log.LogMethodEntry();
            selectedItemCommand = new DelegateCommand(OnSelectionChanged);
            comboList = new ObservableCollection<ComboBoxField>();
            log.LogMethodExit();
        }
        #endregion

        #region Methods
        private void OnSelectionChanged(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                ComboBoxGroupSelectedItem selectedItem = parameter as ComboBoxGroupSelectedItem;
                if (selectedItem != null)
                {
                    selectedItem.ComboBoxGroupUserControl.RaiseSelectionChangedEvent(selectedItem.ComboBoxField);
                }
            }
            log.LogMethodExit();
        }
        #endregion
    }
}
