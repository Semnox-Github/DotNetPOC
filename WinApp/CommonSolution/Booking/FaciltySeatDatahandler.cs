/* Project Name - ReservationCoreDatahnadler Programs 
* Description  - Data object of the ReservationDatahandler
* 
**************
**Version Log
**************
*Version     Date           Modified By             Remarks          
*********************************************************************************************
*1.00        25-Dec-2016    Rakshith                Created 
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.Booking
{
    public class FaciltySeatDatahandler
    {
        
        DataAccessHandler dataAccessHandler;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();

        string connstring;

        /// <summary>
        /// Default constructor of  FaciltySeatDatahandler class
        /// </summary>
        public FaciltySeatDatahandler()
        {
            log.Debug("starts-FaciltySeatDatahandler() Method.");
            dataAccessHandler = new DataAccessHandler();
            connstring = dataAccessHandler.ConnectionString;
            log.Debug("Ends-FaciltySeatDatahandler() Method.");
        }


        public List<FacilitySeatLayoutDTO> GetSeatLayout(int attractionScheduleId)
        {
            List<FacilitySeatLayoutDTO> facilitySeatLayoutList= new List<FacilitySeatLayoutDTO>();
                
            try
            {
                string seatQuery = @"select * 
                                                from FacilitySeatLayout fsl
                                                where FacilityId = (select FacilityId from AttractionSchedules where AttractionScheduleId = @AttractionScheduleId)
                                                and (Type in ('A', 'P')
	                                                or exists (select 1
			                                                from FacilitySeats fs
			                                                where fs.RowIndex = fsl.RowColumnIndex
			                                                    and fsl.Type = 'R'
			                                                    and fsl.FacilityId = fs.FacilityId)
	                                                or exists (select 1
			                                                from FacilitySeats fs
			                                                where fs.ColumnIndex = fsl.RowColumnIndex
			                                                    and fsl.Type = 'C'
			                                                    and fsl.FacilityId = fs.FacilityId))
                                                order by RowColumnIndex, Type desc"; 

                SqlParameter[] seatingParameters = new SqlParameter[1];
                seatingParameters[0] = new SqlParameter("@AttractionScheduleId", attractionScheduleId);

                DataTable dtSeats = dataAccessHandler.executeSelectQuery(seatQuery, seatingParameters);
                if (dtSeats.Rows.Count > 0)
                {
                    foreach (DataRow seatRow in dtSeats.Rows)
                    {
                         FacilitySeatLayoutDTO facilitySeatLayoutDTO=new FacilitySeatLayoutDTO(
                             seatRow["LayoutId"] == DBNull.Value ? -1 : Convert.ToInt32(seatRow["LayoutId"]),
                             seatRow["FacilityId"] == DBNull.Value ? -1 : Convert.ToInt32(seatRow["FacilityId"]),
                             seatRow["RowColumnName"].ToString(),
                             seatRow["Type"] == DBNull.Value ? '\0' : Convert.ToChar(seatRow["Type"]),
                             seatRow["RowColumnIndex"] == DBNull.Value ? -1 : Convert.ToInt32(seatRow["RowColumnIndex"]),
                             seatRow["HasSeats"] == DBNull.Value ? '\0' : Convert.ToChar(seatRow["HasSeats"]),
                             seatRow["Guid"].ToString(),
                             seatRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(seatRow["SynchStatus"]),
                             seatRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(seatRow["site_id"]),
                             seatRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(seatRow["MasterEntityId"]) 
                             );
                         facilitySeatLayoutList.Add(facilitySeatLayoutDTO);
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception("At GetSeatLayout -> " + ex.Message);
            }
            return facilitySeatLayoutList;
        }


        public List<FacilitySeatsDTO> GetSeats(int attractionScheduleId,DateTime scheduleDate)
        {
            List<FacilitySeatsDTO> FacilitySeatsList= new List<FacilitySeatsDTO>();
                
            try
            {
                //List<AttractionBookingSeatsDTO> attractionBookingSeatList = GetAttractionBookinSeats(attractionScheduleId);


                string seatQuery = @"select fs.*, abs.SeatId bookedSeat
                        from FacilitySeats fs left outer join 
                        (
                        select abs.SeatId	
                        from AttractionBookingSeats abs, AttractionBookings atb, AttractionSchedules ats   
                        where atb.BookingId = abs.BookingId
                        and atb.ScheduleTime = @Time
                        and ats.AttractionScheduleId = atb.AttractionScheduleId
                        and ats.AttractionScheduleId = @AttractionScheduleId
                          ) abs
                        on abs.SeatId = fs.SeatId
                        where fs.FacilityId = (select FacilityId from AttractionSchedules where AttractionScheduleId = @AttractionScheduleId)";

                SqlParameter[] seatingParameters = new SqlParameter[2];
                seatingParameters[0] = new SqlParameter("@AttractionScheduleId", attractionScheduleId);
                seatingParameters[1] = new SqlParameter("@Time", scheduleDate);

                DataTable dtSeats = dataAccessHandler.executeSelectQuery(seatQuery, seatingParameters);
                if (dtSeats.Rows.Count > 0)
                {
                    foreach (DataRow seatRow in dtSeats.Rows)
                    {
                        //int bookedId=-1;
                        //foreach (AttractionBookingSeatsDTO attractionBookingSeatsDTO in attractionBookingSeatList)
                        //{
                            
                        //}

                         FacilitySeatsDTO facilitySeatsDTO=new FacilitySeatsDTO(
                                            seatRow["SeatId"] == DBNull.Value ? -1 : Convert.ToInt32(seatRow["SeatId"]),
                                            seatRow["SeatName"].ToString(),
                                            seatRow["RowIndex"] == DBNull.Value ? -1 : Convert.ToInt32(seatRow["RowIndex"]),
                                            seatRow["ColumnIndex"] == DBNull.Value ? -1 : Convert.ToInt32(seatRow["ColumnIndex"]),
                                            seatRow["Active"] == DBNull.Value ? '\0' : Convert.ToChar(seatRow["Active"]),
                                            seatRow["FacilityId"] == DBNull.Value ? -1 : Convert.ToInt32(seatRow["FacilityId"]),
                                            seatRow["IsAccessible"] == DBNull.Value ? '\0' : Convert.ToChar(seatRow["IsAccessible"]),
                                            seatRow["Guid"].ToString(),
                                            seatRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(seatRow["LayoutId"]),
                                            seatRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(seatRow["site_id"]),
                                            seatRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(seatRow["MasterEntityId"])  
                                            
                                            );

                         facilitySeatsDTO.BookedSeat = seatRow["bookedSeat"] == DBNull.Value ? -1 : Convert.ToInt32(seatRow["bookedSeat"]);

                         FacilitySeatsList.Add(facilitySeatsDTO);
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception("At GetSeatLayout -> " + ex.Message);
            }
            return FacilitySeatsList;
        }


        public List<AttractionBookingSeatsDTO> GetAttractionBookinSeats(int attractionScheduleId)
        {
            List<AttractionBookingSeatsDTO> attractionBookingSeatList = new List<AttractionBookingSeatsDTO>();

            try
            {
                string seatQuery = @"select a.* from AttractionBookingSeats a 
                left   join  FacilitySeats fs on fs.SeatId=a.SeatId 
                left   join AttractionSchedules ash on ash.AttractionScheduleId= @AttractionScheduleId)";

                SqlParameter[] seatingParameters = new SqlParameter[1];
                seatingParameters[0] = new SqlParameter("@AttractionScheduleId", attractionScheduleId);
                   DataTable dtSeats = dataAccessHandler.executeSelectQuery(seatQuery, seatingParameters);
                if (dtSeats.Rows.Count > 0)
                {
                    foreach (DataRow seatRow in dtSeats.Rows)
                    {
                         AttractionBookingSeatsDTO attractionBookingSeatsDTO=new AttractionBookingSeatsDTO(
                                seatRow["BookingSeatId"] == DBNull.Value ? -1 : Convert.ToInt32(seatRow["BookingSeatId"]),
                                seatRow["BookingId"] == DBNull.Value ? -1 : Convert.ToInt32(seatRow["BookingId"]),
                                seatRow["SeatId"] == DBNull.Value ? -1 : Convert.ToInt32(seatRow["SeatId"]),
                                seatRow["CardId"] == DBNull.Value ? -1 : Convert.ToInt32(seatRow["CardId"]),
                                seatRow["Guid"].ToString() ,
                                seatRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(seatRow["SynchStatus"]),
                                seatRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(seatRow["site_id"]),
                                seatRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(seatRow["MasterEntityId"])
                               );
                         attractionBookingSeatList.Add(attractionBookingSeatsDTO);
                    }
                }
            }
            catch
            {
            }
            return attractionBookingSeatList;

        }

    }
    
}
