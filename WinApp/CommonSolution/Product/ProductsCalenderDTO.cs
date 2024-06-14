/********************************************************************************************
 * Project Name - ProductCalenderDTO  
 * Description  - Data object of ProductCalenderDTO 
 * 
 **************
 **Version Log
 **************
*Version     Date          Modified By                 Remarks          
 *********************************************************************************************
 *1.00        24-May-2016   Rakshith                    Created 
 *2.70        29-Jan-2019   Jagan Mohana                Added Search Parameter in ProductCalenderDTO,
 *                                                      added new properties guid, synchStatus, masterEntityId properties
 *            26-Apr-2019   Akshay G                    modified showHide dataType(from char to bool)
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;


namespace Semnox.Parafait.Product
{
    /// <summary>
    /// This is the ProductCalenderDTO data object class. This acts as data holder for the ProductCalenderDTO business object
    /// </summary>
    public class ProductsCalenderDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        int productCalendarId;
        int product_Id;
        string day;
        DateTime date;
        double? fromTime;
        double? toTime;
        bool showHide;
        int site_id;
        string guid;
        bool synchStatus;
        int masterEntityId;
        bool isActive;
        bool notifyingObjectIsChanged;
        readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProductsCalenderDTO()
        {
            log.LogMethodEntry();
            productCalendarId = -1;
            product_Id = -1;
            site_id = -1;
            showHide = true;
            isActive = true;
            masterEntityId = -1;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public ProductsCalenderDTO(int ProductCalendarId, int Product_Id, string day, DateTime date, double? FromTime,
                                    double? ToTime, bool showHide, int site_id, string guid, bool synchStatus, int masterEntityId)
        {
            log.LogMethodEntry(ProductCalendarId, Product_Id, day, date, FromTime, ToTime, showHide, site_id, guid, synchStatus, masterEntityId);
            this.productCalendarId = ProductCalendarId;
            this.product_Id = Product_Id;
            this.day = day;
            this.date = date;
            this.fromTime = FromTime;
            this.toTime = ToTime;
            this.showHide = showHide;
            this.site_id = site_id;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.isActive = true; /// This flag is added for the Delete functionality
            log.LogMethodExit();
        }
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by PRODUCT_CALENDER_ID  field
            /// </summary>
            PRODUCT_CALENDER_ID,
            /// <summary>
            /// Search by PRODUCT_ID field
            /// </summary>
            PRODUCT_ID,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by ShowHide field
            /// </summary>
            SHOWHIDE,
            /// <summary>
            /// Search by is active field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by is active field
            /// </summary>
            MASTERENTITY_ID,
            /// <summary>
            /// Search by is PRODUCT_ID_LIST field
            /// </summary>
            PRODUCT_ID_LIST


        }

        /// <summary>
        /// Get/Set method of the ProductCalendarId field
        /// </summary>
        [DisplayName("ProductCalendarId")]
        public int ProductCalendarId { get { return productCalendarId; } set { productCalendarId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Product_Id field
        /// </summary>
        [DisplayName("Product_Id")]
        public int Product_Id { get { return product_Id; } set { product_Id = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Day field
        /// </summary>
        [DisplayName("Day")]
        public string Day { get { return day; } set { day = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Date field
        /// </summary>
        [DisplayName("Date")]
        public DateTime Date { get { return date; } set { date = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the FromTime field
        /// </summary>
        [DisplayName("FromTime")]
        public double? FromTime { get { return fromTime; } set { fromTime = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ToTime field
        /// </summary>
        [DisplayName("ToTime")]
        public double? ToTime { get { return toTime; } set { toTime = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ShowHide field
        /// </summary>
        [DisplayName("ShowHide")]
        public bool ShowHide { get { return showHide; } set { showHide = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Site_id field
        /// </summary>
        [DisplayName("Site_id")]
        public int Site_id { get { return site_id; } set { site_id = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("Master Entity Id")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("IsActive")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

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
                    return notifyingObjectIsChanged || productCalendarId < 0;
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
