/********************************************************************************************
 * Project Name - CustomerRelationship BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        06-Feb-2017      Lakshminarayana     Created 
 *2.70.2      19-Jul-2019      Girish Kundar       Modified : Save() method. Now Insert/Update method returns the DTO instead of Id.
 *2.70.2      17-Dec-2019      Jinto Thomas        Added parameter executioncontext for userrole declaration
 *2.90        21-May-2020      Girish Kundar       Modified : Made default constructor as Private 
 *2.140.0     14-Sep-2021      Guru S A            Waiver mapping UI enhancements
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer.Membership;
using Semnox.Parafait.User;
using Semnox.Parafait.DBSynch;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// Business logic for CustomerRelationship class.
    /// </summary>
    public class CustomerRelationshipBL
    {
        private CustomerRelationshipDTO customerRelationshipDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of CustomerRelationshipBL class
        /// </summary>
        private CustomerRelationshipBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the customerRelationship id as the parameter
        /// Would fetch the customerRelationship object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public CustomerRelationshipBL(ExecutionContext executionContext, int id, bool buildChildRecords = true, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            CustomerRelationshipDataHandler customerRelationshipDataHandler = new CustomerRelationshipDataHandler(sqlTransaction);
            customerRelationshipDTO = customerRelationshipDataHandler.GetCustomerRelationshipDTO(id);
            if (customerRelationshipDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "CustomerRelationship", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (activeChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates CustomerRelationshipBL object using the CustomerRelationshipDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="customerRelationshipDTO">CustomerRelationshipDTO object</param>
        public CustomerRelationshipBL(ExecutionContext executionContext, CustomerRelationshipDTO customerRelationshipDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, customerRelationshipDTO);
            this.customerRelationshipDTO = customerRelationshipDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the child records for CustomerRelationship object.
        /// </summary>
        /// <param name="activeChildRecords">activeChildRecords holds either true or false</param>
        /// <param name="sqlTransaction"sqlTransaction></param>
        private void Build(bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            //List<CustomerRelationshipDTO> customerRelationshipDTOList = new List<CustomerRelationshipDTO>();
            //List<KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>> searchParameter = new List<KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>>();
            //searchParameter.Add(new KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>(CustomerRelationshipDTO.SearchByParameters.CUSTOMER_ID, customerRelationshipDTO.));
            CustomerBL customerBL = new CustomerBL(executionContext, customerRelationshipDTO.CustomerId, false, false, sqlTransaction);
            customerRelationshipDTO.CustomerDTO = customerBL.CustomerDTO;
            customerBL = new CustomerBL(executionContext, customerRelationshipDTO.RelatedCustomerId, false, false, sqlTransaction);
            customerRelationshipDTO.RelatedCustomerDTO = customerBL.CustomerDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the CustomerRelationship
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if(customerRelationshipDTO.IsActive)
            {
                List<ValidationError> validationErrorList = Validate(sqlTransaction);
                if (validationErrorList.Count > 0)
                {
                    throw new ValidationException("Validation Failed", validationErrorList);
                }
            }
            CustomerRelationshipDataHandler customerRelationshipDataHandler = new CustomerRelationshipDataHandler(sqlTransaction);
            if (customerRelationshipDTO.Id < 0)
            {
                customerRelationshipDTO = customerRelationshipDataHandler.InsertCustomerRelationship(customerRelationshipDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                customerRelationshipDTO.AcceptChanges();
                CreateRoamingData(sqlTransaction);
            }
            else
            {
                if (customerRelationshipDTO.IsChanged)
                {
                    customerRelationshipDTO = customerRelationshipDataHandler.UpdateCustomerRelationship(customerRelationshipDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    customerRelationshipDTO.AcceptChanges();
                    CreateRoamingData(sqlTransaction);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public CustomerRelationshipDTO CustomerRelationshipDTO
        {
            get
            {
                return customerRelationshipDTO;
            }
        }

        /// <summary>
        /// Validates the customer relationship DTO
        /// </summary>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            if (customerRelationshipDTO.CustomerRelationshipTypeId < 0)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Customer Relationship Type"));
                validationErrorList.Add(new ValidationError("CustomerRelationship", "CustomerRelationshipTypeId", errorMessage));
                log.LogMethodExit(validationErrorList);
                return validationErrorList;
            }
            if(customerRelationshipDTO.CustomerId < 0)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Customer"));
                validationErrorList.Add(new ValidationError("CustomerRelationship", "CustomerId", errorMessage));
                log.LogMethodExit(validationErrorList);
                return validationErrorList;
            }
            if (customerRelationshipDTO.RelatedCustomerId < 0)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Related Customer"));
                validationErrorList.Add(new ValidationError("CustomerRelationship", "RelatedCustomerId", errorMessage));
                log.LogMethodExit(validationErrorList);
                return validationErrorList;
            }
            if(customerRelationshipDTO.CustomerId == customerRelationshipDTO.RelatedCustomerId)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1446);
                validationErrorList.Add(new ValidationError("CustomerRelationship", "RelatedCustomerId", errorMessage));
                log.LogMethodExit(validationErrorList);
                return validationErrorList;
            }
            if(customerRelationshipDTO.CustomerId > -1 && customerRelationshipDTO.RelatedCustomerId > -1)
            {
                validationErrorList.AddRange(CheckDataAccessPermission(customerRelationshipDTO.CustomerId));
                if (validationErrorList.Count > 0)
                {
                    return validationErrorList;
                }
                validationErrorList.AddRange(CheckDataAccessPermission(customerRelationshipDTO.CustomerId));
                if(validationErrorList.Count > 0)
                {
                    return validationErrorList;
                }
            }
            CustomerRelationshipListBL customerRelationshipListBL = new CustomerRelationshipListBL(executionContext);
            List<KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>(CustomerRelationshipDTO.SearchByParameters.CUSTOMER_ID, customerRelationshipDTO.CustomerId.ToString()));
            searchParameters.Add(new KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>(CustomerRelationshipDTO.SearchByParameters.RELATED_CUSTOMER_ID, customerRelationshipDTO.RelatedCustomerId.ToString()));
            searchParameters.Add(new KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>(CustomerRelationshipDTO.SearchByParameters.IS_ACTIVE, "1"));
            List<CustomerRelationshipDTO> customerRelationshipDTOList = customerRelationshipListBL.GetCustomerRelationshipDTOList(searchParameters, false, false, sqlTransaction);
            if(customerRelationshipDTOList != null && customerRelationshipDTOList.Count > 0)
            {
                foreach (var savedCustomerRelationshipDTO in customerRelationshipDTOList)
                {
                    if(savedCustomerRelationshipDTO.Id != customerRelationshipDTO.Id)
                    {
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 1447);
                        validationErrorList.Add(new ValidationError("CustomerRelationship", "RelatedCustomerId", errorMessage));
                        log.LogMethodExit(validationErrorList);
                        return validationErrorList;
                    }
                }
            }
            if (customerRelationshipDTO.ExpiryDate.HasValue &&
                customerRelationshipDTO.EffectiveDate.HasValue &&
                customerRelationshipDTO.ExpiryDate.Value <= customerRelationshipDTO.EffectiveDate)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1445);
                validationErrorList.Add(new ValidationError("CustomerRelationship", "EffectiveDate", errorMessage));
                log.LogMethodExit(validationErrorList);
                return validationErrorList;
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }


        /// <summary>
        /// Checks Data Access Permission for Customer
        /// </summary>
        /// <param name="customerId">customerId</param>
        /// <param name="sqlTransaction">sqlTransaction </param>
        /// <returns>List of ValidationError</returns>

        private List<ValidationError> CheckDataAccessPermission(int customerId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(customerId, sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            Users user = new Users(executionContext, executionContext.GetUserId(), executionContext.GetSiteId());
            if (user.UserDTO != null)
            {
                UserRoles userRoles = new UserRoles(executionContext, user.UserDTO.RoleId);
                if (userRoles.getUserRolesDTO != null)
                {
                    DataAccessRule dataAccessRule = new DataAccessRule(executionContext,userRoles.getUserRolesDTO.DataAccessRuleId);
                    string membershipGuid = string.Empty;
                    CustomerDTO customerDTO = (new CustomerBL(executionContext, customerId)).CustomerDTO;
                    if (customerDTO.MembershipId > -1)
                    {
                        MembershipBL membershipBL = new MembershipBL(executionContext, customerDTO.MembershipId);
                        membershipGuid = membershipBL.getMembershipDTO.Guid;
                    }

                    if (dataAccessRule.IsEditable("Membership", membershipGuid) == false)
                    {
                        ValidationError validationError = new ValidationError("Customer", "", MessageContainerList.GetMessage(executionContext, 1465));
                        validationErrorList.Add(validationError);
                    }
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        private void CreateRoamingData(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            CustomerRelationshipDataHandler customerRelationshipDataHandler = new CustomerRelationshipDataHandler(sqlTransaction);
            CustomerRelationshipDTO updatedCustomerRelationshipDTO = customerRelationshipDataHandler.GetCustomerRelationshipDTO(customerRelationshipDTO.Id);
            DBSynchLogService dBSynchLogService = new DBSynchLogService(executionContext, "CustomerRelationship", updatedCustomerRelationshipDTO.Guid, updatedCustomerRelationshipDTO.SiteId);
            dBSynchLogService.CreateRoamingDataForCustomer(sqlTransaction);
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of CustomerRelationship
    /// </summary>
    public class CustomerRelationshipListBL
    {
        private static readonly Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        List<CustomerRelationshipDTO> customerRelationShipList;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="customerRelationShipList"></param>
        public CustomerRelationshipListBL(ExecutionContext executionContext, List<CustomerRelationshipDTO> customerRelationShipList)
        {
            log.LogMethodEntry(executionContext);
            this.customerRelationShipList = customerRelationShipList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public CustomerRelationshipListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the CustomerRelationship list
        /// </summary>
        public List<CustomerRelationshipDTO> GetCustomerRelationshipDTOList(List<KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>> searchParameters, 
            bool buildChildRecords = false, bool activeChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            CustomerRelationshipDataHandler customerRelationshipDataHandler = new CustomerRelationshipDataHandler(sqlTransaction);
            List<CustomerRelationshipDTO> customerRelationshipDTOList = customerRelationshipDataHandler.GetCustomerRelationshipDTOList(searchParameters);
            if (customerRelationshipDTOList != null && customerRelationshipDTOList.Any() && buildChildRecords)
            {
                Build(customerRelationshipDTOList, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(customerRelationshipDTOList);
            return customerRelationshipDTOList;
        }

        /// <summary>
        /// Builds the List of CustomerRelationship object based on the list of Customer id.
        /// </summary>
        /// <param name="customerRelationshipDTOList"></param>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        private void Build(List<CustomerRelationshipDTO> customerRelationshipDTOList , bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(customerRelationshipDTOList, activeChildRecords, sqlTransaction);
            if (customerRelationshipDTOList != null && customerRelationshipDTOList.Any())
            {
                List<CustomerDTO> customerDTOList = new List<CustomerDTO>();
                CustomerListBL customerListBL = new CustomerListBL(executionContext);

                // for CustomerID
                List<int> customerIdList = customerRelationshipDTOList.Select(relatedcustomer => relatedcustomer.CustomerId).ToList();
                customerIdList.AddRange(customerRelationshipDTOList.Select(relatedcustomer => relatedcustomer.RelatedCustomerId).ToList());
                customerDTOList = customerListBL.GetCustomerDTOList(customerIdList, true, true, true, sqlTransaction, false, null);
                foreach (CustomerRelationshipDTO customerRelationshipDTO in customerRelationshipDTOList)
                {
                    if (customerDTOList.FindIndex(x =>x.Id == customerRelationshipDTO.CustomerId)  >= 0)
                    {
                        customerRelationshipDTO.CustomerDTO = customerDTOList.Where(x => x.Id == customerRelationshipDTO.CustomerId).FirstOrDefault();
                    }
                    else
                    {
                        customerRelationshipDTO.CustomerDTO = new CustomerBL(executionContext ,customerRelationshipDTO.CustomerId).CustomerDTO;
                        customerDTOList.Add(customerRelationshipDTO.CustomerDTO);
                    }

                    if (customerDTOList.FindIndex(x => x.Id == customerRelationshipDTO.RelatedCustomerId) >= 0)
                    {
                        customerRelationshipDTO.RelatedCustomerDTO = customerDTOList.Where(x => x.Id == customerRelationshipDTO.RelatedCustomerId).FirstOrDefault();
                    }
                    else
                    {
                        customerRelationshipDTO.RelatedCustomerDTO = new CustomerBL(executionContext, customerRelationshipDTO.RelatedCustomerId).CustomerDTO;
                        customerDTOList.Add(customerRelationshipDTO.RelatedCustomerDTO);
                    }
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Saves the CustomerRelationship 
        /// </summary>
        public void SaveCustomerRelationship()
        {
            try
            {
                log.LogMethodEntry();
                if (customerRelationShipList != null && customerRelationShipList.Count > 0)
                {
                    foreach (CustomerRelationshipDTO customerRelationshipDTO in customerRelationShipList)
                    {
                        CustomerRelationshipBL customerRelationship = new CustomerRelationshipBL(executionContext, customerRelationshipDTO);
                        customerRelationship.Save();
                    }
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.LogMethodExit(ex, ex.Message);
                throw new Exception(ex.Message, ex);
            } 
        }
        /// <summary>
        /// Returns the CustomerRelationship list
        /// </summary>
        public List<CustomerRelationshipDTO> GetCustomerRelationshipDTOList(List<int> customerIdList,
            bool buildChildRecords = false, bool activeChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(customerIdList, buildChildRecords, activeChildRecords, sqlTransaction);
            CustomerRelationshipDataHandler customerRelationshipDataHandler = new CustomerRelationshipDataHandler(sqlTransaction);
            List<CustomerRelationshipDTO> customerRelationshipDTOList = customerRelationshipDataHandler.GetCustomerRelationshipDTOList(customerIdList, activeChildRecords);
            if (customerRelationshipDTOList != null && customerRelationshipDTOList.Any() && buildChildRecords)
            {
                Build(customerRelationshipDTOList, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(customerRelationshipDTOList);
            return customerRelationshipDTOList;
        }
    }
}
