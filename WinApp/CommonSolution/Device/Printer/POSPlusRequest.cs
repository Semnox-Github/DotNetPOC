/********************************************************************************************
 * Project Name - POS Plus Request
 * Description  - This contains the parameter passed in post transaction command.
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        06-May-2016   Raghuveera          Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.Printer
{
    public class POSPlusRequest
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Date time of the transaction
        /// </summary>
        public DateTime TransactionDate;
        /// <summary>
        /// Organization number
        /// </summary>
        public long OrganizationNumber;
        /// <summary>
        /// Cash register id
        /// </summary>
        public string CashRegisterId;
        /// <summary>
        /// Device Serial no
        /// </summary>
        public string SerialNo;
        /// <summary>
        /// Type of receipt
        ///normal(normal), kopia(copy), ovning(practice) or profo(profo).
        /// </summary>
        public string Type;
        /// <summary>
        /// Return amount
        /// </summary>
        public double ReturnAmount;
        /// <summary>
        /// Sale amount
        /// </summary>
        public double SaleAmount;
        /// <summary>
        /// VAT percentage array
        /// </summary>
        public double[] VATPercentage=new double[4];
        /// <summary>
        /// VATAmount array
        /// </summary>
        public double[] VATAmount=new double[4];
        /// <summary>
        /// By defualt CRC 0000 will be passed. This will be ignored my the posplus
        /// </summary>
        public string CRC;
    }
}
