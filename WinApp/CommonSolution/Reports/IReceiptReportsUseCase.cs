/********************************************************************************************
* Project Name - Reports
* Description  - IReceiptReportsUseCase
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

namespace Semnox.Parafait.Reports
{
    public interface IReceiptReportsUseCase
    {
        Task<MemoryStream> PrintReceives(string reportKey, string timeStamp, DateTime? fromDate,
                              DateTime? toDate, List<clsReportParameters.SelectedParameterValue> backgroundParameters,
                              string outputFormat);
    }
}
