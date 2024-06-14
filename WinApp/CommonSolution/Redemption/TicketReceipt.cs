/********************************************************************************************
 * Project Name - TicketReceipt
 * Description  - TicketReceipt class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.3.0       05-Jun-2018      Archana/Guru S A     Modified for Redemption Kiosk
 *2.70.2        19-Jul-2019      Deeksha              Modifications as per three tier standard.
 *2.70.2        05-Oct-2019      Girish Kundar        Modified : Ticket Station enhancement.
 *2.110.0     17-Nov-2020      Vikas Dwivedi        Modified : As per 3-Tier Standard Checklist
 *2.130.8     08-Jun-2022      Abhishek             Modified : Added GetTicketReceiptValue() to return the ticket value
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Site;

namespace Semnox.Parafait.Redemption
{
    /// <summary>
    /// Business logic for ticket receipt
    /// </summary>
    public class TicketReceipt
    {
        private string ticketReceiptNo;
        private TicketReceiptDTO ticketReceiptDTO;
        private readonly Semnox.Core.Utilities.ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor
        /// </summary>
        public TicketReceipt(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="ticketReceiptDTO">TicketReceiptDTO</param>
        public TicketReceipt(ExecutionContext executionContext, TicketReceiptDTO ticketReceiptDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, ticketReceiptDTO);
            this.ticketReceiptDTO = ticketReceiptDTO;
            ticketReceiptNo = ticketReceiptDTO.ManualTicketReceiptNo;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the id parameter
        /// </summary>
        /// <param name="receiptId"> id of the receipt</param>
        /// <param name="sqlTrxn"> sqlTrxn</param>
        public TicketReceipt(ExecutionContext executionContext, int receiptId, SqlTransaction sqlTrxn = null)
               : this(executionContext)
        {
            log.LogMethodEntry(executionContext, receiptId, sqlTrxn);
            TicketReceiptDataHandler ticketReceiptDataHandler = new TicketReceiptDataHandler(sqlTrxn);
            ticketReceiptDTO = ticketReceiptDataHandler.GetTicketReceipt(receiptId, null);
            if (ticketReceiptDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "TicketReceiptDTO", receiptId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            ticketReceiptNo = ticketReceiptDTO.ManualTicketReceiptNo;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the receipt number parameter
        /// </summary>
        /// <param name="receiptNumber">Receipt number</param>
        /// <param name="sqlTrxn"> sqlTrxn</param>
        public TicketReceipt(ExecutionContext executionContext ,string receiptNumber, SqlTransaction sqlTrxn = null)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext,receiptNumber, sqlTrxn);
            TicketReceiptDataHandler ticketReceiptDataHandler = new TicketReceiptDataHandler(sqlTrxn);
            ticketReceiptDTO = ticketReceiptDataHandler.GetTicketReceipt(receiptNumber, null);
            ticketReceiptNo = receiptNumber;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with 2 parameters
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="totalTickets">totalTickets</param>
        /// <param name="sourceRedemptionId">sourceRedemptionId</param>
        public TicketReceipt(ExecutionContext executionContext, int totalTickets, int sourceRedemptionId) : this(executionContext)
        {
            log.LogMethodEntry(executionContext,totalTickets, sourceRedemptionId);
            string newBarCode = CreateBarCode(totalTickets);
            this.ticketReceiptDTO = new TicketReceiptDTO(-1, -1, newBarCode, executionContext.GetSiteId(), "", false, -1, totalTickets,
                                                         totalTickets, executionContext.GetUserId(), DateTime.Now, false, sourceRedemptionId, DateTime.Now, executionContext.GetUserId(), DateTime.Now);
            ticketReceiptNo = newBarCode;
            log.LogMethodExit();
        }
        /// <summary>
        /// Saves the ticket receipt
        /// ticket receipt will be inserted if ReceiptId is less than or equal to
        /// zero else updates the records based on primary key
        /// <param name="sqlTrxn">SqlTransaction</param>
        /// </summary>
        public void Save(SqlTransaction sqlTrxn)
        {
            log.LogMethodEntry(sqlTrxn);
            if (ticketReceiptDTO.IsChanged == false)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            List<ValidationError> validationErrorList = Validate(sqlTrxn);
            if (validationErrorList.Count > 0)
            {
                throw new ValidationException("Validation failed", validationErrorList);
            }
            TicketReceiptDataHandler ticketReceiptDataHandler = new TicketReceiptDataHandler(sqlTrxn);
            if (ticketReceiptDTO.Id < 0)
            {
                log.LogVariableState("TicketReceiptDTO", ticketReceiptDTO);
                ticketReceiptDTO = ticketReceiptDataHandler.InsertTicketReceipt(ticketReceiptDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                ticketReceiptDTO.AcceptChanges();
            }
            else if (ticketReceiptDTO.IsChanged)
            {
                log.LogVariableState("TicketReceiptDTO", ticketReceiptDTO);
                ticketReceiptDTO = ticketReceiptDataHandler.UpdateTicketReceipt(ticketReceiptDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                ticketReceiptDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the TicketReceiptDTO
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            ValidationError validationError = null;
            return validationErrorList;
            // Validation Logic here 
        }

        /// <summary>
        /// To check the passed receipt is used or not
        /// </summary>
        /// <param name="sqlTrx">Sql Trx</param>
        /// <returns> returns true if used else false</returns>
        public bool IsUsedTicketReceipt(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            bool retVal = false;
            if (ticketReceiptDTO != null)
            {
                TicketReceiptList ticketReceiptList = new TicketReceiptList(executionContext);
                List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>> searchParam = new List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>>();
                searchParam.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.MANUAL_TICKET_RECEIPT_NO, this.ticketReceiptNo));
                searchParam.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                List<TicketReceiptDTO> ticketReceiptDTOList = ticketReceiptList.GetAllTicketReceipt(searchParam, sqlTrx);
                if (ticketReceiptDTOList != null && ticketReceiptDTOList.Count > 0)
                {
                    if (ticketReceiptDTOList[0].BalanceTickets == 0)
                    {
                        retVal = true;
                    }
                }
            }
            log.LogMethodExit(retVal);
            return retVal;
        }

        /// <summary>
        /// To check the passed receipt is flagged or not
        /// </summary>
        /// <returns> returns true if flagged else false</returns>
        public bool IsFlaggedTicketReceipt()
        {
            log.LogMethodEntry();
            if (ticketReceiptDTO == null || (ticketReceiptDTO != null && ticketReceiptDTO.Id >= 0 && !ticketReceiptDTO.IsSuspected))
            {
                log.LogMethodExit(false);
                return false;
            }
            log.LogMethodExit(true);
            return true;
        }

        /// <summary>
        /// Creates barcode
        /// Internal note: This method need to be moved to some static class, potentially ticket receipt master
        /// </summary>
        /// <param name="totalTickets">Total tickets</param>
        /// <returns>Barcode</returns>
        string CreateBarCode(int totalTickets)
        {
            log.LogMethodEntry(totalTickets);
            string newBarCode = string.Empty;
            TicketStationFactory ticketStationFactory = new TicketStationFactory();
            POSCounterTicketStationBL posCounterTicketStationBL = null;
            try
            {
                if (totalTickets > 0)
                {
                    posCounterTicketStationBL = ticketStationFactory.GetPosCounterTicketStationObject();
                    if (posCounterTicketStationBL == null)
                    {
                        string message = MessageContainerList.GetMessage(executionContext, 2322);
                        throw new Exception(message);
                    }
                    else
                    {
                        newBarCode = posCounterTicketStationBL.GenerateBarCode(totalTickets);
                    }
                }
                else
                {
                    log.Error(MessageContainerList.GetMessage(executionContext, 1623));
                    throw new Exception(MessageContainerList.GetMessage(executionContext, 1623));
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(newBarCode);
                return newBarCode;
            }
            log.LogMethodExit(newBarCode);
            return newBarCode;
        }

        /// <summary>
        /// Returns true if the ticket belongs to ticket station of the current site.
        /// Internal note: Should go to ticket station or some related class
        /// </summary>
        /// <param name="barCode">Barcode of the ticket</param>
        /// <param name="sqlTrx">Sql Trx</param>
        public static bool IsTicketFromCurrentSite(string barCode, SqlTransaction sqlTrxn = null)
        {
            log.LogMethodEntry(barCode, sqlTrxn);
            bool valid = false;
            try
            {
                TicketStationFactory ticketStationFactory = new TicketStationFactory();
                TicketStationBL ticketStationBL = ticketStationFactory.GetTicketStationObject(barCode);
                if (ticketStationBL == null)
                {
                    valid = false;
                }
                else
                {
                    if (ticketStationBL.BelongsToThisStation(barCode) && ticketStationBL.ValidCheckBit(barCode))
                    {
                        valid = true;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                valid = false;
            }
            log.LogMethodExit(valid);
            return valid;
        }

        public bool ValidateTicketReceipts(string receiptNumber, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            bool valid = true;
            if (IsTicketFromCurrentSite(receiptNumber) == false)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2321), MessageContainerList.GetMessage(executionContext,"Validation"), MessageContainerList.GetMessage(executionContext,"TicketFromCurrentSite"), MessageContainerList.GetMessage(executionContext, 2321)); //Invalid ticket receipt, unable to find the matching ticket station
            }
            if (IsFlaggedTicketReceipt())
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Ticket receipt is flagged"), MessageContainerList.GetMessage(executionContext , "Validation"), MessageContainerList.GetMessage(executionContext, "FlaggedTicketReceipt"), MessageContainerList.GetMessage(executionContext, "Ticket receipt is flagged"));
            }
            if (IsUsedTicketReceipt(sqlTransaction))
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 112), MessageContainerList.GetMessage(executionContext , "Validation"), MessageContainerList.GetMessage(executionContext ,"UsedTicketReceipt"), MessageContainerList.GetMessage(executionContext, 112)); //Receipt already used
            }
            log.LogMethodExit(valid);
            return valid;
        }

        /// <summary>
        /// Returns ticket value if the ticket belongs to ticket station of the current site.
        /// </summary>
        /// <param name="receiptNumber">receiptNumber of the ticket</param>
        /// <param name="sqlTrx">Sql Trx</param>
        public int GetTicketReceiptValue(string receiptNumber, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(receiptNumber, sqlTransaction);
            int ticketValue = -1;
            if (IsTicketFromCurrentSite(receiptNumber) == false)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2321), MessageContainerList.GetMessage(executionContext, "Validation"), MessageContainerList.GetMessage(executionContext, "TicketFromCurrentSite"), MessageContainerList.GetMessage(executionContext, 2321)); //Invalid ticket receipt, unable to find the matching ticket station
            }
            if (IsFlaggedTicketReceipt())
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Ticket receipt is flagged"), MessageContainerList.GetMessage(executionContext, "Validation"), MessageContainerList.GetMessage(executionContext, "FlaggedTicketReceipt"), MessageContainerList.GetMessage(executionContext, "Ticket receipt is flagged"));
            }
            if (IsUsedTicketReceipt(sqlTransaction))
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 112), MessageContainerList.GetMessage(executionContext, "Validation"), MessageContainerList.GetMessage(executionContext, "UsedTicketReceipt"), MessageContainerList.GetMessage(executionContext, 112)); //Receipt already used
            }
            TicketStationFactory ticketStationFactory = new TicketStationFactory();
            TicketStationBL ticketStationBL = ticketStationFactory.GetTicketStationObject(receiptNumber);
            if (ticketStationBL.TicketStationDTO != null)
            {
                ticketValue = ticketStationBL.GetTicketValue(receiptNumber);
            }
            log.LogMethodExit(ticketValue);
            return ticketValue;
        }

        /// <summary>
        /// Delete the Redemption Ticket record - Hard Deletion
        /// </summary>
        /// <param name="redemptionGiftsId"></param>
        /// <param name="sqlTransaction"></param>
        public void Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            try
            {
                TicketReceiptDataHandler ticketReceiptDataHandler = new TicketReceiptDataHandler(sqlTransaction);
                ticketReceiptDataHandler.Delete(ticketReceiptDTO.Id);
                log.LogMethodExit();
            }
            catch (ValidationException valEx)
            {
                log.Error(valEx);
                log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                throw;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Create new manual ticket receipt
        /// </summary>
        /// <param name="totalTickets">totalTickets</param>
        /// <param name="sourceRedemptionId">sourceRedemptionId</param>
        /// <param name="sqlTrx">Sql Trx</param>
        public void CreateManualTicketReceipt(int totalTickets, int sourceRedemptionId, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(totalTickets, sourceRedemptionId, sqlTrx);
            string newBarCode = CreateBarCode(totalTickets);
            this.ticketReceiptDTO = new TicketReceiptDTO(-1, -1, newBarCode, executionContext.GetSiteId(), "", false, -1, totalTickets,
                                                         totalTickets, executionContext.GetUserId(), DateTime.Now, false, sourceRedemptionId, DateTime.Now, executionContext.GetUserId(), DateTime.Now);
            Save(sqlTrx);
            log.LogMethodExit();
        }

        /// <summary>
        /// get the Ticket receipt DTO
        /// </summary>
        public TicketReceiptDTO TicketReceiptDTO { get { return ticketReceiptDTO; } }
    }
    /// <summary>
    /// Manages the list of ticket receipt
    /// </summary>
    public class TicketReceiptList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<TicketReceiptDTO> ticketReceiptDTOList = new List<TicketReceiptDTO>();

        /// <summary>
        /// Parameterized constructor for TicketReceiptList
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public TicketReceiptList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor for TicketReceiptList
        /// </summary>
        /// <param name="executionContext">executionContext object passed as a parameter</param>
        /// <param name="ticketReceiptDTOList">ticketReceiptDTOList passed as a parameter</param>
        public TicketReceiptList(ExecutionContext executionContext, List<TicketReceiptDTO> ticketReceiptDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, ticketReceiptDTOList);
            this.ticketReceiptDTOList = ticketReceiptDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the TicketReceiptDTO
        /// </summary>
        /// <param name="ticketReceiptId">ticketReceiptId</param>
        /// <param name="sqlTrx">Sql Trx</param>
        public TicketReceiptDTO GetTicketReceipts(int ticketReceiptId, SqlTransaction sqlTrxn = null)
        {
            log.LogMethodEntry(ticketReceiptId, sqlTrxn);
            TicketReceiptDataHandler ticketReceiptDataHandler = new TicketReceiptDataHandler(sqlTrxn);
            log.LogMethodExit(ticketReceiptDataHandler.GetTicketReceipt(ticketReceiptId, sqlTrxn));
            return ticketReceiptDataHandler.GetTicketReceipt(ticketReceiptId, sqlTrxn);
        }
        /// <summary>
        /// Returns the ticket receipt list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTrx">Sql Trx</param>
        public List<TicketReceiptDTO> GetAllTicketReceipt(List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>> searchParameters, SqlTransaction sqlTrxn = null)
        {
            log.LogMethodEntry(searchParameters, sqlTrxn);
            SetFromSiteTimeOffset(searchParameters);
            TicketReceiptDataHandler ticketReceiptDataHandler = new TicketReceiptDataHandler(sqlTrxn);
            ticketReceiptDTOList = ticketReceiptDataHandler.GetTicketReceiptList(searchParameters);
            SetToSiteTimeOffset(ticketReceiptDTOList);
            log.LogMethodExit(ticketReceiptDTOList);
            return ticketReceiptDTOList;
        }
        internal List<TicketReceiptDTO> SetToSiteTimeOffset(List<TicketReceiptDTO> ticketReceiptDTOList)
        {
            log.LogMethodEntry(ticketReceiptDTOList);
            List<TicketReceiptDTO> localticketReceiptDTOList = new List<TicketReceiptDTO>();
            if (executionContext != null && executionContext.IsCorporate)
            {
                if (localticketReceiptDTOList != null && localticketReceiptDTOList.Any())
                {
                    for (int i = 0; i < localticketReceiptDTOList.Count; i++)
                    {
                        ticketReceiptDTOList[i].CreationDate = SiteContainerList.FromSiteDateTime(ticketReceiptDTOList[i].SiteId, ticketReceiptDTOList[i].CreationDate);
                        ticketReceiptDTOList[i].LastUpdatedDate = SiteContainerList.FromSiteDateTime(ticketReceiptDTOList[i].SiteId, ticketReceiptDTOList[i].LastUpdatedDate);
                        if (ticketReceiptDTOList[i].IssueDate != null)
                        {
                            ticketReceiptDTOList[i].IssueDate = SiteContainerList.FromSiteDateTime(ticketReceiptDTOList[i].SiteId, (DateTime)ticketReceiptDTOList[i].IssueDate);
                        }
                        ticketReceiptDTOList[i].AcceptChanges();
                    }
                }
            }
            log.LogMethodExit(ticketReceiptDTOList);
            return ticketReceiptDTOList;
        }
        internal List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>> SetFromSiteTimeOffset(List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>> searchparams = new List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>>();
            if (searchParameters != null)
            {
                searchparams.AddRange(searchParameters);
            }
            if (executionContext != null && executionContext.IsCorporate)
            {
                if (searchparams != null && searchparams.Any())
                {
                    foreach (KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string> searchParameter in searchparams)
                    {
                        if ((searchParameter.Key == TicketReceiptDTO.SearchByTicketReceiptParameters.ISSUE_FROM_DATE) ||
                            (searchParameter.Key == TicketReceiptDTO.SearchByTicketReceiptParameters.ISSUE_TO_DATE) ||
                            (searchParameter.Key == TicketReceiptDTO.SearchByTicketReceiptParameters.UPDATED_FROM_TIME) ||
                            (searchParameter.Key == TicketReceiptDTO.SearchByTicketReceiptParameters.UPDATED_TO_TIME) )
                        {
                            if (!string.IsNullOrWhiteSpace(searchParameter.Value))
                            {
                                int index = searchParameters.IndexOf(searchParameter);
                                searchParameters[index] = new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(searchParameter.Key, SiteContainerList.ToSiteDateTime(executionContext.GetSiteId(), DateTime.Parse(searchParameter.Value)).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
                            }
                        }
                    }
                }
            }
            log.LogMethodEntry(searchParameters);
            return searchParameters;
        }

        /// <summary>
        /// This Method is used to Save the ticketReceiptDTO.
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (ticketReceiptDTOList == null ||
               ticketReceiptDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < ticketReceiptDTOList.Count; i++)
            {
                var ticketReceiptDTO = ticketReceiptDTOList[i];
                if (ticketReceiptDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    TicketReceipt ticketReceipt = new TicketReceipt(executionContext, ticketReceiptDTO);
                    ticketReceipt.Save(sqlTransaction);
                }
                catch (SqlException ex)
                {
                    log.Error(ex);
                    if (ex.Number == 2601)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1872));
                    }
                    else if (ex.Number == 547)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                    }
                    else
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, ex.Message));
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving TicketReceiptDTOList.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("TicketReceiptDTOList", ticketReceiptDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
