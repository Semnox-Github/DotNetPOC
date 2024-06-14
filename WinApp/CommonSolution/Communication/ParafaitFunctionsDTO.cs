/********************************************************************************************
* Project Name - ParafaitFunctionsDTO
* Description - DTO for ParafaitFunctions 
*
**************
**Version Log 
**************
*Version    Date        Modified By     Remarks
*********************************************************************************************
*2.110.0    09-Dec-2020  Fiona          Created for subscription feature
*********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Communication
{
    /// <summary>
    /// Parafait Functions 
    /// </summary>
    public enum ParafaitFunctions
    { 
        //NONE
        NONE,
        //Customer 
        CUSTOMER_FUNCTIONS,
        //Subscription
        SUBSCRIPTION_FUNCTIONS, 
        //Transaction
        TRANSACTION_FUNCTIONS,
        //Redemption
        REDEMPTION_FUNCTIONS,
        //Maintenance
        MAINTENANCE_FUNCTIONS,
        //Generic
        GENERIC_FUNCTIONS

    }
    /// <summary>
    /// ParafaitFunctionsDTO
    /// </summary>
    public class ParafaitFunctionsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int parafaitFunctionId;
        private ParafaitFunctions parafaitFunctionName;
        private string parafaitFunctionNameString;
        private string parafaitFunctionDescription;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;
        private List<ParafaitFunctionEventDTO> parafaitFunctionEventDTOList;
        //private bool notifyingObjectIsChanged;
        //private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// SearchByParameters
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  ID field
            /// </summary>
            PARAFAIT_FUNCTION_ID,
            /// <summary>
            /// Search by  parafaitFunctionName field
            /// </summary>
            PARAFAIT_FUNCTION_NAME,
            /// <summary>
            /// Search by  IsActive field
            /// </summary>            
            IS_ACTIVE,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by Master entity id field
            /// </summary>
            MASTER_ENTITY_ID
        }
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ParafaitFunctionsDTO()
        {
            log.LogMethodEntry();
            parafaitFunctionId = -1;
            parafaitFunctionName = ParafaitFunctions.NONE;
            isActive = true;
            siteId = -1;
            masterEntityId = -1;
            parafaitFunctionEventDTOList = new List<ParafaitFunctionEventDTO>();
            log.LogMethodExit();
        }
        /// <summary>
        /// Gets the ParafaitFunctionName
        /// </summary>
        /// <param name="parafaitFunctions"></param>
        /// <returns></returns>
        private string GetParafaitFunctionNameString(ParafaitFunctions parafaitFunctions)
        {
            log.LogMethodEntry();
            String parafaitFunctionNameString = "";
            parafaitFunctionNameString = Enum.GetName(typeof(ParafaitFunctions), parafaitFunctions);
            log.LogMethodExit(parafaitFunctionNameString);
            return parafaitFunctionNameString;
        }

        /// <summary>
        ///Constructor with Required Fields
        /// </summary>

        public ParafaitFunctionsDTO(int parafaitFunctionId, ParafaitFunctions parafaitFunctionName, string parafaitFunctionDescription):this()
        {
            log.LogMethodEntry(parafaitFunctionId,  parafaitFunctionName,  parafaitFunctionDescription);
            this.parafaitFunctionId = parafaitFunctionId;
            this.parafaitFunctionName = parafaitFunctionName;
            this.parafaitFunctionDescription = parafaitFunctionDescription;
            log.LogMethodExit();
        }
        /// <summary>
        ///Constructor with All Fields
        /// </summary>
        public ParafaitFunctionsDTO(int parafaitFunctionId, ParafaitFunctions parafaitFunctionName, string parafaitFunctionDescription, 
            bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, int siteId, int masterEntityId, bool synchStatus, string guid) : 
            this(parafaitFunctionId, parafaitFunctionName, parafaitFunctionDescription)
        {
            log.LogMethodEntry( parafaitFunctionId,  parafaitFunctionName,  parafaitFunctionDescription,  isActive,  createdBy,  creationDate,  lastUpdatedBy,
             lastUpdateDate,  siteId,  masterEntityId,  synchStatus,  guid
            );
            this.isActive = isActive;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the ParafaitFunctionId field
        /// </summary>
        public int ParafaitFunctionId
        {
            get { return parafaitFunctionId; }
            set { parafaitFunctionId = value; //this.IsChanged = true;
            }
        }
        /// <summary>
        /// Get/Set method of the ParafaitFunctionName field
        /// </summary>
        public ParafaitFunctions ParafaitFunctionName
        {
            get { return parafaitFunctionName; }
            set { parafaitFunctionName = value; //this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the ParafaitFunctionNameString field
        /// </summary>
        public string ParafaitFunctionNameString
        {
            get
            {
                return GetParafaitFunctionNameString(parafaitFunctionName);
            }
        }

        /// <summary>
        /// Get/Set method of the ParafaitFunctionDescription field
        /// </summary>
        public string ParafaitFunctionDescription
        {
            get { return parafaitFunctionDescription; }
            set { parafaitFunctionDescription = value;// this.IsChanged = true;
            }
        }
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive
        {
            get
            {
                return isActive;
            }

            set
            {
               // this.IsChanged = true;
                isActive = value;
            }
        }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
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
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate
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
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
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
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
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
        /// Get/Set method of the SiteId field
        /// </summary>
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
        public int MasterEntityId
        {
            get
            {
                return masterEntityId;
            }
            set
            {
                //this.IsChanged = true;
                masterEntityId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus
        {
            get
            {
                return synchStatus;
            }
            set
            {
                synchStatus = value;
            }
        }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid
        {
            get
            {
                return guid;
            }
            set
            {
               // this.IsChanged = true;
                guid = value;
            }
        }

        /// <summary>
        /// Get/Set method of parafaitFunctionEventDTOList field
        /// </summary>
        public List<ParafaitFunctionEventDTO> ParafaitFunctionEventDTOList
        {
            get
            {
                return parafaitFunctionEventDTOList;
            }
            set
            { 
                parafaitFunctionEventDTOList = value;
            }
        }
        ///// <summary>
        ///// Get/Set method to track changes to the object
        ///// </summary>
        //public bool IsChanged
        //{
        //    get
        //    {
        //        lock (notifyingObjectIsChangedSyncRoot)
        //        {
        //            return notifyingObjectIsChanged || parafaitFunctionId < 0;
        //        }
        //    }

        //    set
        //    {
        //        lock (notifyingObjectIsChangedSyncRoot)
        //        {
        //            if (!Boolean.Equals(notifyingObjectIsChanged, value))
        //            {
        //                notifyingObjectIsChanged = value;
        //            }
        //        }
        //    }
        //}

        ///// <summary>
        ///// Is Changed Recursive
        ///// </summary>
        //public bool IsChangedRecursive
        //{
        //    get
        //    {
        //        if (IsChanged)
        //        {
        //            return true;
        //        }
        //        if (parafaitFunctionEventDTOList != null &&
        //           parafaitFunctionEventDTOList.Any(x => x.IsChanged))
        //        {
        //            return true;
        //        }
        //        return false;
        //    }
        //}
        ///// <summary>
        ///// Allows to accept the changes
        ///// </summary>
        //public void AcceptChanges()
        //{
        //    log.LogMethodEntry();
        //    this.IsChanged = false;
        //    log.LogMethodExit();
        //}
    }
}
