/********************************************************************************************
 * Project Name - Transaction User Log DTO
 * Description  - Data object of Transaction User Log
 *  
 **************
 * Version Log
 **************
 * Version       Date          Modified By          Remarks          
 *********************************************************************************************
 *2.70.0        06-Jun-2019      Akshay Gulaganji     Code merge from Development to WebManagementStudio
 *2.80          29-May-2020      Vikas Dwivedi        Modified as per the Standard CheckList
 *2.140.0       14-Sep-2021      Guru S A             Waiver mapping UI enhancements
 *2.130.7       13-Apr-2022      Guru S A             Payment mode OTP validation changes
 **********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// DTO class for TrxUserLogs
    /// </summary>
    public class TrxUserLogsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByParameters enum controls the search fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by TRX_USER_LOG_ID field
            /// </summary>
            TRX_USER_LOG_ID,

            /// <summary>
            /// Search by  TRX_ID field
            /// </summary>
            TRX_ID,

            /// <summary>
            /// Search by LINE_ID field
            /// </summary>
            LINE_ID,

            /// <summary>
            /// Search by LOGIN_ID field
            /// </summary>
            LOGIN_ID,

            /// <summary>
            /// Search by POS_MACHINE_ID field
            /// </summary>
            POS_MACHINE_ID,

            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID
        }

        private int trxUserLogId;
        private int trxId;
        private int lineId;
        private string loginId;
        private DateTime activityDate;
        private int posMachineId;
        private string action;
        private string activity;
        private DateTime creationDate;
        private string createdBy;
        private DateTime lastupdateDate;
        private string lastUpdatedBy;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private string productName;
        private string approverId;
        private DateTime? approvalTime;

        /// <summary>
        /// Default constructor
        /// </summary>
        public TrxUserLogsDTO()
        {
            log.LogMethodEntry();
            trxUserLogId = -1;
            trxId = -1;
            lineId = -1;
            posMachineId = -1;
            siteId = -1;
            loginId = "";
            synchStatus = false;
            masterEntityId = -1;
            approverId = "-1";
            approvalTime = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// parameterised constructor
        /// </summary>
        public TrxUserLogsDTO(int trxUserLogId, int trxId, int lineId, string loginId, DateTime activityDate, int posMachineId, string action, string activity, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastupdateDate, string guid, int siteId, bool synchStatus, int masterEntityId, string approverId, DateTime? approvalTime)
        {
            log.LogMethodEntry(trxUserLogId, trxId, lineId, loginId, activityDate, posMachineId, action, activity, createdBy, creationDate, lastUpdatedBy, lastupdateDate, guid, siteId, synchStatus, masterEntityId, approverId, approvalTime);
            this.trxUserLogId = trxUserLogId;
            this.trxId = trxId;
            this.lineId = lineId;
            this.loginId = loginId;
            this.activityDate = activityDate;
            this.posMachineId = posMachineId;
            this.action = action;
            this.activity = activity;
            this.creationDate = creationDate;
            this.createdBy = createdBy;
            this.lastupdateDate = lastupdateDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.approverId = approverId;
            this.approvalTime = approvalTime;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the trxUserLogId field
        /// </summary>
        [DisplayName("TrxUserLogId")]
        [ReadOnly(true)]
        [Browsable(false)]
        public int TrxUserLogId
        {
            get
            {
                return trxUserLogId;
            }

            set
            {
                IsChanged = true;
                trxUserLogId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the trxId field
        /// </summary>
        [DisplayName("TrxId")]
        public int TrxId
        {
            get
            {
                return trxId;
            }

            set
            {
                IsChanged = true;
                trxId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the lineId field
        /// </summary>
        [DisplayName("LineId")]
        public int LineId
        {
            get
            {
                return lineId;
            }

            set
            {
                IsChanged = true;
                lineId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the loginId field
        /// </summary>
        [DisplayName("LoginId")]
        public string LoginId
        {
            get
            {
                return loginId;
            }

            set
            {
                IsChanged = true;
                loginId = value;
            }
        }


        /// <summary>
        /// Get/Set method of the activityDate field
        /// </summary>
        [DisplayName("ActivityDate")]
        public DateTime ActivityDate
        {
            get
            {
                return activityDate;
            }

            set
            {
                IsChanged = true;
                activityDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the posMachineId field
        /// </summary>
        [DisplayName("PosMachineId")]
        [Browsable(false)]
        public int PosMachineId
        {
            get
            {
                return posMachineId;
            }

            set
            {
                IsChanged = true;
                posMachineId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the action field
        /// </summary>
        [DisplayName("Action")]
        public string Action
        {
            get
            {
                return action;
            }

            set
            {
                IsChanged = true;
                action = value;
            }
        }

        /// <summary>
        /// Get/Set method of the activity field
        /// </summary>
        [DisplayName("Activity")]
        public string Activity
        {
            get
            {
                return activity;
            }

            set
            {
                IsChanged = true;
                activity = value;
            }
        }

        /// <summary>
        /// Get/Set method of the createdBy field
        /// </summary>
        [DisplayName("CreatedBy")]
        [Browsable(false)]
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
        /// Get/Set method of the creationDate field
        /// </summary>
        [DisplayName("CreationDate")]
        [Browsable(false)]
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
        /// Get/Set method of the lastUpdatedBy field
        /// </summary>
        [DisplayName("LastUpdatedBy")]
        [Browsable(false)]
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
        /// Get/Set method of the lastupdateDate field
        /// </summary>
        [DisplayName("LastupdateDate")]
        [Browsable(false)]
        public DateTime LastupdateDate
        {
            get
            {
                return lastupdateDate;
            }

            set
            {
                lastupdateDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid
        {
            get
            {
                return guid;
            }

            set
            {
                IsChanged = true;
                guid = value;
            }
        }

        /// <summary>
        /// Get/Set method of the siteId field
        /// </summary>
        [DisplayName("SiteId")]
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
        /// Get/Set method of the synchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        [Browsable(false)]
        public bool SynchStatus
        {
            get
            {
                return synchStatus;
            }

            set
            {
                IsChanged = true;
                synchStatus = value;
            }
        }

        /// <summary>
        /// Get/Set method of the masterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId
        {
            get
            {
                return masterEntityId;
            }

            set
            {
                IsChanged = true;
                masterEntityId = value;
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
                IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the ApproverId field
        /// </summary>
        [DisplayName("ApproverId")]
        public string ApproverId
        {
            get
            {
                return approverId;
            }

            set
            {
                approverId = value;
                IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the ApprovalTime field
        /// </summary>
        [DisplayName("ApprovalTime")]
        public DateTime? ApprovalTime
        {
            get
            {
                return approvalTime;
            }

            set
            {
                approvalTime = value;
                IsChanged = true;
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
                    return notifyingObjectIsChanged || trxUserLogId < 0;
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
    /// <summary>
    /// ApprovalAction
    /// </summary>
    public class ApprovalAction
    {
        private string approverId;
        private DateTime? approvalTime;

        /// <summary>
        /// Approval action type
        /// </summary>
        public enum ApprovalActionType
        {
            /// <summary>
            /// Action Type ADD
            /// </summary>
            ADD,

            /// <summary>
            /// Action type REMOVE
            /// </summary>
            REMOVE,

            /// <summary>
            /// Action type ADD DISCOUNT
            /// </summary>
            ADD_DISCOUNT,

            /// <summary>
            /// Action type REPRINT KOT
            /// </summary>
            REPRINT_KOT,

            /// <summary>
            /// Action type COMPLETE
            /// </summary>
            COMPLETE,

            /// <summary>
            /// Action type SAVE
            /// </summary>
            SAVE,
            /// <summary>
            /// Action type OVERRIDE_WAIVER
            /// </summary>
            OVERRIDE_WAIVER,
            /// <summary>
            /// Action type ITEM_REFUND
            /// </summary>
            ITEM_REFUND,
            /// <summary>
            /// Action type RESET_OVERRIDEN_WAIVER
            /// </summary>
            RESET_OVERRIDEN_WAIVER,
            /// <summary>
            /// Action type OVERRIDE_PAYMENT_MODE_OTP
            /// </summary>
            OVERRIDE_PAYMENT_MODE_OTP

        }

        private static readonly Dictionary<ApprovalActionType, string> ActionMap = new Dictionary<ApprovalActionType, string>
        {
            {ApprovalActionType.ADD, "ADD_PRODUCT"},
            {ApprovalActionType.ADD_DISCOUNT, "ADD_DISCOUNT"},
            {ApprovalActionType.REMOVE, "REMOVE"},
            {ApprovalActionType.REPRINT_KOT, "REPRINT_KOT"},
            {ApprovalActionType.COMPLETE, "COMPLETE"},
            {ApprovalActionType.SAVE, "SAVE"},
            {ApprovalActionType.OVERRIDE_WAIVER, "OVERRIDE_WAIVER"},
            {ApprovalActionType.RESET_OVERRIDEN_WAIVER, "RESET_OVERRIDEN_WAIVER"},
            {ApprovalActionType.ITEM_REFUND, "ITEM_REFUND"},
            {ApprovalActionType.OVERRIDE_PAYMENT_MODE_OTP, "OVERRIDE_PAYMENT_MODE_OTP"}
        };
        /// <summary>
        /// GetApprovalActionType
        /// </summary>
        /// <param name="approvalType"></param>
        /// <returns></returns>
        public static string GetApprovalActionType(ApprovalActionType approvalType)
        {
            return ActionMap[approvalType];
        }
        /// <summary>
        /// ApprovalAction
        /// </summary>
        /// <param name="approverId"></param>
        /// <param name="approvalTime"></param>
        public ApprovalAction(string approverId, DateTime? approvalTime)
        {
            this.approverId = approverId;
            this.approvalTime = approvalTime;
        }
        /// <summary>
        /// ApproverId
        /// </summary>
        public string ApproverId { get { return approverId; } set { approverId = value; } }
        /// <summary>
        /// ApprovalTime
        /// </summary>
        public DateTime? ApprovalTime { get { return approvalTime; } set { approvalTime = value; } }
    }
}
