/********************************************************************************************
 * Project Name - POS
 * Description  - Data object of POSMachineViewDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.70        3- Jul- 2019  Girish Kundar       Modified : Added Constructor with required Parameter
 ********************************************************************************************/
using System;
using System.ComponentModel;
using System.Text;

namespace Semnox.Parafait.POS
{
    /// <summary>
    /// This is the POSMachineViewDTO data object class. This acts as data holder for the POSMachine business object
    /// </summary>
    public class POSMachineViewDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int posMachineId;
        private string posName;

        /// <summary>
        /// Default constructor
        /// </summary>
        public POSMachineViewDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public POSMachineViewDTO(int posMachineId, string posName)
            : this()
        {
            log.LogMethodEntry(posMachineId, posName);
            this.posMachineId = posMachineId;
            this.posName = posName;
            log.LogMethodExit();
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
        /// Get/Set method of the posName field
        /// </summary>
        public string POSName
        {
            get
            {
                return posName;
            }

            set
            {
                posName = value;
            }
        }

    }
}
