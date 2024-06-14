/*/********************************************************************************************
 * Project Name - LegacyCardCreditPlusDTO
 * Description  - Data Object File for LegacyCardCreditPlusDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By             Remarks 
 *********************************************************************************************
 *2.130.4     18-Feb-2022    Dakshakh                Created
 ********************************************************************************************/
using Semnox.Parafait.Customer.Accounts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parafait_POS
{
    /// <summary>
    /// This is the LegacyCardCreditPlusDTO data object class. This acts as data holder for the LegacyCardCreditPlus business objects
    /// </summary>
    public class LegacyCardCreditPlusDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Search By LegacyCardCreditPlus enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by Legacy Card Credit Plus Id field
            /// </summary>
            LEGACY_CARD_CREDIT_PLUS_ID,
            /// <summary>
            /// Search by CreditPlusType field
            /// </summary>
            CREDIT_PLUS_TYPE,
            /// <summary>
            /// Search by LegacyCard_id field
            /// </summary>
            LEGACY_CARD_ID,
            /// <summary>
            /// Search by Site Id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by CREDITPLUSTYPE field
            /// </summary>
            CREDITPLUS_TYPE,
            /// <summary>
            /// Search by ISACTIVE field
            /// </summary>
            ISACTIVE,
            /// <summary>
            /// Search by cardId list field
            /// </summary>
            CARD_ID_LIST,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int legacyCardCreditPlusId;
        private decimal legacyCreditPlus;
        private decimal revisedLegacyCreditPlus;
        private CreditPlusType creditPlusType;
        private bool refundable;
        private string remarks;
        private int legacyCard_id;
        private decimal? creditPlusBalance;
        private DateTime? periodFrom;
        private DateTime? periodTo;
        private decimal? timeFrom;
        private decimal? timeTo;
        private int? numberOfDays;
        private bool monday;
        private bool tuesday;
        private bool wednesday;
        private bool thursday;
        private bool friday;
        private bool saturday;
        private bool sunday;
        private decimal? minimumSaleAmount;
        private bool ticketAllowed;
        private bool expireWithMembership;
        private bool pauseAllowed;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private bool isActive;
        private DateTime lastupdatedDate;
        private int site_id;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private List<LegacyCardCreditPlusConsumptionDTO> legacyCardCreditPlusConsumptionDTOList;


        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor of LegacyCardCreditPlusDTO with required fields
        /// </summary>
        public LegacyCardCreditPlusDTO()
        {
            log.LogMethodEntry();
            legacyCardCreditPlusId = -1;
            legacyCard_id = -1;
            site_id = -1;
            masterEntityId = -1;
            refundable = true;
            sunday = true;
            monday = true;
            tuesday = true;
            wednesday = true;
            thursday = true;
            friday = true;
            saturday = true;
            ticketAllowed = true;
            isActive = true;
            pauseAllowed = true;
            legacyCardCreditPlusConsumptionDTOList = new List<LegacyCardCreditPlusConsumptionDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor of LegacyCardCreditPlusDTO with the required fields
        /// </summary>
        public LegacyCardCreditPlusDTO(int legacyCardCreditPlusId, decimal legacyCreditPlus, decimal revisedLegacyCreditPlus, CreditPlusType creditPlusType, bool refundable, string remarks,
         int legacyCard_id, decimal? creditPlusBalance, DateTime? periodFrom, DateTime? periodTo, decimal? timeFrom, decimal? timeTo, int? numberOfDays, bool monday, bool tuesday,
         bool wednesday, bool thursday, bool friday, bool saturday, bool sunday, decimal? minimumSaleAmount, bool ticketAllowed, bool expireWithMembership, bool pauseAllowed)
            : this()
        {
            log.LogMethodEntry(legacyCardCreditPlusId, legacyCreditPlus, revisedLegacyCreditPlus, creditPlusType, refundable, remarks,
                               legacyCard_id, creditPlusBalance, periodFrom, periodTo, timeFrom, timeTo, numberOfDays, monday, tuesday,
                               wednesday, thursday, friday, saturday, sunday, minimumSaleAmount, ticketAllowed, expireWithMembership,
                               pauseAllowed);
            this.legacyCardCreditPlusId = legacyCardCreditPlusId;
            this.legacyCreditPlus = legacyCreditPlus;
            this.revisedLegacyCreditPlus = revisedLegacyCreditPlus;
            this.creditPlusType = creditPlusType;
            this.refundable = refundable;
            this.remarks = remarks;
            this.legacyCard_id = legacyCard_id;
            this.creditPlusBalance = creditPlusBalance;
            this.periodFrom = periodFrom;
            this.periodTo = periodTo;
            this.timeFrom = timeFrom;
            this.timeTo = timeTo;
            this.numberOfDays = numberOfDays;
            this.monday = monday;
            this.tuesday = tuesday;
            this.wednesday = wednesday;
            this.thursday = thursday;
            this.friday = friday;
            this.saturday = saturday;
            this.sunday = sunday;
            this.minimumSaleAmount = minimumSaleAmount;
            this.ticketAllowed = ticketAllowed;
            this.expireWithMembership = expireWithMembership;
            this.pauseAllowed = pauseAllowed;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor of LegacyCardCreditPlusDTO with all fields
        /// </summary>
        public LegacyCardCreditPlusDTO(int legacyCardCreditPlusId, decimal legacyCreditPlus, decimal revisedLegacyCreditPlus, CreditPlusType creditPlusType, bool refundable, string remarks,
         int legacyCard_id, decimal? creditPlusBalance, DateTime? periodFrom, DateTime? periodTo, decimal? timeFrom, decimal? timeTo, int? numberOfDays, bool monday, bool tuesday,
         bool wednesday, bool thursday, bool friday, bool saturday, bool sunday, decimal? minimumSaleAmount, bool ticketAllowed, bool expireWithMembership, bool pauseAllowed,
         string createdBy, DateTime creationDate, string lastUpdatedBy, bool isActive, DateTime lastupdatedDate, int site_id, string guid, bool synchStatus, int masterEntityId)
        : this(legacyCardCreditPlusId, legacyCreditPlus, revisedLegacyCreditPlus, creditPlusType, refundable, remarks,
                               legacyCard_id, creditPlusBalance, periodFrom, periodTo, timeFrom, timeTo, numberOfDays, monday, tuesday,
                               wednesday, thursday, friday, saturday, sunday, minimumSaleAmount, ticketAllowed, expireWithMembership,
                               pauseAllowed)
        {
            log.LogMethodEntry(lastupdatedDate, site_id, lastUpdatedBy, guid, synchStatus, masterEntityId, isActive);
            this.lastupdatedDate = lastupdatedDate;
            this.site_id = site_id;
            this.lastUpdatedBy = lastUpdatedBy;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the LegacyCardCreditPlusId field
        /// </summary>
        public int LegacyCardCreditPlusId { get { return legacyCardCreditPlusId; } set { legacyCardCreditPlusId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LegacyCreditPlus field
        /// </summary>
        public decimal LegacyCreditPlus { get { return legacyCreditPlus; } set { legacyCreditPlus = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the RevisedLegacyCreditPlus field
        /// </summary>
        public decimal RevisedLegacyCreditPlus { get { return revisedLegacyCreditPlus; } set { revisedLegacyCreditPlus = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CreditPlusType field
        /// </summary>
        public CreditPlusType CreditPlusType { get { return creditPlusType; } set { creditPlusType = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Refundable field
        /// </summary>
        public bool Refundable { get { return refundable; } set { refundable = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Refundable field
        /// </summary>
        public string Remarks { get { return remarks; } set { remarks = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LegacyCard_id field
        /// </summary>
        public int LegacyCard_id { get { return legacyCard_id; } set { legacyCard_id = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CreditPlusBalance field
        /// </summary>
        public decimal? CreditPlusBalance { get { return creditPlusBalance; } set { creditPlusBalance = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the periodFrom field
        /// </summary>
        public DateTime? PeriodFrom { get { return periodFrom; } set { periodFrom = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the periodTo field
        /// </summary>
        public DateTime? PeriodTo { get { return periodTo; } set { periodTo = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the timeFrom field
        /// </summary>
        public decimal? TimeFrom { get { return timeFrom; } set { timeFrom = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the timeTo field
        /// </summary>
        public decimal? TimeTo { get { return timeTo; } set { timeTo = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the NumberOfDays field
        /// </summary>
        public int? NumberOfDays { get { return numberOfDays; } set { numberOfDays = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the monday field
        /// </summary>
        public bool Monday { get { return monday; } set { monday = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Tuesday field
        /// </summary>
        public bool Tuesday { get { return tuesday; } set { tuesday = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Wednesday field
        /// </summary>
        public bool Wednesday { get { return wednesday; } set { wednesday = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Thursday field
        /// </summary>
        public bool Thursday { get { return thursday; } set { thursday = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the friday field
        /// </summary>
        public bool Friday { get { return friday; } set { friday = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Saturday field
        /// </summary>
        public bool Saturday { get { return saturday; } set { saturday = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Sunday field
        /// </summary>
        public bool Sunday { get { return sunday; } set { sunday = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MinimumSaleAmount field
        /// </summary>
        public decimal? MinimumSaleAmount { get { return minimumSaleAmount; } set { minimumSaleAmount = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the TicketAllowed field
        /// </summary>
        public bool TicketAllowed { get { return ticketAllowed; } set { ticketAllowed = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ExpireWithMembership field
        /// </summary>
        public bool ExpireWithMembership { get { return expireWithMembership; } set { expireWithMembership = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the PauseAllowed field
        /// </summary>
        public bool PauseAllowed { get { return pauseAllowed; } set { pauseAllowed = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LastUpdateTime field
        /// </summary>
        public DateTime LastUpdateDate { get { return lastupdatedDate; } set { lastupdatedDate = value; } }
        /// <summary>
        /// Get/Set method of the CardId field
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the UploadSiteId field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }
        /// <summary>
        /// Get/Set method of the ExpiryDate field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the site_id field
        /// </summary>
        public int Site_id { get { return site_id; } set { site_id = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the creationDate field
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the creationDate field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set Methods for LegacyCreditPlusConsumptionDTOList field
        /// </summary>
        public List<LegacyCardCreditPlusConsumptionDTO> LegacyCardCreditPlusConsumptionDTOList
        {
            get
            {
                return legacyCardCreditPlusConsumptionDTOList;
            }
            set
            {
                legacyCardCreditPlusConsumptionDTOList = value;
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
                    return notifyingObjectIsChanged || LegacyCardCreditPlusId < 0;
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
        /// Returns whether LegacyCardCreditPlus or any child record is changed
        /// </summary>
        [Browsable(false)]
        public bool IsChangedRecursive
        {
            get
            {
                bool isChangedRecursive = IsChanged;
                if (LegacyCardCreditPlusConsumptionDTOList != null)
                {
                    foreach (var legacyCardCreditPlusConsumptionDTO in LegacyCardCreditPlusConsumptionDTOList)
                    {
                        isChangedRecursive = isChangedRecursive || legacyCardCreditPlusConsumptionDTO.IsChanged;
                    }
                }
                //if (legacyCardGamesDTOList != null)
                //{
                //    foreach (var legacyCardGamesDTO in legacyCardGamesDTOList)
                //    {
                //        isChangedRecursive = isChangedRecursive || legacyCardGamesDTO.IsChanged;
                //    }
                //}
                //if (legacyCardDiscountsDTOList != null)
                //{
                //    foreach (var legacyCardDiscountsDTO in legacyCardDiscountsDTOList)
                //    {
                //        isChangedRecursive = isChangedRecursive || legacyCardDiscountsDTO.IsChanged;
                //    }
                //}
                return isChangedRecursive;
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
