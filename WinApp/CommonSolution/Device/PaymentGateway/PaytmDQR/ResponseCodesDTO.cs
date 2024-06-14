/******************************************************************************************************
 * Project Name - Device
 * Description  - PayTMDQR Payment gateway
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By    Remarks          
 ******************************************************************************************************
 *2.140.5     11-Jan-2023    Prasad & Fiona   PayTMDQR Payment gateway integration
 ********************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class ResponseCodesDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string code;
        private string status;
        private string message;
        private bool isSuccess;

        public ResponseCodesDTO(string code, string status, string message, bool isSuccess)
        {
            log.LogMethodEntry();
            this.code = code;
            this.status = status;
            this.message = message;
            this.isSuccess = isSuccess;
            log.LogMethodExit();
        }

        public bool IsSuccess
        {
            get { return isSuccess; }
            set { isSuccess = value; }
        }


        public string Message
        {
            get { return message; }
            set { message = value; }
        }


        public string Status
        {
            get { return status; }
            set { status = value; }
        }


        public string Code
        {
            get { return code; }
            set { code = value; }
        }

    }
}
