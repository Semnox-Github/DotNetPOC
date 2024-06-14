/********************************************************************************************
 * Project Name - User
 * Description  - BL for UserPasswordHistory
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.80        4-June-2020   Girish Kundar           Created 
 *2.110.0     27-Nov-2020   Lakshminarayana         Modified : Changed as part of POS UI redesign. Implemented the new design principles 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Parafait.User
{
    internal class UserPasswordHistoryBL
    {
        private UserPasswordHistoryDTO userPasswordHistoryDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        private UserPasswordHistoryBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="UserPasswordHistoryDTO"></param>
        internal UserPasswordHistoryBL(ExecutionContext executionContext, UserPasswordHistoryDTO userPasswordHistoryDTO)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext, userPasswordHistoryDTO);
            this.userPasswordHistoryDTO = userPasswordHistoryDTO;
            log.LogMethodExit();
        }

        internal UserPasswordHistoryBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, userPasswordHistoryDTO, sqlTransaction);
            UserPasswordHistoryDataHandler userPasswordHistoryDataHandler = new UserPasswordHistoryDataHandler(sqlTransaction);
            userPasswordHistoryDTO = userPasswordHistoryDataHandler.GetUserPasswordHistoryDTO(id);
            if (userPasswordHistoryDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "userPasswordHistory", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            UserPasswordHistoryDataHandler userPasswordHistoryDataHandler = new UserPasswordHistoryDataHandler(sqlTransaction);
            if (userPasswordHistoryDTO.UserPasswordHistoryId <= 0)
            {
                userPasswordHistoryDTO = userPasswordHistoryDataHandler.Insert(userPasswordHistoryDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                userPasswordHistoryDTO.AcceptChanges();

            }
            else
            {
                if (userPasswordHistoryDTO.IsChanged)
                {
                    userPasswordHistoryDTO = userPasswordHistoryDataHandler.Update(userPasswordHistoryDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    userPasswordHistoryDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        internal UserPasswordHistoryDTO UserPasswordHistoryDTO
        {
            get { return userPasswordHistoryDTO; }
        }

        internal bool IsMatch(string password, string loginId)
        {
            log.LogMethodEntry("password", loginId);
            UserEncryptionKey userEncryptionKey = new UserEncryptionKey(executionContext, loginId.ToLower());
            UserPasswordHash newPasswordHash = new UserPasswordHash(password, userPasswordHistoryDTO.PasswordSalt, userEncryptionKey);
            UserPasswordHash historyPasswordHash = new UserPasswordHash(userPasswordHistoryDTO.PasswordHash);
            bool result = historyPasswordHash == newPasswordHash;
            log.LogMethodExit(result);
            return result;
        }
    }

    public class UserPasswordHistoryListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private readonly List<UserPasswordHistoryDTO> userPasswordHistoryDTOList;
        /// <summary>
        /// Default constructor
        /// </summary>
        public UserPasswordHistoryListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        internal UserPasswordHistoryListBL(ExecutionContext executionContext, List<UserPasswordHistoryDTO> userPasswordHistoryDTOList)
        {
            log.LogMethodEntry(executionContext, userPasswordHistoryDTOList);
            this.executionContext = executionContext;
            this.userPasswordHistoryDTOList = userPasswordHistoryDTOList;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns Search Request And returns List Of UserPasswordHistoryDTO Class  
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>List of UserPasswordHistoryDTO</returns>
        internal List<UserPasswordHistoryDTO> GetUserPasswordHistoryDTOList(List<KeyValuePair<UserPasswordHistoryDTO.SearchByUserPasswordHistoryParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            UserPasswordHistoryDataHandler userPasswordHistoryDataHandler = new UserPasswordHistoryDataHandler(sqlTransaction);
            List<UserPasswordHistoryDTO> userPasswordHistoryDTOList = userPasswordHistoryDataHandler.GetUserPasswordHistoryDTOList(searchParameters);
            log.LogMethodExit(userPasswordHistoryDTOList);
            return userPasswordHistoryDTOList;
        }

        /// <summary>
        /// This method should be used to Save UserIdentificationTags
        /// </summary>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {                
                if (userPasswordHistoryDTOList != null && userPasswordHistoryDTOList.Count != 0)
                {
                    foreach (UserPasswordHistoryDTO userPasswordHistoryDTO in userPasswordHistoryDTOList)
                    {                        
                        UserPasswordHistoryBL userPasswordHistoryBL = new UserPasswordHistoryBL(executionContext, userPasswordHistoryDTO);
                        userPasswordHistoryBL.Save(sqlTransaction);
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
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }
    }
}
