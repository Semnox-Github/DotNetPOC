
/********************************************************************************************
 * Project Name - Customer Feedback Survey Details
 * Description  - A high level structure created to classify the Customer Feedback Survey Details 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        05-Dec-2016   Raghuveera          Created 
 *2.70.2        19-Jul-2019    Girish Kundar       Modified : Save() method. Now Insert/Update method returns the DTO instead of Id.
 *2.70.3       21-02-2020     Girish Kundar     Modified : 3 tier Changes for REST API
 *2.80        24-Feb-2020   Mushahid Faizan  Modified : 3 tier Changes for REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// Business logic of saving  CustomerFeedbackSurveyDetails
    /// </summary>
    public class CustomerFeedbackSurveyDetails
    {
        private CustomerFeedbackSurveyDetailsDTO customerFeedbackSurveyDetailsDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext machineUserContext;

        /// <summary>
        /// Default constructor of CustomerFeedbackSurveyDetails class
        /// </summary>
        public CustomerFeedbackSurveyDetails(ExecutionContext machineUserContext)
        {
            log.LogMethodEntry(machineUserContext);
            this.machineUserContext = machineUserContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the Customer Feedback Survey Details id as the parameter
        /// Would fetch the Customer Feedback Survey Details object from the database based on the id passed. 
        /// </summary>
        /// <param name="customerFeedbackSurveyDetailsId">Customer Feedback Survey Details id</param>
        public CustomerFeedbackSurveyDetails(ExecutionContext machineUserContext, int customerFeedbackSurveyDetailsId, SqlTransaction sqlTransaction = null)
            : this(machineUserContext)
        {
            log.LogMethodEntry(customerFeedbackSurveyDetailsId, sqlTransaction);
            CustomerFeedbackSurveyDetailsDataHandler customerFeedbackSurveyDetailsDataHandler = new CustomerFeedbackSurveyDetailsDataHandler(sqlTransaction);
            customerFeedbackSurveyDetailsDTO = customerFeedbackSurveyDetailsDataHandler.GetCustomerFeedbackSurveyDetails(customerFeedbackSurveyDetailsId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates Customer Feedback Survey Details object using the CustomerFeedbackSurveyDetailsDTO
        /// </summary>
        /// <param name="customerFeedbackSurveyDetails">CustomerFeedbackSurveyDetailsDTO object</param>
        public CustomerFeedbackSurveyDetails(ExecutionContext machineUserContext, CustomerFeedbackSurveyDetailsDTO customerFeedbackSurveyDetailsDTO)
            : this(machineUserContext)
        {
            log.LogMethodEntry(customerFeedbackSurveyDetailsDTO);
            this.customerFeedbackSurveyDetailsDTO = customerFeedbackSurveyDetailsDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the Customer Feedback Survey Details
        /// Checks if the CustomerFeedbackSurveyDetails id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (customerFeedbackSurveyDetailsDTO.IsChanged == false
                && customerFeedbackSurveyDetailsDTO.CustFbSurveyDetailId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            CustomerFeedbackSurveyDetailsDataHandler customerFeedbackSurveyDetailsDataHandler = new CustomerFeedbackSurveyDetailsDataHandler();
                List<ValidationError> validationErrors = Validate();
                if (validationErrors.Any())
                {
                    string message = MessageContainerList.GetMessage(machineUserContext, "Validation Error");
                    log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                    throw new ValidationException(message, validationErrors);
                }
                if (customerFeedbackSurveyDetailsDTO.CustFbSurveyDetailId < 0)
                {
                    customerFeedbackSurveyDetailsDTO = customerFeedbackSurveyDetailsDataHandler.InsertCustomerFeedbackSurveyDetails(customerFeedbackSurveyDetailsDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId(), sqlTransaction);
                    customerFeedbackSurveyDetailsDTO.AcceptChanges();
                }
                else if (customerFeedbackSurveyDetailsDTO.IsChanged)
                {
                    customerFeedbackSurveyDetailsDTO = customerFeedbackSurveyDetailsDataHandler.UpdateCustomerFeedbackSurveyDetails(customerFeedbackSurveyDetailsDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId(), sqlTransaction);
                    customerFeedbackSurveyDetailsDTO.AcceptChanges();
                }
            log.LogMethodExit();
        }
        /// <summary>
        /// Validate the customerFeedbackSurveyDetails
        /// </summary>
        /// <returns></returns>
        private List<ValidationError> Validate()
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            // Validation Logic here.
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
        /// <summary>
        /// Just checking for the passed customer and survey question asked on passed date or not
        /// </summary>
        /// <param name="surveyId">Integer value used to identify the survey.</param>
        /// <param name="objectName">Name of the entity the object id belongs to </param>
        /// <param name="objectId">ObjectId id is integer value using which code will identify the customer related records. </param>
        /// <param name="dtDate">Datetime value used to check the question is asked on the passed date or not.  </param>
        /// <param name="criteria"> The criteria string.  </param>
        /// <returns> returns true/false based on the customer question asked or not.</returns>
        public bool IsQuestionAsked(int surveyId, string objectName, int objectId, DateTime dtDate, string criteria)
        {
            log.LogMethodEntry(surveyId, objectName, objectId, dtDate, criteria);
            CustomerFeedbackSurveyMappingList customerFeedbackSurveyMappingList = new CustomerFeedbackSurveyMappingList(machineUserContext);
            List<CustomerFeedbackSurveyMappingDTO> customerFeedbackSurveyMappingDTOList = new List<CustomerFeedbackSurveyMappingDTO>();
            List<KeyValuePair<CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters, string>> searchByCustomerFeedbackSurveyMappingParameters = new List<KeyValuePair<CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters, string>>();
            CustomerFeedbackSurveyDetailsList customerFeedbackSurveyDetailsList = new CustomerFeedbackSurveyDetailsList(machineUserContext);
            List<CustomerFeedbackSurveyDetailsDTO> customerFeedbackSurveyDetailsDTOList = new List<CustomerFeedbackSurveyDetailsDTO>();
            List<KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>> searchByCustomerFeedbackSurveyDetailsParameters = new List<KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>>();
            LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
            List<LookupValuesDTO> lookupValuesDTOList = new List<LookupValuesDTO>();
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();

            searchByCustomerFeedbackSurveyMappingParameters.Add(new KeyValuePair<CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters, string>(CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters.IS_ACTIVE, "1"));
            searchByCustomerFeedbackSurveyMappingParameters.Add(new KeyValuePair<CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters, string>(CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            searchByCustomerFeedbackSurveyMappingParameters.Add(new KeyValuePair<CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters, string>(CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters.OBJECT_NAME, objectName.ToString()));
            searchByCustomerFeedbackSurveyMappingParameters.Add(new KeyValuePair<CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters, string>(CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters.OBJECT_ID, objectId.ToString()));
            customerFeedbackSurveyMappingDTOList = customerFeedbackSurveyMappingList.GetAllCustomerFeedbackSurveyMapping(searchByCustomerFeedbackSurveyMappingParameters);
            if (customerFeedbackSurveyMappingDTOList != null && customerFeedbackSurveyMappingDTOList.Any())
            {
                foreach (CustomerFeedbackSurveyMappingDTO customerFeedbackSurveyMappingDTO in customerFeedbackSurveyMappingDTOList)
                {
                    searchByCustomerFeedbackSurveyDetailsParameters = new List<KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>>();
                    searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.IS_ACTIVE, "1"));
                    searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                    searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CUST_FB_SURVEY_DETAIL_ID, customerFeedbackSurveyMappingDTO.LastCustFbSurveyDetailId.ToString()));
                    searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CUST_FB_SURVEY_ID, surveyId.ToString()));
                    customerFeedbackSurveyDetailsDTOList = new List<CustomerFeedbackSurveyDetailsDTO>();
                    customerFeedbackSurveyDetailsDTOList = customerFeedbackSurveyDetailsList.GetAllCustomerFeedbackSurveyDetails(searchByCustomerFeedbackSurveyDetailsParameters);
                    if (customerFeedbackSurveyDetailsDTOList != null && customerFeedbackSurveyDetailsDTOList.Count > 0)
                    {
                        lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "CUSTOMER_FEEDBACK_CRITERIA"));
                        lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, criteria));
                        lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                        lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                        if (lookupValuesDTOList != null && lookupValuesDTOList.Count > 0)
                        {
                            if (lookupValuesDTOList[0].LookupValueId != customerFeedbackSurveyDetailsDTOList[0].CriteriaId)
                            {
                                return true;
                            }
                        }
                        if (dtDate.Date.Equals(customerFeedbackSurveyMappingDTO.LastVisitDate.Date) || (customerFeedbackSurveyDetailsDTOList[0].NextQuestionId == -1 && !customerFeedbackSurveyDetailsDTOList[0].IsRecur))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            log.LogMethodExit(false);
            return false;
        }

        /// <summary>
        /// Just checking for the passed customer and survey question asked on passed date or not
        /// </summary>
        /// <param name="surveyId">Integer value used to identify the survey.</param>           
        /// <param name="criteria"> The criteria string.  </param>
        /// <returns> returns true/false based on the customer question asked or not.</returns>
        public bool IsRetailQuestionAsked(int surveyId, string criteria)
        {
            log.LogMethodEntry(surveyId, criteria);
            CustomerFeedbackSurveyDetailsList customerFeedbackSurveyDetailsList = new CustomerFeedbackSurveyDetailsList(machineUserContext);
            List<CustomerFeedbackSurveyDetailsDTO> customerFeedbackSurveyDetailsDTOList = new List<CustomerFeedbackSurveyDetailsDTO>();
            List<KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>> searchByCustomerFeedbackSurveyDetailsParameters = new List<KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>>();
            LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
            List<LookupValuesDTO> lookupValuesDTOList = new List<LookupValuesDTO>();
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();


            searchByCustomerFeedbackSurveyDetailsParameters = new List<KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>>();
            searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.IS_ACTIVE, "1"));
            searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CUST_FB_SURVEY_ID, surveyId.ToString()));
            customerFeedbackSurveyDetailsDTOList = new List<CustomerFeedbackSurveyDetailsDTO>();
            customerFeedbackSurveyDetailsDTOList = customerFeedbackSurveyDetailsList.GetAllCustomerFeedbackSurveyDetails(searchByCustomerFeedbackSurveyDetailsParameters);
            if (customerFeedbackSurveyDetailsDTOList != null && customerFeedbackSurveyDetailsDTOList.Count > 0)
            {
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "CUSTOMER_FEEDBACK_CRITERIA"));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, criteria));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if (lookupValuesDTOList != null && lookupValuesDTOList.Count > 0)
                {
                    if (lookupValuesDTOList[0].LookupValueId == customerFeedbackSurveyDetailsDTOList[0].CriteriaId)
                    {
                        return false;
                    }
                }
            }
            log.LogMethodExit(true);
            return true;
        }

        /// <summary>
        /// Getting the next question id using this method
        /// </summary>
        /// <param name="surveyId"> Is the integer value.</param>
        /// <param name="lastSurveyDetailId">Is the integer value used to fetch the next question.</param>
        /// <param name="surveyDatasetId">Is the integer value used to validate the response of the privious question.</param>
        /// <param name="VisitCount">visit count is an integer value.</param>
        /// <returns></returns>
        public List<CustomerFeedbackSurveyDetailsDTO> GetNextQuestionId(int surveyId, int lastSurveyDetailId, int surveyDatasetId, int VisitCount)
        {
            log.LogMethodEntry(surveyId, lastSurveyDetailId, surveyDatasetId, VisitCount);
            CustomerFeedbackSurveyDetailsList customerFeedbackSurveyDetailsList = new CustomerFeedbackSurveyDetailsList();
            List<CustomerFeedbackSurveyDetailsDTO> customerFeedbackSurveyDetailsDTOList = new List<CustomerFeedbackSurveyDetailsDTO>();
            List<CustomerFeedbackSurveyDetailsDTO> customerFeedbackSurveyDetailsDTOReturnList = new List<CustomerFeedbackSurveyDetailsDTO>();
            List<KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>> searchByCustomerFeedbackSurveyDetailsParameters = new List<KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>>();

            CustomerFeedbackSurveyDataList custFeedbackSurveyDataList = new CustomerFeedbackSurveyDataList(machineUserContext);
            List<CustomerFeedbackSurveyDataDTO> custFeedbackSurveyDataDTOList = new List<CustomerFeedbackSurveyDataDTO>();
            List<KeyValuePair<CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters, string>> searchByCustFeedbackSurveyDataParameters = new List<KeyValuePair<CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters, string>>();

            searchByCustomerFeedbackSurveyDetailsParameters = new List<KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>>();
            searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.IS_ACTIVE, "1"));
            searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CUST_FB_SURVEY_DETAIL_ID, lastSurveyDetailId.ToString()));
            searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CUST_FB_SURVEY_ID, surveyId.ToString()));
            customerFeedbackSurveyDetailsDTOList = customerFeedbackSurveyDetailsList.GetAllCustomerFeedbackSurveyDetails(searchByCustomerFeedbackSurveyDetailsParameters);
            if (customerFeedbackSurveyDetailsDTOList != null)
            {
                foreach (CustomerFeedbackSurveyDetailsDTO customerFeedbackSurveyDetailsDTO in customerFeedbackSurveyDetailsDTOList)
                {
                    if (customerFeedbackSurveyDetailsDTO.CustFbSurveyId == surveyId)
                    {
                        if (customerFeedbackSurveyDetailsDTO.ExpectedCustFbResponseValueId == -1)//when next question is not depends on the response of last question
                        {
                            log.Debug("Ends-GetNextQuestionId(surveyId,LastsurveyDetailId,surveyDatasetId,VisitCount) method by returning " + customerFeedbackSurveyDetailsDTO.NextQuestionId + " when next question is not depends on privious response.");
                            //CopySurveyDetailDTO(customerFeedbackSurveyDetailsDTO, customerFeedbackSurveyDetailsDTOPassed);
                            customerFeedbackSurveyDetailsDTOReturnList.Add(customerFeedbackSurveyDetailsDTO);
                            return customerFeedbackSurveyDetailsDTOReturnList;
                        }
                        else//When it depends on the response
                        {
                            searchByCustFeedbackSurveyDataParameters = new List<KeyValuePair<CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters, string>>();
                            searchByCustFeedbackSurveyDataParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters, string>(CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters.IS_ACTIVE, "1"));
                            searchByCustFeedbackSurveyDataParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters, string>(CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                            searchByCustFeedbackSurveyDataParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters, string>(CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters.CUST_FB_SURVEY_DATASET_ID, surveyDatasetId.ToString()));
                            searchByCustFeedbackSurveyDataParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters, string>(CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters.CUST_FB_SURVEY_DETAIL_ID, lastSurveyDetailId.ToString()));
                            custFeedbackSurveyDataDTOList = custFeedbackSurveyDataList.GetAllCustomerFeedbackSurveyData(searchByCustFeedbackSurveyDataParameters);
                            if (custFeedbackSurveyDataDTOList != null && custFeedbackSurveyDataDTOList.Count > 0)//fetching the last response.
                            {
                                searchByCustomerFeedbackSurveyDetailsParameters = new List<KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>>();
                                searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.IS_ACTIVE, "1"));
                                searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                                //searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CUST_FB_SURVEY_DETAIL_ID, custFeedbackSurveyDataDTOList[0].CustFbSurveyDetailId.ToString()));
                                searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.EXPECTED_CUST_FB_RESPONSE_VALUE_ID, custFeedbackSurveyDataDTOList[0].CustFbResponseValueId.ToString()));
                                searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CUST_FB_SURVEY_ID, surveyId.ToString()));
                                customerFeedbackSurveyDetailsDTOList = new List<CustomerFeedbackSurveyDetailsDTO>();
                                customerFeedbackSurveyDetailsDTOList = customerFeedbackSurveyDetailsList.GetAllCustomerFeedbackSurveyDetails(searchByCustomerFeedbackSurveyDetailsParameters);
                                if (customerFeedbackSurveyDetailsDTOList != null && customerFeedbackSurveyDetailsDTOList.Count > 0)//using response id and survey detail id fetching the survey detail. There will be one single value for above combination.
                                {
                                    log.Debug("Ends-GetNextQuestionId(surveyId,LastsurveyDetailId,surveyDatasetId,VisitCount) method by returning customerFeedbackSurveyDetailsDTOList.");
                                    return customerFeedbackSurveyDetailsDTOList;
                                    //if (customerFeedbackSurveyDetailsDTOList[0].NextQuestionId == -1 && customerFeedbackSurveyDetailsDTOList[0].IsRecur && VisitCount > Convert.ToInt32(customerFeedbackSurveyDetailsDTOList[0].CriteriaValue))
                                    //{
                                    //    log.Debug("Ends-GetNextQuestionId(surveyId,LastsurveyDetailId,surveyDatasetId,VisitCount) method by returning " + customerFeedbackSurveyDetailsDTOList[0].CustFbQuestionId + " When the question repeats each visit.");
                                    //    CopySurveyDetailDTO( customerFeedbackSurveyDetailsDTOList[0],customerFeedbackSurveyDetailsDTOPassed);
                                    //    return customerFeedbackSurveyDetailsDTOList[0].NextQuestionId;
                                    //}
                                    //else
                                    //{
                                    //    log.Debug("Ends-GetNextQuestionId(surveyId,LastsurveyDetailId,surveyDatasetId,VisitCount) method by returning " + customerFeedbackSurveyDetailsDTOList[0].NextQuestionId + " when next question is depends on privious response.");
                                    //    CopySurveyDetailDTO(customerFeedbackSurveyDetailsDTOList[0], customerFeedbackSurveyDetailsDTOPassed);
                                    //    return customerFeedbackSurveyDetailsDTOList[0].NextQuestionId;
                                    //}
                                }
                            }
                        }
                    }
                    break;//No need to repeat the loop again. Because DTO list is reassigned inside the loop.
                }
            }
            log.LogMethodExit();
            return null;
        }

        /// <summary>
        /// Getting the next question id using this method
        /// </summary>
        /// <param name="surveyId"> Is the integer value.</param>
        /// <param name="lastSurveyDetailId">Is the integer value used to fetch the next question.</param>
        /// <param name="responseValueId">Is the integer value used to validate the response of the privious question.</param>
        /// <param name="VisitCount">visit count is an integer value.</param>
        /// <returns></returns>
        public int GetNextQuestionFromResponseId(int surveyId, int lastSurveyDetailId, int responseValueId, int VisitCount)
        {
            log.LogMethodEntry(surveyId, lastSurveyDetailId, responseValueId, VisitCount);
            CustomerFeedbackSurveyDetailsList customerFeedbackSurveyDetailsList = new CustomerFeedbackSurveyDetailsList();
            List<CustomerFeedbackSurveyDetailsDTO> customerFeedbackSurveyDetailsDTOList = new List<CustomerFeedbackSurveyDetailsDTO>();
            List<KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>> searchByCustomerFeedbackSurveyDetailsParameters = new List<KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>>();

            searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.IS_ACTIVE, "1"));
            searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CUST_FB_SURVEY_DETAIL_ID, lastSurveyDetailId.ToString()));
            searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.EXPECTED_CUST_FB_RESPONSE_VALUE_ID, responseValueId.ToString()));
            searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CUST_FB_SURVEY_ID, surveyId.ToString()));
            customerFeedbackSurveyDetailsDTOList = new List<CustomerFeedbackSurveyDetailsDTO>();
            customerFeedbackSurveyDetailsDTOList = customerFeedbackSurveyDetailsList.GetAllCustomerFeedbackSurveyDetails(searchByCustomerFeedbackSurveyDetailsParameters);
            if (customerFeedbackSurveyDetailsDTOList != null && customerFeedbackSurveyDetailsDTOList.Count == 1)//using response id and survey detail id fetching the survey detail. There will be one single value for above combination.
            {
                if (customerFeedbackSurveyDetailsDTOList[0].NextQuestionId == -1 && customerFeedbackSurveyDetailsDTOList[0].IsRecur && VisitCount > Convert.ToInt32(customerFeedbackSurveyDetailsDTOList[0].CriteriaValue))
                {
                    log.LogMethodExit(customerFeedbackSurveyDetailsDTOList[0].NextQuestionId);
                    return customerFeedbackSurveyDetailsDTOList[0].NextQuestionId;
                }
                else
                {
                    log.LogMethodExit(customerFeedbackSurveyDetailsDTOList[0].NextQuestionId);
                    return customerFeedbackSurveyDetailsDTOList[0].NextQuestionId;
                }
            }
            log.LogMethodExit(-1);
            return -1;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public CustomerFeedbackSurveyDetailsDTO GetCustomerFeedbackSurveyDetails { get { return customerFeedbackSurveyDetailsDTO; } }
    }

    /// <summary>
    /// Manages the list of Customer Feedback Survey Details 
    /// </summary>
    public class CustomerFeedbackSurveyDetailsList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<CustomerFeedbackSurveyDetailsDTO> customerFeedbackSurveyDetailsDTOList = new List<CustomerFeedbackSurveyDetailsDTO>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        public CustomerFeedbackSurveyDetailsList(ExecutionContext executionContext = null)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="customerFeedbackSurveyDetailsDTOList"></param>
        public CustomerFeedbackSurveyDetailsList(ExecutionContext executionContext, List<CustomerFeedbackSurveyDetailsDTO> customerFeedbackSurveyDetailsDTOList) : this(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.customerFeedbackSurveyDetailsDTOList = customerFeedbackSurveyDetailsDTOList;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the Customer Feedback Question
        /// </summary>
        public CustomerFeedbackSurveyDetailsDTO GetCustomerFeedbackSurveyDetail(int CustFbSurveyDetailId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(CustFbSurveyDetailId, sqlTransaction);
            CustomerFeedbackSurveyDetailsDataHandler customerFeedbackSurveyDetailsDataHandler = new CustomerFeedbackSurveyDetailsDataHandler(sqlTransaction);
            CustomerFeedbackSurveyDetailsDTO customerFeedbackSurveyDetailsDTO = customerFeedbackSurveyDetailsDataHandler.GetCustomerFeedbackSurveyDetails(CustFbSurveyDetailId);
            log.LogMethodExit(customerFeedbackSurveyDetailsDTO);
            return customerFeedbackSurveyDetailsDTO;
        }
        /// <summary>
        /// Returns the Customer Feedback Survey Details list
        /// </summary>
        public List<CustomerFeedbackSurveyDetailsDTO> GetAllCustomerFeedbackSurveyDetails(List<KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            CustomerFeedbackSurveyDetailsDataHandler customerFeedbackSurveyDetailsDataHandler = new CustomerFeedbackSurveyDetailsDataHandler(sqlTransaction);
            this.customerFeedbackSurveyDetailsDTOList = customerFeedbackSurveyDetailsDataHandler.GetCustomerFeedbackSurveyDetailsList(searchParameters);
            log.LogMethodEntry(customerFeedbackSurveyDetailsDTOList);
            return customerFeedbackSurveyDetailsDTOList;
        }

        public List<CustomerFeedbackSurveyDetailsDTO> GetAllCustomerFeedbackSurveyDetails(List<KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>> searchParameters, bool buildChildRecords, bool activeChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, buildChildRecords, activeChildRecords, sqlTransaction);
            CustomerFeedbackSurveyDetailsDataHandler customerFeedbackSurveyDetailsDataHandler = new CustomerFeedbackSurveyDetailsDataHandler(sqlTransaction);
            this.customerFeedbackSurveyDetailsDTOList = customerFeedbackSurveyDetailsDataHandler.GetCustomerFeedbackSurveyDetailsList(searchParameters);
            if (this.customerFeedbackSurveyDetailsDTOList != null && this.customerFeedbackSurveyDetailsDTOList.Any() && buildChildRecords)
            {
                Build(this.customerFeedbackSurveyDetailsDTOList, activeChildRecords, sqlTransaction);
            }
            log.LogMethodEntry(customerFeedbackSurveyDetailsDTOList);
            return customerFeedbackSurveyDetailsDTOList;
        }

        private void Build(List<CustomerFeedbackSurveyDetailsDTO> customerFeedbackSurveyDetailsDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(customerFeedbackSurveyDetailsDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, CustomerFeedbackSurveyDetailsDTO> customerFeedbackQuestionIdDictionary = new Dictionary<int, CustomerFeedbackSurveyDetailsDTO>();
            StringBuilder sb = new StringBuilder("");
            string customerFeedbackQuestionIdList;
            for (int i = 0; i < customerFeedbackSurveyDetailsDTOList.Count; i++)
            {
                if (customerFeedbackSurveyDetailsDTOList[i].CustFbQuestionId == -1 ||
                    customerFeedbackQuestionIdDictionary.ContainsKey(customerFeedbackSurveyDetailsDTOList[i].CustFbQuestionId))
                {
                    continue;
                }
                if (i != 0)
                {
                    sb.Append(",");
                }
                sb.Append(customerFeedbackSurveyDetailsDTOList[i].CustFbQuestionId.ToString());
                customerFeedbackQuestionIdDictionary.Add(customerFeedbackSurveyDetailsDTOList[i].CustFbQuestionId, customerFeedbackSurveyDetailsDTOList[i]);
            }
            customerFeedbackQuestionIdList = sb.ToString();
            CustomerFeedbackQuestionsList customerFeedbackQuestionsList = new CustomerFeedbackQuestionsList(executionContext);
            List<KeyValuePair<CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters, string>> searchQuestionParameters = new List<KeyValuePair<CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters, string>>();
            searchQuestionParameters.Add(new KeyValuePair<CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters, string>(CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters.CUST_FB_QUESTION_ID_LIST, customerFeedbackQuestionIdList.ToString()));
            if (activeChildRecords)
            {
                searchQuestionParameters.Add(new KeyValuePair<CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters, string>(CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters.IS_ACTIVE, "1"));
            }
            List<CustomerFeedbackQuestionsDTO> customerFeedbackQuestionsDTOList = customerFeedbackQuestionsList.GetAllCustomerFeedbackQuestions(searchQuestionParameters, executionContext.GetLanguageId(), true, activeChildRecords, sqlTransaction);
            if (customerFeedbackQuestionsDTOList != null && customerFeedbackQuestionsDTOList.Any())
            {
                log.LogVariableState("CustomerFeedbackQuestionsDTO", customerFeedbackQuestionsDTOList);
                foreach (var customerFeedbackQuestionsDTO in customerFeedbackQuestionsDTOList)
                {
                    if (customerFeedbackQuestionIdDictionary.ContainsKey(customerFeedbackQuestionsDTO.CustFbQuestionId))
                    {
                        if (customerFeedbackQuestionIdDictionary[customerFeedbackQuestionsDTO.CustFbQuestionId].SurveyQuestion == null)
                        {
                            customerFeedbackQuestionIdDictionary[customerFeedbackQuestionsDTO.CustFbQuestionId].SurveyQuestion = new CustomerFeedbackQuestionsDTO();
                        }
                        customerFeedbackQuestionIdDictionary[customerFeedbackQuestionsDTO.CustFbQuestionId].SurveyQuestion = customerFeedbackQuestionsDTO;
                    }
                }
            }
        }

        /// <summary>
        /// Returns the Customer Feedback Survey Details for the initial load list
        /// </summary>
        public List<CustomerFeedbackSurveyDetailsDTO> GetCustomerFeedbackSurveyDetailsOfInitialLoadList(List<KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            CustomerFeedbackSurveyDetailsDataHandler customerFeedbackSurveyDetailsDataHandler = new CustomerFeedbackSurveyDetailsDataHandler(sqlTransaction);
            List<CustomerFeedbackSurveyDetailsDTO> customerFeedbackSurveyDetailsDTOList = customerFeedbackSurveyDetailsDataHandler.GetCustomerFeedbackSurveyDetailsOfInitialLoadList(searchParameters);
            log.LogMethodEntry(customerFeedbackSurveyDetailsDTOList);
            return customerFeedbackSurveyDetailsDTOList;
        }

        /// <summary>
        /// Saves the  list of CustomerFeedbackSurveyDetailsDTO.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (customerFeedbackSurveyDetailsDTOList == null ||
                customerFeedbackSurveyDetailsDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < customerFeedbackSurveyDetailsDTOList.Count; i++)
            {
                var customerFeedbackSurveyDetailsDTO = customerFeedbackSurveyDetailsDTOList[i];
                if (customerFeedbackSurveyDetailsDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    CustomerFeedbackSurveyDetails customerFeedbackSurveyDetails = new CustomerFeedbackSurveyDetails(executionContext, customerFeedbackSurveyDetailsDTO);
                    customerFeedbackSurveyDetails.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving customerFeedbackSurveyDetailsDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("customerFeedbackSurveyDetailsDTO", customerFeedbackSurveyDetailsDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
