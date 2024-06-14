

/********************************************************************************************
 * Project Name - Advance search 
 * Description  - Bussiness logic of the Advanced search
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *2.60.2      29-May-2019   Jagan Mohana    Created
 ********************************************************************************************/
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Customer
{
    public class AdvancedSearchBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public AdvancedSearchBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        
    }
}