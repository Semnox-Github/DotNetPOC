using Newtonsoft.Json;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Communication
{
    public class MessagingClientFactory
    {
        private static readonly logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private MessagingClientDTO messagingClientDTO;
        private ExecutionContext executionContext;
        private static MessagingClientFactory messagingClientFactory;
        public enum MessagingClients
        {
            /// <summary>
            /// No SMS Gateway.
            /// </summary>
            None,
            /// <summary>
            /// Generic SMS Gateway
            /// </summary>
            SMS,
            /// <summary>
            /// Email Gateway
            /// </summary>
            Email,
            /// <summary>
            /// Notification Gateway
            /// </summary>
            Notification,
            /// <summary>
            /// Aliyun SMS Gateway
            /// </summary>
            AliyunSMS,
            /// <summary>
            /// Twilio WhatsApp Gateway
            /// </summary>
            TwilioWhatsApp,
            /// <summary>
            /// Twilio SMS Gateway
            /// </summary>
            TwilioSMS,
            /// <summary>
            /// FireSMS Gateway
            /// </summary>
            FireSMS,
            /// <summary>
            /// EzagelSMS Gateway
            /// </summary>
            EzagelSMS,
        }

        private MessagingClientFactory()
        {
            log.LogMethodEntry();
            log.LogMethodExit(null);
        }
        public static MessagingClientFactory GetInstance(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            if (messagingClientFactory == null)
            {
                messagingClientFactory = new MessagingClientFactory(executionContext);
            }
            log.LogMethodExit(messagingClientFactory);
            return messagingClientFactory;
        }
        public MessagingClientFactory(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            //this.messagingClientDTO = messagingClientDTO;
            log.LogMethodExit(); ;
        }
        public MessagingClientBL GetMessageClient(int messageClientId)
        {
            Dictionary<int, MessagingClientBL> messageClientDictionary = new Dictionary<int, MessagingClientBL>();

            MessagingClients gateway;
            MessagingClientBL messageClient;
            if (messageClientDictionary.ContainsKey(messageClientId))
            {
                messageClient = messageClientDictionary[messageClientId];
                return messageClient;
            }
            MessagingClientDataHandler messageClientDataHandler = new MessagingClientDataHandler();
            MessagingClientDTO messageClientDTO = messageClientDataHandler.GetMessagingClient(messageClientId);
            Enum.TryParse<MessagingClients>(messageClientDTO.MessagingSubChannelType, out gateway);
            switch (gateway)
            {
                case MessagingClients.SMS:
                    {
                        Type type = Type.GetType("Semnox.Parafait.MessagingClients.SMS,MessagingClients");
                        object smsGateway = null;
                        if (type != null)
                        {
                            ConstructorInfo constructorN = type.GetConstructor(new Type[] { executionContext.GetType(), messageClientDTO.GetType()});
                            smsGateway = constructorN.Invoke(new object[] { executionContext, messageClientDTO });
                        }
                        else
                            throw new Exception(MessageContainerList.GetMessage(executionContext, 1479, "MessagingClients GenericSMS"));

                        messageClient = (MessagingClientBL)smsGateway;
                        break;
                    }
                case MessagingClients.Email:
                    {
                        Type type = Type.GetType("Semnox.Parafait.MessagingClients.Email,MessagingClients");
                        object smsGateway = null;
                        if (type != null)
                        {
                            ConstructorInfo constructorN = type.GetConstructor(new Type[] { executionContext.GetType(), messageClientDTO.GetType() });
                            smsGateway = constructorN.Invoke(new object[] { executionContext, messageClientDTO });
                        }
                        else
                            throw new Exception(MessageContainerList.GetMessage(executionContext, 1479, "MessagingClients GenericSMS"));

                        messageClient = (MessagingClientBL)smsGateway;
                        break;
                    }
                case MessagingClients.Notification:
                    {
                        Type type = Type.GetType("Semnox.Parafait.MessagingClients.Notification,MessagingClients");
                        object smsGateway = null;
                        if (type != null)
                        {
                            ConstructorInfo constructorN = type.GetConstructor(new Type[] { executionContext.GetType(), messageClientDTO.GetType() });
                            smsGateway = constructorN.Invoke(new object[] { executionContext, messageClientDTO });
                        }
                        else
                            throw new Exception(MessageContainerList.GetMessage(executionContext, 1479, "MessagingClients Notification"));

                        messageClient = (MessagingClientBL)smsGateway;
                        break;
                    }
                case MessagingClients.AliyunSMS:
                    {
                        Type type = Type.GetType("Semnox.Parafait.MessagingClients.AliyunSMS,MessagingClients");
                        object smsGateway = null;
                        if (type != null)
                        {
                            ConstructorInfo constructorN = type.GetConstructor(new Type[] { executionContext.GetType(), messageClientDTO.GetType() });
                            smsGateway = constructorN.Invoke(new object[] { executionContext, messageClientDTO });
                        }
                        else
                            throw new Exception(MessageContainerList.GetMessage(executionContext, 1479, "MessagingClients AliyunSMS"));

                        messageClient = (MessagingClientBL)smsGateway;
                        break;
                    }
                case MessagingClients.TwilioWhatsApp:
                    {
                       Type type = Type.GetType("Semnox.Parafait.MessagingClients.TwilioWhatsApp,MessagingClients");
                        object smsGateway = null;
                        if (type != null)
                        {
                            ConstructorInfo constructorN = type.GetConstructor(new Type[] { executionContext.GetType(), messageClientDTO.GetType() });
                            smsGateway = constructorN.Invoke(new object[] { executionContext, messageClientDTO });
                        }
                        else
                            throw new Exception(MessageContainerList.GetMessage(executionContext, 1479, "MessagingClients TwilioWhatsApp"));

                        messageClient = (MessagingClientBL)smsGateway;
                        break;
                    }
                case MessagingClients.TwilioSMS:
                    {
                        Type type = Type.GetType("Semnox.Parafait.MessagingClients.TwilioSMS,MessagingClients");
                        object smsGateway = null;
                        if (type != null)
                        {
                            ConstructorInfo constructorN = type.GetConstructor(new Type[] { executionContext.GetType(), messageClientDTO.GetType() });
                            smsGateway = constructorN.Invoke(new object[] { executionContext, messageClientDTO });
                        }
                        else
                            throw new Exception(MessageContainerList.GetMessage(executionContext, 1479, "MessagingClients TwilioSMS"));

                        messageClient = (MessagingClientBL)smsGateway;
                        break;
                    }
                case MessagingClients.FireSMS:
                    {
                        Type type = Type.GetType("Semnox.Parafait.MessagingClients.FireSMS,MessagingClients");
                        object smsGateway = null;
                        if (type != null)
                        {
                            ConstructorInfo constructorN = type.GetConstructor(new Type[] { executionContext.GetType(), messageClientDTO.GetType() });
                            smsGateway = constructorN.Invoke(new object[] { executionContext, messageClientDTO });
                        }
                        else
                            throw new Exception(MessageContainerList.GetMessage(executionContext, 1479, "MessagingClients FireSMS"));

                        messageClient = (MessagingClientBL)smsGateway;
                        break;
                    }
                case MessagingClients.EzagelSMS:
                    {
                        Type type = Type.GetType("Semnox.Parafait.MessagingClients.EzagelSMS,MessagingClients");
                        object smsGateway = null;
                        if (type != null)
                        {
                            ConstructorInfo constructorN = type.GetConstructor(new Type[] { executionContext.GetType(), messageClientDTO.GetType() });
                            smsGateway = constructorN.Invoke(new object[] { executionContext, messageClientDTO });
                        }
                        else
                            throw new Exception(MessageContainerList.GetMessage(executionContext, 1479, "MessagingClients EzagelSMS"));

                        messageClient = (MessagingClientBL)smsGateway;
                        break;
                    }
                default:
                    {
                        throw new Exception("Error: "+ MessageContainerList.GetMessage(executionContext, 16645));
                    }
            }
            messageClientDictionary.Add(messageClientId, messageClient);
            return messageClient;
        }
    }
}
