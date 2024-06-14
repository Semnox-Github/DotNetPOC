/********************************************************************************************
 * Project Name - Locker Access Point DTO
 * Description  - Data object of Locker Access Point DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        14-Jul-2017   Raghuveera          Created 
 *2.70.2        18-Jul-2019   Dakshakh raj        Modified : Added Parameterized costrustor.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Semnox.Parafait.Device.Lockers
{
    /// <summary>
    /// This is the  Locker Access Point data object class. This acts as data holder for the Locker Access Point business object
    /// </summary>
    public class LockerAccessPointDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByLockerAccessPointParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByLockerAccessPointParameters
        {
            /// <summary>
            /// Search by ACCESS POINT ID field
            /// </summary>
            ACCESS_POINT_ID,
            
            /// <summary>
            /// Search by NAME field
            /// </summary>
            NAME,
            
            /// <summary>
            /// Search by IP ADDRESS field
            /// </summary>
            IP_ADDRESS,
           
            /// <summary>
            /// Search by LOCKER ID FROM field
            /// </summary>
            LOCKER_ID_FROM,
            
            /// <summary>
            /// Search by LOCKER ID FROM field
            /// </summary>
            LOCKER_ID_TO,
            
            /// <summary>
            /// Search by IS ALIVE field
            /// </summary>
            IS_ALIVE,
           
            /// <summary>
            /// Search by ACTIVE FLAG field
            /// </summary>
            ACTIVE_FLAG,
           
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
           
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by LOAD_CHILD_RECORDS field
            /// </summary>
            LOAD_CHILD_RECORDS = 9
        }

        private int accessPointId;
        private string name;
        private string iPAddress;
        private int portNo;
        private int channel;
        private string gatewayIP;
        private long baudRate;
        private int lockerIDFrom;
        private int lockerIDTo;
        private long dataPackingTime;
        private int dataPackingSize;
        private bool isAlive;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private string groupCode;
        //List<LockerDTO> lockerList;
        List<LockerZonesDTO> lockerZonesDTOList;

        /// <summary>
        /// Default constructor
        /// </summary>
        public LockerAccessPointDTO()
        {
            log.LogMethodEntry();
            accessPointId = -1;
            portNo = 0;
            channel = 0;
            baudRate = 0;
            lockerIDFrom = -1;
            lockerIDTo = -1;
            dataPackingTime = 0;
            dataPackingSize = 0;
            isAlive = false;
            isActive = true;
            groupCode = "0";
            siteId = -1;
            masterEntityId = -1;
            lockerZonesDTOList = new List<LockerZonesDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required fields
        /// </summary>
        public LockerAccessPointDTO(int accessPointId, string name, string iPAddress, int portNo, int channel, string gatewayIP,
                                    long baudRate, int lockerIDFrom, int lockerIDTo, long dataPackingTime, int dataPackingSize, bool isAlive,
                                    bool isActive, string groupCode)
            :this()
        {
            log.LogMethodEntry( accessPointId,  name,  iPAddress,  portNo,  channel,  gatewayIP, baudRate,  lockerIDFrom,  lockerIDTo,  dataPackingTime,  dataPackingSize,  isAlive,
                                isActive,  groupCode);
            this.accessPointId = accessPointId;
            this.name = name;
            this.iPAddress = iPAddress;
            this.portNo = portNo;
            this.channel = channel;
            this.gatewayIP = gatewayIP;
            this.baudRate = baudRate;
            this.lockerIDFrom = lockerIDFrom;
            this.lockerIDTo = lockerIDTo;
            this.dataPackingTime = dataPackingTime;
            this.dataPackingSize = dataPackingSize;
            this.isAlive = isAlive;
            this.isActive = isActive;
            this.groupCode = groupCode;
            this.lockerZonesDTOList = lockerZonesDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public LockerAccessPointDTO(int accessPointId, string name, string iPAddress, int portNo,int channel, string gatewayIP,
                                    long baudRate, int lockerIDFrom, int lockerIDTo, long dataPackingTime, int dataPackingSize, bool isAlive,
                                    bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate,
                                    string guid, int siteId, bool synchStatus, int masterEntityId, string groupCode)
            :this(accessPointId, name, iPAddress, portNo, channel, gatewayIP, baudRate, lockerIDFrom, lockerIDTo, dataPackingTime, dataPackingSize, isAlive,
                                isActive, groupCode)
        {
            log.LogMethodEntry(createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, guid, siteId, synchStatus, masterEntityId);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the AccessPointId field
        /// </summary>
        [DisplayName("AccessPointId")]
        [ReadOnly(true)]
        public int AccessPointId { get { return accessPointId; } set { accessPointId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        [DisplayName("Name")]
        public string Name { get { return name; } set { name = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ScheduleTime field
        /// </summary>
        [DisplayName("IP Address")]
        public string IPAddress { get { return iPAddress; } set { iPAddress = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the RecurFlag field
        /// </summary>
        [DisplayName("Port No")]
        public int PortNo { get { return portNo; } set { portNo = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Channel field
        /// </summary>
        [DisplayName("Channel(1~25)")]
        public int Channel { get { return channel; } set { channel = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the GatewayIP field
        /// </summary>
        [DisplayName("Gateway IP")]
        public string GatewayIP { get { return gatewayIP; } set { gatewayIP = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the BaudRate field
        /// </summary>
        [DisplayName("Baud Rate")]
        public long BaudRate { get { return baudRate; } set { baudRate = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LockerIDFrom field
        /// </summary>
        [DisplayName("Locker From")]
        public int LockerIDFrom { get { return lockerIDFrom; } set { lockerIDFrom = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LockerIDTo field
        /// </summary>
        [DisplayName("Locker To")]
        public int LockerIDTo { get { return lockerIDTo; } set { lockerIDTo = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the DataPackingTime field
        /// </summary>
        [DisplayName("Data Packing Time")]
        public long DataPackingTime { get { return dataPackingTime; } set { dataPackingTime = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the DataPackingSize field
        /// </summary>
        [DisplayName("Data Packing Size")]
        public int DataPackingSize { get { return dataPackingSize; } set { dataPackingSize = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("IsAlive?")]
        public bool IsAlive { get { return isAlive; } set { isAlive = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Active?")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }
        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        [Browsable(false)]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }
        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [DisplayName("Created Date")]
        [Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }
        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Updated By")]
        [Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }
        /// <summary>
        /// Get method of the LastupdatedDate field
        /// </summary>
        [DisplayName("Updated Date")]
        [Browsable(false)]
        public DateTime LastupdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; } }
        /// <summary>
        /// Get/Set method of the Siteid field
        /// </summary>
        [DisplayName("Site id")]
        [Browsable(false)]
        public int Siteid { get { return siteId; } set { siteId = value; } }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>        
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; } }

        ///// <summary>
        ///// Get/Set method of the LockerList field
        ///// </summary>        
        //[DisplayName("Locker List")]
        //[Browsable(false)]
        //public List<LockerDTO> LockerList { get { return lockerList; } set { lockerList = value; } }

        /// <summary>
        /// Get/Set method of the LockerList field
        /// </summary>        
        [DisplayName("Locker Zone List")]
        [Browsable(false)]
        public List<LockerZonesDTO> LockerZonesDTOList { get { return lockerZonesDTOList; } set { lockerZonesDTOList = value; } }
        /// <summary>
        /// Get/Set method of the GroupCode field
        /// </summary>
        [DisplayName("Group Code")]
        public string GroupCode { get { return groupCode; } set { groupCode = value; this.IsChanged = true; } }

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
                    return notifyingObjectIsChanged || accessPointId < 0;
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
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }
}
