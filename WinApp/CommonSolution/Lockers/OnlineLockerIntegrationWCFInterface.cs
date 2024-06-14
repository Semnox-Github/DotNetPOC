using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Semnox.Parafait.Device.Lockers
{
    [ServiceContract]
    [XmlSerializerFormat]
    
    public interface IOnlineLockerIntegrationInterface
    {        
        /// <summary>
        /// Method which recieve the command from client POS
        /// </summary>
        /// <param name="requestType">Enum RequestType</param>
        /// <param name="lockerIdList">This function holds locker id list based on the request type</param>
        /// <param name="cardNoList">This function holds card number list based on the request type</param>
        /// <returns>True if success else false</returns>
        [OperationContract]
        bool SendCommand(RequestType requestType, List<string> lockerIdList, List<string> cardNoList);
    }
}
