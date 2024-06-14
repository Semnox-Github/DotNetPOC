/********************************************************************************************
 * Project Name - TicketReceipt
 * Description  - Ticket Station class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.3.0       05-Jun-2018      Archana/Guru S A     Created 
 *2.70.2.0      16-Sept -2019    Girish Kundar        Modified : Part of Ticket Eater enhancements. 
 *2.110.0      21-Dec-2020      Abhishek           Modified : Modified for web API changes 
 *2.150.3     27-APR-2023   Sweedol             Modified: Management form access new architecture changes
 *2.150.3     29-MAY-2023   Sweedol             Modified: Removed generating management form access records for Ticket Station.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Redemption
{
    /// <summary>
    /// Logic for TicketStationBL class.
    /// </summary>
    public class TicketStationBL
    {
        internal static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected static ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        internal TicketStationDTO ticketStationDTO;
        private Utilities utilities = new Utilities();

        /// <summary>
        /// Default constructor TicketStationBL class
        /// </summary>
        public TicketStationBL()
        {
            log.LogMethodEntry();
            ticketStationDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with one parameter
        /// </summary>
        /// <param name="stationId">Id of the ticket station</param>
        public TicketStationBL(ExecutionContext executionContext, string stationId)
        {
            log.LogMethodEntry();
            TicketStationDataHandler ticketStationDataHandler = new TicketStationDataHandler();
            ticketStationDTO = ticketStationDataHandler.GetTicketStationsDTO(stationId);
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with TicketStationDTO parameter
        /// </summary>
        /// <param name="ticketStationDTO">parameter of type TicketStationDTO </param>
        public TicketStationBL(ExecutionContext executionContext, TicketStationDTO ticketStationDTO)
        {
            log.LogMethodEntry(ticketStationDTO);
            this.ticketStationDTO = ticketStationDTO;
            machineUserContext = executionContext;
            log.LogMethodExit();
        }


        /// <summary>
        /// Saves the Ticket station details to table
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            TicketStationDataHandler ticketStationDataHandler = new TicketStationDataHandler(sqlTransaction);
            List<ValidationError> validationErrorList = Validate();
            if (validationErrorList.Count > 0)
            {
                throw new ValidationException("Validation Failed", validationErrorList);
            }
            if (ticketStationDTO.Id < 0)
            {
                ticketStationDTO = ticketStationDataHandler.Insert(ticketStationDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                ticketStationDTO.AcceptChanges();
            }
            else
            {
                if (ticketStationDTO.IsChanged)
                {
                    ticketStationDTO = ticketStationDataHandler.Update(ticketStationDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                    ticketStationDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
        
        /// <summary>
        /// Checks whether barcode is belongs to the station or not
        /// </summary>
        /// <param name="barCode">barCode</param>
        /// <returns>true/false</returns>
        public bool BelongsToThisStation(string barCode)
        {
            log.LogMethodEntry();
            bool result = false;
            if (ticketStationDTO != null)
            {
                string stationId = barCode.Substring(0, ticketStationDTO.TicketStationId.Length);
                //int ticketLength = ticketStationDTO.CheckDigit ? barCode.Substring(barCode.Length - ticketStationDTO.TicketLength, ticketStationDTO.TicketLength).Length : barCode.Substring(barCode.Length - ticketStationDTO.TicketLength - 1, ticketStationDTO.TicketLength).Length;
                if (barCode.Length == ticketStationDTO.VoucherLength &&
                    stationId.ToUpper() == ticketStationDTO.TicketStationId.ToUpper())
                {
                    result = true;
                    log.LogMethodExit(result);
                    return result;
                }
            }
            log.LogMethodExit(result);
            return result;
        }


        /// <summary>
        /// ValidCheckBit method
        /// </summary>
        /// <param name="barCode">barcode of a ticket/receipt</param>
        /// <remarks>To validate the check bit of a passed barcode.</remarks>
        /// <returns>bool value</returns>
        public virtual bool ValidCheckBit(string barCode)
        {
            log.LogMethodEntry(barCode);
            bool valid = true;
            log.LogMethodExit(valid);
            return valid;
        }

        /// <summary>
        /// MatchCheckBit method
        /// </summary>
        /// <param name="receiptNumber">receipt number of a receipt</param>
        /// <param name="matchCheckBit">CheckBit value of a receipt</param>
        public virtual void MatchCheckBit(string receiptNumber, int matchCheckBit)
        {
            log.LogMethodEntry(receiptNumber, matchCheckBit);
            log.LogMethodExit();
        }

        /// <summary>
        /// GetCheckBit method
        /// </summary>
        /// <param name="receiptNumbers">receipt number of a receipt</param>
        /// <returns>check bit value</returns>
        public virtual int GetCheckBit(string receiptNumber)
        {
            log.LogMethodEntry(receiptNumber);
            int checkBit = 1;
            log.LogMethodExit(checkBit);
            return checkBit;
        }

        /// <summary>
        /// GetTicketValue method
        /// </summary>
        /// <param name="barCode">barCode</param>
        /// <returns>ticketValue</returns>
        public int GetTicketValue(string barCode)
        {
            log.LogMethodEntry();
            int ticketValue = 0;
            ticketValue = Convert.ToInt32(barCode.Substring(barCode.Length - (ticketStationDTO.CheckDigit ? ticketStationDTO.TicketLength + 1 : ticketStationDTO.TicketLength), ticketStationDTO.TicketLength));
            log.LogMethodExit();
            return ticketValue;
        }

        /// <summary>
        ///Generates the BarCode. This method is overridden in POSCounterTicketStationBL class 
        /// </summary>
        /// <param name="tickets">total tickets </param>
        /// <returns>barCode</returns>
        public virtual string GenerateBarCode(int tickets)
        {
            log.LogMethodEntry(tickets);
            string barCode = string.Empty;
            log.LogMethodExit(barCode);
            return barCode;
        }

        /// <summary>
        /// Get Ticket Station Receipt method
        /// </summary>
        /// <param name="barCode">Barcode value of a receipt</param>
        /// <returns>ticketRecipt object</returns>
        public TicketReceipt GetTicketStationRecepit(string barCode)
        {
            log.LogMethodEntry(barCode);
            TicketReceipt ticketReceipt = new TicketReceipt(machineUserContext ,barCode);
            if (ticketReceipt.IsUsedTicketReceipt(null))
            {
                throw new Exception(MessageContainerList.GetMessage(machineUserContext, 112));
                //Scanned Ticket Receipt is invalid 
            }
            try
            {
                int ticketLength = GetTicketLength();
                if (ticketReceipt.TicketReceiptDTO != null)
                {
                    ticketReceipt.TicketReceiptDTO.Tickets = Convert.ToInt32(barCode.Substring(barCode.Length - (ticketStationDTO.CheckDigit ? ticketLength + 1 : ticketLength), ticketLength));
                }
                else
                {
                    TicketReceiptDTO ticketReceiptDTO = new TicketReceiptDTO(-1, -1, barCode, machineUserContext.GetSiteId(), null, false, -1, Convert.ToInt32(barCode.Substring(barCode.Length - (ticketStationDTO.CheckDigit ? ticketLength + 1 : ticketLength), ticketLength)),
                                                                              Convert.ToInt32(barCode.Substring(barCode.Length - (ticketStationDTO.CheckDigit ? ticketLength + 1 : ticketLength), ticketLength)), machineUserContext.GetUserId(), DateTime.Now, false, -1, DateTime.Now, machineUserContext.GetUserId(), DateTime.Now);
                    ticketReceipt = new TicketReceipt(machineUserContext, ticketReceiptDTO);
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(MessageContainerList.GetMessage(machineUserContext, 115));
            }
            log.LogMethodExit(ticketReceipt);
            return ticketReceipt;
        }

        /// <summary>
        /// virtual method which gets Ticket Length
        /// </summary>
        /// <remarks>Ticket Length value for POSCounterTicket station is 4</remarks>
        /// <returns>ticket Length value.</returns>
        internal virtual int GetTicketLength()
        {
            log.LogMethodEntry();
            log.LogMethodExit(ticketStationDTO.TicketLength);
            return ticketStationDTO.TicketLength;
        }

        /// <summary>
        /// calls the GetRandomNumber function from utils
        /// </summary>
        /// <param name="width">width of the number</param>
        /// <returns></returns>
        public string GetRandomNumber(int width)
        {
            return utilities.GenerateRandomNumber(width, Utilities.RandomNumberType.AlphaNumeric);
        }

        /// <summary>
        /// Gets the TicketStationDTO
        /// </summary>
        public TicketStationDTO TicketStationDTO
        {
            get { return ticketStationDTO; }
        }

        /// <summary>
        /// Validate the Ticket station DTO for Duplicate Id.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            if (ticketStationDTO != null && string.IsNullOrEmpty(ticketStationDTO.TicketStationId) == false &&
                (ticketStationDTO.Id == -1 || ticketStationDTO.IsChanged))
            {
                // Check the inactive records also for duplicate stationId because inactive station can be activated in UI.
                TicketStationDataHandler ticketStationDataHandler = new TicketStationDataHandler(sqlTransaction);
                TicketStationListBL ticketStationList = new TicketStationListBL(machineUserContext);
                List<KeyValuePair<TicketStationDTO.SearchByTicketStationParameters, string>> searchParameters = new List<KeyValuePair<TicketStationDTO.SearchByTicketStationParameters, string>>();
                searchParameters.Add(new KeyValuePair<TicketStationDTO.SearchByTicketStationParameters, string>(TicketStationDTO.SearchByTicketStationParameters.TICKET_STATION_ID, ticketStationDTO.TicketStationId.ToString()));
                List<TicketStationDTO> ticketStationDTOList = ticketStationDataHandler.GetTicketStationDTOList(searchParameters);
                bool duplicateStationId = ticketStationDTOList.Where(x => x.TicketStationId == ticketStationDTO.TicketStationId && x.Id != ticketStationDTO.Id)
                                                                .Where(x => x.SiteId == ticketStationDTO.SiteId).Any();
                if (duplicateStationId)
                {
                    string errorMessage = MessageContainerList.GetMessage(utilities.ExecutionContext, 166, "Ticket Station Id", ticketStationDTO.TicketStationId);
                    validationErrorList.Add(new ValidationError("TicketStation", "Id", errorMessage));
                }
            }
            if (string.IsNullOrEmpty(ticketStationDTO.TicketStationId) ||
                ValidateAlphaNumeric(ticketStationDTO.TicketStationId) == false)
            {
                string errorMessage = MessageContainerList.GetMessage(utilities.ExecutionContext, 1144, MessageContainerList.GetMessage(utilities.ExecutionContext, "Ticket Station Id"));
                validationErrorList.Add(new ValidationError("TicketStation", "Id", errorMessage));
            }
            if (string.IsNullOrEmpty(ticketStationDTO.TicketStationId) == false && ticketStationDTO.TicketStationId.Length > 20)
            {
                string errorMessage = MessageContainerList.GetMessage(utilities.ExecutionContext, 2323, MessageContainerList.GetMessage(utilities.ExecutionContext, "Ticket Station Id"));
                validationErrorList.Add(new ValidationError("TicketStation", "Id", errorMessage));
            }

            if (string.IsNullOrEmpty(ticketStationDTO.VoucherLength.ToString()) || (ValidateNumericFields(ticketStationDTO.VoucherLength.ToString()) == false))
            {
                string errorMessage = (MessageContainerList.GetMessage(utilities.ExecutionContext, 1144, MessageContainerList.GetMessage(utilities.ExecutionContext, "Voucher Length")));
                validationErrorList.Add(new ValidationError("TicketStation", "Voucher Length", errorMessage));
            }
            if (string.IsNullOrEmpty(ticketStationDTO.TicketLength.ToString()) ||
                (ValidateNumericFields(ticketStationDTO.TicketLength.ToString()) == false))
            {
                string errorMessage = (MessageContainerList.GetMessage(utilities.ExecutionContext, 1144, MessageContainerList.GetMessage(utilities.ExecutionContext, "Ticket Length")));
                validationErrorList.Add(new ValidationError("TicketStation", "Ticket Length", errorMessage));
            }

            log.LogMethodExit(validationErrorList);
            return validationErrorList;

        }

        /// <summary>
        /// Matches Positive Integers
        /// </summary>
        /// <param name="field"></param>
        /// <returns>bool</returns>
        private bool ValidateNumericFields(string field)
        {
            log.LogMethodEntry();
            Regex objNotNaturalPattern = new Regex("[^0-9]");
            Regex objNaturalPattern = new Regex("0*[1-9][0-9]*");
            bool result = !objNotNaturalPattern.IsMatch(field) && objNaturalPattern.IsMatch(field);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Matches Alpha- Numeric Input
        /// </summary>
        /// <param name="strToCheck"></param>
        /// <returns></returns>
        public bool ValidateAlphaNumeric(String strToCheck)
        {
            log.LogMethodEntry();
            Regex objAlphaNumericPattern = new Regex(@"^[a-zA-Z0-9]+$");
            bool result = objAlphaNumericPattern.IsMatch(strToCheck);
            log.LogMethodExit(result);
            return result;
        }
    }

    /// <summary>
    /// Class for TicketStation List
    /// </summary>
    public class TicketStationListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<TicketStationDTO> ticketStationDTOList = new List<TicketStationDTO>();
        private ExecutionContext executionContext;
        public TicketStationListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Default Constructor
        /// </summary>
        public TicketStationListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor with executionContext and ticketStationDTOList as parameters
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="ticketStationDTOList">ticketStationDTOList</param>
        public TicketStationListBL(ExecutionContext executionContext, List<TicketStationDTO> ticketStationDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry();
            this.ticketStationDTOList = ticketStationDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns All active the TicketStations records from the table 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>List of active TicketStationDTO</returns>
        public List<TicketStationDTO> GetAllTicketStations(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            TicketStationDataHandler ticketStationDataHandler = new TicketStationDataHandler(sqlTransaction);
            List<KeyValuePair<TicketStationDTO.SearchByTicketStationParameters, string>> searchParameters = new List<KeyValuePair<TicketStationDTO.SearchByTicketStationParameters, string>>();
            searchParameters.Add(new KeyValuePair<TicketStationDTO.SearchByTicketStationParameters, string>(TicketStationDTO.SearchByTicketStationParameters.IS_ACTIVE, "1"));
            searchParameters.Add(new KeyValuePair<TicketStationDTO.SearchByTicketStationParameters, string>(TicketStationDTO.SearchByTicketStationParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<TicketStationDTO> ticketStationDTOList = ticketStationDataHandler.GetTicketStationDTOList(searchParameters);
            log.LogMethodExit(ticketStationDTOList);
            return ticketStationDTOList;
        }

        /// <summary>
        ///  Returns All the TicketStations records from the table 
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>List of TicketStationDTO</returns>
        public List<TicketStationDTO> GetTicketStationDTOList(List<KeyValuePair<TicketStationDTO.SearchByTicketStationParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            TicketStationDataHandler ticketStationDataHandler = new TicketStationDataHandler(sqlTransaction);
            List<TicketStationDTO> ticketStationDTOList = ticketStationDataHandler.GetTicketStationDTOList(searchParameters);
            log.LogMethodExit(ticketStationDTOList);
            return ticketStationDTOList;
        }

        /// <summary>
        /// Save and Update SaveUpdatePeripheralsList Method
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            try
            {
                if (ticketStationDTOList != null)
                {
                    foreach (TicketStationDTO ticketStationDTO in ticketStationDTOList)
                    {
                        TicketStationBL ticketStationBL = new TicketStationBL(executionContext, ticketStationDTO);
                        ticketStationBL.Save(sqlTransaction);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }
        internal DateTime? GetTicketStationModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            TicketStationDataHandler ticketStationDataHandler = new TicketStationDataHandler();
            DateTime? result = ticketStationDataHandler.GetTicketStationModuleLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}