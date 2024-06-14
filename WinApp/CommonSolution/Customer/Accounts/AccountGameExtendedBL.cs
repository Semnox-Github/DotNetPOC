/********************************************************************************************
 * Project Name - AccountGameExtendedBL
 * Description  - BL AccountGameExtended
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.80.0      21-May-2020      Girish Kundar  Modified : Made default constructor as Private   
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.DBSynch;
using Semnox.Parafait.Game;
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
    /// Business logic for AccountGameExtended class.
    /// </summary>
    public class AccountGameExtendedBL
    {
        private AccountGameExtendedDTO accountGameExtendedDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of AccountGameExtendedBL class
        /// </summary>
        private AccountGameExtendedBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the accountGameExtended id as the parameter
        /// Would fetch the accountGameExtended object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public AccountGameExtendedBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            AccountGameExtendedDataHandler accountGameExtendedDataHandler = new AccountGameExtendedDataHandler(sqlTransaction);
            accountGameExtendedDTO = accountGameExtendedDataHandler.GetAccountGameExtendedDTO(id);
            if (accountGameExtendedDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "AccountGameExtended", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates AccountGameExtendedBL object using the AccountGameExtendedDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="accountGameExtendedDTO">AccountGameExtendedDTO object</param>
        public AccountGameExtendedBL(ExecutionContext executionContext, AccountGameExtendedDTO accountGameExtendedDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, accountGameExtendedDTO);
            this.accountGameExtendedDTO = accountGameExtendedDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the AccountGameExtended
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        internal void Save(int parentSiteId, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(parentSiteId, sqlTransaction);
            AccountGameExtendedDataHandler accountGameExtendedDataHandler = new AccountGameExtendedDataHandler(sqlTransaction);
            if (accountGameExtendedDTO.IsChanged)
            {
                if (accountGameExtendedDTO.IsActive)
                {
                    if (accountGameExtendedDTO.AccountGameExtendedId < 0)
                    {
                        accountGameExtendedDTO = accountGameExtendedDataHandler.InsertAccountGameExtended(accountGameExtendedDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                        accountGameExtendedDTO.AcceptChanges();
                    }
                    else
                    {
                        if (accountGameExtendedDTO.IsChanged)
                        {
                            accountGameExtendedDTO = accountGameExtendedDataHandler.UpdateAccountGameExtended(accountGameExtendedDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                            accountGameExtendedDTO.AcceptChanges();
                        }
                    }
                    CreateRoamingData(parentSiteId, sqlTransaction);
                }
                else
                {
                    if (accountGameExtendedDTO.AccountGameExtendedId >= 0)
                    {
                        accountGameExtendedDataHandler.DeleteAccountGameExtended(accountGameExtendedDTO.AccountGameExtendedId);
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public AccountGameExtendedDTO AccountGameExtendedDTO
        {
            get
            {
                return accountGameExtendedDTO;
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
        private void CreateRoamingData(int parentSiteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parentSiteId, sqlTransaction);
            if (parentSiteId > -1 && parentSiteId != accountGameExtendedDTO.SiteId && executionContext.GetSiteId() > -1
                    && accountGameExtendedDTO.AccountGameExtendedId > -1)
            {
                DBSynchLogDTO dBSynchLogDTO = new DBSynchLogDTO("I", accountGameExtendedDTO.Guid, "CardGameExtended", DateTime.Now, parentSiteId);
                DBSynchLogBL dBSynchLogBL = new DBSynchLogBL(executionContext, dBSynchLogDTO);
                dBSynchLogBL.Save(sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to get AccountgameExtndedSummaryBL
        /// </summary>
        /// <returns></returns>
        internal AccountGameExtendedSummaryBL GetAccountGameExtendedSummaryBL()
        {
            log.LogMethodEntry();
            string gameProfileName = string.Empty;
            string gameName = string.Empty;
            if (accountGameExtendedDTO.GameProfileId > -1)
            {
                GameProfileContainerDTO gameProfileContainerDTO = GameProfileContainerList.GetGameProfileContainerDTOOrDefault(executionContext, accountGameExtendedDTO.GameProfileId);
                if (gameProfileContainerDTO != null)
                {
                    gameProfileName = gameProfileContainerDTO.ProfileName;
                }
            }

            if (accountGameExtendedDTO.GameId > -1)
            {
                GameContainerDTO gameContainerDTO = GameContainerList.GetGameContainerDTOOrDefault(executionContext, accountGameExtendedDTO.GameId);
                if (gameContainerDTO != null)
                {
                    gameName = gameContainerDTO.GameName;
                }

            }
            AccountGameExtendedSummaryDTO accountGameExtendedSummaryDTO = new AccountGameExtendedSummaryDTO(accountGameExtendedDTO.AccountGameExtendedId,
                                                                                                            accountGameExtendedDTO.AccountGameId, accountGameExtendedDTO.GameId,
                                                                                                            gameName, accountGameExtendedDTO.GameProfileId, gameProfileName,
                                                                                                            accountGameExtendedDTO.Exclude, accountGameExtendedDTO.PlayLimitPerGame);
            AccountGameExtendedSummaryBL accountGameExtendedSummaryBL = new AccountGameExtendedSummaryBL(executionContext, accountGameExtendedSummaryDTO);
            log.LogMethodExit(accountGameExtendedSummaryBL);
            return accountGameExtendedSummaryBL;
        }
    }

    /// <summary>
    /// Manages the list of AccountGameExtended
    /// </summary>
    public class AccountGameExtendedListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public AccountGameExtendedListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the AccountGameExtended list
        /// </summary>
        public List<AccountGameExtendedDTO> GetAccountGameExtendedDTOList(List<KeyValuePair<AccountGameExtendedDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            AccountGameExtendedDataHandler accountGameExtendedDataHandler = new AccountGameExtendedDataHandler(sqlTransaction);
            List<AccountGameExtendedDTO> returnValue = accountGameExtendedDataHandler.GetAccountGameExtendedDTOList(searchParameters);
            log.LogMethodExit(returnValue);
            return returnValue;
        }
    }
}
