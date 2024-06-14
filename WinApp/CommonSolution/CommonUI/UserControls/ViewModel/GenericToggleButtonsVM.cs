/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - Generic toggle button View Model
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.110.0     25-Sep-2020   Raja Uthanda            modified for multi screen
 *2.120.0     09-Apr-2021   Raja Uthanda            avoid multiple on toggle event
 ********************************************************************************************/
using System.Linq;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace Semnox.Parafait.CommonUI
{
    public enum MultiScreenItemBackground
    {
        Grey,
        White
    }

    public class GenericToggleButtonsVM : ViewModelBase
    {
        #region Members
        private int columns;

        private bool verticalScrollable;
        private bool isSameItemSelected;
        private bool isVerticalOrientation;
        private bool isDefaultSelectionNeeded;

        private MultiScreenItemBackground multiScreenItemBackground;

        private ObservableCollection<CustomToggleButtonItem> toggleButtonItems;
        private CustomToggleButtonItem selectedToggleButtonItem;
        private GenericToggleButtonsUserControl toggleButtonsUserControl;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ICommand toggleButtonCheckedCommand;
        private ICommand toggleButtonUncheckedCommand;
        private ICommand loadedCommand;
        private ICommand toggleButtonLoadedCommand;
        #endregion

        #region Properties
        public bool IsDefaultSelectionNeeded
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(isDefaultSelectionNeeded);
                return isDefaultSelectionNeeded;
            }
            set
            {
                log.LogMethodEntry(isDefaultSelectionNeeded, value);
                SetProperty(ref isDefaultSelectionNeeded, value);
                log.LogMethodExit(isDefaultSelectionNeeded);
            }
        }
        public int Columns
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(columns);
                return columns;
            }
            set
            {
                log.LogMethodEntry(columns, value);
                SetProperty(ref columns, value);
                log.LogMethodExit(columns);
            }
        }
        public bool IsVerticalOrientation
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(isVerticalOrientation);
                return isVerticalOrientation;
            }
            set
            {
                log.LogMethodEntry(isVerticalOrientation, value);
                SetProperty(ref isVerticalOrientation, value);
                log.LogMethodExit(isVerticalOrientation);
            }
        }

        public CustomToggleButtonItem SelectedToggleButtonItem
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedToggleButtonItem);
                return selectedToggleButtonItem;
            }
            set
            {
                log.LogMethodEntry(selectedToggleButtonItem, value);
                SetProperty(ref selectedToggleButtonItem, value);
                log.LogMethodExit(selectedToggleButtonItem);
            }
        }

        public ICommand ToggleButtonUncheckedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(toggleButtonUncheckedCommand);
                return toggleButtonUncheckedCommand;
            }
            private set
            {
                log.LogMethodEntry(toggleButtonUncheckedCommand, value);
                SetProperty(ref toggleButtonUncheckedCommand, value);
                log.LogMethodExit(toggleButtonUncheckedCommand);
            }
        }

        public ICommand ToggleButtonCheckedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(toggleButtonCheckedCommand);
                return toggleButtonCheckedCommand;
            }
            private set
            {
                log.LogMethodEntry(toggleButtonCheckedCommand, value);
                SetProperty(ref toggleButtonCheckedCommand, value);
                log.LogMethodExit(toggleButtonCheckedCommand);
            }
        }

        public ICommand LoadedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(loadedCommand);
                return loadedCommand;
            }
            set
            {
                log.LogMethodEntry(loadedCommand, value);
                SetProperty(ref loadedCommand, value);
                log.LogMethodExit(loadedCommand);
            }
        }

        public ICommand ToggleButtonLoadedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(toggleButtonLoadedCommand);
                return toggleButtonLoadedCommand;
            }
            private set
            {
                log.LogMethodEntry(toggleButtonLoadedCommand, value);
                SetProperty(ref toggleButtonLoadedCommand, value);
                log.LogMethodExit(toggleButtonLoadedCommand);
            }
        }

        public MultiScreenItemBackground MultiScreenItemBackground
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(multiScreenItemBackground);
                return multiScreenItemBackground;
            }
            set
            {
                log.LogMethodEntry(multiScreenItemBackground, value);
                SetProperty(ref multiScreenItemBackground, value);
                log.LogMethodExit(multiScreenItemBackground);
            }
        }

        public ObservableCollection<CustomToggleButtonItem> ToggleButtonItems
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(toggleButtonItems);
                return toggleButtonItems;
            }
            set
            {
                log.LogMethodEntry(toggleButtonItems, value);
                SetProperty(ref toggleButtonItems, value);
                log.LogMethodExit(toggleButtonItems);
            }
        }

        #endregion

        #region Methods
        public void RaiseCanExecuteChanged()
        {
            log.LogMethodEntry();
            (toggleButtonCheckedCommand as DelegateCommand).RaiseCanExecuteChanged();
            (toggleButtonUncheckedCommand as DelegateCommand).RaiseCanExecuteChanged();
            log.LogMethodExit();
        }
        private void FindAncestor(Visual myVisual)
        {
            log.LogMethodEntry();
            if (myVisual == null)
            {
                return;
            }
            object visual = VisualTreeHelper.GetParent(myVisual);
            if (visual is GenericToggleButtonsUserControl)
            {
                toggleButtonsUserControl = visual as GenericToggleButtonsUserControl;
            }
            else
            {
                FindAncestor(visual as Visual);
            }
            log.LogMethodExit();
        }

        private void OnToggleChecked(object param)
        {
            log.LogMethodEntry(param);
            bool sameToggleButton = false;
            if (param != null && toggleButtonItems != null && toggleButtonItems.Count > 0)
            {
                CustomToggleButtonItem toggleButtonItem = param as CustomToggleButtonItem;
                if (toggleButtonItem != null)
                {
                    int index = toggleButtonItems.IndexOf(toggleButtonItem);

                    if (index != -1)
                    {
                        for (int i = 0; i < toggleButtonItems.Count; i++)
                        {
                            if (i != index && toggleButtonItems[i].IsChecked)
                            {
                                toggleButtonItems[i].IsChecked = false;
                            }
                        }
                        if (SelectedToggleButtonItem != toggleButtonItem)
                        {
                            SelectedToggleButtonItem = toggleButtonItem;
                        }
                        else
                        {
                            sameToggleButton = true;
                        }
                    }
                }
            }
            if (toggleButtonsUserControl != null && !isSameItemSelected && !sameToggleButton)
            {
                toggleButtonsUserControl.RaiseToggleCheckedEvent();
            }
            log.LogMethodExit();
        }

        private void OnToggleUnchecked(object param)
        {
            log.LogMethodEntry();
            if (param != null && toggleButtonItems != null && toggleButtonItems.Count > 0
                && toggleButtonItems.All(t => t.IsChecked == false))
            {
                CustomToggleButtonItem selectedToggleButtonItem = param as CustomToggleButtonItem;
                if (selectedToggleButtonItem != null)
                {
                    isSameItemSelected = true;
                    selectedToggleButtonItem.IsChecked = true;
                    SelectedToggleButtonItem = selectedToggleButtonItem;
                }
            }
            if (toggleButtonsUserControl != null && !isSameItemSelected)
            {
                toggleButtonsUserControl.RaiseToggleUncheckedEvent();
            }
            isSameItemSelected = false;
            log.LogMethodExit();
        }

        private void OnLoaded(object param)
        {
            log.LogMethodEntry();
            if (param != null)
            {
                toggleButtonsUserControl = param as GenericToggleButtonsUserControl;
                SetSelectedItem();
            }
            log.LogMethodExit();
        }

        private void SetSelectedItem()
        {
            if (toggleButtonItems != null && toggleButtonItems.Count > 0 && selectedToggleButtonItem == null && IsDefaultSelectionNeeded)
            {
                selectedToggleButtonItem = toggleButtonItems.Where(t => t.IsChecked == true).FirstOrDefault();
                if (selectedToggleButtonItem == null)
                {
                    selectedToggleButtonItem = toggleButtonItems.Where(t => t.IsEnabled == true).FirstOrDefault();
                    selectedToggleButtonItem.IsChecked = true;
                }
            }
        }


        private void OnToggleButtonLoaded(object param)
        {
            if (toggleButtonsUserControl == null)
            {
                FindAncestor(param as CustomToggleButton);
                SetSelectedItem();
            }
            if (param != null && selectedToggleButtonItem != null)
            {
                CustomToggleButtonItem toggleButtonItem = (param as CustomToggleButton).DataContext as CustomToggleButtonItem;
                if (toggleButtonItem != null && selectedToggleButtonItem.Equals(toggleButtonItem))
                {
                    if (toggleButtonsUserControl != null)
                    {
                        toggleButtonsUserControl.RaiseToggleCheckedEvent();
                    }
                }
            }
        }
        #endregion

        #region Constructors
        public GenericToggleButtonsVM()
        {
            log.LogMethodEntry();
            columns = 1;
            toggleButtonItems = new ObservableCollection<CustomToggleButtonItem>();
            selectedToggleButtonItem = null;
            isSameItemSelected = false;
            isVerticalOrientation = false;
            isDefaultSelectionNeeded = true;
            toggleButtonCheckedCommand = new DelegateCommand(OnToggleChecked, ButtonEnable);
            toggleButtonUncheckedCommand = new DelegateCommand(OnToggleUnchecked, ButtonEnable);
            loadedCommand = new DelegateCommand(OnLoaded);
            toggleButtonLoadedCommand = new DelegateCommand(OnToggleButtonLoaded);
            multiScreenItemBackground = MultiScreenItemBackground.Grey;
            log.LogMethodExit();
        }
        #endregion
    }
}
