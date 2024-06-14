/********************************************************************************************
 * Project Name - Staff Card Management DataHandler
 * Description  - Data handler of the Staff Card Management data handler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        17-Apr-2017   Amaresh         Created 
 ********************************************************************************************
 *1.00        24-Apr-2017   Suneetha        Modified 
 *2.50.0      03-Dec-2018   Mathew NInan    Deprecating Staticdataexchange class 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// Staff Card managemnt data handler class
    /// </summary>
    public class StaffCardManagementDataHandler
    {
        //Begin: Modified Added for logger function on 08-Mar-2016
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //End: Modified Added for logger function on 08-Mar-2016
        Utilities Utilities;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_utilities"></param>
        public StaffCardManagementDataHandler(Utilities _utilities)
        {
            Utilities = _utilities;
        }

        /// <summary>
        /// Get Staff card products based display group 
        /// </summary>
        /// <returns></returns>
        public DataTable GetProducts()
        {
            log.Debug("Starts-GetProducts()");
            DataTable cardProductsDT = null;
            try
            {
                int displayGrpId = -1;

                if (!DBNull.Value.Equals(Utilities.getParafaitDefaults("STAFF_CARD_PRODUCTS_DISPLAY_GROUP")))
                    displayGrpId = Convert.ToInt32(Utilities.getParafaitDefaults("STAFF_CARD_PRODUCTS_DISPLAY_GROUP"));

                cardProductsDT = Utilities.executeDataTable(@"SELECT p.product_id, p.product_name as product_name, 
		                                                            ISNULL(case p.description when '' then null else p.description end, p.product_name) as description, 
		                                                            pt.product_type, 
		                                                            ISNULL(case pdf.displayGroup when '' then null else pdf.displayGroup end, 'Others') display_group,  
		                                                            invP.ImageFileName, p.ButtonColor, p.TextColor, p.Font,
		                                                            pdf.ButtonColor DispGroupButtonColor, pdf.TextColor DispGroupTextColor, pdf.Font DispGroupFont
                                                            FROM products p LEFT OUTER JOIN product invP on p.product_id = invP.manualproductId
				                                                            LEFT OUTER JOIN ProductsDisplayGroup pdg on pdg.ProductId = p.product_id
				                                                            LEFT OUTER JOIN ProductDisplayGroupFormat pdf on pdf.Id = pdg.DisplayGroupId, 
				                                                            product_type pt 
                                                            WHERE p.product_type_id = pt.product_type_id 
				                                                AND p.active_flag = 'Y' 
				                                                AND p.DisplayInPOS = 'Y'
				                                                AND (p.POSTypeId = @Counter or @Counter = -1 or p.POSTypeId is null)
				                                                AND (p.expiryDate >= getdate() or p.expiryDate is null)
				                                                AND (p.StartDate <= getdate() or p.StartDate is null)
				                                                AND pdf.Id = @displayGrpId 
                                                            ORDER BY ISNULL(pdf.sortOrder,(select top 1 SortOrder from ProductDisplayGroupFormat where DisplayGroup = 'Others')), display_group, sort_order,
                                                            CASE product_type 
				                                                WHEN 'CARDSALE' then 0
				                                                WHEN 'NEW' then 1
				                                                WHEN 'RECHARGE' then 2 
				                                                WHEN 'VARIABLECARD' then 3 
				                                                WHEN 'GAMETIME' then 4
				                                                WHEN 'CHECK-IN' then 5
				                                                WHEN 'CHECK-OUT' then 6 
				                                                ELSE 7 end",
                                                new SqlParameter("@POSMachine", Utilities.ParafaitEnv.POSMachineId),
                                                new SqlParameter("@Counter", Utilities.ParafaitEnv.POSTypeId),
                                                new SqlParameter("@displayGrpId", displayGrpId));
                log.Debug("Ends-GetProducts()");
            }
            catch (Exception ex)
            {
                log.Error("Ends with error GetProducts()," + ex.Message);
                throw ex;
            }

            return cardProductsDT;
        }

        /// <summary>
        /// method will check staff card credit limit
        /// </summary>
        /// <param name="cardNumber">staff card number</param>
        /// <param name="prodId">product which is going load</param>
        /// <param name="message">return message</param>
        /// <returns></returns>
        public bool CheckStaffCardCreditLimit(string cardNumber, int prodId, ref string message)
        {
            log.Debug("Starts-CheckStaffCardCreditLimit()");
            bool retStatus = true;
            try
            {
                if (!string.IsNullOrEmpty(cardNumber))
                {
                    double staffCreditLmt = 0;
                    int staffGameLimit = 0;
                    double prodCredits = 0;
                    if (!string.IsNullOrEmpty(Utilities.getParafaitDefaults("STAFF_CARD_CREDITS_LIMIT")))
                        staffCreditLmt = Convert.ToDouble(Utilities.getParafaitDefaults("STAFF_CARD_CREDITS_LIMIT"));

                    int timeLimit;

                    try
                    {
                        timeLimit = Convert.ToInt32(Utilities.getParafaitDefaults("STAFF_CARD_TIME_LIMIT"));
                    }
                    catch
                    {
                        timeLimit = 30;
                    }

                    try
                    {
                        staffGameLimit = Convert.ToInt32(Utilities.getParafaitDefaults("STAFF_CARD_GAME_LIMIT"));
                    }
                    catch
                    {
                        staffGameLimit = 200;
                    }
                    log.Debug(staffGameLimit);

                    Transaction transaction = new Transaction(Utilities);
                    DataRow prodRow = transaction.getProductDetails(prodId);


                    if (prodRow["Credits"] != DBNull.Value)
                        prodCredits = Convert.ToInt32(prodRow["Credits"]);

                    prodCredits += Convert.ToInt32(Utilities.executeScalar(@"SELECT ISNULL(sum(CreditPlus), 0) 
                                                                                FROM ProductCreditPlus 
                                                                                WHERE product_Id = @productId 
                                                                                    AND CreditPlusType = 'A'",
                                                                                new SqlParameter("@productId", prodId)));

                    Card card = new Card(cardNumber, Utilities.ParafaitEnv.LoginID, Utilities);

                    if (staffCreditLmt > 0)
                    {
                        if (card.credits != 0 || card.CreditPlusCredits != 0 || card.CreditPlusCardBalance != 0 || prodCredits != 0)
                        {
                            double cardCredit = card.credits + card.CreditPlusCredits + card.CreditPlusCardBalance;
                            if ((cardCredit + prodCredits) > staffCreditLmt)
                            {
                                message = Utilities.MessageUtils.getMessage(1164);
                                retStatus = false;
                            }
                        }
                    }
                    if (timeLimit > 0)
                    {
                        log.Debug("Inside Time Limit check: " + prodId.ToString());
                        DataTable dtTime = Utilities.executeDataTable(@"SELECT sum(CreditPlus) CreditPlus
                                                                                FROM ProductCreditPlus
                                                                               WHERE product_id = @ProductId
                                                                                 AND creditPlusType = 'M'",
                                                                                new SqlParameter("@ProductId", prodId));
                        if (dtTime.Rows.Count > 0 && dtTime.Rows[0]["CreditPlus"] != DBNull.Value)
                        {
                            log.Debug("Value of Product Credit Plus : " + dtTime.Rows[0]["CreditPlus"].ToString());
                            int productTime = Convert.ToInt32(dtTime.Rows[0]["CreditPlus"]);
                            log.Debug("Total value of card : " + (card.time + card.CreditPlusTime).ToString());
                            if ((card.time + card.CreditPlusTime + Convert.ToDouble(productTime)) > Convert.ToDouble(timeLimit))
                            {
                                message = Utilities.MessageUtils.getMessage(1385);
                                retStatus = false;
                            }
                        }

                    }
                    if (staffGameLimit > 0)
                    {
                        CardGames cardGames = new CardGames(Utilities);
                        string entitlemenType = null;
                        int balanceTime = 0;

                        int cardGameCount = cardGames.getCardGames(card.card_id, -1, -1, ref entitlemenType, ref balanceTime);
                        DataTable dtCardGames = Utilities.executeDataTable(@"SELECT sum(quantity) CardGames
                                                                               FROM productgames
                                                                              WHERE game_id is null 
                                                                                AND game_profile_id is null
                                                                                AND product_id = @ProductId",
                                                                                new SqlParameter("@ProductId", prodId));
                        if (dtCardGames.Rows.Count > 0 && dtCardGames.Rows[0]["CardGames"] != DBNull.Value)
                        {
                            card.GetGameCount();
                            if (card.CardGames + Convert.ToInt32(dtCardGames.Rows[0]["CardGames"]) > staffGameLimit)
                            {
                                message = Utilities.MessageUtils.getMessage(1444);
                                retStatus = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Ends-CheckStaffCardCreditLimit() Error :" + ex.Message);
                return false;
            }
            log.Debug("Ends-CheckStaffCardCreditLimit()");
            return retStatus;
        }

        /// <summary>
        /// To get card games details
        /// </summary>
        /// <param name="cardId">input parameter to get matched card games</param>
        /// <returns>retunrs list of games</returns>
        public DataTable GetCardGames(int cardId)
        {
            DataTable gamesdt = null;
            try
            {
                gamesdt = Utilities.executeDataTable(@"SELECT SUM(BalanceGames) BalanceGames, ExpiryDate from cardGames  
                                                        WHERE card_id = @cardId 
                                                            AND ISNULL(ExpiryDate, Getdate()) >= getdate()  
                                                            AND BalanceGames > 0     
                                                        GROUP BY ExpiryDate 
                                                        ORDER BY ExpiryDate ASC                                                                            ",
                                                        new SqlParameter("@cardId", cardId));
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return gamesdt;
        }
    }
}
