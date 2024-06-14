/********************************************************************************************
 * Project Name - eJournal Load
 * Description  - Business Logic to print eJournals
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        17-May-2018      Mathew Ninan   Created 
 *2.50.0      03-Dec-2018      Mathew Ninan   Remove staticDataExchange from calls as Staticdataexchange
 *                                            is deprecated
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.POS;
using Semnox.Parafait.Printer;

namespace Semnox.Parafait.Transaction
{
    public class EJournalLoadProcess
    {
        Utilities utilities;
        ParafaitEnv ParafaitEnv;
        TransactionUtils trxUtils;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        List<LookupValuesDTO> eJournalConfigurations;
        public class EJournalDTO
        {
            Dictionary<int, int> trxDetailList;
            string message;
            string trxPrintString;
            string filePath;
            double frequency;
            int backupFrequency;
            DateTime fromDate;
            DateTime toDate;
            string processName;
            string isTrxPrintEnabled;
            bool isSuccess;
            public EJournalDTO()
            {
                trxDetailList = null;
                message = "";
                trxPrintString = "";
                filePath = "";
                frequency = 0;
                backupFrequency = 0;
                processName = "TrxJournalPrint";
                isTrxPrintEnabled = "N";
                isSuccess = false;
            }
            public Dictionary<int, int> TrxDetailsList { get { return trxDetailList; } set { trxDetailList = value; } }

            public string Message { get { return message; } set { message = value; } }

            public string FilePath { get { return filePath; } set { filePath = value; } }

            public double Frequency { get { return frequency; } set { frequency = value; } }

            public int BackupFrequency { get { return backupFrequency; } set { backupFrequency = value; } }

            public DateTime FromDate  { get { return fromDate; } set { fromDate = value; } }
            public DateTime ToDate { get { return toDate; } set { toDate = value; } }

            public string ProcessName { get { return processName; } set { processName = value; } }

            public string IsTrxPrintEnabled { get { return isTrxPrintEnabled; } set { isTrxPrintEnabled = value; } }
            public string TrxPrintString { get { return trxPrintString; } set { trxPrintString = value; } }

            public bool IsSuccess { get { return isSuccess; } set { isSuccess = value; } }
        }

        public EJournalLoadProcess(Utilities Utilities)
        {
            log.LogMethodEntry(Utilities);
            utilities = Utilities;
            trxUtils = new TransactionUtils(utilities);
            ParafaitEnv = utilities.ParafaitEnv;
            ParafaitEnv.Initialize();
            log.LogMethodExit();
        }

        public EJournalDTO PrintTransactions(EJournalDTO eJournalObj)
        {
            log.LogMethodEntry(eJournalObj);
            eJournalObj = PopulateVariables();
            if (eJournalObj.IsTrxPrintEnabled == "Y")
            {
                eJournalObj = GetLastRundate(eJournalObj); // to get last successfull run date and time
                if (utilities.getServerTime() >= eJournalObj.FromDate.AddHours(eJournalObj.Frequency)) //check system current datetime is greater than last successfull run + frequency
                {
                    eJournalObj.TrxDetailsList = new Dictionary<int, int>();
                    eJournalObj.TrxDetailsList = GetTrxIds(eJournalObj.FromDate, eJournalObj.ToDate); // to get transaction id's
                    if (eJournalObj.TrxDetailsList.Count == 0)
                    {
                        eJournalObj.Message = "No Transactions to Print";
                    }
                    else
                    {
                        try
                        {
                            StringBuilder sb = new StringBuilder();
                            string trxPrintString = string.Empty;
                            string retMessage = string.Empty;
                            foreach (KeyValuePair<int, int> entry in eJournalObj.TrxDetailsList)
                            {
                                Transaction trx = trxUtils.CreateTransactionFromDB(entry.Key, utilities);
                                POSMachineList trxPosMachineList = new POSMachineList(utilities.ExecutionContext);
                                List<POSMachineDTO> trxPosMachineDTOList = new List<POSMachineDTO>();
                                List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> searchByPOSMachineParameters = new List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>>();
                                searchByPOSMachineParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.POS_NAME, trx.POSMachine));
                                trxPosMachineDTOList = trxPosMachineList.GetAllPOSMachines(searchByPOSMachineParameters);
                                if (trxPosMachineDTOList != null)
                                    utilities.ParafaitEnv.SetPOSMachine(trxPosMachineDTOList[0].IPAddress, trxPosMachineDTOList[0].ComputerName);
                                else
                                    utilities.ParafaitEnv.SetPOSMachine("-1", trx.POSMachine);
                                trx.TransactionInfo.createTransactionInfo(entry.Key);
                                trx.Utilities.ParafaitEnv.POSMachine = trx.POSMachine;
                                POSPrinterDTO posPrinterDTO = new POSPrinterDTO(-1, -1, -1, -1, -1, -1, entry.Value, null, null, null, true, DateTime.Now, "", DateTime.Now, "", -1, "", false, -1,-1);
                                posPrinterDTO.ReceiptPrintTemplateHeaderDTO = (new ReceiptPrintTemplateHeaderBL(utilities.ExecutionContext, entry.Value, true)).ReceiptPrintTemplateHeaderDTO;
                                posPrinterDTO.PrinterDTO = new PrinterDTO();
                                posPrinterDTO.PrinterDTO.PrinterName = "";
                                PrinterBL printerBL = new PrinterBL(utilities.ExecutionContext, posPrinterDTO.PrinterDTO);
                                Printer.ReceiptClass receiptContent = POSPrint.PrintReceipt(trx, posPrinterDTO, false);
                                printerBL.PrintReceiptToText(receiptContent, ref trxPrintString);
                                eJournalObj.Message += retMessage;
                                if (!string.IsNullOrEmpty(trxPrintString))
                                {
                                    sb.AppendLine(trxPrintString);
                                    eJournalObj.IsSuccess = true;
                                }
                                else
                                {
                                    eJournalObj.IsSuccess = false;
                                    break;
                                }
                                // Check for Senior Citizen discount and print additional copy
                                if (utilities.getParafaitDefaults("ALLOW_MULTIPLE_TRX_PRINT_COPIES") == "Y")
                                {
                                    log.Debug("Multiple copies to be printed");
                                    string dupTrxPrint = string.Empty;
                                    bool isChanged = false;

                                    List<Transaction.TransactionLine> trxLines = new List<Transaction.TransactionLine>();
                                    trxLines = trx.TrxLines.FindAll(x => x.LineValid == true
                                                                    && x.TrxProfileId != -1
                                                                    && x.tax_percentage == 0
                                                                    && trx.TrxProfileVerificationRequired(x.TrxProfileId));

                                    if (trxLines.Count > 0)
                                    {
                                        for (int i = 0; i < receiptContent.TotalLines; i++)
                                        {
                                            if (receiptContent.ReceiptLines[i].TemplateSection == "HEADER")
                                            {
                                                if (receiptContent.ReceiptLines[i].Data[0] == utilities.MessageUtils.getMessage("Invoice")
                                                   || receiptContent.ReceiptLines[i].Data[0] == utilities.MessageUtils.getMessage("Customer Copy"))
                                                {
                                                    log.Debug("Receipt header changed to Accounting copy for SC/PWD discounts");
                                                    receiptContent.ReceiptLines[i].Data[0] = utilities.MessageUtils.getMessage("Accounting Copy");
                                                    isChanged = true;
                                                    break;
                                                }
                                            }
                                        }
                                        if (isChanged)
                                        {
                                            printerBL.PrintReceiptToText(receiptContent, ref dupTrxPrint);
                                            if (!string.IsNullOrEmpty(dupTrxPrint))
                                            {
                                                sb.AppendLine(dupTrxPrint);
                                                log.LogVariableState("Accounting Trx Print Text", dupTrxPrint);
                                            }
                                        }
                                    }
                                    else
                                        continue;
                                }
                            }
                            eJournalObj.TrxPrintString = sb.ToString();
                        }
                        catch (Exception ex)
                        {
                            eJournalObj.IsSuccess = false;
                            eJournalObj.Message = ex.Message;
                            log.Error("Error while getting Receipt date from printReceiptToText method, " + ex);
                        }
                    }//End of else for TrxIdList Count check
                    try
                    {
                        POSMachineList posMachineList = new POSMachineList(utilities.ExecutionContext);
                        List<POSMachineDTO> posMachineDTOList = new List<POSMachineDTO>();
                        List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> searchByPOSMachineParameters = new List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>>();
                        posMachineDTOList = posMachineList.GetAllPOSMachines(searchByPOSMachineParameters);

                        string shiftCloseQuery = @"select top 1 shift_username, shift_time , u.user_id
                                                         from shift , users u
                                                        where pos_machine = @posMachine
                                                          and shift_action = 'Close'
                                                          and u.username = shift.shift_username
                                                          and shift_time <= @shiftEndTime
                                                        order by shift_time desc";
                        string shiftOpenQuery = @"select top 1 shift_username, shift_time, u.user_id 
                                                        from shift , users u
                                                        where pos_machine = @posMachine
                                                        and shift_username = @userName
                                                        and shift_action = 'Open'
                                                        and u.username = shift.shift_username
                                                        and shift_time < @shiftCloseTime
                                                        order by shift_time desc";
                        foreach (POSMachineDTO posMachineDTO in posMachineDTOList)
                        {
                            QueryManagement queryManagement = new QueryManagement(utilities);
                            //POSX check
                            if (posMachineDTO.XReportRunTime >= eJournalObj.FromDate)
                            {
                                SqlParameter[] sqlParameterShiftClose = new SqlParameter[2];
                                sqlParameterShiftClose[0] = new SqlParameter("@posMachine", posMachineDTO.POSName);
                                sqlParameterShiftClose[1] = new SqlParameter("@shiftEndTime", posMachineDTO.XReportRunTime);

                                DataTable dtShiftClose = queryManagement.getDTResultString(shiftCloseQuery, sqlParameterShiftClose);
                                if (dtShiftClose.Rows.Count > 0)
                                {
                                    SqlParameter[] sqlParameterShiftOpen = new SqlParameter[3];
                                    sqlParameterShiftOpen[0] = new SqlParameter("@posMachine", posMachineDTO.POSName);
                                    sqlParameterShiftOpen[1] = new SqlParameter("@userName", dtShiftClose.Rows[0][0]);
                                    sqlParameterShiftOpen[2] = new SqlParameter("@shiftCloseTime", dtShiftClose.Rows[0][1]);

                                    DataTable dtShiftOpen = queryManagement.getDTResultString(shiftOpenQuery, sqlParameterShiftOpen);

                                    if (dtShiftOpen.Rows.Count > 0)
                                    {
                                        string queryString = queryManagement.getQueryString("POSX");
                                        if (!String.IsNullOrEmpty(queryString))
                                        {
                                            SqlParameter[] sqlParameterList = new SqlParameter[4];
                                            sqlParameterList[0] = new SqlParameter("@userid", dtShiftClose.Rows[0][2]);
                                            sqlParameterList[1] = new SqlParameter("@posMachine", posMachineDTO.POSName);
                                            sqlParameterList[2] = new SqlParameter("@fromdate", dtShiftOpen.Rows[0][1]);
                                            sqlParameterList[3] = new SqlParameter("@todate", dtShiftClose.Rows[0][1]);
                                            string resultString = queryManagement.getResultString(queryString, sqlParameterList);
                                            log.LogVariableState("X Report", resultString);
                                            if (!String.IsNullOrEmpty(resultString))
                                            {
                                                eJournalObj.TrxPrintString += Environment.NewLine + resultString;
                                                eJournalObj.IsSuccess = true;
                                            }
                                            else
                                                eJournalObj.IsSuccess = false;
                                        }
                                        else
                                            log.Error("POSX query not found from reports. Check set up and POSX report key.");
                                    }
                                    else
                                    {
                                        log.LogVariableState("Shift Close Time", dtShiftClose.Rows[0][1]);
                                        log.Debug("Shift user and Shift Open time not found for POS: " + posMachineDTO.POSName + " and user: " + dtShiftClose.Rows[0][0]);
                                    }
                                }
                                else
                                    log.Debug("Shift user and Shift Close time not found for POS: " + posMachineDTO.POSName);
                            }
                            //POSZ check
                            if (posMachineDTO.DayEndTime >= eJournalObj.FromDate)
                            {
                                SqlParameter[] sqlParameterShiftClose = new SqlParameter[2];
                                sqlParameterShiftClose[0] = new SqlParameter("@posMachine", posMachineDTO.POSName);
                                sqlParameterShiftClose[1] = new SqlParameter("@shiftEndTime", posMachineDTO.DayEndTime);

                                DataTable dtShiftClose = queryManagement.getDTResultString(shiftCloseQuery, sqlParameterShiftClose);
                                if (dtShiftClose.Rows.Count > 0)
                                {
                                    string queryString = queryManagement.getQueryString("POSZ");
                                    if (!String.IsNullOrEmpty(queryString))
                                    {
                                        SqlParameter[] sqlParameterList = new SqlParameter[4];
                                        sqlParameterList[0] = new SqlParameter("@userid", dtShiftClose.Rows[0][2]);
                                        sqlParameterList[1] = new SqlParameter("@posMachine", posMachineDTO.POSName);
                                        sqlParameterList[2] = new SqlParameter("@fromdate", posMachineDTO.DayBeginTime);
                                        sqlParameterList[3] = new SqlParameter("@todate", posMachineDTO.DayEndTime);
                                        string resultString = queryManagement.getResultString(queryString, sqlParameterList);
                                        log.LogVariableState("Z Report", resultString);

                                        if (!String.IsNullOrEmpty(resultString))
                                        {
                                            eJournalObj.TrxPrintString += Environment.NewLine + resultString;
                                            eJournalObj.IsSuccess = true;
                                        }
                                        else
                                            eJournalObj.IsSuccess = false;
                                    }
                                    else
                                        log.Error("POSZ query not found from reports. Check set up and POSZ report key.");
                                }
                                else
                                    log.Debug("Shift user and Shift Close time not found for POS: " + posMachineDTO.POSName);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        eJournalObj.IsSuccess = false;
                        eJournalObj.Message = ex.Message;
                        log.Error("Error while getting Receipt date from printReceiptToText method, " + ex);
                    }
                }
                else
                {
                    eJournalObj.IsSuccess = false;
                    eJournalObj.Message = "E-Journal Transaction Print, next run start at :" + eJournalObj.FromDate.AddHours(eJournalObj.Frequency);
                }
                if (eJournalObj.IsSuccess && !string.IsNullOrEmpty(eJournalObj.TrxPrintString))
                {
                    bool writeSuccess = WriteToTextFile(eJournalObj);

                    if (writeSuccess)//write into ExsysLog
                        UpdateExSysSynchLog(eJournalObj.ProcessName, eJournalObj.FilePath, true, "Success", DateTime.Now, "E Journal Transaction Print successfull");
                    else
                        UpdateExSysSynchLog(eJournalObj.ProcessName, eJournalObj.FilePath, false, "Failed", DateTime.Now, "Error while Creating/backup file. trxPrintString: " + eJournalObj.TrxPrintString);
                }
                else
                {
                    UpdateExSysSynchLog(eJournalObj.ProcessName, eJournalObj.FilePath, false, "Failed", DateTime.Now, "Error while printing data to Text file. trxPrintString: " + eJournalObj.TrxPrintString);
                }
            }
            log.LogMethodExit(eJournalObj);
            return eJournalObj;
        }

        private EJournalDTO PopulateVariables()
        {
            log.LogMethodEntry();
            EJournalDTO eJournalDto = new EJournalDTO();
            try
            {                
                LookupValuesList lookupValuesList = new LookupValuesList(utilities.ExecutionContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "E_JOURNAL_CONFIGURATIONS"));
                eJournalConfigurations = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);

                //get file storage path
                LookupValuesDTO trxJounalDTOForFilePath = new LookupValuesDTO();
                trxJounalDTOForFilePath = eJournalConfigurations.Find(x => x.LookupValue.Trim() == "FILE_STORAGE_PATH");
                if (trxJounalDTOForFilePath != null)
                    eJournalDto.FilePath = trxJounalDTOForFilePath.Description.Trim();

                //get template Id
                //printTemplateId = Convert.ToInt32(utilities.getParafaitDefaults("RECEIPT_PRINT_TEMPLATE"));

                //load variable value to check print enabled
                LookupValuesDTO trxJournalDTOForCheckPrintEnabled = new LookupValuesDTO();
                trxJournalDTOForCheckPrintEnabled = eJournalConfigurations.Find(x => x.LookupValue.Trim() == "ENABLE_EJOURNAL_PRINT");
                if (trxJournalDTOForCheckPrintEnabled != null)
                    eJournalDto.IsTrxPrintEnabled = trxJournalDTOForCheckPrintEnabled.Description.Trim();

                //load frequency value
                LookupValuesDTO trxJournalDtoForFrequency= new LookupValuesDTO();
                trxJournalDtoForFrequency = eJournalConfigurations.Find(x => x.LookupValue.Trim() == "EJOURNAL_REPORT_FREQUENCY_IN_HRS");
                if (trxJournalDtoForFrequency != null)
                    eJournalDto.Frequency = Convert.ToDouble(trxJournalDtoForFrequency.Description.Trim());

                //load backup frequency value
                LookupValuesDTO trxJournalDtoForBackupFrequency = new LookupValuesDTO();
                trxJournalDtoForBackupFrequency = eJournalConfigurations.Find(x => x.LookupValue.Trim() == "FILE_BACKUP_FREQUENCY_IN_DAYS");
                if (trxJournalDtoForBackupFrequency != null)
                    eJournalDto.BackupFrequency = Convert.ToInt32(trxJournalDtoForBackupFrequency.Description.Trim());

                eJournalDto.ToDate = utilities.getServerTime();
            }
            catch (Exception ex)
            {
                log.Error("Error while populating E Journal Public Varibles, ",ex);
                eJournalDto.Message = ex.Message;
            }
            log.LogMethodExit();
            return eJournalDto;
        }

        //To get last run date and time
        private EJournalDTO GetLastRundate(EJournalDTO eJournalDtoObj)
        {
            log.LogMethodEntry(eJournalDtoObj);
            EJournalDTO eJournal = eJournalDtoObj;
            Dictionary<int, int> dicObj = new Dictionary<int, int>();
            string selectRunDateQuery = @"SELECT max(timestamp)
                                            FROM ExSysSynchLog es
                                            WHERE es.ExSysName = @exSysName 
                                                AND es.IsSuccessFul = 1";

            string selectMinDateQuery = @"SELECT min(TrxDate) 
                                            FROM trx_header where Status='CLOSED'";
            List<SqlParameter> paramsToGetRunDate = new List<SqlParameter>();

            paramsToGetRunDate.Add(new SqlParameter("@exSysName", eJournalDtoObj.ProcessName));
            try
            {
                object dtTimStamp = utilities.executeScalar(selectRunDateQuery, paramsToGetRunDate.ToArray());

                if (!DBNull.Value.Equals(dtTimStamp))
                {
                    eJournal.FromDate = Convert.ToDateTime(dtTimStamp);
                }
                else
                {
                    dtTimStamp = utilities.executeScalar(selectMinDateQuery);
                    if (!DBNull.Value.Equals(dtTimStamp))
                    {
                        eJournal.FromDate = Convert.ToDateTime(dtTimStamp);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("ends with error GetLastRundate(eJournalDtoObj) Method." + ex);
            }
            log.LogMethodExit(eJournalDtoObj);
            return eJournal;
        }

        private Dictionary<int, int> GetTrxIds(DateTime fromDate, DateTime toDate)
        {
            log.LogMethodEntry(fromDate, toDate);
            Dictionary<int, int> trxIdList = new Dictionary<int, int>();
            try
            {                
                DataTable dt = utilities.executeDataTable(@"select h.trxid , ISNULL(pp.PrintTemplateId, (SELECT TOP 1 DEFAULT_VALUE
												                                                        FROM parafait_defaults
												                                                        WHERE DEFAULT_VALUE_NAME = 'RECEIPT_PRINT_TEMPLATE')) TemplateId
                                                            FROM TRX_HEADER H, (select pp.POSMachineId , pp.PrintTemplateId,
					                                                            DENSE_RANK() over (partition by pp.posmachineId order by pp.printTemplateId asc) rnk
					                                                            from POSPrinters pp, LookupView lv
					                                                            where pp.PrinterTypeId = lv.LookupValueId
					                                                            and lv.LookupName = 'PRINTER_TYPE'
					                                                            and lv.LookupValue = 'ReceiptPrinter') pp
                                                             where h.POSMachineId = pp.POSMachineId and pp.rnk = 1
                                                             and h.status = 'CLOSED'
                                                             and h.trxdate between @fromDate and @toDate
                                                             order by h.trxId",
                                                                new SqlParameter("@fromDate", fromDate),
                                                                new SqlParameter("@toDate", toDate));
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (!DBNull.Value.Equals(row["trxId"]) && !DBNull.Value.Equals(row["TemplateId"]))
                        {
                            if(!trxIdList.ContainsKey(Convert.ToInt32(row["trxId"])))
                            {
                                trxIdList.Add(Convert.ToInt32(row["trxId"]), Convert.ToInt32(row["TemplateId"]));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error while getting Transaction Id's, ",ex);
            }
            log.LogMethodExit();
            return trxIdList;
        }

        private bool WriteToTextFile(EJournalDTO eJournalDataObj)
        {
            log.LogMethodEntry(eJournalDataObj);
            bool reStatus = false;
            try
            {
                //Backup files after certain days
                if(System.IO.File.Exists(eJournalDataObj.FilePath))
                {
                    DateTime lastModifiedDate = System.IO.File.GetCreationTime(eJournalDataObj.FilePath);
                    if(utilities.getServerTime() >= lastModifiedDate.AddDays(eJournalDataObj.BackupFrequency))
                    {
                        string backupFileName = Path.ChangeExtension(eJournalDataObj.FilePath, null) + "_Backup_" + utilities.getServerTime().ToString("yyyy-MM-dd-HH-mm-ss") + Path.GetExtension(eJournalDataObj.FilePath);
                        System.IO.File.Move(eJournalDataObj.FilePath, backupFileName);
                        reStatus = true;
                    }
                }
                if (!string.IsNullOrEmpty(eJournalDataObj.FilePath) && !string.IsNullOrEmpty(eJournalDataObj.TrxPrintString))
                {
                    // Creat file if not exist or Append text if already exist
                    System.IO.File.AppendAllText(eJournalDataObj.FilePath, eJournalDataObj.TrxPrintString);
                    reStatus = true;
                }
            }
            catch (Exception ex)
            {
                reStatus = false;
                log.Error("Error while writing transaction Journal into text File,", ex);
            }
            log.LogMethodExit();
            return reStatus;
        }

        private void UpdateExSysSynchLog(string processName, object ParafaitObject, bool success, object status, object last_updated_date, object remarks)
        {
            log.LogMethodEntry(processName, ParafaitObject, success, status, last_updated_date, remarks);
            try
            {
                utilities.executeNonQuery(@"insert into ExSysSynchLog
                                                    (TimeStamp, ExSysName,
                                                    ParafaitObject, 
                                                    ParafaitObjectGuid, IsSuccessFul,
                                                    Status, 
                                                    Remarks)
                                               values 
                                                    (getdate(), @exSysName,
                                                    @ParafaitObject,
                                                     NEWID(), @success,
                                                    @status, 
                                                    substring(@remarks, 1, 500))",
                                             new SqlParameter("@ParafaitObject", Convert.ToString(ParafaitObject)),
                                             new SqlParameter("@success", success),
                                             new SqlParameter("@status", status),
                                             new SqlParameter("@remarks", remarks),
                                             new SqlParameter("@exSysName", processName));
            }
            catch (Exception ex)
            {
                log.Error("Error in UpdateExSysSynchLog while inserting success or failure status into log, "+ ex);
            }
            log.LogMethodExit();
        }
    }
}
