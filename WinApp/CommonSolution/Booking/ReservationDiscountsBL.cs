/********************************************************************************************
 * Project Name - ReservationDiscounts BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        31-Aug-2017      Lakshminarayana     Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Booking
{
    /// <summary>
    /// Business logic for ReservationDiscounts class.
    /// </summary>
    public class ReservationDiscountsBL
    {
        ReservationDiscountsDTO reservationDiscountsDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor of ReservationDiscountsBL class
        /// </summary>
        public ReservationDiscountsBL()
        {
            log.Debug("Starts-ReservationDiscountsBL() default constructor.");
            reservationDiscountsDTO = null;
            log.Debug("Ends-ReservationDiscountsBL() default constructor.");
        }

        /// <summary>
        /// Constructor with the reservationDiscounts id as the parameter
        /// Would fetch the reservationDiscounts object from the database based on the id passed. 
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">SQL Transaction</param>
        public ReservationDiscountsBL(int id, SqlTransaction sqlTransaction = null)
            : this()
        {
            log.Debug("Starts-ReservationDiscountsBL(id) parameterized constructor.");
            ReservationDiscountsDataHandler reservationDiscountsDataHandler = new ReservationDiscountsDataHandler(sqlTransaction);
            reservationDiscountsDTO = reservationDiscountsDataHandler.GetReservationDiscountsDTO(id);
            log.Debug("Ends-ReservationDiscountsBL(id) parameterized constructor.");
        }

        /// <summary>
        /// Creates ReservationDiscountsBL object using the ReservationDiscountsDTO
        /// </summary>
        /// <param name="reservationDiscountsDTO">ReservationDiscountsDTO object</param>
        public ReservationDiscountsBL(ReservationDiscountsDTO reservationDiscountsDTO)
            : this()
        {
            log.Debug("Starts-ReservationDiscountsBL(reservationDiscountsDTO) Parameterized constructor.");
            this.reservationDiscountsDTO = reservationDiscountsDTO;
            log.Debug("Ends-ReservationDiscountsBL(reservationDiscountsDTO) Parameterized constructor.");
        }

        /// <summary>
        /// Saves the ReservationDiscounts
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.Debug("Starts-Save() method.");
            ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
            ReservationDiscountsDataHandler reservationDiscountsDataHandler = new ReservationDiscountsDataHandler(sqlTransaction);
            if (reservationDiscountsDTO.Id < 0)
            {
                int id = reservationDiscountsDataHandler.InsertReservationDiscounts(reservationDiscountsDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                reservationDiscountsDTO.Id = id;
                reservationDiscountsDTO.AcceptChanges();
            }
            else
            {
                if (reservationDiscountsDTO.IsChanged)
                {
                    reservationDiscountsDataHandler.UpdateReservationDiscounts(reservationDiscountsDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                    reservationDiscountsDTO.AcceptChanges();
                }
            }
            log.Debug("Ends-Save() method.");
        }

        /// <summary>
        /// Deletes the ReservationDiscounts
        /// Checks if the id is not equal to -1
        /// If it is not equal to -1, then deletes
        /// else ignores
        /// </summary>
        public int Delete(SqlTransaction sqlTransaction = null)
        {
            log.Debug("Starts-Remove() method.");
            int noOfRowsDeleted = 0;
            ReservationDiscountsDataHandler reservationDiscountsDataHandler = new ReservationDiscountsDataHandler(sqlTransaction);
            if (reservationDiscountsDTO.Id != -1)
            {
                noOfRowsDeleted = reservationDiscountsDataHandler.Delete(reservationDiscountsDTO.Id);
            }
            log.Debug("Ends-Remove() method.");
            return noOfRowsDeleted;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ReservationDiscountsDTO ReservationDiscountsDTO
        {
            get
            {
                return reservationDiscountsDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of ReservationDiscounts
    /// </summary>
    public class ReservationDiscountsListBL
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Returns the ReservationDiscounts list
        /// </summary>
        public List<ReservationDiscountsDTO> GetReservationDiscountsDTOList(List<KeyValuePair<ReservationDiscountsDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.Debug("Starts-GetReservationDiscountsDTOList(searchParameters) method");
            ReservationDiscountsDataHandler reservationDiscountsDataHandler = new ReservationDiscountsDataHandler(sqlTransaction);
            log.Debug("Ends-GetReservationDiscountsDTOList(searchParameters) method by returning the result of reservationDiscountsDataHandler.GetReservationDiscountsDTOList(searchParameters) call");
            return reservationDiscountsDataHandler.GetReservationDiscountsDTOList(searchParameters);
        }

    }
}
