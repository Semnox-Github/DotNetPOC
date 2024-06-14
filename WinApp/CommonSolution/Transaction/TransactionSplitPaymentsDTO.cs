using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{

    /// <summary>
    /// This is the TransactionSplitPayments data object class. This acts as data holder for the TransactionSplitPayments business object
    /// </summary>
    public class TransactionSplitPaymentsDTO
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by PAYMENT_ID field
            /// </summary>
            TRANSACTION_ID = 0,
            /// <summary>
            /// Search by PAYMENT_ID field
            /// </summary>
            SITE_ID = 1,
        }

        int splitId;
        int transactionId;
        string userReference;
        int noOfSplits;
        DateTime lastUpdatedDate;
        string lastUpdatedBy;
        string guid;
        bool synchStatus;
        int siteId;
        int masterEntityId;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();


        /// <summary>
        /// Default constructor
        /// </summary>
        public TransactionSplitPaymentsDTO()
        {
            splitId = -1;
            transactionId = -1;
            masterEntityId = -1;
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public TransactionSplitPaymentsDTO(int splitId, int transactionId, string userReference, int noOfSplits,
                                           DateTime lastUpdatedDate, string lastUpdatedBy, string guid, 
                                           bool synchStatus, int siteId, int masterEntityId)
        {
            this.splitId = splitId;
            this.transactionId = transactionId;
            this.userReference = userReference;
            this.noOfSplits = noOfSplits;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
        }

        /// <summary>
        /// Get/Set method of the splitId field
        /// </summary>
        public int SplitId
        {
            get
            {
                return splitId;
            }

            set
            {
                IsChanged = true;
                splitId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        public int TransactionId
        {
            get
            {
                return transactionId;
            }

            set
            {
                IsChanged = true;
                transactionId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the UserReference field
        /// </summary>
        public string UserReference
        {
            get
            {
                return userReference;
            }

            set
            {
                IsChanged = true;
                userReference = value;
            }
        }

        /// <summary>
        /// Get/Set method of the NoOfSplits field
        /// </summary>
        public int NoOfSplits
        {
            get
            {
                return noOfSplits;
            }

            set
            {
                IsChanged = true;
                noOfSplits = value;
            }
        }

        /// <summary>
        /// Get method of the LastUpdatedDate field
        /// </summary>
        public DateTime LastUpdatedDate
        {
            get
            {
                return lastUpdatedDate;
            }
        }

        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy
        {
            get
            {
                return lastUpdatedBy;
            }
        }

        /// <summary>
        /// Get method of the Guid field
        /// </summary>
        public string Guid
        {
            get
            {
                return guid;
            }

        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        public bool SynchStatus
        {
            get
            {
                return synchStatus;
            }
        }

        /// <summary>
        /// Get method of the Id field
        /// </summary>
        public int SiteId
        {
            get
            {
                return siteId;
            }
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
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
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock(notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || splitId < 0;
                }
            }

            set
            {
                lock(notifyingObjectIsChangedSyncRoot)
                {
                    if(!Boolean.Equals(notifyingObjectIsChanged, value))
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
            this.IsChanged = false;
        }

        /// <summary>
        /// Returns a string that represents the current TransactionSplitPaymentsDTO
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            log.Debug("Starts-ToString() method.");
            StringBuilder returnValue = new StringBuilder("\n-----------------------TransactionSplitPaymentsDTO-----------------------------\n");
            returnValue.Append(" SplitId : " + SplitId);
            returnValue.Append(" TransactionId : " + TransactionId);
            returnValue.Append(" UserReference : " + UserReference);
            returnValue.Append(" NoOfSplits : " + NoOfSplits);
            returnValue.Append(" LastUpdatedDate : " + LastUpdatedDate);
            returnValue.Append(" LastUpdatedBy : " + LastUpdatedBy);
            returnValue.Append("\n-------------------------------------------------------------\n");
            log.Debug("Ends-ToString() Method");
            return returnValue.ToString();

        }
    }
}
