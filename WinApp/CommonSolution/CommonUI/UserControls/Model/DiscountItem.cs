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

namespace Semnox.Parafait.CommonUI
{
    public class DiscountItem : ViewModelBase
    {
        #region Member
        private string originalValue;
        private string discountValue;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public string OriginalValue
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(originalValue);
                return originalValue;
            }
            set
            {
                log.LogMethodEntry(originalValue, value);
                SetProperty(ref originalValue, value);
                log.LogMethodExit(originalValue);
            }
        }

        public string DiscountValue
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(discountValue);
                return discountValue;
            }
            set
            {
                log.LogMethodEntry(discountValue, value);
                SetProperty(ref discountValue, value);
                log.LogMethodExit(discountValue);
            }
        }
        #endregion

        #region Methods
        public DiscountItem()
        {
            log.LogMethodEntry();
            originalValue = string.Empty;
            discountValue = string.Empty;
            log.LogMethodExit();
        }

        public DiscountItem(string originalValue, string discountValue)
        {
            log.LogMethodEntry();
            this.originalValue = originalValue;
            this.discountValue = discountValue;
            log.LogMethodExit();
        }
        #endregion
    }
}
