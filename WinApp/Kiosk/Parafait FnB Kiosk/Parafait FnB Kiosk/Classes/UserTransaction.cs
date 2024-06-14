using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using Semnox.Parafait.Transaction;
using Semnox.Core.Utilities;
using Semnox.Parafait.ThirdParty;
using Semnox.Core.GenericUtilities;

namespace Parafait_FnB_Kiosk
{
    public static class UserTransaction
    {
        public static clsOrderDetails OrderDetails = new clsOrderDetails(-1);

        public static void NewOrder(int NumberOfGuests)
        {
            Common.logEnter();
            OrderDetails = new clsOrderDetails(NumberOfGuests);
            Common.logExit();
        }

        internal static string getElementUserTotal(ScreenModel.UIPanelElement element)
        {
            Common.logEnter();
            string s = getElementTrx(element, false).Net_Transaction_Amount.ToString(Common.utils.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
            Common.logExit();

            return s;
        }

        internal static Transaction getElementTransaction(ScreenModel.UIPanelElement element, Transaction inTrx = null)
        {
            Common.logEnter();
            Transaction trx = getElementTrx(element, true, inTrx);
            Common.logExit();

            return trx;
        }

        static Transaction getElementTrx(ScreenModel.UIPanelElement element, bool OrderedOrSelectedValue, Transaction inTrx = null)
        {
            Common.logEnter();
            Transaction trx;
            if (inTrx == null)
            {
                Utilities utils = Common.utils;
                trx = new Transaction(utils);
            }
            else
                trx = inTrx;

            foreach (ScreenModel.ElementParameter parameter in element.AllParameters)
            {
                parameter.FreeAvailed = false;
            }

            int productId = -1;
            string message = "";

            int Quantity;
            object Value;

            ScreenModel.ElementParameter combo = element.Parameters.Find(x => x.ParameterType == ScreenModel.ParameterType.Combo);
            if (combo != null) // value meal
            {
                Value = combo.UserSelectedValue;

                if (Value != DBNull.Value && Value != null)
                {
                    int comboProductId = Convert.ToInt32(Value);
                    Transaction.TransactionLine ComboParent = new Transaction.TransactionLine();
                    int res = trx.createTransactionLine(null, comboProductId, -1, 1, ref message, ComboParent, false);
                    if (res != 0)
                        throw new ApplicationException(message);

                    foreach (ScreenModel.ElementParameter comboDetail in element.AllParameters)
                    {
                        switch (comboDetail.ParameterType)
                        {
                            case ScreenModel.ParameterType.Default: // F&B product in combo
                                {
                                    if (comboDetail.Toplevel == false)
                                        continue;

                                    Value = comboDetail.UserSelectedValue;

                                    if (Value == null
                                        || Value == DBNull.Value)
                                        continue;

                                    productId = Convert.ToInt32(Value);

                                    int childQty = comboDetail.OrderedQuantity;
                                    // check if default (non-displayed) product has modifiers. if it does not have, then use quantity defined in combo prod
                                    // if a displayed product has modifiers, it has to be defined in the parameters, separately for each quantity
                                    if (element.AllParameters.Find(x => comboDetail.Equals(x.ParentParameter)) == null && comboDetail.DisplayIndex == 0)
                                    {
                                        object oQty = Common.utils.executeScalar(
                                                 @"select isnull(Quantity, 1)
                                                from ComboProduct 
                                                where Product_Id = @productId
                                                  and ChildProductId = @childProductId 
                                                  and ISNULL(IsActive,1) = 1",
                                                    new SqlParameter("@productId", comboProductId),
                                                    new SqlParameter("@childProductId", productId));
                                        if (oQty != null)
                                            childQty = Convert.ToInt32(oQty);
                                    }
                                    
                                    while (childQty-- > 0)
                                    {
                                        Transaction.TransactionLine childFnBParent = new Transaction.TransactionLine();
                                        double price = 0;
                                        if (comboDetail.ParameterId < 0) // if user added parameter, consider product price. e.g., wristband
                                            price = -1;
                                        res = trx.createTransactionLine(null, productId, price, 1, ComboParent, ref message, childFnBParent);
                                        if (res != 0)
                                            throw new ApplicationException(message);

                                        bool halfHalf = false;

                                        int leftProdId = 0;
                                        int rightProdId = 0;
                                        // determine if half-half is chosen
                                        if (element.RightParameters.Find(x => comboDetail == x.ParentParameter) != null)
                                        {
                                            int.TryParse(Common.utils.getParafaitDefaults("LEFT_HALF_PIZZA_PRODUCT"), out leftProdId);
                                            if (leftProdId > 0)
                                            {
                                                int.TryParse(Common.utils.getParafaitDefaults("RIGHT_HALF_PIZZA_PRODUCT"), out rightProdId);
                                                if (rightProdId > 0)
                                                {
                                                    halfHalf = true;
                                                }
                                                else
                                                    throw new ApplicationException("Right-Half Pizza product not defined");
                                            }
                                            else
                                                throw new ApplicationException("Left-Half Pizza product not defined");
                                        }

                                        int IterCount = 2;
                                        if (halfHalf)
                                            IterCount = 3;

                                        int iter = 1;
                                        while (iter <= IterCount)
                                        {
                                            if (halfHalf)
                                            {
                                                if (iter == 2)
                                                {
                                                    res = trx.createTransactionLine(null, leftProdId, -1, 1, childFnBParent, ref message);
                                                    if (res != 0)
                                                        throw new ApplicationException(message);
                                                }
                                                else if (iter == 3)
                                                {
                                                    res = trx.createTransactionLine(null, rightProdId, -1, 1, childFnBParent, ref message);
                                                    if (res != 0)
                                                        throw new ApplicationException(message);
                                                }
                                            }

                                            List<ScreenModel.ElementParameter> childList;
                                            if (iter == 1)
                                                childList = element.Parameters;
                                            else if (iter == 2)
                                                childList = element.LeftParameters;
                                            else
                                                childList = element.RightParameters;

                                            foreach (ScreenModel.ElementParameter modifier in childList)
                                            {
                                                if (comboDetail == modifier.ParentParameter)
                                                {
                                                    createModifierLine(trx, modifier, childFnBParent, OrderedOrSelectedValue, halfHalf, iter);
                                                }
                                            }
                                            iter++;
                                        }
                                    }
                                }
                                break;
                            case ScreenModel.ParameterType.CardSale:
                                {
                                    Value = comboDetail.UserSelectedValue;

                                    if (Value == null
                                        || Value == DBNull.Value)
                                        continue;

                                    productId = Convert.ToInt32(Value);

                                    int qty = comboDetail.CardCount;
                                    bool FirstCard = true;
                                    while (qty-- > 0)
                                    {
                                        Transaction.TransactionLine cardLine = new Transaction.TransactionLine();

                                        string cardNumber = (new RandomTagNumber(Common.utils.ExecutionContext).Value);
                                        cardNumber = "T" + cardNumber.Substring(1);

                                        if (FirstCard == false)
                                            productId = trx.Utilities.ParafaitEnv.CardDepositProductId;

                                        res = trx.createTransactionLine(new Card(cardNumber, "", Common.utils), productId, -1, 1, ComboParent, ref message, cardLine);
                                        if (res != 0)
                                            throw new ApplicationException(message);

                                        cardLine.Price = 0;
                                        FirstCard = false;
                                    }
                                    break;
                                }
                        }
                    }
                }
            }
            else
            {
                foreach (ScreenModel.ElementParameter parameter in element.AllParameters)
                {
                    if (OrderedOrSelectedValue)
                    {
                        Quantity = parameter.OrderedQuantity;
                        Value = parameter.OrderedValue;
                    }
                    else
                    {
                        Quantity = parameter.UserQuantity;
                        Value = parameter.UserSelectedValue;
                    }

                    if (parameter.Toplevel)
                    {
                        if (Quantity > 0
                            && Value != null
                            && Value != DBNull.Value)
                        {
                            productId = Convert.ToInt32(Value);
                            DataRow dr = trx.getProductDetails(productId);

                            string prodType = dr["Product_type"].ToString();
                            switch (prodType)
                            {
                                case "NEW":
                                case "RECHARGE":
                                case "CARDSALE":
                                case "VARIABLECARD":
                                    {
                                        int qty = Math.Max(1, parameter.CardCount);
                                        bool FirstCard = true;
                                        while (qty-- > 0)
                                        {
                                            string cardNumber = parameter.CardNumber;
                                            if (string.IsNullOrEmpty(cardNumber))
                                            {
                                                cardNumber = (new RandomTagNumber(Common.utils.ExecutionContext).Value);
                                                cardNumber = "T" + cardNumber.Substring(1);
                                            }

                                            Card card = new Card(cardNumber, "", Common.utils);
                                            int result = -1;
                                            if (prodType == "VARIABLECARD" && FirstCard)
                                            {
                                                if (card.CardStatus == "NEW")
                                                {
                                                    decimal reducePrice = CardFunctions.variableNewCardCheck(trx, card, parameter.UserPrice);
                                                    if (reducePrice > 0)
                                                    {
                                                        if (CardFunctions.CreateSplitVariableProducts(trx, card, productId, (double)(parameter.UserPrice - reducePrice), ref message) != 0)
                                                        {
                                                            throw new ApplicationException(message);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (CardFunctions.CreateSplitVariableProducts(trx, card, productId, (double)parameter.UserPrice, ref message) != 0)
                                                    {
                                                        throw new ApplicationException(message);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (FirstCard == true)
                                                {
                                                    result = trx.createTransactionLine(card, productId, -1, 1, ref message);
                                                    FirstCard = false;
                                                }
                                                else
                                                    result = trx.createTransactionLine(card, trx.Utilities.ParafaitEnv.CardDepositProductId, 0, 1, ref message);

                                                if (result != 0)
                                                    throw new ApplicationException(message);
                                            }
                                        }
                                        break;
                                    }
                                case "MANUAL":
                                    {
                                        int qty = Quantity;
                                        Transaction.TransactionLine ParentLine = null;
                                        while (qty-- > 0)
                                        {
                                            ParentLine = new Transaction.TransactionLine();
                                            int res = trx.createTransactionLine(null, productId, -1, 1, ref message, ParentLine, false);
                                            if (res != 0)
                                                throw new ApplicationException(message);
                                        }

                                        bool halfHalf = false;

                                        int leftProdId = 0;
                                        int rightProdId = 0;
                                        if (element.RightParameters.Find(x => parameter == x.ParentParameter) != null)
                                        {
                                            int.TryParse(Common.utils.getParafaitDefaults("LEFT_HALF_PIZZA_PRODUCT"), out leftProdId);
                                            if (leftProdId > 0)
                                            {
                                                int.TryParse(Common.utils.getParafaitDefaults("RIGHT_HALF_PIZZA_PRODUCT"), out rightProdId);
                                                if (rightProdId > 0)
                                                {
                                                    halfHalf = true;
                                                }
                                                else
                                                    throw new ApplicationException("Right-Half Pizza product not defined");
                                            }
                                            else
                                                throw new ApplicationException("Left-Half Pizza product not defined");
                                        }

                                        int IterCount = 2;
                                        if (halfHalf)
                                            IterCount = 3;

                                        int iter = 1;
                                        while (iter <= IterCount)
                                        {
                                            if (halfHalf)
                                            {
                                                if (iter == 2)
                                                {
                                                    int res = trx.createTransactionLine(null, leftProdId, -1, 1, ParentLine, ref message);
                                                    if (res != 0)
                                                        throw new ApplicationException(message);
                                                }
                                                else if (iter == 3)
                                                {
                                                    int res = trx.createTransactionLine(null, rightProdId, -1, 1, ParentLine, ref message);
                                                    if (res != 0)
                                                        throw new ApplicationException(message);
                                                }
                                            }

                                            List<ScreenModel.ElementParameter> childList;
                                            if (iter == 1)
                                                childList = element.Parameters;
                                            else if (iter == 2)
                                                childList = element.LeftParameters;
                                            else
                                                childList = element.RightParameters;

                                            foreach (ScreenModel.ElementParameter child in childList)
                                            {
                                                if (parameter == child.ParentParameter)
                                                {
                                                    createModifierLine(trx, child, ParentLine, OrderedOrSelectedValue, halfHalf, iter);
                                                }
                                            }
                                            iter++;
                                        }
                                        break;
                                    }
                            }
                        }
                    }
                }
            }

            trx.updateAmounts();
            Common.logExit();
            return trx;
        }

        private static void createModifierLine(Transaction trx, ScreenModel.ElementParameter child, Transaction.TransactionLine ParentLine, bool OrderedOrSelectedValue, bool halfHalf, int iter)
        {
            Common.logEnter();
            int Quantity;
            object Value;

            if (OrderedOrSelectedValue)
            {
                Quantity = child.OrderedQuantity;
                Value = child.OrderedValue;
            }
            else
            {
                Quantity = child.UserQuantity;
                Value = child.UserSelectedValue;
            }

            if (child.DefaultSelected)
            {
                if (Value != DBNull.Value && Value != null)
                {
                    Common.logExit();
                    return;
                }
                else
                {
                    Value = child.DataSource.Rows[0][0];
                    Quantity = 1;
                }
            }

            if (Quantity <= 0
                || Value == null
                || Value == DBNull.Value)
                return;

            int productId = Convert.ToInt32(Value);
            DataRow drp = trx.getProductDetails(productId);
            //bool rightChild = false;
            //if (halfHalf && iter == 3)
            //    rightChild = true;

            int freeQty = 0;
            if (child.FreeQuantity > 0)
            {
                freeQty = child.FreeQuantity;
                List<ScreenModel.ElementParameter> siblings;
                if (halfHalf)
                {
                    //if (rightChild)
                    //    siblings = child.OwningElement.RightParameters;
                    //else
                    //    siblings = child.OwningElement.LeftParameters;
                    siblings = new List<ScreenModel.ElementParameter>();
                    siblings.AddRange(child.OwningElement.LeftParameters);
                    siblings.AddRange(child.OwningElement.RightParameters);
                    freeQty *= 2;
                }
                else
                {
                    if (child.OwningElement.LeftParameters.Contains(child))
                        siblings = child.OwningElement.LeftParameters;
                    else
                        siblings = child.OwningElement.Parameters;
                }

                foreach (ScreenModel.ElementParameter p in siblings)
                {
                    if (child.ParentParameter.Equals(p.ParentParameter) && p.FreeAvailed)
                        freeQty--;
                }
            }

            switch (drp["Product_type"].ToString())
            {
                case "MANUAL":
                    {
                        int qty = Quantity;
                        while (qty-- > 0)
                        {
                            string message = "";
                            int res = trx.createTransactionLine(null, productId, -1, 1, ParentLine, ref message);
                            if (res != 0)
                                throw new ApplicationException(message);

                            Transaction.TransactionLine tl = trx.TrxLines[trx.TrxLines.Count - 1];
                            tl.ModifierSetId = child.ModifierSetId;
                            if (freeQty > 0)
                            {
                                tl.Price = 0;
                                child.FreeAvailed = true;
                                freeQty--;
                            }
                            else if (halfHalf && iter > 1)
                            {
                                decimal leftPrice, rightPrice;
                              //  leftPrice = (decimal)Math.Round(tl.Price / 2, 2);
                              //  rightPrice = (decimal)tl.Price - leftPrice;
                                rightPrice = (decimal)Math.Round(tl.Price / 2, 2);
                                leftPrice = (decimal)tl.Price - rightPrice;
                                // find the number of trx lines within same modifier set
                                int count = trx.TrxLines.FindAll(x => ParentLine.Equals(x.ParentLine) && x.ModifierSetId == child.ModifierSetId && x.Price > 0).Count;
                                
                                if (count % 2 == 0) // even line
                                {
                                    tl.Price = (double)rightPrice;
                                }
                                else
                                {
                                    tl.Price = (double)leftPrice;
                                }
                            }

                            if (iter > 1)
                                tl.ModifierLine = true;

                            if (child.DefaultSelected)
                            {
                                tl.ProductName = "NO " + tl.ProductName;
                                tl.Price = 0;

                                tl.ParentModifierName = "NO";
                                tl.ParentModifierPrice = 0;

                                DataTable dtParentMod = Common.utils.executeDataTable(@"select top 1 m.modifierSetId, m.ModifierProductId 
                                                                                        from ModifierSetDetails m, products p
                                                                                        where p.product_name = 'NO'
                                                                                        and p.product_id = m.ModifierProductId");
                                if (dtParentMod.Rows.Count > 0)
                                {
                                    tl.ParentModifierSetId = Convert.ToInt32(dtParentMod.Rows[0][0]);
                                    tl.ParentModifierProductId = Convert.ToInt32(dtParentMod.Rows[0][1]);
                                }
                            }
                        }
                        break;
                    }
            }
            Common.logExit();
        }

        internal static void getOrderTotal()
        {
            Common.logEnter();
            OrderDetails.TotalAmount = OrderDetails.SubtotalAmount = OrderDetails.TaxAmount = OrderDetails.PlayPoints = 0;
            foreach (ScreenModel.UIPanelElement element in OrderDetails.ElementList)
            {
                Transaction trx = getElementTransaction(element);

                OrderDetails.TaxAmount += (decimal)trx.Tax_Amount;
                OrderDetails.SubtotalAmount += (decimal)trx.Pre_TaxAmount;
                OrderDetails.TotalAmount += (decimal)trx.Net_Transaction_Amount;

                foreach(Transaction.TransactionLine tl in trx.TrxLines)
                {
                    OrderDetails.PlayPoints += (decimal)(tl.Credits + tl.Bonus);
                }
            }
            Common.logExit();
        }

        internal static Transaction GetTransaction()
        {
            Common.logEnter();
            Utilities utils = Common.utils;
            Transaction trx = new Transaction(utils);

            foreach (ScreenModel.UIPanelElement element in OrderDetails.ElementList)
            {
                getElementTransaction(element, trx);
            }

            Common.logExit();
            return trx;
        }

        internal static void SaveTransaction(Transaction trx)
        {
            Common.logEnter();
            string message = "";
 
            if (trx.SaveTransacation(ref message) != 0)
            {
                throw new ApplicationException(message);
            }
            else
            {
                PrintTransaction printTrx = new PrintTransaction();
                TransactionUtils TransactionUtils = new TransactionUtils(Common.utils);
                Transaction newtrx = TransactionUtils.CreateTransactionFromDB(trx.Trx_id, Common.utils);
                
                if (!printTrx.Print(newtrx, ref message))
                    Common.ShowMessage(message);
            }
            Common.logExit();
        }

        internal static void AlohaLogin()
        {
            Common.logEnter();
            System.ServiceModel.BasicHttpsBinding binding = new System.ServiceModel.BasicHttpsBinding(System.ServiceModel.BasicHttpsSecurityMode.Transport);
            binding.SendTimeout = new TimeSpan(0, 0, 20);
            string ServiceUrl = Common.utils.getParafaitDefaults("PARAFAIT_GATEWAY_URL") + "/UserServices.asmx";

            UserServices.UserServicesSoapClient userServices = new UserServices.UserServicesSoapClient(binding, new System.ServiceModel.EndpointAddress(ServiceUrl));

            string loginId = Common.utils.getParafaitDefaults("ALOHA_USER_ID");
            string AlohaEmpPassword = Common.utils.getParafaitDefaults("ALOHA_USER_PASSWORD");

            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

            userServices.ValidateLogin(loginId, AlohaEmpPassword, "", Environment.MachineName, false);

            try
            {
                userServices.PerformAlohaClockIn(loginId, Environment.MachineName);
            }
            catch { }
            Common.logExit();
        }

        internal static void AlohaTransaction(Transaction currTransaction)
        {
            Common.logEnter();
            List<TransactionServices.ParafaitAlohaInterfaceItem> itemList = new List<TransactionServices.ParafaitAlohaInterfaceItem>();

            foreach (Transaction.TransactionLine currTrxLine in currTransaction.TrxLines)
            {
                currTrxLine.LineProcessed = false;
            }

            foreach (Transaction.TransactionLine currTrxLine in currTransaction.TrxLines)
            {
                if ((currTrxLine.ProductTypeCode != "COMBO") && (currTrxLine.ProductTypeCode != "CARDDEPOSIT") && !currTrxLine.LineProcessed)
                {
                    Common.logToFile("Item: " + currTrxLine.ProductName);

                    object productExtRef = Helper.GetProductExternalSystemReference(currTrxLine.ProductID);
                    if (productExtRef == DBNull.Value || productExtRef == null)
                        throw new Exception("External System Reference for Parafait product: " + currTrxLine.ProductName + " not found");

                    Common.logToFile("External Identifier: " + productExtRef.ToString());

                    List<TransactionServices.ParafaitAlohaInterfaceItem> modifierItemList = new List<TransactionServices.ParafaitAlohaInterfaceItem>();
                    for (int j = 0; j < currTransaction.TrxLines.Count; j++)
                    {
                        if (currTrxLine == currTransaction.TrxLines[j].ParentLine)
                        {
                            Common.logToFile("Modifier: " + currTransaction.TrxLines[j].ProductName);

                            object modifierProductExtRef = Helper.GetProductExternalSystemReference(currTransaction.TrxLines[j].ProductID);

                            if (modifierProductExtRef == DBNull.Value || modifierProductExtRef == null)
                            {
                                currTransaction.TrxLines[j].LineProcessed = true;
                                Common.logToFile("Null external identifier; ignore"); 
                                continue;
                            }

                            Common.logToFile("Modifier: " + modifierProductExtRef.ToString());

                            TransactionServices.ParafaitAlohaInterfaceItem modItem = new TransactionServices.ParafaitAlohaInterfaceItem();

                            modItem.ItemId = Convert.ToInt32(modifierProductExtRef);
                            modItem.Quantity = Convert.ToInt32(currTransaction.TrxLines[j].quantity);
                            modItem.Price = Convert.ToDouble(currTransaction.TrxLines[j].Price);
                            if (currTransaction.TrxLines[j].ProductName.StartsWith("NO "))
                            {
                                modItem.ModCode = (int)ParafaitAlohaInterfaceItem.MODCODES.NO;
                                Common.logToFile(currTransaction.TrxLines[j].ProductName + ": ModCode is " + modItem.ModCode.ToString());
                            }

                            modifierItemList.Add(modItem);

                            currTransaction.TrxLines[j].LineProcessed = true;
                        }
                    }

                    // move the first modifier to last in the list so that aloha does not apply free modifiers to crust, which is the first modifier in parafait
                    if (modifierItemList.Count > 1)
                    {
                        TransactionServices.ParafaitAlohaInterfaceItem firstModifier = modifierItemList[0];
                        TransactionServices.ParafaitAlohaInterfaceItem leftHalfModifier = null; //place holder for Left Half modifier item list
                        TransactionServices.ParafaitAlohaInterfaceItem rightHalfModifier = null; //place holder for Right half modifier item list
                        int rightProdId;
                        int leftProdId;
                        int.TryParse(Common.utils.getParafaitDefaults("LEFT_HALF_PIZZA_PRODUCT"), out leftProdId);
                        object leftHalfProductExtRef = Helper.GetProductExternalSystemReference(leftProdId);
                        int.TryParse(Common.utils.getParafaitDefaults("RIGHT_HALF_PIZZA_PRODUCT"), out rightProdId);
                        object rightHalfProductExtRef = Helper.GetProductExternalSystemReference(rightProdId);
                        for (int i = 0; i < modifierItemList.Count; i++)
                        {
                            if (leftHalfProductExtRef != DBNull.Value && leftHalfProductExtRef != null && modifierItemList[i].ItemId == Convert.ToInt32(leftHalfProductExtRef))
                            {
                                leftHalfModifier = modifierItemList[i];
                                Common.logToFile("Left Half Modifier " + leftHalfModifier.ItemId.ToString() + " identified");
                            }

                            if (rightHalfProductExtRef != DBNull.Value && rightHalfProductExtRef != null && modifierItemList[i].ItemId == Convert.ToInt32(rightHalfProductExtRef))
                            {
                                rightHalfModifier = modifierItemList[i];
                                if (leftHalfModifier != null)
                                {
                                    modifierItemList.Insert(i, leftHalfModifier);
                                    Common.logToFile("Left Half Modifier " + leftHalfModifier.ItemId.ToString() + " added before Right Half at position " + i.ToString());
                                    break;
                                }
                            }
                        }
                        if (rightHalfModifier != null)
                        {
                            modifierItemList.Add(rightHalfModifier);
                            Common.logToFile("Right Half Modifier " + rightHalfModifier.ItemId.ToString() + " added to end of Right Half modifier list");
                        }
                        if (leftHalfProductExtRef != DBNull.Value && leftHalfProductExtRef != null)
                        {
                            if (firstModifier.ItemId != Convert.ToInt32(leftHalfProductExtRef))
                            {
                                modifierItemList.RemoveAt(0);
                                modifierItemList.Add(firstModifier);
                                Common.logToFile("Modifier " + firstModifier.ItemId.ToString() + " moved to end");
                            }
                        }
                    }

                    TransactionServices.ParafaitAlohaInterfaceItem item = new TransactionServices.ParafaitAlohaInterfaceItem();
                    item.ItemId = Convert.ToInt32(productExtRef);
                    item.Quantity = Convert.ToInt32(currTrxLine.quantity);
                    item.Price = Helper.GetProductPrice(currTrxLine.ProductID);
                    item.ModifierList = modifierItemList.ToArray();

                    itemList.Add(item);
                    currTrxLine.LineProcessed = true;
                }
            }

            System.ServiceModel.BasicHttpsBinding binding = new System.ServiceModel.BasicHttpsBinding(System.ServiceModel.BasicHttpsSecurityMode.Transport);
            binding.SendTimeout = new TimeSpan(0, 0, 20);
            string ServiceUrl = Common.utils.getParafaitDefaults("PARAFAIT_GATEWAY_URL") + "/TransactionServices.asmx";
            
            TransactionServices.TransactionServicesSoapClient transactionServices = new TransactionServices.TransactionServicesSoapClient(binding, new System.ServiceModel.EndpointAddress(ServiceUrl));

            Common.logToFile("TransactionServices End Point: " + transactionServices.Endpoint.Address);
            double roundOffAmount = 0;
            roundOffAmount = currTransaction.TransactionPaymentsDTOList.Where(x => x.paymentModeDTO != null
                                                                            && x.paymentModeDTO.IsRoundOff).Sum(x => x.Amount);
            double paymentAmount = Convert.ToDouble((currTransaction.Net_Transaction_Amount - roundOffAmount));

            string loginId = Common.utils.getParafaitDefaults("ALOHA_USER_ID");
            string creditCardNumber = currTransaction.TransactionPaymentsDTOList.Find(x => x.paymentModeDTO != null 
                                                                                      && x.paymentModeDTO.IsCreditCard).CreditCardNumber;  //currTransaction..PaymentModeDetails[0].CreditCardNumber;
            string creditCardType = currTransaction.TransactionPaymentsDTOList.Find(x => x.paymentModeDTO != null
                                                                                      && x.paymentModeDTO.IsCreditCard).CreditCardName; //currTransaction..PaymentModeDetails[0].CreditCardName;
            string creditCardExpDate = currTransaction.TransactionPaymentsDTOList.Find(x => x.paymentModeDTO != null
                                                                                      && x.paymentModeDTO.IsCreditCard).CreditCardExpiry; //currTransaction..PaymentModeDetails[0].CreditCardExpiry;
            int creditCardInvoiceNumber = 0;
            double cashAmount = 0;
            cashAmount = currTransaction.TransactionPaymentsDTOList.Where(x => x.paymentModeDTO != null
                                                                            && x.paymentModeDTO.IsCash).Sum(x => x.Amount);
            if (cashAmount > 0)
                creditCardInvoiceNumber = -1;
            string creditCardPaymentReference = currTransaction.TransactionPaymentsDTOList.Find(x => x.paymentModeDTO != null
                                                                                      && x.paymentModeDTO.IsCreditCard).CreditCardAuthorization;   //currTransaction.StaticDataExchange.PaymentModeDetails[0].CreditCardAuthorization;
            string orderRemarks = OrderDetails.TableNumber;

            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

            TransactionServices.ParafaitStatusNameValue trxSuccessDetails = transactionServices.PerformAlohaTransaction(
                                                Environment.MachineName,
                                                loginId,
                                                itemList.ToArray(),
                                                paymentAmount,
                                                string.IsNullOrEmpty(creditCardNumber) ? "XXXX" : creditCardNumber,
                                                string.IsNullOrEmpty(creditCardType) ? "VISA" : creditCardType,
                                                creditCardExpDate,
                                                creditCardInvoiceNumber,
                                                creditCardPaymentReference,
                                                orderRemarks);

            currTransaction.Remarks = "Check Id: " + trxSuccessDetails.HResult;
            Common.logExit();
        }
    }
}
