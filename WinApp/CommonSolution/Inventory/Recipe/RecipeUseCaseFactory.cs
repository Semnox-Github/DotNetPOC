/********************************************************************************************
 * Project Name - Inventory
 * Description  - RecipeEstimationUseCaseFactory class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.00     16-Nov-2020         Abhishek             Created : POS UI Redesign with REST API
 ********************************************************************************************/

using System.Configuration;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Inventory.Recipe
{
    public class RecipeUseCaseFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static IRecipeEstimationUseCases GetRecipeEstimationUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IRecipeEstimationUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteRecipeEstimationUseCases(executionContext);
            }
            else
            {
                result = new LocalRecipeEstimationUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
        public static IRecipeManufacturingUseCases GetRecipeManufacturingUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IRecipeManufacturingUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteRecipeManufacturingUseCases(executionContext);
            }
            else
            {
                result = new LocalRecipeManufacturingUseCases(executionContext);
            }
            log.LogMethodExit(result);
            return result;
        }
        public static IAccountingCalendarUseCases GetAccountingCalendarUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IAccountingCalendarUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteAccountingCalendarUseCases(executionContext);
            }
            else
            {
                result = new LocalAccountingCalendarUseCases(executionContext);
            }
            log.LogMethodExit(result);
            return result;
        }
        public static IRecipePlanUseCases GetRecipePlanUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IRecipePlanUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteRecipePlanUseCases(executionContext);
            }
            else
            {
                result = new LocalRecipePlanUseCases(executionContext);
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
