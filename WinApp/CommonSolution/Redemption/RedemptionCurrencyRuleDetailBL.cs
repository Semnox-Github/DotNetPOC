/********************************************************************************************
 * Project Name - RedemptionCurrencyRuleDetail BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        19-Aug-2019      Dakshakh    Created 
 *2.110.0     08-Oct-2020   Mushahid Faizan     Added GetAllRedemptionCurrencyRuleDetailList() for pagination & modified as per standards.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Redemption
{
    /// <summary>
    /// Business logic for RedemptionCurrencyRuleDetailBL class.
    /// </summary>
    public class RedemptionCurrencyRuleDetailBL
    {
        private RedemptionCurrencyRuleDetailDTO redemptionCurrencyRuleDetailDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of RedemptionCurrencyRuleDetailBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private RedemptionCurrencyRuleDetailBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.redemptionCurrencyRuleDetailDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the redemptionCurrencyRuleDetail id as the parameter
        /// Would fetch the redemptionCurrencyRuleDetail object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="redemptionCurrencyRuleDetailId">RedemptionCurrencyRuleDetail id</param>
        /// <param name="sqltransaction">sqltransaction</param>
        public RedemptionCurrencyRuleDetailBL(ExecutionContext executionContext, int redemptionCurrencyRuleDetailId, SqlTransaction sqltransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(redemptionCurrencyRuleDetailId, executionContext, sqltransaction);
            RedemptionCurrencyRuleDetailDataHandler redemptionCurrencyRuleDetailDataHandler = new RedemptionCurrencyRuleDetailDataHandler(sqltransaction);
            this.redemptionCurrencyRuleDetailDTO = redemptionCurrencyRuleDetailDataHandler.GetRedemptionCurrencyRuleDetail(redemptionCurrencyRuleDetailId);
            if (redemptionCurrencyRuleDetailDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "redemptionCurrencyRuleDetail", redemptionCurrencyRuleDetailId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit(redemptionCurrencyRuleDetailDTO);
        }

        /// <summary>
        /// Creates RedemptionCurrencyRuleDetailBL object using the redemptionCurrencyRuleDetailDTO
        /// </summary>
        /// <param name="redemptionCurrencyRuleDetailDTO">RedemptionCurrencyRuleDetailDTO object</param>
        /// <param name="executionContext">executionContext</param>
        public RedemptionCurrencyRuleDetailBL(ExecutionContext executionContext, RedemptionCurrencyRuleDetailDTO redemptionCurrencyRuleDetailDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(this.redemptionCurrencyRuleDetailDTO, executionContext);
            this.redemptionCurrencyRuleDetailDTO = redemptionCurrencyRuleDetailDTO;
            log.LogMethodExit(this.redemptionCurrencyRuleDetailDTO);
        }

        /// <summary>
        /// Saves the redemptionCurrencyRuleDetail
        /// Checks if the redemptionCurrencyRuleDetail id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqltransaction">sqltransaction</param>
        public void Save(SqlTransaction sqltransaction = null)
        {
            log.LogMethodEntry(sqltransaction);
            Validate(sqltransaction);
            RedemptionCurrencyRuleDetailDataHandler redemptionCurrencyRuleDetailDataHandler = new RedemptionCurrencyRuleDetailDataHandler(sqltransaction);
            if (redemptionCurrencyRuleDetailDTO.RedemptionCurrencyRuleDetailId < 0)
            {
                redemptionCurrencyRuleDetailDTO = redemptionCurrencyRuleDetailDataHandler.InsertRedemptionCurrencyRuleDetail(redemptionCurrencyRuleDetailDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                redemptionCurrencyRuleDetailDTO.AcceptChanges();
            }
            else
            {
                if (redemptionCurrencyRuleDetailDTO.IsChanged)
                {
                    redemptionCurrencyRuleDetailDTO = redemptionCurrencyRuleDetailDataHandler.UpdateRedemptionCurrencyRuleDetail(redemptionCurrencyRuleDetailDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    redemptionCurrencyRuleDetailDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the redemptionCurrencyRuleDetail. returns validation errors if any of the field values not not valid.
        /// </summary>
        public void Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            string errorMessage = string.Empty;
            if (redemptionCurrencyRuleDetailDTO == null)
            {
                log.Error("Redemption currency rule details are missing");
                errorMessage = MessageContainerList.GetMessage(executionContext, 2244);//"Redemption currency rule details are missing"
                throw new ValidationException(errorMessage);
            }
            if (redemptionCurrencyRuleDetailDTO.RedemptionCurrencyRuleDetailId < 0 || redemptionCurrencyRuleDetailDTO.IsChanged)
            {
                if (redemptionCurrencyRuleDetailDTO.CurrencyId == -1)
                {
                    log.Error("Invalid redemption currency");
                    errorMessage = MessageContainerList.GetMessage(executionContext, 1389);
                    throw new ValidationException(errorMessage);
                }
                if (redemptionCurrencyRuleDetailDTO.Quantity == null || redemptionCurrencyRuleDetailDTO.Quantity <= 0)
                {
                    log.Error("Enter valid quantity for the redemption currency rule detail record");
                    errorMessage = MessageContainerList.GetMessage(executionContext, 2247);//"Enter valid quantity for the redemption currency rule detail record"
                    throw new ValidationException(errorMessage);
                }
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Gets the DTO
        /// </summary>
        public RedemptionCurrencyRuleDetailDTO GetRedemptionCurrencyRuleDetail { get { return redemptionCurrencyRuleDetailDTO; } }
    }

    /// <summary>
    /// Manages the list of RedemptionCurrencyRuleDetail
    /// </summary>
    public class RedemptionCurrencyRuleDetailListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<RedemptionCurrencyRuleDetailDTO> redemptionCurrencyRuleDetailDTOList = new List<RedemptionCurrencyRuleDetailDTO>();
        private ExecutionContext executionContext;

        /// <summary>
        ///  Default constructor of RedemptionCurrencyRuleDetail class 
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public RedemptionCurrencyRuleDetailListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// create the RedemptionCurrencyRuleDetail object
        /// </summary>
        /// <param name="redemptionCurrencyRuleDetailDTOList">redemptionCurrencyRuleDetailDTOList</param>
        /// <param name="executionContext">executionContext</param>
        public RedemptionCurrencyRuleDetailListBL(List<RedemptionCurrencyRuleDetailDTO> redemptionCurrencyRuleDetailDTOList, ExecutionContext executionContext) : this(executionContext)
        {
            log.LogMethodEntry(redemptionCurrencyRuleDetailDTOList, executionContext);
            this.redemptionCurrencyRuleDetailDTOList = redemptionCurrencyRuleDetailDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the redemptionCurrencyRuleDetail list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<RedemptionCurrencyRuleDetailDTO> GetAllRedemptionCurrencyRuleDetailList(
                        List<KeyValuePair<RedemptionCurrencyRuleDetailDTO.SearchByRedemptionCurrencyRuleParameters, string>> 
                        searchParameters, SqlTransaction sqlTransaction = null,
                        int currentPage = 0, int pageSize = 0)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            RedemptionCurrencyRuleDetailDataHandler redemptionCurrencyRuleDetailDataHandler = new RedemptionCurrencyRuleDetailDataHandler(sqlTransaction);
            List<RedemptionCurrencyRuleDetailDTO> redemptionCurrencyRuleDetailDTOList = redemptionCurrencyRuleDetailDataHandler.GetRedemptionCurrencyRuleDetailDTOList(searchParameters, currentPage, pageSize);
            log.LogMethodExit(redemptionCurrencyRuleDetailDTOList);
            return redemptionCurrencyRuleDetailDTOList;
        }

        /// <summary>
        /// Save and Updated the details
        /// </summary>        
        public void SaveList(SqlTransaction sqlTransaction)
        {
            try
            {
                log.LogMethodEntry();
                if (redemptionCurrencyRuleDetailDTOList != null && redemptionCurrencyRuleDetailDTOList.Any())
                {
                    foreach (RedemptionCurrencyRuleDetailDTO redemptionCurrencyRuleDetailDTO in redemptionCurrencyRuleDetailDTOList)
                    {
                        RedemptionCurrencyRuleDetailBL redemptionCurrencyRuleDetailBL = new RedemptionCurrencyRuleDetailBL(executionContext, redemptionCurrencyRuleDetailDTO);
                        redemptionCurrencyRuleDetailBL.Save(sqlTransaction);
                    }
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }

    }
}

