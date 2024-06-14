/********************************************************************************************
 * Project Name - POS
 * Description  - Data object of POSPaymentModeInclusionContainerDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By         Remarks          
 *********************************************************************************************
 *2.120.0     18- Jun- 2021   Prajwal             Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.POS
{
    public class POSPaymentModeInclusionContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int posPaymentModeInclusionId;
        private int posMachineId;
        private int paymentModeId;
        private string friendlyName;

        /// <summary>
        /// Default constructor
        /// </summary>
        public POSPaymentModeInclusionContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public POSPaymentModeInclusionContainerDTO(int posPaymentModeInclusionId, int posMachineId, int paymentModeId, string friendlyName)
            : this()
        {
            log.LogMethodEntry(posPaymentModeInclusionId, posMachineId, paymentModeId, friendlyName);
            this.posPaymentModeInclusionId = posPaymentModeInclusionId;
            this.posMachineId = posMachineId;
            this.paymentModeId = paymentModeId;
            this.friendlyName = friendlyName;
            log.LogMethodExit();
        }
      
        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        public int POSPaymentModeInclusionId
        {
            get { return posPaymentModeInclusionId; }
            set { posPaymentModeInclusionId = value; }
        }

        public int POSMachineId
        {
            get { return posMachineId; }
            set { posMachineId = value; }
        }

        public int PaymentModeId
        {
            get { return paymentModeId; }
            set { paymentModeId = value; }
        }

        /// <summary>
        /// Get/Set method of the FriendlyName field
        /// </summary>
        public string FriendlyName
        {
            get { return friendlyName; }
            set { friendlyName = value; }
        }
    }
}
