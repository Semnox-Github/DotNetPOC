/********************************************************************************************
* Project Name - Reports
* Description  - RemoteReceiptReportsUseCases
* 
**************
**Version Log
**************
*Version     Date          Modified By    Remarks          
*********************************************************************************************
*2.120.0     23-Mar-2021   Mushahid Faizan        Created : Web Inventory UI Redesign.
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Semnox.Parafait.Reports
{
    public class RemoteReceiptReportsUseCases : RemoteUseCases, IReceiptReportsUseCase
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private string PRINT_RECEIVE_URL = "api/Inventory/Receive/{receiptId}/Print";

        public RemoteReceiptReportsUseCases(ExecutionContext executionContext)

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
            {
                try
                {
                    List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
                    searchParameterList.Add(new KeyValuePair<string, string>("reportKey".ToString(), reportKey.ToString()));
                    searchParameterList.Add(new KeyValuePair<string, string>("timeStamp".ToString(), timeStamp.ToString()));
                    searchParameterList.Add(new KeyValuePair<string, string>("fromDate".ToString(), fromDate.ToString()));
                    searchParameterList.Add(new KeyValuePair<string, string>("toDate".ToString(), toDate.ToString()));
                    searchParameterList.Add(new KeyValuePair<string, string>("outputFormat".ToString(), outputFormat.ToString()));

                    if (backgroundParameters != null)
                    {
                        foreach (clsReportParameters.SelectedParameterValue selectedParameterValue in backgroundParameters)
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("selectedParameterValue".ToString(), selectedParameterValue.ToString()));
                        }
                    }
                    MemoryStream responseString = await Get<MemoryStream>(PRINT_RECEIVE_URL, searchParameterList);
                    log.LogMethodExit(responseString);
                    return responseString;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    throw ex;
                }

            }

        }
    }
}
