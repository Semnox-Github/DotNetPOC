﻿/********************************************************************************************
* Project Name - Fiscalization
* Description  - Class for EcuadorFiscalization 
* 
**************
**Version Log
**************
*Version        Date             Modified By        Remarks          
*********************************************************************************************
*2.155.1       13-Aug-2023       Guru S A           Created for Chile fiscalizations 
********************************************************************************************/
using System;
using System.Linq;
using System.Data.SqlClient; 
using System.Collections.Generic; 
using Semnox.Core.Utilities; 

namespace Semnox.Parafait.Fiscalization
{
    /// <summary>
    /// EcuadorFiscalizationList
    /// </summary>
    public class EcuadorFiscalizationList: ParafaitFiscalizationList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// EcuadorFiscalizationList
        /// </summary> 
        public EcuadorFiscalizationList(ExecutionContext executionContext, Utilities utilities): base(executionContext, utilities)
        {
            log.LogMethodEntry(executionContext, "utilities"); 
            log.LogMethodExit();
        } 
        /// <summary>
        /// GetPendingTransactions
        /// </summary>
        public override List<FiscalizationPendingTransactionDTO> GetPendingTransactions(List<KeyValuePair<FiscalizationPendingTransactionDTO.SearchParameters, string>> searchParams, 
            int pageNumber = 0, int pageSize = 50, SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(searchParams, pageNumber, pageSize, sqlTrx);
            FiscalizationDataHandler fiscalizationDataHandler = new EcuadorFiscalizationDataHandler(sqlTrx);
            string passPhrase = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_ENCRYPTION_PASS_PHRASE");
            List<FiscalizationPendingTransactionDTO>  dtoList = fiscalizationDataHandler.GetPendingTransactions(searchParams, passPhrase, pageNumber, pageSize);
            log.LogMethodExit();
            return dtoList;
        }
        /// <summary>
        /// GetPendingTransactionCount
        /// </summary>
        public override int GetPendingTransactionCount(List<KeyValuePair<FiscalizationPendingTransactionDTO.SearchParameters, string>> searchParams,
            SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(searchParams, sqlTrx);
            FiscalizationDataHandler fiscalizationDataHandler = new EcuadorFiscalizationDataHandler(sqlTrx);
            int trxCount = fiscalizationDataHandler.GetPendingTransactionCount(searchParams);
            log.LogMethodExit(trxCount);
            return trxCount;
        }    
    }
}