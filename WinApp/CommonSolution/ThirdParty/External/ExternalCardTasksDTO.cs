/********************************************************************************************
 * Project Name - ThirdParty
 * Description  - ExternalCardTasks Data Transfer Object.
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.140.5    20-Jan-2023   Abhishek             Created : External  REST API.
 ***************************************************************************************************/
using System;
using System.Collections.Generic;

namespace Semnox.Parafait.ThirdParty.External
{
    public class ExternalCardTasksDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int sourceAccountId;
        private int sourceCreditPlusId;
        private int destinationAccountId;
        private int legacyCardId;
        private AccountTaskType accountTaskType;
        private decimal? valueLoaded;
        //private CreditPlusType? entitlementType;
        private string sourceAccountNumber;
        private string destinationAccountNumber;
        private DateTime activityDate;
        private string approvedBy;
        private DateTime? approvalDate;
        private int pOSMachineId;
        private int cashDrawerId;
        private int userId;
        private string groupTaskGuid;
        private string remarks;
        private bool invalidateAccount;


        /// <summary>
        /// Get/Set method of the SourceAccountId field
        /// </summary>
        public int SourceAccountId { get { return sourceAccountId; } set { sourceAccountId = value;  } }
        /// <summary>
        /// Get/Set method of the SourceCreditPlusId field
        /// </summary>
        public int SourceCreditPlusId { get { return sourceCreditPlusId; } set { sourceCreditPlusId = value;  } }

        /// <summary>
        /// Get/Set method of the DestinationAccountId field
        /// </summary>
        public int DestinationAccountId { get { return destinationAccountId; } set { destinationAccountId = value;  } }

        /// <summary>
        /// Get/Set method of the LegacyCardId field
        /// </summary>
        public int LegacyCardId { get { return legacyCardId; } set { legacyCardId = value;  } }

        /// <summary>
        /// Get/Set method of the AccountTaskType field
        /// </summary>
        public AccountTaskType AccountTaskType { get { return accountTaskType; } set { accountTaskType = value;  } }

        /// <summary>
        /// Get/Set method of the ValueLoaded field
        /// </summary>
        public decimal? ValueLoaded { get { return valueLoaded; } set { valueLoaded = value;  } }

        /// <summary>
        /// Get/Set method of the ActivityDate field
        /// </summary>
        public DateTime ActivityDate { get { return activityDate; } set { activityDate = value;  } }

        /// <summary>
        /// Get/Set method of the ApprovalDate field
        /// </summary>
        public DateTime? ApprovalDate { get { return approvalDate; } set { approvalDate = value;  } }

        /// <summary>
        /// Get/Set method of the ApprovedBy field
        /// </summary>
        public string ApprovedBy { get { return approvedBy; } set { approvedBy = value;  } }

        /// <summary>
        /// Get/Set method of the SourceAccountNumber field
        /// </summary>
        public string SourceAccountNumber { get { return sourceAccountNumber; } set { sourceAccountNumber = value;  } }

        /// <summary>
        /// Get/Set method of the DestinationAccountNumber field
        /// </summary>
        public string DestinationAccountNumber { get { return destinationAccountNumber; } set { destinationAccountNumber = value;  } }

        /// <summary>
        /// Get/Set method of the POSMachineId field
        /// </summary>
        public int POSMachineId { get { return pOSMachineId; } set { pOSMachineId = value;  } }

        /// <summary>
        /// Get/Set method of the CashDrawerId field
        /// </summary>
        public int CashDrawerId { get { return cashDrawerId; } set { cashDrawerId = value;  } }

        /// <summary>
        /// Get/Set method of the UserId field
        /// </summary>
        public int UserId { get { return userId; } set { userId = value;  } }

        /// <summary>
        /// Get/Set method of the GroupTaskGuid field
        /// </summary>
        public string GroupTaskGuid { get { return groupTaskGuid; } set { groupTaskGuid = value;  } }

        /// <summary>
        /// Get/Set method of the InvalidateAccount field
        /// </summary>
        public bool InvalidateAccount { get { return invalidateAccount; } set { invalidateAccount = value;  } }

        /// <summary>
        /// Get/Set method of the remarks field
        /// </summary>
        public string Remarks { get { return remarks; } set { remarks = value; } }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ExternalCardTasksDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        ///  constructor with Required Parameter
        /// </summary>
        public ExternalCardTasksDTO(AccountTaskType accountTaskType, int sourceAccountId, int sourceCreditPlusId, int destinationAccountId, int legacyCardId, decimal? valueLoaded,  DateTime activityDate,
                              string approvedBy, DateTime? approvalDate, int pOSMachineId, int cashDrawerId, int userId, string groupTaskGuid, bool invalidateAccount, string sourceAccountNumber, string destinationAccountNumber, string remarks)
            : this()
        {
            log.LogMethodEntry(accountTaskType, sourceAccountId, sourceCreditPlusId, destinationAccountId, legacyCardId, valueLoaded, activityDate,
                               approvedBy, approvalDate, pOSMachineId, userId, groupTaskGuid, invalidateAccount);
            this.accountTaskType = accountTaskType;
            this.sourceAccountId = sourceAccountId;
            this.sourceCreditPlusId = sourceCreditPlusId;
            this.destinationAccountId = destinationAccountId;
            this.legacyCardId = legacyCardId;
            this.valueLoaded = valueLoaded;
            this.activityDate = activityDate;
            this.approvedBy = approvedBy;
            this.approvalDate = approvalDate;
            this.pOSMachineId = pOSMachineId;
            this.cashDrawerId = cashDrawerId;
            this.userId = userId;
            this.groupTaskGuid = groupTaskGuid;
            this.invalidateAccount = invalidateAccount;
            this.sourceAccountNumber = sourceAccountNumber;
            this.destinationAccountNumber = destinationAccountNumber;
            this.remarks = remarks;
            log.LogMethodExit();
        }
    }
}
