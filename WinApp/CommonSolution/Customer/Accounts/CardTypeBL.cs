/********************************************************************************************
 * Project Name - Customer
 * Description  - BL of CardType 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.80        27-May-2020   Girish Kundar               Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer.Accounts
{
    public class CardTypeBL
    {
        private CardTypeDTO cardTypeDTO;
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        private CardTypeBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="CardTypeDTO">CardTypeDTO</param>
        public CardTypeBL(ExecutionContext executionContext, CardTypeDTO cardTypeDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, cardTypeDTO);
            this.cardTypeDTO = cardTypeDTO;
            log.LogMethodExit();
        }
        public CardTypeBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            CardTypeDataHandler cardTypeDataHandler = new CardTypeDataHandler(sqlTransaction);
            cardTypeDTO = cardTypeDataHandler.GetCardType(id);
            if (cardTypeDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "CardType", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
        }

        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            CardTypeDataHandler cardTypeDataHandler = new CardTypeDataHandler(sqlTransaction);
            ValidateData(sqlTransaction);
            if (cardTypeDTO.CardTypeId < 0)
            {
                cardTypeDTO = cardTypeDataHandler.Insert(cardTypeDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                cardTypeDTO.AcceptChanges();
            }
            else
            {
                if (cardTypeDTO.IsChanged)
                {
                    cardTypeDTO = cardTypeDataHandler.Update(cardTypeDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    cardTypeDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        public bool ValidateData(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            if (cardTypeDTO.CardType == "NONCARDTYPEMIGRATIONENTRY")
            {
                log.Debug("CardType == NONCARDTYPEMIGRATIONENTRY");
                return true;
            }
            else
            {
                CardTypeDataHandler cardTypeDataHandler = new CardTypeDataHandler(sqlTransaction);
                DataTable dtMaxDuration = cardTypeDataHandler.GetMaxQualifyingDuration(executionContext.GetSiteId());
                DateTime maxDurationBaseDate = DateTime.MinValue;
                DateTime maxDurationQDate = DateTime.MinValue;
                if (dtMaxDuration.Rows.Count > 0)
                {
                    maxDurationBaseDate = Convert.ToDateTime(dtMaxDuration.Rows[0]["Bdate"]);
                    if (dtMaxDuration.Rows[0]["MQdate"].ToString() != "")
                        maxDurationQDate = Convert.ToDateTime(dtMaxDuration.Rows[0]["MQdate"]);
                }
                if (maxDurationQDate == DateTime.MinValue)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1558));
                }
                List<KeyValuePair<CardTypeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CardTypeDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<CardTypeDTO.SearchByParameters, string>(CardTypeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                CardTypeListBL cardTypeListBL = new CardTypeListBL(executionContext);
                List<CardTypeDTO> existingCardTypeDTOList = cardTypeListBL.GetCardTypeDTOList(searchParameters);

                if (cardTypeDTO.CardTypeMigrated == false)
                {
                    if (cardTypeDTO.MembershipId == -1)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1565));
                    }
                    else
                    {
                        var tempDTOList = existingCardTypeDTOList.Where(x => x.MembershipId == cardTypeDTO.MembershipId && x.CardTypeId != cardTypeDTO.CardTypeId).ToList();
                        if (tempDTOList != null && tempDTOList.Any())
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1566));
                        }
                    }
                    if (cardTypeDTO.ExistingTriggerSource == 0)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1559));
                    }
                    int tempQualifyingDuration;
                    if (string.IsNullOrEmpty(cardTypeDTO.QualifyingDuration.ToString()) ||
                        !Int32.TryParse(cardTypeDTO.QualifyingDuration.ToString(), out tempQualifyingDuration))
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1561));
                    }
                    else
                    {
                        if (tempQualifyingDuration <= 0)
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1561));
                        }
                        DateTime tempDate = maxDurationBaseDate;
                        tempDate = tempDate.AddDays(Convert.ToUInt32(cardTypeDTO.QualifyingDuration));
                        if (tempDate < maxDurationQDate)
                        {
                            System.TimeSpan diffResult = maxDurationQDate.ToUniversalTime().Subtract(maxDurationBaseDate.ToUniversalTime());
                            if (cardTypeDTO.QualifyingDurationProceed == false)
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1562, diffResult.TotalDays));
                        }
                    }
                    decimal loyaltyPointConversionRatio;
                    if (string.IsNullOrEmpty(cardTypeDTO.LoyaltyPointConvRatio.ToString()) || !Decimal.TryParse(cardTypeDTO.LoyaltyPointConvRatio.ToString(), out loyaltyPointConversionRatio))
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1560));
                    }

                    else
                    {
                        if (loyaltyPointConversionRatio <= 0)
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1560));
                        }
                        var tempDTOList = existingCardTypeDTOList.Where(x => x.LoyaltyPointConvRatio == cardTypeDTO.LoyaltyPointConvRatio && x.CardTypeId != cardTypeDTO.CardTypeId).ToList();
                        if (tempDTOList != null && tempDTOList.Count > 1)
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1567));
                        }
                    }

                    int tempMigrationOrder;
                    if (cardTypeDTO.MigrationOrder == null || !Int32.TryParse(cardTypeDTO.MigrationOrder.ToString(), out tempMigrationOrder))
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1569));
                    }
                    else
                    {
                        if (tempMigrationOrder < 0)
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1569));
                        }
                        var tempDTOList = existingCardTypeDTOList.Where(x => x.MigrationOrder == cardTypeDTO.MigrationOrder && x.CardTypeId != cardTypeDTO.CardTypeId).ToList();
                        if (tempDTOList != null && tempDTOList.Any())
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1570));
                        }
                    }
                }
            }
            log.LogMethodExit(true);
            return true;
        }
    }



    public class CardTypeListBL
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        List<CardTypeDTO> cardTypeDTOList;
        /// <summary>
        /// parameterized constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public CardTypeListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public CardTypeListBL(ExecutionContext executionContext, List<CardTypeDTO> cardTypeDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.cardTypeDTOList = cardTypeDTOList;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the CardTypeDTO
        /// </summary>
        public List<CardTypeDTO> GetCardTypeDTOList(List<KeyValuePair<CardTypeDTO.SearchByParameters, string>> searchParameters,
                                SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            CardTypeDataHandler cardTypeDataHandler = new CardTypeDataHandler(sqlTransaction);
            List<CardTypeDTO> cardTypeDTOList = cardTypeDataHandler.GetCardTypeDTOList(searchParameters);
            log.LogMethodExit(cardTypeDTOList);
            return cardTypeDTOList;
        }

        public void Save(bool qualifyingDurationProceed = false)
        {
            log.LogMethodEntry();
            using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
            {
                try
                {
                    if (cardTypeDTOList != null && cardTypeDTOList.Any())
                    {
                        parafaitDBTrx.BeginTransaction();
                        //CardTypeMigrationBL cardTypeMigrationBL = new CardTypeMigrationBL(executionContext, cardTypeDTOList);
                        //cardTypeMigrationBL.ValidateData();
                        foreach (CardTypeDTO cardTypeDTO in cardTypeDTOList)
                        {
                            CardTypeBL cardTypeBL = new CardTypeBL(executionContext, cardTypeDTO);
                            cardTypeBL.Save(parafaitDBTrx.SQLTrx);
                        }
                        parafaitDBTrx.EndTransaction();
                    }
                }
                catch (SqlException sqlEx)
                {
                    log.Error(sqlEx);
                    parafaitDBTrx.RollBack();
                    log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                    if (sqlEx.Number == 547)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                    }
                    if (sqlEx.Number == 2601)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1872));
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (ValidationException valEx)
                {
                    log.Error(valEx);
                    parafaitDBTrx.RollBack();
                    log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                    throw;
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    parafaitDBTrx.RollBack();
                    log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                    throw;
                }
                log.LogMethodExit();
            }
        }
    }
}
