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

using System.ComponentModel;
namespace Semnox.Parafait.Product
{
    public class FacilityTableParams
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);//Added on 26 Feb 2019 by Akshay Gulaganji

        private int facilityId;
        private bool enableOrderShareAcrossPosCounters;
        private bool enableOrderShareAcrossUsers;
        private bool enableOrderShareAcrossPOS;
        private int posMachineId;
        private int posTypeId;
        private string posMachineName;
        private string loginId;

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
            this.loginId = string.Empty;
            posMachineId = -1;
            posTypeId = -1;
            posMachineName = string.Empty;
            log.LogMethodExit();
        }

        /// <summary>
        /// parameterised constructor
        /// </summary>
        public FacilityTableParams(int facilityId, bool enableOrderShareAcrossPOS, bool enableOrderShareAcrossPosCounters,
                     bool enableOrderShareAcrossUsers, int posMachineId, int posTypeId, string posMachineName, string loginId)
            :this()
        {
            log.LogMethodEntry(facilityId, enableOrderShareAcrossPOS, enableOrderShareAcrossPosCounters,
                           enableOrderShareAcrossUsers, posMachineId, posTypeId, posMachineName, loginId);
            this.facilityId = facilityId;
            this.enableOrderShareAcrossPOS = enableOrderShareAcrossPOS;
            this.enableOrderShareAcrossPosCounters = enableOrderShareAcrossPosCounters;
            this.enableOrderShareAcrossUsers = enableOrderShareAcrossUsers;
            this.posMachineId = posMachineId;
            this.posTypeId = posTypeId;
            this.loginId = loginId;
            this.posMachineName = posMachineName;
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
        /// Get/Set method of the POSMachineId field
        /// </summary>
        public int POSMachineId { get { return posMachineId; } set { posMachineId = value; } }
        /// <summary>
        /// Get/Set method of the POS type Id field
        /// </summary>
        public int POSTypeId { get { return posTypeId; } set { posTypeId = value; } }

        /// <summary>
        /// Get/Set method of the POS machine name field
        /// </summary>
        public string POSMachineName { get { return posMachineName; } set { posMachineName = value; } }

        /// <summary>
        /// Get/Set method of the UserDTO field
        /// </summary>
        [DisplayName("LoginId")]
        public string LoginId { get { return loginId; } set { loginId = value; } }

    }
}
