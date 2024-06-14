/********************************************************************************************
* Project Name - Parafait_Kiosk -frmProductSaleValueMeal.cs
* Description  - frmProductSaleValueMeal 
* 
**************
**Version Log
**************
*Version     Date               Modified By        Remarks          
*********************************************************************************************
 * 2.80        09-Sep-2019      Deeksha            Added logger methods.
********************************************************************************************/
using Semnox.Parafait.Transaction;
using System;
using System.Collections.Generic;
using System.Data;

namespace Parafait_FnB_Kiosk
{
    public partial class frmProductSaleValueMeal : BaseForm
    {
        public frmProductSaleValueMeal()
        {
            log.LogMethodEntry();
            InitializeComponent();

            this.Load += frmProductSaleValueMeal_Load;
            log.LogMethodExit();
        }

        void loadScreens()
        {
            log.LogMethodEntry();
            List<ScreenModel.UIPanelElement> elementList = new List<ScreenModel.UIPanelElement>();
            List<ScreenModel.ElementParameter> processedList = new List<ScreenModel.ElementParameter>();
            foreach (ScreenModel.ElementParameter parameter in _callingElement.Parameters)
            {
                if (parameter.ActionScreenId != -1 && !processedList.Contains(parameter))
                {
                    ScreenModel.UIPanelElement element = _callingElement.Clone();
                    element.Parameters.Clear();
                    element.ActionScreenId = parameter.ActionScreenId;
                    foreach (ScreenModel.ElementParameter param in _callingElement.Parameters.FindAll(x => (x.ActionScreenId == parameter.ActionScreenId && x.ScreenGroup == parameter.ScreenGroup)))
                    {
                        element.Parameters.Add(param);
                        foreach(ScreenModel.ElementParameter child in _callingElement.LeftParameters)
                        {
                            if (param.Equals(child.ParentParameter))
                                element.LeftParameters.Add(child);
                        }

                        foreach (ScreenModel.ElementParameter child in _callingElement.RightParameters)
                        {
                            if (param.Equals(child.ParentParameter))
                                element.RightParameters.Add(child);
                        }

                        processedList.Add(param);
                    }
                    elementList.Add(element);
                }
            }

            foreach(ScreenModel.UIPanelElement element in elementList)
            {
                Common.OpenScreen(element);
                if (UserTransaction.OrderDetails.ElementList.Contains(element))
                {
                    UserTransaction.OrderDetails.ElementList.Remove(element);

                    foreach (ScreenModel.ElementParameter param in element.Parameters)
                    {
                        ScreenModel.ElementParameter p = _callingElement.Parameters.Find(x => x.identifier == param.identifier);
                        if (p != null)
                            _callingElement.Parameters.Remove(p);

                        _callingElement.Parameters.Add(param);
                    }

                    foreach (ScreenModel.ElementParameter param in element.LeftParameters)
                    {
                        ScreenModel.ElementParameter p = _callingElement.LeftParameters.Find(x => x.identifier == param.identifier);
                        if (p != null)
                            _callingElement.LeftParameters.Remove(p);
                     
                        _callingElement.LeftParameters.Add(param);
                    }

                    if (element.RightParameters.Count == 0)
                    {
                        ScreenModel.ElementParameter parent = element.Parameters.Find(x => x.Toplevel);
                        if (parent != null)
                        {
                            foreach (ScreenModel.ElementParameter param in _callingElement.RightParameters.FindAll(x => parent.Equals(x.ParentParameter)))
                            {
                                _callingElement.RightParameters.Remove(param);
                            }
                        }
                    }

                    foreach (ScreenModel.ElementParameter param in element.RightParameters)
                    {
                        ScreenModel.ElementParameter p = _callingElement.RightParameters.Find(x => x.identifier == param.identifier);
                        if (p != null)
                            _callingElement.RightParameters.Remove(p);

                        _callingElement.RightParameters.Add(param);
                    }
                }
                else // cancel pressed in one of the screens
                {
                    UserTransaction.getOrderTotal();
                    Close();
                    log.LogMethodExit();
                    return;
                }
            }

            foreach (ScreenModel.ElementParameter parameter in _callingElement.AllParameters)
            {
                parameter.OrderedValue = parameter.UserSelectedValue;
                parameter.OrderedQuantity = parameter.UserQuantity;
            }

            UserTransaction.OrderDetails.AddItem(_callingElement);

            UserTransaction.getOrderTotal();

            Close();
            log.LogMethodExit();
        }

        void frmProductSaleValueMeal_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Opacity = 0;
            try
            {
                TransactionUtils trxUtils = new TransactionUtils(Common.utils);
                foreach (ScreenModel.ElementParameter parameter in _callingElement.Parameters)
                {
                    if (parameter.DisplayIndex == 0 // not displayed
                        && parameter.UserSelectedValue != null
                        && parameter.UserSelectedValue != DBNull.Value)
                    {
                        DataTable dt = trxUtils.getProductDetails((int)parameter.UserSelectedValue, null);
                        if (dt.Rows.Count > 0)
                        {
                            switch (dt.Rows[0]["product_type"].ToString())
                            {
                                case "NEW":
                                case "RECHARGE":
                                case "CARDSALE": parameter.ParameterType = ScreenModel.ParameterType.CardSale; break;
                                case "COMBO": parameter.ParameterType = ScreenModel.ParameterType.Combo; break;
                            }
                        }
                    }
                }

                loadScreens();
            }
            catch (Exception ex)
            {
                Common.logException(ex);
                Common.ShowMessage(ex.Message);
                Close();
            }
            log.LogMethodExit();
        }
    }
}
