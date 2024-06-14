/********************************************************************************************
 * Project Name - Products
 * Description  - Data object of FacilitySeatsContainer
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.130.00   16-Aug-2021    Prajwal S          Created                                                       
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Product
{
    public class FacilitySeatsContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int seatId;
        private string seatName;
        private int rowIndex;
        private int columnIndex;
        private int facilityId;
        private char isAccessible;
        private string guid;
        private int bookedSeat;


        /// <summary>
        /// Default constructor
        /// </summary>
        public FacilitySeatsContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();

        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public FacilitySeatsContainerDTO(int seatId, string seatName, int rowIndex, int columnIndex,
                                int facilityId, char isAccessible, int bookedSeat, string guid)
            : this()
        {
            log.LogMethodEntry(seatId, seatName, rowIndex, columnIndex, guid, facilityId, isAccessible);
            this.seatId = seatId;
            this.seatName = seatName;
            this.rowIndex = rowIndex;
            this.columnIndex = columnIndex;
            this.guid = guid;
            this.facilityId = facilityId;
            this.isAccessible = isAccessible;
            this.bookedSeat = bookedSeat;
            log.LogMethodExit();
        }


        /// <summary>
        /// Get/Set method of the SeatId field
        /// </summary>
        public int SeatId { get { return seatId; } set { seatId = value; } }

        /// <summary>
        /// Get/Set method of the SeatName field
        /// </summary>
        public string SeatName { get { return seatName; } set { seatName = value; } }

        /// <summary>
        /// Get/Set method of the RowIndex field
        /// </summary>
        public int RowIndex { get { return rowIndex; } set { rowIndex = value; } }


        /// <summary>
        /// Get/Set method of the ColumnIndex field
        /// </summary>
        public int ColumnIndex { get { return columnIndex; } set { columnIndex = value; } }

        /// <summary>
        /// Get/Set method of the FacilityId field
        /// </summary>
        public int FacilityId { get { return facilityId; } set { facilityId = value; } }

        /// <summary>
        /// Get/Set method of the IsAccessible field
        /// </summary>
        public char IsAccessible { get { return isAccessible; } set { isAccessible = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; } }

        /// <summary>
        /// Get/Set method of the BookedSeat field
        /// </summary>
        public int BookedSeat { get { return bookedSeat; } set { bookedSeat = value; } }

    }
}
