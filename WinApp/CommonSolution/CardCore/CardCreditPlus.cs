
/********************************************************************************************
 * Project Name - CardCore
 * Description  - Bussiness logic of the   CardcreditPlusclass
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *1.00       17-May-2017    Rakshith         Created 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.CardCore
{
    public class CardCreditPlus
    {
        private CardCreditPlusDTO cardCreditPlusDTO;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Default constructor
        /// </summary>
        public CardCreditPlus()
        {
            log.Debug("Starts-CardcreditPlus() default constructor");
            cardCreditPlusDTO = null;
            log.Debug("Ends-CardcreditPlus() default constructor");
        }


        /// <summary>
        /// Constructor with the CardcreditPlusDTO parameter
        /// </summary>
        /// <param name="cardCreditPlusDTO">CardCreditPlusDTO</param>
        public CardCreditPlus(CardCreditPlusDTO cardCreditPlusDTO)
        {
            log.Debug("Starts-CardcreditPlus(cardcreditPlusDTO) parameterized constructor.");
            this.cardCreditPlusDTO = cardCreditPlusDTO;
            log.Debug("Ends-CardcreditPlus(cardcreditPlusDTO) parameterized constructor.");
        }

        /// <summary>
        /// get CardcreditPlusDTO Object
        /// </summary>
        public CardCreditPlusDTO GetCardcreditPlusDTO
        {
            get { return cardCreditPlusDTO; }
        }

        /// <summary>
        /// Saves the CardcreditPlus
        /// Checks if the CardCreditPlusId is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save(SqlTransaction SQLTrx)
        {
            log.Debug("Starts-Save() method.");
            ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
            CardCreditPlusDataHandler cardcreditPlusDataHandler = new CardCreditPlusDataHandler();
            if (cardCreditPlusDTO.CardCreditPlusId < 0)
            {
                int cardCreditPlusId = cardcreditPlusDataHandler.InsertCardCreditPlusDTO(cardCreditPlusDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId(), SQLTrx);
                cardCreditPlusDTO.CardCreditPlusId = cardCreditPlusId;
            }
            else
            {
                if (cardCreditPlusDTO.IsChanged == true)
                {
                    cardcreditPlusDataHandler.UpdateCardCreditPlusDTO(cardCreditPlusDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId(), SQLTrx);
                    cardCreditPlusDTO.AcceptChanges();
                }
            }
            log.Debug("Ends-Save() method.");
        }


        /// <summary>
        /// returns the GetCreditPlus Balance of the cardId passed as parameter
        /// </summary>
        /// <param name="cardId">cardId</param>
        public double GetCreditPlusBalance(int cardId)
        {
            log.Debug("Begins- GetCreditPlusBalance(string cardNumber).");
            CardCreditPlusDataHandler cardCreditPlusDataHandler = new CardCreditPlusDataHandler();
            log.Debug("Begins- GetCreditPlusBalance(string cardNumber).");
            return cardCreditPlusDataHandler.GetCreditPlusBalance(cardId);
        }



        /// <summary>
        /// returns the GetCredit Plus Balances of the cardId passed as parameter
        /// </summary>
        /// <param name="cardId">cardId</param>
        public CardCreditPlusBalanceDTO GetCreditPlusBalances(int cardId)
        {
            log.Debug("Begins- GetCreditPlusBalance(string cardNumber).");
            CardCreditPlusDataHandler cardCreditPlusDataHandler = new CardCreditPlusDataHandler();
            log.Debug("Begins- GetCreditPlusBalance(string cardNumber).");
            return cardCreditPlusDataHandler.GetCreditPlusBalances(cardId);
        }




       /// <summary>
       /// Generic Method to deduct Credit Plus based on credit plus type
       /// </summary>
       /// <param name="cardId"></param>
       /// <param name="creditPlusType"></param>
       /// <param name="deductvalues"></param>
       /// <param name="SQLTrx"></param>
       /// <returns></returns>
        public double DeductGenericCreditPlus(int cardId, string creditPlusType, double deductvalues, SqlTransaction SQLTrx = null)
        {
            ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();

            double balance = deductvalues;
            
            List<KeyValuePair<CardCreditPlusDTO.SearchByParameters, string>> queryParameters = new List<KeyValuePair<CardCreditPlusDTO.SearchByParameters, string>>();
            queryParameters.Add(new KeyValuePair<CardCreditPlusDTO.SearchByParameters, string>(CardCreditPlusDTO.SearchByParameters.CARD_ID, cardId.ToString()));
            queryParameters.Add(new KeyValuePair<CardCreditPlusDTO.SearchByParameters, string>(CardCreditPlusDTO.SearchByParameters.CREDITPLUSTYPE, creditPlusType));

            CardCreditPlusDataHandler cardCreditPlusDataHandler = new CardCreditPlusDataHandler();
            List<CardCreditPlusDTO> CardCreditPlusDTOList = cardCreditPlusDataHandler.GetCardCreditPlusList(queryParameters);

            foreach (CardCreditPlusDTO lcCardCreditPlusDTO in CardCreditPlusDTOList)
            {
                if (balance > 0)
                {
                    if (balance > lcCardCreditPlusDTO.CreditPlusBalance)
                    {
                        balance = balance - lcCardCreditPlusDTO.CreditPlusBalance;
                        lcCardCreditPlusDTO.CreditPlusBalance = 0;
                    }
                    else
                    {
                        lcCardCreditPlusDTO.CreditPlusBalance = lcCardCreditPlusDTO.CreditPlusBalance - balance;
                        balance = 0;
                    }

                    if (lcCardCreditPlusDTO.IsChanged)
                    {
                        this.cardCreditPlusDTO = lcCardCreditPlusDTO;
                        this.Save(SQLTrx);
                    }
                }

                if (balance <= 0)
                {
                    break;
                }
            }
            return balance;
        }

    }




    /// <summary>
    ///  Manages the list of CardCreditPlus
    /// </summary>
    public class CardCreditPlusList
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Returns the Category list
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<CardCreditPlusDTO> GetCardCreditPlusList(List<KeyValuePair<CardCreditPlusDTO.SearchByParameters, string>> searchParameters)
        {
            log.Debug("Starts-GetCardCreditPlusList(searchParameters) method.");
            CardCreditPlusDataHandler cardCreditPlusDataHandler = new CardCreditPlusDataHandler();
            log.Debug("Ends-GetCardCreditPlusList(searchParameters) method by returning the result of cardCreditPlusDataHandler.GetCardCreditPlusList() call.");
            return cardCreditPlusDataHandler.GetCardCreditPlusList(searchParameters);
        }


        /// <summary>
        /// GetCardCreditPlusLoyaltyList
        /// </summary>
        /// <param name="cardParams"></param>
        /// <returns></returns>
        public List<CardCreditPlusDTO> GetCardCreditPlusLoyaltyList(CardParams cardParams)
        {
            log.Debug("Starts-GetCardCreditPlusLoyaltyList(cardParams) method.");
            CardCreditPlusDataHandler cardCreditPlusDataHandler = new CardCreditPlusDataHandler();
            log.Debug("Ends-GetCardCreditPlusLoyaltyList(cardParams) method by returning the result of cardCreditPlusDataHandler.GetCardCreditPlusLoyalty() call.");
            return cardCreditPlusDataHandler.GetCardCreditPlusLoyalty(cardParams);
        }

        /// <summary>
        /// GetCardCreditPlusLoyaltyList
        /// </summary>
        /// <param name="cardParams"></param>
        /// <returns></returns>
        public List<CardCreditPlusDTO> GetAllCardCreditPlus(List<KeyValuePair<CardCreditPlusDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(searchParameters);
            CardCreditPlusDataHandler cardCreditPlusDataHandler = new CardCreditPlusDataHandler();
            log.LogMethodExit();
            return cardCreditPlusDataHandler.GetAllCardCreditPlus(searchParameters, sqlTrx);
        } 

    }


}
