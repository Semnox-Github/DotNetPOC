/********************************************************************************************
 * Project Name - ReservationDiscounts DTO
 * Description  - Data object of ReservationDiscounts
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *1.00        31-Aug-2017   Lakshminarayana          Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Booking
{
    /// <summary>
    /// This is the ReservationDiscounts data object class. This acts as data holder for the ReservationDiscounts business object
    /// </summary>
    public class ReservationDiscountsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by BookingId field
            /// </summary>
            BOOKING_ID,
            /// <summary>
            /// Search by ReservationDiscountId field
            /// </summary>
            RESERVATION_DISCOUNT_ID,
            /// <summary>
            /// Search by ProductId field
            /// </summary>
            PRODUCT_ID,
            /// <summary>
            /// Search by ReservationDiscountCategory field
            /// </summary>
            RESERVATION_DISCOUNT_CATEGORY,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MasterEntityId field
            /// </summary>
            MASTER_ENTITY_ID
        }

        int id;
        int bookingId;
        int reservationDiscountId;
        int productId;
        double? reservationDiscountPecentage;
        string reservationDiscountCategory;

        int siteId;
        int masterEntityId;
        bool synchStatus;
        string guid;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public ReservationDiscountsDTO()
        {
            log.Debug("Starts-ReservationDiscountsDTO() default constructor.");
            id = -1;
            bookingId = -1;
            reservationDiscountId = -1;
            productId = -1;

            masterEntityId = -1;
            log.Debug("Ends-ReservationDiscountsDTO() default constructor.");
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ReservationDiscountsDTO(int id, int bookingId, int reservationDiscountId, int productId, 
                                       double? reservationDiscountPecentage,
                                       string reservationDiscountCategory, int siteId,
                                       int masterEntityId, bool synchStatus, string guid)
        {
            log.Debug("Starts-ReservationDiscountsDTO(with all the data fields) Parameterized constructor.");
            this.id = id;
            this.bookingId = bookingId;
            this.reservationDiscountId = reservationDiscountId;
            this.productId = productId;
            this.reservationDiscountPecentage = reservationDiscountPecentage;
            this.reservationDiscountCategory = reservationDiscountCategory;

            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            log.Debug("Starts-ReservationDiscountsDTO(with all the data fields) Parameterized constructor.");
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [Browsable(false)]
        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                this.IsChanged = true;
                id = value;
            }
        }

        /// <summary>
        /// Get/Set method of the BookingId field
        /// </summary>
        [Browsable(false)]
        public int BookingId
        {
            get
            {
                return bookingId;
            }

            set
            {
                this.IsChanged = true;
                bookingId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ReservationDiscountId field
        /// </summary>
        [Browsable(false)]
        public int ReservationDiscountId
        {
            get
            {
                return reservationDiscountId;
            }

            set
            {
                this.IsChanged = true;
                reservationDiscountId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ReservationDiscountPecentage field
        /// </summary>
        [Browsable(false)]
        public double? ReservationDiscountPecentage
        {
            get
            {
                return reservationDiscountPecentage;
            }

            set
            {
                this.IsChanged = true;
                reservationDiscountPecentage = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ReservationDiscountCategory field
        /// </summary>
        [Browsable(false)]
        public string ReservationDiscountCategory
        {
            get
            {
                return reservationDiscountCategory;
            }

            set
            {
                this.IsChanged = true;
                reservationDiscountCategory = value;
            }
        }


        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>
        [Browsable(false)]
        public int ProductId
        {
            get
            {
                return productId;
            }

            set
            {
                this.IsChanged = true;
                productId = value;
            }
        }

        /// <summary>
        /// Get method of the SiteId field
        /// </summary>
        [Browsable(false)]
        public int SiteId
        {
            get
            {
                return siteId;
            }
        }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [Browsable(false)]
        public int MasterEntityId
        {
            get
            {
                return masterEntityId;
            }

            set
            {
                this.IsChanged = true;
                masterEntityId = value;
            }
        }

        /// <summary>
        /// Get method of the SynchStatus field
        /// </summary>
        [Browsable(false)]
        public bool SynchStatus
        {
            get
            {
                return synchStatus;
            }
        }

        /// <summary>
        /// Get method of the Guid field
        /// </summary>
        [Browsable(false)]
        public string Guid
        {
            get
            {
                return guid;
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
                    return notifyingObjectIsChanged ||id < 0;
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
            log.Debug("Starts-AcceptChanges() Method.");
            this.IsChanged = false;
            log.Debug("Ends-AcceptChanges() Method.");
        }

        /// <summary>
        /// Returns a string that represents the current ReservationDiscountsDTO
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            log.Debug("Starts-ToString() method.");
            StringBuilder returnValue = new StringBuilder("\n-----------------------ReservationDiscountsDTO-----------------------------\n");
            returnValue.Append(" Id : " + Id);
            returnValue.Append(" BookingId : " + BookingId);
            returnValue.Append(" ReservationDiscountId : " + ReservationDiscountId);
            returnValue.Append(" ReservationDiscountPecentage : " + ReservationDiscountPecentage);
            returnValue.Append(" ReservationDiscountCategory : " + ReservationDiscountCategory);
            returnValue.Append(" ProductId : " + ProductId);
            returnValue.Append("\n-------------------------------------------------------------\n");
            log.Debug("Ends-ToString() Method");
            return returnValue.ToString();

        }
    }
}
