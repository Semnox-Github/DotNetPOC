using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.MessagingClients
{
    class Notification : MessagingClientBL
    {

        string toPhone;
        private MessagingClientDTO messagingClientDTO;
        private ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Notification(ExecutionContext executionContext, MessagingClientDTO messagingClientDTO) : base(executionContext, messagingClientDTO)
        {
            log.LogMethodEntry(executionContext, messagingClientDTO);
            this.executionContext = executionContext;
            this.messagingClientDTO = messagingClientDTO;
            
            log.LogMethodExit(null);
        }

        public override MessagingRequestDTO Send(MessagingRequestDTO messagingRequestDTO)
        {
            log.LogMethodEntry(messagingRequestDTO);

            string returnValueNew = string.Empty;

            AppNotifications appNotifications = new AppNotifications(executionContext);
            try
            {
                returnValueNew = appNotifications.SendMessage(messagingRequestDTO);
                messagingRequestDTO.SendDate = ServerDateTime.Now;
                base.UpdateResults(messagingRequestDTO, "Success", returnValueNew);

            }
            catch(Exception ex)
            {
                base.UpdateResults(messagingRequestDTO, "Error", ex.Message);
            }

            
            log.LogMethodExit(messagingRequestDTO);
            return messagingRequestDTO;
        }

    }
}
