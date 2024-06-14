/********************************************************************************************
 * Project Name - Day Attraction Schedule BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.70.2      16-Oct-2019      Deeksha         Created for club speed
 *2.70.3      07-Jan-2020      Nitin Pai       Day Attraction and Reschedule Slot changes
 *2.80        20-May-2020      Girish Kundar  Modified : Phase -1 changes for REST API
 *2.100       24-Sep-2020      Nitin Pai       Attraction Reschedule: Updated DAS BL logic to 
 *                                             save and get schedule information
 *2.120.0     04-Mar-2021      Sathyavathi     Enabling option nto decide ''Multiple-Booking at Facility level                                            
 *2.130       07-Jun-2021      Nitin Pai       Funstasia Fix: Master schedules did not consider product start date
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.Transaction
{
    public class DayAttractionScheduleBL
    {
        private DayAttractionScheduleDTO dayAttractionScheduleDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private static Dictionary<int, List<int>> relatedFacilityMaps = new Dictionary<int, List<int>>();

        /// <summary>
        /// Default constructor of DayAttractionScheduleBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public DayAttractionScheduleBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the DayAttractionSchedule id as the parameter
        /// Would fetch the DayAttractionSchedule object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="sqlTransaction">SqlTransaction</param>
        /// <param name="id">Id</param>
        public DayAttractionScheduleBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null, bool buildChildRecords = false)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            DayAttractionScheduleDataHandler dayAttractionScheduleDataHandler = new DayAttractionScheduleDataHandler(sqlTransaction);
            dayAttractionScheduleDTO = dayAttractionScheduleDataHandler.GetDayAttractionScheduleDTO(id);

            if (buildChildRecords && dayAttractionScheduleDTO != null && dayAttractionScheduleDTO.FacilityMapId != -1)
            {
                FacilityMapBL facilityMapBL = new FacilityMapBL(executionContext, dayAttractionScheduleDTO.FacilityMapId, true, true, sqlTransaction);
                this.dayAttractionScheduleDTO.FacilityDTO = facilityMapBL.FacilityMapDTO.FacilityMapDetailsDTOList[0].FacilityDTOList[0];
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates DayAttractionScheduleBL object using the dayAttractionScheduleDTO
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="dayAttractionScheduleDTO">dayAttractionScheduleDTO object</param>
        public DayAttractionScheduleBL(ExecutionContext executionContext, DayAttractionScheduleDTO dayAttractionScheduleDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, dayAttractionScheduleDTO);
            this.dayAttractionScheduleDTO = dayAttractionScheduleDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// get DayAttractionScheduleDTO Object
        /// </summary>
        public DayAttractionScheduleDTO GetDayAttractionScheduleDTO
        {
            get { return dayAttractionScheduleDTO; }
        }

        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            String message = "";
            List<ValidationError> validationErrorList = new List<ValidationError>();
            string errorMessage = string.Empty;
            bool rescheduleSlot = false;
            LookupValuesList serverTimeObject = new LookupValuesList(executionContext);

            if (dayAttractionScheduleDTO == null)
            {
                message = "Cannot proceed dayAttractionSchedule record is Empty.";
                errorMessage = MessageContainerList.GetMessage(executionContext, 2446, (MessageContainerList.GetMessage(executionContext, message))); 
                validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "DayAttractionSchedule"), MessageContainerList.GetMessage(executionContext, "DayAttractionScheduleDTO"), errorMessage));
            }

            List<int> facilityMapList = new List<int>();
            if (relatedFacilityMaps.ContainsKey(dayAttractionScheduleDTO.FacilityMapId))
            {
                facilityMapList = relatedFacilityMaps[dayAttractionScheduleDTO.FacilityMapId];
            }
            else
            {
                FacilityMapListBL facilityMapListBL = new FacilityMapListBL(executionContext);
                facilityMapList = facilityMapListBL.GetFacilityMapsForSameFacility(dayAttractionScheduleDTO.FacilityMapId);
                relatedFacilityMaps.Add(dayAttractionScheduleDTO.FacilityMapId, facilityMapList);
            }

            //Get das for input params
            DayAttractionScheduleListBL dayAttractionScheduleListBL = new DayAttractionScheduleListBL(executionContext);
            List<KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>(DayAttractionScheduleDTO.SearchByParameters.ATTRACTION_SCHEDULE_ID, dayAttractionScheduleDTO.AttractionScheduleId.ToString()));
            searchParameters.Add(new KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>(DayAttractionScheduleDTO.SearchByParameters.SCHEDULE_DATETIME, dayAttractionScheduleDTO.ScheduleDateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
            searchParameters.Add(new KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>(DayAttractionScheduleDTO.SearchByParameters.IS_ACTIVE, "Y"));
            if(facilityMapList != null && facilityMapList.Any())
                searchParameters.Add(new KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>(DayAttractionScheduleDTO.SearchByParameters.FACILITY_MAP_ID_LIST, string.Join(",", facilityMapList)));
            searchParameters.Add(new KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>(DayAttractionScheduleDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<DayAttractionScheduleDTO> dayAttractionScheduleDTOs = dayAttractionScheduleListBL.GetAllDayAttractionScheduleList(searchParameters, sqlTransaction);

            if (dayAttractionScheduleDTOs != null && dayAttractionScheduleDTOs.Any())
            {
                dayAttractionScheduleDTOs = dayAttractionScheduleDTOs.Where(x => x.ExpiryTime == DateTime.MinValue || x.ExpiryTime > serverTimeObject.GetServerDateTime()).ToList();
            }

            if (dayAttractionScheduleDTOs != null && dayAttractionScheduleDTOs.Any())
            {
                dayAttractionScheduleDTOs = dayAttractionScheduleDTOs.OrderByDescending(x => x.LastUpdatedDate).ToList();
                if (dayAttractionScheduleDTO == dayAttractionScheduleDTOs[0])
                {
                    // if all the attributes match but the DAS id diffrs it means that the DAS was manually modified. 
                    // replace the DASid of the DTO with DB entry and try to inactivate the send id
                    if (dayAttractionScheduleDTO.DayAttractionScheduleId != dayAttractionScheduleDTOs[0].DayAttractionScheduleId)
                    {
                        if (dayAttractionScheduleDTO.DayAttractionScheduleId == -1)
                        {
                            // set these additional params from DB to te input DTO. Do not replace the DTO
                            dayAttractionScheduleDTO.DayAttractionScheduleId = dayAttractionScheduleDTOs[0].DayAttractionScheduleId;
                            dayAttractionScheduleDTO.Source = dayAttractionScheduleDTOs[0].Source;
                            if (!String.IsNullOrEmpty(dayAttractionScheduleDTO.ScheduleStatus) && (
                                dayAttractionScheduleDTO.ScheduleStatus.Equals(DayAttractionScheduleDTO.ScheduleStatusEnumToString(DayAttractionScheduleDTO.ScheduleStatusEnum.RESCHEDULE_COMPLETE))
                                || dayAttractionScheduleDTO.ScheduleStatus.Equals(DayAttractionScheduleDTO.ScheduleStatusEnumToString(DayAttractionScheduleDTO.ScheduleStatusEnum.RESCHEDULE))))
                            {
                                rescheduleSlot = true;
                            }
                            else
                            {
                                dayAttractionScheduleDTO.ScheduleStatus = dayAttractionScheduleDTOs[0].ScheduleStatus;
                            }
                            dayAttractionScheduleDTO.AttractionPlayId = dayAttractionScheduleDTOs[0].AttractionPlayId;
                            dayAttractionScheduleDTO.AttractionPlayName = dayAttractionScheduleDTOs[0].AttractionPlayName;
                        }
                        else
                        {
                            if (!dayAttractionScheduleDTO.ScheduleStatus.Equals(DayAttractionScheduleDTO.ScheduleStatusEnumToString(DayAttractionScheduleDTO.ScheduleStatusEnum.RESCHEDULE_COMPLETE)))
                            {
                                try
                                {
                                    DayAttractionScheduleBL inputDASBL = new DayAttractionScheduleBL(executionContext, dayAttractionScheduleDTO.DayAttractionScheduleId);
                                    inputDASBL.GetDayAttractionScheduleDTO.IsActive = false;
                                    inputDASBL.Save(sqlTransaction);
                                }
                                catch (Exception ex)
                                {
                                    log.Error("Error while saving the DAS sent as input " + ex.Message);
                                }
                                dayAttractionScheduleDTO.DayAttractionScheduleId = dayAttractionScheduleDTOs[0].DayAttractionScheduleId;
                            }
                        }
                    }
                    else
                    {
                        // set these additional params from DB to te input DTO. Do not replace the DTO
                        dayAttractionScheduleDTO.DayAttractionScheduleId = dayAttractionScheduleDTOs[0].DayAttractionScheduleId;
                        dayAttractionScheduleDTO.Source = dayAttractionScheduleDTOs[0].Source;
                        if (!String.IsNullOrEmpty(dayAttractionScheduleDTOs[0].ExternalSystemReference))
                            dayAttractionScheduleDTO.ExternalSystemReference = dayAttractionScheduleDTOs[0].ExternalSystemReference;
                        if (!String.IsNullOrEmpty(dayAttractionScheduleDTO.ScheduleStatus) && (
                                dayAttractionScheduleDTO.ScheduleStatus.Equals(DayAttractionScheduleDTO.ScheduleStatusEnumToString(DayAttractionScheduleDTO.ScheduleStatusEnum.RESCHEDULE_COMPLETE))
                                || dayAttractionScheduleDTO.ScheduleStatus.Equals(DayAttractionScheduleDTO.ScheduleStatusEnumToString(DayAttractionScheduleDTO.ScheduleStatusEnum.RESCHEDULE))))
                        {
                            rescheduleSlot = true;
                        }
                        else
                        {
                            dayAttractionScheduleDTO.ScheduleStatus = dayAttractionScheduleDTOs[0].ScheduleStatus;
                        }
                        dayAttractionScheduleDTO.AttractionPlayId = dayAttractionScheduleDTOs[0].AttractionPlayId;
                        dayAttractionScheduleDTO.AttractionPlayName = dayAttractionScheduleDTOs[0].AttractionPlayName;
                    }
                }
                else if(dayAttractionScheduleDTO.FacilityMapId != dayAttractionScheduleDTOs[0].FacilityMapId && relatedFacilityMaps[dayAttractionScheduleDTO.FacilityMapId].Contains(dayAttractionScheduleDTOs[0].FacilityMapId))
                {
                    // if the details are not matching, check which details are not matching. If the facility map is not matching, it indicates that the slot is already booked by another facility map
                    if (dayAttractionScheduleDTOs[0].ExpiryTime == DateTime.MinValue || dayAttractionScheduleDTOs[0].ExpiryTime > serverTimeObject.GetServerDateTime())
                    {
                        message = "This slot is blocked by a different facility map.";
                        errorMessage = MessageContainerList.GetMessage(executionContext, 2348, (MessageContainerList.GetMessage(executionContext, message)));
                        validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "DayAttractionSchedule"), "InvalidScheduleSlot", message));
                    }
                }
                else if (dayAttractionScheduleDTO.FacilityMapId == dayAttractionScheduleDTOs[0].FacilityMapId && relatedFacilityMaps[dayAttractionScheduleDTO.FacilityMapId].Contains(dayAttractionScheduleDTOs[0].FacilityMapId))
                {
                    try
                    {
                        if (dayAttractionScheduleDTO.Source != dayAttractionScheduleDTOs[0].Source)
                        {
                            FacilityMapBL facilityMapBL = new FacilityMapBL(executionContext, dayAttractionScheduleDTO.FacilityMapId, true, true, sqlTransaction);
                            facilityMapBL.CanAllowMulitpleBookingsForTheSlot(sqlTransaction);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "DayAttractionSchedule"), MessageContainerList.GetMessage(executionContext, "InvalidScheduleSlot"), ex.Message));   
                    }

                }
            }
            else if(dayAttractionScheduleDTO.DayAttractionScheduleId == -1)
            {
                throw new RowNotInTableException("No day attraction schedule found");
            }

            if (DayAttractionScheduleDTO.ScheduleStatusEnumFromString(dayAttractionScheduleDTO.ScheduleStatus).Equals(DayAttractionScheduleDTO.ScheduleStatusEnum.RESCHEDULE_COMPLETE))
                rescheduleSlot = true;
            //if (facilityMapId > -1)
            //{
            //    if (facilityMapId == dayAttractionScheduleDTO.FacilityMapId)
            //    {
            //        if (ParafaitDefaultContainer.GetParafaitDefault(executionContext, "ALLOW_MULTIPLE_BOOKINGS_WITHIN_SCHEDULE").Equals("N") && dayAttractionScheduleDTO.Source != source )
            //        //if(dayAttractionScheduleDTO.Blocked && dayAttractionScheduleDTO.Source != source)
            //        {
            //            message = "This slot is blocked by a different source.";
            //            errorMessage = MessageContainerList.GetMessage(executionContext, 2447, (MessageContainerList.GetMessage(executionContext, message)));
            //            validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "DayAttractionSchedule"), MessageContainerList.GetMessage(executionContext, "InvalidScheduleSlot"), errorMessage));
            //        }
            //    }
            //    else
            //    {
            //        //if (dayAttractionScheduleDTO.Blocked && (dayAttractionScheduleDTO.ExpiryTime == DateTime.MinValue || dayAttractionScheduleDTO.ExpiryTime > serverTimeObject.GetServerDateTime()))
            //        if (dayAttractionScheduleDTO.ExpiryTime == DateTime.MinValue || dayAttractionScheduleDTO.ExpiryTime > serverTimeObject.GetServerDateTime())
            //        {
            //            message = "This slot is blocked by a different facility map.";
            //            errorMessage = MessageContainerList.GetMessage(executionContext, 2348, (MessageContainerList.GetMessage(executionContext, message)));
            //            validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "DayAttractionSchedule"), "InvalidScheduleSlot", message));
            //        }
            //    }
            //}

            if (!string.IsNullOrEmpty(dayAttractionScheduleDTO.ExternalSystemReference))
            {
                if (dayAttractionScheduleDTO.ExternalSystemReference.Length > 50)
                {
                    message = "ExternalSystemReference should be less than 50 characters.";
                    errorMessage = MessageContainerList.GetMessage(executionContext, 2348, (MessageContainerList.GetMessage(executionContext, message)));
                    validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "DayAttractionSchedule"), MessageContainerList.GetMessage(executionContext, "ExternalSystemReference"), errorMessage));
                }
            }

            if (!rescheduleSlot && !String.IsNullOrEmpty(dayAttractionScheduleDTO.ScheduleStatus) 
                && DayAttractionScheduleDTO.ScheduleStatusEnumFromString(dayAttractionScheduleDTO.ScheduleStatus) != DayAttractionScheduleDTO.ScheduleStatusEnum.OPEN 
                && DayAttractionScheduleDTO.ScheduleStatusEnumFromString(dayAttractionScheduleDTO.ScheduleStatus) != DayAttractionScheduleDTO.ScheduleStatusEnum.BLOCKED)
            {
                message = "This slot is already in use and is not available.";
                errorMessage = MessageContainerList.GetMessage(executionContext, 2449, (MessageContainerList.GetMessage(executionContext, message)));
                validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "DayAttractionSchedule"), MessageContainerList.GetMessage(executionContext, "DayAttractionScheduleDTO"), errorMessage));
            }

            if(!dayAttractionScheduleDTO.IsActive)
            {
                AttractionBookingList attractionBookingList = new AttractionBookingList(executionContext);
                List<KeyValuePair<AttractionBookingDTO.SearchByParameters, string>> atbSearchParams = new List<KeyValuePair<AttractionBookingDTO.SearchByParameters, string>>();
                atbSearchParams.Add(new KeyValuePair<AttractionBookingDTO.SearchByParameters, string>(AttractionBookingDTO.SearchByParameters.DAY_ATTRACTION_SCHEDULE_ID, dayAttractionScheduleDTO.DayAttractionScheduleId.ToString()));
                //atbSearchParams.Add(new KeyValuePair<AttractionBookingDTO.SearchByParameters, string>(AttractionBookingDTO.SearchByParameters.IS_EXPIRY_DATE_EXPIRED, "Y"));
                List<AttractionBookingDTO> attractionBookingDTOList = attractionBookingList.GetAttractionBookingDTOList(atbSearchParams, true, sqlTransaction);
                if(attractionBookingDTOList != null && attractionBookingDTOList.Any())
                {
                    attractionBookingDTOList = attractionBookingDTOList.Where(x => (x.ExpiryDate == DateTime.MinValue || x.ExpiryDate > serverTimeObject.GetServerDateTime())).ToList();
                    if (attractionBookingDTOList != null && attractionBookingDTOList.Any())
                    {
                        message = "Cancel attraction bookings before cancelling Day Attraction Schedule.";
                        errorMessage = MessageContainerList.GetMessage(executionContext, 2775, (MessageContainerList.GetMessage(executionContext, message)));
                        validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "DayAttractionSchedule"), MessageContainerList.GetMessage(executionContext, "DayAttractionScheduleDTO"), errorMessage));
                    }
                }
            }
            else if (dayAttractionScheduleDTO.ExpiryTime != DateTime.MinValue && dayAttractionScheduleDTO.ExpiryTime < serverTimeObject.GetServerDateTime())
            {
                message = "This slot has expired.";
                errorMessage = MessageContainerList.GetMessage(executionContext, 2348, (MessageContainerList.GetMessage(executionContext, message)));
                throw new EntityExpiredException("Day Attraction Schedule has expired");
            }

            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Saves the DayAttractionSchedule
        /// Checks if the DayAttractionScheduleId is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            DayAttractionScheduleDataHandler dayAttractionScheduleDataHandler = new DayAttractionScheduleDataHandler(sqlTransaction);
            try
            {
                List<ValidationError> validationErrorList = Validate(sqlTransaction);
                if (validationErrorList.Count > 0)
                {
                    throw new ValidationException("Validation Failed", validationErrorList);
                }
            }
            catch (RowNotInTableException ex)
            {
                // do nothing
            }
            catch (Exception ex)
            {
                log.Debug("validate day attraction schedule failed." + ex.Message);
                throw;
            }

            if (dayAttractionScheduleDTO.ScheduleStatus.Equals(DayAttractionScheduleDTO.ScheduleStatusEnumToString(DayAttractionScheduleDTO.ScheduleStatusEnum.RESCHEDULE_COMPLETE)))
            {
                dayAttractionScheduleDTO.ScheduleStatus = DayAttractionScheduleDTO.ScheduleStatusEnumToString(DayAttractionScheduleDTO.ScheduleStatusEnum.OPEN);
            }

            if (dayAttractionScheduleDTO.DayAttractionScheduleId < 0)
            {
                dayAttractionScheduleDTO = dayAttractionScheduleDataHandler.InsertDayAttractionSchedule(dayAttractionScheduleDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                dayAttractionScheduleDTO.AcceptChanges();
            }
            else
            {
                if (dayAttractionScheduleDTO.IsChanged)
                {
                    dayAttractionScheduleDTO = dayAttractionScheduleDataHandler.UpdateDayAttractionSchedule(dayAttractionScheduleDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    dayAttractionScheduleDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

    }
    /// <summary>
    /// Manages the list of DayAttractionSchedule
    /// </summary>
    public class DayAttractionScheduleListBL
    {
        private List<DayAttractionScheduleDTO> dayAttractionScheduleDTOList;
        private ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public DayAttractionScheduleListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.dayAttractionScheduleDTOList = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="dayAttractionScheduleDTOList"></param>
        /// <param name="executionContext"></param>
        public DayAttractionScheduleListBL(ExecutionContext executionContext, List<DayAttractionScheduleDTO> dayAttractionScheduleDTOList)
        {
            log.LogMethodEntry(dayAttractionScheduleDTOList, executionContext);
            this.executionContext = executionContext;
            this.dayAttractionScheduleDTOList = dayAttractionScheduleDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Save or update records with inner collections
        /// </summary>
        public void SaveDayAttractionSchedule()
        {
            try
            {
                log.LogMethodEntry();
                if (dayAttractionScheduleDTOList != null)
                {
                    foreach (DayAttractionScheduleDTO dayAttractionScheduleDTO in dayAttractionScheduleDTOList)
                    {
                        DayAttractionScheduleBL dayAttractionScheduleBL = new DayAttractionScheduleBL(executionContext, dayAttractionScheduleDTO);
                        dayAttractionScheduleBL.Save();
                    }
                }

                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Returns the DayAttractionSchedule  List
        /// </summary>
        public List<DayAttractionScheduleDTO> GetAllDayAttractionScheduleList(List<KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null, bool buildChildRecords = false)
        {
            log.LogMethodEntry(searchParameters);
            DayAttractionScheduleDataHandler dayAttractionScheduleDataHandler = new DayAttractionScheduleDataHandler(sqlTransaction);
            List<DayAttractionScheduleDTO> dayAttractionScheduleList = dayAttractionScheduleDataHandler.GetDayAttractionScheduleDTOList(searchParameters);
            log.LogMethodExit(dayAttractionScheduleList);
            if (buildChildRecords && dayAttractionScheduleList != null && dayAttractionScheduleList.Any())
            {
                foreach (DayAttractionScheduleDTO dayAttractionScheduleDTO in dayAttractionScheduleList)
                {
                    FacilityMapBL facilityMapBL = new FacilityMapBL(executionContext, dayAttractionScheduleDTO.FacilityMapId, true, true, sqlTransaction);
                    dayAttractionScheduleDTO.FacilityDTO = facilityMapBL.FacilityMapDTO.FacilityMapDetailsDTOList[0].FacilityDTOList[0];
                }
            }
            log.LogMethodExit(dayAttractionScheduleList);
            return dayAttractionScheduleList;
        }
    }
}

