/********************************************************************************************
 * Project Name - Customer.Accounts
 * Description  - Class for  of AccountSearchParams    
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019   Girish kundar  Modified : Added Logger Methods and Removed Unused namespace's.
 ********************************************************************************************/

namespace Semnox.Parafait.Customer.Accounts
{
    public class AccountSearchParams
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public AccountSearchParams()
        {
            log.LogMethodEntry();
            AccountId = -1;
            TagNumber = string.Empty;
            ValidFlag = string.Empty;
            PageNumber = 0;
            PageSize = 20;
            log.LogMethodExit();
        }
        public int AccountId { get; set; }

        public string TagNumber { get; set; }

        public string ValidFlag { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }
}
