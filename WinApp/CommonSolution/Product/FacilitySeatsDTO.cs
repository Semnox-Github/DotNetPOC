/* Project Name - ReservationCoreDTO Programs 
* Description  - Data object of the Facility SeatsDTO
* 
**************
**Version Log
**************
*Version     Date            Modified By             Remarks          
*********************************************************************************************
*1.00        24-Dec-2016     Rakshith                Created 
********************************************************************************************
*2.60        26-Feb-2019     Akshay Gulaganji        Added SearchByFacilitySeatLayoutParameter and IsChanged property field
*2.80.0      28-Feb-2020     Girish Kundar           Modified : 3 Tier changes for API
*********************************************************************************************/

using System;
using System.ComponentModel;

namespace Semnox.Parafait.Product
{
    public class FacilitySeatsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// SearchByFacilitySeatsParameter enum controls the search fields, this can be expanded to include additional fields
        /// </summary>   
        public enum SearchByFacilitySeatsParameter
        {
            /// <summary>
            /// Search by SEAT_NAME field
            /// </summary>
            SEAT_NAME,
            /// <summary>
            /// Search by FACILITY_ID field
            /// </summary>
            FACILITY_ID,
            /// <summary>
            /// Search by FACILITY_ID field
            /// </summary>
            FACILITY_ID_LIST,
            /// <summary>
            /// Search by IS_ACCESSIBLE field
            /// </summary>
            IS_ACCESSIBLE,
            /// <summary>
            /// Search by ACTIVE field
            /// </summary>
            ACTIVE,
            /// <summary>
            /// Search by SEAT_ID field
            /// </summary>
            SEAT_ID,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID
        }
        private int seatId;
        private string seatName;
        private int rowIndex;
        private int columnIndex;
        private char active;
        private int facilityId;
        private char isAccessible;
        private string guid;
        private bool synchStatus;
        private int site_id;
        private int masterEntityId;
        private int bookedSeat;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        /// <summary>
        /// Default constructor
        /// </summary>
        public FacilitySeatsDTO()
        {
            this.seatId = -1;
            facilityId = -1;
            this.seatName = "";
            this.site_id = -1;
            bookedSeat = -1;
            masterEntityId = -1;
            active = 'Y';
            rowIndex = -1;
            columnIndex = -1;
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public FacilitySeatsDTO(int seatId, string seatName, int rowIndex, int columnIndex, char active,
                                int facilityId, char isAccessible)
            :this()
        {
            log.LogMethodEntry(seatId, seatName, rowIndex, columnIndex, active, facilityId,isAccessible);
            this.seatId = seatId;
            this.seatName = seatName;
            this.rowIndex = rowIndex;
            this.columnIndex = columnIndex;
            this.active = active;
            this.facilityId = facilityId;
            this.isAccessible = isAccessible;
            this.bookedSeat = -1;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public FacilitySeatsDTO(int seatId, string seatName, int rowIndex, int columnIndex, char active,
                                int facilityId,char isAccessible, string guid, bool synchStatus,
                                 int site_id, int masterEntityId, string createdBy, DateTime creationDate,
                                 string lastUpdatedBy, DateTime lastUpdateDate)
            :this(seatId, seatName, rowIndex, columnIndex, active, facilityId, isAccessible)
        {
            log.LogMethodEntry(seatId, seatName, rowIndex, columnIndex, active, facilityId,
                isAccessible, guid, synchStatus, site_id, masterEntityId,
                createdBy, creationDate, lastUpdatedBy, lastUpdateDate);
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.site_id = site_id;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.bookedSeat = -1;
            log.LogMethodExit();
        }



        /// <summary>
        /// Get/Set method of the SeatId field
        /// </summary>
        [DisplayName("SeatId")]
        [DefaultValue(-1)]
        public int SeatId { get { return seatId; } set { IsChanged = true; seatId = value; } }

        /// <summary>
        /// Get/Set method of the SeatName field
        /// </summary>
        [DisplayName("SeatName")]
        [DefaultValue("")]
        public string SeatName { get { return seatName; } set { IsChanged = true; seatName = value; } }

        /// <summary>
        /// Get/Set method of the RowIndex field
        /// </summary>
        [DisplayName("RowIndex")]
        [DefaultValue(-1)]
        public int RowIndex { get { return rowIndex; } set { IsChanged = true; rowIndex = value; } }


        /// <summary>
        /// Get/Set method of the ColumnIndex field
        /// </summary>
        [DisplayName("ColumnIndex")]
        [DefaultValue(-1)]
        public int ColumnIndex { get { return columnIndex; } set { IsChanged = true; columnIndex = value; } }

        /// <summary>
        /// Get/Set method of the Active field
        /// </summary>
        [DisplayName("Active")]
        public char Active { get { return active; } set { IsChanged = true; active = value; } }

        /// <summary>
        /// Get/Set method of the FacilityId field
        /// </summary>
        [DisplayName("FacilityId")]
        [DefaultValue(-1)]
        public int FacilityId { get { return facilityId; } set { IsChanged = true; facilityId = value; } }

        /// <summary>
        /// Get/Set method of the IsAccessible field
        /// </summary>
        [DisplayName("IsAccessible")]
        public char IsAccessible { get { return isAccessible; } set { IsChanged = true; isAccessible = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { IsChanged = true; guid = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        public bool SynchStatus { get { return synchStatus; } set { IsChanged = true; synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the Site_id field
        /// </summary>
        [DisplayName("Site_id")]
        [DefaultValue(-1)]
        public int SiteId { get { return site_id; } set { IsChanged = true; site_id = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        [DefaultValue(-1)]
        public int MasterEntityId { get { return masterEntityId; } set { IsChanged = true; masterEntityId = value; } }


        /// <summary>
        /// Get/Set method of the BookedSeat field
        /// </summary>
        [DisplayName("BookedSeat")]
        [DefaultValue(-1)]
        public int BookedSeat { get { return bookedSeat; } set { IsChanged = true; bookedSeat = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        public DateTime LastUpdatedDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedUser field
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }
        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
        [Browsable(false)]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }
        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }
        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || seatId < 0;
                }
            }

            set
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    if (!Boolean.Equals(notifyingObjectIsChanged, value))
                    {
                        notifyingObjectIsChanged = value;
                    }
                }
            }
        }
        /// <summary>
        /// Allows to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }
}
