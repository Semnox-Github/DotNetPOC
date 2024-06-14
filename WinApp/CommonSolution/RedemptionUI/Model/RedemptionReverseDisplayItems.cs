/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Redemption - model for redemption reverse display items
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.110.0     25-Nov-2020   Raja Uthanda            Created for POS UI Redesign 
 ********************************************************************************************/
using System.Collections.ObjectModel;
using Semnox.Parafait.CommonUI;

namespace Semnox.Parafait.RedemptionUI
{
    public class RedemptionReverseDisplayItems : ViewModelBase
    {
        #region Members
        private bool isCheckboxEnable;
        private int id;
        private ObservableCollection<string> displayredemptionItems;
        private bool selectedCheckbox;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public int Id
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(id);
                return id;
            }
            set
            {
                log.LogMethodEntry(id, value);
                SetProperty(ref id, value);
                log.LogMethodExit(id);
            }
        }

        public ObservableCollection<string> DisplayRedemptionItems
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(displayredemptionItems);
                return displayredemptionItems;
            }
            set
            {
                log.LogMethodEntry(displayredemptionItems, value);
                SetProperty(ref displayredemptionItems, value);
                log.LogMethodExit(displayredemptionItems);
            }
        }

        public bool IsCheckboxEnable
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(isCheckboxEnable);
                return isCheckboxEnable;
            }
            set
            {
                log.LogMethodEntry(isCheckboxEnable, value);
                SetProperty(ref isCheckboxEnable, value);
                log.LogMethodExit(isCheckboxEnable);
            }
        }

        public bool SelectedCheckbox
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedCheckbox);
                return selectedCheckbox;
            }
            set
            {
                log.LogMethodEntry(selectedCheckbox, value);
                if (isCheckboxEnable)
                {
                    SetProperty(ref selectedCheckbox, value);
                }
                else if (SelectedCheckbox)
                {
                    SelectedCheckbox = false;
                }
                log.LogMethodExit(selectedCheckbox);
            }
        }
        #endregion

        #region Methods
        #endregion

        #region Constructor
        public RedemptionReverseDisplayItems()
        {
            log.LogMethodEntry();
            id = 0;
            displayredemptionItems = new ObservableCollection<string>();
            selectedCheckbox = false;
            isCheckboxEnable = true;
            log.LogMethodExit();
        }
        #endregion
    }
}
