/********************************************************************************************
 * Project Name - ThirdParty
 * Description  -CardTransaction - This object hold the Center edge transaction information for Get card transactions
 * 
 **************
 **Version Log
 **************
 *Version     Date                  Modified By           Remarks          
 *********************************************************************************************
 *2.120.00    09-Apr-2021           Girish Kundar          Created 
 *******************************************************************************************/
using System.Collections.Generic;

namespace Semnox.Parafait.ThirdParty.CenterEdge
{
    /// <summary>
    /// This class is response object for card transactions 
    /// holds card number and either Adjustment transactions or gamePlay transactions
    /// This is used only gor Get of transactions
    /// </summary>
    public class CardTransaction
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string cardNumber { get; set; }
        public List<TransactionDTO> transactions { get; set; }
        public int skipped { get; set; }
        public int totalCount { get; set; }
        public int? sinceId { get; set; }
    }
}
