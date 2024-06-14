/********************************************************************************************
 * Project Name - PayConfigurationsDTO
 * Description  - Data object of Pay Configurations
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *2.90.0      01-JUL-2020   Akshay Gulaganji   Created  
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// This is the PayConfigurations data object class. This acts as data holder for the PayConfigurations business object
    /// </summary>
    public class PayConfigurationsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by PAY CONFIGURATION ID field
            /// </summary>
            PAY_CONFIGURATION_ID,
            /// <summary>
            /// Search by PAY CONFIGURATION NAME field
            /// </summary>
            PAY_CONFIGURATION_NAME,
            /// <summary>
            /// Search by PAY TYPE ID field
            /// </summary>
            PAY_TYPE_ID,
            /// <summary>
            /// Search by IS ACTIVE field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        Dictionary<int, string> payTypeIdDictionary = new Dictionary<int, string>()
            {
                { -1,"-- None --" },
                { 1,"Hourly" },
                { 2,"Weekly" },
                { 3,"Monthly" }
            };
        /// <summary>
        /// PayType Enum
        /// </summary>
        public enum PayTypeEnum
        {
            ///<summary>
            ///NONE
            ///</summary>
            [Description("None")] NONE = -1,

            ///<summary>
            ///HOURLY
            ///</summary>
            [Description("Hourly")] HOURLY = 1,

            ///<summary>
            ///WEEKLY
            ///</summary>
            [Description("Weekly")] WEEKLY = 2,

            ///<summary>
            ///MONTHLY
            ///</summary>
            [Description("Monthly")] MONTHLY = 3
        }

        public static int PayTypeEnumToInteger(PayTypeEnum status)
        {
            int returnValue = -1;
            switch (status)
            {
                case PayTypeEnum.HOURLY:
                    returnValue = 1;
                    break;
                case PayTypeEnum.WEEKLY:
                    returnValue = 2;
                    break;
                case PayTypeEnum.MONTHLY:
                    returnValue = 3;
                    break;
                default:
                    returnValue = -1;
                    break;
            }
            return returnValue;
        }

        public static PayTypeEnum PayTypeEnumFromString(string status)
        {
            PayTypeEnum returnValue = 0;
            switch (status.ToUpper())
            {
                case "1":
                    returnValue = PayTypeEnum.HOURLY;
                    break;
                case "2":
                    returnValue = PayTypeEnum.WEEKLY;
                    break;
                case "3":
                    returnValue = PayTypeEnum.MONTHLY;
                    break;
                default:
                    returnValue = PayTypeEnum.NONE;
                    break;
            }
            return returnValue;
        }

        private int payConfigurationId;
        private string payConfigurationName;
        private PayTypeEnum payTypeId;
        private bool isActive;
        private string guid;
        private int masterEntityId;
        private int siteId;
        private bool synchStatus;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string createdBy;
        private DateTime creationDate;
        private List<PayConfigurationDetailsDTO> payConfigurationDetailsDTOList;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public PayConfigurationsDTO()
        {
            log.LogMethodEntry();
            payConfigurationId = -1;
            payTypeId = PayTypeEnum.NONE;
            isActive = true;
            siteId = -1;
            masterEntityId = -1;
            payConfigurationDetailsDTOList = new List<PayConfigurationDetailsDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with required fields
        /// </summary>
        /// <param name="payConfigurationId"></param>
        /// <param name="payConfigurationName"></param>
        /// <param name="payTypeId"></param>
        /// <param name="isActive"></param>
        public PayConfigurationsDTO(int payConfigurationId, string payConfigurationName, PayTypeEnum payTypeId, bool isActive) : this()
        {
            log.LogMethodEntry(payConfigurationId, payConfigurationName, payTypeId, isActive);
            this.payConfigurationId = payConfigurationId;
            this.payConfigurationName = payConfigurationName;
            this.payTypeId = payTypeId;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all fields
        /// </summary>
        /// <param name="payConfigurationId"></param>
        /// <param name="payConfigurationName"></param>
        /// <param name="payTypeId"></param>
        /// <param name="isActive"></param>
        /// <param name="guid"></param>
        /// <param name="masterEntityId"></param>
        /// <param name="siteId"></param>
        /// <param name="synchStatus"></param>
        /// <param name="lastUpdatedBy"></param>
        /// <param name="lastUpdatedDate"></param>
        /// <param name="createdBy"></param>
        /// <param name="creationDate"></param>
        public PayConfigurationsDTO(int payConfigurationId, string payConfigurationName, PayTypeEnum payTypeId, bool isActive, string guid, int masterEntityId, int siteId,
                                    bool synchStatus, string lastUpdatedBy, DateTime lastUpdatedDate, string createdBy, DateTime creationDate)
            : this(payConfigurationId, payConfigurationName, payTypeId, isActive)
        {
            log.LogMethodEntry(payConfigurationId, payConfigurationName, payTypeId, isActive, guid, siteId, synchStatus, masterEntityId, lastUpdatedDate, lastUpdatedBy, createdBy, creationDate);
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Pay Configuration Id field
        /// </summary>
        public int PayConfigurationId
        {
            get { return payConfigurationId; }
            set { payConfigurationId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Pay Configuration Name field
        /// </summary>
        public string PayConfigurationName
        {
            get { return payConfigurationName; }
            set { payConfigurationName = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the payType Id field
        /// </summary>
        public PayTypeEnum PayTypeId
        {
            get { return payTypeId; }
            set { payTypeId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Is Active field
        /// </summary>
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid
        {
            get { return guid; }
            set { guid = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Master Entity Id field
        /// </summary>
        public int MasterEntityId
        {
            get { return masterEntityId; }
            set { masterEntityId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Site Id field
        /// </summary>
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
        }
        /// <summary>
        /// Get/Set method of the Synch Status field
        /// </summary>
        public bool SynchStatus
        {
            get { return synchStatus; }
            set { synchStatus = value; }
        }
        /// <summary>
        /// Get/Set method of the Last Updated By field
        /// </summary>
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value; }
        }
        /// <summary>
        ///  Get/Set method of the Last Update Date field
        /// </summary>
        public DateTime LastUpdatedDate
        {
            get { return lastUpdatedDate; }
            set { lastUpdatedDate = value; }
        }
        /// <summary>
        /// Get/Set method of the Created By field
        /// </summary>
        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        /// <summary>
        /// Get/Set method of the Creation Date field
        /// </summary>
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }
        /// <summary>
        /// Get/Set method of the PayConfigurationDetailsDTOList field
        /// </summary>
        public List<PayConfigurationDetailsDTO> PayConfigurationDetailsDTOList
        {
            get { return payConfigurationDetailsDTOList; }
            set { payConfigurationDetailsDTOList = value; }
        }
        /// 
        /// <summary>
        /// Returns whether the PayConfigurations changed or any of its children are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (payConfigurationDetailsDTOList != null &&
                    payConfigurationDetailsDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
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
                    return notifyingObjectIsChanged || payConfigurationId < 0;
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
            IsChanged = false;
            log.LogMethodExit();
        }
    }
}
