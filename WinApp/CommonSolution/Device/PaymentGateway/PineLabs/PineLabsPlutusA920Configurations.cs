/******************************************************************************************************
 * Project Name - Device
 * Description  - PineLabs Payment gateway
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By    Remarks          
 ******************************************************************************************************
 *2.140.5     11-Jan-2023    Prasad & Fiona   PineLabs Payment gateway integration
 ********************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway
{
    internal class PineLabsPlutusA920Configurations
    {
        public string MerchantId { get; set; }
        public string SecurityToken { get; set; }
        public string API_URL { get; set; }
        public int GetPaymentPollingTimeoutInSeconds { get; set; }
    }
}
