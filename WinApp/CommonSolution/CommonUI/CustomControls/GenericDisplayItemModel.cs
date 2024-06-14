/********************************************************************************************
* Project Name - POS Redesign
* Description  - Common - model for generic display items
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
********************************************************************************************* 
*2.110.0     25-Nov-2020   Raja Uthanda            Created for POS UI Redesign 
********************************************************************************************/
using System.Collections.ObjectModel;

namespace Semnox.Parafait.CommonUI
{
    public class GenericDisplayItemModel : ViewModelBase
    {
        #region Members
        private string key;
        private string heading;
        private ObservableCollection<DiscountItem> itemsSource;
        private ButtonType buttonType;
        private bool isEnabled;
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

        public string Key
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(key);
                return key;
            }
            set
            {
                log.LogMethodEntry(key, value);
                SetProperty(ref key, value);
                log.LogMethodExit(key);
            }
        }

        public string Heading
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(heading);
                return heading;
            }
            set
            {
                log.LogMethodEntry(heading, value);
                SetProperty(ref heading, value);
                log.LogMethodExit(heading);
            }
        }

        public ObservableCollection<DiscountItem> ItemsSource
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(itemsSource);
                return itemsSource;
            }
            set
            {
                log.LogMethodEntry(itemsSource, value);
                SetProperty(ref itemsSource, value);
                log.LogMethodExit(itemsSource);
            }
        }

        public ButtonType ButtonType
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(buttonType);
                return buttonType;
            }
            set
            {
                log.LogMethodEntry(buttonType, value);
                SetProperty(ref buttonType, value);
                log.LogMethodExit(buttonType);
            }
        }
        #endregion

        #region Constructors
        public GenericDisplayItemModel()
        {
            log.LogMethodEntry();

            heading = string.Empty;
            itemsSource = new ObservableCollection<DiscountItem>();
            buttonType = ButtonType.None;
            isEnabled = true;

            log.LogMethodExit();
        }
        #endregion
    }
}
