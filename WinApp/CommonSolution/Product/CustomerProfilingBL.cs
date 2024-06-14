/********************************************************************************************
 * Project Name - Product
 * Description  - CustomerProfilingBL
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.150.0      24-Mar-2022     Girish Kundar              Created : Check in check out changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Product
{
   public class CustomerProfilingBL
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private CustomerProfilingDTO customerProfilingDTO;
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        private CustomerProfilingBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// CustomerProfilingBL
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="id"></param>
        /// <param name="sqlTransaction"></param>
        public CustomerProfilingBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            LoadCustomerProfilingDTO(id, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="parameterCustomerProfilingDTO">parameterCustomerProfilingDTO</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public CustomerProfilingBL(ExecutionContext executionContext, CustomerProfilingDTO parameterCustomerProfilingDTO, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, parameterCustomerProfilingDTO, sqlTransaction);
            if (parameterCustomerProfilingDTO.CustomerProfilingId > -1)
            {
                customerProfilingDTO = parameterCustomerProfilingDTO;
                Validate(sqlTransaction);
            }
            else
            {
                customerProfilingDTO = new CustomerProfilingDTO(-1, parameterCustomerProfilingDTO.CustomerProfilingGroupId, parameterCustomerProfilingDTO.ProfileType,parameterCustomerProfilingDTO.CompareOperator, parameterCustomerProfilingDTO.ProfileValue,
                                                                  parameterCustomerProfilingDTO.IsActive);
                Validate(sqlTransaction);
            }
            log.LogMethodExit();
        }

        private void ThrowIfUserDTOIsNull(int id)
        {
            log.LogMethodEntry();
            if (customerProfilingDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "Customer Profiling ", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        private void LoadCustomerProfilingDTO(int id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(id, sqlTransaction);
            CustomerProfilingDataHandler customerProfilingDataHandler = new CustomerProfilingDataHandler(sqlTransaction);
            customerProfilingDTO = customerProfilingDataHandler.GetCustomerProfilingDTO(id);
            ThrowIfUserDTOIsNull(id);
            if(customerProfilingDTO.ProfileType >  -1)
            {
                LookupValues lookups = new LookupValues(executionContext, customerProfilingDTO.ProfileType, sqlTransaction);
                if(lookups != null && lookups.LookupValuesDTO != null)
                {
                    customerProfilingDTO.ProfileTypeName = lookups.LookupValuesDTO.LookupName;
                }
            }
            log.LogMethodExit();
        }

        internal void Update(CustomerProfilingDTO parameterCustomerProfilingDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameterCustomerProfilingDTO);
            customerProfilingDTO.CustomerProfilingGroupId = parameterCustomerProfilingDTO.CustomerProfilingGroupId;
            customerProfilingDTO.CompareOperator = parameterCustomerProfilingDTO.CompareOperator;
            customerProfilingDTO.ProfileType = parameterCustomerProfilingDTO.ProfileType;
            customerProfilingDTO.ProfileValue = parameterCustomerProfilingDTO.ProfileValue;
            customerProfilingDTO.IsActive = parameterCustomerProfilingDTO.IsActive;
            log.LogMethodExit();
        }

        private void Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            log.LogMethodExit();
        }

        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            CustomerProfilingDataHandler customerProfilingDataHandler = new CustomerProfilingDataHandler(sqlTransaction);
            if (customerProfilingDTO.CustomerProfilingId < 0)
            {
                customerProfilingDTO = customerProfilingDataHandler.Insert(customerProfilingDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                customerProfilingDTO.AcceptChanges();
            }
            else
            {
                if (customerProfilingDTO.IsChanged)
                {
                    customerProfilingDTO = customerProfilingDataHandler.Update(customerProfilingDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    customerProfilingDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// get CustomerProfilingDTO Object
        /// </summary>
        public CustomerProfilingDTO CustomerProfilingDTO
        {
            get
            {
                CustomerProfilingDTO result = new CustomerProfilingDTO(customerProfilingDTO);
                return result;
            }
        }
    }

    /// <summary>
    /// CustomerProfilingListBL list class for Customer Profile details
    /// </summary>
    public class CustomerProfilingListBL
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<CustomerProfilingDTO> customerProfilingDTOList;

        /// <summary>
        /// default constructor
        /// </summary>
        public CustomerProfilingListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// parameterized constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public CustomerProfilingListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="customerProfilingDTOList"></param>
        public CustomerProfilingListBL(ExecutionContext executionContext, List<CustomerProfilingDTO> customerProfilingDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, customerProfilingDTOList);
            this.customerProfilingDTOList = customerProfilingDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// GetCashdrawers
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns> List of CustomerProfilingDTO</returns>
        public List<CustomerProfilingDTO> GetCustomerProfilingGroups(List<KeyValuePair<CustomerProfilingDTO.SearchByParameters, string>> searchParameters,
                                                                     SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            CustomerProfilingDataHandler customerProfilingDataHandler = new CustomerProfilingDataHandler(sqlTransaction);
            List<CustomerProfilingDTO> customerProfilingDTOList = customerProfilingDataHandler.GetCustomerProfilings(searchParameters);
            if (customerProfilingDTOList != null && customerProfilingDTOList.Any())
            {
                foreach (CustomerProfilingDTO customerProfilingDTO in customerProfilingDTOList)
                {
                    if (customerProfilingDTO.ProfileType > -1)
                    {
                        LookupValues lookups = new LookupValues(executionContext, customerProfilingDTO.ProfileType, sqlTransaction);
                        if (lookups != null && lookups.LookupValuesDTO != null)
                        {
                            customerProfilingDTO.ProfileTypeName = lookups.LookupValuesDTO.LookupName;
                        }
                    }
                }
            }
            log.LogMethodExit(customerProfilingDTOList);
            return customerProfilingDTOList;
        }

        /// <summary>
        /// Save List
        /// </summary>
        /// <returns></returns>
        public List<CustomerProfilingDTO> Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<CustomerProfilingDTO> savedCustomerProfilingDTOList = new List<CustomerProfilingDTO>();
            try
            {
                if (customerProfilingDTOList != null && customerProfilingDTOList.Any())
                {
                    foreach (CustomerProfilingDTO customerProfilingGroupDTO in customerProfilingDTOList)
                    {
                        CustomerProfilingBL customerProfilingGroupBL = new CustomerProfilingBL(executionContext, customerProfilingGroupDTO);
                        customerProfilingGroupBL.Save(sqlTransaction);
                        savedCustomerProfilingDTOList.Add(customerProfilingGroupBL.CustomerProfilingDTO);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                log.Error(sqlEx);
                log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                if (sqlEx.Number == 547)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                }
                if (sqlEx.Number == 2601)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1872));
                }
                else
                {
                    throw;
                }
            }
            catch (ValidationException valEx)
            {
                log.Error(valEx);
                log.LogMethodExit("Throwing Validation Exception : " + valEx.Message);
                throw;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit("Throwing Exception : " + ex.Message);
                throw;
            }
            log.LogMethodExit(savedCustomerProfilingDTOList);
            return savedCustomerProfilingDTOList;
        }

        public List<CustomerProfilingDTO> GetCustomerProfilingDTOList(List<int> idList, bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(idList, activeRecords, sqlTransaction);
            CustomerProfilingDataHandler customerProfilingDataHandler = new CustomerProfilingDataHandler(sqlTransaction);
            List<CustomerProfilingDTO> customerProfilingDTOList = customerProfilingDataHandler.GetCustomerProfilingDTOList(idList, activeRecords);
            log.LogMethodExit(customerProfilingDTOList);
            return customerProfilingDTOList;
        }
    }
}
