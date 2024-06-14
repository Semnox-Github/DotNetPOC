/********************************************************************************************
 * Project Name - TranslatedMessageBL
 * Description  - Business Logic of the TranslatedMessage
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.60        06-May-2019   Mushahid Faizan     Created
 *2.70.2        19-Jul -2019  Girish Kundar       Modified :  Save() method and Added Logger methods and Passed SqlTransaction object for Query execution
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data.SqlClient;
namespace Semnox.Parafait.Languages
{
    public class TranslatedMessageBL
    {
        private TranslatedMessageDTO translatedMessageDTO = null;
        private ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public TranslatedMessageBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.translatedMessageDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates Translated Languages object using the translatedMessageDTO
        /// </summary>
        /// <param name="translatedMessageDTO">translatedMessageDTO object</param>
        public TranslatedMessageBL(ExecutionContext executionContext, TranslatedMessageDTO translatedMessageDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, translatedMessageDTO);
            this.executionContext = executionContext;
            this.translatedMessageDTO = translatedMessageDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the Translated Messages
        /// Checks if the  Id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            if (translatedMessageDTO.IsChanged == false
                     && translatedMessageDTO.ID > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            TranslatedMessageDataHandler translatedMessageDataHandler = new TranslatedMessageDataHandler(sqlTransaction);
            if (translatedMessageDTO.IsActive)
            {
                if (translatedMessageDTO.ID < 0)
                {
                    translatedMessageDTO = translatedMessageDataHandler.InsertTranslatedMessages(translatedMessageDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    translatedMessageDTO.AcceptChanges();
                }
                else
                {
                    if (translatedMessageDTO.IsChanged)
                    {
                        translatedMessageDTO = translatedMessageDataHandler.UpdateTranslatedMessages(translatedMessageDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                        translatedMessageDTO.AcceptChanges();
                    }
                }
            }
            else
            {
                if (translatedMessageDTO.ID >= 0)
                {
                    translatedMessageDataHandler.Delete(translatedMessageDTO.ID);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public TranslatedMessageDTO TranslatedMessageDTO
        {
            get
            {
                return translatedMessageDTO;
            }
        }
    }
    /// <summary>
    /// Manages the list of Translated Messages
    /// </summary>
    public class TranslatedMessageListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets the TranslatedMessageDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of TranslatedMessageDTO matching the search criteria</returns>
        public List<TranslatedMessageDTO> GetAllTranslatedMessagesList(List<KeyValuePair<TranslatedMessageDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            TranslatedMessageDataHandler translatedMessageDataHandler = new TranslatedMessageDataHandler(sqlTransaction);
            List<TranslatedMessageDTO> translatedMessageDTOList = translatedMessageDataHandler.GetAllTranslatedMessages(searchParameters);
            log.LogMethodExit(translatedMessageDTOList);
            return translatedMessageDTOList;
        }

        /// <summary>
        /// Gets the TranslatedMessageDTO List for message Id List
        /// </summary>
        /// <param name="messageIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of TranslatedMessageDTO</returns>
        public List<TranslatedMessageDTO> GetTranslatedMessageDTOListForMessages(List<int> messageIdList, bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(messageIdList, activeRecords, sqlTransaction);
            TranslatedMessageDataHandler translatedMessageDataHandler = new TranslatedMessageDataHandler(sqlTransaction);
            List<TranslatedMessageDTO> translatedMessageDTOList = translatedMessageDataHandler.GetTranslatedMessageDTOList(messageIdList, activeRecords);
            log.LogMethodExit(translatedMessageDTOList);
            return translatedMessageDTOList;
        }
    }
}
