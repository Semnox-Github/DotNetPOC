/********************************************************************************************
* Project Name - Reports
* Description  - LocalReceiptReportsUseCases
* 
**************
**Version Log
**************
*Version     Date          Modified By    Remarks          
*********************************************************************************************
*2.120.0     23-Mar-2021   Mushahid Faizan        Created : Web Inventory UI Redesign.
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Semnox.Core.Utilities;


namespace Semnox.Parafait.Reports
{
    public class LocalReceiptReportsUseCases : IReceiptReportsUseCase
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalReceiptReportsUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<MemoryStream> PrintReceives(string reportKey, string timeStamp, DateTime? fromDate,
                              DateTime? toDate, List<clsReportParameters.SelectedParameterValue> backgroundParameters,
                              string outputFormat)
        {
            log.LogMethodEntry(reportKey, timeStamp, fromDate, toDate, backgroundParameters, outputFormat);

            return await Task<MemoryStream>.Factory.StartNew(() =>
            {
                ReceiptReports receiptReports = new ReceiptReports(executionContext, "InventoryReceiveReceipt", timeStamp, fromDate,
                                                                    toDate, backgroundParameters, outputFormat);

                var content = receiptReports.GenerateReport();

                log.LogMethodExit(content);
                return content;
            });

        }
    }
}
