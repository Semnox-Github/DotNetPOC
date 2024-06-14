/********************************************************************************************
 * Project Name - ThirdParty
 * Description  -PostTransaction - This object hold the Center edge transaction information for POST card transactions
 * 
 **************
 **Version Log
 **************
 *Version     Date                  Modified By           Remarks          
 *********************************************************************************************
 *2.120.00    09-Apr-2021           Girish Kundar          Created 
 *******************************************************************************************/

namespace Semnox.Parafait.ThirdParty.CenterEdge
{
    /// <summary>
    /// This class is response object for card transactions 
    /// holds card number and either Adjustment transactions or gamePlay transactions
    /// This is used only gor Get of transactions
    /// </summary>
    public class PostTransaction
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public  TransactionDTO transaction { get; set; }
    }
}
