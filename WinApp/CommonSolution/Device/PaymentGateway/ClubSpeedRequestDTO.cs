using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway
{
    internal class ClubSpeedRequestDTO
    {
        public string cardId { get; set; }
        public double payAmount { get; set; }
        public double balance { get; set; }
        public string sessionId { get; set; }
    }
}
