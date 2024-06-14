/********************************************************************************************
 * Project Name -  Locker Blocked Cards
 * Description  - Bussiness logic of Locker Blocked Cards
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        05-Aug-2017   Raghuveera          Created 
 *2.70.2        19-Jul-2019   Dakshakh raj        Modified : Save() method Insert/Update method returns DTO.
 * 
 ********************************************************************************************/
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.Lockers
{
    /// <summary>
    /// Bussiness logic for locker blocked card
    /// </summary>
    public class LockerBlockedCards
    {
        private LockerBlockedCardsDTO lockerBlockedCardDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor of LockerBlockedCards class
        /// </summary>
        public LockerBlockedCards()
        {
            log.LogMethodEntry();
            lockerBlockedCardDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        ///Constructor will fetch the LockerBlockedCards DTO based on the lockerBlockedCards id passed 
        /// </summary>
        /// <param name="cardBlockId">LockerBlockedCards id</param>
        /// <param name="sqltransaction">sqltransaction</param>
        public LockerBlockedCards(int cardBlockId, SqlTransaction sqltransaction = null)
            : this()
        {
            log.LogMethodEntry(cardBlockId, sqltransaction);
            LockerBlockedCardsDataHandler lockerBlockedCardsDataHandler = new LockerBlockedCardsDataHandler(sqltransaction);
            lockerBlockedCardDTO = lockerBlockedCardsDataHandler.GetLockerBlockedCards(cardBlockId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates lockerBlockedCards object using the LockerBlockedCardsDTO
        /// </summary>
        /// <param name="lockerBlockedCards">LockerBlockedCardsDTO object</param>
        public LockerBlockedCards(LockerBlockedCardsDTO lockerBlockedCards)
            : this()
        {
            log.LogMethodEntry(lockerBlockedCards);
            this.lockerBlockedCardDTO = lockerBlockedCards;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates lockerBlockedCards object using the LockerBlockedCardsDTO
        /// </summary>
        /// <param name="cardNumber">blocked card number </param>
        /// <param name="sqltransaction">sqltransaction</param>
        public LockerBlockedCards(string cardNumber, SqlTransaction sqltransaction = null)
            : this()
        {
            log.LogMethodEntry(cardNumber, sqltransaction);
            LockerBlockedCardsDataHandler lockerBlockedCardsDataHandler = new LockerBlockedCardsDataHandler(sqltransaction);
            this.lockerBlockedCardDTO =lockerBlockedCardsDataHandler.GetLockerBlockedCard(cardNumber);
            log.LogMethodExit();
        }

        /// <summary>
        /// Get the card blocked DTO of the passed card number if exists
        /// </summary>
        /// <param name="cardNumber">string type parameter</param>
        /// <param name="sqltransaction">sqltransaction</param>
        /// <returns>Returns LockerBlockedCardsDTO</returns>
        public LockerBlockedCardsDTO GetBlockedCard(string cardNumber, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(cardNumber, sqlTransaction);
            LockerBlockedCardsDataHandler lockerBlockedCardsDataHandler = new LockerBlockedCardsDataHandler(sqlTransaction);
            LockerBlockedCardsDTO lockerBlockedCardsDTO = lockerBlockedCardsDataHandler.GetLockerBlockedCard(cardNumber);
            log.LogMethodExit(lockerBlockedCardsDTO);
            return lockerBlockedCardsDTO;
        }

        /// <summary>
        /// Removes of the passed card number if exists.
        /// </summary>
        /// <param name="cardNumber">cardNumber</param>
        /// <param name="sqltransaction">sqltransaction</param>
        public void RemoveBlockedCard(string cardNumber, SqlTransaction sqltransaction = null)
        {
            log.LogMethodEntry(cardNumber, sqltransaction);
            LockerBlockedCardsDataHandler lockerBlockedCardsDataHandler = new LockerBlockedCardsDataHandler(sqltransaction);
            lockerBlockedCardsDataHandler.RemoveBlockedCards(cardNumber);
            log.LogMethodExit();            
        }

        /// <summary>
        /// Saves the locker blocked cards
        /// Checks if the card block id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqltransaction">sqltransaction</param>
        public void Save(SqlTransaction sqltransaction = null)
        {
            log.LogMethodEntry(sqltransaction);
            ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
            LockerBlockedCardsDataHandler lockerBlockedCardsDataHandler = new LockerBlockedCardsDataHandler(sqltransaction);
            if (lockerBlockedCardDTO.CardBlockId < 0)
            {
                lockerBlockedCardDTO = lockerBlockedCardsDataHandler.InsertLockerBlockedCards(lockerBlockedCardDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                lockerBlockedCardDTO.AcceptChanges();
            }
            else
            {
                if (lockerBlockedCardDTO.IsChanged)
                {
                    lockerBlockedCardDTO = lockerBlockedCardsDataHandler.UpdateLockerBlockedCards(lockerBlockedCardDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                    lockerBlockedCardDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
        

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public LockerBlockedCardsDTO lockerBlockedCardsDTO { get { return lockerBlockedCardDTO; } }
    }

    /// <summary>
    /// Manages the list of lockerBlockedCardss
    /// </summary>
    public class LockerBlockedCardsList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Returns the lockerBlockedCards list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<LockerBlockedCardsDTO> GetAllLockerBlockedCards(List<KeyValuePair<LockerBlockedCardsDTO.SearchByLockerBlockedCardsParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            LockerBlockedCardsDataHandler lockerBlockedCardsDataHandler = new LockerBlockedCardsDataHandler(sqlTransaction);
            List<LockerBlockedCardsDTO> lockerBlockedCardsDTOList = lockerBlockedCardsDataHandler.GetLockerBlockedCardsList(searchParameters);
            log.LogMethodExit(lockerBlockedCardsDTOList);
            return lockerBlockedCardsDTOList;
        }
    }
}
