/********************************************************************************************
 * Project Name - Promotions
 * Description  - Business logic file for  CampaignCustomer
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70      24-Jun-2019     Girish Kundar           Created 
 *2.70      04-Sep-2019     Mushahid Faizan         Added GetPhoneCount(), GetEmailCount()
 *                                                  UpdateCampaignCustomerRecordSMS() & UpdateCampaignCustomerRecordEmail methods.
 * 2.80     18-Nov-2019     Rakesh                  Added GetCampaignCustomerDTOList and GetCustomersInCampaign method    
 * 2.80     09-Apr-2020     Mushahid Faizan         Modified : 3 tier changes for Rest API.
 * 2.90     07-Sep-2020     Girish Kundar           Modified : Added SendCampaignSMS() and SendCampaignEmail() methods
 *2.100.0   15-Sep-2020     Nitin Pai               Push Notification: Generate notification message count, removed sendemail and sendsms methods
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer.Accounts;

namespace Semnox.Parafait.Promotions
{
    /// <summary>
    /// Business logic for CampaignCustomer class.
    /// </summary>
    public class CampaignCustomerBL
    {
        private CampaignCustomerDTO campaignCustomerDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of CampaignCustomer class
        /// </summary>
        private CampaignCustomerBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates CampaignCustomerBL object using the CampaignCustomerDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="campaignCustomerDTO">CampaignCustomerDTO object</param>
        public CampaignCustomerBL(ExecutionContext executionContext, CampaignCustomerDTO campaignCustomerDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, campaignCustomerDTO);
            this.campaignCustomerDTO = campaignCustomerDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the CampaignCustomer id as the parameter
        /// Would fetch the CampaignCustomer object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="id">id - AppUIPanel</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public CampaignCustomerBL(ExecutionContext executionContext, int id,  SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            CampaignCustomerDataHandler campaignCustomerDataHandler = new CampaignCustomerDataHandler(sqlTransaction);
            campaignCustomerDTO = campaignCustomerDataHandler.GetCampaignCustomerDTO(id);
            if (campaignCustomerDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "CampaignCustomer", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the CampaignCustomer
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (campaignCustomerDTO.IsChanged == false
                && campaignCustomerDTO.CampaignId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            CampaignCustomerDataHandler campaignCustomerDataHandler = new CampaignCustomerDataHandler(sqlTransaction);
            if (campaignCustomerDTO.IsActive == true)
            {
                List<ValidationError> validationErrors = Validate();
                if (validationErrors.Any())
                {
                    string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                    log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                    throw new ValidationException(message, validationErrors);
                }
                if (campaignCustomerDTO.Id < 0)
                {
                    log.LogVariableState("CampaignCustomerDTO", campaignCustomerDTO);
                    campaignCustomerDTO = campaignCustomerDataHandler.Insert(campaignCustomerDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    campaignCustomerDTO.AcceptChanges();
                }
                else if (campaignCustomerDTO.IsChanged)
                {
                    log.LogVariableState("CampaignCustomerDTO", campaignCustomerDTO);
                    campaignCustomerDTO = campaignCustomerDataHandler.Update(campaignCustomerDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    campaignCustomerDTO.AcceptChanges();
                }

            }
            else  
            {

                if (campaignCustomerDTO.Id >= 0)
                {
                    campaignCustomerDataHandler.Delete(campaignCustomerDTO);
                }
                campaignCustomerDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the campaignCustomerDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            // List of values to be validated for each DTO .
            // Like if Balance== -1 or Id = null etc.
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public CampaignCustomerDTO CampaignCustomerDTO
        {
            get
            {
                return campaignCustomerDTO;
            }
        }

    }

    /// <summary>
    /// Manages the list of CampaignCustomer
    /// </summary>
    public class CampaignCustomerListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<CampaignCustomerDTO> campaignCustomerDTOList = new List<CampaignCustomerDTO>(); // To be initialized
        private string passPhrase;

        /// <summary>
        /// Parameterized constructor of CampaignCustomerListBL
        /// </summary>
        /// <param name="executionContext">executionContext object</param>
        public CampaignCustomerListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            passPhrase = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_ENCRYPTION_PASS_PHRASE");
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="campaignCustomerDTOList">CampaignCustomer DTO List as parameter </param>
        public CampaignCustomerListBL(ExecutionContext executionContext,
                                               List<CampaignCustomerDTO> campaignCustomerDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, campaignCustomerDTOList);
            this.campaignCustomerDTOList = campaignCustomerDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Returns the Get the CampaignCustomer DTO list based on the search parameter.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>The List of CampaignCustomerDTO </returns>
        public List<CampaignCustomerDTO> GetCampaignCustomerDTOList(List<KeyValuePair<CampaignCustomerDTO.SearchByParameters, string>> searchParameters,
                                                               SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            CampaignCustomerDataHandler campaignCustomerDataHandler = new CampaignCustomerDataHandler(sqlTransaction);
            List<CampaignCustomerDTO> campaignCustomerDTOList = campaignCustomerDataHandler.GetCampaignCustomerDTOList(searchParameters);
            log.LogMethodExit(campaignCustomerDTOList);
            return campaignCustomerDTOList;
        }
        public int GetPhoneCount(int campaignId)
        {
            log.LogMethodEntry(campaignId);
            DataTable dtPhone;
            int rowCount = 0;
            CampaignCustomerDataHandler campaignCustomerDataHandler = new CampaignCustomerDataHandler();
            dtPhone = campaignCustomerDataHandler.GetPhoneCount(campaignId, passPhrase);
            rowCount = dtPhone.Rows.Count;
            log.LogMethodExit(rowCount);
            return rowCount;
        }
        /// <summary>
        /// Returns the no of Customers emailCount matching the search Parameters
        /// </summary>
        /// <param name="campaignId">campaignId</param>
        /// <returns></returns>
        public int GetEmailCount(int campaignId)
        {
            log.LogMethodEntry(campaignId);
            DataTable dtEmails;
            int rowCount = 0;
            CampaignCustomerDataHandler campaignCustomerDataHandler = new CampaignCustomerDataHandler();
            dtEmails = campaignCustomerDataHandler.GetEmailCount(campaignId, passPhrase);
            rowCount = dtEmails.Rows.Count;
            log.LogMethodExit(rowCount);
            return rowCount;
        }

        /// <summary>
        /// Returns the no of Customers emailCount matching the search Parameters
        /// </summary>
        /// <param name="campaignId">campaignId</param>
        /// <returns></returns>
        public int GetNotificationCount(int campaignId)
        {
            log.LogMethodEntry(campaignId);
            DataTable dtEmails;
            int rowCount = 0;
            CampaignCustomerDataHandler campaignCustomerDataHandler = new CampaignCustomerDataHandler();
            dtEmails = campaignCustomerDataHandler.GetNotificationCount(campaignId, passPhrase);
            rowCount = dtEmails.Rows.Count;
            log.LogMethodExit(rowCount);
            return rowCount;
        }
        
        /// <summary>
        /// Saves the  list of CampaignCustomer DTO.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (campaignCustomerDTOList == null ||
                campaignCustomerDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < campaignCustomerDTOList.Count; i++)
            {
                var campaignCustomerDTO = campaignCustomerDTOList[i];
                if (campaignCustomerDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    CampaignCustomerBL campaignCustomerBL = new CampaignCustomerBL(executionContext, campaignCustomerDTO);
                    campaignCustomerBL.Save(sqlTransaction);
                }
                catch (SqlException sqlEx)
                {
                    log.Error(sqlEx);
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
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving CampaignCustomerDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("CampaignCustomerDTO", campaignCustomerDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
        public List<CampaignCustomerDTO> GetCampaignCustomerDTOList(string membership = null, string totalAddPoint = null, string totalSpend = null,
                                                      string totalAddPointOperator = null, string totalSpendOperator = null, string birthDayInDays = null,
                                                      CampaignCustomerSearchCriteria advCustomerCriteria = null, AccountSearchCriteria advCardCriteria = null)
        {
            log.LogMethodEntry(membership, totalAddPoint, totalSpend, totalAddPointOperator, totalSpendOperator, birthDayInDays, advCustomerCriteria, advCardCriteria);
            CampaignCustomerDataHandler campaignCustomerDataHandler = new CampaignCustomerDataHandler();
            log.LogMethodExit();
            return campaignCustomerDataHandler.GetCampaignCustomerDTOList(passPhrase, membership, totalAddPoint, totalSpend, totalAddPointOperator,
                                                                            totalSpendOperator, birthDayInDays, advCustomerCriteria, advCardCriteria);
        }
        public List<CampaignCustomerDTO> GetCustomersInCampaign(int campaignId)
        {
            log.LogMethodEntry(campaignId);
            CampaignCustomerDataHandler campaignCustomerDataHandler = new CampaignCustomerDataHandler();
            log.LogMethodExit();
            return campaignCustomerDataHandler.GetCustomersInCampaign(campaignId,passPhrase);
        }

    }
}
