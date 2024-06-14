/********************************************************************************************
 * Project Name - Card Credit Plus Pause Time Log BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        19-Jul-2019      Girish Kundar      Modified : Save() method. Now Insert/Update method returns the DTO instead of Id. 
 *2.80.0        21-May-2020      Girish Kundar      Modified : Made default constructor as Private
 *2.90.0        14-Aug-2020      Girish Kundar      Modified : Removed constructor with mant parameters and passed sql transaction to datahandler
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// Business logic class to managing CardCreditPlusPauseTimeLog data
    /// </summary>
    public class CardCreditPlusPauseTimeLogBL
    {
        private CardCreditPlusPauseTimeLogDTO cardCreditPlusPauseTimeLogDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
       
        /// <summary>
        /// Default constructor of CardCreditPlusPauseTimeLogBL class
        /// </summary>
        private CardCreditPlusPauseTimeLogBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates CardCreditPlusPauseTimeLogBL object using the CardCreditPlusPauseTimeLogDTO
        /// </summary>
        /// <param name="cardCreditPlusPauseTimeLogDTO">CardCreditPlusPauseTimeLogDTO object</param>
        public CardCreditPlusPauseTimeLogBL(ExecutionContext executionContext, CardCreditPlusPauseTimeLogDTO cardCreditPlusPauseTimeLogDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext,cardCreditPlusPauseTimeLogDTO);
            this.cardCreditPlusPauseTimeLogDTO = cardCreditPlusPauseTimeLogDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the CardCreditPlusPauseTimeLog details to database
        /// Checks if the CardCPPauseTimeLogId is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts the record to database
        /// else updates the existing details
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            CardCreditPlusPauseTimeLogDataHandler cardCreditPlusPauseTimeLogDataHandler = new CardCreditPlusPauseTimeLogDataHandler(sqlTransaction);
            if (cardCreditPlusPauseTimeLogDTO.CardCPPauseTimeLogId < 0)
            {
                cardCreditPlusPauseTimeLogDTO = cardCreditPlusPauseTimeLogDataHandler.InsertCardCreditPlusPauseTimeLog(cardCreditPlusPauseTimeLogDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                cardCreditPlusPauseTimeLogDTO.AcceptChanges();
            }
            else
            {
                if (cardCreditPlusPauseTimeLogDTO.IsChanged)
                {
                    cardCreditPlusPauseTimeLogDTO = cardCreditPlusPauseTimeLogDataHandler.UpdateCardCreditPlusPauseTimeLog(cardCreditPlusPauseTimeLogDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    cardCreditPlusPauseTimeLogDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of CardCPPauseTimeLog
    /// </summary>
    public class CardCPPauseTimeLogList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<CardCreditPlusPauseTimeLogDTO> cardCreditPlusPauseTimeLogList;

        /// <summary>
        /// Returns the CardCPPauseTimeLog list
        /// </summary>
        public List<CardCreditPlusPauseTimeLogDTO> GetAllCardCPPauseTimeLog(List<KeyValuePair<CardCreditPlusPauseTimeLogDTO.SearchByParameters, string>> searchParameters ,SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters ,sqlTransaction);
            CardCreditPlusPauseTimeLogDataHandler cardCreditPlusPauseTimeLogDataHandler = new CardCreditPlusPauseTimeLogDataHandler(sqlTransaction);
            cardCreditPlusPauseTimeLogList = cardCreditPlusPauseTimeLogDataHandler.GetCardCPPauseTimeLogList(searchParameters);
            log.LogMethodExit(cardCreditPlusPauseTimeLogList);
            return cardCreditPlusPauseTimeLogList;
        }

        /// <summary>
        /// Returns the CardCPPauseTimeLog list
        /// </summary>
        public List<CardCreditPlusPauseTimeLogDTO> GetAllCardCPPauseTimeLogByCardId(int cardId , SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(cardId, sqlTransaction);
            CardCreditPlusPauseTimeLogDataHandler cardCreditPlusPauseTimeLogDataHandler = new CardCreditPlusPauseTimeLogDataHandler(sqlTransaction);
            cardCreditPlusPauseTimeLogList = cardCreditPlusPauseTimeLogDataHandler.GetCardCPPauseTimeLogListByCardId(cardId);
            log.LogMethodExit(cardCreditPlusPauseTimeLogList);
            return cardCreditPlusPauseTimeLogList;
        }
    }
}
