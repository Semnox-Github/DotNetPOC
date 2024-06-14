/* Project Name - Semnox.Parafait.Product.AttractionBooking 
* Description  - BL class for AttractionBooking
* 
**************
**Version Log
**************
*Version     Date           Modified By          Remarks          
********************************************************************************************* 
*2.70.0      14-Mar-2019    Guru S A             Modified for Booking phase 2 enhancement changes 
*2.70.0      06-Aug-2019    Nitin Pai            Added new method to get booked units for day
*2.70.2      06-Nov-2019    Nitin Pai            Club speed enhancements
*2.70.3      07-Jan-2020   Nitin Pai               Day Attraction and Reschedule Slot changes
*2.70.3      06-Feb-2020   Nitin Pai               Day Attraction and Reschedule Slot fixes
*2.100       24-Sep-2020   Nitin Pai             Attraction Reschdule: Moved schedule info from ATB to DAS
*2.120.0     04-Mar-2021    Sathyavathi          Enabling option nto decide ''Multiple-Booking at Facility level
********************************************************************************************/
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System.Globalization;
using Semnox.Parafait.Transaction.TransactionFunctions;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// BL class for AttractionBooking
    /// </summary>
    public class AttractionBooking
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private AttractionBookingDTO attractionBookingDTO;
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor of AttractionBooking class
        /// </summary>
        public AttractionBooking(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            attractionBookingDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the AttractionBooking id as the parameter
        /// Would fetch the AttractionBooking object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">optional sql transaction</param>
        public AttractionBooking(ExecutionContext executionContext, int id, bool loadChildRecords = false, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, loadChildRecords, sqlTransaction);
            AttractionBookingDatahandler attractionBookingDatahandler = new AttractionBookingDatahandler(sqlTransaction);
            attractionBookingDTO = attractionBookingDatahandler.GetAttractionBookingDTO(id);
            if (attractionBookingDTO != null)
            {
                DayAttractionScheduleBL dayAttractionScheduleBL = new DayAttractionScheduleBL(executionContext, attractionBookingDTO.DayAttractionScheduleId);
                attractionBookingDTO.DayAttractionScheduleDTO = dayAttractionScheduleBL.GetDayAttractionScheduleDTO;
            }

            if (loadChildRecords)
            {
                AttractionBookingSeatsListBL attractionBookingSeatsListBL = new AttractionBookingSeatsListBL(executionContext);
                List<KeyValuePair<AttractionBookingSeatsDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<AttractionBookingSeatsDTO.SearchByParameters, string>>();
                searchParam.Add(new KeyValuePair<AttractionBookingSeatsDTO.SearchByParameters, string>(AttractionBookingSeatsDTO.SearchByParameters.ATTRACTION_BOOKING_ID, attractionBookingDTO.BookingId.ToString()));
                searchParam.Add(new KeyValuePair<AttractionBookingSeatsDTO.SearchByParameters, string>(AttractionBookingSeatsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                List<AttractionBookingSeatsDTO> attractionBookingSeatsDTOList = attractionBookingSeatsListBL.GetAttractionBookingSeatsDTOList(searchParam, sqlTransaction);
                attractionBookingDTO.AttractionBookingSeatsDTOList = attractionBookingSeatsDTOList;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates AttractionBooking object using the AttractionBookingDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="attractionBookingDTO">AttractionBookingDTO object</param>
        public AttractionBooking(ExecutionContext executionContext, AttractionBookingDTO attractionBookingDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, attractionBookingDTO);
            this.attractionBookingDTO = attractionBookingDTO;
            log.LogMethodExit();
        }

        public bool ValidateDayAttractionBooking(SqlTransaction sqlTransaction = null)
        {
            String message, errorMessage = "";
            List<ValidationError> validationErrorList = new List<ValidationError>();
            //LookupValuesList serverTimeObject = new LookupValuesList(executionContext);
            //// Fetch the entries from the day attraction schedule table for given schedule id and date
            //// if there is an entry available, then validate if the attraction booking is permitted in the given slot
            ////          if not permitted, then throw the error
            ////          if permitted, check if the expiry time if limited, if yes then set it to null, else continue
            //// if there is no entry, then add a new entry in the table
            //DayAttractionScheduleListBL dayAttractionScheduleListBL = new DayAttractionScheduleListBL(executionContext);
            //List<KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>>();
            //searchParameters.Add(new KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>(DayAttractionScheduleDTO.SearchByParameters.ATTRACTION_SCHEDULE_ID, attractionBookingDTO.AttractionScheduleId.ToString()));
            //searchParameters.Add(new KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>(DayAttractionScheduleDTO.SearchByParameters.SCHEDULE_DATETIME, attractionBookingDTO.ScheduleFromDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
            //searchParameters.Add(new KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>(DayAttractionScheduleDTO.SearchByParameters.IS_ACTIVE, "Y"));
            //searchParameters.Add(new KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>(DayAttractionScheduleDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            //// add active records to filter list
            //// change the method, throw exception
            ////searchParameters.Add(new KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>(DayAttractionScheduleDTO.SearchByParameters.Acti, attractionBookingDTO.ScheduleFromDate.Date.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
            //List<DayAttractionScheduleDTO> dayAttractionScheduleDTOs = dayAttractionScheduleListBL.GetAllDayAttractionScheduleList(searchParameters, sqlTransaction);
            //DayAttractionScheduleDTO dayAttractionScheduleDTO = null;
            //// if this schedule slot is not booked for the day, then create a new slot
            //if (dayAttractionScheduleDTOs != null && dayAttractionScheduleDTOs.Any())
            //{
            //    // if there are slots (there should be only 1, get the first slot and validate
            //    dayAttractionScheduleDTO = dayAttractionScheduleDTOs[0];
            //    DayAttractionScheduleBL dayAttractionScheduleBL = new DayAttractionScheduleBL(executionContext, dayAttractionScheduleDTO);
            //    List<ValidationError> validationErrors = dayAttractionScheduleBL.Validate(sqlTransaction);
            //    if (validationErrors != null && validationErrors.Any())
            //    {
            //        validationErrorList.AddRange(validationErrors);
            //        throw new ValidationException("Invalid Schedule Slot selected", validationErrorList);
            //    }
            //}
            //else
            //{
            //    throw new RowNotInTableException("No day attraction schedule found");
            //}
            DayAttractionScheduleBL dayAttractionScheduleBL = new DayAttractionScheduleBL(executionContext, attractionBookingDTO.DayAttractionScheduleDTO);
            validationErrorList.AddRange(dayAttractionScheduleBL.Validate(sqlTransaction));
            if (validationErrorList != null && validationErrorList.Any())
            {
                throw new ValidationException("Invalid Schedule Slot selected", validationErrorList);
            }
            return true;
        }

        /// <summary>
        /// Validate
        /// </summary>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            List<ValidationError> validationErrorList = new List<ValidationError>();

            if(attractionBookingDTO.DayAttractionScheduleDTO == null)
            {
                String message = MessageContainerList.GetMessage(executionContext, "Day Attraction Schedule DTO not found");
                log.LogVariableState("Error message ", message);
                validationErrorList.Add(new ValidationError("AttractionBooking", "DayAttractionScheduleDTO", message));
            }
            else
            {
                DayAttractionScheduleBL dayAttractionScheduleBL = new DayAttractionScheduleBL(executionContext, attractionBookingDTO.DayAttractionScheduleDTO);
                validationErrorList.AddRange(dayAttractionScheduleBL.Validate(sqlTransaction));
                if (attractionBookingDTO.DayAttractionScheduleId == -1)
                {
                    attractionBookingDTO.DayAttractionScheduleId = dayAttractionScheduleBL.GetDayAttractionScheduleDTO.DayAttractionScheduleId;
                    attractionBookingDTO.DayAttractionScheduleDTO = dayAttractionScheduleBL.GetDayAttractionScheduleDTO;
                }
            }

            return validationErrorList;
        }

        public List<ValidationError> ValidateCapacity(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();

            // check if the slot has capcity, need to do this here to check for overlapping schedules also
            if (attractionBookingDTO.AvailableUnits > 0 && attractionBookingDTO.BookingId == -1)
            {
                int availableUnits = attractionBookingDTO.AvailableUnits == null ? 0 : Convert.ToInt32(attractionBookingDTO.AvailableUnits);

                // website may bot have the correct capacity, so need to get this from the DB
                AttractionBookingSchedulesBL attractionBookingSchedulesBL = new AttractionBookingSchedulesBL(executionContext);
                ScheduleDetailsDTO ScheduleDetailsDTO = attractionBookingSchedulesBL.GetScheduleDetailsById(attractionBookingDTO.ScheduleFromDate, attractionBookingDTO.FacilityMapId, attractionBookingDTO.AttractionScheduleId, executionContext.SiteId);
                if (ScheduleDetailsDTO != null && ScheduleDetailsDTO.TotalUnits != null)
                {
                    // why should we add booked units, it will come as part of the total booked units?
                    availableUnits = Convert.ToInt32(ScheduleDetailsDTO.TotalUnits);
                }
                else
                {
                    log.Error("Schedule details not found " + attractionBookingDTO.ScheduleFromDate + ":" + attractionBookingDTO.FacilityMapId + ":" + attractionBookingDTO.AttractionScheduleId + ":" + executionContext.SiteId);
                    // This is specifically in the web scenario, where available Units, is Total - Booked. For correct calculations, add the booked units also.
                    availableUnits = Convert.ToInt32(attractionBookingDTO.AvailableUnits != null ? attractionBookingDTO.AvailableUnits : 0) + Convert.ToInt32(attractionBookingDTO.BookedUnits != null ? attractionBookingDTO.BookedUnits : 0);
                }

                int totalBookedUnits = GetTotalBookedUnits(sqlTransaction);

                log.Debug("Available units " + attractionBookingDTO.AvailableUnits + " availableUnits " + availableUnits + " Booked units " + totalBookedUnits + " attractionBookingDTO.BookedUnits  " + attractionBookingDTO.BookedUnits);
                if (availableUnits < totalBookedUnits + attractionBookingDTO.BookedUnits)
                {
                    int calcUnits = (int)(attractionBookingDTO.AvailableUnits - totalBookedUnits);
                    String message = MessageContainerList.GetMessage(executionContext, 326, attractionBookingDTO.BookedUnits, Math.Max(calcUnits, 0).ToString() + "(" + attractionBookingDTO.AvailableUnits.ToString() + ")");
                    log.LogVariableState("Error message ", message);
                    validationErrorList.Add(new ValidationError("AttractionBooking", "AvailableUnits", message));
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        //private void ConfirmDayAttractionSchedule(SqlTransaction sqlTransaction = null)
        //{
        //    if (attractionBookingDTO.DayAttractionScheduleDTO != null && (attractionBookingDTO.DayAttractionScheduleDTO.ExpiryTime != DateTime.MinValue))
        //    {
        //        attractionBookingDTO.DayAttractionScheduleDTO.ExpiryTime = DateTime.MinValue;
        //        DayAttractionScheduleBL dayAttractionScheduleBL = new DayAttractionScheduleBL(executionContext, attractionBookingDTO.DayAttractionScheduleDTO);
        //        dayAttractionScheduleBL.Save(sqlTransaction);
        //    }
        //}

        /// <summary>
        /// Saves the AttractionBooking
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(int cardId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(cardId, sqlTransaction);

            LookupValuesList serverTimeObject = new LookupValuesList(executionContext);
            DateTime bookingExpiryTime = serverTimeObject.GetServerDateTime().AddMinutes(GetBookingCompletionTimeLimit());
            String message, errorMessage = "";
            List<ValidationError> validationErrorList = null;

            // Do this check only in case of new attraction booking saves\ignore in case of transaction reversals
            if (attractionBookingDTO.ExpiryDate == DateTime.MinValue || attractionBookingDTO.ExpiryDate > serverTimeObject.GetServerDateTime())
            {
                message = "ToDO : Validate impact in all POS usecases";
                //try
                //{
                //    ValidateDayAttractionBooking(sqlTransaction);
                //}
                //catch (RowNotInTableException ex)
                //{
                //    DayAttractionScheduleDTO dayAttractionScheduleDTO = new DayAttractionScheduleDTO(
                //                                    -1,
                //                                    attractionBookingDTO.AttractionScheduleId,
                //                                    attractionBookingDTO.FacilityMapId,
                //                                    attractionBookingDTO.ScheduleFromDate.Date,
                //                                    attractionBookingDTO.ScheduleFromDate,
                //                                    DayAttractionScheduleDTO.ScheduleStatusEnumToString(DayAttractionScheduleDTO.ScheduleStatusEnum.OPEN), // this will be populated by the interface
                //                                    "", // this will be populated by the interface
                //                                    true,
                //                                    AttractionBookingDTO.SourceEnumToString(attractionBookingDTO.Source),
                //                                    true,//ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "ALLOW_MULTIPLE_BOOKINGS_WITHIN_SCHEDULE") ? false:true, // by default set this to blocked
                //                                    attractionBookingDTO.TrxId > 0 ? DateTime.MinValue : bookingExpiryTime,
                //                                    ""); // this will be populated by the interface

                //    DayAttractionScheduleBL dayAttractionScheduleBL = new DayAttractionScheduleBL(executionContext, dayAttractionScheduleDTO);
                //    dayAttractionScheduleBL.Save(sqlTransaction);
                //}
                //catch (Exception ex)
                //{
                //    log.Debug("validate day attraction schedule failed." + ex.Message);
                //    throw;
                //}
            }
            try
            {
                validationErrorList = Validate(sqlTransaction);
                if (validationErrorList.Count > 0)
                {
                    throw new ValidationException("Validation Failed", validationErrorList);
                }
            }
            catch (RowNotInTableException ex)
            {
                attractionBookingDTO.DayAttractionScheduleDTO.Blocked = true;
                attractionBookingDTO.DayAttractionScheduleDTO.ExpiryTime = attractionBookingDTO.TrxId > 0 ? DateTime.MinValue : bookingExpiryTime;
                attractionBookingDTO.DayAttractionScheduleDTO.Source = AttractionBookingDTO.SourceEnumToString(attractionBookingDTO.Source);
                attractionBookingDTO.DayAttractionScheduleDTO.ScheduleStatus = DayAttractionScheduleDTO.ScheduleStatusEnumToString(DayAttractionScheduleDTO.ScheduleStatusEnum.OPEN);

                DayAttractionScheduleBL dayAttractionScheduleBL = new DayAttractionScheduleBL(executionContext, attractionBookingDTO.DayAttractionScheduleDTO);
                dayAttractionScheduleBL.Save(sqlTransaction);
                attractionBookingDTO.DayAttractionScheduleId = dayAttractionScheduleBL.GetDayAttractionScheduleDTO.DayAttractionScheduleId;
                attractionBookingDTO.DayAttractionScheduleDTO = dayAttractionScheduleBL.GetDayAttractionScheduleDTO;
            }
            catch (EntityExpiredException ex)
            {
                // this exception comes for DASSchedules which are active but expired. Likely leftover of an attempted booking in web.
                if(attractionBookingDTO.BookingId == -1)
                {
                    attractionBookingDTO.DayAttractionScheduleDTO.Blocked = true;
                    attractionBookingDTO.DayAttractionScheduleDTO.ExpiryTime = attractionBookingDTO.TrxId > 0 ? DateTime.MinValue : bookingExpiryTime;
                    attractionBookingDTO.DayAttractionScheduleDTO.Source = AttractionBookingDTO.SourceEnumToString(attractionBookingDTO.Source);
                    attractionBookingDTO.DayAttractionScheduleDTO.ScheduleStatus = DayAttractionScheduleDTO.ScheduleStatusEnumToString(DayAttractionScheduleDTO.ScheduleStatusEnum.OPEN);

                    DayAttractionScheduleBL dayAttractionScheduleBL = new DayAttractionScheduleBL(executionContext, attractionBookingDTO.DayAttractionScheduleDTO);
                    dayAttractionScheduleBL.Save(sqlTransaction);
                    attractionBookingDTO.DayAttractionScheduleId = dayAttractionScheduleBL.GetDayAttractionScheduleDTO.DayAttractionScheduleId;
                    attractionBookingDTO.DayAttractionScheduleDTO = dayAttractionScheduleBL.GetDayAttractionScheduleDTO;
                }
                // if the day attraction schedule is expired but the attraction is not yet expired, then throw an error
                else if (attractionBookingDTO.ExpiryDate == DateTime.MinValue || attractionBookingDTO.ExpiryDate > serverTimeObject.GetServerDateTime())
                    throw;
            }
            catch (Exception ex)
            {
                log.Debug("validate day attraction schedule failed." + ex.Message);
                throw;
            }

            // Do capacity validation after DA validation so that it is not skipped in scenarios where DA is not created
            validationErrorList = ValidateCapacity(sqlTransaction);
            if (validationErrorList.Count > 0)
            {
                throw new ValidationException("Validation Failed", validationErrorList);
            }

            if (attractionBookingDTO.DayAttractionScheduleDTO != null)
            {
                if (attractionBookingDTO.TrxId > -1 && attractionBookingDTO.DayAttractionScheduleDTO.ExpiryTime != DateTime.MinValue)
                {
                    attractionBookingDTO.DayAttractionScheduleDTO.ExpiryTime = DateTime.MinValue;
                }
                if (attractionBookingDTO.IsChanged)
                {
                    DayAttractionScheduleBL dayAttractionScheduleBL = new DayAttractionScheduleBL(executionContext, attractionBookingDTO.DayAttractionScheduleDTO);
                    dayAttractionScheduleBL.Save(sqlTransaction);
                }
            }

            AttractionBookingDatahandler attractionBookingDatahandler = new AttractionBookingDatahandler(sqlTransaction);
            if (attractionBookingDTO.BookingId < 0)
            {
                
                if (attractionBookingDTO.TrxId == -1)
                {
                    attractionBookingDTO.ExpiryDate = bookingExpiryTime;
                }
                int id = attractionBookingDatahandler.InsertAttractionBooking(attractionBookingDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                attractionBookingDTO.BookingId = id;
                attractionBookingDTO.AcceptChanges();
            }
            else
            {
                if (attractionBookingDTO.IsChanged)
                {
                    attractionBookingDatahandler.UpdateAttractionBooking(attractionBookingDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    attractionBookingDTO.AcceptChanges();
                }
            }

            if (attractionBookingDTO.AttractionBookingSeatsDTOList != null && attractionBookingDTO.AttractionBookingSeatsDTOList.Count > 0)
            {
                foreach (AttractionBookingSeatsDTO attractionBookingSeatsDTO in attractionBookingDTO.AttractionBookingSeatsDTOList)
                {
                    if (attractionBookingSeatsDTO.CardId != cardId)
                    {
                        attractionBookingSeatsDTO.CardId = cardId;
                    }
                    if (attractionBookingSeatsDTO.BookingId != attractionBookingDTO.BookingId)
                    {
                        attractionBookingSeatsDTO.BookingId = attractionBookingDTO.BookingId;
                    }
                    if (attractionBookingSeatsDTO.IsChanged)
                    {
                        AttractionBookingSeatsBL attractionBookingSeatsBL = new AttractionBookingSeatsBL(executionContext, attractionBookingSeatsDTO);
                        attractionBookingSeatsBL.Save(sqlTransaction);
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public AttractionBookingDTO AttractionBookingDTO
        {
            get
            {
                return attractionBookingDTO;
            }
        }

        //public int AttractionPlayId = -1;
        //public int BookingId = -1;
        //public int TrxId = -1;
        //public int LineId = -1;
        //public int site_id = -1;
        //public int AttractionScheduleId = -1;
        //public string AttractionScheduleName; 
        //public string AttractionPlayName;
        //public DateTime ScheduleTime;
        //public DateTime ExpiryDate;
        //public int PromotionId;  
        //public double Price;
        //public List<int> SelectedSeats = new List<int>();
        //public List<string> SelectedSeatNames = new List<string>(); 
        //public int AvailableUnits;
        //public int BookedUnits;
        //public decimal ScheduleFromTime = -1;
        //public decimal ScheduleToTime = -1;
        //public int Identifier = -1;
        public List<string> cardNumberList = new List<string>();
        public List<Card> cardList = new List<Card>();

        public int GetTotalBookedUnits(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            int alreadyBookedUnits = 0;
            if (attractionBookingDTO != null && attractionBookingDTO.AttractionScheduleId > -1 && attractionBookingDTO.ScheduleFromDate > DateTime.MinValue)
            {
                AttractionBookingDatahandler attractionBookingDatahandler = new AttractionBookingDatahandler(sqlTransaction);
                alreadyBookedUnits = attractionBookingDatahandler.GetTotalBookedUnits(attractionBookingDTO.AttractionScheduleId, attractionBookingDTO.ScheduleFromDate, attractionBookingDTO.ScheduleToDate, executionContext.GetSiteId(), attractionBookingDTO.FacilityMapId);
            }
            log.LogMethodExit(alreadyBookedUnits);
            return alreadyBookedUnits;
        }

        //public bool Save(int cardId, SqlTransaction SQLTrx, ref string message)
        //      {
        //          log.LogMethodEntry(cardId, SQLTrx, message);

        //if (AvailableUnits > 0)
        //{
        //    int totalBookedUnits = GetTotalBookedUnits();
        //    if (AvailableUnits < totalBookedUnits + BookedUnits)
        //    {
        //        message = Utilities.MessageUtils.getMessage(326, BookedUnits, (AvailableUnits - totalBookedUnits).ToString() + "(" + AvailableUnits.ToString() + ")");
        //        log.LogVariableState("message ", message);
        //        log.LogMethodExit();
        //        return false;
        //    }
        //}

        ////Added an additoinal field Siteid on 01-Oct-2015//
        //SqlCommand cmd = Utilities.getCommand(SQLTrx);
        //cmd.CommandText = "insert into AttractionBookings ( " +
        //                    "AttractionScheduleId, " +
        //                    "AttractionPlayId, " +
        //                    "ScheduleTime, " +
        //                    "TrxId, " +
        //                    "LineId, " +
        //                    "BookedUnits, " +
        //                    "ExpiryDate, " +
        //                    "Site_id) " +
        //                    "values ( " +
        //                    "@AttractionScheduleId, " +
        //                    "@AttractionPlayId, " +
        //                    "@ScheduleTime, " +
        //                    "@TrxId, " +
        //                    "@LineId, " +
        //                    "@BookedUnits, " +
        //                    "@ExpiryDate, " +
        //                    "@SiteId); select @@identity";
        //cmd.Parameters.AddWithValue("@AttractionScheduleId", AttractionScheduleId);
        //cmd.Parameters.AddWithValue("@AttractionPlayId", AttractionPlayId);
        //cmd.Parameters.AddWithValue("@ScheduleTime", ScheduleTime);
        //cmd.Parameters.AddWithValue("@TrxId", TrxId == -1 ? DBNull.Value : (object)TrxId);
        //cmd.Parameters.AddWithValue("@LineId", LineId == -1 ? DBNull.Value : (object)LineId);
        //cmd.Parameters.AddWithValue("@BookedUnits", BookedUnits);

        //log.LogVariableState("@AttractionScheduleId", AttractionScheduleId);
        //log.LogVariableState("@AttractionPlayId", AttractionPlayId);
        //log.LogVariableState("@ScheduleTime", ScheduleTime);
        //log.LogVariableState("@TrxId", TrxId);
        //log.LogVariableState("@LineId", LineId);
        //log.LogVariableState("@BookedUnits", BookedUnits);

        //// reserve for 5 minutes while booking, to allow for card tap 
        //if (TrxId == -1)
        //{
        //    cmd.Parameters.AddWithValue("@ExpiryDate", DateTime.Now.AddMinutes(GetBookingCompletionTimeLimit()));
        //}
        //else
        //{
        //    if (ExpiryDate == DateTime.MinValue)
        //    {
        //        cmd.Parameters.AddWithValue("@ExpiryDate", DBNull.Value);
        //        log.LogVariableState("@ExpiryDate", DBNull.Value);
        //    }
        //    else
        //    {
        //        cmd.Parameters.AddWithValue("@ExpiryDate", ExpiryDate);
        //        log.LogVariableState("@ExpiryDate", ExpiryDate);
        //    }
        //}
        ////Begin Modification - Added site id for HQ synch - 01-Oct-2015
        //if (Utilities.ParafaitEnv.IsCorporate == false || Utilities.ParafaitEnv.SiteId <= 0)
        //    site_id = DBNull.Value;
        //else
        //    site_id = Utilities.ParafaitEnv.SiteId;
        //cmd.Parameters.AddWithValue("@SiteId", site_id);
        ////End Modification - Added site id for HQ synch - 01-Oct-2015
        //BookingId = Convert.ToInt32(cmd.ExecuteScalar());

        //if (SelectedSeats != null)
        //{
        //    DataTable dt = Utilities.executeDataTable(@"select abs.SeatId, fs.SeatName
        //                                                from AttractionBookings atb, AttractionBookingSeats abs, FacilitySeats fs
        //                                            where atb.BookingId = abs.BookingId
        //                                            and atb.AttractionScheduleId = @AttractionScheduleId
        //                                            and atb.ScheduleTime = @ScheduleTime
        //                                            and fs.SeatId = abs.SeatId", SQLTrx,
        //                                                new SqlParameter("@AttractionScheduleId", AttractionScheduleId),
        //                                                new SqlParameter("@ScheduleTime", ScheduleTime)
        //                                                );
        //    log.LogVariableState("@AttractionScheduleId", AttractionScheduleId);
        //    log.LogVariableState("@ScheduleTime", ScheduleTime);

        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        if (SelectedSeats.Contains(Convert.ToInt32(dr[0])))
        //        {
        //            message = Utilities.MessageUtils.getMessage(342, dr[1]);

        //            log.LogVariableState("message = ",message);
        //            log.LogMethodExit(false);
        //            return false;
        //        }
        //    }

        //    foreach (object seat in SelectedSeats)
        //    {
        //        object cardparam;
        //        if (cardId == -1)
        //            cardparam = DBNull.Value;
        //        else
        //            cardparam = cardId;
        //        Utilities.executeNonQuery("insert into AttractionBookingSeats (BookingId, SeatId, CardId, Site_id) values (@BookingId, @SeatId, @CardId, @SiteId)",
        //                                    SQLTrx, 
        //                                    new SqlParameter("@BookingId", BookingId),
        //                                    new SqlParameter("@SeatId", seat),
        //                                    new SqlParameter("@CardId", cardparam),
        //                                    new SqlParameter("@SiteId", site_id)); //Begin Mod - Added site id for HQ 01-Oct-2015

        //        log.LogVariableState("@BookingId", BookingId);
        //        log.LogVariableState("@SeatId", seat);
        //        log.LogVariableState("@CardId", cardparam);
        //        log.LogVariableState("@SiteId", site_id);
        //    }
        //}

        //    log.LogMethodExit(true); 
        //    return true;
        //}

        public void Expire(SqlTransaction sqlTransaction = null, bool ignoreTrxCheck = false)
        {
            log.LogMethodEntry();
            if (attractionBookingDTO != null && attractionBookingDTO.BookingId > -1)
            {
                AttractionBookingDatahandler attractionBookingDatahandler = new AttractionBookingDatahandler(sqlTransaction);
                if (attractionBookingDTO.TrxId > -1)
                {
                    attractionBookingDatahandler.ExpireBookingEntry(attractionBookingDTO.BookingId, executionContext.GetUserId());
                }
                else
                {
                    attractionBookingDatahandler.HardDeleteBookingEntry(attractionBookingDTO.BookingId);
                }

                if (attractionBookingDTO.BookingId > -1 && (attractionBookingDTO.TrxId > -1 || ignoreTrxCheck))
                {
                    ReverseDayAttraction(attractionBookingDTO.TrxId, attractionBookingDTO.LineId, sqlTransaction);
                }
            }
            log.LogMethodExit();
        }


        public void ReduceBookedUnits(int reduceUnits, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(reduceUnits);
            if (attractionBookingDTO != null)
            {
                if (attractionBookingDTO.BookedUnits == reduceUnits)
                {
                    Expire(sqlTransaction);
                    attractionBookingDTO.BookedUnits = 0;

                    //if (attractionBookingDTO.TrxId > -1)
                    //    CheckAndReverseDayAttraction(attractionBookingDTO.TrxId, attractionBookingDTO.LineId, sqlTransaction);
                }
                else if (attractionBookingDTO.BookedUnits > reduceUnits)
                {
                    int bookedUnits = attractionBookingDTO.BookedUnits;
                    attractionBookingDTO.BookedUnits = bookedUnits - reduceUnits;
                    if (attractionBookingDTO.AttractionBookingSeatsDTOList != null
                                    && attractionBookingDTO.AttractionBookingSeatsDTOList.Count > 0)
                    {
                        List<AttractionBookingSeatsDTO> removeFromList = new List<AttractionBookingSeatsDTO>();
                        for (int i = 0; i < reduceUnits; i++)
                        {
                            removeFromList.Add(attractionBookingDTO.AttractionBookingSeatsDTOList[i]);
                            AttractionBookingSeatsBL atbSeatsBL = new AttractionBookingSeatsBL(executionContext, attractionBookingDTO.AttractionBookingSeatsDTOList[i]);
                            atbSeatsBL.Expire(sqlTransaction);
                        }
                        foreach (AttractionBookingSeatsDTO removedATBSeats in removeFromList)
                        {
                            attractionBookingDTO.AttractionBookingSeatsDTOList.Remove(removedATBSeats);
                        }
                    }
                    int cardId = -1;
                    if (attractionBookingDTO.AttractionBookingSeatsDTOList != null && attractionBookingDTO.AttractionBookingSeatsDTOList.Count > 0)
                    {
                        cardId = attractionBookingDTO.AttractionBookingSeatsDTOList[0].CardId;
                    }

                    // save only if it is an existing entity
                    if(attractionBookingDTO.BookingId > -1)
                        Save(cardId, sqlTransaction);
                }

            }
            log.LogMethodExit();
        }

        public int GetBookingCompletionTimeLimit()
        {
            log.LogMethodEntry();
            int bookingCompletionTimeLimit = 5;
            string defaultValue = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "BOOKING_COMPLETION_TIME_LIMIT");
            if (int.TryParse(defaultValue, out bookingCompletionTimeLimit) == false)
            {
                bookingCompletionTimeLimit = 5;
            }
            bookingCompletionTimeLimit = (bookingCompletionTimeLimit <= 0 ? 5 : bookingCompletionTimeLimit);
            log.LogMethodExit(bookingCompletionTimeLimit);
            return bookingCompletionTimeLimit;

            //int bookingCompletionTimeLimit = Convert.ToInt32(Utilities.executeScalar(@"select isnull((select default_value from parafait_defaults where default_value_name = @defaultValueName and (site_id = @siteId or  @siteId = -1 )), 0)",
            //														new SqlParameter("@defaultValueName", "BOOKING_COMPLETION_TIME_LIMIT"),
            //														new SqlParameter("@siteId", site_id)));

            //log.LogVariableState("@defaultValueName", "BOOKING_COMPLETION_TIME_LIMIT");
            //log.LogVariableState("@siteId", site_id);
            //bookingCompletionTimeLimit = (bookingCompletionTimeLimit <= 0 ? 5 : bookingCompletionTimeLimit);
            //log.LogVariableState("BOOKING_COMPLETION_TIME_LIMIT", bookingCompletionTimeLimit);

            //return bookingCompletionTimeLimit;
        }

        private void CloneDTO(AttractionBookingDTO attractionBookingDTOOriginal)
        {
            log.LogMethodEntry(attractionBookingDTOOriginal);
            if (this.attractionBookingDTO == null)
            {
                this.attractionBookingDTO = new AttractionBookingDTO();
            }
            this.attractionBookingDTO.DayAttractionScheduleDTO = attractionBookingDTOOriginal.DayAttractionScheduleDTO;
            //this.attractionBookingDTO.TrxId = attractionBookingDTOOriginal.TrxId;
            //this.attractionBookingDTO.LineId = attractionBookingDTOOriginal.LineId;
            this.attractionBookingDTO.AttractionPlayId = attractionBookingDTOOriginal.AttractionPlayId;
            this.attractionBookingDTO.AttractionPlayName = attractionBookingDTOOriginal.AttractionPlayName;
            this.attractionBookingDTO.AttractionScheduleId = attractionBookingDTOOriginal.AttractionScheduleId;
            this.attractionBookingDTO.AttractionScheduleName = attractionBookingDTOOriginal.AttractionScheduleName;
            this.attractionBookingDTO.AvailableUnits = attractionBookingDTOOriginal.AvailableUnits;
            this.attractionBookingDTO.BookedUnits = attractionBookingDTOOriginal.BookedUnits;
            //this.attractionBookingDTO.ExpiryDate = attractionBookingDTOOriginal.ExpiryDate;
            this.attractionBookingDTO.FacilityMapId = attractionBookingDTOOriginal.FacilityMapId;
            this.attractionBookingDTO.Identifier = attractionBookingDTOOriginal.Identifier;
            this.attractionBookingDTO.Price = attractionBookingDTOOriginal.Price;
            this.attractionBookingDTO.PromotionId = attractionBookingDTOOriginal.PromotionId;
            this.attractionBookingDTO.ScheduleFromDate = attractionBookingDTOOriginal.ScheduleFromDate;
            this.attractionBookingDTO.ScheduleToDate = attractionBookingDTOOriginal.ScheduleToDate;
            this.attractionBookingDTO.ScheduleFromTime = attractionBookingDTOOriginal.ScheduleFromTime;
            this.attractionBookingDTO.ScheduleToTime = attractionBookingDTOOriginal.ScheduleToTime;
            this.attractionBookingDTO.ExternalSystemReference = attractionBookingDTOOriginal.ExternalSystemReference;
            this.attractionBookingDTO.Source = attractionBookingDTOOriginal.Source;
            if (attractionBookingDTOOriginal.AttractionBookingSeatsDTOList != null && attractionBookingDTOOriginal.AttractionBookingSeatsDTOList.Count > 0)
            {
                if (this.attractionBookingDTO.AttractionBookingSeatsDTOList == null)
                {
                    this.attractionBookingDTO.AttractionBookingSeatsDTOList = new List<AttractionBookingSeatsDTO>();
                }
                foreach (AttractionBookingSeatsDTO attractionBookingSeatsDTOOriginal in attractionBookingDTOOriginal.AttractionBookingSeatsDTOList)
                {
                    AttractionBookingSeatsDTO attractionBookingSeatsDTO = new AttractionBookingSeatsDTO();
                    AttractionBookingSeatsBL attractionBookingSeatsBL = new AttractionBookingSeatsBL(executionContext, attractionBookingSeatsDTO);
                    attractionBookingSeatsBL.CloneObject(attractionBookingSeatsDTOOriginal);
                    this.attractionBookingDTO.AttractionBookingSeatsDTOList.Add(attractionBookingSeatsBL.AttractionBookingSeatsDTO);
                }
            }
            log.LogMethodExit();
        }

        public void CloneObject(AttractionBooking attractionBookingOriginal, int bookedUnitsToBeCopied)
        {
            log.LogMethodEntry(attractionBookingOriginal, bookedUnitsToBeCopied);
            this.cardList = attractionBookingOriginal.cardList;
            this.cardNumberList = attractionBookingOriginal.cardNumberList;
            CloneDTO(attractionBookingOriginal.AttractionBookingDTO);
            this.attractionBookingDTO.BookedUnits = bookedUnitsToBeCopied;
            this.attractionBookingDTO.BookingId = -1;
            if (this.attractionBookingDTO.AttractionBookingSeatsDTOList != null && this.attractionBookingDTO.AttractionBookingSeatsDTOList.Count > 0)
            {
                if (this.attractionBookingDTO.AttractionBookingSeatsDTOList.Count >= bookedUnitsToBeCopied)
                {
                    List<AttractionBookingSeatsDTO> newSeatList = new List<AttractionBookingSeatsDTO>();
                    for (int i = 0; i < bookedUnitsToBeCopied; i++)
                    {
                        AttractionBookingSeatsDTO attractionBookingSeatsDTO = this.attractionBookingDTO.AttractionBookingSeatsDTOList[i];
                        newSeatList.Add(attractionBookingSeatsDTO);
                    }
                    this.attractionBookingDTO.AttractionBookingSeatsDTOList = newSeatList;
                }
            }
            log.LogMethodExit();
        }

        public void ReverseDayAttraction(int origTrxId, int TrxLineId, SqlTransaction sqlTrx, bool overrideDayAttractionCheck = true)
        {
            LookupValuesList serverTimeObject = new LookupValuesList(executionContext);
            DateTime bookingExpiryTime = serverTimeObject.GetServerDateTime();
            DateTime previousExpiryTime = attractionBookingDTO.DayAttractionScheduleDTO.ExpiryTime;
            //// First search for day attraction schedules in open status, if no day attraction schedule if found, it means that the slot has been utilized and cannot be reversed
            //DayAttractionScheduleListBL dayAttractionScheduleListBL = new DayAttractionScheduleListBL(executionContext);
            //List<KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>>();
            //searchParameters.Add(new KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>(DayAttractionScheduleDTO.SearchByParameters.ATTRACTION_SCHEDULE_ID, attractionBookingDTO.AttractionScheduleId.ToString()));
            //searchParameters.Add(new KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>(DayAttractionScheduleDTO.SearchByParameters.SCHEDULE_DATETIME, attractionBookingDTO.ScheduleFromDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
            //searchParameters.Add(new KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>(DayAttractionScheduleDTO.SearchByParameters.SCHEDULE_STATUS, DayAttractionScheduleDTO.ScheduleStatusEnumToString(DayAttractionScheduleDTO.ScheduleStatusEnum.OPEN)));
            ////searchParameters.Add(new KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>(DayAttractionScheduleDTO.SearchByParameters.IS_ACTIVE, "Y"));
            //List<DayAttractionScheduleDTO> dayAttractionScheduleDTOs = dayAttractionScheduleListBL.GetAllDayAttractionScheduleList(searchParameters, sqlTrx);

            //if (!overrideDayAttractionCheck && (dayAttractionScheduleDTOs == null || !dayAttractionScheduleDTOs.Any()))
            //{
            //    // Day attraction entry not found, throw an exception that transaction cannot be reversed
            //    String message = MessageContainerList.GetMessage(executionContext, 326);
            //    throw new Exception("Schedule slot has been utilized, this transaction cannot be reversed");
            //}

            //// if there are not active booking left continue
            //if (!overrideDayAttractionCheck)
            //{
            //    dayAttractionScheduleDTOs = dayAttractionScheduleDTOs.OrderByDescending(x => x.LastUpdatedDate).ToList();
            //    if (!dayAttractionScheduleDTOs[0].IsActive)
            //        return; 
            //}

            //AttractionBookingList otherAttractionBookingList = new AttractionBookingList(this.executionContext);
            //List<KeyValuePair<AttractionBookingDTO.SearchByParameters, string>> otherAtbSearchParams = new List<KeyValuePair<AttractionBookingDTO.SearchByParameters, string>>();
            //otherAtbSearchParams.Add(new KeyValuePair<AttractionBookingDTO.SearchByParameters, string>(AttractionBookingDTO.SearchByParameters.SCHEDULE_ID, attractionBookingDTO.AttractionScheduleId.ToString()));
            //otherAtbSearchParams.Add(new KeyValuePair<AttractionBookingDTO.SearchByParameters, string>(AttractionBookingDTO.SearchByParameters.ATTRACTION_FROM_DATE, attractionBookingDTO.ScheduleFromDate.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)));
            //List<AttractionBookingDTO> otherAttractionBookingDTOList = otherAttractionBookingList.GetAttractionBookingDTOList(otherAtbSearchParams, false, sqlTrx);

            //if (otherAttractionBookingDTOList != null)
            //{
            //    otherAttractionBookingDTOList = otherAttractionBookingDTOList.Where(x => x.BookingId != attractionBookingDTO.BookingId && (x.ExpiryDate == DateTime.MinValue || x.ExpiryDate > bookingExpiryTime) && x.BookedUnits > 0).ToList();
            //    //if (TrxLineId > -1)
            //    //    otherAttractionBookingDTOList = otherAttractionBookingDTOList.Where(x => x.TrxId != origTrxId && x.LineId != TrxLineId && (x.ExpiryDate == DateTime.MinValue || x.ExpiryDate > bookingExpiryTime)).ToList();
            //    //else
            //    //    otherAttractionBookingDTOList = otherAttractionBookingDTOList.Where(x => x.TrxId != origTrxId && (x.ExpiryDate == DateTime.MinValue || x.ExpiryDate > bookingExpiryTime)).ToList();
            //    if (otherAttractionBookingDTOList != null && !otherAttractionBookingDTOList.Any())
            //    {
            //        if (dayAttractionScheduleDTOs != null && dayAttractionScheduleDTOs.Any())
            //        {
            //            DayAttractionScheduleDTO dayAttractionScheduleDTO = dayAttractionScheduleDTOs[0];
            //            DayAttractionScheduleBL dayAttractionScheduleBL = new DayAttractionScheduleBL(executionContext, dayAttractionScheduleDTO);
            //            dayAttractionScheduleDTO.Blocked = false;
            //            dayAttractionScheduleDTO.IsActive = false;
            //            dayAttractionScheduleDTO.ExpiryTime = bookingExpiryTime;
            //            dayAttractionScheduleBL.Save(sqlTrx);
            //        }
            //    }
            //}

            try
            {
                DayAttractionScheduleDTO dayAttractionScheduleDTO = attractionBookingDTO.DayAttractionScheduleDTO;
                DayAttractionScheduleBL dayAttractionScheduleBL = new DayAttractionScheduleBL(executionContext, dayAttractionScheduleDTO);
                dayAttractionScheduleDTO.Blocked = false;
                dayAttractionScheduleDTO.IsActive = false;
                dayAttractionScheduleDTO.ExpiryTime = bookingExpiryTime;
                dayAttractionScheduleBL.Save(sqlTrx);
            }
            catch(ValidationException ex)
            {
                // reset the values
                attractionBookingDTO.DayAttractionScheduleDTO.Blocked = true;
                attractionBookingDTO.DayAttractionScheduleDTO.IsActive = true;
                attractionBookingDTO.DayAttractionScheduleDTO.ExpiryTime = previousExpiryTime;
                // if there are other bookings for this DAS, it will throw a validation exception, in that scenario ignore and continue
            }
        }


    }


    /// <summary>
    /// Manages the list of AttractionBookings
    /// </summary>
    public class AttractionBookingList
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public AttractionBookingList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// GetAttractionBookingDTOList
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns>List<AttractionBookingDTO></returns>
        public List<AttractionBookingDTO> GetAttractionBookingDTOList(List<KeyValuePair<AttractionBookingDTO.SearchByParameters, string>> searchParameters, bool loadSeatDetails, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadSeatDetails, sqlTransaction);
            AttractionBookingDatahandler attractionBookingDatahandler = new AttractionBookingDatahandler(sqlTransaction);
            List<AttractionBookingDTO> returnValue = attractionBookingDatahandler.GetAttractionBookingDTOList(searchParameters);
            if (returnValue != null && returnValue.Count > 0)
            {
                StringBuilder idListStringBuilder = new StringBuilder("");
                string dasIdList;
                for (int i = 0; i < returnValue.Count; i++)
                {
                    if (i != 0)
                    {
                        idListStringBuilder.Append(",");
                    }
                    idListStringBuilder.Append(returnValue[i].DayAttractionScheduleId.ToString());
                }

                dasIdList = idListStringBuilder.ToString();
                DayAttractionScheduleListBL dasListBL = new DayAttractionScheduleListBL(executionContext);
                List<KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>> dasSearchParams = new List<KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>>();
                dasSearchParams.Add(new KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>(DayAttractionScheduleDTO.SearchByParameters.DAY_ATTRACTION_SCHEDULE_ID_LIST, dasIdList));
                List<DayAttractionScheduleDTO> dasDTOList = dasListBL.GetAllDayAttractionScheduleList(dasSearchParams, sqlTransaction, false);
                if (dasDTOList != null)
                {
                    foreach (var atbDTO in returnValue)
                    {
                        atbDTO.DayAttractionScheduleDTO = dasDTOList.Where(x => x.DayAttractionScheduleId == atbDTO.DayAttractionScheduleId).FirstOrDefault();
                    }
                }
            }

            if (loadSeatDetails && returnValue != null && returnValue.Count > 0)
            {
                returnValue = BuildSeatDetails(returnValue);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// GetAttractionBookingDTOList
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns>List<AttractionBookingDTO></returns>
        public List<AttractionBookingViewDTO> GetAttractionBookingViewDTOList(List<KeyValuePair<AttractionBookingDTO.SearchByParameters, string>> searchParameters, bool loadSeatDetails, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadSeatDetails, sqlTransaction);
            
            AttractionBookingDatahandler attractionBookingDatahandler = new AttractionBookingDatahandler(sqlTransaction);
            List<AttractionBookingViewDTO> attractionBookingViewDTOlist = attractionBookingDatahandler.GetAttractionBookingViewDTOList(searchParameters, executionContext);
            //List<AttractionBookingDTO> returnValue = GetAttractionBookingDTOList(searchParameters, loadSeatDetails);
            if (attractionBookingViewDTOlist != null && attractionBookingViewDTOlist.Count > 0)
            {
                StringBuilder idListStringBuilder = new StringBuilder("");
                string dasIdList;
                for (int i = 0; i < attractionBookingViewDTOlist.Count; i++)
                {
                    if (i != 0)
                    {
                        idListStringBuilder.Append(",");
                    }
                    idListStringBuilder.Append(attractionBookingViewDTOlist[i].AttractionBookingDTO.DayAttractionScheduleId.ToString());
                }

                dasIdList = idListStringBuilder.ToString();
                DayAttractionScheduleListBL dasListBL = new DayAttractionScheduleListBL(executionContext);
                List<KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>> dasSearchParams = new List<KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>>();
                dasSearchParams.Add(new KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>(DayAttractionScheduleDTO.SearchByParameters.DAY_ATTRACTION_SCHEDULE_ID_LIST, dasIdList));
                List<DayAttractionScheduleDTO> dasDTOList = dasListBL.GetAllDayAttractionScheduleList(dasSearchParams, sqlTransaction, false);
                if (dasDTOList != null)
                {
                    foreach (var atbViewDTO in attractionBookingViewDTOlist)
                    {
                        atbViewDTO.AttractionBookingDTO.DayAttractionScheduleDTO = dasDTOList.Where(x => x.DayAttractionScheduleId == atbViewDTO.AttractionBookingDTO.DayAttractionScheduleId).FirstOrDefault();
                    }
                }

                //    trxIdList = idListStringBuilder.ToString();

                //    TransactionListBL trxListBL = new TransactionListBL(executionContext);
                //    List<KeyValuePair<TransactionDTO.SearchByParameters, string>> trxSearchParams = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();
                //    trxSearchParams.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_ID_LIST, trxIdList));
                //    List<TransactionDTO> trxDTOList = trxListBL.GetTransactionDTOList(trxSearchParams, null, sqlTransaction);
                //    if (trxDTOList != null)
                //    {
                //        foreach (var atbViewDTO in attractionBookingViewDTOlist)
                //        {
                //            TransactionDTO tempDTO = trxDTOList.Where(x => x.TransactionId == atbViewDTO.AttractionBookingDTO.TrxId).FirstOrDefault();
                //            if (tempDTO != null)
                //            {
                //                atbViewDTO.TrxNo = tempDTO.TransactionNumber;
                //                atbViewDTO.Remarks = tempDTO.Remarks;

                //            }
                //        }
                //    }

                //    TransactionLineListBL trxLineListBL = new TransactionLineListBL(executionContext);
                //    List<KeyValuePair<TransactionLineDTO.SearchByParameters, string>> trxLineSearchParams = new List<KeyValuePair<TransactionLineDTO.SearchByParameters, string>>();
                //    trxLineSearchParams.Add(new KeyValuePair<TransactionLineDTO.SearchByParameters, string>(TransactionLineDTO.SearchByParameters.TRANSACTION_ID_LIST, trxIdList));
                //    List<TransactionLineDTO> trxLineDTOList = trxLineListBL.GetTransactionLineDTOList(trxLineSearchParams, 0, 500, sqlTransaction);
                //    if (trxLineDTOList != null)
                //    {
                //        foreach (var atbViewDTO in attractionBookingViewDTOlist)
                //        {
                //            TransactionLineDTO tempDTO = trxLineDTOList.Where(x => x.TransactionId == atbViewDTO.AttractionBookingDTO.TrxId && x.LineId == atbViewDTO.AttractionBookingDTO.LineId).FirstOrDefault();
                //            if (tempDTO != null)
                //            {
                //                atbViewDTO.CardId = tempDTO.CardId;
                //                atbViewDTO.CardNumber = tempDTO.CardNumber;

                //            }
                //        }
                //    }

                //if (loadSeatDetails && returnValue != null && returnValue.Count > 0)
                //{
                //    returnValue = BuildSeatDetails(returnValue);
                //}
            }
            log.LogMethodExit(attractionBookingViewDTOlist);
            return attractionBookingViewDTOlist;
        }

        private List<AttractionBookingDTO> BuildSeatDetails(List<AttractionBookingDTO> attractionBookingDTOList)
        {
            log.LogMethodEntry(attractionBookingDTOList);
            foreach (AttractionBookingDTO attractionBookingDTO in attractionBookingDTOList)
            {
                AttractionBookingSeatsListBL attractionBookingSeatsListBL = new AttractionBookingSeatsListBL(executionContext);
                List<KeyValuePair<AttractionBookingSeatsDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<AttractionBookingSeatsDTO.SearchByParameters, string>>();
                searchParam.Add(new KeyValuePair<AttractionBookingSeatsDTO.SearchByParameters, string>(AttractionBookingSeatsDTO.SearchByParameters.ATTRACTION_BOOKING_ID, attractionBookingDTO.BookingId.ToString()));
                searchParam.Add(new KeyValuePair<AttractionBookingSeatsDTO.SearchByParameters, string>(AttractionBookingSeatsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                List<AttractionBookingSeatsDTO> attractionBookingSeatsDTOList = attractionBookingSeatsListBL.GetAttractionBookingSeatsDTOList(searchParam);
                if (attractionBookingSeatsDTOList != null && attractionBookingSeatsDTOList.Count > 0)
                {
                    attractionBookingDTO.AttractionBookingSeatsDTOList = attractionBookingSeatsDTOList;
                }
            }
            log.LogMethodExit(attractionBookingDTOList);
            return attractionBookingDTOList;
        }

        /// <summary>
        /// Get the booked unit details for given day by schedule slot
        /// </summary>
        /// <param name="facilityMapIdList"></param> 
        /// <param name="ScheduleFromDate"></param>
        /// <param name="scheduleToDate"></param>
        /// <param name="productId"></param>
        /// <param name="bookingId"></param>
        /// <param name="minusOffsetSecs"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<AttractionBookingDTO> GetTotalBookedUnitsForAttractionBySchedule(List<int> facilityMapIdList, DateTime ScheduleFromDate, DateTime scheduleToDate, int productId = -1, int bookingId = -1, int minusOffsetSecs = 0, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(ScheduleFromDate, scheduleToDate, productId, bookingId, minusOffsetSecs, sqlTransaction);
            List<AttractionBookingDTO> bookedUnitsMap = new List<AttractionBookingDTO>();
            if (facilityMapIdList != null)
            {
                    AttractionBookingDatahandler attractionBookingDatahandler = new AttractionBookingDatahandler(sqlTransaction);
                    log.Debug(facilityMapIdList);
                    log.Debug("Get total booked units for facilities :" + ScheduleFromDate + ":" + scheduleToDate + ":" + -1);
                    bookedUnitsMap.AddRange(attractionBookingDatahandler.GetTotalBookedUnitsForAttractionsBySchedule(facilityMapIdList, ScheduleFromDate, scheduleToDate, productId, bookingId, minusOffsetSecs));
            }
            else
            {
                log.LogMethodExit("this.facilityMapDTO == null");
                throw new ValidationException(MessageContainerList.GetMessage(this.executionContext, "Virtual Facility details are not loaded"));
            }
            log.LogMethodExit(bookedUnitsMap);
            return bookedUnitsMap;
        }
    }

}
