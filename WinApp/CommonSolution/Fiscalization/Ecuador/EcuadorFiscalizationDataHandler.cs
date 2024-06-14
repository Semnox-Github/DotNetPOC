/********************************************************************************************
* Project Name - Fiscalization
* Description  - Class for EcuadorFiscalizationDataHandler 
* 
**************
**Version Log
**************
*Version     Date              Modified By        Remarks          
*********************************************************************************************
*2.155.1       13-Aug-2023       Guru S A         Created for chile fiscalization
********************************************************************************************/ 
using System.Data.SqlClient;
using System.Linq;
using System.Text; 

namespace Semnox.Parafait.Fiscalization
{ 
    internal class EcuadorFiscalizationDataHandler: FiscalizationDataHandler
    {
        private Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType); 
        private const string SELECT_QRY = @"SELECT 'ECUADOR' as fiscalization, th.TrxId as transactionId, th.trx_no as transactionNumber,
												    th.trxDate as transactionDate, ISNULL(c.FirstName,'') + ' ' +ISNULL(c.LastName, '')  as transactionCustomerName, 
												    th.pos_machine as transactionPOSMachine, 
												    (select top 1 Remarks + ': ' + Data 
													    from ExSysSynchLog ex
													    where ParafaitObject ='Transaction'
													    and ExSysName ='EcuadorFiscalization'
													    and ParafaitObjectGuid = th.Guid
													    and ex.Timestamp =(SELECT max(exin.TimeStamp) from ExSysSynchLog exin 
																		    where exin.ParafaitObject = ex.ParafaitObject
																			    and exin.ExSysName = ex.ExSysName 
																			    and exin.ParafaitObjectGuid = th.Guid								  
																			    and exists (Select 1 
										  												    from ConcurrentPrograms cp, 
																								    ConcurrentRequests cr 
																						    where cp.ExecutableName in ('InvoicePostXMLProgram.exe', 
																						                            	'InvoiceJsonReprocessingProgram.exe')
																							    and cp.ProgramId = cr.ProgramId
																							    and cr.RequestId = ex.ConcurrentRequestId))) as PostErrorInfo,
												    ('') as JsonErrorInfo,
												    th.CreationDate as transactionCreationDate, th.CreatedBy as transactionCreatedBy, th.LastUpdateTime as transactionLastUpdateDate, 
												    th.LastUpdatedBy as transactionLastUpdatedBy,  
												    (SELECT top 1 ppo.OptionName from TrxPOSPrinterOverrideRules tpor , POSPrinterOverrideOptions ppo
																    where tpor.transactionId = th.TrxId
																    and tpor.POSPrinterOverrideOptionID = ppo.POSPrinterOverrideOptionId) as InvoiceOptionName,
												    CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,c.TaxCode)) AS TaxCode, 
												    CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,c.UniqueId)) AS UniqueID,
												    ( select top 1 ex.ConcurrentRequestId
													    from ExSysSynchLog ex
													    where ParafaitObject ='Transaction'
													    and ExSysName ='EcuadorFiscalization'
													    and ParafaitObjectGuid = th.Guid
													    and ex.Timestamp =(SELECT max(exin.TimeStamp) from ExSysSynchLog exin 
																		    where exin.ParafaitObject = ex.ParafaitObject
																			    and exin.ExSysName = ex.ExSysName 
																			    and exin.ConcurrentRequestId is not null
																			    and exin.ParafaitObjectGuid = th.Guid)) as LatestRequestId,
												    ( select top 1 cr.Phase
													    from ExSysSynchLog ex, ConcurrentRequests cr
													    where ParafaitObject ='Transaction'
													    and ExSysName ='EcuadorFiscalization'
													    and ParafaitObjectGuid = th.Guid
													    and cr.RequestId = ex.ConcurrentRequestId
													    and ex.Timestamp =(SELECT max(exin.TimeStamp) from ExSysSynchLog exin 
																		    where exin.ParafaitObject = ex.ParafaitObject
																			    and exin.ExSysName = ex.ExSysName 
																			    and exin.ConcurrentRequestId is not null
																			    and exin.ParafaitObjectGuid = th.Guid)) as LatestRequestPhase,
												    ( select top 1 cr.Status
													    from ExSysSynchLog ex, ConcurrentRequests cr
													    where ParafaitObject ='Transaction'
													    and ExSysName ='EcuadorFiscalization'
													    and ParafaitObjectGuid = th.Guid
													    and cr.RequestId = ex.ConcurrentRequestId
													    and ex.Timestamp =(SELECT max(exin.TimeStamp) from ExSysSynchLog exin 
																		    where exin.ParafaitObject = ex.ParafaitObject
																			    and exin.ExSysName = ex.ExSysName 
																			    and exin.ConcurrentRequestId is not null
																			    and exin.ParafaitObjectGuid = th.Guid)) as LatestRequestStatus,
                                              th.Site_id
										  from ( Select * from trx_header th 
												  where th.CreationDate > '01-Aug-2023' 
                                                    and th.status = 'CLOSED'
                                                    and th.External_System_Reference is null 
													and th.OriginalTrxID is null
												union all
												 Select * from trx_header th 
												  where th.CreationDate > '01-Aug-2023' 
                                                    and th.External_System_Reference is null 
													and th.status = 'CLOSED'
													and th.OriginalTrxID > -1 
													and exists (select 1 from trx_header thin 
																 where thin.trxId = th.OriginalTrxID 
																   and thin.External_System_Reference = th.External_System_Reference)
												) th left outer join (select c.customer_id, p.FirstName, p.LastName, p.TaxCode, p.UniqueId 
																	    from customers c, profile p 
																       where c.ProfileId = p.id ) as c on c.customer_id = th.customerId ";

        private const string SELECT_COUNT_QRY = @"SELECT  trxId as TrxId
										            from ( Select * from trx_header th 
												            where th.CreationDate > '01-Aug-2023' 
                                                              and th.External_System_Reference is null 
                                                              and th.status = 'CLOSED'
													          and th.OriginalTrxID is null
												            union all
												           Select * from trx_header th 
												            where th.CreationDate > '01-Aug-2023' 
                                                              and th.External_System_Reference is null 
                                                              and th.status = 'CLOSED'
													          and th.OriginalTrxID > -1  
												            union all
												           Select * from trx_header th 
												            where th.CreationDate > '01-Aug-2023' 
                                                              and th.External_System_Reference is not null 
                                                              and th.status = 'CLOSED'
													          and th.OriginalTrxID > -1 
													          and exists (select 1 from trx_header thin 
																           where thin.trxId = th.OriginalTrxID 
																             and ISNULL(thin.External_System_Reference,'') = ISNULL(th.External_System_Reference,''))
												         ) th";

        internal EcuadorFiscalizationDataHandler(SqlTransaction sqlTransaction = null): base(sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.selectQry = SELECT_QRY;
            log.LogMethodExit();
        }
        protected override string GetPendingTransactionQuery()
        {
            log.LogMethodEntry();
            string query = this.selectQry;
            log.LogMethodExit(query);
            return query;
        }
        protected override string GetPendingTransactionCountQuery()
        {
            log.LogMethodEntry();
            string query = SELECT_COUNT_QRY;
            log.LogMethodExit(query);
            return query;
        }   
        protected override string GetLatestSubmittedRequestqry()
        {
            log.LogMethodEntry();
            string qry = @"WITH SplitCTE AS (
			                                    SELECT CAST('<value>' + REPLACE(ParameterValue, ',', '</value><value>') + '</value>' AS XML) AS xmlvalues,
			                                            pp.ParameterValue, 
				                                        RANK () OVER (  ORDER BY ISNULL(cr.actualStarttime, ISNULL(cr.startTime, cr.RequestedTime)) DESC, cr.requestId desc ) crrank , 
	                                                    cr.* 
			                                        FROM ConcurrentPrograms cp,
				                                        ConcurrentProgramParameters cpp,
				                                        ConcurrentProgramSchedules cps,
				                                        ConcurrentRequests cr,
			                                            ProgramParameterValue pp
			                                        where cp.ExecutableName = 'InvoiceJsonReprocessingProgram.Exe'
			                                        and cp.ProgramId = cpp.ProgramId
			                                        and cpp.ParameterName = 'TransactionIdList' 
												    and cps.ProgramId = cp.ProgramId 
			                                        and cr.ProgramScheduleId = cps.ProgramScheduleId
			                                        and cr.ProgramId = cp.ProgramId
			                                        and pp.ProgramId = cpp.ProgramId 
			                                        and pp.ParameterId = cpp.ConcurrentProgramParameterId  
												    and pp.ConcurrentProgramScheduleId = cps.ProgramScheduleId 
			                                        and not EXISTS (SELECT 1 from ExSysSynchLog ex
									                                    where ParafaitObject ='Transaction'
									                                    and ExSysName ='EcuadorFiscalization'
									                                    and ex.ConcurrentRequestId = cr.RequestId)
			                                    )
			                                    select * from (
			                                    SELECT  ParameterValue,
				                                    LTRIM(RTRIM(xmlvalue.value('.', 'nvarchar(max)'))) AS SplitValue,
				                                    crrank, requestId, programId, programScheduleId, requestedTime, requestedBy,
                                                    startTime, actualStartTime, endTime, phase, status, relaunchOnExit,  argument1, argument2, argument3, argument4,
                                                    argument5,  argument6,  argument7,  argument8, argument9,  argument10,  processId,  errorCount, isActive
			                                    FROM SplitCTE
			                                    CROSS APPLY xmlvalues.nodes('/value') AS xmltable(xmlvalue) ) as t
			                                    inner join @trxIdList List on t.SplitValue = List.Value
			                                    where t.crrank = 1";
            log.LogMethodExit(qry);
            return qry;
        }
    }
}
