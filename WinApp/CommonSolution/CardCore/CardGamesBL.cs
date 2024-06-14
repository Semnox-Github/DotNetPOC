
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.CardCore
{
    /// <summary>
    /// Business logic for CardGames class.
    /// </summary>
    public class CardGamesBL
    {
        CardGamesDTO cardGamesDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of CardGamesBL class
        /// </summary>
        public CardGamesBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            cardGamesDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the card games  id as the parameter
        /// Would fetch the CardGames object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="cardGamesId">cardGamesId</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public CardGamesBL(ExecutionContext executionContext, int cardGamesId, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, cardGamesId, sqlTransaction);
            CardGamesDataHandler cardGamesDataHandler = new CardGamesDataHandler(sqlTransaction);
            cardGamesDTO = cardGamesDataHandler.GetCardGamesDTO(cardGamesId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates CardGamesBL object using the CardGamesDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="cardGamesDTO">CardGamesDTO object</param>
        public CardGamesBL(ExecutionContext executionContext, CardGamesDTO cardGamesDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, cardGamesDTO);
            this.cardGamesDTO = cardGamesDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the CardGames
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            CardGamesDataHandler cardGamesDataHandler = new CardGamesDataHandler(sqlTransaction);
            if (cardGamesDTO.CardGameId < 0)
            {
                int id = cardGamesDataHandler.InsertCardGames(cardGamesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                cardGamesDTO.CardGameId = id;
                cardGamesDTO.AcceptChanges();
            }
            else
            {
                if (cardGamesDTO.IsChanged)
                {
                    cardGamesDataHandler.UpdateCardGames(cardGamesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    cardGamesDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public CardGamesDTO CardGamesDTO
        {
            get
            {
                return cardGamesDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of CardGames
    /// </summary>
    public class CardGamesListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public CardGamesListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the CardGames list
        /// </summary>
        public List<CardGamesDTO> GetCardGamesDTOList(List<KeyValuePair<CardGamesDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            CardGamesDataHandler cardGamesDataHandler = new CardGamesDataHandler(sqlTransaction);
            List<CardGamesDTO> returnValue = cardGamesDataHandler.GetCardGamesDTOList(searchParameters);
            log.LogMethodExit(returnValue);
            return returnValue;
        }
    }
}
