/********************************************************************************************
 * Project Name - Reservation
 * Description  - Business Logic to create and save Reservations
 * 
 **************
 **Version Log
 **************
 *Version     Date              Modified By         Remarks          
 *********************************************************************************************
 *1.00        17-Sep-2008       Iqbal Mohammad      Created 
 *2.50.0      03-Dec-2018       Mathew Ninan        Remove staticDataExchange from calls as Staticdataexchange
 *                                                  is deprecated
 *2.50.0      28-Jan-2019       Guru S A            Booking changes
 *2.70        1-Jul-2019        Lakshminarayana     Modified to add support for ULC cards 
 *2.50.0      28-Jan-2019       Guru S A            Booking changes   
 *2.70        26-Mar-2019       Guru S A            Booking phase 2 changes   
 *2.70.2      26-Oct-2019       Guru S A            Waiver phase 2 changes   
 *2.70.3      30-Mar-2020       Jeevan              Booking attende Save order changes
 *2.80.0      28-Apr-2020       Guru S A            Send sign waiver email changes
 *2.80.0      09-Jun-2020       Jinto Thomas        Enable Active flag for Comboproduct data
 *2.90        03-Jun-2020       Guru S A            Reservation enhancements for commando release
 *2.140.0     12-Dec-2021       Guru S A            Booking execute process performance fixes
 *2.160.0     12-May-2022       Guru S A            Auto gratuity and service charge changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Semnox.Parafait.Discounts;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Product;
using System.Linq;
using Semnox.Parafait.Communication;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Maintenance;
using System.Globalization;
using System.Text;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Transaction
{
    public class ReservationBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private ReservationDTO reservationDTO;
        private ReservationDTO reservationDTOBeforeEdit;
        private Utilities utilities;
        private Core.GenericUtilities.EventLog audit;
        private Transaction bookingTransaction;
        private Transaction bookingTransactionBeforeEdit;
        private Dictionary<int, ComboProductBL> comboProductBLDictionary = new Dictionary<int, ComboProductBL>();
        private Dictionary<int, Products> productsBLDictionary = new Dictionary<int, Products>();
        private Dictionary<int, List<ComboProductDTO>> combProductDTOListDictionary = new Dictionary<int, List<ComboProductDTO>>();
        private Dictionary<int, SchedulesBL> schedulesBLDictionary = new Dictionary<int, SchedulesBL>();
        /// <summary>
        /// Default constructor of Bookings class
        /// </summary>
        public ReservationBL(ExecutionContext executionContext, Utilities utilities)
        {
            log.LogMethodEntry(executionContext, utilities);
            reservationDTO = null;
            this.executionContext = executionContext;
            this.utilities = utilities;
            this.audit = new Core.GenericUtilities.EventLog(utilities.ExecutionContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// Parmaeterized constructor of Bookings class
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="utilities"></param>
        /// <param name="bookingId"></param>
        /// <param name="sqlTrx"></param>
        public ReservationBL(ExecutionContext executionContext, Utilities utilities, int bookingId, SqlTransaction sqlTrx = null)
            : this(executionContext, utilities)
        {
            log.LogMethodEntry(bookingId, sqlTrx);
            ReservationDatahandler reservationDataHandler = new ReservationDatahandler(sqlTrx);
            reservationDTO = reservationDataHandler.GetReservationDTO(executionContext, bookingId);
            BuildReservationDetails(sqlTrx);
            LoadBeforeEditDetails(sqlTrx);
            log.LogMethodExit();
        }


        /// <summary>
        /// Parmaeterized constructor of Bookings class
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="utilities"></param>
        /// <param name="reservationDTO"></param>
        /// <param name="sqlTrx"></param>
        public ReservationBL(ExecutionContext executionContext, Utilities utilities, ReservationDTO reservationDTO, SqlTransaction sqlTrx = null)
            : this(executionContext, utilities)
        {
            log.LogMethodEntry(reservationDTO, sqlTrx);
            this.reservationDTO = reservationDTO;
            BuildReservationDetails(sqlTrx);
            LoadBeforeEditDetails(sqlTrx);
            log.LogMethodExit();
        }
        /// <summary>
        /// Parmaeterized constructor of Bookings class
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="utilities"></param>
        /// <param name="reservationCode"></param>
        /// <param name="sqlTrx"></param>
        public ReservationBL(ExecutionContext executionContext, Utilities utilities, string reservationCode, SqlTransaction sqlTrx = null)
            : this(executionContext, utilities)
        {
            log.LogMethodEntry(reservationCode);
            ReservationDatahandler reservationDataHandler = new ReservationDatahandler(sqlTrx);
            reservationDTO = reservationDataHandler.GetReservationDTO(executionContext, reservationCode);
            BuildReservationDetails(sqlTrx);
            LoadBeforeEditDetails(sqlTrx);
            log.LogMethodExit();
        }

        private void BuildReservationDetails(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry();
            LoadAttendeeDetails(sqlTrx);
            LoadCheckListDetails(sqlTrx);
            LoadTransactionDetails(sqlTrx);
            log.LogMethodExit();
        }

        /// <summary>
        /// Load attendee details
        /// </summary>
        public void LoadAttendeeDetails(SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(sqlTrx);
            if (this.reservationDTO != null && this.reservationDTO.BookingId > -1 && this.bookingTransaction != null && this.bookingTransaction.Trx_id > 0)
            {
                //BookingAttendeeList bookingAttendeeList = new BookingAttendeeList(executionContext);
                //List<KeyValuePair<BookingAttendeeDTO.SearchByParameters, string>> attendeeSearch = new List<KeyValuePair<BookingAttendeeDTO.SearchByParameters, string>>();
                //attendeeSearch.Add(new KeyValuePair<BookingAttendeeDTO.SearchByParameters, string>(BookingAttendeeDTO.SearchByParameters.BOOKING_ID, this.reservationDTO.BookingId.ToString()));
                //attendeeSearch.Add(new KeyValuePair<BookingAttendeeDTO.SearchByParameters, string>(BookingAttendeeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                //this.reservationDTO.BookingAttendeeList = bookingAttendeeList.GetAllBookingAttendeeList(attendeeSearch);
                this.bookingTransaction.LoadAttendeeDetails(sqlTrx);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Load CheckList details
        /// </summary>
        public void LoadCheckListDetails(SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry();
            if (this.reservationDTO != null && this.reservationDTO.BookingId > -1)
            {
                BookingCheckListListBL bookingCheckListListBL = new BookingCheckListListBL(executionContext);
                List<KeyValuePair<BookingCheckListDTO.SearchByParameters, string>> checkListSearch = new List<KeyValuePair<BookingCheckListDTO.SearchByParameters, string>>();
                checkListSearch.Add(new KeyValuePair<BookingCheckListDTO.SearchByParameters, string>(BookingCheckListDTO.SearchByParameters.BOOKING_ID, this.reservationDTO.BookingId.ToString()));
                checkListSearch.Add(new KeyValuePair<BookingCheckListDTO.SearchByParameters, string>(BookingCheckListDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                this.reservationDTO.BookingCheckListDTOList = bookingCheckListListBL.GetBookingCheckListDTOList(checkListSearch, sqlTrx);
            }
            log.LogMethodExit();
        }

        private void LoadTransactionDetails(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry();
            if (this.reservationDTO != null && this.reservationDTO.BookingId > -1 && this.reservationDTO.TrxId > -1)
            {
                TransactionUtils transactionUtils = new TransactionUtils(utilities);
                bool loadCancelledLines = false;
                if (this.reservationDTO.Status == ReservationDTO.ReservationStatus.CANCELLED.ToString())
                {
                    loadCancelledLines = true;
                }
                bookingTransaction = transactionUtils.CreateTransactionFromDB(this.reservationDTO.TrxId, utilities, false, loadCancelledLines, sqlTrx);
                if (bookingTransaction != null)
                {
                    bookingTransaction.ApplyBookingDatePromotionPrice = true;
                }
            }
            log.LogMethodExit();
        }

        private void LoadBeforeEditDetails(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry();
            if (reservationDTO != null && reservationDTO.Status == ReservationDTO.ReservationStatus.WIP.ToString() && reservationDTO.BookingId > -1)
            {
                ReservationDatahandler reservationDataHandler = new ReservationDatahandler(sqlTrx);
                reservationDTOBeforeEdit = reservationDataHandler.GetReservationDTO(executionContext, reservationDTO.BookingId);

                BookingCheckListListBL bookingCheckListListBL = new BookingCheckListListBL(executionContext);
                List<KeyValuePair<BookingCheckListDTO.SearchByParameters, string>> checkListSearch = new List<KeyValuePair<BookingCheckListDTO.SearchByParameters, string>>();
                checkListSearch.Add(new KeyValuePair<BookingCheckListDTO.SearchByParameters, string>(BookingCheckListDTO.SearchByParameters.BOOKING_ID, this.reservationDTOBeforeEdit.BookingId.ToString()));
                checkListSearch.Add(new KeyValuePair<BookingCheckListDTO.SearchByParameters, string>(BookingCheckListDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                this.reservationDTOBeforeEdit.BookingCheckListDTOList = bookingCheckListListBL.GetBookingCheckListDTOList(checkListSearch, sqlTrx);
                if (reservationDTOBeforeEdit.TrxId > -1)
                {
                    TransactionUtils transactionUtils = new TransactionUtils(utilities);
                    bookingTransactionBeforeEdit = transactionUtils.CreateTransactionFromDB(reservationDTOBeforeEdit.TrxId, utilities, false, false, sqlTrx);
                    bookingTransactionBeforeEdit.LoadAttendeeDetails(sqlTrx);
                }
            }
            log.LogMethodExit();
        }

        public ReservationDTO GetReservationDTO { get { return reservationDTO; } }

        public Transaction BookingTransaction { get { return bookingTransaction; } }

        public int TransactionOffsetDuration { get { return GetTransactionOffsetDuration(); } set { SetTransactionOffsetDuration(value); } }

        private void SetTransactionOffsetDuration(int offsetDurationValue)
        {
            log.LogMethodEntry(offsetDurationValue);
            if (this.reservationDTO != null)
            {
                if (this.bookingTransaction == null)
                {
                    InitiateTransaction();
                }
                this.bookingTransaction.offSetDuration = offsetDurationValue;
            }
            else
            {
                log.LogVariableState("reservationDTO", this.reservationDTO);
                // "Sorry, unable to proceed. Reservation details are missing" 
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2131));
            }
            log.LogMethodExit();
        }

        private int GetTransactionOffsetDuration()
        {
            log.LogMethodEntry();
            int offsetDurationValue = 0;
            if (this.bookingTransaction != null)
            {
                offsetDurationValue = this.bookingTransaction.offSetDuration;
            }

            log.LogMethodExit(offsetDurationValue);
            return offsetDurationValue;
        }

        public void Save(SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(sqlTrx);
            if (this.reservationDTO == null)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2131));// "Sorry, unable to proceed. Reservation details are missing"
            }
            ParafaitDBTransaction dBTransaction = null;
            try
            {
                if (sqlTrx == null)
                {
                    dBTransaction = new ParafaitDBTransaction();
                    dBTransaction.BeginTransaction();
                    sqlTrx = dBTransaction.SQLTrx;
                }
                SaveReservationHeader(sqlTrx);
                SaveBookingCheckList(sqlTrx);
                SaveTransaction(sqlTrx);
                SaveAttendees(sqlTrx);

                if (dBTransaction != null)
                {
                    dBTransaction.EndTransaction();
                    sqlTrx = null;
                    dBTransaction.Dispose();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (dBTransaction != null)
                {
                    dBTransaction.RollBack();
                    dBTransaction.Dispose();
                    sqlTrx = null;
                }
                throw;
            }
        }

        private void SaveReservationHeader(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            SetReservationHeaderDetails();
            ReservationDatahandler reservationDataHandler = new ReservationDatahandler(sqlTrx);
            if (reservationDTO.BookingId < 0)
            {
                if (String.IsNullOrEmpty(reservationDTO.ReservationCode))
                {
                    reservationDTO.ReservationCode = utilities.GenerateRandomCardNumber(5);
                }
                if (reservationDTO.Status == ReservationDTO.ReservationStatus.NEW.ToString())
                {
                    reservationDTO.Status = ReservationDTO.ReservationStatus.BOOKED.ToString();
                }
                reservationDTO = reservationDataHandler.InsertReservationDTO(reservationDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                reservationDTO.AcceptChanges();
            }
            else
            {
                if (reservationDTO.IsChanged)
                {
                    reservationDTO = reservationDataHandler.UpdateReservationDTO(reservationDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    reservationDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }


        private void SetReservationHeaderDetails()
        {
            log.LogMethodEntry();
            if (ReservationTransactionIsNotNull())
            {
                List<Transaction.TransactionLine> scheduleLines = GetScheduleTransactionLines();
                if (scheduleLines != null && scheduleLines.Count > 0)
                {
                    List<Transaction.TransactionLine> activeScheduleLines = scheduleLines.Where(tl => tl.LineValid == true).ToList();
                    if (activeScheduleLines != null && activeScheduleLines.Count > 0)
                    {
                        DateTime scheduleFromDate;
                        try
                        {
                            scheduleFromDate = activeScheduleLines.Min(tl => tl.TransactionReservationScheduleDTOList.Find(trs => trs.Cancelled == false).ScheduleFromDate);
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            scheduleFromDate = GetMinFromDate(activeScheduleLines);
                        }
                        DateTime scheduleToDate;
                        try
                        {
                            scheduleToDate = activeScheduleLines.Max(tl => tl.TransactionReservationScheduleDTOList.Find(trs => trs.Cancelled == false).ScheduleToDate);
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            scheduleToDate = GetMaxToDate(activeScheduleLines);
                        }
                        log.LogVariableState("scheduleFromDate", scheduleFromDate);
                        log.LogVariableState("scheduleToDate", scheduleToDate);

                        if (this.reservationDTO.FromDate != scheduleFromDate && scheduleFromDate != DateTime.MaxValue)
                        {
                            this.reservationDTO.FromDate = scheduleFromDate;
                            Transaction.TransactionLine minScheduleLine = activeScheduleLines.Find(tl => tl.TransactionReservationScheduleDTOList.Exists(trs => trs.ScheduleFromDate == scheduleFromDate));
                            if (minScheduleLine != null && minScheduleLine.TransactionReservationScheduleDTOList != null)
                            {
                                this.reservationDTO.AttractionScheduleId = minScheduleLine.GetCurrentTransactionReservationScheduleDTO().SchedulesId;
                            }
                        }
                        if (this.reservationDTO.ToDate != scheduleToDate && scheduleToDate != DateTime.MinValue)
                        {
                            this.reservationDTO.ToDate = scheduleToDate;
                        }
                    }
                    int guestQty = GetGuestQuantity();
                    log.LogVariableState("guestQty", guestQty);
                    if (this.reservationDTO.Quantity != guestQty
                        && this.reservationDTO.Status != ReservationDTO.ReservationStatus.CANCELLED.ToString())  //retain qty during cancellation
                    {
                        this.reservationDTO.Quantity = guestQty;
                    }
                    Transaction.TransactionLine bookingProductLine = this.BookingTransaction.GetBookingProductTransactionLine();
                    if (bookingProductLine != null && bookingProductLine.ProductID != this.reservationDTO.BookingProductId)
                    {
                        this.reservationDTO.BookingProductId = bookingProductLine.ProductID;
                    }
                    else if (bookingProductLine == null)
                    {
                        this.reservationDTO.BookingProductId = -1;
                    }

                    if (this.bookingTransaction != null && this.bookingTransaction.customerDTO != null
                        && this.bookingTransaction.customerDTO.Id != this.reservationDTO.CustomerId)
                    {
                        this.reservationDTO.CustomerId = this.bookingTransaction.customerDTO.Id;
                        this.reservationDTO.CustomerName = this.bookingTransaction.customerDTO.FirstName;
                    }
                    else if (this.bookingTransaction == null || (this.bookingTransaction != null && this.bookingTransaction.customerDTO == null))
                    {
                        this.reservationDTO.CustomerId = -1;
                        this.reservationDTO.CustomerName = string.Empty;
                    }
                }
            }
            log.LogMethodExit();
        }

        private DateTime GetMaxToDate(List<Transaction.TransactionLine> activeScheduleLines)
        {
            log.LogMethodEntry(activeScheduleLines);
            DateTime dateTimeValue = DateTime.MinValue;
            if (activeScheduleLines != null && activeScheduleLines.Any())
            {
                for (int i = 0; i < activeScheduleLines.Count; i++)
                {
                    int maxId = -1;
                    for (int j = 0; j < activeScheduleLines[i].TransactionReservationScheduleDTOList.Count; j++)
                    {
                        if (maxId < activeScheduleLines[i].TransactionReservationScheduleDTOList[j].TrxReservationScheduleId)
                        {
                            maxId = activeScheduleLines[i].TransactionReservationScheduleDTOList[j].TrxReservationScheduleId;
                        }
                    }
                    List<TransactionReservationScheduleDTO> list = activeScheduleLines[i].TransactionReservationScheduleDTOList.Where(trs => trs.TrxReservationScheduleId == maxId).ToList();
                    if (list != null && list.Any() && dateTimeValue < list[0].ScheduleToDate)
                    {
                        dateTimeValue = list[0].ScheduleToDate;
                    }
                }
            }
            log.LogMethodExit(dateTimeValue);
            return dateTimeValue;
        }

        private DateTime GetMinFromDate(List<Transaction.TransactionLine> activeScheduleLines)
        {
            log.LogMethodEntry(activeScheduleLines);
            DateTime dateTimeValue = DateTime.MaxValue;
            if (activeScheduleLines != null && activeScheduleLines.Any())
            {
                for (int i = 0; i < activeScheduleLines.Count; i++)
                {
                    int maxId = -1;
                    for (int j = 0; j < activeScheduleLines[i].TransactionReservationScheduleDTOList.Count; j++)
                    {
                        if (maxId < activeScheduleLines[i].TransactionReservationScheduleDTOList[j].TrxReservationScheduleId)
                        {
                            maxId = activeScheduleLines[i].TransactionReservationScheduleDTOList[j].TrxReservationScheduleId;
                        }
                    }
                    List<TransactionReservationScheduleDTO> list = activeScheduleLines[i].TransactionReservationScheduleDTOList.Where(trs => trs.TrxReservationScheduleId == maxId).ToList();
                    if (list != null && list.Any() && dateTimeValue > list[0].ScheduleFromDate)
                    {
                        dateTimeValue = list[0].ScheduleFromDate;
                    }
                }
            }
            log.LogMethodExit(dateTimeValue);
            return dateTimeValue;
        }

        /// <summary>
        /// Set Reservation Header Details
        /// </summary>
        public void SetHeaderDetails()
        {
            log.LogMethodEntry();
            if (BookingIsInEditMode())
            {
                SetReservationHeaderDetails();
                if (bookingTransaction != null)
                {
                    this.bookingTransaction.SetReservationTransactionDate(this.reservationDTO);
                }
            }
            log.LogMethodExit();
        }

        private bool BookingIsInEditMode()
        {
            log.LogMethodEntry();
            bool inEditMode = ((this.reservationDTO == null || (this.reservationDTO != null
                                                                                                      && (this.reservationDTO.Status == ReservationDTO.ReservationStatus.NEW.ToString()
                                                                                                         || this.reservationDTO.Status == ReservationDTO.ReservationStatus.WIP.ToString()
                                                                                                         || this.reservationDTO.Status == ReservationDTO.ReservationStatus.BLOCKED.ToString())
                                                                                                     )));
            log.LogMethodExit(inEditMode);
            return inEditMode;
        }
        private void SaveAttendees(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            if (reservationDTO != null && reservationDTO.BookingId > -1 && this.bookingTransaction != null)
            {
                this.bookingTransaction.SaveReservationAttendees(reservationDTO.BookingId, reservationDTO.Guid, sqlTrx);
            }
            log.LogMethodExit();
        }

        private void SaveBookingCheckList(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            if (reservationDTO != null && reservationDTO.BookingId > -1
                && reservationDTO.BookingCheckListDTOList != null
                && reservationDTO.BookingCheckListDTOList.Any())
            {
                for (int i = 0; i < reservationDTO.BookingCheckListDTOList.Count; i++)
                {
                    if (reservationDTO.BookingCheckListDTOList[i].IsChanged || reservationDTO.BookingCheckListDTOList[i].BookingCheckListId == -1)
                    {
                        BookingCheckListBL bookingCheckListBL = new BookingCheckListBL(executionContext, reservationDTO.BookingCheckListDTOList[i]);
                        bookingCheckListBL.Save(sqlTrx);
                    }
                }
            }
            log.LogMethodExit();
        }

        private void SaveTransaction(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            if (reservationDTO != null && this.bookingTransaction != null)
            {
                if (reservationDTO.Status == ReservationDTO.ReservationStatus.NEW.ToString())
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2133));// "Save booking entry before saving booking transaction"
                }

                //if ((transaction.Status != Transaction.TrxStatus.CANCELLED || transaction.Status != Transaction.TrxStatus.SYSTEMABANDONED))
                //{
                //    throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Cannot save booking transaction as it is " + transaction.Status.ToString()));
                //}

                this.bookingTransaction.SetReservationTransactionStatus(this.reservationDTO, sqlTrx);
                this.bookingTransaction.SetReservationTransactionDate(this.reservationDTO);

                string msg = "";
                if (ReservationDefaultSetup.IsAutoChargeOptionEnabledForReservation(executionContext) == false)
                {
                    //keep this calculation just before save so that all trx lines are considered
                    if (this.reservationDTO.ServiceChargeAmount > 0 || this.reservationDTO.ServiceChargePercentage > 0)
                    {
                        Card card = null;
                        if (this.reservationDTO.CardId > -1)
                        {
                            card = new Card(this.reservationDTO.CardId, executionContext.GetUserId(), utilities);
                        }
                        if (this.reservationDTO.Status != ReservationDTO.ReservationStatus.CANCELLED.ToString()
                            && this.reservationDTO.Status != ReservationDTO.ReservationStatus.COMPLETE.ToString()
                            && this.reservationDTO.Status != ReservationDTO.ReservationStatus.SYSTEMABANDONED.ToString())
                        {
                            this.bookingTransaction.AddManuallyAppliedServiceCharges(card, this.reservationDTO.ServiceChargeAmount, this.reservationDTO.ServiceChargePercentage, sqlTrx);
                        }

                    }
                    else
                    {
                        if (this.reservationDTO.Status != ReservationDTO.ReservationStatus.CANCELLED.ToString()
                            && this.reservationDTO.Status != ReservationDTO.ReservationStatus.COMPLETE.ToString()
                            && this.reservationDTO.Status != ReservationDTO.ReservationStatus.SYSTEMABANDONED.ToString())
                        {
                            if (HasServiceCharges())
                            {
                                CallTrxRemoveManuallyAppliedServiceCharges(sqlTrx);
                            }
                        }
                    }
                }
                this.bookingTransaction.SaveCustomer(sqlTrx);
                this.bookingTransaction.UpdateReservationTrxDate(sqlTrx);
                if (this.bookingTransaction.SaveOrder(ref msg, sqlTrx) != 0)
                {
                    log.Error(msg);
                    throw new ValidationException(msg);
                }
                if (this.reservationDTO.TrxId != this.bookingTransaction.Trx_id
                    || (this.bookingTransaction.customerDTO != null && this.reservationDTO.CustomerId != this.bookingTransaction.customerDTO.Id))
                {
                    this.reservationDTO.TrxId = this.bookingTransaction.Trx_id;
                    if (this.bookingTransaction.customerDTO != null)
                    {
                        this.reservationDTO.CustomerId = this.bookingTransaction.customerDTO.Id;
                        this.reservationDTO.CustomerName = this.bookingTransaction.customerDTO.FirstName;
                    }
                    else
                    {
                        this.reservationDTO.CustomerId = -1;
                        this.reservationDTO.CustomerName = string.Empty;
                    }
                    ReservationDatahandler reservationDataHandler = new ReservationDatahandler(sqlTrx);
                    reservationDataHandler.UpdateReservationDTO(reservationDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    reservationDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        private void DTONullCheck()
        {
            log.LogMethodEntry();
            if (this.reservationDTO == null)
            {
                //Sorry, unable to proceed. Reservation details are missing
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2131));// "Sorry, unable to proceed. Reservation details are missing"
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// check whether reservation has transaction or not
        /// </summary>
        /// <returns></returns>
        public bool ReservationTransactionIsNotNull()
        {
            log.LogMethodEntry();
            bool isNotNull = this.reservationDTO != null && this.bookingTransaction != null && this.bookingTransaction.TrxLines != null;
            log.LogMethodExit(isNotNull);
            return isNotNull;
        }

        /// <summary>
        /// Book the Reservation
        /// </summary>
        /// <param name="sqlTrx"></param>
        public void BookReservation(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            DTONullCheck();
            bool newBooking = true;
            if (this.reservationDTO.Status != ReservationDTO.ReservationStatus.NEW.ToString()
               && this.reservationDTO.Status != ReservationDTO.ReservationStatus.BLOCKED.ToString()
               && this.reservationDTO.Status != ReservationDTO.ReservationStatus.WIP.ToString())
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2134, this.reservationDTO.Status));// "Sorry, you cannot save the booking when it is in " + this.reservationDTO.Status + " status"
            }
            if (this.reservationDTO.Status == ReservationDTO.ReservationStatus.WIP.ToString())
            {
                newBooking = false;
            }
            ParafaitDBTransaction dBTransaction = null;
            if (sqlTrx == null)
            {
                dBTransaction = new ParafaitDBTransaction();
                dBTransaction.BeginTransaction();
                sqlTrx = dBTransaction.SQLTrx;
            }
            ValidateReservation(sqlTrx);
            try
            {
                this.reservationDTO.Status = ReservationDTO.ReservationStatus.BOOKED.ToString();
                this.reservationDTO.ExpiryTime = null;
                this.Save(sqlTrx);
                if (newBooking)
                {
                    //Booking is saved sucessfully. Reservation code is &1. Booking status is &2
                    audit.logEvent(MessageContainerList.GetMessage(executionContext, "Reservation"), 'D', executionContext.GetUserId(),
                        MessageContainerList.GetMessage(executionContext, 2135, reservationDTO.ReservationCode, reservationDTO.Status),
                        MessageContainerList.GetMessage(executionContext, "Book Reservation"), 0, "", reservationDTO.Guid.ToString(), sqlTrx);
                }
                else
                {
                    LogEvent(true, sqlTrx);
                }
                //throw new Exception("test");
                if (dBTransaction != null)
                {
                    dBTransaction.EndTransaction();
                    sqlTrx = null;
                    dBTransaction.Dispose();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (dBTransaction != null)
                {
                    dBTransaction.RollBack();
                    dBTransaction.Dispose();
                    sqlTrx = null;
                }
                throw;
            }
            log.LogMethodExit();
        }

        private void ValidateReservation(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            DTONullCheck();
            HasBookingProduct();
            HasValidSchedule();
            ValidateScheduleNQuantity();
            HasValidCustomer();
            HasPackageProducts();
            log.LogMethodExit();
        }

        public void HasBookingProduct()
        {
            log.LogMethodEntry();
            bool present = false;
            if (this.reservationDTO != null && this.bookingTransaction != null)
            {
                present = this.bookingTransaction.HasBookingProduct();
                log.LogVariableState("present", present);
            }
            if (present == false)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2155));//"Booking product is mandatory"
            }
            log.LogMethodExit();
        }


        /// <summary>
        ///Check whether reservation has valid schedule 
        /// </summary>
        public void HasValidSchedule()
        {
            log.LogMethodEntry();
            bool hasSchedule = false;
            if (this.reservationDTO != null && this.bookingTransaction != null)
            {
                hasSchedule = this.bookingTransaction.HasValidPartyBookingSchedule();
                log.LogVariableState("hasSchedule", hasSchedule);
            }
            if (hasSchedule == false)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2156));// "Booking schedule entry is mandatory"
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Check whether reservation has customer details
        /// </summary>
        public void HasValidCustomer()
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            if (this.reservationDTO != null && this.bookingTransaction != null)
            {
                validationErrorList = this.bookingTransaction.HasValidCustomer();
            }

            log.LogVariableState("validationErrorList", validationErrorList);
            if (validationErrorList != null && validationErrorList.Count > 0)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2157), validationErrorList);//"Valid Customer is required"
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Check whether reservation has package products
        /// </summary>
        public void HasPackageProducts()
        {
            log.LogMethodEntry();
            bool hasPackageProduct = false;
            if (this.reservationDTO != null && this.bookingTransaction != null)
            {
                hasPackageProduct = this.bookingTransaction.HasBookingPackageProducts();
                log.LogVariableState("hasPackageProduct", hasPackageProduct);
            }
            if (hasPackageProduct == false)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2158));//"Atleast one package product is mandatory for the booking"
            }
            log.LogMethodExit();
        }


        private void ValidateScheduleNQuantity()
        {
            log.LogMethodEntry();
            ReservationIsInEditMode();
            if (ReservationTransactionIsNotNull())
            {
                Products bookingProduct;
                bookingProduct = GetBookingProduct();
                List<Transaction.TransactionLine> scheduleTrxLines = GetScheduleTransactionLines();
                if (scheduleTrxLines != null && scheduleTrxLines.Count > 0)
                {
                    scheduleTrxLines = scheduleTrxLines.Where(tl => tl.LineValid == true).ToList();
                    if (scheduleTrxLines != null && scheduleTrxLines.Count > 0)
                    {
                        foreach (Transaction.TransactionLine scheduleTrxLine in scheduleTrxLines)
                        {
                            PerformValidation(scheduleTrxLine.GetCurrentTransactionReservationScheduleDTO(), bookingProduct.GetProductsDTO);
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Validate schedule and quantity
        /// </summary>
        /// <param name="bookingProductId"></param>
        /// <param name="transactionReservationScheduleDTO"></param>
        public void ValidateScheduleNQuantity(int bookingProductId, TransactionReservationScheduleDTO transactionReservationScheduleDTO, int trxLineIndex)
        {
            log.LogMethodEntry();
            ReservationIsInEditMode();
            if (bookingProductId == -1 && trxLineIndex != -1)
            {
                if (this.bookingTransaction != null && this.bookingTransaction.TrxLines != null && this.bookingTransaction.TrxLines.Any())
                {
                    Transaction.TransactionLine scheduleLine = this.bookingTransaction.TrxLines[trxLineIndex];
                    Transaction.TransactionLine bookingProdcutLine = this.bookingTransaction.GetBookingProductTransactionLine();
                    if (bookingProdcutLine.ParentLine == scheduleLine)
                    {
                        bookingProductId = bookingProdcutLine.ProductID;
                    }
                }
            }
            Products bookingProduct = null;
            if (bookingProductId > -1)
            {
                bookingProduct = GetProductsBL(bookingProductId);
            }

            if (transactionReservationScheduleDTO != null)
            {
                PerformValidation(transactionReservationScheduleDTO, (bookingProduct != null ? bookingProduct.GetProductsDTO : null));
            }
            else
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2138));//"Please pass booking product or selected schedule details"
            }
            log.LogMethodExit();
        }

        private void PerformValidation(TransactionReservationScheduleDTO transactionReservationScheduleDTO, ProductsDTO bookingProductDTO = null)
        {
            log.LogMethodEntry(transactionReservationScheduleDTO, bookingProductDTO);
            string message;
            //int scheduleId = transactionReservationScheduleDTO.SchedulesId;
            //int facilityMapId = transactionReservationScheduleDTO.FacilityMapId;
            //int quantity = transactionReservationScheduleDTO.GuestQuantity;
            DateTime scheduleFrom = transactionReservationScheduleDTO.ScheduleFromDate;
            DateTime scheduleTo = transactionReservationScheduleDTO.ScheduleToDate;
            SchedulesBL schedulesBL = GetSchedulesBL(transactionReservationScheduleDTO.SchedulesId);//
            decimal fromTime = Convert.ToDecimal(scheduleFrom.Hour + scheduleFrom.Minute / 100.0);
            decimal toTime = Convert.ToDecimal(scheduleTo.Hour + scheduleTo.Minute / 100.0);
            int duration = (int)(scheduleTo - scheduleFrom).TotalMinutes;

            //if (scheduleFrom > scheduleTo)
            //{
            //    message = MessageContainerList.GetMessage(executionContext, 305);
            //    log.Error(message);
            //    log.LogMethodExit(message);
            //    throw new ValidationException(message);
            //}
            //if (transactionReservationScheduleDTO.GuestQuantity < 1)
            //{
            //    message = MessageContainerList.GetMessage(executionContext, 2104);//Please enter valid guest quantity
            //    log.Error(message);
            //    log.LogMethodExit(message);
            //    throw new ValidationException(message);
            //}
            ScheduleDetailsDTO elibleSchedule = schedulesBL.GetEligibleScheduleDetails(scheduleFrom, fromTime, toTime, transactionReservationScheduleDTO.FacilityMapId);
            if (elibleSchedule != null)
            {
                if (elibleSchedule.FixedSchedule == false && bookingProductDTO != null)
                {
                    if (bookingProductDTO.MinimumTime > 0)
                    {
                        if (duration < bookingProductDTO.MinimumTime)
                        {
                            message = MessageContainerList.GetMessage(executionContext, 324, bookingProductDTO.MinimumTime.ToString());
                            log.Error(message);
                            log.LogMethodExit(message);
                            throw new ValidationException(message);
                        }
                    }

                    if (bookingProductDTO.MaximumTime > 0 && duration > bookingProductDTO.MaximumTime)
                    {
                        message = MessageContainerList.GetMessage(executionContext, 325, bookingProductDTO.MaximumTime.ToString());
                        log.Error(message);
                        log.LogMethodExit(message);
                        throw new ValidationException(message);
                    }
                }
                TransactionReservationScheduleBL transactionReservationScheduleBL = new TransactionReservationScheduleBL(executionContext, transactionReservationScheduleDTO);
                transactionReservationScheduleBL.CanAcceptReservationSchedule();
                //FacilityMapBL facilityMapBL = new FacilityMapBL(executionContext, facilityMapId);
                //facilityMapBL.CanAccomodateReservationQty(transactionReservationScheduleDTO.GuestQuantity, elibleSchedule.RuleUnits, scheduleFrom, scheduleTo, this.reservationDTO.TrxId, transactionReservationScheduleDTO.TrxReservationScheduleId);
                //int bookedQty = facilityMapBL.GetTotalBookedUnitsForBookings(scheduleFrom, scheduleTo, this.reservationDTO.BookingId);

                //int maxQty = (int)(elibleSchedule.RuleUnits != null ? elibleSchedule.RuleUnits : (facilityMapBL.FacilityMapDTO.FacilityCapacity != null ? facilityMapBL.FacilityMapDTO.FacilityCapacity : 0));
                //if (bookedQty + quantity > maxQty)
                //{
                //    message = MessageContainerList.GetMessage(executionContext, 326, quantity, (maxQty - bookedQty));
                //    log.Error(message);
                //    log.LogMethodExit(message);
                //    throw new ValidationException(message);
                //}

                if (bookingProductDTO != null && bookingProductDTO.MinimumQuantity > 0)
                {

                    if (bookingProductDTO.MinimumQuantity > 0 && transactionReservationScheduleDTO.GuestQuantity < bookingProductDTO.MinimumQuantity)
                    {
                        message = MessageContainerList.GetMessage(executionContext, 2139, bookingProductDTO.MinimumQuantity);// "Minimum quantity should be " + bookingProduct.GetProductsDTO.MinimumQuantity.ToString()
                        log.Error(message);
                        log.LogMethodExit(message);
                        throw new ValidationException(message);
                    }
                }

                //if (bookedQty > 0)
                //{
                //    if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ALLOW_MULTIPLE_BOOKINGS_WITHIN_SCHEDULE").Equals("N"))
                //    {
                //        message = MessageContainerList.GetMessage(executionContext, 1254);
                //        log.Error(message);
                //        log.LogMethodExit(message);
                //        throw new ValidationException(message);
                //    }
                //    if (facilityMapBL.FacilityMapDTO.FacilityCapacity >= 0)
                //    {
                //        if (bookedQty + quantity > facilityMapBL.FacilityMapDTO.FacilityCapacity)
                //        {
                //            message = MessageContainerList.GetMessage(executionContext, 1253, quantity, (facilityMapBL.FacilityMapDTO.FacilityCapacity - bookedQty), facilityMapBL.FacilityMapDTO.FacilityCapacity);
                //            log.Error(message);
                //            log.LogMethodExit(message);
                //            throw new ValidationException(message);
                //        }
                //    }
                //}
            }
            else
            {
                log.Error("Invalid scheudle info - scheduleId: " + transactionReservationScheduleDTO.SchedulesId.ToString() + "FacilityMapId: " + transactionReservationScheduleDTO.FacilityMapId.ToString() + "scheduleFrom: " + scheduleFrom.ToString() + " scheduleTo:" + toTime.ToString());
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2140, scheduleFrom.ToString(utilities.ParafaitEnv.DATETIME_FORMAT), scheduleTo.ToString(utilities.ParafaitEnv.DATETIME_FORMAT)));
                // "Schedule " + scheduleFrom.ToString(utilities.ParafaitEnv.DATETIME_FORMAT) + " to " + scheduleTo.ToString(utilities.ParafaitEnv.DATETIME_FORMAT) + "is not valid"
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Open reservation for edit
        /// </summary>
        /// <param name="sqlTrx"></param>
        public void EditReservation(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            DTONullCheck();
            if (this.reservationDTO.Status != ReservationDTO.ReservationStatus.BOOKED.ToString()
               && this.reservationDTO.Status != ReservationDTO.ReservationStatus.CONFIRMED.ToString())
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2141, this.reservationDTO.Status));// "Sorry, you cannot edit the booking when it is in " + this.reservationDTO.Status + " status"
            }
            int bookingId = -1;
            ParafaitDBTransaction dBTransaction = null;
            try
            {
                if (sqlTrx == null)
                {
                    dBTransaction = new ParafaitDBTransaction();
                    dBTransaction.BeginTransaction();
                    sqlTrx = dBTransaction.SQLTrx;
                }
                bookingId = this.reservationDTO.BookingId;
                this.reservationDTO.Status = ReservationDTO.ReservationStatus.WIP.ToString();
                this.Save(sqlTrx);
                //Booking with Reservation Code &1 is opened for edit.
                audit.logEvent(MessageContainerList.GetMessage(executionContext, "Reservation"), 'D', executionContext.GetUserId(),
                    MessageContainerList.GetMessage(executionContext, 2142, reservationDTO.ReservationCode),
                    MessageContainerList.GetMessage(executionContext, "EditBooking"), 0, "", reservationDTO.Guid.ToString(), sqlTrx);

                LoadTransactionDetails(sqlTrx);
                LoadBeforeEditDetails(sqlTrx);

                if (dBTransaction != null)
                {
                    dBTransaction.EndTransaction();
                    sqlTrx = null;
                    dBTransaction.Dispose();
                }
            }
            catch (Exception ex)
            {
                if (dBTransaction != null)
                {
                    dBTransaction.RollBack();
                    dBTransaction.Dispose();
                    sqlTrx = null;
                }
                log.Error(ex);
                throw;
            }
            log.LogMethodExit();
        }



        public void ConfirmReservation(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            DTONullCheck();
            if (this.reservationDTO.Status != ReservationDTO.ReservationStatus.BOOKED.ToString())
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2143, this.reservationDTO.Status));// "Sorry, you cannot confirm the booking when it is in " + this.reservationDTO.Status + " status"
            }
            int bookingId = -1;
            ParafaitDBTransaction dBTransaction = null;
            try
            {
                if (sqlTrx == null)
                {
                    dBTransaction = new ParafaitDBTransaction();
                    dBTransaction.BeginTransaction();
                    sqlTrx = dBTransaction.SQLTrx;
                }
                bookingId = this.reservationDTO.BookingId;
                this.reservationDTO.Status = ReservationDTO.ReservationStatus.CONFIRMED.ToString();
                SetReservationCheckList(sqlTrx);
                this.Save(sqlTrx);
                SignWaiverEmail signWaiverEmail = new SignWaiverEmail(this.executionContext, this.BookingTransaction, this.utilities);
                signWaiverEmail.SendWaiverSigningLink(sqlTrx);
                //'Booking is Confirmed. Reservation code is &1'
                audit.logEvent(MessageContainerList.GetMessage(executionContext, "Reservation"), 'D', executionContext.GetUserId(),
                    MessageContainerList.GetMessage(executionContext, 2144, reservationDTO.ReservationCode),
                    MessageContainerList.GetMessage(executionContext, "Confirm Reservation"), 0, "", reservationDTO.Guid.ToString(), sqlTrx);
                //throw new Exception("test");
                if (dBTransaction != null)
                {
                    dBTransaction.EndTransaction();
                    sqlTrx = null;
                    dBTransaction.Dispose();
                }
            }
            catch (Exception ex)
            {
                if (dBTransaction != null)
                {
                    dBTransaction.RollBack();
                    dBTransaction.Dispose();
                    sqlTrx = null;
                }
                log.Error(ex);
                throw;
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Cancel reservation
        /// </summary>
        /// <param name="sqlTrx"></param>
        public void CancelReservation(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            if (this.reservationDTO != null)
            {
                if (this.reservationDTO.Status == ReservationDTO.ReservationStatus.CANCELLED.ToString() ||
                    this.reservationDTO.Status == ReservationDTO.ReservationStatus.SYSTEMABANDONED.ToString() ||
                    this.reservationDTO.Status == ReservationDTO.ReservationStatus.COMPLETE.ToString()
                  )
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2145, this.reservationDTO.Status));//Sorry, cannot cancel the reservation when it is in " + this.reservationDTO.Status + " status
                }

                if (this.reservationDTO.Status == ReservationDTO.ReservationStatus.WIP.ToString())
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2146));//Sorry, save the reservation changes first to proceed with reservation cancellation
                }

                if (this.reservationDTO.Status == ReservationDTO.ReservationStatus.NEW.ToString())
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2147));//"Sorry, reservation is not yet saved"
                }
                ParafaitDBTransaction dBTransaction = null;
                int bookingId = this.reservationDTO.BookingId;
                try
                {
                    if (sqlTrx == null)
                    {
                        dBTransaction = new ParafaitDBTransaction();
                        dBTransaction.BeginTransaction();
                        sqlTrx = dBTransaction.SQLTrx;
                    }
                    CancelReservationTransaction(sqlTrx);

                    if (dBTransaction != null)
                    {
                        dBTransaction.EndTransaction();
                        sqlTrx = null;
                        dBTransaction.Dispose();
                    }

                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    if (dBTransaction != null)
                    {
                        dBTransaction.RollBack();
                        dBTransaction.Dispose();
                        sqlTrx = null;
                    }
                    throw;
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Save the cancelled reservation
        /// </summary>
        /// <param name="sqlTrx"></param>
        public void SaveCancelledReservation(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            if (this.reservationDTO != null)
            {
                ParafaitDBTransaction dBTransaction = null;
                try
                {
                    if (sqlTrx == null)
                    {
                        dBTransaction = new ParafaitDBTransaction();
                        dBTransaction.BeginTransaction();
                        sqlTrx = dBTransaction.SQLTrx;
                    }
                    this.reservationDTO.Status = ReservationDTO.ReservationStatus.CANCELLED.ToString();
                    this.Save(sqlTrx);
                    audit.logEvent(MessageContainerList.GetMessage(executionContext, "Reservation"), 'D', executionContext.GetUserId(),
                        MessageContainerList.GetMessage(executionContext, "Booking is Cancelled"),
                        MessageContainerList.GetMessage(executionContext, "CancelBooking"), 0, "", reservationDTO.Guid.ToString(), sqlTrx);
                    if (dBTransaction != null)
                    {
                        dBTransaction.EndTransaction();
                        sqlTrx = null;
                        dBTransaction.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    if (dBTransaction != null)
                    {
                        dBTransaction.RollBack();
                        dBTransaction.Dispose();
                        sqlTrx = null;
                    }
                    throw;
                }
                BuildReservationDetails(sqlTrx);
                LoadBeforeEditDetails(sqlTrx);
            }
            log.LogMethodExit();
        }

        private void CancelReservationTransaction(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            DTONullCheck();
            if (this.bookingTransaction != null)
            {
                this.bookingTransaction.CancelReservationTransaction(sqlTrx);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Get cancellation products for the booking
        /// </summary>
        /// <param name="sqlTrx"></param>
        /// <returns></returns>
        public List<ProductsDTO> GetCancellationChargeProducts(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry();
            DTONullCheck();
            List<ProductsDTO> cancellationChargeProducts = null;
            if (ReservationTransactionIsNotNull())
            {
                cancellationChargeProducts = this.bookingTransaction.GetBookingCancellationChargeProducts(sqlTrx);
            }
            log.LogMethodExit(cancellationChargeProducts);
            return cancellationChargeProducts;
        }


        /// <summary>
        /// Check whether cancellation charge is received or not
        /// </summary>
        /// <param name="sqlTrx"></param>
        /// <returns></returns>
        public bool HasCancellationChargeReceived(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry();
            bool cancellationChargeReceived = true;
            DTONullCheck();
            if (this.bookingTransaction != null && this.bookingTransaction.TotalPaidAmount != 0)
            {
                cancellationChargeReceived = this.bookingTransaction.HasBookingCancellationCharge(sqlTrx);
            }
            log.LogMethodExit(cancellationChargeReceived);
            return cancellationChargeReceived;
        }


        /// <summary>
        /// Execute Reservation Transation
        /// </summary>
        /// <param name="cardList"></param>
        /// <param name="progress"></param>
        /// <param name="completeTransaction"></param>
        public void ExecuteReservationTransaction(Dictionary<string, string> cardList, IProgress<ProgressReport> progress, bool completeTransaction = false) //Never pass completeTransaction as true from POS. KOT print will get fired.
        {
            log.LogMethodEntry(cardList, completeTransaction);
            string message = "";
            DTONullCheck();
            if (this.reservationDTO.Status != ReservationDTO.ReservationStatus.CONFIRMED.ToString())
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2149));// "Sorry, only confirmed reservation can be executed"
            }
            if (this.bookingTransaction == null)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2150));// "Sorry, cannot proceed as there is no transaction created for the booking"
            }

            bool trxReLoaded = false;
            using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
            {
                int bookingId = this.reservationDTO.BookingId;
                try
                {
                    parafaitDBTrx.BeginTransaction();
                    log.Debug("Start trx ExecuteReservationTransaction");
                    this.bookingTransaction.ExecuteReservationTransaction(cardList, progress, parafaitDBTrx.SQLTrx, completeTransaction);
                    log.Debug("End trx ExecuteReservationTransaction");
                    bool iswaiverPending = this.bookingTransaction.IsWaiverSignaturePending();
                    log.LogVariableState("iswaiverPending", iswaiverPending);
                    LoadTransactionDetails(parafaitDBTrx.SQLTrx);
                    trxReLoaded = true;
                    bool hasTempCards = HasTempCards();
                    if (iswaiverPending == false
                        && hasTempCards == false)
                    {
                        this.reservationDTO.Status = ReservationDTO.ReservationStatus.COMPLETE.ToString();
                    }
                    log.Debug("Start reservation Save");
                    progress.Report(new ProgressReport(95, MessageContainerList.GetMessage(executionContext, "Saving execute transaction changes")));
                    this.Save(parafaitDBTrx.SQLTrx);
                    log.Debug("End reservation Save");
                    if (completeTransaction && iswaiverPending == false && hasTempCards == false)
                    {
                        if (this.bookingTransaction.CompleteTransaction(parafaitDBTrx.SQLTrx, ref message) == false)
                        {
                            log.LogVariableState("Error message ", message);
                            message = MessageContainerList.GetMessage(executionContext, 526);
                            log.LogMethodExit();
                            throw new Exception(message);
                        }
                    }
                    progress.Report(new ProgressReport(99, MessageContainerList.GetMessage(executionContext, "Saving execute transaction changes")));
                    if (iswaiverPending)
                    {
                        // "Booking is partially executed due to pending waivers"
                        audit.logEvent(MessageContainerList.GetMessage(executionContext, "Reservation"), 'D', executionContext.GetUserId(),
                            MessageContainerList.GetMessage(executionContext, 2421),
                            MessageContainerList.GetMessage(executionContext, "BookingExecution"), 0, "", reservationDTO.Guid.ToString(), parafaitDBTrx.SQLTrx);
                    }
                    else if (hasTempCards)
                    {
                        // "Booking is partially executed due to pending temp cards"
                        audit.logEvent(MessageContainerList.GetMessage(executionContext, "Reservation"), 'D', executionContext.GetUserId(),
                            MessageContainerList.GetMessage(executionContext, 4146),
                            MessageContainerList.GetMessage(executionContext, "BookingExecution"), 0, "", reservationDTO.Guid.ToString(), parafaitDBTrx.SQLTrx);
                    }
                    else
                    {
                        audit.logEvent(MessageContainerList.GetMessage(executionContext, "Reservation"), 'D', executionContext.GetUserId(),
                            MessageContainerList.GetMessage(executionContext, "Booking is Executed"),
                            MessageContainerList.GetMessage(executionContext, "BookingExecution"), 0, "", reservationDTO.Guid.ToString(), parafaitDBTrx.SQLTrx);
                    }
                    parafaitDBTrx.EndTransaction();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    if (parafaitDBTrx.SQLTrx.Connection != null)
                    { parafaitDBTrx.RollBack(); }
                    parafaitDBTrx.Dispose();
                    log.LogVariableState("message ", message);
                    log.LogMethodExit();
                    trxReLoaded = false;
                    throw;
                }
                finally
                {
                    parafaitDBTrx.Dispose();
                }
            }
            if (trxReLoaded == false)
            {
                LoadTransactionDetails(null);
            }
            log.LogVariableState("message ", message);
            log.LogMethodExit();
        }
        /// <summary>
        /// Block reservation
        /// </summary>
        /// <param name="sqlTrx"></param>
        public void BlockReservationSchedule(SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(sqlTrx);
            if (this.reservationDTO != null)
            {
                if (reservationDTO.Status != ReservationDTO.ReservationStatus.NEW.ToString() && reservationDTO.Status != ReservationDTO.ReservationStatus.BLOCKED.ToString())
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2151));//Sorry, only new booking can be blocked

                }
                if (ReservationTransactionIsNotNull() == false)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2150));// "Sorry, cannot proceed as there is no transaction created for the booking"
                }

                HasValidSchedule();
                HasBookingProduct();
                ValidateScheduleNQuantity();
                int blockBookingForXMinutes = GetBlockBookingExpiryMinutes();
                ParafaitDBTransaction dBTransaction = null;
                try
                {
                    if (sqlTrx == null)
                    {
                        dBTransaction = new ParafaitDBTransaction();
                        dBTransaction.BeginTransaction();
                        sqlTrx = dBTransaction.SQLTrx;
                    }
                    this.reservationDTO.ExpiryTime = utilities.getServerTime().AddMinutes(blockBookingForXMinutes);
                    this.reservationDTO.Status = ReservationDTO.ReservationStatus.BLOCKED.ToString();
                    this.Save(sqlTrx);
                    //'Booking is blocked successfully, Reservation code  is &1'
                    audit.logEvent(MessageContainerList.GetMessage(executionContext, "Reservation"), 'D', executionContext.GetUserId(),
                        MessageContainerList.GetMessage(executionContext, 2153, reservationDTO.ReservationCode),
                        MessageContainerList.GetMessage(executionContext, "BlockReservation"), 0, "", reservationDTO.Guid.ToString(), sqlTrx);
                    // throw new Exception("test");
                    if (dBTransaction != null)
                    {
                        dBTransaction.EndTransaction();
                        sqlTrx = null;
                        dBTransaction.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    if (dBTransaction != null)
                    {
                        dBTransaction.RollBack();
                        dBTransaction.Dispose();
                        sqlTrx = null;
                    }
                    throw;
                }
            }
            log.LogMethodExit();
        }

        private int GetBlockBookingExpiryMinutes()
        {
            log.LogMethodEntry();
            ReservationDefaultSetup reservationDefaultSetup = new ReservationDefaultSetup(executionContext);
            int blockBookingForXMinutes = reservationDefaultSetup.GetMinutesForBlockReservation;
            log.LogMethodExit(blockBookingForXMinutes);
            return blockBookingForXMinutes;
        }



        private void ReservationIsInEditMode()
        {
            log.LogMethodEntry();
            DTONullCheck();
            if (this.reservationDTO.Status != ReservationDTO.ReservationStatus.NEW.ToString()
                && this.reservationDTO.Status != ReservationDTO.ReservationStatus.WIP.ToString()
                && this.reservationDTO.Status != ReservationDTO.ReservationStatus.BLOCKED.ToString())
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2141, this.reservationDTO.Status));//"Sorry, you cannot edit the booking when it is in &1 status
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Add customer to reservation transaction
        /// </summary>
        /// <param name="customerDTO"></param>
        public void AddCustomer(CustomerDTO customerDTO)
        {
            log.LogMethodEntry(customerDTO);
            DTONullCheck();
            ReservationIsInEditMode();
            this.bookingTransaction.AddCustomer(customerDTO);
            this.reservationDTO.CustomerId = customerDTO.Id;
            this.reservationDTO.CustomerName = customerDTO.FirstName;
            log.LogMethodExit();
        }

        /// <summary>
        /// Add attendee to the reservation
        /// </summary>
        /// <param name="bookingAttendeesDTO"></param>
        public void AddUpdateReservationAttendees(BookingAttendeeDTO bookingAttendeesDTO)
        {
            log.LogMethodEntry(bookingAttendeesDTO);
            DTONullCheck();
            ReservationIsInEditMode();
            if (this.bookingTransaction != null)
            {
                this.bookingTransaction.AddUpdateReservationAttendees(bookingAttendeesDTO);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Remove attendee from the reservation
        /// </summary>
        /// <param name="bookingAttendeesDTO"></param>
        public void RemoveReservationAttendees(BookingAttendeeDTO bookingAttendeesDTO)
        {
            log.LogMethodEntry(bookingAttendeesDTO);
            DTONullCheck();
            ReservationIsInEditMode();
            if (this.bookingTransaction != null)
            {
                this.bookingTransaction.RemoveReservationAttendees(bookingAttendeesDTO);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves attendee details only
        /// </summary>
        /// <param name="sqlTrx"></param>
        public void SaveReservationAttendeesOnly(SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(sqlTrx);
            if (this.reservationDTO == null || this.bookingTransaction == null)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2154, MessageContainerList.GetMessage(executionContext, "Reservation")));//"Sorry, cannot proceed. &1 details are missing"
            }
            if (this.reservationDTO.BookingId > -1)
            {
                ReservationIsInEditMode();
                this.bookingTransaction.SaveReservationAttendeesOnly(this.reservationDTO.BookingId, this.reservationDTO.Guid, sqlTrx);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves attendee details only
        /// </summary>
        /// <param name="sqlTrx"></param>
        public void SaveReservationCheckListOnly(SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(sqlTrx);
            if (this.reservationDTO == null)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2154, MessageContainerList.GetMessage(executionContext, "Reservation")));//"Sorry, cannot proceed. &1 details are missing"
            }
            if (this.reservationDTO.BookingId > -1
                && reservationDTO.BookingCheckListDTOList != null && reservationDTO.BookingCheckListDTOList.Any())
            {
                ReservationIsInEditMode();
                ParafaitDBTransaction dBTransaction = null;
                try
                {
                    if (sqlTrx == null)
                    {
                        dBTransaction = new ParafaitDBTransaction();
                        dBTransaction.BeginTransaction();
                        sqlTrx = dBTransaction.SQLTrx;
                    }
                    SaveBookingCheckList(sqlTrx);
                    if (dBTransaction != null)
                    {
                        dBTransaction.EndTransaction();
                        sqlTrx = null;
                        dBTransaction.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    if (dBTransaction != null)
                    {
                        dBTransaction.RollBack();
                        dBTransaction.Dispose();
                        sqlTrx = null;
                    }
                    throw;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Get Reservation AttendeeList
        /// </summary> 
        public List<BookingAttendeeDTO> GetReservationAttendeeList()
        {
            log.LogMethodEntry();
            List<BookingAttendeeDTO> attendeDTOList = null;
            if (this.bookingTransaction != null)
            {
                attendeDTOList = this.bookingTransaction.BookingAttendeeList;
            }
            log.LogMethodExit(attendeDTOList);
            return attendeDTOList;
        }

        /// <summary>
        /// Check whether reservation has any active attendees
        /// </summary>
        /// <returns></returns>
        public bool ReservationHasActiveAttendeeList()
        {
            log.LogMethodEntry();
            bool hasList = false;
            if (this.BookingTransaction != null)
            {
                hasList = this.BookingTransaction.HasActiveAttendeeList();
            }
            log.LogMethodExit(hasList);
            return hasList;
        }

        private void InitiateTransaction()
        {
            log.LogMethodEntry();
            if (this.bookingTransaction == null)
            {
                this.bookingTransaction = new Transaction(this.utilities, this.reservationDTO);
            }
            log.LogMethodExit();
        }
        private Card InitateCardObject()
        {
            log.LogMethodEntry();
            Card card = null;
            if (this.reservationDTO.CardId > -1)
            {
                card = new Card(this.reservationDTO.CardId, executionContext.GetUserId(), this.utilities);
            }
            log.LogMethodExit(card);
            return card;
        }
        /// <summary>
        /// Check whether reservation has booking product. Throw exception if booking product is not found
        /// </summary>




        /// <summary>
        /// Check whether schedule is already added to the booking or not
        /// </summary>
        /// <param name="scheduleId"></param>
        /// <param name="facilityMapId"></param>
        public void AlreadyAddedSchedule(int scheduleId, int facilityMapId)
        {
            log.LogMethodEntry(scheduleId, facilityMapId);
            if (this.reservationDTO != null && this.bookingTransaction != null)
            {
                bool alreadyPresent = this.bookingTransaction.HasThisSchedule(scheduleId, facilityMapId);
                log.LogVariableState("alreadyPresent", alreadyPresent);
                if (alreadyPresent)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2160));//"Selected schedule is already added"
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Check whether Booking product is already added to the booking or not
        /// </summary>
        /// <param name="bookingProductId"></param>
        public void AlreadyAddedBookingProduct(int bookingProductId)
        {
            log.LogMethodEntry(bookingProductId);
            if (this.reservationDTO != null && this.bookingTransaction != null)
            {
                bool alreadyPresent = this.bookingTransaction.HasThisBookingProduct(bookingProductId);
                log.LogVariableState("alreadyPresent", alreadyPresent);
                if (alreadyPresent)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2230));// 2161-"Selected booking product is already added"
                    //2230,'Only one booking product can be added to the reservation'
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Add rental product and schedule details to the booking
        /// </summary>
        /// <param name="rentalProductId"></param>
        /// <param name="transactionReservationScheduleDTO"></param>
        /// <returns></returns>
        public Transaction.TransactionLine AddFacilityRentalProduct(int rentalProductId, TransactionReservationScheduleDTO transactionReservationScheduleDTO)
        {
            log.LogMethodEntry(rentalProductId, transactionReservationScheduleDTO);
            ReservationIsInEditMode();
            InitiateTransaction();
            string msg = "";
            AlreadyAddedSchedule(transactionReservationScheduleDTO.SchedulesId, transactionReservationScheduleDTO.FacilityMapId);
            RentalIsAlreadyAddedToTheSchedule(transactionReservationScheduleDTO.SchedulesId, transactionReservationScheduleDTO.FacilityMapId, rentalProductId);

            Card card = InitateCardObject();
            //TransactionReservationScheduleBL transactionReservationScheduleBL = new TransactionReservationScheduleBL(executionContext, transactionReservationScheduleDTO);
            //transactionReservationScheduleBL.Save();
            Transaction.TransactionLine outTrxLine = new Transaction.TransactionLine();
            if (this.bookingTransaction.createTransactionLine(card, rentalProductId, transactionReservationScheduleDTO, outTrxLine, -1, 1, ref msg) != 0)
            {
                if (outTrxLine != null && outTrxLine.TransactionReservationScheduleDTOList != null && outTrxLine.TransactionReservationScheduleDTOList.Any())
                {
                    TransactionReservationScheduleDTO trsDTO = outTrxLine.GetCurrentTransactionReservationScheduleDTO();
                    int index = outTrxLine.TransactionReservationScheduleDTOList.IndexOf(trsDTO);
                    if (trsDTO != null)
                    {
                        TransactionReservationScheduleBL transactionReservationScheduleBL = new TransactionReservationScheduleBL(executionContext, trsDTO);
                        transactionReservationScheduleBL.ExpireSchedule(null);
                        outTrxLine.TransactionReservationScheduleDTOList.RemoveAt(index);
                    }
                }
                log.Error(msg);
                throw new Exception(msg);
            }

            log.LogMethodExit(outTrxLine);
            return outTrxLine;
        }

        /// <summary>
        /// Add booking product to the reservation
        /// </summary>
        /// <param name="bookingProductId"></param>
        /// <param name="parentTrxLine"></param>
        public void AddBookingProduct(int bookingProductId, Transaction.TransactionLine parentTrxLine)
        {
            log.LogMethodEntry(bookingProductId, parentTrxLine);
            ReservationIsInEditMode();
            InitiateTransaction();
            AlreadyAddedBookingProduct(bookingProductId);
            string msg = "";
            Card card = InitateCardObject();
            if (this.bookingTransaction.createTransactionLine(card, bookingProductId, -1, 1, parentTrxLine, ref msg) != 0)
            {
                log.Error(msg);
                throw new Exception(msg);
            }
            Transaction.TransactionLine bookingProductTrxLine = GetBookingProductTransactionLine();
            this.reservationDTO.BookingProductId = bookingProductTrxLine.ProductID;
            this.reservationDTO.BookingProductName = bookingProductTrxLine.ProductName;
            this.reservationDTO.Quantity = GetGuestQuantity();
            log.LogMethodExit();
        }

        /// <summary>
        /// Add cancellation charge product
        /// </summary>
        /// <param name="productList"></param>
        /// <param name="addCashPayment"></param>
        public void AddCancellationProduct(List<KeyValuePair<int, double>> productList, bool addCashPayment)
        {
            log.LogMethodEntry(productList);
            DTONullCheck();
            if (this.reservationDTO.Status != ReservationDTO.ReservationStatus.BOOKED.ToString()
                && this.reservationDTO.Status != ReservationDTO.ReservationStatus.CONFIRMED.ToString())
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2162));//Sorry, cannot to proceed. Bookings should be in Booked Or Confirmed statu
            }
            if (this.bookingTransaction != null && productList != null && productList.Count > 0)
            {
                Card card = InitateCardObject();
                this.bookingTransaction.AddReservationCancellationProduct(card, productList, addCashPayment);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Remove product line
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="lineId"></param>
        /// <param name="sqlTrx"></param>
        /// <param name="saveTrx"></param>
        public void RemoveProduct(int productId, int lineId, SqlTransaction sqlTrx = null, bool saveTrx = true)
        {
            log.LogMethodEntry(productId, lineId, sqlTrx, saveTrx);
            ReservationIsInEditMode();
            if (this.bookingTransaction != null)
            {
                if (this.bookingTransaction.TrxLines != null)
                {
                    Transaction.TransactionLine lineToBeCancelled = this.bookingTransaction.TrxLines[lineId];
                    if (lineToBeCancelled != null)
                    {
                        if (lineToBeCancelled.CancelledLine == false && lineToBeCancelled.LineValid == true)
                        {
                            this.bookingTransaction.CancelTransactionLine(lineId, sqlTrx, saveTrx);
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// RemoveBookingProduct and set reservation quantity as zero
        /// </summary>
        /// <param name="bookingProductId"></param>
        /// <param name="lineId"></param>
        public void RemoveBookingProduct(int bookingProductId, int lineId)
        {
            log.LogMethodEntry(bookingProductId, lineId);
            ReservationIsInEditMode();
            RemoveProduct(bookingProductId, lineId);
            log.LogVariableState("this.reservationDTO.Quantity", this.reservationDTO.Quantity);
            this.reservationDTO.Quantity = 0;
            log.LogMethodExit();
        }

        //public void ReduceProductQty(int qtyToBeReduced, int productId, int comboProductId)
        //{
        //    log.LogMethodEntry(qtyToBeReduced, productId, comboProductId);
        //    ReservationIsInEditMode();
        //    if (this.bookingTransaction != null)
        //    {
        //        if (this.bookingTransaction.TrxLines != null)
        //        {
        //            List<int> trxLineIdList = GetTrxLineIDList(productId, comboProductId);
        //            if (trxLineIdList != null)
        //            {
        //                // trxLineIdList = trxLineIdList.OrderByDescending(lineId => lineId ).ToList();
        //                log.LogVariableState("trxLineIdList", trxLineIdList);
        //                //cancel new entries first
        //                for (int lineIdToBeCancelled = trxLineIdList.Count - 1; lineIdToBeCancelled >= 0; lineIdToBeCancelled--)
        //                {
        //                    if (qtyToBeReduced > 0)
        //                    {
        //                        this.bookingTransaction.cancelLine(lineIdToBeCancelled);
        //                        qtyToBeReduced--;
        //                    }
        //                    else
        //                    {
        //                        //if (qtyToBeReduced == 0)
        //                        break;
        //                    }
        //                }

        //            }
        //        }
        //    }
        //    log.LogMethodExit();
        //}

        /// <summary>
        /// Apply discount
        /// </summary>
        /// <param name="discountId"></param>
        /// <param name="productId"></param>
        /// <param name="comboProductId"></param>
        /// <param name="variableAmount"></param>
        public void ApplyDiscounts(int discountId, int productId, int comboProductId, decimal? variableAmount = null, int approvedBy = -1)
        {
            log.LogMethodEntry(discountId, productId, comboProductId, variableAmount);
            try
            {
                ReservationIsInEditMode();
                if (productId > -1 && comboProductId > -1)
                {
                    List<int> trxLineIdList = this.bookingTransaction.GetTrxLineIDList(productId, comboProductId);
                    if (trxLineIdList != null)
                    {
                        log.LogVariableState("trxLineIdList", trxLineIdList);
                        for (int i = trxLineIdList.Count - 1; i >= 0; i--)
                        {
                            this.bookingTransaction.ApplyDiscount(discountId, null, -1, variableAmount, this.bookingTransaction.TrxLines[trxLineIdList[i]]);
                        }
                    }
                }
                else if (productId == -1 && comboProductId == -1)
                {
                    this.bookingTransaction.ApplyDiscount(discountId, null, -1, variableAmount);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Un apply the discount
        /// </summary>
        /// <param name="discountId"></param>
        /// <param name="productId"></param>
        /// <param name="comboProductId"></param>
        public void UnApplyDiscounts(int discountId, int productId, int comboProductId)
        {
            log.LogMethodEntry(discountId, productId, comboProductId);
            ReservationIsInEditMode();
            if (productId > -1 && comboProductId > -1)
            {
                List<int> trxLineIdList = this.bookingTransaction.GetTrxLineIDList(productId, comboProductId);
                if (trxLineIdList != null)
                {
                    log.LogVariableState("trxLineIdList", trxLineIdList);
                    for (int lineIDToUnApplyDiscount = trxLineIdList.Count - 1; lineIDToUnApplyDiscount >= 0; lineIDToUnApplyDiscount--)
                    {
                        if (this.bookingTransaction.CancelDiscountLine(discountId, trxLineIdList[lineIDToUnApplyDiscount]) == false)
                        {
                            string msg = MessageContainerList.GetMessage(executionContext, 2163);// "Unable to un-apply the discount"
                            log.Error(msg);
                            log.LogMethodExit(msg);
                            throw new ValidationException(msg);
                        }
                    }
                }
            }
            else if (productId == -1 && comboProductId == -1)
            {
                if (this.bookingTransaction.cancelDiscountLine(discountId) == false)
                {
                    string msg = MessageContainerList.GetMessage(executionContext, 2163);
                    log.Error(msg);
                    log.LogMethodExit(msg);
                    throw new ValidationException(msg);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Apply discount coupon
        /// </summary>
        /// <param name="discountCoupon"></param>
        public void ApplyDiscountCoupon(string discountCoupon)
        {
            log.LogMethodEntry(discountCoupon);
            ReservationIsInEditMode();
            if (string.IsNullOrEmpty(discountCoupon) == false)
            {
                this.bookingTransaction.ApplyCoupon(discountCoupon);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// GetAppliedDiscountInfo 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="comboProductId"></param>
        /// <returns></returns>
        public List<DiscountContainerDTO> GetAppliedDiscountInfo(int productId, int comboProductId)
        {
            log.LogMethodEntry(productId, comboProductId);
            List<DiscountContainerDTO> appliedDiscountDTOList = null;
            if (ReservationTransactionIsNotNull())
            {
                appliedDiscountDTOList = this.bookingTransaction.GetReservationTransactionAppliedDiscountInfo(productId, comboProductId);
            }
            log.LogMethodExit(appliedDiscountDTOList);
            return appliedDiscountDTOList;
        }

        private List<DiscountContainerDTO> GetOrginalAppliedDiscountInfo(int productId, int comboProductId)
        {
            log.LogMethodEntry(productId, comboProductId);
            List<DiscountContainerDTO> appliedDiscountContainerDTOList = null;
            if (this.reservationDTOBeforeEdit != null && this.bookingTransactionBeforeEdit != null)
            {
                appliedDiscountContainerDTOList = this.bookingTransactionBeforeEdit.GetReservationTransactionAppliedDiscountInfo(productId, comboProductId);
            }
            log.LogMethodExit(appliedDiscountContainerDTOList);
            return appliedDiscountContainerDTOList;
        }

        /// <summary>
        /// ApplyTransactionProfile
        /// </summary>
        /// <param name="transactionProfileId"></param>
        /// <param name="productId"></param>
        /// <param name="comboProductId"></param>
        /// <param name="userVerificationId"></param>
        /// <param name="userVerificationName"></param>
        public void ApplyTransactionProfile(int transactionProfileId, int productId, int comboProductId, string userVerificationId, string userVerificationName)
        {
            log.LogMethodEntry(transactionProfileId, productId, comboProductId, userVerificationId, userVerificationName);
            try
            {
                ReservationIsInEditMode();
                this.bookingTransaction.ApplyTransactionProfile(transactionProfileId, productId, comboProductId, userVerificationId, userVerificationName);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit();
        }

        ///// <summary>
        ///// AddServiceCharges
        ///// </summary>
        ///// <param name="chargeAmount"></param>
        ///// <param name="chargePercentage"></param>
        //public void AddServiceCharges(double chargeAmount, double chargePercentage)
        //{
        //    log.LogMethodEntry(chargeAmount, chargePercentage);
        //    try
        //    {
        //        ReservationIsInEditMode();
        //        if (this.bookingTransaction != null)
        //        {
        //            Card card = null;
        //            if (this.reservationDTO.CardId > -1)
        //            {
        //                card = new Card(this.reservationDTO.CardId, executionContext.GetUserId(), utilities);
        //            }
        //            this.bookingTransaction.AddServiceCharges(card, chargeAmount, chargePercentage);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex);
        //        throw;
        //    }
        //    log.LogMethodExit();
        //}

        /// <summary>
        /// RemoveServiceCharges
        /// </summary>
        public void RemoveManuallyAppliedServiceCharges(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            try
            {
                ReservationIsInEditMode();
                if (this.bookingTransaction != null)
                {
                    CallTrxRemoveManuallyAppliedServiceCharges(sqlTrx);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit();
        }

        private void CallTrxRemoveManuallyAppliedServiceCharges(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            this.bookingTransaction.CancelServiceChargeLine(sqlTrx);
            this.reservationDTO.ServiceChargeAmount = 0;
            this.reservationDTO.ServiceChargePercentage = 0;
            log.LogMethodExit();
        }

        #region AddProduct and its methods
        /// <summary>
        /// AddProduct
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="price"></param>
        /// <param name="quantity"></param>
        /// <param name="comboProductId"></param>
        /// <param name="parentTrxLine1"></param>
        /// <param name="purchasedProductList"></param>
        /// <param name="atbList"></param>
        /// <param name="categoryProducts"></param>
        public void AddProduct(int productId, double price, int quantity, int comboProductId, Transaction.TransactionLine parentTrxLine1, List<KeyValuePair<int, PurchasedProducts>> purchasedProductList, List<AttractionBooking> atbList, List<ReservationDTO.SelectedCategoryProducts> categoryProducts)
        {
            log.LogMethodEntry(productId, price, quantity, comboProductId, purchasedProductList, atbList, parentTrxLine1);
            ReservationIsInEditMode();
            InitiateTransaction();
            Card card = InitateCardObject();
            ComboProductBL comboProductBL = GetComboProductBL(comboProductId);
            if (comboProductBL.ComboProductDTO == null || comboProductBL.ComboProductDTO.ComboProductId == -1)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2164));//Sorry, cannot to proceed. Unable to find product detail in booking product setup
            }
            else
            {
                Products productBL = GetProductsBL(productId);
                if (productBL.GetProductsDTO == null || productBL.GetProductsDTO.ProductId == -1)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2165));//"Sorry, cannot to proceed. Unable to find product details"
                }
                int quantityFromSetup = 0;
                int additionalQuantity = 0;
                if (comboProductBL.ComboProductDTO.AdditionalProduct == false)
                {
                    if (comboProductBL.ComboProductDTO.PriceInclusive)
                    {
                        quantityFromSetup = (comboProductBL.ComboProductDTO.Quantity == null ? 0 : (int)comboProductBL.ComboProductDTO.Quantity);
                        int[] quantityInfo = GetQtyAndAdditionalQtyDetails(productId, comboProductId, quantity, quantityFromSetup);
                        quantity = quantityInfo[0];
                        additionalQuantity = quantityInfo[1];
                        price = 0;
                    }
                    else
                    {
                        price = -1;
                    }
                }
                else
                {
                    price = -1; //additional products
                }
                bool savIsClientServer = utilities.ParafaitEnv.IsClientServer;
                try
                {
                    utilities.ParafaitEnv.IsClientServer = false;
                    if (productBL.GetProductsDTO.ProductType == ProductTypeValues.COMBO)
                    {
                        CreateComboProductLine(productId, card, price, quantity, additionalQuantity, atbList, categoryProducts, purchasedProductList, parentTrxLine1, comboProductId);
                    }
                    else if (productBL.GetProductsDTO.ProductType == ProductTypeValues.ATTRACTION)
                    {
                        CreateAttractionProductLine(atbList, productBL, price, quantity, additionalQuantity, parentTrxLine1, comboProductId);
                    }
                    else if (productBL.GetProductsDTO.ProductType != ProductTypeValues.NEW
                            && productBL.GetProductsDTO.ProductType != ProductTypeValues.RECHARGE
                            && productBL.GetProductsDTO.ProductType != ProductTypeValues.GAMETIME
                            && productBL.GetProductsDTO.ProductType != ProductTypeValues.CARDSALE)
                    {
                        CreateNonCardTypeProductLine(productBL, card, price, quantity, additionalQuantity, purchasedProductList, parentTrxLine1, comboProductId);
                    }
                    else
                    {
                        CreateCardTypeProductLine(productBL, price, quantity, additionalQuantity, parentTrxLine1, comboProductId);
                    }
                }
                finally
                {
                    utilities.ParafaitEnv.IsClientServer = savIsClientServer;
                }
            }
            log.LogMethodExit();
        }

        private Products GetProductsBL(int productId)
        {
            log.LogMethodEntry(productId);
            Products productsBL;
            if (productsBLDictionary.ContainsKey(productId))
            {
                productsBL = productsBLDictionary[productId];
            }
            else
            {
                productsBL = new Products(executionContext, productId);
                productsBLDictionary.Add(productId, productsBL);
            }
            log.LogMethodExit(productsBL);
            return productsBL;
        }

        private ComboProductBL GetComboProductBL(int comboProductId)
        {
            log.LogMethodEntry(comboProductId);
            ComboProductBL comboProductBL;
            if (comboProductBLDictionary.ContainsKey(comboProductId))
            {
                comboProductBL = comboProductBLDictionary[comboProductId];
            }
            else
            {
                comboProductBL = new ComboProductBL(executionContext, comboProductId);
                comboProductBLDictionary.Add(comboProductId, comboProductBL);
            }
            log.LogMethodExit(comboProductBL);
            return comboProductBL;

        }

        /// <summary>
        /// Adding modifiers on child product
        /// </summary>
        /// <param name="selectedTrxLine"></param>
        /// <param name="purchasedProductList"></param>
        /// <param name="quantity"></param>
        public void AddManualChildProduct(Transaction.TransactionLine selectedTrxLine, List<KeyValuePair<int, PurchasedProducts>> purchasedProductList, int quantity)
        {
            log.LogMethodEntry(selectedTrxLine, purchasedProductList, quantity);
            ReservationIsInEditMode();
            //InitiateTransaction();
            Card card = InitateCardObject();
            int comboProductId = -1;
            if (selectedTrxLine.ComboproductId != -1)
            {
                comboProductId = selectedTrxLine.ComboproductId;
            }
            else
            {
                comboProductId = GetProductComboId(selectedTrxLine);
            }
            // if (parentTrxLine != null)
            {
                ComboProductBL comboProductBL = new ComboProductBL(executionContext, comboProductId);
                if (comboProductBL.ComboProductDTO == null || comboProductBL.ComboProductDTO.ComboProductId == -1)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2164));//Sorry, cannot to proceed. Unable to find product detail in booking product setup
                }
                else
                {
                    Products productBL = new Products(selectedTrxLine.ProductID);
                    if (productBL.GetProductsDTO == null || productBL.GetProductsDTO.ProductId == -1)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2165));//Sorry, cannot to proceed. Unable to find product details
                    }
                    double price = (double)productBL.GetProductsDTO.Price;
                    int quantityFromSetup = 0;
                    int additionalQuantity = 0;
                    if (comboProductBL.ComboProductDTO.AdditionalProduct == false)
                    {
                        if (comboProductBL.ComboProductDTO.PriceInclusive)
                        {
                            quantityFromSetup = (comboProductBL.ComboProductDTO.Quantity == null ? 0 : (int)comboProductBL.ComboProductDTO.Quantity);
                            int[] quantityInfo = GetQtyAndAdditionalQtyDetails(comboProductBL.ComboProductDTO.ChildProductId, comboProductBL.ComboProductDTO.ComboProductId, quantity, quantityFromSetup);
                            quantity = quantityInfo[0];
                            additionalQuantity = quantityInfo[1];
                            price = 0;
                        }
                        else
                        {
                            price = -1;
                        }
                    }
                    bool savIsClientServer = utilities.ParafaitEnv.IsClientServer;
                    try
                    {
                        utilities.ParafaitEnv.IsClientServer = false;
                        if (productBL.GetProductsDTO.ProductType == ProductTypeValues.MANUAL)
                        {
                            CreateNonCardTypeProductLine(productBL, card, price, quantity, additionalQuantity, purchasedProductList, selectedTrxLine.ParentLine, selectedTrxLine.ComboproductId);
                        }
                        else
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2166));//Sorry, cannot to proceed. Only manual product with modifier is allowed for this operation
                        }
                    }
                    finally
                    {
                        utilities.ParafaitEnv.IsClientServer = savIsClientServer;
                    }
                }
            }
            log.LogMethodExit();
        }

        private int GetProductComboId(Transaction.TransactionLine selectedTrxLine)
        {
            log.LogMethodEntry(selectedTrxLine);
            int comboProductId = -1;
            Transaction.TransactionLine parentTrxLine = selectedTrxLine.ParentLine;
            if (parentTrxLine != null)
            {
                if (parentTrxLine.ComboproductId != -1)
                {
                    comboProductId = parentTrxLine.ComboproductId;
                }
                else
                {
                    comboProductId = GetProductComboId(parentTrxLine);
                }
            }
            log.LogMethodExit(comboProductId);
            return comboProductId;
        }

        private void CreateComboProductLine(int productId, Card card, double price, int quantity, int additionalQuantity, List<AttractionBooking> atbList, List<ReservationDTO.SelectedCategoryProducts> categoryProducts, List<KeyValuePair<int, PurchasedProducts>> purchasedProductList, Transaction.TransactionLine parentTrxLineForCombo, int comboProductId)
        {
            log.LogMethodEntry(productId, card, price, quantity, additionalQuantity, atbList, categoryProducts, purchasedProductList, parentTrxLineForCombo, comboProductId);
            Transaction.TransactionLine parentTrxLine = new Transaction.TransactionLine();
            //parentTrxLine.ParentLine = parentTrxLineForCombo;
            string msg = "";
            int packageQty = quantity + additionalQuantity;
            int ComboQuantity = quantity;

            List<ComboProductDTO> comboProductDTOList = GetComboProductDTOList(productId);
            if (comboProductDTOList != null && comboProductDTOList.Count > 0)
            {
                //List<ComboProductDTO> attractionProductChildList = comboProductDTOList.Where(comboP => comboP.ChildProductType == ProductTypeValues.ATTRACTION
                //                                                                     && comboP.Quantity != null && (int)comboP.Quantity > 0).ToList();

                string[] cardTypes = new[] { ProductTypeValues.NEW, ProductTypeValues.CARDSALE, ProductTypeValues.RECHARGE, ProductTypeValues.GAMETIME };

                List<ComboProductDTO> cardProductChildList = comboProductDTOList.Where(comboP => cardTypes.Contains(comboP.ChildProductType)
                                                                                                 && comboP.Quantity != null && (int)comboP.Quantity > 0).ToList();
                List<string> cardList = new List<string>();
                bool autoGenCardNumber = false;
                //if (attractionProductChildList != null && attractionProductChildList.Count > 0 && atbList != null && atbList.Any())
                if (atbList != null && atbList.Any())
                {
                    foreach (AttractionBooking attractionBookingItem in atbList)
                    {
                        if (attractionBookingItem.cardList != null && attractionBookingItem.cardList.Count > 0)
                        {
                            cardList.AddRange(attractionBookingItem.cardList.Select(cc => cc.CardNumber).ToList());
                        }
                    }
                }
                int qty = 0;
                List<Transaction.ComboCardProduct> cardProductList = new List<Transaction.ComboCardProduct>();
                List<Transaction.TransactionLine> cardDepositLinesList = new List<Transaction.TransactionLine>();
                int newCardCount = 0;
                if (cardProductChildList != null && cardProductChildList.Count > 0)
                {
                    foreach (ComboProductDTO comboProductDTOItem in cardProductChildList)
                    {
                        qty = (comboProductDTOItem.Quantity == null ? 0 : (int)comboProductDTOItem.Quantity);// Convert.ToInt32(dr["quantity"]);
                        if (qty > 0)
                        {
                            Transaction.ComboCardProduct cpDetails = new Transaction.ComboCardProduct();
                            cpDetails.ChildProductId = comboProductDTOItem.ChildProductId;// Convert.ToInt32(dr["product_id"]);
                            cpDetails.ChildProductName = comboProductDTOItem.ChildProductName; // dr["product_name"].ToString();
                            cpDetails.ComboProductId = productId;
                            cpDetails.ChildProductType = comboProductDTOItem.ChildProductType;// dr["product_type"].ToString();
                            cpDetails.Price = (comboProductDTOItem.Price != null ? (float)(comboProductDTOItem.Price) : 0);
                            cpDetails.Quantity = qty;
                            cardProductList.Add(cpDetails);
                            bool cardAutoGen = comboProductDTOItem.ChildProductAutoGenerateCardNumber.Equals("Y");// dr["AutoGenerateCardNumber"].ToString().Equals("Y");
                            if (autoGenCardNumber == cardAutoGen)
                            {
                                if (cpDetails.ChildProductType.Equals("NEW"))
                                {
                                    if (newCardCount == 0) // one NEW card and attraction schedules will share same card
                                    {
                                        newCardCount++;
                                        if (cardList.Count > 0)
                                        {
                                            foreach (String existingCard in cardList)
                                            {
                                                cpDetails.CardNumbers.Add(existingCard);
                                            }
                                        }
                                        else
                                        {
                                            for (int j = 0; j < (packageQty * qty); j++)
                                            {

                                                RandomTagNumber randomTagNumber = new RandomTagNumber(executionContext);
                                                string cardNumber = randomTagNumber.Value;
                                                if (!cardAutoGen)
                                                    cardNumber = "T" + cardNumber.Substring(1);
                                                cpDetails.CardNumbers.Add(cardNumber);
                                                //cardList.Add(new Card(cardNumber, utilities.ExecutionContext.GetUserId(), utilities));
                                                cardList.Add(cardNumber);
                                            }
                                        }
                                    }
                                    else // other new cards will get new card numbers
                                    {
                                        for (int j = 0; j < (packageQty * qty); j++)
                                        {
                                            RandomTagNumber randomTagNumber = new RandomTagNumber(executionContext);
                                            string cardNumber = randomTagNumber.Value;
                                            if (!cardAutoGen)
                                                cardNumber = "T" + cardNumber.Substring(1);
                                            cpDetails.CardNumbers.Add(cardNumber);
                                            cardList.Add(cardNumber);
                                            //cardList.Add(new Card(cardNumber, utilities.ExecutionContext.GetUserId(), utilities));
                                        }
                                    }
                                } // use same cards for other card product types
                                else
                                {
                                    if (cardList != null && cardList.Any())
                                    {
                                        foreach (String existingCard in cardList)
                                        {
                                            cpDetails.CardNumbers.Add(existingCard);
                                        }
                                    }
                                    else
                                    {
                                        for (int j = 0; j < (packageQty * qty); j++)
                                        {
                                            RandomTagNumber randomTagNumber = new RandomTagNumber(executionContext);
                                            string cardNumber = randomTagNumber.Value;
                                            if (!cardAutoGen)
                                                cardNumber = "T" + cardNumber.Substring(1);
                                            cpDetails.CardNumbers.Add(cardNumber);
                                            cardList.Add(cardNumber);
                                            //cardList.Add(new Card(cardNumber, utilities.ExecutionContext.GetUserId(), utilities));
                                        }

                                    }
                                }
                            } // autogen is not matching, so create new card numbers
                            else
                            {
                                for (int j = 0; j < (packageQty * qty); j++)
                                {
                                    RandomTagNumber randomTagNumber = new RandomTagNumber(executionContext);
                                    string cardNumber = randomTagNumber.Value;
                                    if (!cardAutoGen)
                                        cardNumber = "T" + cardNumber.Substring(1);
                                    cpDetails.CardNumbers.Add(cardNumber);
                                    cardList.Add(cardNumber);
                                    //cardList.Add(new Card(cardNumber, utilities.ExecutionContext.GetUserId(), utilities));
                                }
                            }
                        }
                    }
                    //foreach (Card c in cardList)
                    //    Cards.Add(c.CardNumber);
                    if (cardProductList.Count > 0)
                    {
                        foreach (Transaction.ComboCardProduct cpDetail in cardProductList)
                        {
                            if (cpDetail.ChildProductType.ToString().Equals("RECHARGE") && (newCardCount <= 0))//Added new card Count Check if New card and recharge product exists in combo 
                            {
                                for (int cardIndex = 0; cardIndex < packageQty; cardIndex++)
                                {
                                    //Create a card Deposit line if combo child product is of type recharge//
                                    Transaction.TransactionLine cardDepositLine = new Transaction.TransactionLine();
                                    if (this.bookingTransaction.createTransactionLine(new Card(cpDetail.CardNumbers[cardIndex], utilities.ExecutionContext.GetUserId(), utilities), utilities.ParafaitEnv.CardDepositProductId, 0, 1, ref msg, cardDepositLine, true, null, -1) != 0)
                                    {
                                        log.Error(msg);
                                        log.LogMethodExit(msg);
                                        throw new ValidationException(msg);
                                    }
                                    cardDepositLinesList.Add(cardDepositLine);
                                }
                            }
                        }
                    }
                }
                //Convert the  category products list to list ok key value pair
                List<KeyValuePair<int, int>> categorySelectedProducts = new List<KeyValuePair<int, int>>();
                if (categoryProducts != null)
                {
                    foreach (ReservationDTO.SelectedCategoryProducts currSelectedProducts in categoryProducts)
                    {
                        if (currSelectedProducts.parentComboProductId == productId)
                        {
                            categorySelectedProducts.Add(new KeyValuePair<int, int>(currSelectedProducts.productId, currSelectedProducts.quantity));
                        }
                    }
                }

                int comboLineId = -1;
                msg = "";
                if (ComboQuantity != 0)
                {
                    if (this.bookingTransaction.CreateComboProduct(productId, price, ComboQuantity, ref msg, parentTrxLine, cardProductList, categorySelectedProducts, true, false, comboProductId, parentTrxLineForCombo, purchasedProductList, atbList) == 0)
                    {
                        //comboLineId = this.bookingTransaction.TrxLines.IndexOf(parentTrxLine);
                        List<Transaction.TransactionLine> combLines = this.bookingTransaction.TrxLines.Where(tl => tl.LineValid && tl.ProductID == productId && tl.ComboproductId == comboProductId && tl.ParentLine == parentTrxLine.ParentLine).ToList();
                        if (combLines != null && combLines.Any())
                        {
                            if (combLines.Count > ComboQuantity)
                            {
                                int diffCount = combLines.Count - ComboQuantity;
                                combLines.RemoveRange(0, diffCount);
                            }
                        }
                        for (int i = 0; i < cardDepositLinesList.Count; i++)
                        {
                            if (i < ComboQuantity)
                            {
                                int cardLineId = this.bookingTransaction.TrxLines.IndexOf(cardDepositLinesList[i]);
                                if (combLines != null && combLines.Count == ComboQuantity)
                                {
                                    this.bookingTransaction.TrxLines[cardLineId].ParentLine = combLines[i];
                                }
                                else
                                {
                                    this.bookingTransaction.TrxLines[cardLineId].ParentLine = parentTrxLine;
                                }
                            }
                            else
                            {
                                break; //done with processing ComboQuantity lines
                            }
                        }
                        //if (attractionProductChildList != null && attractionProductChildList.Count > 0)
                        //{
                        //    CreateComboAttractionLine(attractionProductChildList, comboLineId, atbList, parentTrxLine, card, ComboQuantity, productId, 0);
                        //}
                        //if (purchasedProductList != null && purchasedProductList.Count > 0)
                        //{
                        //    CreateModifiersForComboChild(purchasedProductList, parentTrxLine);
                        //} 
                    }
                    else
                    {
                        log.Error(msg);
                        log.LogMethodExit(msg);
                        throw new ValidationException(msg);
                    }
                }
                parentTrxLine = new Transaction.TransactionLine();

                if (additionalQuantity != 0)
                {
                    if (ComboQuantity != 0)
                    {
                        foreach (Transaction.ComboCardProduct cardProduct in cardProductList)
                        {
                            cardProduct.CardNumbers.RemoveRange(0, ComboQuantity);
                        }
                        if (cardDepositLinesList != null && cardDepositLinesList.Count > 0)
                        {
                            if (cardDepositLinesList.Count >= ComboQuantity)
                            {
                                cardDepositLinesList.RemoveRange(0, ComboQuantity);
                            }
                        }
                    }
                    msg = "";
                    if (this.bookingTransaction.CreateComboProduct(productId, -1, additionalQuantity, ref msg, parentTrxLine, cardProductList, categorySelectedProducts, true, false, comboProductId, parentTrxLineForCombo, purchasedProductList, atbList) == 0)
                    {
                        //comboLineId = this.bookingTransaction.TrxLines.IndexOf(parentTrxLine);
                        List<Transaction.TransactionLine> combLines = this.bookingTransaction.TrxLines.Where(tl => tl.LineValid && tl.ProductID == productId && tl.ComboproductId == comboProductId && tl.ParentLine == parentTrxLine.ParentLine).ToList();
                        if (combLines != null && combLines.Any())
                        {
                            if (combLines.Count > additionalQuantity)
                            {
                                int diffCount = combLines.Count - additionalQuantity;
                                combLines.RemoveRange(0, diffCount);
                            }
                        }
                        for (int i = 0; i < cardDepositLinesList.Count; i++)
                        {
                            if (i < additionalQuantity)
                            {
                                int cardLineId = this.bookingTransaction.TrxLines.IndexOf(cardDepositLinesList[i]);
                                if (combLines != null && combLines.Count == additionalQuantity)
                                {
                                    this.bookingTransaction.TrxLines[cardLineId].ParentLine = combLines[i];
                                }
                                else
                                {
                                    this.bookingTransaction.TrxLines[cardLineId].ParentLine = parentTrxLine;
                                }
                            }
                            else
                            {
                                break; //done with processing additionalQuantity lines
                            }
                        }
                        //comboLineId = this.bookingTransaction.TrxLines.IndexOf(parentTrxLine);
                        //this.bookingTransaction.TrxLines[comboLineId].ParentLine = parentTrxLineForCombo;
                        //if (attractionProductChildList != null && attractionProductChildList.Count > 0)
                        //{
                        //    CreateComboAttractionLine(attractionProductChildList, comboLineId, atbList, parentTrxLine, card, additionalQuantity, productId, 0);
                        //}
                        //if (purchasedProductList != null && purchasedProductList.Count > 0)
                        //{
                        //    CreateModifiersForComboChild(purchasedProductList, parentTrxLine);
                        //}
                    }
                    else
                    {
                        log.Error(msg);
                        log.LogMethodExit(msg);
                        throw new ValidationException(msg);
                    }
                }

            }
        }

        private List<ComboProductDTO> GetComboProductDTOList(int productId)
        {
            log.LogMethodEntry(productId);
            List<ComboProductDTO> comboProductDTOList = null;
            if (combProductDTOListDictionary.ContainsKey(productId))
            {
                comboProductDTOList = combProductDTOListDictionary[productId];
            }
            else
            {
                ComboProductList comboProductList = new ComboProductList(executionContext);
                List<KeyValuePair<ComboProductDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ComboProductDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<ComboProductDTO.SearchByParameters, string>(ComboProductDTO.SearchByParameters.PRODUCT_ID, productId.ToString()));
                searchParameters.Add(new KeyValuePair<ComboProductDTO.SearchByParameters, string>(ComboProductDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                searchParameters.Add(new KeyValuePair<ComboProductDTO.SearchByParameters, string>(ComboProductDTO.SearchByParameters.IS_ACTIVE, "1"));
                comboProductDTOList = comboProductList.GetComboProductDTOList(searchParameters);
                combProductDTOListDictionary.Add(productId, comboProductDTOList);
            }
            log.LogMethodExit(comboProductDTOList);
            return comboProductDTOList;
        }


        private int[] GetQtyAndAdditionalQtyDetails(int productId, int comboProductId, int quantity, int quantityFromSetup)
        {
            log.LogMethodEntry(productId, comboProductId, quantity, quantityFromSetup);
            int[] qtyInfo = new int[2];
            int alreadyAddedQty = 0;
            int additionalQuantity = 0;
            List<Transaction.TransactionLine> lineForSameProduct = this.bookingTransaction.TrxLines.Where(tl => tl.CancelledLine == false
                                                                                                                  && tl.LineValid == true
                                                                                                                  && tl.ComboproductId == comboProductId
                                                                                                                  && tl.ProductID == productId).ToList();
            if (lineForSameProduct != null && lineForSameProduct.Count > 0)
            {
                alreadyAddedQty = (int)lineForSameProduct.Sum(tl => tl.quantity);
            }
            log.LogVariableState("alreadyAddedQty", alreadyAddedQty);
            if (alreadyAddedQty > 0 && alreadyAddedQty > quantityFromSetup)
            {
                additionalQuantity = quantity;
                quantity = 0;
            }
            else
            {

                if (alreadyAddedQty == 0)
                {
                    if (quantity > quantityFromSetup)
                    {
                        additionalQuantity = quantity - quantityFromSetup;
                        quantity = quantityFromSetup;
                    }
                    else
                    {
                        additionalQuantity = 0;
                    }
                }
                else
                {
                    if ((alreadyAddedQty + quantity) > quantityFromSetup)
                    {
                        additionalQuantity = (alreadyAddedQty + quantity) - quantityFromSetup;
                        quantity = quantityFromSetup - alreadyAddedQty;
                    }
                    else
                    {
                        additionalQuantity = 0;
                    }
                }
            }
            log.LogVariableState("quantity", quantity);
            log.LogVariableState("additionalQuantity", additionalQuantity);

            qtyInfo[0] = quantity;
            qtyInfo[1] = additionalQuantity;
            log.LogMethodExit(qtyInfo);
            return qtyInfo;
        }

        private List<Card> GetCards(bool autoGenCards, int productQty)
        {
            log.LogMethodEntry(autoGenCards, productQty);
            List<Card> cardList = new List<Card>();
            while (productQty-- > 0)
            {
                RandomTagNumber randomTagNumber = new RandomTagNumber(executionContext);
                string cardNumber = randomTagNumber.Value;
                if (!autoGenCards)
                    cardNumber = "T" + cardNumber.Substring(1);

                Card card = new Card(cardNumber, executionContext.GetUserId(), utilities);
                cardList.Add(card);
            }
            log.LogMethodExit(cardList);
            return cardList;
        }

        private void CreateAttractionProductLine(List<AttractionBooking> atbList, Products productBL, double price, int quantity, int additionalQuantity, Transaction.TransactionLine parentTrxLine1, int comboProductId)
        {
            log.LogMethodEntry(atbList, productBL, price, quantity, additionalQuantity, parentTrxLine1, comboProductId);
            string msg = "";
            if (atbList != null && atbList.Any())
            {
                foreach (AttractionBooking atb in atbList)
                {
                    if (productBL.GetProductsDTO.CardSale.Equals("Y") == false)
                    {
                        if (atb.cardList != null)
                            atb.cardList = null;
                    }
                    int bookedUnits = atb.AttractionBookingDTO.BookedUnits;
                    while (bookedUnits > 0)
                    {
                        int bookedUnitsToCreate = 0;
                        if (quantity > 0 && quantity > bookedUnits)
                        {
                            quantity = quantity - bookedUnits;
                            bookedUnitsToCreate = bookedUnits;
                            bookedUnits = 0;
                            msg = "";
                            if (this.bookingTransaction.CreateAttractionProduct(productBL.GetProductsDTO.ProductId, price, bookedUnitsToCreate, this.bookingTransaction.TrxLines.IndexOf(parentTrxLine1), atb, atb.cardList, ref msg, comboProductId) != 0)
                            {
                                log.Error(msg);
                                throw new ValidationException(msg);
                            }
                        }
                        else if (quantity > 0 && bookedUnits > 0)
                        {
                            bookedUnitsToCreate = quantity;
                            bookedUnits = bookedUnits - quantity;
                            quantity = 0;
                            msg = "";
                            if (this.bookingTransaction.CreateAttractionProduct(productBL.GetProductsDTO.ProductId, price, bookedUnitsToCreate, this.bookingTransaction.TrxLines.IndexOf(parentTrxLine1), atb, atb.cardList, ref msg, comboProductId) != 0)
                            {
                                log.Error(msg);
                                throw new ValidationException(msg);
                            }
                        }

                        if (additionalQuantity > 0 && additionalQuantity > bookedUnits)
                        {
                            additionalQuantity = additionalQuantity - bookedUnits;
                            bookedUnitsToCreate = bookedUnits;
                            bookedUnits = 0;
                            msg = "";
                            if (this.bookingTransaction.CreateAttractionProduct(productBL.GetProductsDTO.ProductId, -1, bookedUnitsToCreate, this.bookingTransaction.TrxLines.IndexOf(parentTrxLine1), atb, atb.cardList, ref msg, comboProductId) != 0)
                            {
                                log.Error(msg);
                                throw new ValidationException(msg);
                            }
                        }
                        else if (additionalQuantity > 0 && bookedUnits > 0)
                        {
                            bookedUnitsToCreate = additionalQuantity;
                            bookedUnits = bookedUnits - additionalQuantity;
                            additionalQuantity = 0;
                            msg = "";
                            if (this.bookingTransaction.CreateAttractionProduct(productBL.GetProductsDTO.ProductId, -1, bookedUnitsToCreate, this.bookingTransaction.TrxLines.IndexOf(parentTrxLine1), atb, atb.cardList, ref msg, comboProductId) != 0)
                            {
                                log.Error(msg);
                                throw new ValidationException(msg);
                            }
                        }
                    }

                    if (atb != null)
                        atb.Expire();

                }
            }
            else
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Attraction booking details are missing"));
            }
            log.LogMethodExit();
        }


        private void CreateCardTypeProductLine(Products productBL, double price, int quantity, int additionalQuantity, Transaction.TransactionLine parentTrxLine, int comboProductId)
        {
            log.LogMethodEntry(productBL, price, quantity, additionalQuantity, parentTrxLine, comboProductId);
            string msg = "";
            int lclQty = quantity + additionalQuantity;
            int baseCardIndex = 0;

            List<Card> cardList = GetCards(productBL.GetProductsDTO.AutoGenerateCardNumber.ToString().Equals("Y"), lclQty);
            while (lclQty > 0)
            {

                int ret = -1;
                Card lclCard = cardList[baseCardIndex++];

                if (productBL.GetProductsDTO.ProductType == ProductTypeValues.RECHARGE) //Added new card Count Check if New card and recharge product exists 
                {
                    bool newProductFound = false;
                    foreach (Transaction.TransactionLine addedTrxLine in this.bookingTransaction.TrxLines)
                    {
                        if (addedTrxLine.LineValid && addedTrxLine.CardNumber == lclCard.CardNumber
                             && lclCard.CardStatus == "NEW"
                                && (addedTrxLine.ProductTypeCode == "NEW"
                                || addedTrxLine.ProductTypeCode == "GAMETIME"
                                || addedTrxLine.ProductTypeCode == "CARDDEPOSIT")
                            )
                        {
                            newProductFound = true;
                            break;
                        }
                    }
                    if (newProductFound == false)
                    {
                        msg = "";
                        ret = this.bookingTransaction.createTransactionLine(lclCard, utilities.ParafaitEnv.CardDepositProductId, 0, 1, parentTrxLine, ref msg, null, true, comboProductId);
                        if (ret != 0)
                        {
                            log.Error(msg);
                            log.LogMethodExit();
                            throw new ValidationException(msg);
                        }
                    }
                }
                msg = "";
                if (lclQty > quantity)
                    ret = this.bookingTransaction.createTransactionLine(lclCard, productBL.GetProductsDTO.ProductId, -1, 1, parentTrxLine, ref msg, null, true, comboProductId);
                else
                    ret = this.bookingTransaction.createTransactionLine(lclCard, productBL.GetProductsDTO.ProductId, price, 1, parentTrxLine, ref msg, null, true, comboProductId);

                if (ret != 0)
                {
                    log.Error(msg);
                    log.LogMethodExit();
                    throw new ValidationException(msg);
                }

                this.bookingTransaction.updateAmounts();
                lclQty--;
            }
            log.LogMethodExit();
        }



        private void CreateNonCardTypeProductLine(Products productBL, Card card, double price, int quantity, int additionalQuantity, List<KeyValuePair<int, PurchasedProducts>> purchasedProductList, Transaction.TransactionLine parentTrxLine, int comboProductId)
        {
            log.LogMethodEntry(productBL, card, price, quantity, additionalQuantity, purchasedProductList, parentTrxLine, comboProductId);
            string msg = "";
            List<KeyValuePair<int, PurchasedProducts>> purchasedModfilerList = null;
            if (purchasedProductList != null && purchasedProductList.Count > 0)
            {
                purchasedModfilerList = purchasedProductList.Where(item => item.Key == productBL.GetProductsDTO.ProductId).ToList();
            }
            PurchasedProducts selectedModifier = null;
            if (purchasedModfilerList != null && purchasedModfilerList.Count > 0)
            {
                selectedModifier = purchasedModfilerList[0].Value;
            }
            if (additionalQuantity != 0)
            {
                while (additionalQuantity > 0)
                {
                    additionalQuantity--;
                    Transaction.TransactionLine outTrxLine = new Transaction.TransactionLine();
                    msg = "";
                    //public int createTransactionLine(Card inCard, int productId, double in_price, decimal in_quantity, ref string message, TransactionLine outTrxLine = null, bool CreateChildLines = true, SqlTransaction sqlTrx = null, int comboProductId = -1, PurchasedProducts productModiferInfo = null)
                    if (this.bookingTransaction.createTransactionLine(card, productBL.GetProductsDTO.ProductId, -1, 1, ref msg, outTrxLine, true, null, comboProductId, selectedModifier) != 0)
                    {
                        additionalQuantity = 0;
                        log.Error(msg);
                        throw new ValidationException(msg);
                    }
                    outTrxLine.ParentLine = parentTrxLine;
                    //if (purchasedProductList != null && purchasedProductList.Count > 0)
                    //{
                    //    foreach (KeyValuePair<int, PurchasedProducts> purchasedProductKey in purchasedProductList)
                    //    {
                    //        if (purchasedProductKey.Key == productBL.GetProductsDTO.ProductId)
                    //        {
                    //            CreateTransactionModifierLine(purchasedProductKey.Value, outTrxLine);
                    //        }
                    //    }
                    //}
                }
            }
            while (quantity > 0)
            {
                quantity--;
                Transaction.TransactionLine outTrxLine = new Transaction.TransactionLine();
                msg = "";
                //public int createTransactionLine(Card inCard, int productId, double in_price, decimal in_quantity, ref string message, TransactionLine outTrxLine = null, bool CreateChildLines = true, SqlTransaction sqlTrx = null, int comboProductId = -1, PurchasedProducts productModiferInfo = null)
                if (this.bookingTransaction.createTransactionLine(card, productBL.GetProductsDTO.ProductId, price, 1, ref msg, outTrxLine, true, null, comboProductId, selectedModifier) != 0)
                {
                    quantity = 0;
                    log.Error(msg);
                    throw new ValidationException(msg);
                }
                outTrxLine.ParentLine = parentTrxLine;
                //if (purchasedProductList != null && purchasedProductList.Count > 0)
                //{
                //    foreach (KeyValuePair<int, PurchasedProducts> purchasedProductKey in purchasedProductList)
                //    {
                //        if (purchasedProductKey.Key == productBL.GetProductsDTO.ProductId)
                //        {
                //            CreateTransactionModifierLine(purchasedProductKey.Value, outTrxLine);
                //        }
                //    }
                //} 
            }
            log.LogMethodExit();
        }

        //private void CreateTransactionModifierLine(PurchasedProducts purchasedSelectedProduct, Transaction.TransactionLine parentTrxLine)
        //{
        //    log.LogMethodEntry(purchasedSelectedProduct, parentTrxLine);
        //    if (purchasedSelectedProduct.PurchasedModifierSetDTOList != null &&
        //        purchasedSelectedProduct.PurchasedModifierSetDTOList.Count > 0)
        //    {
        //        int count = 0;

        //        foreach (PurchasedModifierSet modifierSet in purchasedSelectedProduct.PurchasedModifierSetDTOList)
        //        {
        //            if (modifierSet != null && modifierSet.PurchasedProductsList != null && modifierSet.PurchasedProductsList.Count > 0)
        //            {

        //                if (modifierSet.FreeQuantity > 0 && modifierSet.PurchasedProductsList != null)
        //                {
        //                    foreach (PurchasedProducts purchasedProducts in modifierSet.PurchasedProductsList)
        //                    {
        //                        if (count < modifierSet.FreeQuantity)
        //                        {
        //                            purchasedProducts.Price = 0;
        //                            count++;
        //                        }
        //                        else
        //                            break;
        //                    }
        //                }

        //                foreach (PurchasedProducts localProductDTO in modifierSet.PurchasedProductsList)
        //                {
        //                    int lclChildProduct = Convert.ToInt32(localProductDTO.ProductId);

        //                    Transaction.TransactionLine newModifierLine = new Transaction.TransactionLine();
        //                    newModifierLine.ModifierLine = true;
        //                    string message = "";
        //                    if (this.bookingTransaction.createTransactionLine(null, lclChildProduct, (double)localProductDTO.Price, 1, parentTrxLine, ref message, newModifierLine) != 0)
        //                    {
        //                        log.Error("Error craeting the transaction line of Product : " + localProductDTO.ProductName);
        //                    }
        //                    else
        //                    {
        //                        if (localProductDTO.PurchasedModifierSetDTOList != null && localProductDTO.PurchasedModifierSetDTOList.Count > 0)
        //                        {
        //                            CreateTransactionModifierLine(localProductDTO, newModifierLine);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    log.LogMethodExit();
        //}


        #endregion AddProduct and its methods
        /// <summary>
        /// RentalIsAlreadyAddedToTheSchedule
        /// </summary>
        /// <param name="scheduleId"></param>
        /// <param name="facilityMapId"></param>
        /// <param name="rentalProduct"></param>
        public void RentalIsAlreadyAddedToTheSchedule(int scheduleId, int facilityMapId, int rentalProduct)
        {
            log.LogMethodEntry(scheduleId, facilityMapId);
            if (this.reservationDTO != null && this.bookingTransaction != null)
            {
                bool alreadyPresent = this.bookingTransaction.HasThisReservationRentalProductAndSchedule(scheduleId, facilityMapId, rentalProduct);
                if (alreadyPresent)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2168));//"Selected rental product is already added to the selected schedule"
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// GetReservationProductTransactionLines
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="comboProductId"></param>
        /// <returns></returns>
        public List<Transaction.TransactionLine> GetReservationProductTransactionLines(int productId, int comboProductId)
        {
            log.LogMethodEntry();
            try
            {
                List<Transaction.TransactionLine> selectedTrxLines = null;
                DTONullCheck();
                if (this.bookingTransaction != null)
                {
                    selectedTrxLines = this.bookingTransaction.GetReservationProductTransactionLines(productId, comboProductId);
                }

                log.LogMethodExit(selectedTrxLines);
                return selectedTrxLines;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(ex.Message);
                throw;
            }
        }

        private List<Transaction.TransactionLine> GetOrginalReservationProductTransactionLines(int productId, int comboProductId)
        {
            log.LogMethodEntry();
            try
            {
                List<Transaction.TransactionLine> selectedTrxLines = null;
                if (this.reservationDTOBeforeEdit != null && this.bookingTransactionBeforeEdit != null)
                {
                    selectedTrxLines = this.bookingTransactionBeforeEdit.GetReservationProductTransactionLines(productId, comboProductId);
                }
                log.LogMethodExit(selectedTrxLines);
                return selectedTrxLines;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(ex.Message);
                throw;
            }
        }


        /// <summary>
        /// GetPurchasedPackageProducts
        /// </summary>
        /// <returns></returns>
        public List<Transaction.TransactionLine> GetPurchasedPackageProducts()
        {
            log.LogMethodEntry();
            List<Transaction.TransactionLine> transactionLines = null;
            if (this.bookingTransaction != null)
            {
                transactionLines = this.bookingTransaction.GetPurchasedBookingPackageProducts();
            }
            log.LogMethodExit(transactionLines);
            return transactionLines;
        }
        private List<Transaction.TransactionLine> GetOriginalPurchasedPackageProducts()
        {
            log.LogMethodEntry();
            List<Transaction.TransactionLine> transactionLines = null;
            if (this.bookingTransactionBeforeEdit != null)
            {
                transactionLines = this.bookingTransactionBeforeEdit.GetPurchasedBookingPackageProducts();
            }
            log.LogMethodExit(transactionLines);
            return transactionLines;
        }

        /// <summary>
        /// GetPurchasedAdditionalProducts
        /// </summary>
        /// <returns></returns>
        public List<Transaction.TransactionLine> GetPurchasedAdditionalProducts()
        {
            log.LogMethodEntry();
            List<Transaction.TransactionLine> transactionLines = null;
            if (this.bookingTransaction != null)
            {
                transactionLines = this.bookingTransaction.GetPurchasedAdditionalProductsForBooking();
            }
            log.LogMethodExit(transactionLines);
            return transactionLines;
        }

        private List<Transaction.TransactionLine> GetOrginalPurchasedAdditionalProducts()
        {
            log.LogMethodEntry();
            List<Transaction.TransactionLine> transactionLines = null;
            if (this.bookingTransactionBeforeEdit != null)
            {
                transactionLines = this.bookingTransactionBeforeEdit.GetPurchasedAdditionalProductsForBooking();
            }
            log.LogMethodExit(transactionLines);
            return transactionLines;
        }



        /// <summary>
        /// GetScheduleTransactionLines
        /// </summary>
        /// <returns></returns>
        public List<Transaction.TransactionLine> GetScheduleTransactionLines()
        {
            log.LogMethodEntry();
            List<Transaction.TransactionLine> selectedTrxLines = null;
            try
            {
                if (this.reservationDTO != null && this.bookingTransaction != null)
                {
                    selectedTrxLines = this.bookingTransaction.GetScheduleTransactionLines();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(ex.Message);
                throw;
            }
            log.LogMethodExit(selectedTrxLines);
            return selectedTrxLines;
        }



        private List<Transaction.TransactionLine> GetOriginalScheduleTransactionLines()
        {
            log.LogMethodEntry();
            List<Transaction.TransactionLine> selectedTrxLines = null;
            try
            {
                if (this.reservationDTOBeforeEdit != null && this.bookingTransactionBeforeEdit != null)
                {
                    selectedTrxLines = this.bookingTransactionBeforeEdit.GetScheduleTransactionLines();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(ex.Message);
                throw;
            }
            log.LogMethodExit(selectedTrxLines);
            return selectedTrxLines;
        }

        ///// <summary>
        ///// GetReservationFacilities
        ///// </summary>
        ///// <returns></returns>
        //private string GetReservationFacilities()
        //{
        //    log.LogMethodEntry();
        //    string facilityNameList = string.Empty;
        //    if (this.bookingTransaction != null)
        //    {
        //        facilityNameList = this.bookingTransaction.GetReservationFacilities(this.reservationDTO.Status);
        //    }
        //    log.LogMethodExit(facilityNameList);
        //    return facilityNameList;
        //}


        public Transaction.TransactionLine GetBookingProductTransactionLine()
        {
            log.LogMethodEntry();
            try
            {
                Transaction.TransactionLine selectedTrxLine = null;
                if (this.reservationDTO != null && this.bookingTransaction != null)
                {
                    // int bookingProductId = this.reservationDTO.BookingProductId;
                    selectedTrxLine = this.bookingTransaction.GetBookingProductTransactionLine();
                }
                log.LogMethodExit(selectedTrxLine);
                return selectedTrxLine;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(ex.Message);
                throw;
            }
        }


        private Transaction.TransactionLine GetOriginalBookingProductTranasctionLine()
        {
            log.LogMethodEntry();
            try
            {
                Transaction.TransactionLine selectedTrxLines = null;
                if (this.reservationDTOBeforeEdit != null && this.bookingTransactionBeforeEdit != null && this.bookingTransactionBeforeEdit.TrxLines != null)
                {
                    // int bookingProductId = this.reservationDTO.BookingProductId;
                    selectedTrxLines = this.bookingTransactionBeforeEdit.GetBookingProductTransactionLine();
                }
                log.LogMethodExit(selectedTrxLines);
                return selectedTrxLines;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// GetServiceChargeTransactionLine
        /// </summary>
        /// <returns></returns>
        public Transaction.TransactionLine GetServiceChargeTransactionLine()
        {
            log.LogMethodEntry();
            try
            {
                Transaction.TransactionLine selectedTrxLines = null;
                if (this.reservationDTO != null && this.bookingTransaction != null)
                {
                    selectedTrxLines = this.bookingTransaction.GetActiveLineForType(ProductTypeValues.SERVICECHARGE, null);
                }
                log.LogMethodExit(selectedTrxLines);
                return selectedTrxLines;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// GetServiceChargeAmount
        /// </summary>
        /// <returns></returns>
        public double GetServiceChargeAmount()
        {
            log.LogMethodEntry();
            double serviceChargeAmount = 0;
            if (ReservationTransactionIsNotNull())
            {
                Transaction.TransactionLine serviceChargeLine = GetServiceChargeTransactionLine();
                if (serviceChargeLine != null)
                {
                    serviceChargeAmount = serviceChargeLine.LineAmount;
                }
            }
            log.LogMethodExit(serviceChargeAmount);
            return serviceChargeAmount;
        }

        /// <summary>
        /// GetGuestQuantity
        /// </summary>
        /// <returns></returns>
        public int GetGuestQuantity()
        {
            log.LogMethodEntry();
            int guestQty = 0;
            if (ReservationTransactionIsNotNull())
            {
                guestQty = this.bookingTransaction.GetReservationTransactionGuestQuantity();
            }
            log.LogMethodExit(guestQty);
            return guestQty;
        }

        private void SetReservationCheckList(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            DTONullCheck();
            if (this.reservationDTO.Status == ReservationDTO.ReservationStatus.CONFIRMED.ToString())
            {
                Transaction.TransactionLine bookingProductTrxLine = this.bookingTransaction.GetBookingProductTransactionLine();
                if (bookingProductTrxLine == null)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2155));//"Booking product is mandatory"
                }
                if (this.reservationDTO.BookingCheckListDTOList != null && this.reservationDTO.BookingCheckListDTOList.Any())
                {
                    foreach (BookingCheckListDTO bookingCheckListDTO in this.reservationDTO.BookingCheckListDTOList)
                    {
                        JobScheduleTasksListBL jobScheduleTasksListBL = new JobScheduleTasksListBL(executionContext);
                        List<KeyValuePair<JobScheduleTasksDTO.SearchByJobScheduleTaskParameters, string>> searchParameters = new List<KeyValuePair<JobScheduleTasksDTO.SearchByJobScheduleTaskParameters, string>>();
                        searchParameters.Add(new KeyValuePair<JobScheduleTasksDTO.SearchByJobScheduleTaskParameters, string>(JobScheduleTasksDTO.SearchByJobScheduleTaskParameters.BOOKING_ID, bookingCheckListDTO.BookingId.ToString()));
                        searchParameters.Add(new KeyValuePair<JobScheduleTasksDTO.SearchByJobScheduleTaskParameters, string>(JobScheduleTasksDTO.SearchByJobScheduleTaskParameters.BOOKING_CHECK_LIST_ID, bookingCheckListDTO.BookingCheckListId.ToString()));
                        searchParameters.Add(new KeyValuePair<JobScheduleTasksDTO.SearchByJobScheduleTaskParameters, string>(JobScheduleTasksDTO.SearchByJobScheduleTaskParameters.IS_ACTIVE, "1"));
                        searchParameters.Add(new KeyValuePair<JobScheduleTasksDTO.SearchByJobScheduleTaskParameters, string>(JobScheduleTasksDTO.SearchByJobScheduleTaskParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<JobScheduleTasksDTO> jobScheduleTasksDTOList = jobScheduleTasksListBL.GetAllJobScheduleTaskDTOList(searchParameters);
                        if (jobScheduleTasksDTOList != null && jobScheduleTasksDTOList.Count > 0)
                        {
                            UpdateCheckListScheduleNTaskEntries(bookingCheckListDTO, jobScheduleTasksDTOList, sqlTrx);
                        }
                        else if (bookingCheckListDTO.IsActive)
                        {
                            CreateCheckListScheduleNTaskEntries(bookingCheckListDTO, sqlTrx);
                        }
                    }

                }
            }
            log.LogMethodExit();
        }

        private void UpdateCheckListScheduleNTaskEntries(BookingCheckListDTO bookingCheckListDTO, List<JobScheduleTasksDTO> jobScheduleTasksDTOList, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(bookingCheckListDTO, jobScheduleTasksDTOList, sqlTrx);
            if (jobScheduleTasksDTOList != null && bookingCheckListDTO != null)
            {
                int siteId = executionContext.GetSiteId();
                string userName = executionContext.GetUserId();
                ScheduleCalendarListBL scheduleCalendarListBL = new ScheduleCalendarListBL(executionContext);
                List<KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>> searchParameters = new List<KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>>();
                searchParameters.Add(new KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>(ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SCHEDULE_ID, jobScheduleTasksDTOList[0].JobScheduleId.ToString()));
                searchParameters.Add(new KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>(ScheduleCalendarDTO.SearchByScheduleCalendarParameters.IS_ACTIVE, "1"));
                searchParameters.Add(new KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>(ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SITE_ID, siteId.ToString()));
                List<ScheduleCalendarDTO> scheduleCalendarDTOList = scheduleCalendarListBL.GetAllSchedule(searchParameters);
                if (scheduleCalendarDTOList != null && scheduleCalendarDTOList.Count > 0)
                {
                    if (bookingCheckListDTO.IsActive)
                    {
                        ScheduleCalendarDTO scheduleCalendarDTO = scheduleCalendarDTOList[0];
                        if (bookingCheckListDTO.ChecklistTaskGroupId != jobScheduleTasksDTOList[0].JObTaskGroupId)
                        {
                            CancelNRecreateCheckList(bookingCheckListDTO, jobScheduleTasksDTOList[0].JobScheduleId, sqlTrx);
                        }
                        else
                        {
                            UpdateScheduleNUserInfoOnChecklist(bookingCheckListDTO, jobScheduleTasksDTOList[0].JObTaskGroupId, scheduleCalendarDTOList[0], sqlTrx);
                        }
                    }
                    else
                    {   //Cancel
                        for (int i = 0; i < jobScheduleTasksDTOList.Count; i++)
                        {
                            CancellCheckListScheduleNTaskEntries(bookingCheckListDTO, jobScheduleTasksDTOList[i].JobScheduleId, sqlTrx);
                        }
                    }
                }

            }
            log.LogMethodExit();
        }

        private void CancelNRecreateCheckList(BookingCheckListDTO bookingCheckListDTO, int jobScheduleId, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(bookingCheckListDTO, jobScheduleId, sqlTrx);
            CancellCheckListScheduleNTaskEntries(bookingCheckListDTO, jobScheduleId, sqlTrx);
            CreateCheckListScheduleNTaskEntries(bookingCheckListDTO, sqlTrx);
            log.LogMethodExit();
        }

        private void CancellCheckListScheduleNTaskEntries(BookingCheckListDTO bookingCheckListDTO, int jobScheduleId, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(bookingCheckListDTO, jobScheduleId, sqlTrx);
            int siteId = executionContext.GetSiteId();
            string userName = executionContext.GetUserId();
            ScheduleCalendarListBL scheduleCalendarListBL = new ScheduleCalendarListBL(executionContext);
            List<KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>> searchParameters = new List<KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>>();
            searchParameters.Add(new KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>(ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SCHEDULE_ID, jobScheduleId.ToString()));
            searchParameters.Add(new KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>(ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SITE_ID, siteId.ToString()));
            searchParameters.Add(new KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>(ScheduleCalendarDTO.SearchByScheduleCalendarParameters.IS_ACTIVE, "1"));
            List<ScheduleCalendarDTO> scheduleCalendarDTOList = scheduleCalendarListBL.GetAllSchedule(searchParameters);
            if (scheduleCalendarDTOList != null && scheduleCalendarDTOList.Count > 0)
            {
                foreach (ScheduleCalendarDTO scheduleCalendarDTOItems in scheduleCalendarDTOList)
                {
                    scheduleCalendarDTOItems.IsActive = false;
                    ScheduleCalendarBL scheduleCalendarBL = new ScheduleCalendarBL(executionContext, scheduleCalendarDTOItems);
                    scheduleCalendarBL.Save(sqlTrx);
                    JobScheduleListBL jobScheduleListBL = new JobScheduleListBL(executionContext);
                    List<KeyValuePair<JobScheduleDTO.SearchByJobScheduleDTOParameters, string>> searchParams = new List<KeyValuePair<JobScheduleDTO.SearchByJobScheduleDTOParameters, string>>();
                    searchParams.Add(new KeyValuePair<JobScheduleDTO.SearchByJobScheduleDTOParameters, string>(JobScheduleDTO.SearchByJobScheduleDTOParameters.SCHEDULE_ID, scheduleCalendarDTOItems.ScheduleId.ToString()));
                    searchParams.Add(new KeyValuePair<JobScheduleDTO.SearchByJobScheduleDTOParameters, string>(JobScheduleDTO.SearchByJobScheduleDTOParameters.IS_ACTIVE, "1"));
                    searchParams.Add(new KeyValuePair<JobScheduleDTO.SearchByJobScheduleDTOParameters, string>(JobScheduleDTO.SearchByJobScheduleDTOParameters.SITE_ID, siteId.ToString()));
                    List<JobScheduleDTO> jobScheduleDTOList = jobScheduleListBL.GetAllJobScheduleDTOList(searchParams);
                    if (jobScheduleDTOList != null && jobScheduleDTOList.Count > 0)
                    {
                        foreach (JobScheduleDTO jobScheduleDTOitem in jobScheduleDTOList)
                        {
                            jobScheduleDTOitem.IsActive = false;
                            JobScheduleBL jobScheduleBL = new JobScheduleBL(executionContext, jobScheduleDTOitem);
                            jobScheduleBL.Save(sqlTrx);
                        }
                    }
                }

                JobScheduleTasksListBL jobScheduleTasksListBL = new JobScheduleTasksListBL(executionContext);
                List<KeyValuePair<JobScheduleTasksDTO.SearchByJobScheduleTaskParameters, string>> searchPara = new List<KeyValuePair<JobScheduleTasksDTO.SearchByJobScheduleTaskParameters, string>>();
                searchPara.Add(new KeyValuePair<JobScheduleTasksDTO.SearchByJobScheduleTaskParameters, string>(JobScheduleTasksDTO.SearchByJobScheduleTaskParameters.BOOKING_ID, bookingCheckListDTO.BookingId.ToString()));
                searchPara.Add(new KeyValuePair<JobScheduleTasksDTO.SearchByJobScheduleTaskParameters, string>(JobScheduleTasksDTO.SearchByJobScheduleTaskParameters.BOOKING_CHECK_LIST_ID, bookingCheckListDTO.BookingCheckListId.ToString()));
                searchPara.Add(new KeyValuePair<JobScheduleTasksDTO.SearchByJobScheduleTaskParameters, string>(JobScheduleTasksDTO.SearchByJobScheduleTaskParameters.SITE_ID, siteId.ToString()));
                searchPara.Add(new KeyValuePair<JobScheduleTasksDTO.SearchByJobScheduleTaskParameters, string>(JobScheduleTasksDTO.SearchByJobScheduleTaskParameters.IS_ACTIVE, "1"));
                List<JobScheduleTasksDTO> seelctedJobScheduleTasksDTOList = jobScheduleTasksListBL.GetAllJobScheduleTaskDTOList(searchPara);
                if (seelctedJobScheduleTasksDTOList != null && seelctedJobScheduleTasksDTOList.Count > 0)
                {
                    foreach (JobScheduleTasksDTO jobScheduleTasksDTOItem in seelctedJobScheduleTasksDTOList)
                    {
                        jobScheduleTasksDTOItem.IsActive = false;
                        JobScheduleTasksBL jobScheduleTasksBL = new JobScheduleTasksBL(executionContext, jobScheduleTasksDTOItem);
                        jobScheduleTasksBL.Save(sqlTrx);
                    }
                }

                UserJobItemsListBL userJobItemsListBL = new UserJobItemsListBL(executionContext);
                List<KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>> searchP = new List<KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>>();
                searchP.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.JOB_SCHEDULE_ID, jobScheduleId.ToString()));
                searchP.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.SITE_ID, siteId.ToString()));
                searchP.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.IS_ACTIVE, "1"));
                List<UserJobItemsDTO> userJobItemsDTOList = userJobItemsListBL.GetAllUserJobItemDTOList(searchP, -1);
                if (userJobItemsDTOList != null && userJobItemsDTOList.Count > 0)
                {
                    foreach (UserJobItemsDTO userJobItemsDTOitem in userJobItemsDTOList)
                    {
                        userJobItemsDTOitem.IsActive = false;
                        UserJobItemsBL userJobItemsBL = new UserJobItemsBL(executionContext, userJobItemsDTOitem);
                        userJobItemsBL.Save(sqlTrx);
                    }
                }

            }
            log.LogMethodExit();
        }

        private void UpdateScheduleNUserInfoOnChecklist(BookingCheckListDTO bookingCheckListDTO, int jobTaskGroupId, ScheduleCalendarDTO scheduleCalendarDTO, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(bookingCheckListDTO, jobTaskGroupId, scheduleCalendarDTO, sqlTrx);
            bool bookingScheduleChanged = false;
            bool bookingHostChanged = false;
            int dayBeforeScheduleDate = 0;
            try
            {
                dayBeforeScheduleDate = -1 * Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CREATE_RESERVATION_CHECKLIST_BEFORE_DAYS"));
            }
            catch (Exception ex)
            {
                log.Error(ex);
                dayBeforeScheduleDate = 0;
            }
            DateTime startDate = this.reservationDTO.FromDate.Date.AddDays(dayBeforeScheduleDate);
            if (startDate < utilities.getServerTime())
            {
                startDate = utilities.getServerTime().Date;
            }

            if (bookingCheckListDTO.ChecklistTaskGroupId == jobTaskGroupId && startDate != scheduleCalendarDTO.ScheduleTime)
            {
                scheduleCalendarDTO.ScheduleTime = startDate;
                ScheduleCalendarBL scheduleCalendarBL = new ScheduleCalendarBL(executionContext, scheduleCalendarDTO);
                scheduleCalendarBL.Save(sqlTrx);
                bookingScheduleChanged = true;
            }
            JobScheduleListBL jobScheduleListBL = new JobScheduleListBL(executionContext);
            List<KeyValuePair<JobScheduleDTO.SearchByJobScheduleDTOParameters, string>> searchParams = new List<KeyValuePair<JobScheduleDTO.SearchByJobScheduleDTOParameters, string>>();
            searchParams.Add(new KeyValuePair<JobScheduleDTO.SearchByJobScheduleDTOParameters, string>(JobScheduleDTO.SearchByJobScheduleDTOParameters.SCHEDULE_ID, scheduleCalendarDTO.ScheduleId.ToString()));
            searchParams.Add(new KeyValuePair<JobScheduleDTO.SearchByJobScheduleDTOParameters, string>(JobScheduleDTO.SearchByJobScheduleDTOParameters.IS_ACTIVE, "1"));
            searchParams.Add(new KeyValuePair<JobScheduleDTO.SearchByJobScheduleDTOParameters, string>(JobScheduleDTO.SearchByJobScheduleDTOParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<JobScheduleDTO> jobScheduleDTOList = jobScheduleListBL.GetAllJobScheduleDTOList(searchParams);
            if (jobScheduleDTOList != null && jobScheduleDTOList.Count > 0)
            {
                JobScheduleDTO jobScheduleDTO = jobScheduleDTOList[0];
                if (bookingCheckListDTO.EventHostUserId != jobScheduleDTOList[0].UserId)
                {
                    bookingHostChanged = true;
                    jobScheduleDTO.UserId = jobScheduleDTOList[0].UserId;
                    JobScheduleBL jobScheduleBL = new JobScheduleBL(executionContext, jobScheduleDTO);
                    jobScheduleBL.Save(sqlTrx);
                }
                if (bookingScheduleChanged)
                {
                    LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                    List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                    lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_JOB_STATUS"));
                    lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    List<LookupValuesDTO> statusDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                    int closedStatusId = -9999;
                    if (statusDTOList != null)
                    {
                        LookupValuesDTO statusDTO = statusDTOList.Find(status => status.LookupValue == "Closed");
                        closedStatusId = statusDTO.LookupValueId;
                    }

                    UserJobItemsListBL userJobItemsListBL = new UserJobItemsListBL(executionContext);
                    List<KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>> searchPara = new List<KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>>();
                    searchPara.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.JOB_SCHEDULE_ID, jobScheduleDTO.ScheduleId.ToString()));
                    searchPara.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    searchPara.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.IS_ACTIVE, "1"));
                    List<UserJobItemsDTO> userJobItemsDTOList = userJobItemsListBL.GetAllUserJobItemDTOList(searchPara, -1);
                    if (userJobItemsDTOList != null && userJobItemsDTOList.Count > 0)
                    {
                        foreach (UserJobItemsDTO userJobItemsDTOitem in userJobItemsDTOList)
                        {
                            if (userJobItemsDTOitem.Status != closedStatusId)
                            {
                                if (bookingHostChanged)
                                {
                                    userJobItemsDTOitem.AssignedUserId = bookingCheckListDTO.EventHostUserId;
                                    userJobItemsDTOitem.AssignedTo = bookingCheckListDTO.HostName;
                                }
                                userJobItemsDTOitem.ChklstScheduleTime = startDate.ToString("yyyy-MM-dd HH:mm:ss");
                                UserJobItemsBL userJobItemsBL = new UserJobItemsBL(executionContext, userJobItemsDTOitem);
                                userJobItemsBL.Save(sqlTrx);
                            }
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        private void CreateCheckListScheduleNTaskEntries(BookingCheckListDTO bookingCheckListDTO, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(bookingCheckListDTO);
            if (bookingCheckListDTO != null)
            {
                int dayBeforeScheduleDate = 0;
                try
                {
                    dayBeforeScheduleDate = -1 * Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "CREATE_RESERVATION_CHECKLIST_BEFORE_DAYS", 5));
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    dayBeforeScheduleDate = 0;
                }

                int siteId = executionContext.GetSiteId();
                string userName = executionContext.GetUserId();
                DateTime startDate = this.reservationDTO.FromDate.Date.AddDays(dayBeforeScheduleDate);
                if (startDate < utilities.getServerTime())
                {
                    startDate = utilities.getServerTime().Date;
                }
                ScheduleCalendarDTO scheduleCalendarDTO = new ScheduleCalendarDTO(-1, MessageContainerList.GetMessage(executionContext, "Checklist Schedule for ") + this.reservationDTO.ReservationCode,
                                                                                  startDate, DateTime.MaxValue, "N", "", DateTime.MaxValue, "", true,
                                                                                  userName, utilities.getServerTime(), userName, utilities.getServerTime(), "",
                                                                                  siteId, false, -1);
                ScheduleCalendarBL scheduleCalendarBL = new ScheduleCalendarBL(executionContext, scheduleCalendarDTO);
                scheduleCalendarBL.Save(sqlTrx);

                JobScheduleDTO jobScheduleDTO = new JobScheduleDTO(-1, scheduleCalendarDTO.ScheduleId, bookingCheckListDTO.EventHostUserId, -1, 0, DateTime.MinValue, true,
                                                                    userName, utilities.getServerTime(), userName, utilities.getServerTime(), "", siteId
                                                                   , false, -1);
                JobScheduleBL jobScheduleBL = new JobScheduleBL(executionContext, jobScheduleDTO);
                jobScheduleBL.Save(sqlTrx);

                JobTaskList jobTaskListBL = new JobTaskList(executionContext);
                List<KeyValuePair<JobTaskDTO.SearchByJobTaskParameters, string>> searchParams = new List<KeyValuePair<JobTaskDTO.SearchByJobTaskParameters, string>>();
                searchParams.Add(new KeyValuePair<JobTaskDTO.SearchByJobTaskParameters, string>(JobTaskDTO.SearchByJobTaskParameters.JOB_TASK_GROUP_ID, bookingCheckListDTO.ChecklistTaskGroupId.ToString()));
                searchParams.Add(new KeyValuePair<JobTaskDTO.SearchByJobTaskParameters, string>(JobTaskDTO.SearchByJobTaskParameters.SITE_ID, siteId.ToString()));
                List<JobTaskDTO> jobTaskDTOList = jobTaskListBL.GetAllJobTasks(searchParams);
                if (jobTaskDTOList != null && jobTaskDTOList.Count > 0)
                {
                    JobScheduleTasksDTO jobScheduleTasksDTO = new JobScheduleTasksDTO(-1, jobScheduleDTO.JobScheduleId, -1, -1, -1, bookingCheckListDTO.ChecklistTaskGroupId, -1, true,
                                                                                        userName, utilities.getServerTime(), userName, utilities.getServerTime(), "",
                                                                                        siteId, false, -1, bookingCheckListDTO.BookingId, bookingCheckListDTO.BookingCheckListId);
                    JobScheduleTasksBL jobScheduleTasksBL = new JobScheduleTasksBL(executionContext, jobScheduleTasksDTO);
                    jobScheduleTasksBL.Save(sqlTrx);
                }
            }
            log.LogMethodExit();
        }



        /// <summary>
        /// GetReservationCheckList
        /// </summary>
        /// <returns></returns>
        public List<UserJobItemsDTO> GetReservationCheckList()
        {
            log.LogMethodEntry();
            DTONullCheck();
            List<UserJobItemsDTO> userJobItemsDTOList = null;
            if (this.reservationDTO.BookingId > -1)
            {
                JobScheduleTasksListBL jobScheduleTasksListBL = new JobScheduleTasksListBL(executionContext);
                List<KeyValuePair<JobScheduleTasksDTO.SearchByJobScheduleTaskParameters, string>> searchParameters = new List<KeyValuePair<JobScheduleTasksDTO.SearchByJobScheduleTaskParameters, string>>();
                searchParameters.Add(new KeyValuePair<JobScheduleTasksDTO.SearchByJobScheduleTaskParameters, string>(JobScheduleTasksDTO.SearchByJobScheduleTaskParameters.BOOKING_ID, this.reservationDTO.BookingId.ToString()));
                searchParameters.Add(new KeyValuePair<JobScheduleTasksDTO.SearchByJobScheduleTaskParameters, string>(JobScheduleTasksDTO.SearchByJobScheduleTaskParameters.IS_ACTIVE, "1"));
                searchParameters.Add(new KeyValuePair<JobScheduleTasksDTO.SearchByJobScheduleTaskParameters, string>(JobScheduleTasksDTO.SearchByJobScheduleTaskParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                List<JobScheduleTasksDTO> jobScheduleTasksDTOList = jobScheduleTasksListBL.GetAllJobScheduleTaskDTOList(searchParameters);
                if (jobScheduleTasksDTOList != null && jobScheduleTasksDTOList.Count > 0)
                {
                    foreach (JobScheduleTasksDTO item in jobScheduleTasksDTOList)
                    {
                        UserJobItemsListBL userJobItemsListBL = new UserJobItemsListBL(executionContext);
                        List<KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>> searchPara = new List<KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>>();
                        searchPara.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.JOB_SCHEDULE_ID, item.JobScheduleId.ToString()));
                        searchPara.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        searchPara.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.IS_ACTIVE, "1"));
                        List<UserJobItemsDTO> tempDTOList = userJobItemsListBL.GetAllUserJobItemDTOList(searchPara, -1);
                        if (tempDTOList != null && tempDTOList.Any())
                        {
                            if (userJobItemsDTOList == null)
                            {
                                userJobItemsDTOList = new List<UserJobItemsDTO>();
                            }
                            userJobItemsDTOList.AddRange(new List<UserJobItemsDTO>(tempDTOList));
                        }
                    }

                }
            }
            log.LogMethodExit(userJobItemsDTOList);
            return userJobItemsDTOList;
        }

        /// <summary>
        /// GetBookingProduct
        /// </summary>
        /// <returns></returns>
        public Products GetBookingProduct()
        {
            log.LogMethodEntry();
            Products productsBL = null;
            if (ReservationTransactionIsNotNull())
            {
                productsBL = this.bookingTransaction.GetBookingProduct();
                if (productsBL == null && this.reservationDTO != null && this.reservationDTO.Status == ReservationDTO.ReservationStatus.CANCELLED.ToString()
                    && this.reservationDTO.BookingProductId > -1)
                {
                    productsBL = new Products(executionContext, this.reservationDTO.BookingProductId);
                }
            }
            log.LogMethodExit(productsBL);
            return productsBL;
        }


        /// <summary>
        /// GetAppliedDiscountCouponForBooking
        /// </summary>
        /// <returns></returns>
        public DiscountContainerDTO GetAppliedDiscountCouponForBooking()
        {
            log.LogMethodEntry();
            DiscountContainerDTO discountContainerDTO = null;
            if (ReservationTransactionIsNotNull())
            {
                discountContainerDTO = this.bookingTransaction.GetAppliedDiscountCouponForReservation();
            }
            log.LogMethodExit(discountContainerDTO);
            return discountContainerDTO;
        }

        /// <summary>
        /// GetAppliedDiscountForBooking
        /// </summary>
        /// <returns></returns>
        public DiscountContainerDTO GetAppliedDiscountForBooking()
        {
            log.LogMethodEntry();
            DiscountContainerDTO discountContainerDTO = null;
            if (ReservationTransactionIsNotNull())
            {
                discountContainerDTO = this.bookingTransaction.GetAppliedDiscountForReservation();
            }
            log.LogMethodExit(discountContainerDTO);
            return discountContainerDTO;
        }

        /// <summary>
        /// GetBookingDetailsForEmail
        /// </summary>
        /// <param name="template"></param>
        /// <param name="contentId"></param>
        /// <returns></returns>
        public string GetBookingDetailsForEmail(string template, string contentId)
        {
            log.LogMethodEntry(template);
            string bookingDetails = string.Empty;
            try
            {
                if (ReservationTransactionIsNotNull())
                {
                    double fromtime = Convert.ToDouble(reservationDTO.FromDate.Hour + reservationDTO.FromDate.Minute / 100.0);
                    double min = (fromtime - Convert.ToInt32(fromtime)) * 100;
                    fromtime = Convert.ToInt32(fromtime) * 60 + min;

                    double totime = Convert.ToDouble(reservationDTO.ToDate.Hour + reservationDTO.ToDate.Minute / 100.0);
                    min = (totime - Convert.ToInt32(totime)) * 100;
                    totime = Convert.ToInt32(totime) * 60 + min;

                    string bookingProductContents = "";
                    string additionalproducts = "";
                    string facilityName = "";
                    string advancePaidDate = "";

                    //DataTable dtBookingInformation = Utilities.executeDataTable(@"SELECT CF.FacilityName, TP.PaymentDate
                    //                                                          FROM BOOKINGS B LEFT OUTER JOIN TRXPAYMENTS TP ON B.TRXID = TP.TRXID, 
                    //                                                               AttractionSchedules ATS, CheckInFacility CF
                    //                                                         WHERE B.AttractionScheduleId = ATS.AttractionScheduleId
                    //                                                           AND ATS.FacilityID = CF.FacilityId
                    //                                                           AND B.BookingId = @BookingId",
                    //                                                            new SqlParameter("@BookingId", BookingId));
                    //if (dtBookingInformation.Rows.Count > 0)
                    //{
                    //    if (dtBookingInformation.Rows[0]["FacilityName"] != DBNull.Value)
                    //        facilityName = dtBookingInformation.Rows[0]["FacilityName"].ToString();

                    //    if (dtBookingInformation.Rows[0]["PaymentDate"] != DBNull.Value)
                    //        advancePaidDate = Convert.ToDateTime(dtBookingInformation.Rows[0]["PaymentDate"]).ToString(ParafaitEnv.DATETIME_FORMAT);
                    //}
                    facilityName = this.bookingTransaction.GetReservationFacilities(reservationDTO.Status);
                    //if (facilityList != null && facilityList.Count > 0)
                    //{
                    //    for (int i = 0; i < facilityList.Count; i++)
                    //    {
                    //        if (i == facilityList.Count - 1)
                    //        {
                    //            facilityName = facilityName + facilityList[i];
                    //        }
                    //        else
                    //        {
                    //            facilityName = facilityName + facilityList[i] + ", ";
                    //        }
                    //    }
                    //}

                    if (this.bookingTransaction.TransactionPaymentsDTOList != null && this.bookingTransaction.TransactionPaymentsDTOList.Count > 0)
                    {
                        DateTime paidDate = this.bookingTransaction.TransactionPaymentsDTOList.Min(tp => tp.PaymentDate);
                        advancePaidDate = paidDate.ToString(utilities.ParafaitEnv.DATETIME_FORMAT);
                    }

                    bookingProductContents = GeneratePkgProductContent();
                    additionalproducts = GenerateAdditionalProductContent();
                    Transaction.TransactionLine bookingProductLine = this.bookingTransaction.GetBookingProductTransactionLine();

                    string address = string.Empty;
                    if (this.bookingTransaction.customerDTO != null &&
                         this.bookingTransaction.customerDTO.AddressDTOList != null &&
                         this.bookingTransaction.customerDTO.AddressDTOList.Count > 0)
                    {
                        address = this.bookingTransaction.customerDTO.AddressDTOList[0].Line1;
                    }
                    CustomerDTO customerDTO = this.bookingTransaction.customerDTO;
                    string firstName = "";
                    string phoneNumber = "";
                    string bookingEstimate = "";
                    string transactionAmount = "";
                    string paidAmount = "";
                    string discountAmount = "";
                    string balanceAmount = "";
                    string taxAmount = "";
                    if (this.bookingTransaction.customerDTO != null)
                    {
                        firstName = this.bookingTransaction.customerDTO.FirstName;
                        phoneNumber = this.bookingTransaction.customerDTO.PhoneNumber;
                    }
                    if (this.bookingTransaction != null)
                    {
                        transactionAmount = this.bookingTransaction.Transaction_Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                        bookingEstimate = this.bookingTransaction.Net_Transaction_Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                        discountAmount = this.bookingTransaction.Discount_Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                        paidAmount = this.bookingTransaction.TotalPaidAmount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                        balanceAmount = Math.Round(this.bookingTransaction.Net_Transaction_Amount - this.bookingTransaction.TotalPaidAmount, utilities.ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero).ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                        taxAmount = this.bookingTransaction.Tax_Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                    }
                    bookingDetails = template.Replace("@customerName", firstName).Replace("@address", address).Replace("@phoneNumber", phoneNumber).Replace("@bookingName", reservationDTO.BookingName)
                                                    .Replace("@emailAddress", customerDTO.Email).Replace("@reservationCode", reservationDTO.ReservationCode).Replace("@fromDate", reservationDTO.FromDate.ToString(utilities.ParafaitEnv.DATE_FORMAT))
                                                    .Replace("@fromTime", utilities.getServerTime().Date.AddMinutes(fromtime).ToString("h:mm tt")).Replace("@toTime", utilities.getServerTime().Date.AddMinutes(totime).ToString("h:mm tt"))
                                                    .Replace("@bookingProduct", bookingProductLine.ProductName).Replace("@guestCount", reservationDTO.Quantity.ToString(utilities.ParafaitEnv.NUMBER_FORMAT))//g nudGuestCount.Value.ToString())
                                                    .Replace("@ProductName", bookingProductContents).Replace("@estimateAmount", bookingEstimate).Replace("@transactionAmount", transactionAmount)
                                                    .Replace("@advancePaid", paidAmount).Replace("@discountAmount", discountAmount)
                                                    .Replace("@balanceDue", balanceAmount).Replace("@advancePaidDate", advancePaidDate).Replace("@partyRoom", facilityName)
                                                    .Replace("@additionalItems", additionalproducts).Replace("@taxAmount", taxAmount)
                                                    .Replace("@remarks", reservationDTO.Remarks).Replace("@siteName", utilities.ParafaitEnv.SiteName).Replace("@siteAddress", utilities.ParafaitEnv.SiteAddress)
                                                    .Replace("@status", reservationDTO.Status).Replace("@sitelogo", "<img src=\"cid:" + contentId + "\">");

                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null);
                return null;
            }
            log.LogMethodExit(bookingDetails);
            return bookingDetails;
        }

        private string GeneratePkgProductContent()
        {
            log.LogMethodEntry();
            string bookingProductContents = "";
            List<Transaction.TransactionLine> purchasedPkgProductList = GetPurchasedPackageProducts();
            if (purchasedPkgProductList != null && purchasedPkgProductList.Count > 0)
            {
                purchasedPkgProductList = purchasedPkgProductList.Where(tl => tl.LineValid == true && tl.ComboproductId != -1).ToList();
                if (purchasedPkgProductList != null && purchasedPkgProductList.Count > 0)
                {
                    purchasedPkgProductList = purchasedPkgProductList.OrderBy(tl => tl.ProductName).ThenBy(tl => tl.Remarks).ToList();
                }
            }

            if (purchasedPkgProductList != null && purchasedPkgProductList.Count > 0)
            {
                List<int> disinctParentPackageProductId = purchasedPkgProductList.Select(tl => tl.ProductID).Distinct().ToList();
                foreach (int purchasedPkgProductId in disinctParentPackageProductId)
                {
                    decimal productQty = purchasedPkgProductList.Where(tl => tl.ProductID == purchasedPkgProductId).Sum(tlin => tlin.quantity);
                    Transaction.TransactionLine productLine = purchasedPkgProductList.Find(tl => tl.ProductID == purchasedPkgProductId);

                    bookingProductContents += "\t\t " + productLine.ProductName + " [Qty.: " + productQty.ToString() + "]" + Environment.NewLine;
                    string remarks = "";
                    foreach (Transaction.TransactionLine item in purchasedPkgProductList.Where(tl => tl.ProductID == purchasedPkgProductId))
                    {
                        if (string.IsNullOrEmpty(item.Remarks) == false)
                        {
                            if (remarks != item.Remarks)
                            {
                                remarks = item.Remarks;
                                bookingProductContents += " " + remarks;
                            }
                        }
                    }
                    bookingProductContents += Environment.NewLine;
                }
            }
            log.LogMethodExit();
            return bookingProductContents;
        }

        private string GenerateAdditionalProductContent()
        {
            log.LogMethodEntry();
            //foreach (DataRow additional in dtadditionalProductDetails.Rows)
            //{
            //    try
            //    {
            //        //additionalproducts += "\t Product Name: [" + additional["product_name"].ToString() + "]\t Product Price: [" + Math.Round(Convert.ToDouble(additional["price"].ToString())) + "]\t \t Product Quantity: [" + Convert.ToDouble(additional["quantity"].ToString()) + "]" + Environment.NewLine;
            //       additionalproducts += "\t " + additional["product_name"].ToString() + " [Price: " + additional["price"].ToString() + "]" + " [QTY.:" + Convert.ToDouble(additional["quantity"].ToString()) + "]" + Environment.NewLine;
            //        if (!string.IsNullOrEmpty(additional["dcRemarks"].ToString()))
            //            additionalproducts += " " + additional["dcRemarks"].ToString();
            //        additionalproducts += Environment.NewLine;
            //    }
            //    catch (Exception ex)
            //    {
            //        log.Error(ex);
            //    }
            //}
            string additionalproducts = "";
            List<Transaction.TransactionLine> purchasedAdditionalProducts = GetPurchasedAdditionalProducts();
            if (purchasedAdditionalProducts != null && purchasedAdditionalProducts.Count > 0)
            {
                purchasedAdditionalProducts = purchasedAdditionalProducts.Where(tl => tl.LineValid == true && tl.ComboproductId != -1).ToList();
                if (purchasedAdditionalProducts != null && purchasedAdditionalProducts.Count > 0)
                {
                    purchasedAdditionalProducts = purchasedAdditionalProducts.OrderBy(tl => tl.ProductName).ThenBy(tl => tl.Remarks).ToList();
                }
            }

            if (purchasedAdditionalProducts != null && purchasedAdditionalProducts.Count > 0)
            {
                List<int> disinctParentAdditionalProductId = purchasedAdditionalProducts.Select(tl => tl.ProductID).Distinct().ToList();
                foreach (int purchasedAdditionalProductId in disinctParentAdditionalProductId)
                {
                    decimal productQty = purchasedAdditionalProducts.Where(tl => tl.ProductID == purchasedAdditionalProductId).Sum(tlin => tlin.quantity);
                    List<double> productPriceList = purchasedAdditionalProducts.Where(tl => tl.ProductID == purchasedAdditionalProductId).Select(tl => tl.Price).Distinct().ToList();
                    Transaction.TransactionLine productLine = purchasedAdditionalProducts.Find(tl => tl.ProductID == purchasedAdditionalProductId);

                    additionalproducts += "\t\t " + productLine.ProductName + " [Qty.: " + productQty.ToString() + "]" + Environment.NewLine;
                    foreach (decimal additionalProdPrice in productPriceList)
                    {
                        additionalproducts += "\t " + productLine.ProductName + " [Price: " + additionalProdPrice.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) + "]" + " [QTY.:" + productQty.ToString() + "]" + Environment.NewLine;
                    }

                    string remarks = "";
                    foreach (Transaction.TransactionLine item in purchasedAdditionalProducts.Where(tl => tl.ProductID == purchasedAdditionalProductId))
                    {
                        if (string.IsNullOrEmpty(item.Remarks) == false)
                        {
                            if (remarks != item.Remarks)
                            {
                                remarks = item.Remarks;
                                additionalproducts += " " + remarks;
                            }
                        }
                    }
                    additionalproducts += Environment.NewLine;
                }
            }
            log.LogMethodExit();
            return additionalproducts;
        }

        private void LogEvent(bool editedBooking = false, SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(editedBooking);
            try
            {
                if (ReservationTransactionIsNotNull())
                {
                    string fromTime = reservationDTO.FromDate.ToString(utilities.ParafaitEnv.DATETIME_FORMAT);
                    string toTime = reservationDTO.ToDate.ToString(utilities.ParafaitEnv.DATETIME_FORMAT);
                    string gender = string.Empty;
                    if (this.bookingTransaction != null && bookingTransaction.customerDTO != null)
                    {
                        gender = bookingTransaction.customerDTO.Gender;
                    }
                    if (editedBooking)
                    {
                        log.Info("Edited Booking is saved successfully, Booking Id is " + reservationDTO.BookingId.ToString());
                        //Edited Booking is saved sucessfully. Reservation code is &1. Booking status is &2
                        audit.logEvent(MessageContainerList.GetMessage(executionContext, "Reservation"), 'D', executionContext.GetUserId(),
                            MessageContainerList.GetMessage(executionContext, 2170, reservationDTO.ReservationCode, reservationDTO.Status),
                            MessageContainerList.GetMessage(executionContext, "SaveBooking"), 0, "", reservationDTO.Guid.ToString(), sqlTrx);
                        Transaction.TransactionLine updatedBookingTransactionLines = this.bookingTransaction.GetBookingProductTransactionLine();
                        Transaction.TransactionLine orginalBookingTransactionLines = GetOriginalBookingProductTranasctionLine();
                        string userLoginId = executionContext.GetUserId();
                        if (orginalBookingTransactionLines != null && updatedBookingTransactionLines != null)
                        {
                            if (orginalBookingTransactionLines.ProductID != updatedBookingTransactionLines.ProductID)
                            {
                                audit.logEvent(MessageContainerList.GetMessage(executionContext, "Reservation"), 'D', userLoginId,
                                    MessageContainerList.GetMessage(executionContext, 2171, orginalBookingTransactionLines.ProductName, updatedBookingTransactionLines.ProductName),
                                    MessageContainerList.GetMessage(executionContext, "SaveBooking"), 0, "", reservationDTO.Guid.ToString(), sqlTrx);
                                //"Booking Product is changed from " + orginalBookingTransactionLines.ProductName + " to " + updatedBookingTransactionLines.ProductName
                            }
                        }
                        List<Transaction.TransactionLine> updatedScheduleTransactionLines = GetScheduleTransactionLines();
                        List<Transaction.TransactionLine> orginalScheduleTransactionLines = GetOriginalScheduleTransactionLines();
                        if (orginalScheduleTransactionLines != null && orginalScheduleTransactionLines.Count > 0 && updatedScheduleTransactionLines != null)
                        {
                            foreach (Transaction.TransactionLine updatedScheduleLine in updatedScheduleTransactionLines)
                            {
                                if (updatedScheduleLine.LineValid && !orginalScheduleTransactionLines.Exists(tl => tl.LineValid == updatedScheduleLine.LineValid && tl.ProductID == updatedScheduleLine.ProductID
                                                                                 && tl.GetCurrentTransactionReservationScheduleDTO() != null
                                                                                 && updatedScheduleLine.GetCurrentTransactionReservationScheduleDTO() != null
                                                                                 && tl.GetCurrentTransactionReservationScheduleDTO().SchedulesId == updatedScheduleLine.GetCurrentTransactionReservationScheduleDTO().SchedulesId
                                                                                 && tl.GetCurrentTransactionReservationScheduleDTO().FacilityMapId == updatedScheduleLine.GetCurrentTransactionReservationScheduleDTO().FacilityMapId
                                                                                 && tl.GetCurrentTransactionReservationScheduleDTO().GuestQuantity == updatedScheduleLine.GetCurrentTransactionReservationScheduleDTO().GuestQuantity
                                                                                 && tl.GetCurrentTransactionReservationScheduleDTO().ScheduleFromDate == updatedScheduleLine.GetCurrentTransactionReservationScheduleDTO().ScheduleFromDate
                                                                                 && tl.GetCurrentTransactionReservationScheduleDTO().ScheduleToDate == updatedScheduleLine.GetCurrentTransactionReservationScheduleDTO().ScheduleToDate))
                                {
                                    TransactionReservationScheduleDTO updatedTRSDTO = updatedScheduleLine.GetCurrentTransactionReservationScheduleDTO();
                                    string modifiedData = MessageContainerList.GetMessage(executionContext, "Updated schedule: ") + updatedTRSDTO.FacilityMapName + " : "
                                                          + updatedTRSDTO.GuestQuantity.ToString(utilities.ParafaitEnv.NUMBER_FORMAT) + " : "
                                                          + updatedTRSDTO.ScheduleFromDate.ToString(utilities.ParafaitEnv.DATETIME_FORMAT) + " - "
                                                          + updatedTRSDTO.ScheduleToDate.ToString(utilities.ParafaitEnv.DATETIME_FORMAT);
                                    audit.logEvent(MessageContainerList.GetMessage(executionContext, "Reservation"), 'D', userLoginId, modifiedData,
                                        MessageContainerList.GetMessage(executionContext, "SaveBooking"), 0, "", reservationDTO.Guid.ToString(), sqlTrx);
                                }
                            }
                        }
                        List<Transaction.TransactionLine> updatedPkgTransactionLines = GetPurchasedPackageProducts();
                        List<Transaction.TransactionLine> orginalPkgTransactionLines = GetOriginalPurchasedPackageProducts();
                        List<Transaction.TransactionLine> updatedAdditionalProdTransactionLines = GetPurchasedAdditionalProducts();
                        List<Transaction.TransactionLine> orginalAdditionalProdTransactionLines = GetOrginalPurchasedAdditionalProducts();

                        Products bookingProduct = GetBookingProduct();
                        List<ComboProductDTO> comboProductDTOs = bookingProduct.GetComboProductSetup(false);
                        if (comboProductDTOs != null && comboProductDTOs.Count > 0)
                        {
                            List<ComboProductDTO> pkgProductsList = comboProductDTOs.Where(cmbP => cmbP.AdditionalProduct == false).ToList();
                            LogProductChanges(userLoginId, updatedPkgTransactionLines, orginalPkgTransactionLines, pkgProductsList, sqlTrx);

                            List<ComboProductDTO> additionalProductsList = comboProductDTOs.Where(cmbP => cmbP.AdditionalProduct == true).ToList();
                            LogProductChanges(userLoginId, updatedAdditionalProdTransactionLines, orginalAdditionalProdTransactionLines, additionalProductsList, sqlTrx);
                        }
                        //Reservation level discounts
                        LogDiscountChanges(userLoginId, -1, -1, -1, -1, sqlTrx);

                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit();
        }

        private void LogProductChanges(string userLoginId, List<Transaction.TransactionLine> updatedProTransactionLines, List<Transaction.TransactionLine> orginalProdTransactionLines, List<ComboProductDTO> pkgProductsList, SqlTransaction sqltrx)
        {
            log.LogMethodEntry(userLoginId, updatedProTransactionLines, orginalProdTransactionLines, pkgProductsList);
            if (pkgProductsList != null && pkgProductsList.Count > 0
                //&& updatedProTransactionLines != null && updatedProTransactionLines.Any()
                )
            {
                if (orginalProdTransactionLines != null && orginalProdTransactionLines.Count > 0)
                {
                    foreach (ComboProductDTO pkgProductItem in pkgProductsList)
                    {
                        List<Transaction.TransactionLine> updatedPkgProdTrxLines = updatedProTransactionLines.Where(tl => tl.LineValid == true
                                                                                                                          && tl.ProductID == pkgProductItem.ChildProductId
                                                                                                                          && tl.ComboproductId == pkgProductItem.ComboProductId).ToList();
                        List<Transaction.TransactionLine> orginalPkgProdTrxLines = orginalProdTransactionLines.Where(tl => tl.LineValid == true
                                                                                                                          && tl.ProductID == pkgProductItem.ChildProductId
                                                                                                                          && tl.ComboproductId == pkgProductItem.ComboProductId).ToList();
                        if (updatedPkgProdTrxLines != null && updatedPkgProdTrxLines.Count > 0)
                        {
                            if (orginalPkgProdTrxLines != null && orginalPkgProdTrxLines.Count > 0)
                            {
                                if (updatedPkgProdTrxLines.Count != orginalPkgProdTrxLines.Count)
                                {
                                    //updatedPkgProdTrxLines[0].ProductName + " quantity is changed from " + orginalPkgProdTrxLines.Count + " to " + updatedPkgProdTrxLines.Count
                                    audit.logEvent(MessageContainerList.GetMessage(executionContext, "Reservation"), 'D', userLoginId,
                                        MessageContainerList.GetMessage(executionContext, 2172, updatedPkgProdTrxLines[0].ProductName, orginalPkgProdTrxLines.Count, updatedPkgProdTrxLines.Count),
                                                                MessageContainerList.GetMessage(executionContext, "SaveBooking"), 0, "", reservationDTO.Guid.ToString(), sqltrx);
                                }
                                LogDiscountChanges(userLoginId, updatedPkgProdTrxLines[0].ProductID, updatedPkgProdTrxLines[0].ComboproductId,
                                                             orginalPkgProdTrxLines[0].ProductID, orginalPkgProdTrxLines[0].ComboproductId, sqltrx);
                            }
                            else
                            {
                                // "Newly added product :" + updatedPkgProdTrxLines[0].ProductName + " quantity : " + updatedPkgProdTrxLines.Count
                                audit.logEvent(MessageContainerList.GetMessage(executionContext, "Reservation"), 'D', userLoginId,
                                    MessageContainerList.GetMessage(executionContext, 2176, updatedPkgProdTrxLines[0].ProductName, updatedPkgProdTrxLines.Count),
                                             MessageContainerList.GetMessage(executionContext, "SaveBooking"), 0, "", reservationDTO.Guid.ToString(), sqltrx);
                            }

                        }
                        else if (orginalPkgProdTrxLines != null && orginalPkgProdTrxLines.Count > 0)
                        {
                            //"Removed Product :" + orginalPkgProdTrxLines[0].ProductName + " quantity : " + orginalPkgProdTrxLines.Count,
                            audit.logEvent(MessageContainerList.GetMessage(executionContext, "Reservation"), 'D', userLoginId,
                                MessageContainerList.GetMessage(executionContext, 2177, orginalPkgProdTrxLines[0].ProductName, orginalPkgProdTrxLines.Count),
                                            MessageContainerList.GetMessage(executionContext, "SaveBooking"), 0, "", reservationDTO.Guid.ToString(), sqltrx);
                        }

                    }
                }
                else
                {
                    foreach (ComboProductDTO pkgProductItem in pkgProductsList)
                    {
                        List<Transaction.TransactionLine> updatedPkgProdTrxLines = updatedProTransactionLines.Where(tl => tl.LineValid == true
                                                                                                                          && tl.ProductID == pkgProductItem.ChildProductId
                                                                                                                          && tl.ComboproductId == pkgProductItem.ComboProductId).ToList();
                        if (updatedPkgProdTrxLines != null && updatedPkgProdTrxLines.Count > 0)
                        {
                            //"Newly added product :" + updatedPkgProdTrxLines[0].ProductName + " quantity : " + updatedPkgProdTrxLines.Count,
                            audit.logEvent(MessageContainerList.GetMessage(executionContext, "Reservation"), 'D', userLoginId,
                                MessageContainerList.GetMessage(executionContext, 2176, updatedPkgProdTrxLines[0].ProductName, updatedPkgProdTrxLines.Count),
                                       MessageContainerList.GetMessage(executionContext, "SaveBooking"), 0, "", reservationDTO.Guid.ToString(), sqltrx);

                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        private void LogDiscountChanges(string userLoginId, int lineProductId, int lineComboProductId, int orginalLineProductId, int originalLineComboProductId, SqlTransaction sqltrx)
        {
            log.LogMethodEntry(userLoginId, lineProductId, lineComboProductId, orginalLineProductId, originalLineComboProductId, sqltrx);
            List<DiscountContainerDTO> applicableDiscountsContainerDTOlist = GetAppliedDiscountInfo(lineProductId, lineComboProductId);
            List<DiscountContainerDTO> orginalApplicableDiscountsContainerDTOList = GetOrginalAppliedDiscountInfo(orginalLineProductId, originalLineComboProductId);
            string productName = string.Empty;
            if (lineProductId > -1)
            {
                productName = this.bookingTransaction.TrxLines.Find(tl => tl.ProductID == lineProductId).ProductName;
            }
            if (applicableDiscountsContainerDTOlist != null && applicableDiscountsContainerDTOlist.Count > 0)
            {
                //Get distinct records only
                applicableDiscountsContainerDTOlist = applicableDiscountsContainerDTOlist.GroupBy(disc => new { disc.DiscountId, disc.DiscountAmount, disc.DiscountPercentage }).Select(g => g.First()).ToList();
                foreach (DiscountContainerDTO applicableDiscountItem in applicableDiscountsContainerDTOlist)
                {
                    if (orginalApplicableDiscountsContainerDTOList != null && orginalApplicableDiscountsContainerDTOList.Count > 0)
                    {
                        if (orginalApplicableDiscountsContainerDTOList.Exists(disc => disc.DiscountId == applicableDiscountItem.DiscountId) == false)
                        {
                            if (lineProductId > -1)
                            {
                                //"Product level discount is changed for " + updatedPkgProdTrxLines[0].ProductName 
                                audit.logEvent(MessageContainerList.GetMessage(executionContext, "Reservation"), 'D', userLoginId,
                                    MessageContainerList.GetMessage(executionContext, 2173, productName),
                                       MessageContainerList.GetMessage(executionContext, "SaveBooking"), 0, "", reservationDTO.Guid.ToString(), sqltrx);
                            }
                            else
                            {
                                //&1 is applied to this &2
                                audit.logEvent(MessageContainerList.GetMessage(executionContext, "Reservation"), 'D', userLoginId,
                                          MessageContainerList.GetMessage(executionContext, 2716, applicableDiscountItem.DiscountName,
                                                     MessageContainerList.GetMessage(executionContext, "reservation")),
                                          MessageContainerList.GetMessage(executionContext, "SaveBooking"), 0, "", reservationDTO.Guid.ToString(), sqltrx);
                            }
                        }
                    }
                    else
                    {
                        if (lineProductId > -1)
                        {
                            //"Product level discount is added for " + updatedPkgProdTrxLines[0].ProductName
                            audit.logEvent(MessageContainerList.GetMessage(executionContext, "Reservation"), 'D', userLoginId,
                                MessageContainerList.GetMessage(executionContext, 2174, productName),
                                      MessageContainerList.GetMessage(executionContext, "SaveBooking"), 0, "", reservationDTO.Guid.ToString(), sqltrx);
                        }
                        else
                        {
                            //&1 is applied to this &2
                            audit.logEvent(MessageContainerList.GetMessage(executionContext, "Reservation"), 'D', userLoginId,
                                      MessageContainerList.GetMessage(executionContext, 2716, applicableDiscountItem.DiscountName,
                                                 MessageContainerList.GetMessage(executionContext, "reservation")),
                                      MessageContainerList.GetMessage(executionContext, "SaveBooking"), 0, "", reservationDTO.Guid.ToString(), sqltrx);
                        }
                    }
                }
            }
            else if (orginalApplicableDiscountsContainerDTOList != null && orginalApplicableDiscountsContainerDTOList.Count > 0)
            {
                if (lineProductId > -1)
                {
                    //"Product level discount is removed from " + updatedPkgProdTrxLines[0].ProductName
                    audit.logEvent(MessageContainerList.GetMessage(executionContext, "Reservation"), 'D', userLoginId,
                        MessageContainerList.GetMessage(executionContext, 2175, productName),
                                      MessageContainerList.GetMessage(executionContext, "SaveBooking"), 0, "", reservationDTO.Guid.ToString(), sqltrx);
                }
                else
                {
                    foreach (DiscountContainerDTO OrigDiscountItem in orginalApplicableDiscountsContainerDTOList)
                    {
                        //&1 is no longer applicable to this &2
                        audit.logEvent(MessageContainerList.GetMessage(executionContext, "Reservation"), 'D', userLoginId,
                        MessageContainerList.GetMessage(executionContext, 2715, OrigDiscountItem.DiscountName,
                                     MessageContainerList.GetMessage(executionContext, "reservation")),
                           MessageContainerList.GetMessage(executionContext, "SaveBooking"), 0, "", reservationDTO.Guid.ToString(), sqltrx);
                    }

                }
            }
            log.LogMethodExit();
        }

        public void IncrementNSaveEmailSentCount()
        {
            log.LogMethodEntry();
            int emailSent = this.reservationDTO.IsEmailSent;
            this.reservationDTO.IsEmailSent = emailSent + 1;
            ReservationDatahandler reservationDatahandler = new ReservationDatahandler(null);
            reservationDatahandler.UpdateReservationDTO(reservationDTO, executionContext.GetUserId(), executionContext.GetSiteId());
            reservationDTO.AcceptChanges();
            log.LogMethodExit();
        }

        /// <summary>
        /// Clear unsaved attraction schedules
        /// </summary>
        public void ClearUnSavedAttractionSchedules()
        {
            log.LogMethodEntry();
            if (this.bookingTransaction != null)
            {
                this.bookingTransaction.ClearUnSavedAttractionSchedules();
            }
            log.LogMethodExit();
        }

        public void RestoreReservationDTOFromBlockedStateToNew(DateTime? expiryTime, string previousBookingStatus)
        {
            log.LogMethodEntry(expiryTime, previousBookingStatus);
            this.reservationDTO.BookingId = -1;
            this.reservationDTO.ExpiryTime = expiryTime;
            this.reservationDTO.Status = previousBookingStatus;
            this.reservationDTO.TrxId = -1;
            if (this.bookingTransaction != null)
            {
                this.bookingTransaction.RestoreReservationTransactionFromBlockedStateToNew();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Check whether reservation has service charge details
        /// </summary>
        public bool HasServiceCharges()
        {
            log.LogMethodEntry();
            bool hasServiceCharges = false;
            if (this.bookingTransaction != null)
            {
                hasServiceCharges = bookingTransaction.HasServiceCharges();
            }
            log.LogMethodExit(hasServiceCharges);
            return hasServiceCharges;
        }
        /// <summary>
        /// GetAdvanceRequired for the booking
        /// </summary>
        /// <returns></returns>
        public decimal GetAdvanceRequired()
        {
            log.LogMethodEntry();
            decimal advanceRequired = 0;
            if (this.reservationDTO != null && this.bookingTransaction != null && this.bookingTransaction.TrxLines != null)
            {

                Products bookingProduct = GetBookingProduct();
                if (bookingProduct != null && bookingProduct.GetProductsDTO != null)
                {
                    log.LogVariableState("bookingProduct.GetProductsDTO.AdvanceAmount", bookingProduct.GetProductsDTO.AdvanceAmount);
                    log.LogVariableState("bookingProduct.GetProductsDTO.AdvancePercentage", bookingProduct.GetProductsDTO.AdvancePercentage);
                    if (bookingProduct.GetProductsDTO.AdvanceAmount != -1)
                    {
                        advanceRequired = bookingProduct.GetProductsDTO.AdvanceAmount;
                    }
                    else if (bookingProduct.GetProductsDTO.AdvancePercentage != -1)
                    {
                        decimal amount = this.bookingTransaction != null ? Convert.ToDecimal(this.bookingTransaction.Net_Transaction_Amount) : bookingProduct.GetProductsDTO.Price;
                        advanceRequired = bookingProduct.GetProductsDTO.AdvancePercentage * amount / 100;
                    }

                    advanceRequired = (this.bookingTransaction != null ? Math.Min(Convert.ToDecimal(this.bookingTransaction.Net_Transaction_Amount), advanceRequired) : advanceRequired);
                }
            }
            log.LogMethodExit(advanceRequired);
            return advanceRequired;
        }
        /// <summary>
        /// Set Customer Card
        /// </summary>
        /// <param name="card"></param>
        public void SetCustomerCard(Card card)
        {
            log.LogMethodEntry(card);
            if (card != null && card.card_id != -1)
            {
                ReservationIsInEditMode();
                if (this.reservationDTO != null)
                {
                    this.reservationDTO.CardId = card.card_id;
                    this.reservationDTO.CardNumber = card.CardNumber;
                    if (this.bookingTransaction != null)
                    {
                        this.bookingTransaction.SetPrimaryCard(card);
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Reset Customer Card
        /// </summary>
        public void ResetCustomerCard()
        {
            log.LogMethodEntry();
            ReservationIsInEditMode();
            if (this.reservationDTO != null && this.reservationDTO.CardId != -1)
            {
                log.LogVariableState("this.reservationDTO", this.reservationDTO);
                this.reservationDTO.CardId = -1;
                this.reservationDTO.CardNumber = string.Empty;
                if (this.bookingTransaction != null)
                {
                    this.bookingTransaction.ResetPrimaryCard();
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Waiver Signautre Required
        /// </summary>
        /// <returns>bool</returns>
        public bool WaiverSignatureRequired()
        {
            log.LogMethodEntry();
            bool waiverSignatureRequired = false;
            if (this.bookingTransaction != null)
            {
                waiverSignatureRequired = bookingTransaction.WaiverSignatureRequired();
            }
            log.LogMethodExit(waiverSignatureRequired);
            return waiverSignatureRequired;
        }

        /// <summary>
        /// Is Waiver Signature Pending
        /// </summary>
        /// <returns>bool</returns>
        public bool IsWaiverSignaturePending()
        {
            log.LogMethodEntry();
            bool waiverSignaturePending = false;
            if (this.bookingTransaction != null)
            {
                waiverSignaturePending = bookingTransaction.IsWaiverSignaturePending();
            }
            log.LogMethodExit(waiverSignaturePending);
            return waiverSignaturePending;
        }
        /// <summary>
        /// Clear UnSaved Reservation Schedules
        /// </summary>
        /// <param name="sqlTrx"></param>
        public void ClearUnSavedReservationSchedules(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            if (this.bookingTransaction != null)
            {
                this.bookingTransaction.ClearUnSavedReservationSchedules(sqlTrx);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Clear UnSaved Reservation Schedules/Attraction schedules
        /// </summary>
        /// <param name="sqlTrx"></param>
        public void ClearUnSavedSchedules(SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(sqlTrx);
            if (this.bookingTransaction != null)
            {
                this.bookingTransaction.ClearUnSavedSchedules(sqlTrx);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Validate Reschedule Reservation
        /// </summary>
        /// <param name="transactionReservationScheduleDTO"></param>
        /// <param name="allowOverride"></param>
        /// <returns></returns>
        public List<KeyValuePair<Transaction.TransactionLine, List<ValidationError>>> ValidateRescheduleReservation
            (bool allowOverride)
        {
            log.LogMethodEntry(allowOverride);
            Transaction.TransactionLine bookingProductTrxLine = GetBookingProductTransactionLine();
            Transaction.TransactionLine bookingProductScheduleTrxLine = GetBookingProductParentScheduleLine();
            TransactionReservationScheduleDTO transactionReservationScheduleDTO = bookingProductScheduleTrxLine.TransactionReservationScheduleDTOList.Find(trs => trs.Cancelled == false && trs.TrxId == -1);
            ValidateScheduleNQuantity(bookingProductTrxLine.ProductID, transactionReservationScheduleDTO, -1);
            List<KeyValuePair<Transaction.TransactionLine, List<ValidationError>>> validationErrorList = new List<KeyValuePair<Transaction.TransactionLine, List<ValidationError>>>();

            if (this.bookingTransaction != null)
            {
                validationErrorList = this.bookingTransaction.ValidateRescheduleReservation(allowOverride);
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        //private TransactionReservationScheduleDTO GetRescheduledBookingProductScheduleDTO(List<TransactionReservationScheduleDTO> transactionReservationScheduleDTOList, Transaction.TransactionLine bookingProductTrxLine)
        //{
        //    log.LogMethodEntry(transactionReservationScheduleDTOList, bookingProductTrxLine);
        //    TransactionReservationScheduleDTO rescheduledBookingProductSchedule = null; 
        //    List<Transaction.TransactionLine> scheduleTrxLineList = GetScheduleTransactionLines();
        //    Transaction.TransactionLine bookingProductScheduleLine = scheduleTrxLineList.Find(tl => tl == bookingProductTrxLine.ParentLine);
        //    int scheduleLineIndex = BookingTransaction.TrxLines.IndexOf(bookingProductScheduleLine);
        //    rescheduledBookingProductSchedule =
        //        transactionReservationScheduleDTOList.Find(rTRS => rTRS.LineId == (bookingProductScheduleLine.DBLineId > 0 ?
        //                                                                              bookingProductScheduleLine.DBLineId : scheduleLineIndex + 1));
        //    log.LogMethodExit(rescheduledBookingProductSchedule);
        //    return rescheduledBookingProductSchedule;
        //}

        /// <summary>
        /// Can Reschedule Reservation - throws exception if any validation fails
        /// </summary> 
        /// <param name="allowOverride"></param>
        public void CanRescheduleReservation(bool allowOverride)
        {
            log.LogMethodEntry(allowOverride);
            Transaction.TransactionLine bookingProductLine = GetBookingProductTransactionLine();
            if (bookingProductLine == null)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2169));//Transaction does not have a booking product
            }
            Transaction.TransactionLine bookingScheduleProductLine = GetBookingProductParentScheduleLine();
            TransactionReservationScheduleDTO rescheduledBookingProductSchedule = bookingScheduleProductLine.GetCurrentTransactionReservationScheduleDTO();
            ValidateScheduleNQuantity(bookingProductLine.ProductID, rescheduledBookingProductSchedule, -1);

            if (this.bookingTransaction != null)
            {
                this.bookingTransaction.CanRescheduleReservation(allowOverride);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Can Reschedule Reservation - throws exception if any validation fails
        /// </summary> 
        /// <param name="canOverride"></param>
        /// <param name="sqlTrx"></param>
        public void RescheduleReservation(bool canOverride, SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(canOverride, sqlTrx);
            Transaction.TransactionLine bookingProductLine = GetBookingProductTransactionLine();
            Transaction.TransactionLine bookingSchedukeProductLine = GetBookingProductParentScheduleLine();
            if (bookingProductLine == null)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2169));//Transaction does not have a booking product
            }
            TransactionReservationScheduleDTO rescheduledBookingProductScheduleDTO = bookingSchedukeProductLine.TransactionReservationScheduleDTOList.Find(trs => trs.Cancelled == false && trs.TrxId == -1);
            ValidateScheduleNQuantity(bookingProductLine.ProductID, rescheduledBookingProductScheduleDTO, -1);
            ParafaitDBTransaction dBTransaction = null;
            try
            {
                if (sqlTrx == null)
                {
                    dBTransaction = new ParafaitDBTransaction();
                    dBTransaction.BeginTransaction();
                    sqlTrx = dBTransaction.SQLTrx;
                }
                if (this.bookingTransaction != null)
                {
                    this.bookingTransaction.RescheduleReservation(canOverride, sqlTrx);
                }
                SaveReservationHeader(sqlTrx);
                LogEvent(true, sqlTrx);
                if (dBTransaction != null)
                {
                    dBTransaction.EndTransaction();
                    dBTransaction.Dispose();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (dBTransaction != null)
                {
                    dBTransaction.RollBack();
                    dBTransaction.Dispose();
                }
                throw;
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Verify Discount Changes On Reservation
        /// </summary>
        /// <returns></returns>
        public string VerifyDiscountChangesOnReservation()
        {
            log.LogMethodEntry();
            StringBuilder validationMessage = new StringBuilder();
            List<DiscountsSummaryDTO> discountSummaryDTOListBefore = new List<DiscountsSummaryDTO>(bookingTransactionBeforeEdit.DiscountsSummaryDTOList);
            List<DiscountsSummaryDTO> discountSummaryDTOListAfter = new List<DiscountsSummaryDTO>(BookingTransaction.DiscountsSummaryDTOList);
            foreach (DiscountsSummaryDTO oldDiscountsSummaryItem in discountSummaryDTOListBefore)
            {
                if (discountSummaryDTOListAfter != null
                    && discountSummaryDTOListAfter.Exists(discSum => discSum.DiscountId == oldDiscountsSummaryItem.DiscountId && discSum.Count > 0) == false)
                {
                    validationMessage = validationMessage.Append(MessageContainerList.GetMessage(executionContext, 2715, oldDiscountsSummaryItem.DiscountName,
                                                 MessageContainerList.GetMessage(executionContext, "reservation")) + Environment.NewLine);
                    //&1 is no longer applicable to this &2 
                }
            }
            foreach (DiscountsSummaryDTO newDiscountsSummaryItem in discountSummaryDTOListAfter)
            {
                if (newDiscountsSummaryItem.Count > 0 &&
                    discountSummaryDTOListBefore != null
                    && discountSummaryDTOListBefore.Exists(discSum => discSum.DiscountId == newDiscountsSummaryItem.DiscountId) == false)
                {
                    validationMessage = validationMessage.Append(MessageContainerList.GetMessage(executionContext, 2716, newDiscountsSummaryItem.DiscountName,
                                                 MessageContainerList.GetMessage(executionContext, "reservation")) + Environment.NewLine);
                    //&1 is applied to this &2 
                }
            }
            log.LogMethodExit(validationMessage.ToString());
            return validationMessage.ToString();
        }

        private SchedulesBL GetSchedulesBL(int schedulesId)
        {
            log.LogMethodEntry(schedulesId);
            SchedulesBL schedulesBL;
            if (schedulesBLDictionary.ContainsKey(schedulesId))
            {
                schedulesBL = schedulesBLDictionary[schedulesId];
            }
            else
            {
                schedulesBL = new SchedulesBL(executionContext, schedulesId, null, true);
                schedulesBLDictionary.Add(schedulesId, schedulesBL);
            }
            log.LogMethodExit(schedulesBL);
            return schedulesBL;
        }

        /// <summary>
        /// Get Booking Product Parent ScheduleLine
        /// </summary>
        /// <returns></returns>
        public Transaction.TransactionLine GetBookingProductParentScheduleLine()
        {
            log.LogMethodEntry();
            Transaction.TransactionLine bookingScheduleTrxLine = null;
            try
            {
                bookingScheduleTrxLine = this.bookingTransaction.GetBookingProductParentScheduleLine();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(ex.Message);
                throw;
            }
            log.LogMethodExit(bookingScheduleTrxLine);
            return bookingScheduleTrxLine;
        }

        /// <summary>
        /// HasTempCards?
        /// </summary>
        /// <returns>bool</returns>
        public bool HasTempCards()
        {
            log.LogMethodEntry();
            bool hasTempCards = false;
            if (this.bookingTransaction != null)
            {
                hasTempCards = this.bookingTransaction.TrxLines.Exists(tl => tl.LineValid && string.IsNullOrWhiteSpace(tl.CardNumber) == false
                                                                    && tl.CardNumber.StartsWith("T"));
            }
            log.LogMethodExit(hasTempCards);
            return hasTempCards;
        }
        /// <summary>
        /// ApplyServiceCharges
        /// </summary>
        /// <param name="sqlTrx"></param>
        public void ApplyServiceCharges(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            ParafaitDBTransaction dBTransaction = null;
            try
            {
                ReservationIsInEditMode();
                if (sqlTrx == null)
                {
                    dBTransaction = new ParafaitDBTransaction();
                    dBTransaction.BeginTransaction();
                    sqlTrx = dBTransaction.SQLTrx;
                }
                this.bookingTransaction.ApplyServiceCharges(sqlTrx);
                string msg = string.Empty;
                if (reservationDTO.BookingId > -1)
                {
                    if (this.bookingTransaction.SaveOrder(ref msg, sqlTrx) != 0)
                    {
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 4487, msg);
                        //'Error while saving service charge: &1. Transaction Save failed'
                        throw new Exception((errorMessage));
                    }

                    audit.logEvent(MessageContainerList.GetMessage(executionContext, "Reservation"), 'D', executionContext.GetUserId(),
                        MessageContainerList.GetMessage(executionContext, 4488, reservationDTO.ReservationCode),
                        MessageContainerList.GetMessage(executionContext, "ServiceCharge"), 0, "", reservationDTO.Guid.ToString(), dBTransaction.SQLTrx);
                    //'Service charge is manually applied for Reservation Code &1'
                }
                if (dBTransaction != null)
                {
                    dBTransaction.EndTransaction();
                    dBTransaction.Dispose();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (dBTransaction != null)
                {
                    dBTransaction.RollBack();
                    dBTransaction.Dispose();
                }
                throw;
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// ApplyAutoGratuityAmount
        /// </summary>
        /// <param name="sqlTrx"></param>
        public void ApplyAutoGratuityAmount(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            ParafaitDBTransaction dBTransaction = null;
            try
            {
                ReservationIsInEditMode();
                if (sqlTrx == null)
                {
                    dBTransaction = new ParafaitDBTransaction();
                    dBTransaction.BeginTransaction();
                    sqlTrx = dBTransaction.SQLTrx;
                }
                this.bookingTransaction.ApplyAutoGratuityAmount(sqlTrx);
                string msg = string.Empty;
                if (reservationDTO.BookingId > -1)
                {
                    if (this.bookingTransaction.SaveOrder(ref msg, sqlTrx) != 0)
                    {
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 4489, msg);
                        //'Error while saving gratuity amount: &1. Transaction Save failed'
                        throw new Exception((errorMessage));
                    }
                    audit.logEvent(MessageContainerList.GetMessage(executionContext, "Reservation"), 'D', executionContext.GetUserId(),
                        MessageContainerList.GetMessage(executionContext, 4490, reservationDTO.ReservationCode),
                        MessageContainerList.GetMessage(executionContext, "Gratuity"), 0, "", reservationDTO.Guid.ToString(), dBTransaction.SQLTrx);
                    //'Gratuity amount is manually applied for Reservation Code &1'
                }
                if (dBTransaction != null)
                {
                    dBTransaction.EndTransaction();
                    dBTransaction.Dispose();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (dBTransaction != null)
                {
                    dBTransaction.RollBack();
                    dBTransaction.Dispose();
                }
                throw;
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// CancelServiceChargeLine
        /// </summary>
        /// <param name="sqlTrx"></param>
        public void CancelServiceChargeLine(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            ParafaitDBTransaction dBTransaction = null;
            try
            {
                ReservationIsInEditMode();
                if (sqlTrx == null)
                {
                    dBTransaction = new ParafaitDBTransaction();
                    dBTransaction.BeginTransaction();
                    sqlTrx = dBTransaction.SQLTrx;
                }
                this.bookingTransaction.CancelServiceChargeLine(sqlTrx);
                string msg = string.Empty;
                if (reservationDTO.BookingId > -1)
                {
                    if (this.bookingTransaction.SaveOrder(ref msg, sqlTrx) != 0)
                    {
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 4491, msg);
                        //'Error while cancelling service charge: &1. Transaction Save failed'
                        throw new Exception((errorMessage));
                    }
                    audit.logEvent(MessageContainerList.GetMessage(executionContext, "Reservation"), 'D', executionContext.GetUserId(),
                                   MessageContainerList.GetMessage(executionContext, 4493, reservationDTO.ReservationCode),
                                   MessageContainerList.GetMessage(executionContext, "ServiceCharge"), 0, "", reservationDTO.Guid.ToString(), dBTransaction.SQLTrx);
                    //'Service charge is cancelled for Reservation Code &1' 
                }
                if (dBTransaction != null)
                {
                    dBTransaction.EndTransaction();
                    dBTransaction.Dispose();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (dBTransaction != null)
                {
                    dBTransaction.RollBack();
                    dBTransaction.Dispose();
                }
                throw;
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// CancelAutoGratuityLine
        /// </summary>
        /// <param name="sqlTrx"></param>
        public void CancelAutoGratuityLine(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            ParafaitDBTransaction dBTransaction = null;
            try
            {
                ReservationIsInEditMode();
                if (sqlTrx == null)
                {
                    dBTransaction = new ParafaitDBTransaction();
                    dBTransaction.BeginTransaction();
                    sqlTrx = dBTransaction.SQLTrx;
                }
                this.bookingTransaction.CancelAutoGratuityLine(sqlTrx);
                string msg = string.Empty;
                if (reservationDTO.BookingId > -1)
                {
                    if (this.bookingTransaction.SaveOrder(ref msg, sqlTrx) != 0)
                    {
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 4492, msg);
                        //'Error while cancelling gratuity amount: &1. Transaction Save failed'
                        throw new Exception((errorMessage));
                    }
                    audit.logEvent(MessageContainerList.GetMessage(executionContext, "Reservation"), 'D', executionContext.GetUserId(),
                                   MessageContainerList.GetMessage(executionContext, 4494, reservationDTO.ReservationCode),
                                   MessageContainerList.GetMessage(executionContext, "Gratuity"), 0, "", reservationDTO.Guid.ToString(), dBTransaction.SQLTrx);
                    //'Gratuity amount is cancelled for Reservation Code &1'
                }
                if (dBTransaction != null)
                {
                    dBTransaction.EndTransaction();
                    dBTransaction.Dispose();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (dBTransaction != null)
                {
                    dBTransaction.RollBack();
                    dBTransaction.Dispose();
                }
                throw;
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// GetGratuityAmountTransactionLine
        /// </summary>
        /// <returns></returns>
        public Transaction.TransactionLine GetGratuityAmountTransactionLine()
        {
            log.LogMethodEntry();
            try
            {
                Transaction.TransactionLine selectedTrxLines = null;
                if (this.reservationDTO != null && this.bookingTransaction != null)
                {
                    selectedTrxLines = this.bookingTransaction.GetActiveLineForType(ProductTypeValues.GRATUITY, null);
                }
                log.LogMethodExit(selectedTrxLines);
                return selectedTrxLines;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// GetAutoGratuityAmount
        /// </summary>
        /// <returns></returns>
        public double GetAutoGratuityAmount()
        {
            log.LogMethodEntry();
            double autoGratuityAmount = 0;
            if (ReservationTransactionIsNotNull())
            {
                Transaction.TransactionLine autoGratuityLine = GetGratuityAmountTransactionLine();
                if (autoGratuityLine != null)
                {
                    autoGratuityAmount = autoGratuityLine.LineAmount;
                }
            }
            log.LogMethodExit(autoGratuityAmount);
            return autoGratuityAmount;
        }
        /// <summary>
        /// Check whether reservation has service charge details
        /// </summary>
        public bool HasAutoGratuityAmount()
        {
            log.LogMethodEntry();
            bool hasGratuityAmount = false;
            if (this.bookingTransaction != null)
            {
                hasGratuityAmount = bookingTransaction.HasAutoGratuityAmount();
            }
            log.LogMethodExit(hasGratuityAmount);
            return hasGratuityAmount;
        }
        /// <summary>
        /// AutoApplyCharges
        /// </summary>
        /// <param name="sqlTrx"></param>
        public void AutoApplyCharges(SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(sqlTrx);
            bool autoApplyCharges = false;
            try
            {
                ReservationIsInEditMode();
                autoApplyCharges = true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                autoApplyCharges = false;
            }
            if (this.bookingTransaction != null && autoApplyCharges)
            {
                this.bookingTransaction.SetServiceCharges(sqlTrx);
                this.bookingTransaction.SetAutoGratuityAmount(sqlTrx);
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of Bookings
    /// </summary>
    /// 
    public class ReservationListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public ReservationListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// GetReservationDTOList
        /// </summary>
        /// <param name="searchParameters"></param> 
        /// <returns></returns>
        public List<ReservationDTO> GetReservationDTOList(List<KeyValuePair<ReservationDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(searchParameters);
            ReservationDatahandler reservationDatahandler = new ReservationDatahandler(sqlTrx);
            List<ReservationDTO> returnValue = reservationDatahandler.GetReservationDTOList(searchParameters, executionContext);
            log.LogMethodExit(returnValue);
            return returnValue;
        }
        /// <summary>
        /// Validate EventCode
        /// </summary>
        /// <param name="registrationCode"></param>
        /// <returns></returns>
        public bool ValidEventCode(string registrationCode)
        {
            log.LogMethodEntry(registrationCode);
            bool validCode = false;
            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            List<KeyValuePair<ReservationDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<ReservationDTO.SearchByParameters, string>>();
            searchParams.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.RESERVATION_CODE_EXACT, registrationCode));
            searchParams.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.STATUS_LIST_NOT_IN, "'" + ReservationDTO.ReservationStatus.SYSTEMABANDONED.ToString() + "', '" + ReservationDTO.ReservationStatus.CANCELLED + "'"));
            searchParams.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.RESERVATION_FROM_DATE, lookupValuesList.GetServerDateTime().Date.ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)));
            List<ReservationDTO> returnValueList = GetReservationDTOList(searchParams);
            if (returnValueList != null)
            {
                log.LogVariableState("returnValueList.Count", returnValueList.Count);
                if (returnValueList.Count == 1)
                {
                    validCode = true;
                }
            }
            log.LogMethodExit(validCode);
            return validCode;
        }

    }
    /// <summary>
    /// Class containing defaul setup information for booking
    /// </summary>
    public class ReservationDefaultSetup
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        private const string DEFAULT_BOOKINGS_CHANNEL = "DEFAULT_BOOKINGS_CHANNEL";
        private const string FIXED_SCHEDULE_BOOKING_GRACE_PERIOD = "FIXED_SCHEDULE_BOOKING_GRACE_PERIOD";
        private const string BLOCK_BOOKING_FOR_X_MINUTES = "BLOCK_BOOKING_FOR_X_MINUTES";
        private const string CALENDAR_TIME_SLOT_GAP = "CALENDAR_TIME_SLOT_GAP";

        private string defaultChannel;
        private int fixedScheduleGracePeriodForBooking;
        private int calenderTimeSlotGap;
        private int blockBookingForXMinutes;
        /// <summary>
        /// GetDefault Channel
        /// </summary>
        public string GetDefaultChannel { get { return defaultChannel; } }
        /// <summary>
        /// GetGracePeriodForFixedSchedule
        /// </summary>
        public int GetGracePeriodForFixedSchedule { get { return fixedScheduleGracePeriodForBooking; } }
        /// <summary>
        /// GetCalendarTimeSlotGap
        /// </summary>
        public int GetCalendarTimeSlotGap { get { return calenderTimeSlotGap; } }
        /// <summary>
        /// GetMinutesForBlockReservation
        /// </summary>
        public int GetMinutesForBlockReservation { get { return blockBookingForXMinutes; } }


        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public ReservationDefaultSetup(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            LoadDefaultSetupDetails();
            log.LogMethodExit();
        }

        private void LoadDefaultSetupDetails()
        {
            log.LogMethodEntry();
            defaultChannel = "Phone";
            fixedScheduleGracePeriodForBooking = 60;
            calenderTimeSlotGap = 5;
            blockBookingForXMinutes = 15;

            LookupValuesList lookupValuesListBL = new LookupValuesList(executionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "BOOKINGS_SETUP"));
            searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<LookupValuesDTO> lookupValuesDTOList = lookupValuesListBL.GetAllLookupValues(searchParameters);
            if (lookupValuesDTOList != null && lookupValuesDTOList.Count > 0)
            {
                for (int i = 0; i < lookupValuesDTOList.Count; i++)
                {
                    switch (lookupValuesDTOList[i].LookupValue)
                    {
                        case DEFAULT_BOOKINGS_CHANNEL:
                            try
                            {
                                defaultChannel = lookupValuesDTOList[i].Description;
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                                defaultChannel = "Phone";
                            }
                            break;
                        case FIXED_SCHEDULE_BOOKING_GRACE_PERIOD:
                            try
                            {
                                fixedScheduleGracePeriodForBooking = Convert.ToInt32(lookupValuesDTOList[i].Description);
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                                fixedScheduleGracePeriodForBooking = 60;
                            }
                            break;
                        case BLOCK_BOOKING_FOR_X_MINUTES:
                            try
                            {
                                blockBookingForXMinutes = Convert.ToInt32(lookupValuesDTOList[i].Description);
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                                blockBookingForXMinutes = 15;
                            }
                            break;
                        case CALENDAR_TIME_SLOT_GAP:
                            try
                            {
                                calenderTimeSlotGap = Convert.ToInt32(lookupValuesDTOList[i].Description);
                                if (calenderTimeSlotGap != 5)
                                {
                                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Invalid CALENDAR_TIME_SLOT_GAP value " + calenderTimeSlotGap));
                                }
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                                calenderTimeSlotGap = 5;
                            }
                            break;
                    }
                }

            }
            log.LogMethodExit();
        }


        public static bool IsAutoChargeOptionEnabledForReservation(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            bool isAutoChargeOptionEnabled = false;
            bool autoAppplySvcCharge = ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "AUTO_APPLY_SERVICE_CHARGE", false);
            decimal svcChargePercentage = ParafaitDefaultContainerList.GetParafaitDefault<decimal>(executionContext, "SERVICE_CHARGE_PERCENTAGE", 0);
            int minGuestQtySvcCharge = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "MINIMUM_GUEST_QTY_FOR_RESERVATION_SERVICE_CHARGE", 0);

            bool autoAppplyGrauity = ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "AUTO_APPLY_GRATUITY", false);
            decimal svcGrauityPercentage = ParafaitDefaultContainerList.GetParafaitDefault<decimal>(executionContext, "GRATUITY_AMOUNT_PERCENTAGE", 0);
            int minGuestQtyGrauity = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "MINIMUM_GUEST_QTY_FOR_RESERVATION_GRATUITY", 0);

            if ((autoAppplySvcCharge && svcChargePercentage > 0 && minGuestQtySvcCharge > 0)
                 || (autoAppplyGrauity && svcGrauityPercentage > 0 && minGuestQtyGrauity > 0))
            {
                isAutoChargeOptionEnabled = true;
            }
            log.LogMethodExit(isAutoChargeOptionEnabled);
            return isAutoChargeOptionEnabled;
        }

    }
}