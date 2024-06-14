/********************************************************************************************
 * Project Name - Promotions
 * Description  - Factory class to instantiate the promotion use cases 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00     05-Mar-2021      Roshan Devadiga        Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;


namespace Semnox.Parafait.Promotions
{
   public  class PromotionUseCaseFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ILoyaltyRedemptionRuleUseCases GetLoyaltyRedemptionRuleUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ILoyaltyRedemptionRuleUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteLoyaltyRedemptionRuleUseCases(executionContext);
            }
            else
            {
                result = new LocalLoyaltyRedemptionRuleUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
        public static ILoyaltyAttributeUseCases GetLoyaltyAttributeUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ILoyaltyAttributeUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteLoyaltyAttributeUseCases(executionContext);
            }
            else
            {
                result = new LocalLoyaltyAttributeUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// GetPromotionUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static IPromotionUseCases GetPromotionUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IPromotionUseCases result = null;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemotePromotionUseCases(executionContext);
            }
            else
            {
                result = new LocalPromotionUseCases(executionContext);
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
