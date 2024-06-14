/********************************************************************************************
 * Project Name - Card Credit Plus Pause Time Log DTO
 * Description  - Data object of CardCreditPlusPauseTimeLog
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *1.00       02-Feb-2017   Lakshminarayana           Created 
 *2.70.2       23-Jul-2019    Girish Kundar            Modified : Added Constructor with required Parameter
 ********************************************************************************************/

using System;
using System.ComponentModel;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// DTO class for CardCreditPlusPauseTimeLog
    /// </summary>
    public class CardCreditPlusPauseTimeLogDTO
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
            /// Search by CARD_CP_PAUSE_TIMELOG_ID field
            /// </summary>
            CARD_CP_PAUSE_TIMELOG_ID,

            /// <summary>
            /// Search by  CARD_CREDIT_PLUS_ID field
            /// </summary>
            CARD_CREDIT_PLUS_ID,

            /// <summary>
            /// Search by BALANCE_TIME field
            /// </summary>
            BALANCE_TIME,

            /// <summary>
            /// Search by REFERENCE field
            /// </summary>
            REFERENCE,

            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,

            /// <summary>
            /// Search by POS_MACHINE field
            /// </summary>
            POS_MACHINE,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
        }
        private int cardCPPauseTimeLogId;
        private int cardCreditPlusId;
        private DateTime playStartTime;
        private DateTime pauseStartTime;
        private double balanceTime;
        private string reference;
        private string posMachine;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastupdatedDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;


        /// <summary>
        /// Default constructor
        /// </summary>
        public CardCreditPlusPauseTimeLogDTO()
        {
            log.LogMethodEntry();
            cardCPPauseTimeLogId = -1;
            cardCreditPlusId = -1;
            balanceTime = 0.0;
            siteId = -1;
            synchStatus = false;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// parameterized constructor with required fields
        /// </summary>
        public CardCreditPlusPauseTimeLogDTO(int cardCPPauseTimeLogId, int cardCreditPlusId, DateTime playStartTime,
                                             DateTime pauseStartTime, double balanceTime, string reference,
                                             string posMachine)
            :this()
        {
            log.LogMethodEntry(cardCPPauseTimeLogId, cardCreditPlusId, playStartTime, pauseStartTime,
                                balanceTime, reference, posMachine);
            this.cardCPPauseTimeLogId = cardCPPauseTimeLogId;
            this.cardCreditPlusId = cardCreditPlusId;
            this.playStartTime = playStartTime;
            this.pauseStartTime = pauseStartTime;
            this.balanceTime = balanceTime;
            this.reference = reference;
            this.posMachine = posMachine;
            log.LogMethodExit();
        }


        /// <summary>
        /// parameterized constructor with all the fields
        /// </summary>
        public CardCreditPlusPauseTimeLogDTO(int cardCPPauseTimeLogId, int cardCreditPlusId, DateTime playStartTime,
                                             DateTime pauseStartTime, double balanceTime, string reference,
                                             string posMachine, string createdBy, DateTime creationDate,
                                             string lastUpdatedBy, DateTime lastupdatedDate, string guid,
                                             int siteId, bool synchStatus, int masterEntityId)
            :this(cardCPPauseTimeLogId, cardCreditPlusId, playStartTime, pauseStartTime,
                                balanceTime, reference, posMachine)
        {
            log.LogMethodEntry(cardCPPauseTimeLogId, cardCreditPlusId, playStartTime, pauseStartTime,
                                balanceTime, reference, posMachine, createdBy, creationDate,
                                lastUpdatedBy, lastupdatedDate, guid, siteId, synchStatus, masterEntityId);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastupdatedDate = lastupdatedDate;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the cardCPPauseTimeLogId field
        /// </summary>
        public int CardCPPauseTimeLogId
        {
            get
            {
                return cardCPPauseTimeLogId;
            }

            set
            {
                IsChanged = true;
                cardCPPauseTimeLogId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the cardCreditPlusId field
        /// </summary>
        public int CardCreditPlusId
        {
            get
            {
                return cardCreditPlusId;
            }

            set
            {
                IsChanged = true;
                cardCreditPlusId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the playStartTime field
        /// </summary>
        public DateTime PlayStartTime
        {
            get
            {
                return playStartTime;
            }

            set
            {
                IsChanged = true;
                playStartTime = value;
            }
        }

        /// <summary>
        /// Get/Set method of the pauseStartTime field
        /// </summary>
        public DateTime PauseStartTime
        {
            get
            {
                return pauseStartTime;
            }

            set
            {
                IsChanged = true;
                pauseStartTime = value;
            }
        }

        /// <summary>
        /// Get/Set method of the balanceTime field
        /// </summary>
        public double BalanceTime
        {
            get
            {
                return balanceTime;
            }

            set
            {
                IsChanged = true;
                balanceTime = value;
            }
        }

        /// <summary>
        /// Get/Set method of the reference field
        /// </summary>
        public string Reference
        {
            get
            {
                return reference;
            }

            set
            {
                IsChanged = true;
                reference = value;
            }
        }

        /// <summary>
        /// Get/Set method of the posMachine field
        /// </summary>
        public string POSMachine
        {
            get
            {
                return posMachine;
            }

            set
            {
                IsChanged = true;
                posMachine = value;
            }
        }

        /// <summary>
        /// Get/Set method of the createdBy field
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
        /// Get/Set method of the creationDate field
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
        /// Get/Set method of the lastUpdatedBy field
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
        /// Get/Set method of the lastupdatedDate field
        /// </summary>
        public DateTime LastupdatedDate
        {
            get
            {
                return lastupdatedDate;
            }

            set
            {
                lastupdatedDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the guid field
        /// </summary>
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
        /// Get/Set method of the masterEntityId field
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
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || cardCPPauseTimeLogId < 0;
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
