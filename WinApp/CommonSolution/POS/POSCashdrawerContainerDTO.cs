/********************************************************************************************
 * Project Name - POS
 * Description  - Data object of POSCashdrawerContainerDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By         Remarks          
 *********************************************************************************************
 *2.140.0    3-Sep-2021       Girish Kundar       Created : Multicashdrawer enhancement
 ********************************************************************************************/

using System.Collections.Generic;
using Semnox.Parafait.Device.Peripherals;

namespace Semnox.Parafait.POS
{
    /// <summary>
    /// This is the POSCashdrawerContainerDTO data object class. This acts as data holder for the POSCahsdrawer business object
    /// </summary>
    public class POSCashdrawerContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int posCashdrawerId;
        private int posMachineId;
        private int cashdrawerId;
        private bool isActive;
       
        /// <summary>
        /// Default constructor
        /// </summary>
        public POSCashdrawerContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public POSCashdrawerContainerDTO(int posCashdrawerId, int posMachineId, int cashdrawerId, bool isActive)
            : this()
        {
            log.LogMethodEntry(posCashdrawerId, posMachineId, cashdrawerId, isActive);
            this.posCashdrawerId = posCashdrawerId;
            this.posMachineId = posMachineId;
            this.cashdrawerId = cashdrawerId;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the posCashdrawerId field
        /// </summary>
        public int POSCashdrawerId
        {
            get
            {
                return posCashdrawerId;
            }

            set
            {
                posCashdrawerId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the POSMachineId field
        /// </summary>
        public int POSMachineId
        {
            get
            {
                return posMachineId;
            }

            set
            {
                posMachineId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CashdrawerId field
        /// </summary>
        public int CashdrawerId
        {
            get
            {
                return cashdrawerId;
            }

            set
            {
                cashdrawerId = value;
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
                isActive = value;
            }
        }



    }
}
