using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.CardCore
{

    public class CardCreditPlusConsumptionHandler
    {

        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DataAccessHandler dataAccessHandler;

        ////<summary>
        ////For search parameter Specified
        ////</summary>
        private static readonly Dictionary<CardCreditPlusConsumptionDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<CardCreditPlusConsumptionDTO.SearchByParameters, string>
        {
            {CardCreditPlusConsumptionDTO.SearchByParameters.PKId, "PKId"},
            {CardCreditPlusConsumptionDTO.SearchByParameters.CARD_CREDIT_PLUS_ID, "CardCreditPlusId"}
        };


        /// <summary>
        /// Default constructor of CardcreditPlusDataHandler class
        /// </summary>
        public CardCreditPlusConsumptionHandler()
        {
            log.Debug("Starts-CardCreditPlusConsumptionHandler() default constructor.");
            dataAccessHandler = new DataAccessHandler();
            log.Debug("Ends-CardCreditPlusConsumptionHandler() default constructor.");
        }



        /// <summary>
        /// return CardCreditPlusConsumptionDTO from datarow
        /// </summary>
        /// <param name="cardCreditPlusConsumptionDataRow"></param>
        /// <returns></returns>
        private CardCreditPlusConsumptionDTO GetCardCreditPlusConsumptionDTO(DataRow cardCreditPlusConsumptionDataRow)
        {
            log.Debug("Starts-GetCardCreditPlusConsumptionDTO(cardCreditPlusConsumptionDataRow) Method.");
            CardCreditPlusConsumptionDTO cardcreditPlusConsumptionDTO = new CardCreditPlusConsumptionDTO
            (
                cardCreditPlusConsumptionDataRow["PKIdPk"] == DBNull.Value ? -1 : Convert.ToInt32(cardCreditPlusConsumptionDataRow["PKIdPk"]),
                cardCreditPlusConsumptionDataRow["CardCreditPlusId"] == DBNull.Value ? -1 : Convert.ToInt32(cardCreditPlusConsumptionDataRow["CardCreditPlusId"]),
                cardCreditPlusConsumptionDataRow["POSTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(cardCreditPlusConsumptionDataRow["POSTypeId"]),
                cardCreditPlusConsumptionDataRow["ExpiryDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(cardCreditPlusConsumptionDataRow["ExpiryDate"]),
                cardCreditPlusConsumptionDataRow["ProductId"] == DBNull.Value ? -1 : Convert.ToInt32(cardCreditPlusConsumptionDataRow["ProductId"]),
                cardCreditPlusConsumptionDataRow["GameProfileId"] == DBNull.Value ? -1 : Convert.ToInt32(cardCreditPlusConsumptionDataRow["GameProfileId"]),
                cardCreditPlusConsumptionDataRow["GameId"] == DBNull.Value ? -1 : Convert.ToInt32(cardCreditPlusConsumptionDataRow["GameId"]),
                cardCreditPlusConsumptionDataRow["DiscountedPrice"] == DBNull.Value ? -1 : Convert.ToDouble(cardCreditPlusConsumptionDataRow["DiscountedPrice"]),
                cardCreditPlusConsumptionDataRow["DiscountPercentage"] == DBNull.Value ? -1 : Convert.ToDecimal(cardCreditPlusConsumptionDataRow["DiscountPercentage"]),
                cardCreditPlusConsumptionDataRow["DiscountAmount"] == DBNull.Value ? -1 : Convert.ToDecimal(cardCreditPlusConsumptionDataRow["DiscountAmount"]),
                cardCreditPlusConsumptionDataRow["ConsumptionBalance"] == DBNull.Value ? -1 : Convert.ToInt32(cardCreditPlusConsumptionDataRow["ConsumptionBalance"]),
                cardCreditPlusConsumptionDataRow["QuantityLimit"] == DBNull.Value ? -1 : Convert.ToInt32(cardCreditPlusConsumptionDataRow["QuantityLimit"]),
                cardCreditPlusConsumptionDataRow["CategoryId"] == DBNull.Value ? -1 : Convert.ToInt32(cardCreditPlusConsumptionDataRow["CategoryId"]),
                cardCreditPlusConsumptionDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(cardCreditPlusConsumptionDataRow["site_id"]),
                cardCreditPlusConsumptionDataRow["Guid"].ToString(),
                cardCreditPlusConsumptionDataRow["LastUpdatedBy"].ToString(),
                cardCreditPlusConsumptionDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(cardCreditPlusConsumptionDataRow["LastupdatedDate"]),
                cardCreditPlusConsumptionDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(cardCreditPlusConsumptionDataRow["SynchStatus"]),
                cardCreditPlusConsumptionDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(cardCreditPlusConsumptionDataRow["MasterEntityId"])
             );

            log.Debug("Ends-GetCardCreditPlusConsumptionDTO(cardCreditPlusConsumptionDataRow) Method.");
            return cardcreditPlusConsumptionDTO;
        }


        /// <summary>
        /// GetCardCreditPlusConsumptionDTOList from dataTable
        /// </summary>
        /// <param name="dtInConsumption"></param>
        /// <returns>List of CardCreditPlusConsumptionDTO</returns>
        private List<CardCreditPlusConsumptionDTO> GetCardCreditPlusConsumptionDTOList(DataTable dtInConsumption)
        {
            log.Debug("Start-GetCardCreditPlusConsumptionDTOList(dtInConsumption) Method.");

            List<CardCreditPlusConsumptionDTO> cardCreditPlusConsumptionDTOList = new List<CardCreditPlusConsumptionDTO>();
            if (dtInConsumption.Rows.Count > 0)
            {
                foreach (DataRow ccpRow in dtInConsumption.Rows)
                {
                    CardCreditPlusConsumptionDTO ccpDTO = GetCardCreditPlusConsumptionDTO(ccpRow);
                    cardCreditPlusConsumptionDTOList.Add(ccpDTO);
                }
            }
            log.Debug("Ends-GetCardCreditPlusConsumptionDTOList(dtInConsumption) Method.");
            return cardCreditPlusConsumptionDTOList;

        }



        /// <summary>
        /// Gets the CardCreditPlusConsumptionDTO matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CardCreditPlusConsumption matching the search criteria</returns>
        public DataTable GetCardCreditPlusConsumption(int cardCreditPlusId)
        {
            log.Debug("Starts-GetCardCreditPlusConsumption(cardCreditPlusId) Method.");
            try
            {
                string selectCCPQuery = @"SELECT 
                                            [POSTypeName] Counter
                                            ,[Name] Category
                                            ,[Product_name]
                                            ,[profile_name] Game_Profile
                                            ,[Game_name]
                                            ,[DiscountPercentage] disc_Percent
                                            ,[DiscountAmount] Disc_Amount
                                            ,[DiscountedPrice] Disc_Price
                                            ,[ConsumptionBalance] Balance
                                            ,cn.ExpiryDate expiry_date
                                            ,cn.QuantityLimit Qty_Limit
                                        FROM [CardCreditPlusConsumption] cn
                                            left outer join POSTypes p
                                            on p.POSTypeId = cn.POSTypeId 
                                            left outer join Products pr
                                            on pr.Product_Id = cn.ProductId
                                            left outer join game_profile gp
                                            on gp.game_profile_id = cn.gameProfileid 
                                            left outer join games g 
                                            on g.game_id = cn.gameId
                                            left outer join Category c 
                                            on c.categoryId = cn.CategoryId
                                        where CardCreditPlusId = @CardCreditPlusId ";

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@CardCreditPlusId", cardCreditPlusId));

                DataTable dtCardCreditPlusConsumption = dataAccessHandler.executeSelectQuery(selectCCPQuery, parameters.ToArray());
                log.Debug("Ends-GetCardCreditPlusConsumption(cardCreditPlusId) Method.");
                return dtCardCreditPlusConsumption;

            }
            catch (Exception ex)
            {
                log.Log("Error-GetCardCreditPlusConsumption ", ex);
                throw ;
            }
        }


    }
}
