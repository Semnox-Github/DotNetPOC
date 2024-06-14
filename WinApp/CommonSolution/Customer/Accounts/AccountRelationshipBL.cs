/********************************************************************************************
 * Project Name - AccountRelationshipBL
 * Description  - BL AccountRelationship
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.80.0      21-May-2020      Girish Kundar  Modified : Made default constructor as Private   
 *2.140.0     23-June-2021      Prashanth V    Modified : Added Parameterized constructor in AccountRelationListBL and accountRelationshipDTOList field
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// Business logic for AccountRelationship class.
    /// </summary>
    public class AccountRelationshipBL
    {
        private AccountRelationshipDTO accountRelationshipDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of AccountRelationshipBL class
        /// </summary>
        private AccountRelationshipBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the accountRelationship id as the parameter
        /// Would fetch the accountRelationship object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public AccountRelationshipBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            AccountRelationshipDataHandler accountRelationshipDataHandler = new AccountRelationshipDataHandler(sqlTransaction);
            accountRelationshipDTO = accountRelationshipDataHandler.GetAccountRelationshipDTO(id);
            if (accountRelationshipDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "AccountRelationship", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates AccountRelationshipBL object using the AccountRelationshipDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="accountRelationshipDTO">AccountRelationshipDTO object</param>
        public AccountRelationshipBL(ExecutionContext executionContext, AccountRelationshipDTO accountRelationshipDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, accountRelationshipDTO);
            this.accountRelationshipDTO = accountRelationshipDTO;
            log.LogMethodExit();
        }

        
        /// <summary>
        /// Saves the AccountRelationship
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        internal void Save(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            AccountRelationshipDataHandler accountRelationshipDataHandler = new AccountRelationshipDataHandler(sqlTransaction);
            List<ValidationError> validationError = Validate(sqlTransaction);
            if(validationError != null && validationError.Count >0)
            {
                throw new ValidationException("Validation Failed", validationError);
            }

            if (accountRelationshipDTO.AccountRelationshipId < 0)
            {
                accountRelationshipDTO = accountRelationshipDataHandler.InsertAccountRelationship(accountRelationshipDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                accountRelationshipDTO.AcceptChanges();
            }
            else
            {
                if (accountRelationshipDTO.IsChanged)
                {
                    accountRelationshipDTO = accountRelationshipDataHandler.UpdateAccountRelationship(accountRelationshipDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    accountRelationshipDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public AccountRelationshipDTO AccountRelationshipDTO
        {
            get
            {
                return accountRelationshipDTO;
            }
        }

        /// <summary>
        /// Validates the customer relationship DTO
        /// </summary>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
    }

    /// <summary>
    /// Manages the list of AccountRelationship
    /// </summary>
    public class AccountRelationshipListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<AccountRelationshipDTO> accountRelationshipDTOList;
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public AccountRelationshipListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.accountRelationshipDTOList = null; 
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="accountRelationshipDTOs">accountRelationshipDTOs</param>
        public AccountRelationshipListBL(List<AccountRelationshipDTO> accountRelationshipDTOs, ExecutionContext executionContext) : this(executionContext)
        {
            log.LogMethodEntry();
            this.accountRelationshipDTOList = accountRelationshipDTOs;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the AccountRelationship list
        /// </summary>
        public List<AccountRelationshipDTO> GetAccountRelationshipDTOList(List<KeyValuePair<AccountRelationshipDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            AccountRelationshipDataHandler accountRelationshipDataHandler = new AccountRelationshipDataHandler(sqlTransaction);
            List<AccountRelationshipDTO> returnValue = accountRelationshipDataHandler.GetAccountRelationshipDTOList(searchParameters);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        public void Save(SqlTransaction sqlTransaction)
        {
            try
            {
                if (accountRelationshipDTOList != null)
                {
                    foreach (AccountRelationshipDTO accountRelationshipDTO in accountRelationshipDTOList)
                    {
                        AccountRelationshipBL accountRelationshipBL = new AccountRelationshipBL(executionContext, accountRelationshipDTO);
                        accountRelationshipBL.Save(sqlTransaction);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }

        }
        /// <summary>
        /// Returns the AccountRelationship list
        /// </summary>
        public List<AccountRelationshipDTO> GetAccountRelationshipDTOListByAccountIds(List<int> accountIdList, bool activeChildRecords, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(accountIdList, activeChildRecords);
            AccountRelationshipDataHandler accountRelationshipDataHandler = new AccountRelationshipDataHandler(sqlTransaction);
            List<AccountRelationshipDTO> returnValue = accountRelationshipDataHandler.GetAccountRelationshipDTOListByAccountIdList(accountIdList, activeChildRecords);
            log.LogMethodExit(returnValue);
            return returnValue;
        }
        /// <summary>
        /// Returns the AccountRelationship list
        /// </summary>
        public List<AccountRelationshipDTO> GetAccountRelationshipDTOListByRelatedAccountIds(List<int> relatedAccountIdList, bool activeChildRecords, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(relatedAccountIdList, activeChildRecords);
            AccountRelationshipDataHandler accountRelationshipDataHandler = new AccountRelationshipDataHandler(sqlTransaction);
            List<AccountRelationshipDTO> returnValue = accountRelationshipDataHandler.GetAccountRelationshipDTOListByRelatedAccountIdList(relatedAccountIdList, activeChildRecords);
            log.LogMethodExit(returnValue);
            return returnValue;
        }
    }
}
