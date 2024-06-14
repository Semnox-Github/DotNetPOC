/********************************************************************************************
 * Project Name - Product
 * Description  - CustomerProfilingGroupBL
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
    public class CustomerProfilingGroupBL
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private CustomerProfilingGroupDTO customerProfilingGroupDTO;
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        private CustomerProfilingGroupBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// CustomerProfilingGroupBL
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="id">id</param>
        /// <param name="buildChildRecords">buildChildRecords</param>
        /// <param name="activeChildRecords">activeChildRecords</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public CustomerProfilingGroupBL(ExecutionContext executionContext, int id, bool buildChildRecords = false,
                                         bool activeChildRecords = false, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, buildChildRecords, activeChildRecords, sqlTransaction);
            LoadCustomerProfilingGroupDTO(id, activeChildRecords, activeChildRecords, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="parameterCustomerProfilingGroupDTO">parameterCustomerProfilingGroupDTO</param>
        /// <param name="buildChildRecords">buildChildRecords</param>
        /// <param name="activeChildRecords">activeChildRecords</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public CustomerProfilingGroupBL(ExecutionContext executionContext, CustomerProfilingGroupDTO parameterCustomerProfilingGroupDTO,
                                        bool buildChildRecords = false, bool activeChildRecords = false,
                                        SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, parameterCustomerProfilingGroupDTO, buildChildRecords, activeChildRecords, sqlTransaction);
            if (parameterCustomerProfilingGroupDTO.CustomerProfilingGroupId > -1)
            {
                LoadCustomerProfilingGroupDTO(parameterCustomerProfilingGroupDTO.CustomerProfilingGroupId,
                                               buildChildRecords, activeChildRecords, sqlTransaction);
                ThrowIfUserDTOIsNull(parameterCustomerProfilingGroupDTO.CustomerProfilingGroupId);
                Update(parameterCustomerProfilingGroupDTO);
            }
            else
            {
                customerProfilingGroupDTO = new CustomerProfilingGroupDTO(-1, parameterCustomerProfilingGroupDTO.GroupName, parameterCustomerProfilingGroupDTO.IsActive);
                Validate(sqlTransaction);
            }
            log.LogMethodExit();
        }

        private void ThrowIfUserDTOIsNull(int id)
        {
            log.LogMethodEntry(id);
            if (customerProfilingGroupDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "Customer Profiling Group", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        private void LoadCustomerProfilingGroupDTO(int id, bool buildChildRecords, bool activeChildRecords,
                                                           SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(id, buildChildRecords, activeChildRecords, sqlTransaction);
            CustomerProfilingGroupDataHandler customerProfilingGroupDataHandler = new CustomerProfilingGroupDataHandler(sqlTransaction);
            customerProfilingGroupDTO = customerProfilingGroupDataHandler.GetCustomerProfilingGroupDTO(id);
            ThrowIfUserDTOIsNull(id);
            if (buildChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }

        private void Build(bool activeChildRecords, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            CustomerProfilingListBL customerProfilingListBL = new CustomerProfilingListBL(executionContext);
            customerProfilingGroupDTO.CustomerProfilingDTOList = customerProfilingListBL.GetCustomerProfilingDTOList(new List<int>() { customerProfilingGroupDTO.CustomerProfilingGroupId }, activeChildRecords, sqlTransaction);
            log.LogMethodExit();
        }

        private void Update(CustomerProfilingGroupDTO parameterCustomerProfilingGroupDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameterCustomerProfilingGroupDTO, sqlTransaction);
            customerProfilingGroupDTO.CustomerProfilingGroupId = parameterCustomerProfilingGroupDTO.CustomerProfilingGroupId;
            customerProfilingGroupDTO.GroupName = parameterCustomerProfilingGroupDTO.GroupName;
            customerProfilingGroupDTO.IsActive = parameterCustomerProfilingGroupDTO.IsActive;
            Dictionary<int, CustomerProfilingDTO> customerProfilingDTODictionary = new Dictionary<int, CustomerProfilingDTO>();
            if (customerProfilingGroupDTO.CustomerProfilingDTOList != null &&
                customerProfilingGroupDTO.CustomerProfilingDTOList.Any())
            {
                foreach (var customerProfilingDTO in customerProfilingGroupDTO.CustomerProfilingDTOList)
                {
                    customerProfilingDTODictionary.Add(customerProfilingDTO.CustomerProfilingId, customerProfilingDTO);
                }
            }
            if (parameterCustomerProfilingGroupDTO.CustomerProfilingDTOList != null &&
                parameterCustomerProfilingGroupDTO.CustomerProfilingDTOList.Any())
            {
                foreach (var parameterCustomerProfilingDTO in parameterCustomerProfilingGroupDTO.CustomerProfilingDTOList)
                {
                    if (customerProfilingDTODictionary.ContainsKey(parameterCustomerProfilingDTO.CustomerProfilingId))
                    {
                        CustomerProfilingBL customerProfilingBL = new CustomerProfilingBL(executionContext, customerProfilingDTODictionary[parameterCustomerProfilingDTO.CustomerProfilingId]);
                        customerProfilingBL.Update(parameterCustomerProfilingDTO, sqlTransaction);
                    }
                    else if (parameterCustomerProfilingDTO.CustomerProfilingId > -1)
                    {
                        CustomerProfilingBL customerProfilingBL = new CustomerProfilingBL(executionContext, parameterCustomerProfilingDTO.CustomerProfilingId, sqlTransaction);
                        if (customerProfilingGroupDTO.CustomerProfilingDTOList == null)
                        {
                            customerProfilingGroupDTO.CustomerProfilingDTOList = new List<CustomerProfilingDTO>();
                        }
                        customerProfilingGroupDTO.CustomerProfilingDTOList.Add(customerProfilingBL.CustomerProfilingDTO);
                        customerProfilingBL.Update(parameterCustomerProfilingDTO, sqlTransaction);
                    }
                    else
                    {
                        CustomerProfilingBL customerProfilingBL = new CustomerProfilingBL(executionContext, parameterCustomerProfilingDTO);
                        if (customerProfilingGroupDTO.CustomerProfilingDTOList == null)
                        {
                            customerProfilingGroupDTO.CustomerProfilingDTOList = new List<CustomerProfilingDTO>();
                        }
                        customerProfilingGroupDTO.CustomerProfilingDTOList.Add(customerProfilingBL.CustomerProfilingDTO);
                    }
                }
            }
            log.LogMethodExit();
        }

        private void Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            log.LogMethodExit();
        }

        public bool ValidateAgeProfiling(DateTime dateOfBirth)
        {
            log.LogMethodEntry(dateOfBirth);
            bool valid = false;
            int age = 0;
            if (dateOfBirth != DateTime.MinValue &&
                                ((DateTime.Today.Year - Convert.ToDateTime(dateOfBirth).Year) < 100) &&
                                ((Convert.ToDateTime(dateOfBirth).Year) != 1900))
                age = DateTime.Today.Year - Convert.ToDateTime(dateOfBirth).Year;
            log.Debug("age: " + age);


            log.LogMethodExit(valid);
            return valid;
        }

        public bool ValidateHeightProfiling(decimal height)
        {
            log.LogMethodEntry(height);
            bool valid = true;
            log.LogMethodExit(valid);
            return valid;
        }

        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            CustomerProfilingGroupDataHandler customerProfilingGroupDataHandler = new CustomerProfilingGroupDataHandler(sqlTransaction);
            if (customerProfilingGroupDTO.CustomerProfilingGroupId < 0)
            {
                customerProfilingGroupDTO = customerProfilingGroupDataHandler.Insert(customerProfilingGroupDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                customerProfilingGroupDTO.AcceptChanges();
            }
            else
            {
                if (customerProfilingGroupDTO.IsChanged)
                {
                    customerProfilingGroupDTO = customerProfilingGroupDataHandler.Update(customerProfilingGroupDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    customerProfilingGroupDTO.AcceptChanges();
                }
            }
            if (customerProfilingGroupDTO.CustomerProfilingDTOList != null && customerProfilingGroupDTO.CustomerProfilingDTOList.Count > 0)
            {
                foreach (CustomerProfilingDTO customerProfilingDTO in customerProfilingGroupDTO.CustomerProfilingDTOList)
                {
                    if (customerProfilingDTO.CustomerProfilingGroupId != customerProfilingGroupDTO.CustomerProfilingGroupId)
                    {
                        customerProfilingDTO.CustomerProfilingGroupId = customerProfilingGroupDTO.CustomerProfilingGroupId;
                    }
                }
                CustomerProfilingListBL customerProfilingListBL = new CustomerProfilingListBL(executionContext, customerProfilingGroupDTO.CustomerProfilingDTOList);
                customerProfilingGroupDTO.CustomerProfilingDTOList = customerProfilingListBL.Save(sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// get CustomerProfilingGroupDTO Object
        /// </summary>
        public CustomerProfilingGroupDTO CustomerProfilingGroupDTO
        {
            get
            {
                CustomerProfilingGroupDTO result = new CustomerProfilingGroupDTO(customerProfilingGroupDTO);
                return result;
            }
        }
    }

    /// <summary>
    /// CustomerProfilingGroupListBL list class for CustomerProfileGroup
    /// </summary>
    public class CustomerProfilingGroupListBL
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<CustomerProfilingGroupDTO> customerProfilingGroupDTOList;

        /// <summary>
        /// default constructor
        /// </summary>
        public CustomerProfilingGroupListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// parameterized constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public CustomerProfilingGroupListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="customerProfilingGroupDTOList">customerProfilingGroupDTOList</param>
        public CustomerProfilingGroupListBL(ExecutionContext executionContext, List<CustomerProfilingGroupDTO> customerProfilingGroupDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, customerProfilingGroupDTOList);
            this.customerProfilingGroupDTOList = customerProfilingGroupDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// GetCashdrawers
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns>customerProfilingGroupDTOList</returns>
        public List<CustomerProfilingGroupDTO> GetCustomerProfilingGroups(List<KeyValuePair<CustomerProfilingGroupDTO.SearchByParameters, string>> searchParameters,
                                                                                            bool buildChildRecords, bool activeChildRecords, SqlTransaction sqlTransaction = null)
        { 
            log.LogMethodEntry(searchParameters, buildChildRecords, activeChildRecords, sqlTransaction);
            CustomerProfilingGroupDataHandler customerProfilingGroupDataHandler = new CustomerProfilingGroupDataHandler(sqlTransaction);
            List<CustomerProfilingGroupDTO> customerProfilingGroupDTOList = customerProfilingGroupDataHandler.GetCustomerProfilingGroups(searchParameters);
            if (buildChildRecords)
            {
                Build(customerProfilingGroupDTOList, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(customerProfilingGroupDTOList);
            return customerProfilingGroupDTOList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<CustomerProfilingGroupDTO> Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<CustomerProfilingGroupDTO> savedCustomerProfilingGroupDTOList = new List<CustomerProfilingGroupDTO>();
            try
            {
                if (customerProfilingGroupDTOList != null && customerProfilingGroupDTOList.Any())
                {
                    foreach (CustomerProfilingGroupDTO customerProfilingGroupDTO in customerProfilingGroupDTOList)
                    {
                        CustomerProfilingGroupBL customerProfilingGroupBL = new CustomerProfilingGroupBL(executionContext, customerProfilingGroupDTO, true);
                        customerProfilingGroupBL.Save(sqlTransaction);
                        savedCustomerProfilingGroupDTOList.Add(customerProfilingGroupBL.CustomerProfilingGroupDTO);
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
            log.LogMethodExit(savedCustomerProfilingGroupDTOList);
            return savedCustomerProfilingGroupDTOList;
        }

        private void Build(List<CustomerProfilingGroupDTO> customerProfilingGroupDTOList, bool activeChildRecords, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(customerProfilingGroupDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, CustomerProfilingGroupDTO> customerProfilingGroupIdIdUserDTODictionary = new Dictionary<int, CustomerProfilingGroupDTO>();
            List<int> profilegroupIdList = new List<int>();

            if (customerProfilingGroupDTOList != null && customerProfilingGroupDTOList.Any())
            {
                for (int i = 0; i < customerProfilingGroupDTOList.Count; i++)
                {
                    if (customerProfilingGroupDTOList[i].CustomerProfilingGroupId == -1 ||
                        customerProfilingGroupIdIdUserDTODictionary.ContainsKey(customerProfilingGroupDTOList[i].CustomerProfilingGroupId))
                    {
                        continue;
                    }
                    
                    profilegroupIdList.Add(customerProfilingGroupDTOList[i].CustomerProfilingGroupId);
                    customerProfilingGroupIdIdUserDTODictionary.Add(customerProfilingGroupDTOList[i].CustomerProfilingGroupId, customerProfilingGroupDTOList[i]);
                }
                CustomerProfilingListBL customerProfilingListBL = new CustomerProfilingListBL();
                List<CustomerProfilingDTO> customerProfilingDTOList = customerProfilingListBL.GetCustomerProfilingDTOList(profilegroupIdList, activeChildRecords, sqlTransaction);
                if (customerProfilingDTOList != null && customerProfilingDTOList.Any())
                {
                    log.LogVariableState("customerProfilingDTOList", customerProfilingDTOList);
                    foreach (CustomerProfilingDTO customerProfilingDTO in customerProfilingDTOList)
                    {
                        if (customerProfilingGroupIdIdUserDTODictionary.ContainsKey(customerProfilingDTO.CustomerProfilingGroupId))
                        {
                            if (customerProfilingGroupIdIdUserDTODictionary[customerProfilingDTO.CustomerProfilingGroupId].CustomerProfilingDTOList == null)
                            {
                                customerProfilingGroupIdIdUserDTODictionary[customerProfilingDTO.CustomerProfilingGroupId].CustomerProfilingDTOList = new List<CustomerProfilingDTO>();
                            }
                            customerProfilingGroupIdIdUserDTODictionary[customerProfilingDTO.CustomerProfilingGroupId].CustomerProfilingDTOList.Add(customerProfilingDTO);
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
    }
}
