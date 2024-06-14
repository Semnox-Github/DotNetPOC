/********************************************************************************************
 * Project Name - CardCoreBL Programs 
 * Description  - Data object of the CardCoreBL
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00        15-Nov-2016   Rakshith           Created 
 *2.70.0      18-Jul-2019   Mathew             Added logic to handle roaming data for card 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.DBSynch;
using Semnox.Parafait.CardCore;
using Semnox.Parafait.Device.PaymentGateway;

namespace Semnox.Parafait.CardCore
{
    public class CardCoreBL
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        CardCoreDTO cardCoreDTO;


        /// <summary>
        /// Default Constructor
        /// </summary>
        public CardCoreBL()
        {
            cardCoreDTO=new CardCoreDTO();
        }

        /// <summary>
        /// Cocnstuctor  CardCoreBL initailized based on cardCoreDto
        /// </summary>
        /// <param name="cardCoreDto">cardCoreDto</param>
        public CardCoreBL(CardCoreDTO cardCoreDto)
        {
            this.cardCoreDTO = cardCoreDto;
        }

        /// <summary>
        /// Cocnstuctor  CardCoreBL initailized based on cardCoreDto
        /// </summary>
        /// <param name="cardCoreDto">cardCoreDto</param
        /// <param name="executionContext">executionContext</param>
        public CardCoreBL(ExecutionContext executionContext, CardCoreDTO cardCoreDto)
        {
            this.cardCoreDTO = cardCoreDto;
            this.executionContext = executionContext;
        }
        /// <summary>
        /// Cocnstuctor  CardCoreBL initailized based on cardId
        /// </summary>
        /// <param name="cardId">cardId</param>
        public CardCoreBL(int cardId)
        { 
            CardCoreDatahandler cardCoreDatahandler=new CardCoreDatahandler();
            cardCoreDTO = cardCoreDatahandler.GetCardDTOById(cardId);
        }

        /// <summary>
        /// Cocnstuctor  CardCoreBL initailized based on CardNumber
        /// </summary>
        /// <param name="CardNumber">CardNumber</param>
        public CardCoreBL(string CardNumber)
        {
            CardCoreDatahandler cardCoreDatahandler = new CardCoreDatahandler();
            cardCoreDTO= cardCoreDatahandler.GetCardDTOByCardNumber(CardNumber);
        }


        /// <summary>
        /// gets the CardCoreDTO
        /// </summary>
        public CardCoreDTO GetCardCoreDTO { get { return cardCoreDTO; } }
        /*
        /// <summary>
        /// GetGamePlays(string cardNumber, string loginId) method
        /// </summary>
        /// <param name="cardNumber">cardNumber</param>
        /// <param name="loginId">loginId</param>
        /// <returns> returns List of GamePlayStruct  </returns>
        public List<GamePlayStruct> GetGamePlays(string cardNumber, string loginId)
        {
            log.Debug("Begins-GetGamePlays(string cardNumber, string loginId).");
            CardCoreDatahandler cardCoreDatahandler = new CardCoreDatahandler();
            log.Debug("Ends-GetGamePlays(string cardNumber, string loginId).");
            return cardCoreDatahandler.GetGamePlays(cardNumber, loginId);
        }


        public List<PurchasesStruct> GetPurchases(string cardNumber, string loginId)
        {
            log.Debug("Begins-GetPurchases(string cardNumber, string loginId).");
            CardCoreDatahandler cardCoreDatahandler = new CardCoreDatahandler();
            log.Debug("Ends-GetPurchases(string cardNumber, string loginId).");
            return cardCoreDatahandler.GetPurchases(cardNumber, loginId);
        }*/


        /*/// <summary>
        /// returns the details of the CardNumber passed as parameter
        /// </summary>
        /// <param name="CardNumber">CardNumber</param>
        public List<Semnox.Core.Utilities.CoreKeyValueStruct> GetCardDetails(string cardNumber, string loginId)
        {
            log.Debug("Starts-GetCardDetails(string cardNumber, string loginId) Method.");
            CardCoreDatahandler cardCoreDatahandler = new CardCoreDatahandler();
            log.Debug("Ends-GetCardDetails(string cardNumber, string loginId) Method.");
            return cardCoreDatahandler.GetCardDetails(cardNumber, loginId);
        }*/



        /// <summary>
        /// GetParentChildCardInfo(int customerId, ref int parentCardId) method
        /// </summary>
        /// <param name="customerId">customerId</param>
        /// <param name="parentCardId">parentCardId</param>
        /// <returns>returns list of CardCoreDTO</returns>
        public List<CardCoreDTO> GetParentChildCardInfo(int customerId, ref int parentCardId)
        {
            log.Debug("Begins- GetLinkedCardinfo(int customerId, ref int parentCardId, ref string retMessage).");
            CardCoreDatahandler cardCoreDatahandler = new CardCoreDatahandler();
            log.Debug("Ends- GetLinkedCardinfo(int customerId, ref int parentCardId, ref string retMessage).");
            return cardCoreDatahandler.GetParentChildCardInfo(customerId, ref parentCardId);
        }
        /// <summary>
        /// returns the card details associated with the cardParams passed as parameter
        /// </summary>
        /// <param name="cardParams">cardParams</param>
        /// <returns>returns list of CardCoreDTO</returns>
        public List<CardCoreDTO> GetCardsList(CardParams cardParams, Boolean loadCardDetails, System.Data.SqlClient.SqlTransaction sqlTrx = null)
        {
            log.Debug("Begins- GetCardsList(int customerId).");
            CardCoreDatahandler cardCoreDatahandler = new CardCoreDatahandler();
            log.Debug("Ends- GetCardsList(int customerId).");
            executionContext = ExecutionContext.GetExecutionContext();
            executionContext.SetUserId("Semnox");
            string  passPhrase =  ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_ENCRYPTION_PASS_PHRASE"); //
            return cardCoreDatahandler.GetAllCardsList(BuildCardSearchParameters(cardParams), loadCardDetails, sqlTrx, passPhrase);
        }

        /// <summary>
        /// returns the card details associated with the cardParams passed as parameter
        /// </summary>
        /// <param name="cardParams">cardParams</param>
        /// <returns>returns list of CardCoreDTO</returns>
        public List<CardCoreDTO> GetAllCardsList(CardParams cardParams, Boolean loadCardDetails, System.Data.SqlClient.SqlTransaction sqlTrx = null)
        {
            log.Debug("Begins- GetCardsList(int customerId).");
            CardCoreDatahandler cardCoreDatahandler = new CardCoreDatahandler();
            log.Debug("Ends- GetCardsList(int customerId).");
            List<KeyValuePair<CardCoreDTO.SearchByParameters, string>> mSearchParams = new List<KeyValuePair<CardCoreDTO.SearchByParameters, string>>();
            if (cardParams.CustomerId != -1)
                mSearchParams.Add(new KeyValuePair<CardCoreDTO.SearchByParameters, string>(CardCoreDTO.SearchByParameters.CUSTOMER_ID, Convert.ToString(cardParams.CustomerId)));
            if (cardParams.CardId != -1)
                mSearchParams.Add(new KeyValuePair<CardCoreDTO.SearchByParameters, string>(CardCoreDTO.SearchByParameters.CARD_ID, Convert.ToString(cardParams.CardId)));
            if (!String.IsNullOrEmpty(cardParams.CardNumber))
                mSearchParams.Add(new KeyValuePair<CardCoreDTO.SearchByParameters, string>(CardCoreDTO.SearchByParameters.CARD_NUMBER, Convert.ToString(cardParams.CardNumber)));
            return cardCoreDatahandler.GetAllCardsList(mSearchParams, loadCardDetails, sqlTrx);
        }

        /// <summary>
        /// returns the card details associated with the cardParams passed as parameter
        /// </summary>
        /// <param name="cardParams">cardParams</param>
        /// <returns>returns list of CardCoreDTO</returns>
        public List<CardCoreDTO> GetCardCoreDTOList(List<KeyValuePair<CardCoreDTO.SearchByParameters, string>> searchParam, Boolean loadCardDetails, System.Data.SqlClient.SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(searchParam, loadCardDetails, sqlTrx);
            CardCoreDatahandler cardCoreDatahandler = new CardCoreDatahandler();
            return cardCoreDatahandler.GetAllCardsList(searchParam, loadCardDetails, sqlTrx);
        }



        /// <summary>
        /// Deducts Tiket Count for the card for redemention
        /// </summary>
        /// <param name="cardParams">cardParams</param>
        /// <returns>returns list of CardCoreDTO</returns>
        public bool RedeemBalanceFromCard(int cardId,  int ticketCount, System.Data.SqlClient.SqlTransaction SQLTrx)
        {
            log.Debug("Begins- RedeemBalanceFromCard(int cardId, int ticketCount)).");

            if (ticketCount <= 0 )
            {
                throw new Exception("Redeem Value should be > 0 ");
            }
            CardCoreDatahandler cardCoreDatahandler = new CardCoreDatahandler();
            log.Debug("Ends- DeductRedemeptionFromCard(int cardId, int ticketCount).");
            return cardCoreDatahandler.RedeemBalanceFromCard(cardId, ticketCount, SQLTrx);
        }




        /// <summary>
        /// Takes MachineParams as parameter
        /// </summary>
        /// <returns>Returns List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> by converting MachineParams</returns>
        public List<KeyValuePair<CardCoreDTO.SearchByParameters, string>> BuildCardSearchParameters(CardParams cardParams)
        {
            log.Debug("Starts-BuildCardSearchParameters Method");
            List<KeyValuePair<CardCoreDTO.SearchByParameters, string>> mSearchParams = new List<KeyValuePair<CardCoreDTO.SearchByParameters, string>>();
            mSearchParams.Add(new KeyValuePair<CardCoreDTO.SearchByParameters, string>(CardCoreDTO.SearchByParameters.VALID_FLAG, "Y"));
            if (cardParams.CustomerId != -1)
                mSearchParams.Add(new KeyValuePair<CardCoreDTO.SearchByParameters, string>(CardCoreDTO.SearchByParameters.CUSTOMER_ID, Convert.ToString(cardParams.CustomerId)));
            if (cardParams.CardId != -1)
                mSearchParams.Add(new KeyValuePair<CardCoreDTO.SearchByParameters, string>(CardCoreDTO.SearchByParameters.CARD_ID, Convert.ToString(cardParams.CardId)));
            if (!String.IsNullOrEmpty(cardParams.CardNumber))
                mSearchParams.Add(new KeyValuePair<CardCoreDTO.SearchByParameters, string>(CardCoreDTO.SearchByParameters.CARD_NUMBER, Convert.ToString(cardParams.CardNumber)));
            if (!String.IsNullOrEmpty(cardParams.CustomerPhoneNumber))
                mSearchParams.Add(new KeyValuePair<CardCoreDTO.SearchByParameters, string>(CardCoreDTO.SearchByParameters.CUSTOMER_PHONE_NUMBER, Convert.ToString(cardParams.CustomerPhoneNumber)));

            log.Debug("Starts-BuildCardSearchParameters Method");
            return mSearchParams;
        }
        /// <summary>
        /// DeactivateFingerprint
        /// </summary>
        public bool DeactivateFingerprint(int cardId, string userId)
        {
            log.Debug("Starts-DeactivateFingerprint(int cardId, string userId)");
            CardCoreDatahandler cardCoreDatahandler = new CardCoreDatahandler();
            log.Debug("Ends-DeactivateFingerprint(int cardId, string userId)");
            return cardCoreDatahandler.DeactivateFingerprint(cardId, userId);
        }


        /// <summary>
        /// UpdateCardIdentifier
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="cardIdentifier"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool UpdateCardIdentifier(int cardId, string cardIdentifier, string userId)
        {
            log.Debug("Starts-UpdateCardIdentifier(int cardId, string cardIdentifier)");
            CardCoreDatahandler cardCoreDatahandler = new CardCoreDatahandler();
            log.Debug("Ends-UpdateCardIdentifier(int cardId, string cardIdentifier)");
            return cardCoreDatahandler.UpdateCardIdentifier(cardId, cardIdentifier, userId);
        }


        /// <summary>
        /// LinkCardToCustomer
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="customerId"></param>
        /// <param name="userId"></param>
        /// <returns>success or failure</returns>
        public bool LinkCardToCustomer(int cardId, int customerId, string userId, SqlTransaction sqlTrx = null)
        {
            log.Debug("Starts-UpdateCardIdentifier(int cardId, string cardIdentifier)");
            bool returnValue = false;
            CardCoreDatahandler cardCoreDatahandler = new CardCoreDatahandler();
            log.Debug("Ends-UpdateCardIdentifier(int cardId, string cardIdentifier)");
            returnValue = cardCoreDatahandler.LinkCardToCustomer(cardId, customerId, userId, sqlTrx);
            if (returnValue)
            {
                if (cardId > 0)
                {
                    try
                    {
                        CardCoreDTO cardDTO = cardCoreDatahandler.GetCardDTOById(cardId);
                        DBSynchLogService dBSynchLogService = new DBSynchLogService(ExecutionContext.GetExecutionContext(), "Cards", cardDTO.Guid, cardDTO.Site_id);
                        dBSynchLogService.CreateRoamingData(sqlTrx);
                    }
                    catch(Exception ex)
                    {
                        log.Error("Create roaming data failed for card: " + cardId.ToString(), ex);
                    }
                }
            }
            return returnValue;
        }

        private void LoadBonus(int cardId, int customerId,int productId, Utilities parafaitUtility, System.Data.SqlClient.SqlTransaction SQLTrx)
        {
            executionContext = ExecutionContext.GetExecutionContext();
            if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "REGISTRATION_BONUS_ON_VERIFICATION").Equals("N"))
            {
                string strProdId = ParafaitDefaultContainerList.GetParafaitDefault(parafaitUtility.ExecutionContext, "LOAD_PRODUCT_ON_REGISTRATION");
                //int productId = -1;

                if (int.TryParse(strProdId, out productId) == true && productId != -1)
                {
                    string message = "";
                    if (int.TryParse(strProdId, out productId) == true && productId != -1)
                    {
                        Type cardType = Type.GetType("Semnox.Parafait.Transaction.Card,Transaction");
                        object card = null;
                        if (cardType != null)
                        {
                            ConstructorInfo constructorN = cardType.GetConstructor(new Type[] { typeof(int), parafaitUtility.ParafaitEnv.LoginID.GetType(), typeof(Utilities), SQLTrx.GetType() });
                            card = constructorN.Invoke(new object[] { -1, parafaitUtility.ParafaitEnv.LoginID, parafaitUtility, SQLTrx });
                        }
                        else
                        {
                            throw new Exception("Unable to retrive Card class from assembly");
                        }

                        MethodInfo cardMethodType = cardType.GetMethod("getCardDetails", new[] { cardId.GetType(), SQLTrx.GetType() });
                        cardMethodType.Invoke(card, new object[] { cardId, SQLTrx });

                        Type transactionType = Type.GetType("Semnox.Parafait.Transaction.Transaction,Transaction");
                        object transaction = null;
                        if (transactionType != null)
                        {
                            ConstructorInfo constructorN = transactionType.GetConstructor(new Type[] { parafaitUtility.GetType() });
                            transaction = constructorN.Invoke(new object[] { parafaitUtility });
                        }
                        else
                        {
                            throw new Exception("Unable to retrive Transaction class from assembly");
                        }

                        decimal i = 1;
                        MethodInfo transactionMethodInfo = transactionType.GetMethod("createTransactionLine", new[] { card.GetType(), productId.GetType(), typeof(decimal), message.GetType().MakeByRefType() });
                        transactionMethodInfo.Invoke(transaction, new object[] { card, productId, i, message });

                        FieldInfo transactionFieldInfo = transactionType.GetField("Net_Transaction_Amount");
                        PaymentModeList paymentModeListBL = new PaymentModeList(executionContext);
                        List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchPaymentModeParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                        searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.SITE_ID, (parafaitUtility.ParafaitEnv.IsCorporate ? parafaitUtility.ParafaitEnv.SiteId : -1).ToString()));
                        searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISCASH, "Y"));
                        List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchPaymentModeParameters);
                        if (paymentModeDTOList != null)
                        {
                            TransactionPaymentsDTO trxPaymentDTO = new TransactionPaymentsDTO(-1, -1, paymentModeDTOList[0].PaymentModeId, Convert.ToInt32(transactionFieldInfo.GetValue(transaction)),
                                                                                                "", "", "", "", "", -1, "", -1, 0, -1, "", "", false, -1, -1, "", parafaitUtility.getServerTime(),
                                                                                                parafaitUtility.ParafaitEnv.LoginID, -1, null, 0, -1, parafaitUtility.ParafaitEnv.POSMachine, -1, "", null);
                            trxPaymentDTO.paymentModeDTO = paymentModeDTOList[0];

                            transactionFieldInfo = transactionType.GetField("TransactionPaymentsDTOList");
                            List<TransactionPaymentsDTO> transactionPaymentsDTOList = transactionFieldInfo.GetValue(transaction) as List<TransactionPaymentsDTO>;

                            transactionPaymentsDTOList.Add(trxPaymentDTO);
                        }

                        transactionMethodInfo = transactionType.GetMethod("SaveTransacation", new[] { SQLTrx.GetType(), message.GetType().MakeByRefType() });
                        object retVal = transactionMethodInfo.Invoke(transaction, new object[] { SQLTrx, message });

                        if (Convert.ToInt32(retVal) != 0)
                        {
                            throw new Exception("Unable to Load Registration Bonus!");
                        }
                    }
                }
            }
        }

        public void LoadVerificationBonus(int cardId, int customerId, Utilities parafaitUtility, System.Data.SqlClient.SqlTransaction SQLTrx)
        {
            executionContext = ExecutionContext.GetExecutionContext();
            string strProdId = ParafaitDefaultContainerList.GetParafaitDefault(parafaitUtility.ExecutionContext, "LOAD_PRODUCT_ON_REGISTRATION");
            int productId = -1;

            if (int.TryParse(strProdId, out productId) == true && productId != -1)
            {
                if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "REGISTRATION_BONUS_ON_VERIFICATION").Equals("Y"))
                {
                    LoadBonus(cardId, customerId, productId, parafaitUtility, SQLTrx);
                }
            }
        }


        public void LoadRegistrationBonus(int cardId, int customerId, Utilities parafaitUtility, System.Data.SqlClient.SqlTransaction SQLTrx, string macAddress)
        {
            executionContext = ExecutionContext.GetExecutionContext();
            string strProdId = ParafaitDefaultContainerList.GetParafaitDefault(parafaitUtility.ExecutionContext, "LOAD_PRODUCT_ON_REGISTRATION");
            int productId = -1;

            if (int.TryParse(strProdId, out productId) == true && productId != -1)
            {
                
                if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "REGISTRATION_BONUS_ON_VERIFICATION").Equals("N"))
                {
                    LoadBonus(cardId, customerId, productId, parafaitUtility, SQLTrx);
                }
                else
                {
                    log.Debug("Verification code to be sent");
                    Type customerType = Type.GetType("Semnox.Parafait.Customer.CustomerVerificationBL,Customer");
                    object customer = null;
                    if (customerType != null && !string.IsNullOrEmpty(macAddress))
                    {
                        ConstructorInfo constructorN = customerType.GetConstructor(new Type[] { executionContext.GetType() });
                        customer = constructorN.Invoke(new object[] { executionContext });
                    }
                    else
                    {
                        throw new Exception("Unable to retrive CustomerVerificationBL class from assembly");
                    }

                    MethodInfo customerMethodType = customerType.GetMethod("SendVerificationCode", new[] { customerId.GetType(), parafaitUtility.ParafaitEnv.LoginID.GetType(), macAddress.GetType() });
                    customerMethodType.Invoke(customer, new object[] { customerId, parafaitUtility.ParafaitEnv.LoginID, macAddress });
                }
            }
        }

        /// <summary>
        /// Saves the Cards
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            CardCoreDatahandler cardCoreDatahandler = new CardCoreDatahandler();

            if (executionContext == null)
            {
                executionContext = ExecutionContext.GetExecutionContext(); 
            }
            if (cardCoreDTO.CardId < 0)
            { 
                int id = cardCoreDatahandler.InsertCards(cardCoreDTO, executionContext.GetUserId(), executionContext.GetSiteId(), sqlTransaction);
                cardCoreDTO.CardId = id;
                cardCoreDTO.AcceptChanges();
            }
            else
            { 
                if (cardCoreDTO.IsChanged)
                {
                    cardCoreDatahandler.UpdateCards(cardCoreDTO, executionContext.GetUserId(), executionContext.GetSiteId(), sqlTransaction);
                    cardCoreDTO.AcceptChanges();
                }
            }
            //Updated linked objects
            if (cardCoreDTO.CardCreditPlusDTOList != null)
            {
                foreach (CardCreditPlusDTO cardCreditPlusDTO in cardCoreDTO.CardCreditPlusDTOList)
                {
                    if (cardCreditPlusDTO.CardCreditPlusId < 0 || cardCreditPlusDTO.IsChanged)
                    {
                        CardCreditPlus cardCreditPlusBl = new CardCreditPlus(cardCreditPlusDTO);
                        cardCreditPlusBl.Save(sqlTransaction); 
                    }
                }
            }
            if (cardCoreDTO.CardDiscountsDTOList != null)
            {
                foreach (CardDiscountsDTO cardDiscountsDTO in cardCoreDTO.CardDiscountsDTOList)
                {
                    if (cardDiscountsDTO.CardDiscountId < 0 || cardDiscountsDTO.IsChanged)
                    {
                        CardDiscountsBL cardDiscountsBL = new CardDiscountsBL(cardDiscountsDTO);
                        cardDiscountsBL.Save(sqlTransaction); 
                    }
                }
            }
            if (cardCoreDTO.CardGamesDTOList != null)
            {
                foreach (CardGamesDTO cardGamesDTO in cardCoreDTO.CardGamesDTOList)
                {
                    if (cardGamesDTO.CardGameId < 0 || cardGamesDTO.IsChanged)
                    {
                        CardGamesBL cardGamesBL = new CardGamesBL(executionContext, cardGamesDTO);
                        cardGamesBL.Save(sqlTransaction);
                    }
                }
            }
            log.LogMethodExit();
        }

        public void LoadDailyCardBalance(DateTime membershipProgressionDate)
        {
            log.LogMethodEntry(membershipProgressionDate);
            DateTime? startDate = null; 
            if( this.cardCoreDTO.CardCreditPlusDTOList == null && this.cardCoreDTO.CardCreditPlusDTOList.Count == 0)
            {
                CardCreditPlusList cardCreditPlusList = new CardCreditPlusList();
                List<KeyValuePair<CardCreditPlusDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<CardCreditPlusDTO.SearchByParameters, string>>();
                searchParams.Add(new KeyValuePair<CardCreditPlusDTO.SearchByParameters, string>(CardCreditPlusDTO.SearchByParameters.CARD_ID, this.cardCoreDTO.CardId.ToString()));
                //searchParams.Add(new KeyValuePair<CardCreditPlusDTO.SearchByParameters, string>(CardCreditPlusDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                List<CardCreditPlusDTO> cardCreditPlusDTOList = cardCreditPlusList.GetCardCreditPlusList(searchParams);
                if(cardCreditPlusDTOList != null && cardCreditPlusDTOList.Count > 1)
                {
                    this.cardCoreDTO.CardCreditPlusDTOList = new List<CardCreditPlusDTO>();
                    this.cardCoreDTO.CardCreditPlusDTOList.AddRange(cardCreditPlusDTOList);
                }
            }
            if (this.cardCoreDTO.CardCreditPlusDTOList != null && this.cardCoreDTO.CardCreditPlusDTOList.Count > 0)
            {
                DailyCardBalanceListBL dailyCardBalanceListBL = new DailyCardBalanceListBL(executionContext);
                List<KeyValuePair<DailyCardBalanceDTO.SearchByParameters, string>> searchParamDCB = new List<KeyValuePair<DailyCardBalanceDTO.SearchByParameters, string>>();
                searchParamDCB.Add(new KeyValuePair<DailyCardBalanceDTO.SearchByParameters, string>(DailyCardBalanceDTO.SearchByParameters.CARD_ID, this.cardCoreDTO.CardId.ToString()));
                //searchParamDCB.Add(new KeyValuePair<DailyCardBalanceDTO.SearchByParameters, string>(DailyCardBalanceDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                List<DailyCardBalanceDTO> dailyCardBalanceDTOList = dailyCardBalanceListBL.GetDailyCardBalanceDTOList(searchParamDCB);
                if (dailyCardBalanceDTOList != null && dailyCardBalanceDTOList.Count > 0)
                {
                    dailyCardBalanceDTOList = dailyCardBalanceDTOList.OrderByDescending(t => t.CardBalanceDate).ToList();
                    if (dailyCardBalanceDTOList[0].CardBalanceDate == DateTime.Now.Date) //IF latestEntryDate is today then  Skip, no entries required for today
                    {
                        startDate = null;
                    }
                    else if (dailyCardBalanceDTOList[0].CardBalanceDate == DateTime.Now.Date.AddDays(-1))  //ELSE IF latestEntryDate equal to Yeasterday then
                    {
                        startDate = DateTime.Now.Date;
                    }
                    else
                    {
                        if (membershipProgressionDate >= dailyCardBalanceDTOList[0].CardBalanceDate) //IF membershipEffectiveFromDate > latestEntryDate then
                        {
                            if (this.cardCoreDTO.Issue_date >= membershipProgressionDate) //IF Card Issue Date > membershipEffectiveFromDate Consider Card Issue Date as start day
                            {
                                startDate = this.cardCoreDTO.Issue_date.Date;
                            }
                            else //Else Consider MembershipEffectiveFrom date as start day
                            {
                                startDate = membershipProgressionDate.Date;
                            }
                        }
                        else //ELSE Consider latestEntryDate + 1 as start day
                        {
                            startDate = ((DateTime)dailyCardBalanceDTOList[0].CardBalanceDate).AddDays(1);
                        }
                    }
                }
                else
                {
                    //If MembershipEffectiveFromDate is within 24 hrs then only one entry is required
                    if (membershipProgressionDate >= DateTime.Now.AddHours(-24))
                    { startDate = DateTime.Now.Date; }
                    else
                    {    //Else membershipEffectiveFromDate > 24 hrs then  Check card issue date.
                        if (this.cardCoreDTO.Issue_date >= DateTime.Now.AddHours(-24))  //    If it is within 24 hrs then  Only one entry is required
                        {
                            startDate = DateTime.Now.Date;
                        }
                        else
                        {
                            // If Card Issue Date is > MembershipEffectiveFrom Date then  Consider Card Issue Date as start day
                            if (this.cardCoreDTO.Issue_date >= membershipProgressionDate)
                            {
                                startDate = this.cardCoreDTO.Issue_date.Date;
                            }
                            else  // Else Consider MembershipEffectiveFrom date as start day
                            {
                                startDate = membershipProgressionDate.Date;
                            }
                        }
                    }

                }
            } 

            if(startDate != null)
            {
                DateTime startDateValue = ((DateTime)startDate);
                DateTime startDateValueCheck = startDateValue.AddDays(15);
                if ( DateTime.Now.Date > startDateValueCheck)
                {
                    throw new Exception("More than 15 day data is missing");
                }
                else
                {
                    //For Tickets 
                    double totalTicketBalance = this.cardCoreDTO.Ticket_count + this.cardCoreDTO.CardCreditPlusDTOList.Where(cp =>( cp.CreditPlusType == "T" && (cp.PeriodTo != DateTime.MinValue ? cp.PeriodTo : DateTime.Now) >= DateTime.Now)).Sum(cpl => cpl.CreditPlusBalance);
                    double totalEarnedTicketBalance = this.cardCoreDTO.Ticket_count + this.cardCoreDTO.CardCreditPlusDTOList.Where(cp => (cp.CreditPlusType == "T" && (cp.MembershipRewardsId == -1) && (cp.PeriodTo != DateTime.MinValue ? cp.PeriodTo : DateTime.Now) >= DateTime.Now)).Sum(cpl => cpl.CreditPlusBalance);
                    //for LP
                    double totalLoyaltyPointsBalance = this.cardCoreDTO.Ticket_count + this.cardCoreDTO.CardCreditPlusDTOList.Where(cp => (cp.CreditPlusType == "L" && (cp.PeriodTo != DateTime.MinValue ? cp.PeriodTo : DateTime.Now) >= DateTime.Now)).Sum(cpl => cpl.CreditPlusBalance);
                    double totalRedeemableLoyaltyPointsBalance = this.cardCoreDTO.Ticket_count + this.cardCoreDTO.CardCreditPlusDTOList.Where(cp => (cp.CreditPlusType == "L" && (cp.MembershipRewardsId == -1) && (cp.ForMembershipOnly != "Y") && (cp.PeriodTo != DateTime.MinValue ? cp.PeriodTo : DateTime.Now) >= DateTime.Now)).Sum(cpl => cpl.CreditPlusBalance);
                    do
                    {
                        DailyCardBalanceDTO ticketDailyCardBalanceDTO  = new DailyCardBalanceDTO(-1, this.cardCoreDTO.Customer_id, this.cardCoreDTO.CardId, startDateValue, totalTicketBalance, totalEarnedTicketBalance, "T", executionContext.GetUserId(), DateTime.Now, executionContext.GetUserId(), DateTime.Now, "", executionContext.GetSiteId(), -1, false);
                        DailyCardBalanceDTO loayltyDailyCardBalanceDTO  = new DailyCardBalanceDTO(-1, this.cardCoreDTO.Customer_id, this.cardCoreDTO.CardId, startDateValue, totalLoyaltyPointsBalance, totalRedeemableLoyaltyPointsBalance, "L", executionContext.GetUserId(), DateTime.Now, executionContext.GetUserId(), DateTime.Now, "", executionContext.GetSiteId(), -1, false);
                        DailyCardBalanceBL ticketDailyCardBalanceBL = new DailyCardBalanceBL(executionContext, ticketDailyCardBalanceDTO);
                        DailyCardBalanceBL loyaltyDailyCardBalanceBL = new DailyCardBalanceBL(executionContext, loayltyDailyCardBalanceDTO);
                        ticketDailyCardBalanceBL.Save();
                        loyaltyDailyCardBalanceBL.Save();
                        startDateValue = startDateValue.AddDays(1);
                    }
                    while (startDateValue < DateTime.Now);
                }
            }
            log.LogMethodExit();
        }

    }
}
