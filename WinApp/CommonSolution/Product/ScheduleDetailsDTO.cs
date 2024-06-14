/* Project Name - Semnox.Parafait.Product.ScheduleSummaryDTO 
* Description  - DTO to provide consolidated view of the master schedule, schedule, facility and product details
* 
**************
**Version Log
**************
*Version     Date           Modified By          Remarks          
********************************************************************************************* 
*2.70        18-Mar-2019    Guru S A             Created Booking phase 2 enhancement changes 
*2.100       24-Sep-2020    Nitin Pai            Attraction Reschedule: Added Day Attraction Schedule Id to list
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{

    /// <summary>
    /// Class ScheduleSummaryDTO to provide consolidated view of the master schedule, schedule, facility and product details
    /// </summary>
   public class ScheduleDetailsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields
        /// </summary>

        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  FACILITY_MAP_ID field
            /// </summary>
            FACILITY_MAP_ID,
            /// <summary>
            /// Search by  ScheduleId field
            /// </summary>
            SCHEDULE_ID ,
            /// <summary>
            /// Search by  MasterScheduleId field
            /// </summary>
            MASTER_SCHEDULE_ID,
            /// <summary>
            /// Search by  productId field
            /// </summary>
            PRODUCT_ID ,
            /// <summary>
            /// Search by  attractionPlayId field
            /// </summary>
            ATTRACTION_PLAY_ID , 
            /// <summary>
            /// Search by  FixedSchedule field
            /// </summary>
            FIXED_SCHEDULE,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID 
        }

        //private int facilityId;
        //private string facilityName;
        private int facilityMapId;
        private string facilityMapName;
        private int masterScheduleId;
        private string masterScheduleName;
        private int scheduleId;
        private string scheduleName;
        private DateTime scheduleFromDate;
        private DateTime scheduleToDate;
        private bool fixedSchedule;
        private decimal scheduleFromTime;
        private decimal scheduleToTime;
        private int attractionPlayId;
        private string attractionPlayName;
        private int productId;
        private string productName;
        private double? price;
        private double? attractionPlayPrice;
        private int? facilityCapacity;
        private int? ruleUnits;
       //private int? productLevelUnits;
        private int? totalUnits;
        private int? bookedUnits;
        private int? availableUnits;
        private int? desiredUnits;
        private DateTime? expiryDate;
        private int categoryId;
        private int promotionId;
        private int? seats;
        private int siteId;
        //private FacilityDTO facilityDTO;
        private FacilityMapDTO facilityMapDTO;
        private int dayAttractionScheduleId;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public ScheduleDetailsDTO()
        {
            log.LogMethodEntry();
            //facilityId = -1;
            facilityMapId = -1;
            masterScheduleId = -1;
            scheduleId = -1;
            attractionPlayId = -1;
            productId = -1;
            categoryId = -1;
            promotionId = -1;
            siteId = -1;
            dayAttractionScheduleId = -1;
            log.LogMethodExit();
        }


        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public ScheduleDetailsDTO(int facilityMapId, string facilityMapName, int masterScheduleId, string masterScheduleName, int scheduleId, string scheduleName,
            DateTime scheduleFromDate, DateTime scheduleToDate, decimal scheduleFromTime, decimal scheduleToTime, bool fixedSchedule, int attractionPlayId, string attractionPlayName, int productId, string productName, 
            double? price, int? facilityCapacity, int? ruleUnits, 
            //int? productLevelUnits, 
            int? totalUnits, int? bookedUnits, int? availableUnits, int? desiredUnits, DateTime? expiryDate, int categoryId,
            int promotionId, int? seats, int siteId, double? attractionPlayPrice, FacilityMapDTO facilityMapDTO)
        {
            log.LogMethodEntry(facilityMapId, facilityMapName, masterScheduleId, masterScheduleName, scheduleId, scheduleName, scheduleFromDate, scheduleToDate, scheduleFromTime, scheduleToTime,
                 attractionPlayId, attractionPlayName, facilityCapacity, productId, productName, price, ruleUnits, bookedUnits, availableUnits, desiredUnits, expiryDate,
                 categoryId, promotionId, seats, siteId, attractionPlayPrice, facilityMapDTO);//facilityDTO, productLevelUnits);
            //this.facilityId = facilityId;
            //this.facilityName = facilityName;
            this.facilityMapId = facilityMapId;
            this.facilityMapName = facilityMapName;
            this.masterScheduleId = masterScheduleId;
            this.masterScheduleName = masterScheduleName;
            this.scheduleId = scheduleId;
            this.scheduleName = scheduleName;
            this.scheduleFromDate = scheduleFromDate;
            this.scheduleToDate = scheduleToDate;
            this.scheduleFromTime = scheduleFromTime;
            this.scheduleToTime = scheduleToTime;
            this.fixedSchedule = fixedSchedule;
            this.attractionPlayId = attractionPlayId;
            this.attractionPlayName = attractionPlayName;
            this.productId = productId;
            this.productName = productName;
            this.price = price;
            this.facilityCapacity = facilityCapacity;
            //this.productLevelUnits = productLevelUnits;
            this.ruleUnits = ruleUnits;
            this.totalUnits = totalUnits;
            this.bookedUnits = bookedUnits;
            this.availableUnits = availableUnits;
            this.desiredUnits = desiredUnits;
            this.expiryDate = expiryDate;
            this.categoryId = categoryId;
            this.promotionId = promotionId;
            this.seats = seats;
            this.siteId = siteId;
            this.attractionPlayPrice = attractionPlayPrice;
            this.facilityMapDTO = facilityMapDTO;
            this.dayAttractionScheduleId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the FacilityMapId field
        /// </summary>
        [DisplayName("Facility Map Id")]
        public int FacilityMapId { get { return facilityMapId; } set { facilityMapId = value;  } }

        /// <summary>
        /// Get/Set method of the FacilityMapName field
        /// </summary>
        [DisplayName("Facility Map Name")]
        public string FacilityMapName { get { return facilityMapName; } set { facilityMapName = value; } }
        /// <summary>
        /// Get/Set method of the MasterScheduleId field
        /// </summary>
        [DisplayName("Master Schedule Id")]
        public int MasterScheduleId { get { return masterScheduleId; } set { masterScheduleId = value; } }

        /// <summary>
        /// Get/Set method of the masterScheduleName field
        /// </summary>
        [DisplayName("Master Schedule Name")]
        public string MasterScheduleName { get { return masterScheduleName; } set { masterScheduleName = value; } }
       
        /// <summary>
        /// Get/Set method of the ScheduleId field
        /// </summary>
        [DisplayName("Schedule Id")]
        public int ScheduleId { get { return scheduleId; } set { scheduleId = value; } }

        /// <summary>
        /// Get/Set method of the ScheduleName field
        /// </summary>
        [DisplayName("Schedule Name")]
        public string ScheduleName { get { return scheduleName; } set { scheduleName = value; } }

        /// <summary>
        /// Get/Set method of the ScheduleFromDate field
        /// </summary>
        [DisplayName("Schedule From Date")]
        public DateTime ScheduleFromDate { get { return scheduleFromDate; } set { scheduleFromDate = value; } }
        /// <summary>
        /// Get/Set method of the scheduleToDate field
        /// </summary>
        [DisplayName("Schedule To Date")]
        public DateTime ScheduleToDate { get { return scheduleToDate; } set { scheduleToDate = value; } }
        /// <summary>
        /// Get/Set method of the ScheduleFromTime field
        /// </summary>
        [DisplayName("ScheduleFromTime")]
        public decimal ScheduleFromTime
        {
            get { return scheduleFromTime; }
            set
            {
                if (value <= 99999999) //for 8 
                {
                    scheduleFromTime = Math.Round(value, 2);
                    this.IsChanged = true;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        /// <summary>
        /// Get/Set method of the ScheduleToTime field
        /// </summary>
        [DisplayName("ScheduleToTime")]
        public decimal ScheduleToTime
        {
            get { return scheduleToTime; }
            set
            {
                if (value <= 99999999) //for 8 
                {
                    scheduleToTime = Math.Round(value, 2);
                    this.IsChanged = true;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        /// <summary>
        /// Get/Set method of the fixedSchedule field
        /// </summary>
        [DisplayName("Fixed Schedule")]
        public bool FixedSchedule { get { return fixedSchedule; } set { fixedSchedule = value; } }

        /// <summary>
        /// Get/Set method of the AttractionPlayId field
        /// </summary>
        [DisplayName("Attraction Play Id")]
        public int AttractionPlayId { get { return attractionPlayId; } set { attractionPlayId = value; } }

        /// <summary>
        /// Get/Set method of the attractionPlayName field
        /// </summary>
        [DisplayName("Play Name")]
        public string AttractionPlayName { get { return attractionPlayName; } set { attractionPlayName = value; } }

        /// <summary>
        /// Get/Set method of the AttractionPlayPrice field
        /// </summary>
        [DisplayName("AttractionPlayPrice")]
        public double? AttractionPlayPrice { get { return attractionPlayPrice; } set { attractionPlayPrice = value; } }
        

        /// <summary>
        /// Get/Set method of the productId field
        /// </summary>
        [DisplayName("Product Id")]
        public int ProductId { get { return productId; } set { productId = value; } }

        /// <summary>
        /// Get/Set method of the productName field
        /// </summary>
        [DisplayName("Product Name")]
        public string ProductName { get { return productName; } set { productName = value; } }
        /// <summary>
        /// Get/Set method of the Price field
        /// </summary>
        [DisplayName("Price")]
        public double? Price { get { return price; } set { price = value; } }
        /// <summary>
        /// Get/Set method of the facilityCapacity field
        /// </summary>
        [DisplayName("Facility Capacity")]
        public int? FacilityCapacity { get { return facilityCapacity; } set { facilityCapacity = value; } }

        ///// <summary>
        ///// Get/Set method of the productLevelUnits field
        ///// </summary>
        //[DisplayName("Product Level Units")]
        //public int? ProductLevelUnits { get { return productLevelUnits; } set { productLevelUnits = value; } }
        
        /// <summary>
        /// Get/Set method of the RuleUnits field
        /// </summary>
        [DisplayName("Rule Units")]
        public int? RuleUnits { get { return ruleUnits; } set { ruleUnits = value; } }

        /// <summary>
        /// Get/Set method of the TotalUnits field
        /// </summary>
        [DisplayName("Total Units")]
        public int? TotalUnits { get { return totalUnits; } set { totalUnits = value; } }

        /// <summary>
        /// Get/Set method of the BookedUnits field
        /// </summary>
        [DisplayName("Booked Units")]
        public int? BookedUnits { get { return bookedUnits; } set { bookedUnits = value; } }

        /// <summary>
        /// Get/Set method of the AvailableUnits field
        /// </summary>
        [DisplayName("Available Units")]
        public int? AvailableUnits { get { return availableUnits; } set { availableUnits = value; } }
        /// <summary>
        /// Get/Set method of the DesiredUnits field
        /// </summary>
        [DisplayName("Desired Units")]
        public int? DesiredUnits { get { return desiredUnits; } set { desiredUnits = value; } }

        /// <summary>
        /// Get/Set method of the ExpiryDate field
        /// </summary>
        public DateTime? ExpiryDate { get { return expiryDate; } set { expiryDate = value; } }

        /// <summary>
        /// Get/Set method of the categoryId field
        /// </summary>
        [DisplayName("Category Id")]
        public int CategoryId { get { return categoryId; } set { categoryId = value; } }

        /// <summary>
        /// Get/Set method of the PromotionId field
        /// </summary>
        [DisplayName("Promotion Id")]
        public int PromotionId { get { return promotionId; } set { promotionId = value; } }

        /// <summary>
        /// Get/Set method of the Seats field
        /// </summary>
        [DisplayName("Seats")]
        public int? Seats { get { return seats; } set { seats = value; } }


        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the FacilityMapDTO field
        /// </summary>
        [DisplayName("FacilityMapDTO")]
        public FacilityMapDTO FacilityMapDTO { get { return facilityMapDTO; } set { facilityMapDTO = value; } }

        /// <summary>
        /// Get/Set method of the PromotionId field
        /// </summary>
        [DisplayName("DayAttractionScheduleId")]
        public int DayAttractionScheduleId { get { return dayAttractionScheduleId; } set { dayAttractionScheduleId = value; } }

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
                    return notifyingObjectIsChanged || facilityMapId < 0;
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
            log.LogMethodExit(null);
        }
    }
}
