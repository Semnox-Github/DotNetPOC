/********************************************************************************************
 * Project Name - InvoiceFileGeneratorFactory
 * Description  - Factory class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By      Remarks          
 *********************************************************************************************
 *2.110.0     05-Jan-2021      Dakshakh Raj     Created for Peru Invoice Enhancement - Parafait Job changes
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using System;
using System.Data.SqlClient;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// InvoiceFileGenerators
    /// </summary>
    public enum InvoiceFileGenerators
    {
        /// <summary>
        /// None.
        /// </summary>
        None,
        /// <summary>
        /// Peru Invoice File Generator
        /// </summary>
        PERU
    }
    /// <summary>
    /// InvoiceFileGeneratorFactory
    /// </summary>
    public class InvoiceFileGeneratorFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private InvoiceFileGenerator invoiceFileGenerator = null;
        private ExecutionContext executionContext;
        /// <summary>
        /// /GetInvoiceFileGenerator
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="utilities"></param>
        /// <param name="invoiceFileGeneratorName"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public InvoiceFileGenerator GetInvoiceFileGenerator(ExecutionContext executionContext, Utilities utilities,  string invoiceFileGeneratorName, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(executionContext, invoiceFileGeneratorName, sqlTransaction);
            this.executionContext = executionContext;
            InvoiceFileGenerator invoiceFileGenerator = null;
            if (string.IsNullOrWhiteSpace(invoiceFileGeneratorName) == false 
                && SourceEnumFromString(invoiceFileGeneratorName) == InvoiceFileGenerators.PERU)
            {
                invoiceFileGenerator = new PeruInvoiceFileGenerator(executionContext, utilities, sqlTransaction);
            }
            else
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2935));//Sorry invalid invoice file generator format
            }
            log.LogMethodExit(invoiceFileGenerator);
            return invoiceFileGenerator;
        }
        /// <summary>
        /// SourceEnumToString
        /// </summary>
        /// <param name="fileFormat"></param>
        /// <returns></returns>
        public static string SourceEnumToString(InvoiceFileGenerators fileFormat)
        {
            String returnString = "";
            switch (fileFormat)
            {
                case InvoiceFileGenerators.None:
                    returnString = "None";
                    break;
                case InvoiceFileGenerators.PERU:
                    returnString = "PERU";
                    break;
                default:
                    returnString = "";
                    break;
            }
            return returnString;
        }
        /// <summary>
        /// SourceEnumFromString
        /// </summary>
        /// <param name="fileFormat"></param>
        /// <returns></returns>
        public static InvoiceFileGenerators SourceEnumFromString(String fileFormat)
        {
            InvoiceFileGenerators returnValue = InvoiceFileGenerators.None;
            switch (fileFormat)
            {
                case "PERU":
                    returnValue = InvoiceFileGenerators.PERU;
                    break; 
                default:
                    returnValue = InvoiceFileGenerators.None;
                    break;
            }
            return returnValue;
        }
    }
}
