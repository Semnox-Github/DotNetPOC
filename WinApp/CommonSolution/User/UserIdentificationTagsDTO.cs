/********************************************************************************************
 * Project Name - UserIdentificationTags DTO
 * Description  - Data object of user
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *1.00        13-Apr-2017   Amaresh          Created 
 *2.80        15-Jul-2019   Girish Kundar    Modified : Added Parametrized Constructor with required fields
 *            09-Apr-2020   Indrajeet Kumar  Modified : Added Property FPTemplate & FPSalt 
 *2.110.0     27-Nov-2020   Lakshminarayana  Modified : Changed as part of POS UI redesign. Implemented the new design principles 
 *2.140.0     23-June-2021  Prashanth V      Modified : Added field HRApprovalLogsDTOList
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// UserIdentificationTags Data Object class
    /// </summary>
    public class UserIdentificationTagsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByUserParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByUserIdTagsParameters
        {
            /// <summary>
            /// Search by USER_NAME field
            /// </summary>
            ID,

            /// <summary>
            /// Search by USER_ID field
            /// </summary>
            USER_ID,
            /// <summary>
            /// Search by USER_ID field
            /// </summary>
            USER_ID_LIST,
            /// <summary>
            /// Search by CARD_NUMBER field
            /// </summary>
            CARD_NUMBER,

            /// <summary>
            /// Search by ACTIVE_FLAG field
            /// </summary>
            ACTIVE_FLAG,

            /// <summary>
            /// Search by ATTENDANCE_READER_TAG field
            /// </summary>
            ATTENDANCE_READER_TAG,

            /// <summary>
            /// Search by CARD_ID field
            /// </summary>
            CARD_ID,

            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by FP_TEMPLATE field
            /// </summary>
            FP_TEMPLATE,
            /// <summary>
            /// Search by FP_TEMPLATE field
            /// </summary>
            FP_NUMBER
        }

        private int id;
        private int userId;
        private string cardNumber;
        private string fingerPrint;
        private int fingerNumber;
        private bool activeFlag;
        private DateTime startDate;
        private DateTime endDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private bool attendanceReaderTag;
        private int cardId;
        private string createdBy;
        private DateTime creationDate;
        private byte[] fpTemplate;
        private string fpSalt;
        private List<HRApprovalLogsDTO> hrApprovalLogsDTOList;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public UserIdentificationTagsDTO()
        {
            log.LogMethodEntry();
            id = -1;
            userId = -1;
            fingerNumber = -1;
            siteId = -1;
            cardId = -1;
            masterEntityId = -1;
            attendanceReaderTag = false;
            hrApprovalLogsDTOList = new List<HRApprovalLogsDTO>();
            log.LogMethodExit();
        }


        /// <summary>
        /// UserIdentificationTagsDTO parameterized constructor with required fields
        /// </summary>
        public UserIdentificationTagsDTO(int id, int userId, string cardNumber, string fingerPrint, int fingerNumber, bool activeFlag, DateTime startDate, DateTime endDate,
                                         bool attendanceReaderTag, int cardId, byte[] fpTemplate, string fpSalt)
            : this()
        {
            log.LogMethodEntry(id, userId, cardNumber, fingerPrint, fingerNumber, activeFlag, startDate, endDate,
                                attendanceReaderTag, cardId, fpTemplate, fpSalt);
            this.id = id;
            this.userId = userId;
            this.cardNumber = cardNumber;
            this.fingerPrint = fingerPrint;
            this.fingerNumber = fingerNumber;
            this.activeFlag = activeFlag;
            this.startDate = startDate;
            this.endDate = endDate;
            this.attendanceReaderTag = attendanceReaderTag;
            this.cardId = cardId;
            this.fpTemplate = fpTemplate;
            this.fpSalt = fpSalt;
            log.LogMethodExit();
        }


        /// <summary>
        /// UserIdentificationTagsDTO parameterized constructor with all the fields
        /// </summary>
        public UserIdentificationTagsDTO(int id, int userId, string cardNumber, string fingerPrint, int fingerNumber,
                                          bool activeFlag, DateTime startDate, DateTime endDate,
                                          string lastUpdatedBy, DateTime lastUpdatedDate, string guid, int siteId,
                                          bool synchStatus, int masterEntityId, bool attendanceReaderTag, int cardId,
                                          string createdBy, DateTime creationDate, byte[] fpTemplate, string fpSalt)
            : this(id, userId, cardNumber, fingerPrint, fingerNumber, activeFlag, startDate, endDate,
                                attendanceReaderTag, cardId, fpTemplate, fpSalt)
        {
            log.LogMethodEntry(id, userId, cardNumber, fingerPrint, fingerNumber, activeFlag, startDate, endDate,
                                           lastUpdatedBy, lastUpdatedDate, guid, siteId,
                                           synchStatus, masterEntityId, attendanceReaderTag, cardId,
                                           createdBy, creationDate, fpTemplate, fpSalt);

            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        public UserIdentificationTagsDTO(UserIdentificationTagsDTO userIdentificationTagsDTO)
        {
            log.LogMethodEntry(userIdentificationTagsDTO);
            id = userIdentificationTagsDTO.id;
            userId = userIdentificationTagsDTO.userId;
            cardNumber = userIdentificationTagsDTO.cardNumber;
            fingerPrint = userIdentificationTagsDTO.fingerPrint;
            fingerNumber = userIdentificationTagsDTO.fingerNumber;
            activeFlag = userIdentificationTagsDTO.activeFlag;
            startDate = userIdentificationTagsDTO.startDate;
            endDate = userIdentificationTagsDTO.endDate;
            attendanceReaderTag = userIdentificationTagsDTO.attendanceReaderTag;
            cardId = userIdentificationTagsDTO.cardId;
            fpTemplate = userIdentificationTagsDTO.fpTemplate;
            fpSalt = userIdentificationTagsDTO.fpSalt;
            lastUpdatedBy = userIdentificationTagsDTO.lastUpdatedBy;
            lastUpdatedDate = userIdentificationTagsDTO.lastUpdatedDate;
            guid = userIdentificationTagsDTO.guid;
            siteId = userIdentificationTagsDTO.siteId;
            synchStatus = userIdentificationTagsDTO.synchStatus;
            masterEntityId = userIdentificationTagsDTO.masterEntityId;
            createdBy = userIdentificationTagsDTO.createdBy;
            creationDate = userIdentificationTagsDTO.creationDate;
            if (userIdentificationTagsDTO.hrApprovalLogsDTOList != null)
            {
                hrApprovalLogsDTOList = new List<HRApprovalLogsDTO>();
                foreach (HRApprovalLogsDTO hrApprovalLogsDTO in userIdentificationTagsDTO.hrApprovalLogsDTOList)
                {
                    hrApprovalLogsDTOList.Add(new HRApprovalLogsDTO(hrApprovalLogsDTO));
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [DisplayName("ID")]
        [ReadOnly(true)]
        public int Id { get { return id; } set { id = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the UserId field
        /// </summary>
        [DisplayName("User ID")]
        public int UserId { get { return userId; } set { userId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CardNumber field
        /// </summary>
        [DisplayName("Card Number")]
        public string CardNumber { get { return cardNumber; } set { cardNumber = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the FingerPrint field
        /// </summary>
        [DisplayName("Finger Print")]
        public string FingerPrint { get { return fingerPrint; } set { fingerPrint = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the FingerNumber field
        /// </summary>
        [DisplayName("Finger Number")]
        public int FingerNumber { get { return fingerNumber; } set { fingerNumber = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ActiveFlag field
        /// </summary>
        [DisplayName("ActiveFlag")]
        public bool ActiveFlag { get { return activeFlag; } set { activeFlag = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the StartDate field
        /// </summary>
        [DisplayName("StartDate")]
        public DateTime StartDate { get { return startDate; } set { startDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the EndDate field
        /// </summary>
        [DisplayName("EndDate")]
        [ReadOnly(true)]
        public DateTime EndDate { get { return endDate; } set { endDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("LastUpdatedBy")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value;  } }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("LastUpdatedDate")]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value;  } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("Site Id")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value;  } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value;  } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("Master Entity Id")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }//Ends:Modification  on 28-Jun-2016 Passwordhash, passwordsalt,password and masterEntityId

        /// <summary>
        /// Get/Set method of the AttendanceReaderTag field
        /// </summary>
        [DisplayName("AttendanceReaderTag")]
        public bool AttendanceReaderTag { get { return attendanceReaderTag; } set { attendanceReaderTag = value; this.IsChanged = true; } }//Ends:Modification  on 28-Jun-2016 Passwordhash, passwordsalt,password and masterEntityId

        /// <summary>
        /// Get/Set method of the CardId field
        /// </summary>
        [DisplayName("CardId")]
        public int CardId { get { return cardId; } set { cardId = value; this.IsChanged = true; } }//Ends:Modification  on 28-Jun-2016 Passwordhash, passwordsalt,password and masterEntityId


        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("CreatedBy")]
        public string CreatedBy { get { return createdBy; } set { createdBy = value;  } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("CreationDate")]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value;  } }

        /// <summary>
        /// Get/Set method of the FPTemplate field
        /// </summary>
        [DisplayName("FPTemplate")]
        public byte[] FPTemplate { get { return fpTemplate; } set { fpTemplate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the FPSalt field
        /// </summary>
        [DisplayName("FPSalt")]
        public string FPSalt { get { return fpSalt; } set { fpSalt = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of HRApprovalLogsDTOList
        /// </summary>
        public List<HRApprovalLogsDTO> HRApprovalLogsDTOList
        {
            get
            {
                return hrApprovalLogsDTOList;
            }
            set
            {
                hrApprovalLogsDTOList = value;
                this.IsChanged = true;
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
