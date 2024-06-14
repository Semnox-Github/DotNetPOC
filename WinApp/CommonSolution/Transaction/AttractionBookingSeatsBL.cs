/* Project Name - Semnox.Parafait.Product.AttractionBookingSeatsBL 
* Description  - BL class of AttractionBookingSeats
* 
**************
**Version Log
**************
*Version     Date           Modified By          Remarks          
********************************************************************************************* 
*2.60.0      14-Mar-2019    Guru S A             Created for Booking phase 2 enhancement changes 
*2.71        24-Jul-2019    Nitin Pai            Attraction enhancement for combo products
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// BL class for AttractionBooking
    /// </summary>
    public class AttractionBookingSeatsBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private AttractionBookingSeatsDTO attractionBookingSeatsDTO;
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor of AttractionBookingSeatsBL class
        /// </summary>
        public AttractionBookingSeatsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            attractionBookingSeatsDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the AttractionBookingSeats id as the parameter
        /// Would fetch the AttractionBookingSeats object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">optional sql transaction</param>
        public AttractionBookingSeatsBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            AttractionBookingSeatsDatahandler attractionBookingSeatsDatahandler = new AttractionBookingSeatsDatahandler(sqlTransaction);
            attractionBookingSeatsDTO = attractionBookingSeatsDatahandler.GetAttractionBookingSeatsDTO(id);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates AttractionBookingSeatsBL object using the AttractionBookingSeatsDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="attractionBookingSeatsDTO">AttractionBookingSeatsDTO object</param>
        public AttractionBookingSeatsBL(ExecutionContext executionContext, AttractionBookingSeatsDTO attractionBookingSeatsDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, attractionBookingSeatsDTO);
            this.attractionBookingSeatsDTO = attractionBookingSeatsDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the AttractionBookingSeats
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            AttractionBookingSeatsDatahandler attractionBookingSeatsDatahandler = new AttractionBookingSeatsDatahandler(sqlTransaction);
            if (attractionBookingSeatsDTO.BookingSeatId < 0)
            {
                int id = attractionBookingSeatsDatahandler.InsertAttractionBooking(attractionBookingSeatsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                attractionBookingSeatsDTO.BookingSeatId = id;
                attractionBookingSeatsDTO.AcceptChanges();
            }
            else
            {
                if (attractionBookingSeatsDTO.IsChanged)
                {
                    attractionBookingSeatsDatahandler.UpdateAttractionBooking(attractionBookingSeatsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    attractionBookingSeatsDTO.AcceptChanges();
                }
                else
                {
                    attractionBookingSeatsDatahandler.HardDeleteBookingSeatEntry(attractionBookingSeatsDTO.BookingSeatId);
                    attractionBookingSeatsDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public AttractionBookingSeatsDTO AttractionBookingSeatsDTO
        {
            get
            {
                return attractionBookingSeatsDTO;
            }
        }


        public void CloneObject(AttractionBookingSeatsDTO attractionBookingSeatsDTOOriginal)
        {
            log.LogMethodEntry(attractionBookingSeatsDTOOriginal);
            if (this.attractionBookingSeatsDTO == null)
            {
                this.attractionBookingSeatsDTO = new AttractionBookingSeatsDTO();
            } 
            this.attractionBookingSeatsDTO.SeatId = attractionBookingSeatsDTOOriginal.SeatId;
            this.attractionBookingSeatsDTO.SeatName = attractionBookingSeatsDTOOriginal.SeatName;
            this.attractionBookingSeatsDTO.CardId = attractionBookingSeatsDTOOriginal.CardId;              
            log.LogMethodExit();
        }

        internal void Expire(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            if (attractionBookingSeatsDTO != null)
            {
                AttractionBookingSeatsDatahandler attractionBookingSeatsDatahandler = new AttractionBookingSeatsDatahandler(sqlTransaction);
                attractionBookingSeatsDatahandler.HardDeleteBookingSeatEntry(attractionBookingSeatsDTO.BookingSeatId); 
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of AttractionBookingSeats
    /// </summary>
    public class AttractionBookingSeatsListBL
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public AttractionBookingSeatsListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// GetAttractionBookingSeatsDTOList
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns>List<AttractionBookingSeatsDTO></returns>
        public List<AttractionBookingSeatsDTO> GetAttractionBookingSeatsDTOList(List<KeyValuePair<AttractionBookingSeatsDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            AttractionBookingSeatsDatahandler attractionBookingSeatsDatahandler = new AttractionBookingSeatsDatahandler(sqlTransaction);
            List<AttractionBookingSeatsDTO> returnValue = attractionBookingSeatsDatahandler.GetAttractionBookingSeatsDTOList(searchParameters);
            log.LogMethodExit(returnValue);
            return returnValue;
        }
    }
}
