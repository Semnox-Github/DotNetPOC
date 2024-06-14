/********************************************************************************************
 * Project Name - AccountingCodeCombination DTO
 * Description  - Data object of AccountingCodeCombination
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        28-Dec-2016   Amaresh          Created 
 ********************************************************************************************/
using Semnox.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    /// <summary>
    ///  This is the AccountingCodeCombination data object class. This acts as data holder for the AccountingCodeCombination business object
    /// </summary>   
    public class AccountingCodeCombinationDTO
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByUserParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByAccountingCodeCombinationParameters
        {
            /// <summary>
            /// Search by ID field
            /// </summary>
            ID = 0,
            /// <summary>
            /// Search by OBJECT_TYPE field
            /// </summary>
            OBJECTTYPE = 1,
            /// <summary>
            /// Search by TRANSACTION_TYPE field
            /// </summary>
            TRANSACTIONTYPE = 2,
            /// <summary>
            /// Search by ACCOUNTING_CODE field
            /// </summary>
            ACCOUNTINGCODE = 3,
            /// <summary>
            /// Search by OBJECT_ID field
            /// </summary>
            OBJECTID = 4,
            /// <summary>//starts:Modification  on 28-Jun-2016 added site id
            /// Search by SITE_ID field
            /// </summary>
            SITEID = 5,//Ends:Modification  on 28-Jun-2016 added site id
            /// <summary>//starts:Modification  on 26-Jan-2017 added isactive
            /// Search by ISACTIVE field
            /// </summary>
            ISACTIVE = 6//Ends:Modification  on 26-Jan-2017 added isactive
        }

        int id;
        string objectType;
        string transactionType;
        string accountingCode;
        string description;
        int tax;
        int objectId;
        string objectName;
        DateTime lastUpdatedDate;
        String lastUpdatedUser;
        string guid;
        bool synchStatus;
        int masterEntityId;
        int siteId;
        bool isActive;

        /// <summary>
        /// Default constructor
        /// </summary>
        public AccountingCodeCombinationDTO()
        {
            log.Debug("Starts-AccountingCodeCombinationDTO() default constructor.");
            id = -1;
            tax = -1;
            objectId = -1;
            masterEntityId = -1;
            objectType = "Revenue";
            transactionType = "Debit";
            isActive = true;
            log.Debug("ends-AccountingCodeCombinationDTO() default constructor.");
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public AccountingCodeCombinationDTO(int id, string objectType, string transactionType, string accountingCode, string description, int tax,
                                            int objectId, string objectName, DateTime lastUpdatedDate, String lastUpdatedUser, string guid, bool synchStatus,
                                            int masterEntityId, int siteId, bool isActive)
        {
            log.Debug("Starts-AccountingCodeCombinationDTO() argument constructor.");
            this.id = id;
            this.objectType = objectType;
            this.transactionType = transactionType;
            this.accountingCode = accountingCode;
            this.description = description;
            this.tax = tax;
            this.objectId = objectId;
            this.objectName = objectName;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedUser = lastUpdatedUser;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.siteId = siteId;
            this.isActive = isActive;
            log.Debug("Ends-AccountingCodeCombinationDTO() argument constructor.");
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [DisplayName("Id")]
        [ReadOnly(true)]
        public int Id { get { return id; } set { id = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ObjectType field
        /// </summary>
        [DisplayName("ObjectType")]
        public string ObjectType { get { return objectType; } set { objectType = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the TransactionType field
        /// </summary>
        [DisplayName("TransactionType")]
        public string TransactionType { get { return transactionType; } set { transactionType = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the AccountingCode field
        /// </summary>
        [DisplayName("AccountingCode")]
        public string AccountingCode { get { return accountingCode; } set { accountingCode = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        [DisplayName("Description")]
        public string Description { get { return description; } set { description = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Tax field
        /// </summary>
        [DisplayName("Tax")]
        public int Tax { get { return tax; } set { tax = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ObjectId field
        /// </summary>
        [DisplayName("ObjectId")]
        public int ObjectId { get { return objectId; } set { objectId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ObjectName field
        /// </summary>
        [DisplayName("ObjectName")]
        public string ObjectName { get { return objectName; } set { objectName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("LastUpdatedDate")]
        [ReadOnly(true)]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedUser field
        /// </summary>
        [DisplayName("LastUpdatedBy")]
        [ReadOnly(true)]
        public string LastUpdatedUser { get { return lastUpdatedUser; } set { lastUpdatedUser = value; } }

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
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("Site Id")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value; this.IsChanged = true; } }

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
                    return notifyingObjectIsChanged || id < 0;
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
    }
}
