/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Display tags view model
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Raja Uthanda           Created for POS UI Redesign 
 *2.110.0     25-Nov-2020   Raja Uthanda           Modified for interactive tag
 ********************************************************************************************/
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Semnox.Parafait.CommonUI
{
    public class DisplayTagsVM : ViewModelBase
    {
        #region Members
        private bool stretch;

        private ObservableCollection<ObservableCollection<DisplayTag>> displayTags;
        private DisplayTagsUserControl displayTagsUserControl;

        private ICommand itemClickedCommand;
        private ICommand loadedCommand;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties

        public ICommand ItemClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(itemClickedCommand);
                return itemClickedCommand;
            }
            set
            {
                log.LogMethodEntry(itemClickedCommand, value);
                SetProperty(ref itemClickedCommand, value);
                log.LogMethodExit(itemClickedCommand);
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

        public ObservableCollection<ObservableCollection<DisplayTag>> DisplayTags
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(displayTags);
                return displayTags;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref displayTags, value);
                log.LogMethodExit(displayTags);
            }
        }

        public bool Stretch
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(stretch);
                return stretch;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref stretch, value);
                log.LogMethodExit(stretch);
            }
        }

        #endregion

        #region Methods
        private void FindAncestor(Visual myVisual)
        {
            log.LogMethodEntry();
            if (myVisual == null)
            {
                return;
            }
            object visual = VisualTreeHelper.GetParent(myVisual);
            if (visual is DisplayTagsUserControl)
            {
                displayTagsUserControl = visual as DisplayTagsUserControl;
            }
            else
            {
                FindAncestor(visual as Visual);
            }
            log.LogMethodExit();
        }

        private void OnItemClicked(object parameter)
        {
            log.LogMethodEntry();
            if (displayTagsUserControl == null)
            {
                FindAncestor(parameter as Visual);
            }
            if (parameter != null)
            {
                Border border = parameter as Border;
                if (border != null)
                {
                    ObservableCollection<DisplayTag> selectedItem = border.DataContext as ObservableCollection<DisplayTag>;
                    if (selectedItem != null && selectedItem.Any(s => s.Type == DisplayTagType.Button)
                        && displayTagsUserControl != null)
                    {
                        displayTagsUserControl.RaiseItemClickedEvent();
                    }
                }
            }
            log.LogMethodExit();
        }


        private void OnLoaded(object parameter)
        {
            log.LogMethodEntry();
            if (parameter != null)
            {
                DisplayTagsUserControl displayTagsControl = parameter as DisplayTagsUserControl;
                if (displayTagsControl != null)
                {
                    displayTagsUserControl = displayTagsControl;
                }
            }
            log.LogMethodExit();
        }
        #endregion

        #region Constructors
        public DisplayTagsVM()
        {
            log.LogMethodEntry();

            displayTags = new ObservableCollection<ObservableCollection<DisplayTag>>();
            stretch = false;

            itemClickedCommand = new DelegateCommand(OnItemClicked);
            loadedCommand = new DelegateCommand(OnLoaded);

            log.LogMethodExit();
        }

        #endregion
    }
}
