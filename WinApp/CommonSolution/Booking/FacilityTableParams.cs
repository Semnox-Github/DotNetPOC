/********************************************************************************************
 * Project Name - Achievement
 * Description  - Player Achievement Details
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.2        13-Aug-2019     Deeksha                 Added logger methods.
 ********************************************************************************************/
using Semnox.Parafait.POS;
using Semnox.Parafait.User;
using System.ComponentModel;
namespace Semnox.Parafait.Booking
{
    public class FacilityTableParams
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);//Added on 26 Feb 2019 by Akshay Gulaganji

        private int facilityId;
        private bool enableOrderShareAcrossPosCounters;
        private bool enableOrderShareAcrossUsers;
        private bool enableOrderShareAcrossPOS;
        private POSMachineDTO posMachine;
        private UsersDTO userDTO;

        /// <summary>
        /// Default constructor
        /// </summary>
        public FacilityTableParams()
        {
            log.LogMethodEntry();
            this.facilityId = -1;
            this.enableOrderShareAcrossPOS = false;
            this.enableOrderShareAcrossPosCounters = false;
            this.enableOrderShareAcrossUsers = false;
            this.posMachine = null;
            this.userDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// parameterised constructor
        /// </summary>
        public FacilityTableParams(int facilityId, bool enableOrderShareAcrossPOS, bool enableOrderShareAcrossPosCounters, bool enableOrderShareAcrossUsers, POSMachineDTO posMachine, UsersDTO userDTO)
            :this()
        {
            log.LogMethodEntry(facilityId, enableOrderShareAcrossPOS, enableOrderShareAcrossPosCounters, enableOrderShareAcrossUsers, posMachine, userDTO);
            this.facilityId = facilityId;
            this.enableOrderShareAcrossPOS = enableOrderShareAcrossPOS;
            this.enableOrderShareAcrossPosCounters = enableOrderShareAcrossPosCounters;
            this.enableOrderShareAcrossUsers = enableOrderShareAcrossUsers;
            this.posMachine = posMachine;
            this.userDTO = userDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the FacilityId field
        /// </summary>
        [DisplayName("FacilityId")]
        [DefaultValue(-1)]
        public int FacilityId { get { return facilityId; } set { facilityId = value; } }

        /// <summary>
        /// Get/Set method of the EnableOrderShareAcrossPOS field
        /// </summary>
        [DisplayName("EnableOrderShareAcrossPOS")]
        [DefaultValue(false)]
        public bool EnableOrderShareAcrossPOS { get { return enableOrderShareAcrossPOS; } set { enableOrderShareAcrossPOS = value; } }

        /// <summary>
        /// Get/Set method of the EnableOrderShareAcrossPosCounters field
        /// </summary>
        [DisplayName("EnableOrderShareAcrossPosCounters")]
        [DefaultValue(false)]
        public bool EnableOrderShareAcrossPosCounters { get { return enableOrderShareAcrossPosCounters; } set { enableOrderShareAcrossPosCounters = value; } }

        /// <summary>
        /// Get/Set method of the EnableOrderShareAcrossUsers field
        /// </summary>
        [DisplayName("EnableOrderShareAcrossUsers")]
        [DefaultValue(false)]
        public bool EnableOrderShareAcrossUsers { get { return enableOrderShareAcrossUsers; } set { enableOrderShareAcrossUsers = value; } }

        /// <summary>
        /// Get/Set method of the POSMachine field
        /// </summary>
        [DisplayName("POSMachine")]
        public POSMachineDTO POSMachine { get { return posMachine; } set { posMachine = value; } }

        /// <summary>
        /// Get/Set method of the UserDTO field
        /// </summary>
        [DisplayName("UserDTO")]
        public UsersDTO UserDTO { get { return userDTO; } set { userDTO = value; } }

    }
}
