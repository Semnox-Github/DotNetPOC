/* Project Name - TransactionReservationScheduleDTO  
* Description  - Data object of the TrxReservationSchedule
* 
**************
**Version Log
**************
*Version     Date           Modified By          Remarks          
********************************************************************************************* 
*2.70        26-Mar-2019    Guru S A             Created for Booking phase 2 enhancement changes 
*2.90        03-Jun-2020    Guru S A             Reservation enhancements for commando release
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    public class TransactionReservationScheduleDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>

        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  trxReservationScheduleId field
            /// </summary>
            TRX_RESERVATION_SCHEDULE_ID,
            /// <summary>
            /// Search by  TRX_ID field
            /// </summary>
            TRX_ID,
            /// <summary>
            /// Search by  LINE_ID field
            /// </summary>
            LINE_ID,
            /// <summary>
            /// Search by  SCHEDULE_ID field
            /// </summary>
            SCHEDULE_ID,
            /// <summary>
            /// Search by  SCHEDULE_FROM_DATE field
            /// </summary>
            SCHEDULE_FROM_DATE,
            /// <summary>
            /// Search by  SCHEDULE_TO_DATE field
            /// </summary>
            SCHEDULE_TO_DATE,
            /// <summary>
            /// Search by  FACILITY_MAP_ID field
            /// </summary>
            FACILITY_MAP_ID,
            /// <summary>
            /// Search by  IS_CANCELLED field
            /// </summary>
            IS_CANCELLED,
            /// <summary>
            /// Search by  SITE_ID field
            /// </summary>             
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by NOT_THIS_BOOKING_ID field
            /// </summary>
            NOT_THIS_BOOKING_ID,
            /// <summary>
            /// Search by NOT_THIS_TRX_ID field
            /// </summary>
            NOT_THIS_TRX_ID,
            /// <summary>
            /// Search by TRX_STATUS_NOT_IN field
            /// </summary>
            TRX_STATUS_NOT_IN
        }
        private int trxReservationScheduleId;
        private int trxId;
        private int lineId;
        private int trxLineProductId;
        private string trxLineProductName;
        private int guestQuantity;
        private int schedulesId;
        private string scheduleName;
        private DateTime scheduleFromDate;
        private DateTime scheduleToDate;
        //private int facilityId;
        //private string facilityName;
        private int facilityMapId;
        private string facilityMapName;
        private bool cancelled;
        private string cancelledBy;
        private string guid;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private bool notifyingObjectIsChanged;
        private DateTime? expiryDate;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// Default constructor
        /// </summary>
        public TransactionReservationScheduleDTO()
        {
            log.LogMethodEntry();
            trxReservationScheduleId = -1;
            trxId = -1;
            lineId = -1;
            schedulesId = -1;
            //facilityId = -1;
            facilityMapId = -1;
            cancelled = false;
            siteId = -1;
            masterEntityId = -1;
            trxLineProductId = -1;
            expiryDate = null;
            log.LogMethodExit();
        }

        public TransactionReservationScheduleDTO(int trxReservationScheduleId, int trxId, int lineId, int guestQuantity, int schedulesId, string scheduleName, DateTime scheduleFromDate, DateTime scheduleToDate, 
                                                 //int facilityId, string facilityName,
                                                       bool cancelled, string cancelledBy, string guid, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, int siteId,
                                                       bool synchStatus, int masterEntityId, int trxLineProductId, string trxLineProductName, int facilityMapId, string facilityMapName, DateTime? expiryDate)
        {
            log.LogMethodEntry(trxReservationScheduleId, trxId, lineId, guestQuantity, schedulesId, scheduleName, scheduleFromDate, scheduleToDate,// facilityId, facilityName, 
                               cancelled, cancelledBy, guid, createdBy, creationDate, lastUpdatedBy, lastUpdateDate, siteId, synchStatus, masterEntityId, trxLineProductId, trxLineProductName,
                                facilityMapId, facilityMapName, expiryDate);
            this.trxReservationScheduleId = trxReservationScheduleId;
            this.trxId = trxId;
            this.lineId = lineId;
            this.trxLineProductId = trxLineProductId;
            this.trxLineProductName = trxLineProductName; 
            this.guestQuantity = guestQuantity;
            this.schedulesId = schedulesId;
            this.scheduleName = scheduleName;
            this.scheduleFromDate = scheduleFromDate;
            this.scheduleToDate = scheduleToDate;
            //this.facilityId = facilityId;
            //this.facilityName = facilityName;
            this.facilityMapId = facilityMapId;
            this.facilityMapName = facilityMapName;
            this.cancelled = cancelled;
            this.cancelledBy = cancelledBy;
            this.guid = guid;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.expiryDate = expiryDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the trxReservationScheduleId field
        /// </summary>
        [DisplayName("Trx Reservation Schedule Id")]
        public int TrxReservationScheduleId { get { return trxReservationScheduleId; } set { trxReservationScheduleId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the TrxId field
        /// </summary>
        [DisplayName("Trx Id")]
        public int TrxId { get { return trxId; } set { trxId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the lineId field
        /// </summary>
        [DisplayName("Line Id")]
        public int LineId { get { return lineId; } set { lineId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the TrxLineProductId field
        /// </summary>
        [DisplayName("Product Id")]
        public int TrxLineProductId { get { return trxLineProductId; } set { trxLineProductId = value;  } }

        /// <summary>
        /// Get method of the trxLineProductName field
        /// </summary>
        [DisplayName("Product Name")]
        public string TrxLineProductName { get { return trxLineProductName; } set { trxLineProductName = value; } }

        /// <summary>
        /// Get/Set method of the GuestQuantity field
        /// </summary>
        [DisplayName("Guest Quantity")]
        public int GuestQuantity { get { return guestQuantity; } set { guestQuantity = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the schedulesId field
        /// </summary>
        [DisplayName("Schedules Id")]
        public int SchedulesId { get { return schedulesId; } set { schedulesId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the scheduleName field
        /// </summary>
        [DisplayName("Schedule Name")]
        public string ScheduleName { get { return scheduleName; } set { scheduleName = value; } }

        /// <summary>
        /// Get method of the scheduleFromDate field
        /// </summary>
        [DisplayName("Schedule From Date")]
        public DateTime ScheduleFromDate { get { return scheduleFromDate; } set { scheduleFromDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the scheduleToDate field
        /// </summary>
        [DisplayName("Schedule To Date")]
        public DateTime ScheduleToDate { get { return scheduleToDate; } set { scheduleToDate = value; this.IsChanged = true; } }

        ///// <summary>
        ///// Get/Set method of the facilityId field
        ///// </summary>
        //[DisplayName("Facility Id")]
        //public int FacilityId { get { return facilityId; } set { facilityId = value; this.IsChanged = true; } }

        ///// <summary>
        ///// Get method of the facilityName field
        ///// </summary>
        //[DisplayName("Facility Name")]
        //public string FacilityName { get { return facilityName; } set { facilityName = value; } }
        /// <summary>
        /// Get/Set method of the FacilityMapId field
        /// </summary>
        [DisplayName("Facility Map Id")]
        public int FacilityMapId { get { return facilityMapId; } set { facilityMapId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the FacilityMapName field
        /// </summary>
        [DisplayName("Facility Map Name")]
        public string FacilityMapName { get { return facilityMapName; } set { facilityMapName = value; } }

        /// <summary>
        /// Get/Set method of the Cancelled field
        /// </summary>
        [DisplayName("Cancelled")]
        public bool Cancelled { get { return cancelled; } set { cancelled = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the cancelledBy field
        /// </summary>
        [DisplayName("cancelled By")]
        public string CancelledBy { get { return cancelledBy; } set { cancelledBy = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SeatId field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        public int SiteId { get { return siteId; } set { siteId = value; this.IsChanged = true; } } 

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")] 
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [DisplayName("Created Date")] 
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Last Updated By")]
        [Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("Last Update Date")] 
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

        /// <summary>
        /// Get method of the expiryDate field
        /// </summary>
        [DisplayName("Expiry Date")]
        public DateTime? ExpiryDate { get { return expiryDate; } set { expiryDate = value; this.IsChanged = true; } }
        /// <summary>
        /// Set and get for Ischanged 
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || trxReservationScheduleId < 0;
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
        /// Allowes to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }
}
