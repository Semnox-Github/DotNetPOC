/* Project Name -FacilitySeats
* Description  - BL class of FacilitySeats
* 
**************
**Version Log
**************
*Version     Date           Modified By          Remarks          
*********************************************************************************************
*1.00        25-Dec-2016    Rakshith             Created 
*2.60.3      26-Feb-2019    Akshay Gulaganji    Created a FacilitySeatsBL Class 
*2.70        26-Mar-2019    Guru S A             Booking phase 2 enhancement changes 
********************************************************************************************/

using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
//using Semnox.Core.HR.Users;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Business logic for FacilitySeats class.
    /// </summary>
    public class FacilitySeatsBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);//Added on 26 Feb 2019 by Akshay Gulaganji
        private ExecutionContext executionContext;
        private FacilitySeatsDTO facilitySeatsDTO;

        ///<summary>
        /// Default constructor
        ///</summary>
        public FacilitySeatsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.facilitySeatsDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the parameters
        /// </summary>
        /// <param name="facilitySeatsDTO"></param>
        /// <param name="executionContext"></param>
        public FacilitySeatsBL(ExecutionContext executionContext, FacilitySeatsDTO facilitySeatsDTO) : this(executionContext)
        {
            log.LogMethodEntry(facilitySeatsDTO, executionContext);
            this.facilitySeatsDTO = facilitySeatsDTO;
            log.LogMethodEntry();
        }

        /// <summary>
        /// Saves the FacilitySeats
        /// Checks if the  LayoutId is not less than 0
        /// If it is less than 0, then inserts
        /// else updates
        /// </summary>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            FaciltySeatDatahandler faciltySeatDatahandler = new FaciltySeatDatahandler(sqlTransaction);

            if (facilitySeatsDTO.SeatId < 0)
            {
                facilitySeatsDTO = faciltySeatDatahandler.InsertFacilitySeats(facilitySeatsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                facilitySeatsDTO.AcceptChanges();
            }
            else
            {
                if (facilitySeatsDTO.SeatId > 0 && facilitySeatsDTO.IsChanged)
                {
                    facilitySeatsDTO = faciltySeatDatahandler.UpdateFacilitySeats(facilitySeatsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    facilitySeatsDTO.AcceptChanges();
                }

            }
            log.LogMethodExit();
        }

        internal void Delete(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            try
            {
                FaciltySeatDatahandler faciltySeatDatahandler = new FaciltySeatDatahandler(sqlTransaction);
                faciltySeatDatahandler.Delete(facilitySeatsDTO.SeatId);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }



        /// <summary>
        /// Validates the facilitySeatLayoutDTO
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            if (facilitySeatsDTO.FacilityId < 0)
            {
                validationErrorList.Add(new ValidationError("Facility", "FacilityId", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Facility Id"))));
            }

            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        ///// <summary>
        ///// Checkes the attractionBookingSeats while making Active as D for Facility Seats
        ///// </summary>
        ///// <returns>bool value</returns>
        //private bool GetAttractionBookingSeats()
        //{
        //    log.LogMethodEntry();
        //    AttractionBookingSeatsDTO attractionBookingSeatsDTO = null;
        //    //Needs to implement functionality for this block Once MT Creates 3 tier for AttractionBookingSeats

        //    if (attractionBookingSeatsDTO != null)
        //    {
        //        log.LogMethodExit();
        //        return true;
        //    }
        //    log.LogMethodExit();
        //    return false;
        //}

        public List<FacilitySeatLayoutDTO> GetFacilitySeatLayout(int facilityId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(facilityId);
            FaciltySeatDatahandler faciltySeatDatahandler = new FaciltySeatDatahandler(sqlTransaction);
            List<FacilitySeatLayoutDTO> facilitySeatLayoutDTOList = faciltySeatDatahandler.GetFacilitySeatLayout(facilityId);
            log.LogMethodExit(facilitySeatLayoutDTOList);
            return facilitySeatLayoutDTOList;
        }

        public List<CoreKeyValueStruct> GetFacilitySeatLayoutDetails(int facilityId)
        {
            log.LogMethodEntry(facilityId);
            List<CoreKeyValueStruct> CoreKeyValueStructList = new List<CoreKeyValueStruct>();
            int row = 0;
            int col = 0;
            List<FacilitySeatLayoutDTO> facilitySeatLayoutList = GetFacilitySeatLayout(facilityId);

            foreach (FacilitySeatLayoutDTO facilitySeatLayoutDTO in facilitySeatLayoutList)
            {
                if (facilitySeatLayoutDTO.Type == 'R' && facilitySeatLayoutDTO.HasSeats == 'Y')
                {
                    row++;
                }
                if (facilitySeatLayoutDTO.Type == 'C' && facilitySeatLayoutDTO.HasSeats == 'Y')
                {
                    col++;
                }
            }

            CoreKeyValueStructList.Add(new CoreKeyValueStruct("row", row.ToString()));
            CoreKeyValueStructList.Add(new CoreKeyValueStruct("column", col.ToString()));

            log.LogMethodExit(CoreKeyValueStructList);
            return CoreKeyValueStructList;
        }

        public List<FacilitySeatsDTO> GetFacilitySeats(int facilityId, int attractionScheduleId, DateTime scheduleDate,
                                                        SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(facilityId, attractionScheduleId, scheduleDate);
            FaciltySeatDatahandler faciltySeatDatahandler = new FaciltySeatDatahandler(sqlTransaction);
            List<FacilitySeatsDTO> facilitySeatsDTOList = faciltySeatDatahandler.GetFacilitySeats(facilityId, attractionScheduleId, scheduleDate);
            log.LogMethodExit(facilitySeatsDTOList);
            return facilitySeatsDTOList;
        }

    }

    /// <summary>
    /// Manages the list of FacilitySeatLayout
    /// </summary>
    public class FacilitySeatsListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<FacilitySeatsDTO> facilitySeatsDTOList;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public FacilitySeatsListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.facilitySeatsDTOList = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor of FacilitySeatsListBL class with facilitySeatsDTOList and executionContext
        /// </summary>
        /// <param name="facilitySeatsDTOList">facilitySeatLayoutDTOList</param>
        /// <param name="executionContext">executionContext</param>
        public FacilitySeatsListBL(ExecutionContext executionContext, List<FacilitySeatsDTO> facilitySeatsDTOList)
        {
            log.LogMethodEntry(facilitySeatsDTOList, executionContext);
            this.executionContext = executionContext;
            this.facilitySeatsDTOList = facilitySeatsDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the FacilitySeatsDTO List
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <returns></returns>
        public List<FacilitySeatsDTO> GetFacilitySeatsDTOList(List<KeyValuePair<FacilitySeatsDTO.SearchByFacilitySeatsParameter, string>> searchParameters,
                                      SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            List<FacilitySeatsDTO> facilitySeatsDTOList;
            FaciltySeatDatahandler faciltySeatDatahandler = new FaciltySeatDatahandler(sqlTransaction);
            facilitySeatsDTOList = faciltySeatDatahandler.GetFacilitySeatsDTOList(searchParameters);

            log.LogMethodEntry(facilitySeatsDTOList);
            return facilitySeatsDTOList;
        }


        /// <summary>
        /// Saves the FacilitySeatsDTO List
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            try
            {
                log.LogMethodEntry(sqlTransaction);
                if (facilitySeatsDTOList != null && facilitySeatsDTOList.Any())
                {
                    foreach (FacilitySeatsDTO FacilitySeatsDTO in facilitySeatsDTOList)
                    {
                        FacilitySeatsBL facilitySeats = new FacilitySeatsBL(executionContext, FacilitySeatsDTO);
                        facilitySeats.Save(sqlTransaction);
                    }
                }
                log.LogMethodExit();
            }
            catch (SqlException sqlEx)
            {
                log.Error(sqlEx);
                log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                if (sqlEx.Number == 547)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        public void Delete(SqlTransaction sqlTransaction = null)
        {
            try
            {
                log.LogMethodEntry(sqlTransaction);
                if (facilitySeatsDTOList != null && facilitySeatsDTOList.Any())
                {
                    foreach (FacilitySeatsDTO FacilitySeatsDTO in facilitySeatsDTOList)
                    {
                        FacilitySeatsBL facilitySeats = new FacilitySeatsBL(executionContext, FacilitySeatsDTO);
                        facilitySeats.Delete(sqlTransaction);
                    }
                }
                log.LogMethodExit();
            }
            catch (SqlException sqlEx)
            {
                log.Error(sqlEx);
                log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                if (sqlEx.Number == 547)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

    }
}
