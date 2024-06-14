/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - Created to hold the card details .
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.130.7    07-Apr-2022   Abhishek           Created : External  REST API.
 ***************************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.ThirdParty.External
{

    public enum AdjustmentTypes
    {
        AddValue,
        AddProduct,
        RemoveProduct,
        RemoveValue,
        TransactionRefund,
        Cash,
        CreditDebit
       
    }
}
