/********************************************************************************************
 * Project Name - PayConfigurations
 * Description  - Business logic file for  PayConfigurations 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.90.0      06-Jul-2020   Akshay Gulaganji        Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// Business logic for PayConfigurations class.
    /// </summary>
    public class PayConfigurationsBL
    {
        private PayConfigurationsDTO payConfigurationsDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of PayConfigurationsBL class
        /// </summary>
        private PayConfigurationsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates PayConfigurationsBL object using the PayConfigurationsDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="payConfigurationsDTO">PayConfigurationsDTO object</param>
        public PayConfigurationsBL(ExecutionContext executionContext, PayConfigurationsDTO payConfigurationsDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, payConfigurationsDTO);
            this.payConfigurationsDTO = payConfigurationsDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the PayConfiguration Id as the parameter
        /// Would fetch the PayConfigurations object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="payConfigurationId">id - payConfigurationId</param>
        /// <param name="loadChildRecords">loadChildRecords either true or false</param>
        /// <param name="activeChildRecords">activeChildRecords either true or false</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public PayConfigurationsBL(ExecutionContext executionContext, int payConfigurationId, bool loadChildRecords = true,
            bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, payConfigurationId, sqlTransaction);
            PayConfigurationsDataHandler payConfigurationsDataHandler = new PayConfigurationsDataHandler(sqlTransaction);
            payConfigurationsDTO = payConfigurationsDataHandler.GetPayConfigurationsDTO(payConfigurationId);
            if (payConfigurationsDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "PayConfigurationsDTO", payConfigurationId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the child records for PayConfigurationsDTO object.
        /// </summary>
        /// <param name="activeChildRecords">activeChildRecords holds either true or false</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        private void Build(bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            PayConfigurationDetailsListBL payConfigurationDetailsListBL = new PayConfigurationDetailsListBL(executionContext);
            List<KeyValuePair<PayConfigurationDetailsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PayConfigurationDetailsDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<PayConfigurationDetailsDTO.SearchByParameters, string>(PayConfigurationDetailsDTO.SearchByParameters.PAY_CONFIGURATION_ID, payConfigurationsDTO.PayConfigurationId.ToString()));
            searchParameters.Add(new KeyValuePair<PayConfigurationDetailsDTO.SearchByParameters, string>(PayConfigurationDetailsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            if (activeChildRecords)
            {
                searchParameters.Add(new KeyValuePair<PayConfigurationDetailsDTO.SearchByParameters, string>(PayConfigurationDetailsDTO.SearchByParameters.IS_ACTIVE, "1"));
            }
            payConfigurationsDTO.PayConfigurationDetailsDTOList = payConfigurationDetailsListBL.GetPayConfigurationDetailsDTOList(searchParameters, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the PayConfigurationsDTO
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);

            if (payConfigurationsDTO.IsChangedRecursive == false
                   && payConfigurationsDTO.PayConfigurationId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            PayConfigurationsDataHandler payConfigurationsDataHandler = new PayConfigurationsDataHandler(sqlTransaction);
            if (payConfigurationsDTO.PayConfigurationId < 0)
            {
                log.LogVariableState("PayConfigurationsDTO", payConfigurationsDTO);
                payConfigurationsDTO = payConfigurationsDataHandler.Insert(payConfigurationsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                payConfigurationsDTO.AcceptChanges();
            }
            else if (payConfigurationsDTO.IsChanged)
            {
                log.LogVariableState("PayConfigurationsDTO", payConfigurationsDTO);
                payConfigurationsDTO = payConfigurationsDataHandler.Update(payConfigurationsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                payConfigurationsDTO.AcceptChanges();
            }
            SavePayConfigurationsChild(sqlTransaction);
        }

        /// <summary>
        /// Saves the child records : PayConfigurationDetailDTO List
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        private void SavePayConfigurationsChild(SqlTransaction sqlTransaction)
        {
            if (payConfigurationsDTO.PayConfigurationDetailsDTOList != null &&
                payConfigurationsDTO.PayConfigurationDetailsDTOList.Any())
            {
                List<PayConfigurationDetailsDTO> updatedPayConfigurationDetailsDTOList = new List<PayConfigurationDetailsDTO>();
                foreach (PayConfigurationDetailsDTO payConfigurationDetailsDTO in payConfigurationsDTO.PayConfigurationDetailsDTOList)
                {
                    if (payConfigurationDetailsDTO.PayConfigurationId != payConfigurationsDTO.PayConfigurationId)
                    {
                        payConfigurationDetailsDTO.PayConfigurationId = payConfigurationsDTO.PayConfigurationId;
                    }
                    if (payConfigurationDetailsDTO.IsChanged)
                    {
                        updatedPayConfigurationDetailsDTOList.Add(payConfigurationDetailsDTO);
                    }
                }
                if (updatedPayConfigurationDetailsDTOList.Any())
                {
                    log.LogVariableState("UpdatedPayConfigurationDetailsDTOList", updatedPayConfigurationDetailsDTOList);
                    PayConfigurationDetailsListBL payConfigurationDetailsListBL = new PayConfigurationDetailsListBL(executionContext, updatedPayConfigurationDetailsDTOList);
                    payConfigurationDetailsListBL.Save(sqlTransaction);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the PayConfigurationsDTO and PayConfigurationDetailsDTO - children 
        /// </summary>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            // Required validations to be added here
            if (PayConfigurationsDTO.PayTypeId == PayConfigurationsDTO.PayTypeEnum.NONE)
            {
                log.Debug("Pay Type does not exist");
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2841, MessageContainerList.GetMessage(executionContext, ""), MessageContainerList.GetMessage(executionContext, "Validation Error")));
            }
            if (payConfigurationsDTO.PayConfigurationDetailsDTOList != null)
            {
                foreach (PayConfigurationDetailsDTO payConfigurationDetailsDTO in payConfigurationsDTO.PayConfigurationDetailsDTOList)
                {
                    if (payConfigurationDetailsDTO.IsChanged)
                    {
                        log.LogVariableState("PayConfigurationDetailsDTO", payConfigurationDetailsDTO);
                        PayConfigurationDetailsBL payConfigurationDetailsBL = new PayConfigurationDetailsBL(executionContext, payConfigurationDetailsDTO);
                        validationErrorList.AddRange(payConfigurationDetailsBL.Validate(sqlTransaction));
                    }
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public PayConfigurationsDTO PayConfigurationsDTO
        {
            get
            {
                return payConfigurationsDTO;
            }
        }

    }
    /// <summary>
    /// Manages the list of PayConfigurations
    /// </summary>
    public class PayConfigurationsListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<PayConfigurationsDTO> payConfigurationsDTOList = new List<PayConfigurationsDTO>();

        /// <summary>
        /// Parameterized constructor for PayConfigurationsListBL
        /// </summary>
        /// <param name="executionContext">ExecutionContext object as parameter</param>
        public PayConfigurationsListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor for PayConfigurationsListBL
        /// </summary>
        /// <param name="executionContext">ExecutionContext object as parameter</param>
        /// <param name="payConfigurationsDTOList">PayConfigurationsDTOList object as parameter</param>
        public PayConfigurationsListBL(ExecutionContext executionContext, List<PayConfigurationsDTO> payConfigurationsDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, payConfigurationsDTOList);
            this.payConfigurationsDTOList = payConfigurationsDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the PayConfigurationsDTO List based on the search Parameters
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="loadChildRecords">loadChildRecords holds either true or false</param>
        /// <param name="activeChildRecords">activeChildRecords holds either true or false</param>
        /// <param name="sqlTransaction">SqlTransaction</param>
        /// <returns>returns the PayConfigurationsDTO List</returns>
        public List<PayConfigurationsDTO> GetPayConfigurationsDTOList(List<KeyValuePair<PayConfigurationsDTO.SearchByParameters, string>> searchParameters,
                                                                       bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            PayConfigurationsDataHandler payConfigurationsDTODataHandler = new PayConfigurationsDataHandler(sqlTransaction);
            List<PayConfigurationsDTO> payConfigurationsDTOList = payConfigurationsDTODataHandler.GetPayConfigurationsDTOList(searchParameters);
            if (loadChildRecords && payConfigurationsDTOList.Any())
            {
                Build(payConfigurationsDTOList, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(payConfigurationsDTOList);
            return payConfigurationsDTOList;
        }

        /// <summary>
        /// Builds the List of PayConfigurations objects based on the list of PayConfiguration Id.
        /// </summary>
        /// <param name="payConfigurationsDTOList">PayConfigurationsDTO List</param>
        /// <param name="activeChildRecords">activeChildRecords</param>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        private void Build(List<PayConfigurationsDTO> payConfigurationsDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(payConfigurationsDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, PayConfigurationsDTO> payConfigurationIdPayConfigurationDetailsIdDictionary = new Dictionary<int, PayConfigurationsDTO>();
            string payConfigurationIdSet = string.Empty;
            StringBuilder sb = new StringBuilder("");
            for (int i = 0; i < payConfigurationsDTOList.Count; i++)
            {
                if (payConfigurationsDTOList[i].PayConfigurationId == -1 ||
                    payConfigurationIdPayConfigurationDetailsIdDictionary.ContainsKey(payConfigurationsDTOList[i].PayConfigurationId))
                {
                    continue;
                }
                if (i != 0)
                {
                    sb.Append(",");
                }
                sb.Append(payConfigurationsDTOList[i].PayConfigurationId);
                payConfigurationIdPayConfigurationDetailsIdDictionary.Add(payConfigurationsDTOList[i].PayConfigurationId, payConfigurationsDTOList[i]);
            }
            payConfigurationIdSet = sb.ToString();
            PayConfigurationDetailsListBL payConfigurationDetailsListBL = new PayConfigurationDetailsListBL(executionContext);
            List<KeyValuePair<PayConfigurationDetailsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PayConfigurationDetailsDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<PayConfigurationDetailsDTO.SearchByParameters, string>(PayConfigurationDetailsDTO.SearchByParameters.PAY_CONFIGURATION_ID_LIST, payConfigurationIdSet.ToString()));
            searchParameters.Add(new KeyValuePair<PayConfigurationDetailsDTO.SearchByParameters, string>(PayConfigurationDetailsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            if (activeChildRecords)
            {
                searchParameters.Add(new KeyValuePair<PayConfigurationDetailsDTO.SearchByParameters, string>(PayConfigurationDetailsDTO.SearchByParameters.IS_ACTIVE, "1"));
            }
            List<PayConfigurationDetailsDTO> payConfigurationDetailsDTOList = payConfigurationDetailsListBL.GetPayConfigurationDetailsDTOList(searchParameters, sqlTransaction);
            if (payConfigurationDetailsDTOList != null && payConfigurationDetailsDTOList.Any())
            {
                log.LogVariableState("PayConfigurationDetailsDTOList", payConfigurationDetailsDTOList);
                foreach (PayConfigurationDetailsDTO payConfigurationDetailsDTO in payConfigurationDetailsDTOList)
                {
                    if (payConfigurationIdPayConfigurationDetailsIdDictionary.ContainsKey(payConfigurationDetailsDTO.PayConfigurationId))
                    {
                        if (payConfigurationIdPayConfigurationDetailsIdDictionary[payConfigurationDetailsDTO.PayConfigurationId].PayConfigurationDetailsDTOList == null)
                        {
                            payConfigurationIdPayConfigurationDetailsIdDictionary[payConfigurationDetailsDTO.PayConfigurationId].PayConfigurationDetailsDTOList = new List<PayConfigurationDetailsDTO>();
                        }
                        payConfigurationIdPayConfigurationDetailsIdDictionary[payConfigurationDetailsDTO.PayConfigurationId].PayConfigurationDetailsDTOList.Add(payConfigurationDetailsDTO);
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the PayConfigurationsDTO List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public List<PayConfigurationsDTO> Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<PayConfigurationsDTO> payConfigurationsDTOLists = new List<PayConfigurationsDTO>();
            if (payConfigurationsDTOList != null)
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (PayConfigurationsDTO payConfigurationsDTO in payConfigurationsDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            PayConfigurationsBL payConfigurationsBL = new PayConfigurationsBL(executionContext, payConfigurationsDTO);
                            payConfigurationsBL.Save(sqlTransaction);
                            payConfigurationsDTOLists.Add(payConfigurationsBL.PayConfigurationsDTO);
                            parafaitDBTrx.EndTransaction();

                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw;
                        }
                    }
                    log.LogMethodExit();
                }
            }
            return payConfigurationsDTOLists;
        }

    }
}
