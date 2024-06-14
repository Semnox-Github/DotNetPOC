/********************************************************************************************
 * Project Name - PayConfigurationMap
 * Description  - Business logic file for  PayConfigurationMap
 * 
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
    /// Business logic for PayConfigurationMap class
    /// </summary>
    public class PayConfigurationMapBL
    {
        private PayConfigurationMapDTO payConfigurationMapDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of PayConfigurationMapBL class
        /// </summary>
        private PayConfigurationMapBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates PayConfigurationMapBL object using the PayConfigurationMapDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="payConfigurationMapDTO">PayConfigurationMapDTO object</param>
        public PayConfigurationMapBL(ExecutionContext executionContext, PayConfigurationMapDTO payConfigurationMapDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, payConfigurationMapDTO);
            this.payConfigurationMapDTO = payConfigurationMapDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the payConfigurationMapId as the parameter
        /// Would fetch the PayConfigurationMap object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="payConfigurationMapId">id - payConfigurationMapId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public PayConfigurationMapBL(ExecutionContext executionContext, int payConfigurationMapId, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, payConfigurationMapId, sqlTransaction);
            PayConfigurationMapDataHandler payConfigurationMapDataHandler = new PayConfigurationMapDataHandler(sqlTransaction);
            payConfigurationMapDTO = payConfigurationMapDataHandler.GetPayConfigurationMapDTO(payConfigurationMapId);
            if (payConfigurationMapDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "PayConfigurationMapDTO", payConfigurationMapId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the PayConfigurationMapDTO
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (payConfigurationMapDTO.IsChanged == false && payConfigurationMapDTO.PayConfigurationMapId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            PayConfigurationMapDataHandler payConfigurationMapDataHandler = new PayConfigurationMapDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (payConfigurationMapDTO.PayConfigurationMapId < 0)
            {
                log.LogVariableState("PayConfigurationMapDTO", payConfigurationMapDTO);
                payConfigurationMapDTO = payConfigurationMapDataHandler.Insert(payConfigurationMapDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                payConfigurationMapDTO.AcceptChanges();
            }
            else if (payConfigurationMapDTO.IsChanged)
            {
                log.LogVariableState("PayConfigurationMapDTO", payConfigurationMapDTO);
                payConfigurationMapDTO = payConfigurationMapDataHandler.Update(payConfigurationMapDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                payConfigurationMapDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the PayConfigurationMapDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            ValidationError validationError = null;
            if (payConfigurationMapDTO.IsActive)
            {
                /// Added Server Time
                LookupValuesList serverTimeObject = new LookupValuesList(executionContext);
                DateTime currentTime = serverTimeObject.GetServerDateTime();
                // Required validations to be added here
                if (payConfigurationMapDTO.PayConfigurationMapId != -1 && payConfigurationMapDTO.IsActive && DateTime.Compare(payConfigurationMapDTO.EffectiveDate, currentTime) < 0)
                {
                    log.Debug("Please enter a valid Effective Date");
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2836, MessageContainerList.GetMessage(executionContext, ""), MessageContainerList.GetMessage(executionContext, "Validation Error")));
                }
                if (payConfigurationMapDTO.EndDate != null && payConfigurationMapDTO.EffectiveDate >= payConfigurationMapDTO.EndDate)
                {
                    log.Debug("Please enter a valid End Date");
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2837, MessageContainerList.GetMessage(executionContext, ""), MessageContainerList.GetMessage(executionContext, "Validation Error")));
                }
                if (payConfigurationMapDTO.PayConfigurationId < 0)
                {
                    log.Debug("Please enter a valid Pay Configuration Id");
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2839, MessageContainerList.GetMessage(executionContext, ""), MessageContainerList.GetMessage(executionContext, "Validation Error")));
                }
                List<KeyValuePair<PayConfigurationMapDTO.SearchByParameters, string>> payConfigurationMapSearchParameters = new List<KeyValuePair<PayConfigurationMapDTO.SearchByParameters, string>>();
                if (payConfigurationMapDTO.UserId >= 0)
                {
                    payConfigurationMapSearchParameters.Add(new KeyValuePair<PayConfigurationMapDTO.SearchByParameters, string>(PayConfigurationMapDTO.SearchByParameters.USER_ID, payConfigurationMapDTO.UserId.ToString()));
                }
                else if (payConfigurationMapDTO.UserRoleId >= 0)
                {
                    payConfigurationMapSearchParameters.Add(new KeyValuePair<PayConfigurationMapDTO.SearchByParameters, string>(PayConfigurationMapDTO.SearchByParameters.USER_ROLE_ID, payConfigurationMapDTO.UserRoleId.ToString()));
                }
                payConfigurationMapSearchParameters.Add(new KeyValuePair<PayConfigurationMapDTO.SearchByParameters, string>(PayConfigurationMapDTO.SearchByParameters.IS_ACTIVE, "1"));
                payConfigurationMapSearchParameters.Add(new KeyValuePair<PayConfigurationMapDTO.SearchByParameters, string>(PayConfigurationMapDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                PayConfigurationMapDataHandler payConfigurationMapDataHandler = new PayConfigurationMapDataHandler(sqlTransaction);
                List<PayConfigurationMapDTO> payConfigurationMapDTOList = payConfigurationMapDataHandler.GetPayConfigurationMapDTOList(payConfigurationMapSearchParameters);
                if (payConfigurationMapDTOList != null && payConfigurationMapDTOList.Any())
                {
                    payConfigurationMapDTOList = payConfigurationMapDTOList.Where(x => x.PayConfigurationMapId != payConfigurationMapDTO.PayConfigurationMapId).ToList();
                    List<PayConfigurationMapDTO> orderedPayConfigurationMapDTOList = payConfigurationMapDTOList.OrderBy(pay => pay.EffectiveDate).ToList();
                    foreach (PayConfigurationMapDTO payConfigurationMapDTOObj in orderedPayConfigurationMapDTOList)
                    {
                        if (IncludesEffectiveDate(payConfigurationMapDTOObj, payConfigurationMapDTO))
                        {
                            log.Debug("Please enter a valid Effective Date");
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2836, MessageContainerList.GetMessage(executionContext, ""), MessageContainerList.GetMessage(executionContext, "Validation Error")));
                        }

                        if (IncludesEndDate(payConfigurationMapDTOObj, payConfigurationMapDTO))
                        {
                            log.Debug("Please enter a valid End Date");
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2837, MessageContainerList.GetMessage(executionContext, ""), MessageContainerList.GetMessage(executionContext, "Validation Error")));
                        }
                    }

                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public PayConfigurationMapDTO PayConfigurationMapDTO
        {
            get
            {
                return payConfigurationMapDTO;
            }
        }
        public bool IncludesEffectiveDate(PayConfigurationMapDTO payConfigurationMapDTOObj, PayConfigurationMapDTO payConfigurationMapDTO)
        {
            return (payConfigurationMapDTO.EffectiveDate.Date >= payConfigurationMapDTOObj.EffectiveDate.Date && payConfigurationMapDTO.EffectiveDate < payConfigurationMapDTOObj.EndDate);
        }

        public bool IncludesEndDate(PayConfigurationMapDTO payConfigurationMapDTOObj, PayConfigurationMapDTO payConfigurationMapDTO)
        {
            return (payConfigurationMapDTO.EndDate >= payConfigurationMapDTOObj.EffectiveDate.Date && payConfigurationMapDTO.EndDate < payConfigurationMapDTOObj.EndDate)
                || (payConfigurationMapDTOObj.EndDate >= payConfigurationMapDTO.EffectiveDate.Date && payConfigurationMapDTOObj.EndDate < payConfigurationMapDTO.EndDate);
        }
    }
    /// <summary>
    /// Manages the list of PayConfigurationMap
    /// </summary>
    public class PayConfigurationMapListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private readonly List<PayConfigurationMapDTO> payConfigurationMapDTOList = new List<PayConfigurationMapDTO>();

        /// <summary>
        /// Parameterized constructor for PayConfigurationMapListBL
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public PayConfigurationMapListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor for PayConfigurationMapListBL
        /// </summary>
        /// <param name="executionContext">executionContext object passed as a parameter</param>
        /// <param name="payConfigurationMapDTOList">PayConfigurationMapDTOList passed as a parameter</param>
        public PayConfigurationMapListBL(ExecutionContext executionContext, List<PayConfigurationMapDTO> payConfigurationMapDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, payConfigurationMapDTOList);
            this.payConfigurationMapDTOList = payConfigurationMapDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the PayConfigurationMapDTOList based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>returns the PayConfigurationMapDTO List</returns>
        public List<PayConfigurationMapDTO> GetPayConfigurationMapDTOList(List<KeyValuePair<PayConfigurationMapDTO.SearchByParameters, string>> searchParameters,
                                                                                    SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            PayConfigurationMapDataHandler payConfigurationMapDTODataHandler = new PayConfigurationMapDataHandler(sqlTransaction);
            List<PayConfigurationMapDTO> payConfigurationMapDTOList = payConfigurationMapDTODataHandler.GetPayConfigurationMapDTOList(searchParameters);
            log.LogMethodExit(payConfigurationMapDTOList);
            return payConfigurationMapDTOList;
        }

        /// <summary>
        /// Saves the PayConfigurationMapDTO List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public List<PayConfigurationMapDTO> Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<PayConfigurationMapDTO> payConfigurationMapDTOLists = new List<PayConfigurationMapDTO>();
            if (payConfigurationMapDTOList != null)
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (PayConfigurationMapDTO payConfigurationMapDTO in payConfigurationMapDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            PayConfigurationMapBL payConfigurationMapBL = new PayConfigurationMapBL(executionContext, payConfigurationMapDTO);
                            payConfigurationMapBL.Save(sqlTransaction);
                            payConfigurationMapDTOLists.Add(payConfigurationMapBL.PayConfigurationMapDTO);
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
            return payConfigurationMapDTOLists;
        }
    }
}
