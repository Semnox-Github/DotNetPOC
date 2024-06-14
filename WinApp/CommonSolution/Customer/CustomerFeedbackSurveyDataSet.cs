/********************************************************************************************
 * Project Name - Customer Feedback Survey Data Set
 * Description  - Bussiness logic of customer feedback survey data set
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        13-Dec-2016   Raghuveera        Created 
 *2.70.2      19-Jul-2019   Girish Kundar     Modified : Save() method. Now Insert/Update method returns the DTO instead of Id. 
 *2.70.3      20-Feb-2020   Girish Kundar     Modified : Added Build method and updated Save() method , Saves the Child List
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
    /// Business logic for Customer Feedback Survey Data Set
    /// </summary>
    public class CustomerFeedbackSurveyDataSet
    {
        private Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private CustomerFeedbackSurveyDataSetDTO customerFeedbackSurveyDataSetDTO;
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Default constructor
        /// </summary>
        private CustomerFeedbackSurveyDataSet(ExecutionContext excecutionContext)
        {
            log.LogMethodEntry();
            this.executionContext = excecutionContext;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="customerFeedbackSurveyDataSetDTO">Parameter of the type CustomerFeedbackSurveyDataSetDTO</param>
        public CustomerFeedbackSurveyDataSet(ExecutionContext excecutionContext,
                                             CustomerFeedbackSurveyDataSetDTO customerFeedbackSurveyDataSetDTO)
            : this(excecutionContext)
        {
            log.LogMethodEntry(customerFeedbackSurveyDataSetDTO);
            this.customerFeedbackSurveyDataSetDTO = customerFeedbackSurveyDataSetDTO;
            log.LogMethodExit();
        }

        private void Build(bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            CustomerFeedbackSurveyDataList customerFeedbackSurveyDataListBL = new CustomerFeedbackSurveyDataList(executionContext);
            List<KeyValuePair<CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters, string>> searchParameters = new List<KeyValuePair<CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters, string>>();
            searchParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters, string>(CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters.CUST_FB_SURVEY_DATASET_ID, customerFeedbackSurveyDataSetDTO.CustFbSurveyDataSetId.ToString()));
            if (activeChildRecords)
            {
                searchParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters, string>(CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters.IS_ACTIVE, "1"));
            }
            customerFeedbackSurveyDataSetDTO.CustomerFeedbackSurveyDataDTOList = customerFeedbackSurveyDataListBL.GetAllCustomerFeedbackSurveyData(searchParameters, sqlTransaction);
            //Mapping
            CustomerFeedbackSurveyMappingList customerFeedbackSurveyMappingList = new CustomerFeedbackSurveyMappingList(executionContext);
            List<KeyValuePair<CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters, string>> psearchParameters = new List<KeyValuePair<CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters, string>>();
            psearchParameters.Add(new KeyValuePair<CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters, string>(CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters.CUST_FB_SURVEY_DATA_SET_ID, customerFeedbackSurveyDataSetDTO.CustFbSurveyDataSetId.ToString()));
            if (activeChildRecords)
            {
                psearchParameters.Add(new KeyValuePair<CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters, string>(CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters.IS_ACTIVE, "1"));
            }
            customerFeedbackSurveyDataSetDTO.CustomerFeedbackSurveyMappingDTOList = customerFeedbackSurveyMappingList.GetAllCustomerFeedbackSurveyMapping(psearchParameters, sqlTransaction);
            log.LogMethodExit();
        }

        public CustomerFeedbackSurveyDataSet(ExecutionContext executionContext, int customerFeedbackSurveyDataSetId,
                                         bool loadChildRecords = false,
                                         bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(customerFeedbackSurveyDataSetId, sqlTransaction);
            CustomerFeedbackSurveyDataSetDataHandler customerFeedbackSurveyDataSetDataHandler = new CustomerFeedbackSurveyDataSetDataHandler(sqlTransaction);
            customerFeedbackSurveyDataSetDTO = customerFeedbackSurveyDataSetDataHandler.GetCustomerFeedbackSurveyDataSet(customerFeedbackSurveyDataSetId);
            if (customerFeedbackSurveyDataSetDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "customerFeedbackSurveyDataSetDTO", customerFeedbackSurveyDataSetId);
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
        /// Saves the CustomerFeedbackSurveyDataSet
        /// CustomerFeedbackSurveyDataSet will be inserted if CustFbSurveyDataSetId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (customerFeedbackSurveyDataSetDTO.IsChangedRecursive == false
                && customerFeedbackSurveyDataSetDTO.CustFbSurveyDataSetId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            CustomerFeedbackSurveyDataSetDataHandler customerFeedbackSurveyDataSetDataHandler = new CustomerFeedbackSurveyDataSetDataHandler(sqlTransaction);
            if (customerFeedbackSurveyDataSetDTO.CustFbSurveyDataSetId <= 0)
            {
                customerFeedbackSurveyDataSetDTO = customerFeedbackSurveyDataSetDataHandler.InsertCustomerFeedbackSurveyDataSet(customerFeedbackSurveyDataSetDTO, executionContext.GetUserId(), executionContext.GetSiteId(), sqlTransaction);
                customerFeedbackSurveyDataSetDTO.AcceptChanges();
            }
            else
            {
                if (customerFeedbackSurveyDataSetDTO.IsChanged)
                {
                    customerFeedbackSurveyDataSetDTO = customerFeedbackSurveyDataSetDataHandler.UpdateCustomerFeedbackSurveyDataSet(customerFeedbackSurveyDataSetDTO, executionContext.GetUserId(), executionContext.GetSiteId(), sqlTransaction);
                    customerFeedbackSurveyDataSetDTO.AcceptChanges();
                }
            }
            if (customerFeedbackSurveyDataSetDTO.CustomerFeedbackSurveyDataDTOList != null &&
                  customerFeedbackSurveyDataSetDTO.CustomerFeedbackSurveyDataDTOList.Count != 0)
            {
                foreach (CustomerFeedbackSurveyDataDTO customerFeedbackSurveyDataDTO in customerFeedbackSurveyDataSetDTO.CustomerFeedbackSurveyDataDTOList)
                {
                    if (customerFeedbackSurveyDataDTO.IsChanged)
                    {
                        customerFeedbackSurveyDataDTO.CustFbSurveyDataSetId = customerFeedbackSurveyDataSetDTO.CustFbSurveyDataSetId;
                        CustomerFeedbackSurveyData customerFeedbackSurveyDataBL = new CustomerFeedbackSurveyData(executionContext, customerFeedbackSurveyDataDTO);
                        customerFeedbackSurveyDataBL.Save(sqlTransaction);
                    }
                }
            }
            if (customerFeedbackSurveyDataSetDTO.CustomerFeedbackSurveyMappingDTOList != null &&
                  customerFeedbackSurveyDataSetDTO.CustomerFeedbackSurveyMappingDTOList.Count != 0)
            {
                foreach (CustomerFeedbackSurveyMappingDTO customerFeedbackSurveyMappingDTO in customerFeedbackSurveyDataSetDTO.CustomerFeedbackSurveyMappingDTOList)
                {
                    if (customerFeedbackSurveyMappingDTO.IsChanged)
                    {
                        customerFeedbackSurveyMappingDTO.CustFbSurveyDataSetId = customerFeedbackSurveyDataSetDTO.CustFbSurveyDataSetId;
                        CustomerFeedbackSurveyMapping customerFeedbackSurveyMappingBL = new CustomerFeedbackSurveyMapping(executionContext, customerFeedbackSurveyMappingDTO);
                        customerFeedbackSurveyMappingBL.Save(sqlTransaction);
                    }
                }
            }
            log.LogMethodExit();
        }

        public CustomerFeedbackSurveyDataSetDTO CustomerFeedbackSurveyDataSetDTO
        {
            get
            {
                return customerFeedbackSurveyDataSetDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of segment categorization
    /// </summary>
    public class CustomerFeedbackSurveyDataSetList
    {
        private Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<CustomerFeedbackSurveyDataSetDTO> customerFeedbackSurveyDataSetDTOList = new List<CustomerFeedbackSurveyDataSetDTO>();
        public CustomerFeedbackSurveyDataSetList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public CustomerFeedbackSurveyDataSetList(ExecutionContext executionContext,
                                   List<CustomerFeedbackSurveyDataSetDTO> customerFeedbackSurveyDataSetDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.customerFeedbackSurveyDataSetDTOList = customerFeedbackSurveyDataSetDTOList;
            log.LogMethodExit();
        }



        /// <summary>
        /// Returns the CustomerFeedbackSurveyDataSet  List
        /// </summary>
        public List<CustomerFeedbackSurveyDataSetDTO> GetAllCustomerFeedbackSurveyDataSetList(List<KeyValuePair<CustomerFeedbackSurveyDataSetDTO.SearchByCustomerFeedbackSurveyDataSetParameters, string>> searchParameters,
                                          bool loadChildRecords = false, bool activeChildRecords = true,
                                          SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            CustomerFeedbackSurveyDataSetDataHandler customerFeedbackSurveyDataSetDataHandler = new CustomerFeedbackSurveyDataSetDataHandler(sqlTransaction);
            List<CustomerFeedbackSurveyDataSetDTO> customerFeedbackSurveyDataSetDTOList = customerFeedbackSurveyDataSetDataHandler.GetCustomerFeedbackSurveyDataSetList(searchParameters);
            if (customerFeedbackSurveyDataSetDTOList != null && customerFeedbackSurveyDataSetDTOList.Any() && loadChildRecords)
            {
                Build(customerFeedbackSurveyDataSetDTOList, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(customerFeedbackSurveyDataSetDTOList);
            return customerFeedbackSurveyDataSetDTOList;
        }

        private void Build(List<CustomerFeedbackSurveyDataSetDTO> customerFeedbackSurveyDataSetDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(customerFeedbackSurveyDataSetDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, CustomerFeedbackSurveyDataSetDTO> customerFeedbackSurveyDataSetIdDictionary = new Dictionary<int, CustomerFeedbackSurveyDataSetDTO>();
            StringBuilder sb = new StringBuilder("");
            string customerFeedbackSurveyDataSetIdList;
            for (int i = 0; i < customerFeedbackSurveyDataSetDTOList.Count; i++)
            {
                if (customerFeedbackSurveyDataSetDTOList[i].CustFbSurveyDataSetId == -1 ||
                    customerFeedbackSurveyDataSetIdDictionary.ContainsKey(customerFeedbackSurveyDataSetDTOList[i].CustFbSurveyDataSetId))
                {
                    continue;
                }
                if (i != 0)
                {
                    sb.Append(",");
                }
                sb.Append(customerFeedbackSurveyDataSetDTOList[i].CustFbSurveyDataSetId.ToString());
                customerFeedbackSurveyDataSetIdDictionary.Add(customerFeedbackSurveyDataSetDTOList[i].CustFbSurveyDataSetId, customerFeedbackSurveyDataSetDTOList[i]);
            }
            customerFeedbackSurveyDataSetIdList = sb.ToString();
            CustomerFeedbackSurveyDataList customerFeedbackSurveyDataListBL = new CustomerFeedbackSurveyDataList(executionContext);
            List<KeyValuePair<CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters, string>> searchParameters = new List<KeyValuePair<CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters, string>>();
            searchParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters, string>(CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters.CUST_FB_SURVEY_DATASET_ID_LIST, customerFeedbackSurveyDataSetIdList.ToString()));
            if (activeChildRecords)
            {
                searchParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters, string>(CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters.IS_ACTIVE, "1"));
            }
            List<CustomerFeedbackSurveyDataDTO> customerFeedbackSurveyDataDTOList = customerFeedbackSurveyDataListBL.GetAllCustomerFeedbackSurveyData(searchParameters, sqlTransaction);
            if (customerFeedbackSurveyDataDTOList != null && customerFeedbackSurveyDataDTOList.Any())
            {
                log.LogVariableState("customerFeedbackSurveyDataDTOList", customerFeedbackSurveyDataDTOList);
                foreach (var customerFeedbackSurveyDataDTO in customerFeedbackSurveyDataDTOList)
                {
                    if (customerFeedbackSurveyDataSetIdDictionary.ContainsKey(customerFeedbackSurveyDataDTO.CustFbSurveyDataSetId))
                    {
                        if (customerFeedbackSurveyDataSetIdDictionary[customerFeedbackSurveyDataDTO.CustFbSurveyDataSetId].CustomerFeedbackSurveyDataDTOList == null)
                        {
                            customerFeedbackSurveyDataSetIdDictionary[customerFeedbackSurveyDataDTO.CustFbSurveyDataSetId].CustomerFeedbackSurveyDataDTOList = new List<CustomerFeedbackSurveyDataDTO>();
                        }
                        customerFeedbackSurveyDataSetIdDictionary[customerFeedbackSurveyDataDTO.CustFbSurveyDataSetId].CustomerFeedbackSurveyDataDTOList.Add(customerFeedbackSurveyDataDTO);
                    }
                }
            }


            CustomerFeedbackSurveyMappingList customerFeedbackSurveyMappingListBL = new CustomerFeedbackSurveyMappingList(executionContext);
            List<KeyValuePair<CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters, string>> psearchParameters = new List<KeyValuePair<CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters, string>>();
            psearchParameters.Add(new KeyValuePair<CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters, string>(CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters.CUST_FB_SURVEY_DATA_SET_ID_LIST, customerFeedbackSurveyDataSetIdList.ToString()));
            if (activeChildRecords)
            {
                psearchParameters.Add(new KeyValuePair<CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters, string>(CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters.IS_ACTIVE, "1"));
            }
            List<CustomerFeedbackSurveyMappingDTO> customerFeedbackSurveyMappingDTOList = customerFeedbackSurveyMappingListBL.GetAllCustomerFeedbackSurveyMapping(psearchParameters, sqlTransaction);
            if (customerFeedbackSurveyMappingDTOList != null && customerFeedbackSurveyMappingDTOList.Any())
            {
                log.LogVariableState("customerFeedbackSurveyMappingDTOList", customerFeedbackSurveyMappingDTOList);
                foreach (var customerFeedbackSurveyMappingDTO in customerFeedbackSurveyMappingDTOList)
                {
                    if (customerFeedbackSurveyDataSetIdDictionary.ContainsKey(customerFeedbackSurveyMappingDTO.CustFbSurveyDataSetId))
                    {
                        if (customerFeedbackSurveyDataSetIdDictionary[customerFeedbackSurveyMappingDTO.CustFbSurveyDataSetId].CustomerFeedbackSurveyMappingDTOList == null)
                        {
                            customerFeedbackSurveyDataSetIdDictionary[customerFeedbackSurveyMappingDTO.CustFbSurveyDataSetId].CustomerFeedbackSurveyMappingDTOList = new List<CustomerFeedbackSurveyMappingDTO>();
                        }
                        customerFeedbackSurveyDataSetIdDictionary[customerFeedbackSurveyMappingDTO.CustFbSurveyDataSetId].CustomerFeedbackSurveyMappingDTOList.Add(customerFeedbackSurveyMappingDTO);
                    }
                }
            }
            log.LogMethodExit();
        }

        public List<CustomerFeedbackSurveyDataSetDTO> Save(SqlTransaction sqlTransaction = null)
        {
            try
            {
                log.LogMethodEntry(sqlTransaction);
                List<CustomerFeedbackSurveyDataSetDTO> customerFeedbackSurveyDataSetList = null;
                if (customerFeedbackSurveyDataSetDTOList != null)
                {
                    customerFeedbackSurveyDataSetList = new List<CustomerFeedbackSurveyDataSetDTO>();
                    foreach (CustomerFeedbackSurveyDataSetDTO customerFeedbackSurveyDataSetDTO in customerFeedbackSurveyDataSetDTOList)
                    {
                        CustomerFeedbackSurveyDataSet customerFeedbackSurveyDataSetBL = new CustomerFeedbackSurveyDataSet(executionContext, customerFeedbackSurveyDataSetDTO);
                        customerFeedbackSurveyDataSetBL.Save(sqlTransaction);
                        customerFeedbackSurveyDataSetList.Add(customerFeedbackSurveyDataSetBL.CustomerFeedbackSurveyDataSetDTO);
                    }
                }
                log.LogMethodExit(customerFeedbackSurveyDataSetList);
                return customerFeedbackSurveyDataSetList;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }
    }
}
