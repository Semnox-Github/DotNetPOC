/********************************************************************************************
 * Project Name - PayConfigurationDetails
 * Description  - Business logic file for  PayConfigurationDetails
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
    /// Business logic for PayConfigurationDetails class
    /// </summary>
    public class PayConfigurationDetailsBL
    {
        private PayConfigurationDetailsDTO payConfigurationDetailsDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of PayConfigurationDetailsBL class
        /// </summary>
        private PayConfigurationDetailsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates PayConfigurationDetailsBL object using the PayConfigurationDetailsDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="payConfigurationDetailsDTO">PayConfigurationDetailsDTO object</param>
        public PayConfigurationDetailsBL(ExecutionContext executionContext, PayConfigurationDetailsDTO payConfigurationDetailsDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, payConfigurationDetailsDTO);
            this.payConfigurationDetailsDTO = payConfigurationDetailsDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the PayConfigurationDetailId as the parameter
        /// Would fetch the PayConfigurationDetails object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="payConfigurationDetailId">id - payConfigurationDetailId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public PayConfigurationDetailsBL(ExecutionContext executionContext, int payConfigurationDetailId, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, payConfigurationDetailId, sqlTransaction);
            PayConfigurationDetailsDataHandler payConfigurationDetailsDataHandler = new PayConfigurationDetailsDataHandler(sqlTransaction);
            payConfigurationDetailsDTO = payConfigurationDetailsDataHandler.GetPayConfigurationDetailsDTO(payConfigurationDetailId);
            if (payConfigurationDetailsDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "PayConfigurationDetailsDTO", payConfigurationDetailId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the PayConfigurationDetailsDTO
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction"></param>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (payConfigurationDetailsDTO.IsChanged == false && payConfigurationDetailsDTO.PayConfigurationDetailId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            PayConfigurationDetailsDataHandler payConfigurationDetailsDataHandler = new PayConfigurationDetailsDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (payConfigurationDetailsDTO.PayConfigurationDetailId < 0)
            {
                log.LogVariableState("PayConfigurationDetailsDTO", payConfigurationDetailsDTO);
                payConfigurationDetailsDTO = payConfigurationDetailsDataHandler.Insert(payConfigurationDetailsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                payConfigurationDetailsDTO.AcceptChanges();
            }
            else if (payConfigurationDetailsDTO.IsChanged)
            {
                log.LogVariableState("PayConfigurationDetailsDTO", payConfigurationDetailsDTO);
                payConfigurationDetailsDTO = payConfigurationDetailsDataHandler.Update(payConfigurationDetailsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                payConfigurationDetailsDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the PayConfigurationDetailsDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            ValidationError validationError = null;
            if (payConfigurationDetailsDTO.IsActive)
            {
                /// Added Server Time
                LookupValuesList serverTimeObject = new LookupValuesList(executionContext);
            DateTime currentTime = serverTimeObject.GetServerDateTime();
            // Required validations to be added here
            if (payConfigurationDetailsDTO.PayConfigurationDetailId != -1 && payConfigurationDetailsDTO.IsActive && DateTime.Compare(payConfigurationDetailsDTO.EffectiveDate, currentTime) < 0)
            {
                log.Debug("Please enter a valid Effective Date");
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2836, MessageContainerList.GetMessage(executionContext, ""), MessageContainerList.GetMessage(executionContext, "Validation Error")));
            }
            if (payConfigurationDetailsDTO.EndDate != null && payConfigurationDetailsDTO.EffectiveDate >= payConfigurationDetailsDTO.EndDate)
            {
                log.Debug("Please enter a valid End Date");
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2837, MessageContainerList.GetMessage(executionContext, ""), MessageContainerList.GetMessage(executionContext, "Validation Error")));
            }
            List<KeyValuePair<PayConfigurationDetailsDTO.SearchByParameters, string>> payConfigurationDetailsSearchParameters = new List<KeyValuePair<PayConfigurationDetailsDTO.SearchByParameters, string>>();
            payConfigurationDetailsSearchParameters.Add(new KeyValuePair<PayConfigurationDetailsDTO.SearchByParameters, string>(PayConfigurationDetailsDTO.SearchByParameters.PAY_CONFIGURATION_ID, payConfigurationDetailsDTO.PayConfigurationId.ToString()));
            payConfigurationDetailsSearchParameters.Add(new KeyValuePair<PayConfigurationDetailsDTO.SearchByParameters, string>(PayConfigurationDetailsDTO.SearchByParameters.IS_ACTIVE, "1"));
            payConfigurationDetailsSearchParameters.Add(new KeyValuePair<PayConfigurationDetailsDTO.SearchByParameters, string>(PayConfigurationDetailsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            PayConfigurationDetailsDataHandler payConfigurationDetailsDataHandler = new PayConfigurationDetailsDataHandler(sqlTransaction);
            List<PayConfigurationDetailsDTO> payConfigurationDetailsDTOList = payConfigurationDetailsDataHandler.GetPayConfigurationDetailsDTOList(payConfigurationDetailsSearchParameters);
            if (payConfigurationDetailsDTOList != null && payConfigurationDetailsDTOList.Any())
                {
                    List<PayConfigurationDetailsDTO> orderedPayConfigurationDetailsDTOList = payConfigurationDetailsDTOList.OrderBy(pay => pay.EffectiveDate).ToList();
                    foreach (PayConfigurationDetailsDTO payConfigurationDetailsDTOObj in orderedPayConfigurationDetailsDTOList)
                    {
                        if (IncludesEffectiveDate(payConfigurationDetailsDTOObj, payConfigurationDetailsDTO))
                        {
                            log.Debug("Please enter a valid Effective Date");
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2836, MessageContainerList.GetMessage(executionContext, ""), MessageContainerList.GetMessage(executionContext, "Validation Error")));
                        }

                        if (IncludesEndDate(payConfigurationDetailsDTOObj, payConfigurationDetailsDTO))
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

        public bool IncludesEffectiveDate(PayConfigurationDetailsDTO payConfigurationDetailsDTOObj, PayConfigurationDetailsDTO payConfigurationDetailsDTO)
        {
            return (payConfigurationDetailsDTO.EffectiveDate.Date >= payConfigurationDetailsDTOObj.EffectiveDate.Date && payConfigurationDetailsDTO.EffectiveDate < payConfigurationDetailsDTOObj.EndDate);
        }

        public bool IncludesEndDate(PayConfigurationDetailsDTO payConfigurationDetailsDTOObj, PayConfigurationDetailsDTO payConfigurationDetailsDTO)
        {
            return (payConfigurationDetailsDTO.EndDate >= payConfigurationDetailsDTOObj.EffectiveDate.Date && payConfigurationDetailsDTO.EndDate < payConfigurationDetailsDTOObj.EndDate)
                || (payConfigurationDetailsDTOObj.EndDate >= payConfigurationDetailsDTO.EffectiveDate.Date && payConfigurationDetailsDTOObj.EndDate < payConfigurationDetailsDTO.EndDate);
        }
        
        /// <summary>
         /// Returns whether the schedule is relevent at the specified date and time.
         /// </summary>
         /// <param name="dateTime">date</param>
         /// <returns></returns>
        //private bool IsRelevant(DateTime dateTime)
        //{
        //    log.LogMethodEntry(dateTime);
        //    bool recurFrequencyCheckPassed = true;
        //    if (scheduleDTO == null)
        //    {
        //        log.Debug("Ends-IsRelevant() Method.");
        //        return false;
        //    }
        //    if (DateTime.Compare(dateTime.Date, scheduleDTO.ScheduleTime.Date) < 0)
        //    {
        //        log.Debug("Ends-IsRelevant() Method.");
        //        return false;
        //    }
        //    if (string.Equals(scheduleDTO.RecurFlag, "Y"))
        //    {
        //        if (DateTime.Compare(dateTime.Date, scheduleDTO.RecurEndDate.Date) > 0)
        //        {
        //            log.Debug("Ends-IsRelevant() Method.");
        //            return false;
        //        }
        //        decimal scheduleTime = new decimal(scheduleDTO.ScheduleTime.Hour + (((double)scheduleDTO.ScheduleTime.Minute) / 100));
        //        decimal scheduleEndTime = new decimal(scheduleDTO.ScheduleEndDate.Hour + (((double)scheduleDTO.ScheduleEndDate.Minute) / 100));
        //        decimal currentTime = new decimal(dateTime.Hour + (((double)dateTime.Minute) / 100));
        //        if (scheduleTime > currentTime)
        //        {
        //            log.Debug("Ends-IsRelevant() Method.");
        //            return false;
        //        }
        //        if (scheduleEndTime <= currentTime)
        //        {
        //            log.Debug("Ends-IsRelevant() Method.");
        //            return false;
        //        }
        //        recurFrequencyCheckPassed = CheckRecurFrequency(dateTime);
        //        if (recurFrequencyCheckPassed)
        //        {
        //            if (string.Equals(CheckInclusionExclusion(dateTime), "N"))
        //            {
        //                log.Debug("Ends-IsRelevant() Method.");
        //                recurFrequencyCheckPassed = false;
        //            }
        //        }
        //        else
        //        {
        //            if (string.Equals(CheckInclusionExclusion(dateTime), "Y"))
        //            {
        //                log.Debug("Ends-IsRelevant() Method.");
        //                recurFrequencyCheckPassed = true;
        //            }
        //        }
        //        log.Debug("Ends-IsRelevant() Method.");
        //        return recurFrequencyCheckPassed;
        //    }
        //    else
        //    {
        //        if (DateTime.Compare(dateTime, scheduleDTO.ScheduleTime) < 0)
        //        {
        //            log.Debug("Ends-IsRelevant() Method.");
        //            return false;
        //        }
        //        if (DateTime.Compare(dateTime, scheduleDTO.ScheduleEndDate) >= 0)
        //        {
        //            log.Debug("Ends-IsRelevant() Method.");
        //            return false;
        //        }
        //        if (string.Equals(CheckInclusionExclusion(dateTime), "N"))
        //        {
        //            log.Debug("Ends-IsRelevant() Method.");
        //            return false;
        //        }
        //        else
        //        {
        //            log.Debug("Ends-IsRelevant() Method.");
        //            return true;
        //        }
        //    }
        //}
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public PayConfigurationDetailsDTO PayConfigurationDetailsDTO
        {
            get
            {
                return payConfigurationDetailsDTO;
            }
        }

    }
    /// <summary>
    /// Manages the list of PayConfigurationDetails
    /// </summary>
    public class PayConfigurationDetailsListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private readonly List<PayConfigurationDetailsDTO> payConfigurationDetailsDTOList = new List<PayConfigurationDetailsDTO>();

        /// <summary>
        /// Parameterized constructor for PayConfigurationDetailsListBL
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public PayConfigurationDetailsListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor for PayConfigurationDetailsListBL
        /// </summary>
        /// <param name="executionContext">executionContext object passed as a parameter</param>
        /// <param name="payConfigurationDetailsDTOList">PayConfigurationDetailsDTOList passed as a parameter</param>
        public PayConfigurationDetailsListBL(ExecutionContext executionContext, List<PayConfigurationDetailsDTO> payConfigurationDetailsDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, payConfigurationDetailsDTOList);
            this.payConfigurationDetailsDTOList = payConfigurationDetailsDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the PayConfigurationDetailsDTOList based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>returns the PayConfigurationDetailsDTO List</returns>
        public List<PayConfigurationDetailsDTO> GetPayConfigurationDetailsDTOList(List<KeyValuePair<PayConfigurationDetailsDTO.SearchByParameters, string>> searchParameters,
                                                                                    SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            PayConfigurationDetailsDataHandler payConfigurationDetailsDataHandler = new PayConfigurationDetailsDataHandler(sqlTransaction);
            List<PayConfigurationDetailsDTO> payConfigurationDetailsDTOList = payConfigurationDetailsDataHandler.GetPayConfigurationDetailsDTOList(searchParameters);
            log.LogMethodExit(payConfigurationDetailsDTOList);
            return payConfigurationDetailsDTOList;
        }

        /// <summary>
        /// Saves the PayConfigurationDetailsDTO List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (payConfigurationDetailsDTOList == null || payConfigurationDetailsDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < payConfigurationDetailsDTOList.Count; i++)
            {
                PayConfigurationDetailsDTO payConfigurationDetailsDTO = payConfigurationDetailsDTOList[i];
                if (payConfigurationDetailsDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    PayConfigurationDetailsBL PayConfigurationDetailsBL = new PayConfigurationDetailsBL(executionContext, payConfigurationDetailsDTO);
                    PayConfigurationDetailsBL.Save(sqlTransaction);
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
                    log.Error("Error occurred while saving PayConfigurationDetailsDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("PayConfigurationDetailsDTO", payConfigurationDetailsDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
