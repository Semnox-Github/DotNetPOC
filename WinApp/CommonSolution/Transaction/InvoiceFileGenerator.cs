/********************************************************************************************
 * Project Name - InvoiceFileGenerator
 * Description  - File Generator class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By      Remarks          
 *********************************************************************************************
 *2.110.0     05-Jan-2021      Dakshakh Raj     Created for Peru Invoice Enhancement - Parafait Job changes
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Semnox.Parafait.Transaction
{

    /// <summary>
    /// InvoiceFileGenerator
    /// </summary>
    public class InvoiceFileGenerator
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// executionContext
        /// </summary>
        protected ExecutionContext executionContext;
        ///<summary>
        /// utilities
        /// </summary>
        protected Utilities utilities;
        /// <summary>
        /// InvoiceFileGenerator
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="utilities"></param>
        public InvoiceFileGenerator(ExecutionContext executionContext, Utilities utilities)
        { 
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.utilities = utilities;
            log.LogMethodExit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual void BuildInvoiceFile(Transaction trx)
        {
            log.LogMethodEntry(); 
            log.LogMethodExit(); 
        }
        /// <summary>
        /// GetEligibleInvoiceRecords
        /// </summary>
        /// <param name="utilities"></param>
        /// <param name="currentTime"></param>
        /// <param name="LastRunTime"></param>
        /// <returns></returns>
        public virtual List<TransactionDTO> GetEligibleInvoiceRecords(DateTime currentTime, DateTime LastRunTime)
        {
            log.LogMethodEntry();
            log.LogMethodExit();
            return null;
        }
        /// <summary>
        /// BuildHeaderSection
        /// </summary>
        /// <param name="trx"></param>
        /// <param name="invoiceType"></param>
        /// <param name="isReversal"></param>
        /// <returns></returns>
        public virtual string BuildHeaderSection(Transaction trx, String invoiceType, bool isReversal)
        {
            log.LogMethodEntry();
            log.LogMethodExit();
            return null;
        }
        /// <summary>
        /// BuildInvoiceLineSection
        /// </summary>
        /// <param name="trxLine"></param>
        /// <returns></returns>
        public virtual string BuildInvoiceLineSection(Transaction.TransactionLine trxLine)
        {
            log.LogMethodEntry();
            log.LogMethodExit();
            return null;
        }

        /// <summary>
        /// BuildLinesDetailSection
        /// </summary>
        /// <param name="trx"></param>
        /// <param name="invoiceType"></param>
        /// <param name="isReversal"></param>
        /// <returns></returns>
        public virtual string BuildTaxAndLegalSummaryDetailSection(Transaction trx, string invoiceType, bool isReversal)
        {
            log.LogMethodEntry();
            log.LogMethodExit();
            return null;
        }
    }
}
