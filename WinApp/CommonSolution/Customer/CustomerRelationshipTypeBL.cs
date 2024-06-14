/********************************************************************************************
 * Project Name - CustomerRelationshipType BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        06-Feb-2017      Lakshminarayana     Created 
 *2.70.2        19-Jul-2019      Girish Kundar     Modified : Save() method. Now Insert/Update method returns the DTO instead of Id.
 *2.90        21-May-2020      Girish Kundar       Modified : Made default constructor as Private 
 *2.130.0     31-Aug-2021   Mushahid Faizan   Modified : Pos UI redesign changes.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// Business logic for CustomerRelationshipType class.
    /// </summary>
    public class CustomerRelationshipTypeBL
    {
        private CustomerRelationshipTypeDTO customerRelationshipTypeDTO;
        private readonly ExecutionContext executionContext;
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of CustomerRelationshipTypeBL class
        /// </summary>
        private CustomerRelationshipTypeBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the customerRelationshipType id as the parameter
        /// Would fetch the customerRelationshipType object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public CustomerRelationshipTypeBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            CustomerRelationshipTypeDataHandler customerRelationshipTypeDataHandler = new CustomerRelationshipTypeDataHandler(sqlTransaction);
            customerRelationshipTypeDTO = customerRelationshipTypeDataHandler.GetCustomerRelationshipTypeDTO(id);
            if (customerRelationshipTypeDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "CustomerRelationshipType", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates CustomerRelationshipTypeBL object using the CustomerRelationshipTypeDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="customerRelationshipTypeDTO">CustomerRelationshipTypeDTO object</param>
        public CustomerRelationshipTypeBL(ExecutionContext executionContext, CustomerRelationshipTypeDTO customerRelationshipTypeDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, customerRelationshipTypeDTO);
            this.customerRelationshipTypeDTO = customerRelationshipTypeDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the CustomerRelationshipType
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            CustomerRelationshipTypeDataHandler customerRelationshipTypeDataHandler = new CustomerRelationshipTypeDataHandler(sqlTransaction);
            if (customerRelationshipTypeDTO.Id < 0)
            {
                customerRelationshipTypeDTO = customerRelationshipTypeDataHandler.InsertCustomerRelationshipType(customerRelationshipTypeDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                customerRelationshipTypeDTO.AcceptChanges();
            }
            else
            {
                if (customerRelationshipTypeDTO.IsChanged)
                {
                    customerRelationshipTypeDTO = customerRelationshipTypeDataHandler.UpdateCustomerRelationshipType(customerRelationshipTypeDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    customerRelationshipTypeDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public CustomerRelationshipTypeDTO CustomerRelationshipTypeDTO
        {
            get
            {
                return customerRelationshipTypeDTO;
            }
        }

    }

    /// <summary>
    /// Manages the list of CustomerRelationshipType
    /// </summary>
    public class CustomerRelationshipTypeListBL
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        public CustomerRelationshipTypeListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public CustomerRelationshipTypeListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the CustomerRelationshipType list
        /// </summary>
        public List<CustomerRelationshipTypeDTO> GetCustomerRelationshipTypeDTOList(List<KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            CustomerRelationshipTypeDataHandler customerRelationshipTypeDataHandler = new CustomerRelationshipTypeDataHandler(sqlTransaction);
            List<CustomerRelationshipTypeDTO> returnValue = customerRelationshipTypeDataHandler.GetCustomerRelationshipTypeDTOList(searchParameters);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        public DateTime? GetCustomerRelationshipTypeLastUpdateTime(int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(siteId, sqlTransaction);
            CustomerRelationshipTypeDataHandler customerRelationshipTypeDataHandler = new CustomerRelationshipTypeDataHandler(sqlTransaction);
            DateTime? result = customerRelationshipTypeDataHandler.GetCustomerRelationshipTypeLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
