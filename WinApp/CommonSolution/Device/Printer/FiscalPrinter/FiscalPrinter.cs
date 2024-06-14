/********************************************************************************************
 * Project Name - Device                                                                      
 * Description  - Based class for fiscalization
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By         Remarks          
 *********************************************************************************************
 **             27-Jul-2017   Archana Kulal       Created
 *2.90.0        14-Jul-2020   Gururaja Kanjan     Updated for processing failed entries of fiskaltrust integration
 *2.90.0        18-Aug-2020   Laster Menezes      Added new method PrintMultipleReceipt
 *2.140.0       08-Feb-2022	  Girish Kundar       Modified: Smartro Fiscalization
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.Printer.FiscalPrint
{
    public class FiscalPrinter
    {
        private static readonly Semnox.Parafait.logging.Logger log = 
            new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parafait utilities.
        /// </summary>
        protected Utilities utilities;

        public FiscalPrinter(Utilities _utilities)
        {
            utilities = _utilities;
        }

        /// <summary>
        /// Opens the Port for the Printer.
        /// </summary>
        /// <returns></returns>
        public virtual bool OpenPort()
        {
            log.LogMethodEntry();
            log.LogMethodExit(false);
            return false;
        }

        /// <summary>
        /// Method to print receipt
        /// </summary>
        /// <param name="TrxId"></param>
        /// <param name="tenderedCash"></param>
        /// <param name="SQLTrx"></param>
        /// <param name="isFiscal"></param>
        /// <param name="trxReprint"></param>
        /// <returns></returns>
        public virtual bool PrintReceipt(int TrxId, ref string Message, SqlTransaction SQLTrx = null, 
            decimal tenderedCash = 0,  bool isFiscal = true, bool trxReprint = false)
        {
            log.LogMethodEntry();
            log.LogMethodExit(false);
            return false;
        }

        /// <summary>
        /// Method to print receipt
        /// </summary>
        /// <param name="FiscalizationRequest"></param>
        /// <returns></returns>
        public virtual bool PrintReceipt(FiscalizationRequest receiptRequest, ref string Message)
        {
            log.LogMethodEntry();
            log.LogMethodExit(false);
            return false;
        }

        /// <summary>
        /// Print Report
        /// </summary>
        /// <param name="Report"></param>
        public virtual void PrintReport(string Report, ref string Message)
        {
            log.LogMethodEntry(Report);
            log.LogMethodExit(null);
        }
        /// <summary>
        /// check the Fiscal printer status
        /// </summary>
        /// <returns></returns>
        public virtual bool CheckPrinterStatus(StringBuilder errorMessage)
        {
            return true;
        }
        /// <summary>
        /// Print Monthly Report
        /// </summary>
        /// <param name="Message"></param>
        public virtual void PrintMonthlyReport(DateTime fromDate, DateTime toDate,
            char reportType, ref string Message)
        {
            log.LogMethodEntry(Message);
            log.LogMethodExit(null);
        }


        /// <summary>
        /// Closes the Port of the Printer.
        /// </summary>
        /// <returns></returns>
        public virtual void ClosePort()
        {
            log.LogMethodEntry();
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Shift Details Print
        /// </summary>
        /// <returns></returns>
        public virtual bool DepositeInDrawer(double amount, double systemAmount = 0, bool cashout = false)
        {
            log.LogMethodEntry();
            log.LogMethodExit(false);
            return false;
        }

        /// <summary>
        /// Change Tax Values
        /// </summary>
        /// <returns></returns>
        public virtual bool ChangeTaxValues(ref string Message, List<string> taxValuesList)
        {
            log.LogMethodEntry();
            log.LogMethodExit(false);
            return false;
        }

        /// <summary>
        /// Change Tax Values
        /// </summary>
        /// <returns></returns>
        public virtual List<string> InitializeTax()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
            return null;
        }

        /// <summary>
        /// Send Invoice For Fiscalization
        /// </summary>
        /// <param name="trxId"></param>
        /// <returns></returns>
        public virtual string SendInvoiceForFiscalization(FiscalizationRequest receiptRequest)
        {
            log.LogMethodEntry();
            log.LogMethodExit();
            return null;
        }


        public virtual string GetFiscalReference(string trxId)
        {
            log.LogMethodEntry(trxId);
            log.LogMethodExit(null);
            return null;
        } 
        protected void UpdateUSN(SqlCommand sqlCommand, int trxId, string reference)
        {
            log.LogMethodEntry(reference, trxId);
            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.CommandText = 
                    @"update trx_header set External_System_Reference = @externalSystemReference where TrxId = @trxId";
                sqlCommand.Parameters.AddWithValue("@externalSystemReference", reference);
                sqlCommand.Parameters.AddWithValue("@trxId", trxId);
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                log.Error("Error in updating receipt no", ex);
            }
            log.LogMethodExit(null);
        }

        /// <summary>
        /// PrintMultipleReceipt
        /// </summary>
        /// <param name="TrxId"></param>
        /// <returns></returns>
        public virtual bool PrintMultipleReceipt(List<int> TrxId)
        {
            log.LogMethodEntry();
            log.LogMethodExit(false);
            return false;
        }
        /// <summary>
        /// Method to check Pop up confirmation required
        /// </summary>
        /// <param name="IsConfirmationRequired"></param>
        /// <returns></returns>
        public virtual bool IsConfirmationRequired(FiscalizationRequest receiptRequest)
        {
            log.LogMethodEntry();
            log.LogMethodExit(false);
            return false;
        }
    }
}
