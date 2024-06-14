/********************************************************************************************
 * Project Name - Promotions
 * Description  - Business logic file for  LoyaltyRedemptionRule
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.3     24-Jun-2019     Girish Kundar           Created 
 *2.80       19-May-2020     Mushahid Faizan         Modified : Added Delete for WMS requirement.
 *2.120.0    26-Mar-2021     Fiona                   Modified for container changes to add GetLoyaltyRedemptionRuleLastUpdateTime 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Promotions
{
    /// <summary>
    /// Business logic for LoyaltyRedemptionRule class.
    /// </summary>
    public class LoyaltyRedemptionRuleBL
    {
        private LoyaltyRedemptionRuleDTO loyaltyRedemptionRuleDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of LoyaltyRedemptionRule class
        /// </summary>
        private LoyaltyRedemptionRuleBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates LoyaltyRedemptionRuleBL object using the LoyaltyRedemptionRuleDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="loyaltyRedemptionRuleDTO">LoyaltyRedemptionRule DTO object</param>
        public LoyaltyRedemptionRuleBL(ExecutionContext executionContext, LoyaltyRedemptionRuleDTO loyaltyRedemptionRuleDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, loyaltyRedemptionRuleDTO);
            this.loyaltyRedemptionRuleDTO = loyaltyRedemptionRuleDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the loyaltyRedemptionRule  id as the parameter
        /// Would fetch the loyaltyRedemptionRule object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="id">id -LloyaltyRedemptionRule </param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public LoyaltyRedemptionRuleBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            LoyaltyRedemptionRuleDataHandler loyaltyRedemptionRuleDataHandler = new LoyaltyRedemptionRuleDataHandler(sqlTransaction);
            loyaltyRedemptionRuleDTO = loyaltyRedemptionRuleDataHandler.GetLoyaltyRedemptionRuleDTO(id);
            if (loyaltyRedemptionRuleDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "LoyaltyRedemptionRule", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the LoyaltyRedemptionRule
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (loyaltyRedemptionRuleDTO.IsChanged == false
                && loyaltyRedemptionRuleDTO.RedemptionRuleId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            LoyaltyRedemptionRuleDataHandler loyaltyRedemptionRuleDataHandler = new LoyaltyRedemptionRuleDataHandler(sqlTransaction);
            Validate(sqlTransaction);
            if (loyaltyRedemptionRuleDTO.RedemptionRuleId < 0)
            {
                log.LogVariableState("LoyaltyRedemptionRuleDTO", loyaltyRedemptionRuleDTO);
                loyaltyRedemptionRuleDTO = loyaltyRedemptionRuleDataHandler.Insert(loyaltyRedemptionRuleDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                loyaltyRedemptionRuleDTO.AcceptChanges();
            }
            else if (loyaltyRedemptionRuleDTO.IsChanged)
            {
                log.LogVariableState("LoyaltyRedemptionRuleDTO", loyaltyRedemptionRuleDTO);
                loyaltyRedemptionRuleDTO = loyaltyRedemptionRuleDataHandler.Update(loyaltyRedemptionRuleDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                loyaltyRedemptionRuleDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the loyaltyRedemptionRuleDTO records from database 
        /// This method is only used for Web Management Studio.
        /// </summary>
        public void Delete(LoyaltyRedemptionRuleDTO loyaltyRedemptionRuleDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(loyaltyRedemptionRuleDTO, sqlTransaction);
            try
            {
                LoyaltyRedemptionRuleDataHandler loyaltyRedemptionRuleDataHandler = new LoyaltyRedemptionRuleDataHandler(sqlTransaction);
                loyaltyRedemptionRuleDataHandler.Delete(loyaltyRedemptionRuleDTO);
                loyaltyRedemptionRuleDTO.AcceptChanges();
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Validates the loyaltyRedemptionRuleDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public void Validate(SqlTransaction sqlTransaction = null)
        {
            // List of values to be validated for each DTO .
            // Like if Balance== -1 or Id = null etc.
            // Validation do here
            if (loyaltyRedemptionRuleDTO.LoyaltyPoints > 0 && loyaltyRedemptionRuleDTO.VirtualPoints > 0)
            {
                // both can not be set for one rule
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4024));
            }
            if (loyaltyRedemptionRuleDTO.LoyaltyPoints <= 0 && loyaltyRedemptionRuleDTO.VirtualPoints <= 0)
            {
                // Any one value mandatory
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4025));
            }
            if (loyaltyRedemptionRuleDTO.LoyaltyPoints < 0)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Loyalty Points")));
            }
            if (loyaltyRedemptionRuleDTO.VirtualPoints < 0)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Virtual Points")));
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public LoyaltyRedemptionRuleDTO LoyaltyRedemptionRuleDTO
        {
            get
            {
                return loyaltyRedemptionRuleDTO;
            }
        }

    }

    /// <summary>
    /// Manages the list of LoyaltyRedemptionRule
    /// </summary>
    public class LoyaltyRedemptionRuleListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<LoyaltyRedemptionRuleDTO> loyaltyRedemptionRuleDTOList = new List<LoyaltyRedemptionRuleDTO>(); // To be initialized
        /// <summary>
        /// Parameterized constructor of LoyaltyRedemptionRuleListBL
        /// </summary>
        /// <param name="executionContext">executionContext object</param>
        public LoyaltyRedemptionRuleListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="loyaltyRedemptionRuleDTOList">LoyaltyRedemptionRule DTO List as parameter </param>
        public LoyaltyRedemptionRuleListBL(ExecutionContext executionContext,
                                               List<LoyaltyRedemptionRuleDTO> loyaltyRedemptionRuleDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, loyaltyRedemptionRuleDTOList);
            this.loyaltyRedemptionRuleDTOList = loyaltyRedemptionRuleDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Returns the Get the LoyaltyRedemptionRule DTO list based on the search parameter.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>The List of LoyaltyRedemptionRule </returns>
        public List<LoyaltyRedemptionRuleDTO> GetLoyaltyRedemptionRuleList(List<KeyValuePair<LoyaltyRedemptionRuleDTO.SearchByParameters, string>> searchParameters,
                                                              SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            LoyaltyRedemptionRuleDataHandler loyaltyRedemptionRuleDataHandler = new LoyaltyRedemptionRuleDataHandler(sqlTransaction);
            List<LoyaltyRedemptionRuleDTO> loyaltyRedemptionRuleDTOList = loyaltyRedemptionRuleDataHandler.GetLoyaltyRedemptionRuleDTOList(searchParameters);
            log.LogMethodExit(loyaltyRedemptionRuleDTOList);
            return loyaltyRedemptionRuleDTOList;
        }

        /// <summary>
        /// Saves the  list of LoyaltyRedemptionRule  DTO.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (loyaltyRedemptionRuleDTOList == null ||
                loyaltyRedemptionRuleDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < loyaltyRedemptionRuleDTOList.Count; i++)
            {
                var loyaltyRedemptionRuleDTO = loyaltyRedemptionRuleDTOList[i];
                if (loyaltyRedemptionRuleDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    LoyaltyRedemptionRuleBL loyaltyRedemptionRuleBL = new LoyaltyRedemptionRuleBL(executionContext, loyaltyRedemptionRuleDTO);
                    loyaltyRedemptionRuleBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving LoyaltyRedemptionRuleDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("LoyaltyRedemptionRuleDTO", loyaltyRedemptionRuleDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the loyaltyRedemptionRuleDTOList
        /// This method is only used for Web Management Studio.
        /// </summary>
        public void Delete()
        {
            log.LogMethodEntry();
            if (loyaltyRedemptionRuleDTOList != null && loyaltyRedemptionRuleDTOList.Any())
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        foreach (LoyaltyRedemptionRuleDTO loyaltyRedemptionRuleDTO in loyaltyRedemptionRuleDTOList)
                        {
                            if (loyaltyRedemptionRuleDTO.IsChanged)
                            {
                                LoyaltyRedemptionRuleBL loyaltyRedemptionRuleBL = new LoyaltyRedemptionRuleBL(executionContext, loyaltyRedemptionRuleDTO);
                                loyaltyRedemptionRuleBL.Delete(loyaltyRedemptionRuleDTO, parafaitDBTrx.SQLTrx);
                            }
                        }
                        parafaitDBTrx.EndTransaction();
                    }
                    catch (SqlException sqlEx)
                    {
                        log.Error(sqlEx);
                        parafaitDBTrx.RollBack();
                        log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                        if (sqlEx.Number == 547)
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                        }
                        else
                        {
                            throw;
                        }
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
                        parafaitDBTrx.RollBack();
                        log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                        throw;
                    }
                }
                log.LogMethodExit();
            }
        }
        public DateTime? GetLoyaltyRedemptionRuleLastUpdateTime(int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(siteId, sqlTransaction);
            LoyaltyRedemptionRuleDataHandler loyaltyRedemptionRuleDataHandler = new LoyaltyRedemptionRuleDataHandler(sqlTransaction);
            DateTime? result = loyaltyRedemptionRuleDataHandler.GetLoyaltyRedemptionRuleLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }

        public decimal GetRedemptionValueForEntilementType(decimal virtualPoints, string entitlementType, bool isVirtualPoint, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(virtualPoints, entitlementType, sqlTransaction);
            decimal redemptionValue = 0m;
            LoyaltyRedemptionRuleDataHandler loyaltyRedemptionRuleDataHandler = new LoyaltyRedemptionRuleDataHandler(sqlTransaction);
            DataTable redemptionRulesData = loyaltyRedemptionRuleDataHandler.GetEntitlementValue(virtualPoints, isVirtualPoint);
            if (redemptionRulesData.Rows.Count > 0)
            {
                switch (entitlementType)
                {
                    case "T":
                        {
                            foreach (DataRow dataRow in redemptionRulesData.Rows)
                            {
                                if (dataRow[0].ToString() == "tickets")
                                {
                                    redemptionValue = Convert.ToDecimal(dataRow["Redemption_Value"]);
                                    log.Debug("Redemption_Value : " + redemptionValue);
                                    break;
                                }
                            }
                        }
                        break;
                    case "A":
                    case "L":
                    case "G":
                    case "B":
                    case "M":
                    case "V":
                        {
                            log.Debug(" Not Yet implemented");
                        }
                        break;
                    default:
                        {
                            log.Error("Invalid  entitlementType");
                        }
                        break;
                }
            }
            else
            {
                log.Debug("loyalty rules are not defined . Can not load the entitlement to the card");
            }
            log.LogMethodExit();
            return redemptionValue;
        }
    }
}
