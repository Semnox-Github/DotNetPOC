/********************************************************************************************
 * Project Name - CardDiscounts BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        15-Jul-2017      Lakshminarayana     Created 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.CardCore;
using Semnox.Parafait.Discounts;

namespace Semnox.Parafait.CardCore
{
    /// <summary>
    /// Business logic for CardDiscounts class.
    /// </summary>
    public class CardDiscountsBL
    {
        CardDiscountsDTO cardDiscountsDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor of CardDiscountsBL class
        /// </summary>
        public CardDiscountsBL()
        {
            log.Debug("Starts-CardDiscountsBL() default constructor.");
            cardDiscountsDTO = null;
            log.Debug("Ends-CardDiscountsBL() default constructor.");
        }

        /// <summary>
        /// Constructor with the cardDiscounts id as the parameter
        /// Would fetch the cardDiscounts object from the database based on the id passed. 
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">SQL Transaction</param>
        public CardDiscountsBL(int id, SqlTransaction sqlTransaction = null)
            : this()
        {
            log.Debug("Starts-CardDiscountsBL(id) parameterized constructor.");
            CardDiscountsDataHandler cardDiscountsDataHandler = new CardDiscountsDataHandler(sqlTransaction);
            cardDiscountsDTO = cardDiscountsDataHandler.GetCardDiscountsDTO(id);
            log.Debug("Ends-CardDiscountsBL(id) parameterized constructor.");
        }

        /// <summary>
        /// Creates CardDiscountsBL object using the CardDiscountsDTO
        /// </summary>
        /// <param name="cardDiscountsDTO">CardDiscountsDTO object</param>
        public CardDiscountsBL(CardDiscountsDTO cardDiscountsDTO)
            : this()
        {
            log.Debug("Starts-CardDiscountsBL(cardDiscountsDTO) Parameterized constructor.");
            this.cardDiscountsDTO = cardDiscountsDTO;
            log.Debug("Ends-CardDiscountsBL(cardDiscountsDTO) Parameterized constructor.");
        }

        /// <summary>
        /// Saves the CardDiscounts
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.Debug("Starts-Save() method.");
            ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
            CardDiscountsDataHandler cardDiscountsDataHandler = new CardDiscountsDataHandler(sqlTransaction);
            if (cardDiscountsDTO.IsActive == "Y")
            {
                CardDiscountsListBL cardDiscountsListBL = new CardDiscountsListBL();
                List<KeyValuePair<CardDiscountsDTO.SearchByParameters, string>> searchCardDiscountsParams = new List<KeyValuePair<CardDiscountsDTO.SearchByParameters, string>>();
                searchCardDiscountsParams.Add(new KeyValuePair<CardDiscountsDTO.SearchByParameters, string>(CardDiscountsDTO.SearchByParameters.DISCOUNT_ID, cardDiscountsDTO.DiscountId.ToString()));
                searchCardDiscountsParams.Add(new KeyValuePair<CardDiscountsDTO.SearchByParameters, string>(CardDiscountsDTO.SearchByParameters.CARD_ID, cardDiscountsDTO.CardId.ToString()));
                searchCardDiscountsParams.Add(new KeyValuePair<CardDiscountsDTO.SearchByParameters, string>(CardDiscountsDTO.SearchByParameters.IS_ACTIVE, "Y"));
                searchCardDiscountsParams.Add(new KeyValuePair<CardDiscountsDTO.SearchByParameters, string>(CardDiscountsDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                searchCardDiscountsParams.Add(new KeyValuePair<CardDiscountsDTO.SearchByParameters, string>(CardDiscountsDTO.SearchByParameters.EXPIRY_DATE_GREATER_THAN, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")));
                List<CardDiscountsDTO> cardDiscountsDTOList = cardDiscountsListBL.GetCardDiscountsDTOList(searchCardDiscountsParams, sqlTransaction);
                if(cardDiscountsDTOList != null && cardDiscountsDTOList.Count > 0)
                {
                    if (cardDiscountsDTOList[0].CardDiscountId != cardDiscountsDTO.CardDiscountId)
                    {
                        throw new DuplicateCardDiscountException();
                    }
                }
                DiscountsBL discountsBL = new DiscountsBL(machineUserContext, new ExternallyManagedUnitOfWork(sqlTransaction), cardDiscountsDTO.DiscountId, false);
                if (!discountsBL.DiscountsDTO.IsActive)
                {
                    throw new InvalidDiscountException("Only active discounts can be used.");
                }
            }
            if(cardDiscountsDTO.CardDiscountId < 0)
            {
                int id = cardDiscountsDataHandler.InsertCardDiscounts(cardDiscountsDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                cardDiscountsDTO.CardDiscountId = id;
                cardDiscountsDTO.AcceptChanges();
            }
            else
            {
                if(cardDiscountsDTO.IsChanged)
                {
                    cardDiscountsDataHandler.UpdateCardDiscounts(cardDiscountsDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                    cardDiscountsDTO.AcceptChanges();
                }
            }
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public CardDiscountsDTO CardDiscountsDTO
        {
            get
            {
                return cardDiscountsDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of CardDiscounts
    /// </summary>
    public class CardDiscountsListBL
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Returns the CardDiscounts list
        /// </summary>
        public List<CardDiscountsDTO> GetCardDiscountsDTOList(List<KeyValuePair<CardDiscountsDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            CardDiscountsDataHandler cardDiscountsDataHandler = new CardDiscountsDataHandler(sqlTransaction);
            log.Debug("Ends-GetCardDiscountsDTOList(searchParameters) method by returning the result of cardDiscountsDataHandler.GetCardDiscountsDTOList(searchParameters) call");
            return cardDiscountsDataHandler.GetCardDiscountsDTOList(searchParameters);
        }

    }

    /// <summary>
    /// Represents duplicate card discount error that occur during application execution. 
    /// </summary>
    public class DuplicateCardDiscountException : Exception
    {
        /// <summary>
        /// Default constructor of DuplicateCardDiscountException.
        /// </summary>
        public DuplicateCardDiscountException()
        {
        }

        /// <summary>
        /// Initializes a new instance of DuplicateCardDiscountException class with a specified error message.
        /// </summary>
        /// <param name="message">message</param>
        public DuplicateCardDiscountException(string message)
        : base(message)
        {
        }
        /// <summary>
        /// Initializes a new instance of DuplicateCardDiscountException class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="inner">inner exception</param>
        public DuplicateCardDiscountException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }

    /// <summary>
    /// Represents invalid discount error that occur during application execution. 
    /// </summary>
    public class InvalidDiscountException : Exception
    {
        /// <summary>
        /// Default constructor of InvalidDiscountException.
        /// </summary>
        public InvalidDiscountException()
        {
        }

        /// <summary>
        /// Initializes a new instance of InvalidDiscountException class with a specified error message.
        /// </summary>
        /// <param name="message">message</param>
        public InvalidDiscountException(string message)
        : base(message)
        {
        }
        /// <summary>
        /// Initializes a new instance of InvalidDiscountException class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="inner">inner exception</param>
        public InvalidDiscountException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
