/********************************************************************************************
* Project Name - POS Redesign
* Description  - Custom toggle button item model
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
********************************************************************************************* 
*2.110.0     25-Nov-2020   Raja Uthanda           Created for POS UI Redesign 
 ********************************************************************************************/

using System.Collections.ObjectModel;
using System.Windows;
namespace Semnox.Parafait.CommonUI
{
    public class CustomToggleButtonItem : ViewModelBase
    {
        #region Members
        private ObservableCollection<DisplayTag> displayTags;
        private bool isEnabled;
        private bool isChecked;
        private string key;
        private VerticalAlignment verticalAlignment;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public bool IsEnabled
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(isEnabled);
                return isEnabled;
            }
            set
            {
                log.LogMethodEntry(isEnabled, value);
                SetProperty(ref isEnabled, value);
                log.LogMethodExit(isEnabled);
            }
        }
        public VerticalAlignment VerticalAlignment
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(verticalAlignment);
                return verticalAlignment;
            }
            set
            {
                log.LogMethodEntry(verticalAlignment, value);
                SetProperty(ref verticalAlignment, value);
                log.LogMethodExit(verticalAlignment);
            }
        }
        public string Key
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(isChecked);
                return key;
            }
            set
            {
                log.LogMethodEntry(key, value);
                SetProperty(ref key, value);
                log.LogMethodExit(key);
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

        public ObservableCollection<DisplayTag> DisplayTags
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(displayTags);
                return displayTags;
            }
            set
            {
                log.LogMethodEntry(displayTags, value);
                SetProperty(ref displayTags, value);
                log.LogMethodExit(displayTags);
            }
        }
        #endregion

        #region Constructors
        public CustomToggleButtonItem()
        {
            log.LogMethodEntry();
            displayTags = new ObservableCollection<DisplayTag>();
            isChecked = false;
            isEnabled = true;
            key = string.Empty;
            verticalAlignment = VerticalAlignment.Top;
            log.LogMethodExit();
        }
        #endregion

    }
}
