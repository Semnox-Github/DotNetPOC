/********************************************************************************************
 * Project Name - Game Audit Business layer                                                                          
 * Description  - Business layer of the Game Audit class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.40        18-Sep-2018   Rajiv          Created files to pull the audit values for machine, game and gaem_profile.
 *********************************************************************************************
 *2.60        07-Mar-2019   Akshay Gulaganji    Added Constructor and ExecutionContext and modified GetAuditList(auditId,tableName)
 *2.60        18-Mar-2019   Akshay Gulaganji    Modified CaseNames for ProductsSetup/ProductGames/ProductCreditPlus/ProductCreditPlusConsumption as of Lookup/Localization case names 
 *2.60.2      27-May-2019   Mehraj              Added the new AuditTable() for saving the audit details
 *            27-May-2019   Jagan Mohana        Added the new parameter to the AuditTable()
 **********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.GenericUtilities
{
    public class AuditLog
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Constructor to initialize the executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public AuditLog(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the game Audit list.
        /// </summary>
        /// <param name="auditId"></param>
        /// <param name="entityName"></param>
        /// <returns>List<GameAuditDTO></returns>
        public List<List<List<string>>> GetAuditList(int auditId, string entityName)
        {
            log.LogMethodEntry(auditId, entityName);
            List<KeyValuePair<AuditLogParams.SearchByAuditParameters, string>> searchParameters = new List<KeyValuePair<AuditLogParams.SearchByAuditParameters, string>>();
            try
            {
                List<List<List<string>>> auditList = new List<List<List<string>>>();
                string query = String.Empty;
                switch (entityName.ToUpper())
                {
                    case "GAMES":
                        searchParameters.Add(new KeyValuePair<AuditLogParams.SearchByAuditParameters, string>(AuditLogParams.SearchByAuditParameters.TABLE_NAME, "GAMES"));
                        searchParameters.Add(new KeyValuePair<AuditLogParams.SearchByAuditParameters, string>(AuditLogParams.SearchByAuditParameters.FIELD_NAME, "game_id"));
                        break;
                    case "MACHINES":
                        searchParameters.Add(new KeyValuePair<AuditLogParams.SearchByAuditParameters, string>(AuditLogParams.SearchByAuditParameters.TABLE_NAME, "MACHINES"));
                        searchParameters.Add(new KeyValuePair<AuditLogParams.SearchByAuditParameters, string>(AuditLogParams.SearchByAuditParameters.FIELD_NAME, "machine_id"));
                        break;
                    case "GAME_PROFILE":
                        searchParameters.Add(new KeyValuePair<AuditLogParams.SearchByAuditParameters, string>(AuditLogParams.SearchByAuditParameters.TABLE_NAME, "GAME_PROFILE"));
                        searchParameters.Add(new KeyValuePair<AuditLogParams.SearchByAuditParameters, string>(AuditLogParams.SearchByAuditParameters.FIELD_NAME, "game_profile_id"));
                        break;
                    case "MASTERS":
                        searchParameters.Add(new KeyValuePair<AuditLogParams.SearchByAuditParameters, string>(AuditLogParams.SearchByAuditParameters.TABLE_NAME, "MASTERS"));
                        searchParameters.Add(new KeyValuePair<AuditLogParams.SearchByAuditParameters, string>(AuditLogParams.SearchByAuditParameters.FIELD_NAME, "master_id"));
                        break;
                    case "PRODUCTSSETUP":
                        searchParameters.Add(new KeyValuePair<AuditLogParams.SearchByAuditParameters, string>(AuditLogParams.SearchByAuditParameters.TABLE_NAME, "PRODUCTS"));
                        searchParameters.Add(new KeyValuePair<AuditLogParams.SearchByAuditParameters, string>(AuditLogParams.SearchByAuditParameters.FIELD_NAME, "product_id"));
                        break;
                    case "PRODUCTGAMESENTITLEMENTS":
                        searchParameters.Add(new KeyValuePair<AuditLogParams.SearchByAuditParameters, string>(AuditLogParams.SearchByAuditParameters.TABLE_NAME, "ProductGames"));
                        searchParameters.Add(new KeyValuePair<AuditLogParams.SearchByAuditParameters, string>(AuditLogParams.SearchByAuditParameters.FIELD_NAME, "product_game_id"));
                        break;
                    case "PRODUCTCREDITPLUS":
                        searchParameters.Add(new KeyValuePair<AuditLogParams.SearchByAuditParameters, string>(AuditLogParams.SearchByAuditParameters.TABLE_NAME, "ProductCreditPlus"));
                        searchParameters.Add(new KeyValuePair<AuditLogParams.SearchByAuditParameters, string>(AuditLogParams.SearchByAuditParameters.FIELD_NAME, "ProductCreditPlusId"));
                        break;
                    case "PRODUCTCREDITPLUSCONSUMPTION":
                        searchParameters.Add(new KeyValuePair<AuditLogParams.SearchByAuditParameters, string>(AuditLogParams.SearchByAuditParameters.TABLE_NAME, "ProductCreditPlusConsumption"));
                        searchParameters.Add(new KeyValuePair<AuditLogParams.SearchByAuditParameters, string>(AuditLogParams.SearchByAuditParameters.FIELD_NAME, "PKId"));
                        break;
                    case "PRICELISTPRODUCTS":
                        searchParameters.Add(new KeyValuePair<AuditLogParams.SearchByAuditParameters, string>(AuditLogParams.SearchByAuditParameters.TABLE_NAME, "PriceListProducts"));
                        searchParameters.Add(new KeyValuePair<AuditLogParams.SearchByAuditParameters, string>(AuditLogParams.SearchByAuditParameters.FIELD_NAME, "PriceListProductId"));
                        break;
                }
                if (searchParameters.Count > 0)
                {
                    searchParameters.Add(new KeyValuePair<AuditLogParams.SearchByAuditParameters, string>(AuditLogParams.SearchByAuditParameters.AUDIT_ID, auditId.ToString()));
                    searchParameters.Add(new KeyValuePair<AuditLogParams.SearchByAuditParameters, string>(AuditLogParams.SearchByAuditParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    AuditLogDataHandler auditLogDataHandler = new AuditLogDataHandler(searchParameters);
                    auditList = auditLogDataHandler.GetAuditList();
                }
                log.LogMethodExit(auditList);
                return auditList;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Save audit log details for all modules
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="guid"></param>
        public void AuditTable(string tableName, string guid, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(tableName, guid);
            AuditLogDataHandler auditLogDataHandler = new AuditLogDataHandler(sqlTransaction);
            auditLogDataHandler.AuditTable(tableName, guid, executionContext.GetUserId(),executionContext.GetSiteId());
            log.LogMethodExit();
        }
    }
}
