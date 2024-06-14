/********************************************************************************************
* Project Name - Parafait_Kiosk -CardFunctions.cs
* Description  - CardFunctions 
* 
**************
**Version Log
**************
*Version     Date               Modified By        Remarks          
*********************************************************************************************
 * 2.80        09-Sep-2019      Deeksha            Added logger methods.
********************************************************************************************/
using System;
using System.Data;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.KioskCore;

namespace Parafait_FnB_Kiosk
{
    public class CardFunctions
    {
        // in case of new card issue with variable amount, create a carddeposit product before applying variable recharge
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static bool variableNewCardCheck(Transaction CurrentTrx, Card CurrentCard)
        {
            log.LogMethodEntry(CurrentTrx, CurrentCard);
            if (CurrentTrx.Utilities.ParafaitEnv.CardDepositProductId < 0)
            {
                throw new ApplicationException("CardDepositProduct not defined. Contact Manager");
            }

            string message = "";
            if (CurrentTrx.createTransactionLine(CurrentCard, CurrentTrx.Utilities.ParafaitEnv.CardDepositProductId, Common.utils.ParafaitEnv.CardFaceValue, 1, ref message) != 0)
            {
                throw new ApplicationException(message);
            }
            log.LogMethodExit(true);
            return true;
        }

        // in case of new card issue with variable amount, create a carddeposit product before applying variable recharge
        internal static decimal variableNewCardCheck(Transaction CurrentTrx, Card CurrentCard, decimal Amount)
        {
            log.LogMethodEntry(CurrentTrx, CurrentCard, Amount);
            DataTable dt = KioskStatic.Utilities.executeDataTable(@"select product_id, p.price, v.CustomDataNumber, p.product_name
                                                                                from products p, product_type pt, CustomDataView v 
                                                                                where pt.product_type_id = p.product_type_id 
                                                                                and pt.product_type = 'NEW' 
                                                                                and price <= @amount 
                                                                                and price > 0
                                                                                and p.CustomDataSetId = v.CustomDataSetId
                                                                                and v.Name = 'External System Identifier'
                                                                                and v.CustomDataNumber is not null
                                                                                order by p.price desc",
                                                                    new System.Data.SqlClient.SqlParameter("@amount", Amount));

            string message = "";
            if (dt.Rows.Count > 0)
            {
                int prodId = Convert.ToInt32(dt.Rows[0]["product_id"]);
                int externalRef = Convert.ToInt32(dt.Rows[0]["CustomDataNumber"]);
                double price = Convert.ToDouble(dt.Rows[0]["price"]);
                if (CurrentTrx.createTransactionLine(CurrentCard, prodId, price, 1, ref message) != 0)
                    throw new ApplicationException(message);

                Common.logToFile("Created split product. Ext Ref: " + externalRef.ToString() + ". Parafait product: " + dt.Rows[0]["product_name"].ToString());

                log.LogMethodExit(price);
                return (decimal)price;
            }
            else
            {
                if (variableNewCardCheck(CurrentTrx, CurrentCard))
                    return (decimal)Common.utils.ParafaitEnv.CardFaceValue;
            }

            log.LogMethodExit(0);
            return 0;
        }

        internal static int CreateSplitVariableProducts(Transaction CurrentTrx, Card CurrentCard, int variableProductId, double Amount, ref string message)
        {
            log.LogMethodEntry(CurrentTrx, CurrentCard, variableProductId, Amount, message);
            if (!KioskStatic.SPLIT_AND_MAP_VARIABLE_PRODUCT)
            {
                int ret = CurrentTrx.createTransactionLine(CurrentCard, variableProductId, Amount, 1, ref message);
                log.LogMethodExit(ret);
                return ret;
            }
            else
            {
                try
                {
                    while (Amount > 0)
                    {
                        DataTable dt = KioskStatic.Utilities.executeDataTable(@"select product_id, p.price, v.CustomDataNumber, p.product_name
                                                                                from products p, product_type pt, CustomDataView v 
                                                                                where pt.product_type_id = p.product_type_id 
                                                                                and pt.product_type = 'RECHARGE' 
                                                                                and price <= @amount 
                                                                                and price > 0
                                                                                and p.CustomDataSetId = v.CustomDataSetId
                                                                                and v.Name = 'External System Identifier'
                                                                                and v.CustomDataNumber is not null
                                                                                order by p.price desc",
                                                                                new System.Data.SqlClient.SqlParameter("@amount", Amount));

                        if (dt.Rows.Count > 0)
                        {
                            int prodId = Convert.ToInt32(dt.Rows[0]["product_id"]);
                            int externalRef = Convert.ToInt32(dt.Rows[0]["CustomDataNumber"]);
                            double price = Convert.ToDouble(dt.Rows[0]["price"]);
                            if (CurrentTrx.createTransactionLine(CurrentCard, prodId, price, 1, ref message) != 0)
                            {
                                log.LogMethodExit(-1);
                                return -1;
                            }
                            Amount -= price;

                            Common.logToFile("Created split product. Ext Ref: " + externalRef.ToString() + ". Parafait product: " + dt.Rows[0]["product_name"].ToString());
                        }
                        else
                        {
                            object oExternalRef = KioskStatic.getProductExternalSystemReference(variableProductId);
                            if (oExternalRef == DBNull.Value)
                            {
                                Common.logToFile("External System Reference for Variable product not found");
                            }

                            if (CurrentTrx.createTransactionLine(CurrentCard, variableProductId, Amount, 1, ref message) != 0)
                                Common.logToFile("Created split product. Ext Ref: " + oExternalRef.ToString() + ". Parafait variable product (" + Amount.ToString() + ")");
                            break;
                        }
                    }

                    log.LogMethodExit(0);
                    return 0;
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    log.LogMethodExit(-1);
                    return -1;
                }
            }
        }
    }
}
