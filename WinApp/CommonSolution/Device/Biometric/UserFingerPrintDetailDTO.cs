/********************************************************************************************
* Project Name - UserFingerPrintDetailDTO
* Description  - 
* 
**************
**Version Log
**************
*Version     Date          Modified By        Remarks          
*********************************************************************************************
*2.80        09-Mar-2020   Indrajeet Kumar    Created 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.Biometric
{
    public class UserFingerPrintDetailDTO
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int userId;
        private int fingerNumber;
        private byte[] fpTemplate;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public UserFingerPrintDetailDTO()
        {
            log.LogMethodEntry();
            this.userId = -1;
            this.fingerNumber = -1;
            this.fpTemplate = null;
            log.LogMethodExit();
        }

        public UserFingerPrintDetailDTO(int userId, int fingerNumber, byte[] fpTemplate)
        {
            log.LogMethodEntry(userId, fingerNumber, fpTemplate);
            this.userId = userId;
            this.fingerNumber = fingerNumber;
            this.fpTemplate = fpTemplate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the UserId field
        /// </summary>
        public int UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        /// <summary>
        /// Get/Set method of the FingerNumber field
        /// </summary>
        public int FingerNumber
        {
            get { return fingerNumber; }
            set { fingerNumber = value; }
        }

        /// <summary>
        /// Get/Set method of the FPTemplate field
        /// </summary>
        public byte[] FPTemplate
        {
            get { return fpTemplate; }
            set { fpTemplate = value; }
        }
    }
}
