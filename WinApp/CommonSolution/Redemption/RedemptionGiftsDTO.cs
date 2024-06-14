/********************************************************************************************
 * Project Name - RedemptionGifts DTO
 * Description  - Data object of RedemptionGifts
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *1.00        11-May-2017   Lakshminarayana         Created 
 *2.3.0       03-Jul-2018   Archana/Guru S A        Redemption kioks related changes 
 *2.4.0       03-Sep-2018   Archana                 Modified to add orignial redemption gift id parameter
 *                                                  for redemption reversal changes
 *2.70.2       19-Jul-2019   Deeksha                  Modifications as per three tier standard.
 *2.110.0      07-Jan-2021   Girish Kundar           Modified : Added Copy constructor and ProductQuantity in the constructor - Redemption UI changes
 ********************************************************************************************/
using System;
using System.ComponentModel;
using System.Text;

namespace Semnox.Parafait.Redemption
{
    /// <summary>
    /// This is the RedemptionGifts data object class. This acts as data holder for the RedemptionGifts business object
    /// </summary>
    public class RedemptionGiftsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by RedemptionGiftsID field
            /// </summary>
            REDEMPTION_GIFTS_ID ,
            /// <summary>
            /// Search by RedemptionId field
            /// </summary>
            REDEMPTION_ID ,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID ,
            /// <summary>
            /// Search by MasterEntityId field
            /// </summary>
            MASTER_ENTITY_ID ,
            /// <summary>
            /// Search by Gift Line is reversed field
            /// </summary>
            GIFT_LINE_IS_REVERSED 
        }

        private int redemptionGiftsId;
        private int redemptionId;
        private string giftCode;
        private int productId;
        private int locationId;
        private int? tickets;
        private int? graceTickets;
        private int lotId;
        private int siteId;
        int masterEntityId;
        private bool synchStatus;
        private string guid;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private DateTime? creationDate;
        private string createdBy;
        private int originalPriceInTickets;
        private int productQuantity;
        private string productDescription;
        private string productName;
        private bool giftLineIsReversed;
        private string imageFileName;
        private int orignialRedemptionGiftId;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public RedemptionGiftsDTO()
        {
            log.LogMethodEntry();
            redemptionGiftsId = -1;
            masterEntityId = -1;
            productId = -1;
            redemptionId = -1;
            locationId = -1;
            lotId = -1;
            siteId = -1;
            orignialRedemptionGiftId = -1;
            log.LogMethodExit();
        }

        public RedemptionGiftsDTO(RedemptionGiftsDTO copyRedemptionGiftsDTO)
           : this ()
        {
            log.LogMethodEntry(copyRedemptionGiftsDTO);
            this.redemptionGiftsId = copyRedemptionGiftsDTO.RedemptionGiftsId;
            this.redemptionId = copyRedemptionGiftsDTO.RedemptionId;
            this.productQuantity = copyRedemptionGiftsDTO.ProductQuantity;
            this.giftCode = copyRedemptionGiftsDTO.GiftCode;
            this.productName = copyRedemptionGiftsDTO.ProductName;
            this.productDescription = copyRedemptionGiftsDTO.ProductDescription;
            this.productId = copyRedemptionGiftsDTO.ProductId;
            this.locationId = copyRedemptionGiftsDTO.LocationId;
            this.lotId = copyRedemptionGiftsDTO.LotId;
            this.orignialRedemptionGiftId = copyRedemptionGiftsDTO.OrignialRedemptionGiftId;
            this.siteId = copyRedemptionGiftsDTO.SiteId;
            this.tickets = copyRedemptionGiftsDTO.Tickets;
            this.giftLineIsReversed = copyRedemptionGiftsDTO.GiftLineIsReversed;
            this.graceTickets = copyRedemptionGiftsDTO.GraceTickets;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public RedemptionGiftsDTO(int redemptionGiftsId, int redemptionId, string giftCode, int productId, int locationId, int? tickets,
                         int? graceTickets, int lotId, int originalPriceInTickets, string productName, string productDescription, bool giftLineIsReversed,
                         int orignialRedemptionGiftId,int productQuantity)
            :this()
        {
            log.LogMethodEntry(redemptionGiftsId, redemptionId, giftCode, productId, locationId, tickets,
                          graceTickets, lotId, originalPriceInTickets, productName, productDescription,giftLineIsReversed, orignialRedemptionGiftId);
            this.redemptionGiftsId = redemptionGiftsId;
            this.redemptionId = redemptionId;
            this.giftCode = giftCode;
            this.productId = productId;
            this.locationId = locationId;
            this.tickets = tickets;
            this.graceTickets = graceTickets;
            this.lotId = lotId;
            this.originalPriceInTickets = originalPriceInTickets;
            this.productName = productName;
            this.productDescription = productDescription;
            this.giftLineIsReversed = giftLineIsReversed;
            this.orignialRedemptionGiftId = orignialRedemptionGiftId;
            this.productQuantity = productQuantity;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public RedemptionGiftsDTO(int redemptionGiftsId, int redemptionId, string giftCode, int productId, int locationId, int? tickets,
                         int? graceTickets, int lotId, int siteId, int masterEntityId, bool synchStatus, string guid, string lastUpdatedBy, DateTime lastUpdateDate,
                         DateTime? creationDate, string createdBy, int originalPriceInTickets, string productName, string productDescription, bool giftLineIsReversed,
                         int orignialRedemptionGiftId,int productQuantity)
            :this(redemptionGiftsId, redemptionId, giftCode, productId, locationId, tickets,
                          graceTickets, lotId, originalPriceInTickets, productName, productDescription, giftLineIsReversed, orignialRedemptionGiftId, productQuantity)
        {
            log.LogMethodEntry(redemptionGiftsId, redemptionId, giftCode, productId, locationId, tickets,
                          graceTickets, lotId, siteId, masterEntityId, synchStatus, guid, lastUpdatedBy, lastUpdateDate,
                          creationDate, createdBy, originalPriceInTickets, productName, giftLineIsReversed, orignialRedemptionGiftId, productQuantity);
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.creationDate = creationDate;
            this.createdBy = createdBy;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the RedemptionGiftsId field
        /// </summary>
        [DisplayName("Id")]
        [ReadOnly(true)]
        public int RedemptionGiftsId
        {
            get
            {
                return redemptionGiftsId;
            }

            set
            {
                this.IsChanged = true;
                redemptionGiftsId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        [Browsable(false)]
        public int RedemptionId
        {
            get
            {
                return redemptionId;
            }

            set
            {
                this.IsChanged = true;
                redemptionId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the GiftCode Text field
        /// </summary>
        [DisplayName("Gift Code")]
        public string GiftCode
        {
            get
            {
                return giftCode;
            }

            set
            {
                this.IsChanged = true;
                giftCode = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>
        [DisplayName("Product")]
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
        /// Get/Set method of the LocationId field
        /// </summary>
        [DisplayName("Location")]
        public int LocationId
        {
            get
            {
                return locationId;
            }

            set
            {
                this.IsChanged = true;
                locationId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Tickets field
        /// </summary>
        [DisplayName("Tickets")]
        public int? Tickets
        {
            get
            {
                return tickets;
            }

            set
            {
                this.IsChanged = true;
                tickets = value;
            }
        }

        /// <summary>
        /// Get/Set method of the GraceTickets field
        /// </summary>
        [DisplayName("Grace Tickets")]
        public int? GraceTickets
        {
            get
            {
                return graceTickets;
            }

            set
            {
                this.IsChanged = true;
                graceTickets = value;
            }
        }

        /// <summary>
        /// Get/Set method of the LotId field
        /// </summary>
        [DisplayName("Lot")]
        public int LotId
        {
            get
            {
                return lotId;
            }

            set
            {
                this.IsChanged = true;
                lotId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [Browsable(false)]
        public int SiteId
        {
            get
            {
                return siteId;
            }
            set
            {
                siteId = value;
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
        /// Get/Set method of the SynchStatus field
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
        /// Get/Set method of the Guid field
        /// </summary>
        [Browsable(false)]
        public string Guid
        {
            get
            {
                return guid;
            }
            set
            {
                this.IsChanged = true;
                guid = value;
            }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy  field
        /// </summary>
        [DisplayName("Last Updated By")]
        public string LastUpdatedBy
        {
            get
            {
                return lastUpdatedBy;
            }

            set
            {
                lastUpdatedBy = value;
            }
        }

        /// <summary>
        /// Get/Set method of the LastUpdateDate  field
        /// </summary>
        [DisplayName("Last Update Date")]
        public DateTime LastUpdateDate
        {
            get
            {
                return lastUpdateDate;
            }

            set
            {
                lastUpdateDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CreationDate  field
        /// </summary>
        [DisplayName("Creation Date")]
        public DateTime? CreationDate
        {
            get
            {
                return creationDate;
            }

            set
            {
                creationDate = value;
            }
        }


        /// <summary>
        /// Get/Set method of the CreatedBy  field
        /// </summary>
        [DisplayName("Created By")]
        public string CreatedBy
        {
            get
            {
                return createdBy;
            }

            set
            {
                createdBy = value;
            }
        }


        /// <summary>
        /// Get/Set method of the OriginalPriceInTickets  field
        /// </summary>
        [DisplayName("Original Price In Tickets")]
        public int OriginalPriceInTickets
        {
            get
            {
                return originalPriceInTickets;
            }

            set
            {
                this.IsChanged = true;
                originalPriceInTickets = value;
            }
        }


        /// <summary>
        /// Get/Set method of the productQuantity field
        /// </summary>
        [DisplayName("ProductQuantity")]
        public int ProductQuantity
        {
            get
            {
                return productQuantity;
            }

            set
            {
                productQuantity = value;
                IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the productDescription field
        /// </summary>
        [DisplayName("ProductDescription")]
        public string ProductDescription
        {
            get
            {
                return productDescription;
            }

            set
            {
                productDescription = value;
              //  IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the productName field
        /// </summary>
        [DisplayName("ProductName")]
        public string ProductName
        {
            get
            {
                return productName;
            }

            set
            {
                productName = value;
                //IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the imageFileName field
        /// </summary>
        [DisplayName("ImageFileName")]
        public string ImageFileName
        {
            get
            {
                return imageFileName;
            }

            set
            {
                imageFileName = value;
                IsChanged = true;
            }
        }

        /// <summary>
        /// Get method of the giftLineIsReversed field
        /// </summary>
        [DisplayName("GiftLineIsReversed")]
        public bool GiftLineIsReversed
        {
            get
            {
                return giftLineIsReversed;
            }
            set
            {
                this.IsChanged = true;
                giftLineIsReversed = value;
            }
        }

        /// <summary>
        /// Get/Set method of the OrignialRedemptionGiftId field
        /// </summary>
        [DisplayName("OrignialRedemptionGiftId")]
        [ReadOnly(true)]
        public int OrignialRedemptionGiftId
        {
            get
            {
                return orignialRedemptionGiftId; 
            }

            set
            {
                this.IsChanged = true;
                orignialRedemptionGiftId = value;
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
                    return notifyingObjectIsChanged || redemptionGiftsId < 0;
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

        /// <summary>
        /// Returns a string that represents the current RedemptionGiftsDTO.
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            log.LogMethodEntry();
            StringBuilder returnValue = new StringBuilder("\n-----------------------RedemptionGiftsDTO-----------------------------\n");
            returnValue.Append(" RedemptionGiftsId : " + RedemptionGiftsId);
            returnValue.Append(" RedemptionId : " + RedemptionId);
            returnValue.Append(" GiftCode : " + GiftCode);
            returnValue.Append(" ProductId : " + ProductId);
            returnValue.Append(" LocationId : " + LocationId);
            returnValue.Append(" Tickets : " + Tickets);
            returnValue.Append(" GraceTickets : " + GraceTickets);
            returnValue.Append(" LotId : " + LotId);
            returnValue.Append(" LastUpdatedBy : " + LastUpdatedBy);
            returnValue.Append(" LastUpdateDate : " + LastUpdateDate);
            returnValue.Append(" CreationDate : " + CreationDate);
            returnValue.Append(" CreatedBy : " + CreatedBy);
            returnValue.Append(" OriginalPriceInTickets : " + OriginalPriceInTickets);
            returnValue.Append("\n-------------------------------------------------------------\n");
            log.LogMethodExit(returnValue.ToString());
            return returnValue.ToString();
        }
    }
}
