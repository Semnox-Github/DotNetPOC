/********************************************************************************************
 * Project Name - Product
 * Description  - ProductUseCaseFactory class
 *
 **************
 ** Version Log
  **************
  * Version      Date              Modified By     Remarks
 *********************************************************************************************
 2.110.0         01-Dec-2020       Deeksha         Created : Web Inventory Design with REST API
 2.110.0         25-Jan-2021       Guru S A        For Subscription changes
 2.130.0         21-Jul-2021       Mushahid Faizan     Modified : POS UI Redesign changes
 2.150.0         18-May-2022       Abhishek        Modified: Added GetCustomerProfileGroupsUseCases for Customer Profile
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using System.Configuration;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// ProductsUseCaseFactory
    /// </summary>
    public class ProductsUseCaseFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// GetProductUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static IProductsUseCases GetProductUseCases(ExecutionContext executionContext, string requestGuid = null)
        {
            log.LogMethodEntry(executionContext, requestGuid);
            IProductsUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteProductsUseCases(executionContext, requestGuid);
            }
            else
            {
                result = new LocalProductsUseCases(executionContext, requestGuid);
            }

            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// GetProductSubscriptionUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static IProductSubscriptionUseCases GetProductSubscriptionUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IProductSubscriptionUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteProductSubscriptionUseCases(executionContext);
            }
            else
            {
                result = new LocalProductSubscriptionUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        } 
        
        /// <summary>
        /// GetOrderTypeUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static IOrderTypeUseCases GetOrderTypeUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IOrderTypeUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteOrderTypeUseCases(executionContext);
            }
            else
            {
                result = new LocalOrderTypeUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// GetProductsCalenderUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>

        public static IProductsCalenderUseCases GetProductsCalenderUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IProductsCalenderUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteProductsCalenderUseCases(executionContext);
            }
            else
            {
                result = new LocalProductsCalenderUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// GetProductGamesUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>

        public static IProductGamesUseCases GetProductGamesUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IProductGamesUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteProductGamesUseCases(executionContext);
            }
            else
            {
                result = new LocalProductGamesUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// GetUpsellOfferUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>

        public static IUpsellOfferUseCases GetUpsellOfferUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IUpsellOfferUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteUpsellOfferUseCases(executionContext);
            }
            else
            {
                result = new LocalUpsellOfferUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        ///GetProductDiscountUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static IProductDiscountUseCases GetProductDiscountUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IProductDiscountUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteProductDiscountUseCases(executionContext);
            }
            else
            {
                result = new LocalProductDiscountUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///GetFacilityUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static IFacilityUseCases GetFacilityUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IFacilityUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteFacilityUseCases(executionContext);
            }
            else
            {
                result = new LocalFacilityUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        ///GetFacilityMapUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static IFacilityMapUseCases GetFacilityMapUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IFacilityMapUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteFacilityMapUseCases(executionContext);
            }
            else
            {
                result = new LocalFacilityMapUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        ///GetMasterScheduleUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static IMasterScheduleUseCases GetMasterScheduleUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IMasterScheduleUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteMasterScheduleUseCases(executionContext);
            }
            else
            {
                result = new LocalMasterScheduleUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// GetSaleGroupProductMapUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>

        public static ISaleGroupProductMapUseCases GetSaleGroupProductMapUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ISaleGroupProductMapUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteSaleGroupProductMapUseCases(executionContext);
            }
            else
            {
                result = new LocalSaleGroupProductMapUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// GetModifierSetUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>

        public static IModifierSetUseCases GetModifierSetUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IModifierSetUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteModifierSetUseCases(executionContext);
            }
            else
            {
                result = new LocalModifierSetUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// GetOrderTypeUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>


        public static IOrderTypeGroupUseCases GetOrderTypeGroupUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IOrderTypeGroupUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteOrderTypeGroupUseCases(executionContext);
            }
            else
            {
                result = new LocalOrderTypeGroupUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// GetProductsAllowedInFacilityMaps
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static IProductsAllowedInFacilityMapUseCases GetProductsAllowedInFacilityMaps(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IProductsAllowedInFacilityMapUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteProductsAllowedInFacilityMapUseCases(executionContext);
            }
            else
            {
                result = new LocalProductsAllowedInFacilityMapUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// GetProductsDisplayGroups
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static IProductDisplayGroupUseCases GetProductsDisplayGroups(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IProductDisplayGroupUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteProductsDisplayGroupUseCases(executionContext);
            }
            else
            {
                result = new LocalProductsDisplayGroupUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// GetProductsSpecialPricings
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static IProductsSpecialPricingUseCases GetProductsSpecialPricings(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IProductsSpecialPricingUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteProductsSpecialPricingUseCases(executionContext);
            }
            else
            {
                result = new LocalProductsSpecialPricingUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// GetSalesOfferGroups
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static ISalesOfferGroupUseCases GetSalesOfferGroups(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ISalesOfferGroupUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteSalesOfferGroupUseCases(executionContext);
            }
            else
            {
                result = new LocalSalesOfferGroupUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// GetBOMs
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static IBOMUseCases GetBOMs(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IBOMUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteBOMUseCases(executionContext);
            }
            else
            {
                result = new LocalBOMUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// GetCustomerProfilesUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static IProductsUseCases GetCustomerProfileGroupsUseCases(ExecutionContext executionContext, string requestGuid = null)
        {
            log.LogMethodEntry(executionContext);
            IProductsUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteProductsUseCases(executionContext, requestGuid);
            }
            else
            {
                result = new LocalProductsUseCases(executionContext, requestGuid);
            }

            log.LogMethodExit(result);
            return result;
        }

    }
}
