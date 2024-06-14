/********************************************************************************************
 * Project Name - CreditPlusConsumptionRules BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By            Remarks          
 *********************************************************************************************
 *2.70        01-Feb-2019      Indrajeet Kumar        Created 
 *            29-June-2019     Indrajeet Kumar        Added DeleteCreditPlusConsumptionRule() method.
 *            25-Sept-2019     Jagan Mohana           Added AuditLog code to Save() for saving the Audits to DB Table DBAuditLog
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using System.Data.SqlClient;
using Semnox.Parafait.GenericUtilities;

namespace Semnox.Parafait.Product
{
    class CreditPlusConsumptionRulesBL
    {
        private CreditPlusConsumptionRulesDTO creditPlusConsumptionRulesDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor with Parameter executionContext
        /// </summary>        
        /// <param name="executionContext"></param>
        public CreditPlusConsumptionRulesBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(creditPlusConsumptionRulesDTO, executionContext);
            this.executionContext = executionContext;
            this.creditPlusConsumptionRulesDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="creditPlusConsumptionRulesDTO"></param>
        /// <param name="executionContext"></param>
        public CreditPlusConsumptionRulesBL(CreditPlusConsumptionRulesDTO creditPlusConsumptionRulesDTO, ExecutionContext executionContext)
        {
            log.LogMethodEntry(creditPlusConsumptionRulesDTO, executionContext);
            this.executionContext = executionContext;
            this.creditPlusConsumptionRulesDTO = creditPlusConsumptionRulesDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Save
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            CreditPlusConsumptionRulesDataHandler creditPlusConsumptionRuleDataHandler = new CreditPlusConsumptionRulesDataHandler(sqlTransaction);
            if (creditPlusConsumptionRulesDTO.PKId < 0)
            {
                int PKId = creditPlusConsumptionRuleDataHandler.InsertCreditPlusConsumptionRules(creditPlusConsumptionRulesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                creditPlusConsumptionRulesDTO.PKId = PKId;
            }
            else
            {
                if (creditPlusConsumptionRulesDTO.PKId > 0 && creditPlusConsumptionRulesDTO.IsChanged == true)
                {
                    creditPlusConsumptionRuleDataHandler.UpdateCreditPlusConsumptionRules(creditPlusConsumptionRulesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    creditPlusConsumptionRulesDTO.AcceptChanges();
                }
            }
            /// Below code is to save the Audit log details into DBAuditLog
            creditPlusConsumptionRulesDTO = creditPlusConsumptionRuleDataHandler.GetCreditPlusConsumptionRulesDTO(creditPlusConsumptionRulesDTO.PKId);
            if (!string.IsNullOrEmpty(creditPlusConsumptionRulesDTO.Guid))
            {
                AuditLog auditLog = new AuditLog(executionContext);
                auditLog.AuditTable("ProductCreditPlusConsumption", creditPlusConsumptionRulesDTO.Guid, sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the CreditPlusConsumptionRule record.
        /// </summary>
        /// <param name="pkId"></param>
        /// <param name="sqlTransaction"></param>
        public void DeleteCreditPlusConsumptionRule(int pkId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(pkId);
            try
            {
                CreditPlusConsumptionRulesDataHandler creditPlusConsumptionRulesDataHandler = new CreditPlusConsumptionRulesDataHandler(sqlTransaction);
                creditPlusConsumptionRulesDataHandler.DeleteCreditPlusConsumptionRule(pkId);   
                log.LogMethodExit();
            }
            catch (ValidationException valEx)
            {
                log.Error(valEx);
                log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                throw;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }
    }

    public class CreditPlusConsumptionRulesBLList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<CreditPlusConsumptionRulesDTO> creditPlusConsumptionRulesList;
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public CreditPlusConsumptionRulesBLList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.creditPlusConsumptionRulesList = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="creditPlusConsumptionRulesList"></param>
        /// <param name="executionContext"></param>
        public CreditPlusConsumptionRulesBLList(List<CreditPlusConsumptionRulesDTO> creditPlusConsumptionRulesList, ExecutionContext executionContext)
        {
            log.LogMethodEntry(creditPlusConsumptionRulesList, executionContext);
            this.executionContext = executionContext;
            this.creditPlusConsumptionRulesList = creditPlusConsumptionRulesList;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get AllCreditPlus ConsumptionRules 
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<CreditPlusConsumptionRulesDTO> GetAllCreditPlusConsumptionRulesList(List<KeyValuePair<CreditPlusConsumptionRulesDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            CreditPlusConsumptionRulesDataHandler creditPlusConsumptionRulesDataHandler = new CreditPlusConsumptionRulesDataHandler(sqlTransaction);
            log.LogMethodExit();
            return creditPlusConsumptionRulesDataHandler.GetAllCreditPlusConsumptionRulesList(searchParameters);
        }

        /// <summary>
        /// Save Update CreditPlus Consumption Rules
        /// </summary>
        public void SaveUpdateCreditPlusConsumptionRulesList()
        {
            try
            {
                log.LogMethodEntry();
                if (creditPlusConsumptionRulesList != null)
                {
                    foreach (CreditPlusConsumptionRulesDTO creditPlusConsumptionRulesDTO in creditPlusConsumptionRulesList)
                    {
                        CreditPlusConsumptionRulesBL creditPlusConsumptionRuleObj = new CreditPlusConsumptionRulesBL(creditPlusConsumptionRulesDTO, executionContext);
                        creditPlusConsumptionRuleObj.Save();
                    }
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Gets the CreditPlusConsumptionRulesDTO List for scoringEventId Id List
        /// </summary>
        /// <param name="productCreditPlusIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of CreditPlusConsumptionRulesDTO</returns>
        public List<CreditPlusConsumptionRulesDTO> GetCreditPlusConsumptionRulesDTOList(List<int> productCreditPlusIdList, bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(productCreditPlusIdList, activeRecords, sqlTransaction);
            CreditPlusConsumptionRulesDataHandler creditPlusConsumptionRulesDataHandler = new CreditPlusConsumptionRulesDataHandler(sqlTransaction);
            List<CreditPlusConsumptionRulesDTO> creditPlusConsumptionRulesDTOList = creditPlusConsumptionRulesDataHandler.GetCreditPlusConsumptionRulesDTOList(productCreditPlusIdList, activeRecords);
            log.LogMethodExit(creditPlusConsumptionRulesDTOList);
            return creditPlusConsumptionRulesDTOList;
        }
    }
}
