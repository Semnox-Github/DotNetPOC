/********************************************************************************************
 * Project Name - Reports
 * Description  - Data Object of WirelessDashBoardBL for Reports module
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.80        15-Jun-2020   Vikas Dwivedi        Created
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Reports
{
    public class WirelessDashBoardDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        // For Hubs Grid
        private int hubId;
        private string hubName;
        private int? portNumber;
        private string address;
        private int machineCount;
        private int totalPolls;
        private int failures;
        private double failurePercent;
        private double trxFailuresPercent;
        private double rcvFailuresPercent;


        // For Machines Grid
        private int machineId;
        private string machineName;
        private string machineAddress;
        private int totalFailures;
        private int trxFailures;
        private int rcvFailures;

        public WirelessDashBoardDTO()
        {
            log.LogMethodEntry();
            hubId = -1;
            machineId = -1;
            machineName = string.Empty;
            machineAddress = string.Empty;
            totalFailures = -1;
            trxFailures = -1;
            rcvFailures = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the HubId field
        /// </summary>
        [DisplayName("HubId")]
        public int HubId
        {
            get
            {
                return hubId;
            }
            set
            {
                hubId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the MachineId field
        /// </summary>
        [DisplayName("MachineId")]
        public int MachineId
        {
            get
            {
                return machineId;
            }
            set
            {
                machineId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the MachineName field
        /// </summary>
        [DisplayName("MachineName")]
        public string MachineName
        {
            get
            {
                return machineName;
            }
            set
            {
                machineName = value;
            }
        }

        /// <summary>
        /// Get/Set method of the MachineName field
        /// </summary>
        [DisplayName("HubName")]
        public string HubName
        {
            get
            {
                return hubName;
            }
            set
            {
                hubName = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Address field
        /// </summary>
        [DisplayName("Address")]
        public string Address
        {
            get
            {
                return address;
            }
            set
            {
                address = value;
            }
        }

        /// <summary>
        /// Get/Set method of the MachineAddress field
        /// </summary>
        [DisplayName("MachineAddress")]
        public string MachineAddress
        {
            get
            {
                return machineAddress;
            }
            set
            {
                machineAddress = value;
            }
        }

        /// <summary>
        /// Get/Set method of the PortNumber field
        /// </summary>
        [DisplayName("PortNumber")]
        public int? PortNumber
        {
            get
            {
                return portNumber;
            }
            set
            {
                portNumber = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TotalPolls field
        /// </summary>
        [DisplayName("TotalPolls")]
        public int TotalPolls
        {
            get
            {
                return totalPolls;
            }
            set
            {
                totalPolls = value;
            }
        }

        /// <summary>
        /// Get/Set method of the MachineCount field
        /// </summary>
        [DisplayName("MachineCount")]
        public int MachineCount
        {
            get
            {
                return machineCount;
            }
            set
            {
                machineCount = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Failures field
        /// </summary>
        [DisplayName("Failures")]
        public int Failures
        {
            get
            {
                return failures;
            }
            set
            {
                failures = value;
            }
        }

        /// <summary>
        /// Get/Set method of the FailurePercent field
        /// </summary>
        [DisplayName("Failure%")]
        public double FailurePercent
        {
            get
            {
                return failurePercent;
            }
            set
            {
                failurePercent = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TotalFailures field
        /// </summary>
        [DisplayName("TotalFailures")]
        public int TotalFailures
        {
            get
            {
                return totalFailures;
            }
            set
            {
                totalFailures = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TrxFailures field
        /// </summary>
        [DisplayName("TrxFailures")]
        public int TrxFailures
        {
            get
            {
                return trxFailures;
            }
            set
            {
                trxFailures = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TrxFailuresPercent field
        /// </summary>
        [DisplayName("TrxFailures%")]
        public double TrxFailuresPercent
        {
            get
            {
                return trxFailuresPercent;
            }
            set
            {
                trxFailuresPercent = value;
            }
        }

        /// <summary>
        /// Get/Set method of the RcvFailures field
        /// </summary>
        [DisplayName("RcvFailures")]
        public int RcvFailures
        {
            get
            {
                return rcvFailures;
            }
            set
            {
                rcvFailures = value;
            }
        }

        /// <summary>
        /// Get/Set method of the RcvFailuresPercent field
        /// </summary>
        [DisplayName("RcvFailures%")]
        public double RcvFailuresPercent
        {
            get
            {
                return rcvFailuresPercent;
            }
            set
            {
                rcvFailuresPercent = value;
            }
        }
    }
}
