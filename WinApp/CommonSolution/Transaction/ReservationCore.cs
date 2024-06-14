/* Project Name - ReservationCoreBL Programs 
* Description  - Data object of the ReservationCore
* 
**************
**Version Log
**************
*Version     Date           Modified By             Remarks          
*********************************************************************************************
*1.00        25-Dec-2016    Rakshith                Created 
*2.50.0      28-Jan-2019    Guru S A                Booking changes     
*2.70        26-Mar-2019    Guru S A                Booking phase 2 changes               
********************************************************************************************/

using System.Collections.Generic;
//using Semnox.Core.HR.Users;
using Semnox.Core.Utilities;
using Semnox.Parafait.User;
using Semnox.Parafait.Booking;
using System;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.Transaction
{
    public class ReservationCore
    {
        ///// <summary>
        ///// GetBoookings
        ///// </summary>
        ///// <param name="reservationParams">reservationParams</param>
        ///// <returns> returns List<Reservation.clsBookingDetails></returns>
        //public List<Reservation.clsBookingDetails> GetBookings(ReservationParams reservationParams)
        //{
        //    ReservationDatahandler reservationCoreDatahandler = new ReservationDatahandler();
        //    return reservationCoreDatahandler.GetBookings(reservationParams);
        //}


        ///// <summary>
        ///// Get Reservation 
        ///// </summary>
        ///// <param name="BookingId">int</param>
        ///// <param name="LoginId">string</param>
        ///// <returns> returns ReservationParams</returns>
        //public ReservationParams GetReservation(int BookingId, string LoginId)
        //{
        //    ReservationDatahandler reservationCoreDatahandler = new ReservationDatahandler();
        //    return reservationCoreDatahandler.GetReservation(BookingId, LoginId);
        //}


        ///// <summary>
        ///// Saves Reservation 
        ///// </summary>
        ///// <param name="reservationParams">reservationParams</param>
        ///// <returns> returns List<KeyValuePair<string,string>></returns>
        //public List<CoreKeyValueStruct> MakeReservation(ReservationParams reservationParams)
        //{
        //    if (reservationParams.CustomerId == -1)
        //    {
        //        throw new Exception("Customer Invalid!");
        //    }

        //    ExecutionContext executionContext = ExecutionContext.GetExecutionContext();
        //    executionContext.SetSiteId(-1);
        //    Semnox.Parafait.Customer.CustomerBL customerBL = new Semnox.Parafait.Customer.CustomerBL(executionContext, reservationParams.CustomerId);
        //    reservationParams.BookingDetailsList[0].CustomerDTO = customerBL.CustomerDTO;
        //    ReservationDatahandler reservationCoreDatahandler = new ReservationDatahandler();
        //    return reservationCoreDatahandler.MakeReservation(reservationParams);
        //}


        ///// <summary>
        ///// Confirm Reservation 
        ///// </summary>
        ///// <param name="reservationParams">reservationParams</param>
        ///// <returns> returns List<KeyValuePair<string,string>></returns>
        //public List<CoreKeyValueStruct> ConfirmReservation(ReservationParams resParams)
        //{
        //    ReservationDatahandler reservationCoreDatahandler = new ReservationDatahandler();
        //    return reservationCoreDatahandler.ConfirmReservation(resParams);
        //}


        
        ///// <summary>
        ///// Edit Reservation 
        ///// </summary>
        ///// <param name="resParams">ReservationParams</param>
        ///// <returns> returns int </returns>
        //public int EditReservation(ReservationParams resParams)
        //{
        //    ReservationDatahandler reservationCoreDatahandler = new ReservationDatahandler();
        //    return reservationCoreDatahandler.EditReservation(resParams);
        //}


        ///// <summary>
        ///// Cancel Reservation 
        ///// </summary>
        ///// <param name="reservationCode">String</param>
        ///// <returns> returns List<CoreKeyValueStruct<string,string>></returns>
        //public List<CoreKeyValueStruct> CancelReservation(string reservationCode)
        //{
        //    ReservationDatahandler reservationCoreDatahandler = new ReservationDatahandler(null);
        //    return reservationCoreDatahandler.CancelReservation(reservationCode);
        //}

        ///// <summary>
        ///// checkIsCorporate
        ///// </summary>
        ///// <returns> list of FacilityDTO</returns>
        //public List<FacilityDTO> GetConfiguredFacility(string macAddress)
        //{
        //    ReservationDatahandler reservationCoreDatahandler = new ReservationDatahandler(null);
        //    return reservationCoreDatahandler.GetConfiguredFacility(macAddress);
        //}
    }

}
