/* Project Name - Semnox.Parafait.Booking.ScheduleRulesDTO 
* Description  - Data object of the AttractionScheduleRules
* 
**************
**Version Log
**************
*Version     Date           Modified By             Remarks          
********************************************************************************************* 
*2.50        26-Nov-2018    Guru S A             Created for Booking enhancement changes 
********************************************************************************************/

using System;
using System.ComponentModel;


namespace Semnox.Parafait.Booking
{

    /// <summary>
    /// ScheduleRulesDTO Class
    /// </summary>
    public class ScheduleRulesDTO
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>

        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  id field
            /// </summary>
            SCHEDULE_RULE_ID = 0,
            /// <summary>
            /// Search by  ScheduleId field
            /// </summary>
            SCHEDULE_ID = 1,
            /// <summary>
            /// Search by  ProductId field
            /// </summary>
            PRODUCT_ID = 2,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID = 3,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID = 4
        }

        private int scheduleRulesId;
        private int scheduleId;
        private decimal? day;
        private DateTime? fromDate;
        private DateTime? toDate;
        private int? units;
        private decimal? price;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int productId;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;


        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public ScheduleRulesDTO()
        {
            log.LogMethodEntry();
            scheduleRulesId = -1;
            scheduleId = -1;
            siteId = -1;
            productId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }



        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ScheduleRulesDTO(int scheduleRulesId, int scheduleId, decimal? day, DateTime? fromDate, DateTime? toDate, int? units, decimal? price, 
                                          int siteId, string guid, bool synchStatus, int productId, int masterEntityId, string createdBy, DateTime creationDate, 
                                          string lastUpdatedBy, DateTime lastUpdateDate) 
        {
            log.LogMethodEntry(scheduleRulesId, scheduleId, day, fromDate, toDate, units, price,
                               siteId, guid, synchStatus, productId, masterEntityId, createdBy, creationDate, lastUpdatedBy, lastUpdateDate);
            this.scheduleRulesId = scheduleRulesId;
            this.scheduleId = scheduleId;
            this.day = day;
            this.fromDate = fromDate;
            this.toDate = toDate;
            this.units = units;
            this.price = price;
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.productId = productId;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the ScheduleRulesId field
        /// </summary>
        [DisplayName("Schedule RulesId")] 
        public int ScheduleRulesId { get { return scheduleRulesId; } set { scheduleRulesId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ScheduleId field
        /// </summary>
        [DisplayName("ScheduleId")] 
        public int ScheduleId { get { return scheduleId; } set { scheduleId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the day field
        /// </summary>
        public decimal? Day { get { return day; }
            set {
                if (value <= 999999999999999999) //for 18 
                {
                    day = value;
                    this.IsChanged = true;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
         }

        /// <summary>
        /// Get method of the FromDate field
        /// </summary>
        [DisplayName("From Date")] 
        public DateTime? FromDate
        {
            get
            {
                return fromDate;
            }
            set { fromDate = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get method of the ToDate field
        /// </summary>
        [DisplayName("To Date")]
        public DateTime? ToDate
        {
            get
            {
                return toDate;
            }
            set { toDate = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the Units field
        /// </summary>
        [DisplayName("Units")] 
        public int? Units { get { return units; } set { units = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Price field
        /// </summary>
        public decimal Price
        {
            get { return Price; }
            set
            {
                if (value <= 999999999999999999) //for 18 
                {
                    Price = value;
                    this.IsChanged = true;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>
        [DisplayName("Product Id")]
        public int ProductId { get { return productId; } set { productId = value; this.IsChanged = true; } }
         
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("Site Id")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("Master Entity Id")]
        public int MasterEntityId { get { return masterEntityId; } }

        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        [Browsable(false)]
        public string CreatedBy
        {
            get
            {
                return createdBy;
            }
        }

        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [DisplayName("Created Date")]
        [Browsable(false)]
        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }
        }

        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Last Updated By")]
        [Browsable(false)]
        public string LastUpdatedBy
        {
            get
            {
                return lastUpdatedBy;
            }
        }

        /// <summary>
        /// Get method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("Last Update Date")]
        [Browsable(false)]
        public DateTime LastUpdateDate
        {
            get
            {
                return lastUpdateDate;
            }
        }

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
                    return notifyingObjectIsChanged;
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
