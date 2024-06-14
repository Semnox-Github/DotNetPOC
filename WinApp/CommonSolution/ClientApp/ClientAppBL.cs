/********************************************************************************************
 * Project Name - ClientAppBL
 * Description  - Business Login of Client App entity created for Dashboards
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *2.110       20-Dec-2020   Nitin Pai         Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.ClientApp
{
    public class ClientAppBL
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ClientAppDTO clientAppDTO;
        private ExecutionContext executionContext;
        /// <summary>
        /// Default constructor
        /// </summary>
        public ClientAppBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);            
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="clientAppId"></param>
        /// <param name="sqlTransaction"></param>
        public ClientAppBL(ExecutionContext executionContext,int clientAppId,SqlTransaction sqlTransaction=null) 
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext,clientAppId, sqlTransaction);
            ClientAppDataHandler clientAppDatahandler = new ClientAppDataHandler(sqlTransaction);
            this.clientAppDTO = clientAppDatahandler.GetClientAppDTOById(clientAppId);
            if (this.clientAppDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "ClientApp", clientAppId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="clientAppDTO"></param>
        public ClientAppBL(ExecutionContext executionContext, ClientAppDTO clientAppDTO) 
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, clientAppDTO);
            this.clientAppDTO = clientAppDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Used For Save 
        /// It may by Insert Or Update
        /// </summary>
        public void Save(SqlTransaction sqlTransaction=null)
        {
            ClientAppDataHandler clientAppDataHandler = new ClientAppDataHandler(sqlTransaction);
            if (clientAppDTO.ClientAppId <= 0)
            {
                clientAppDTO = clientAppDataHandler.InsertClientAppVersionMapping(clientAppDTO, executionContext.GetUserId());
                clientAppDTO.AcceptChanges();
            }
            else
            {
                if (clientAppDTO.IsChanged == true)
                {
                    clientAppDataHandler.UpdateClientAppVersionMapping(clientAppDTO, executionContext.GetUserId());
                    clientAppDTO.AcceptChanges();
                }

            }
            log.LogMethodExit();

        }

        /// <summary>
        /// gets the clientAppDTO
        /// </summary>
        public ClientAppDTO GetClientAppDTO
        {
            get { return clientAppDTO; }
        }
    }

    public class ClientAppListBL
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        
        /// <summary>
        /// Returns Search based on Parqameters And returns  List<ClientAppDTO>   
        /// </summary>
        /// 
        public ClientAppListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns Search based on Parqameters And returns  List<ClientAppDTO>   
        /// </summary>
        /// 
        public List<ClientAppDTO> GetAllClientApp(List<KeyValuePair<ClientAppDTO.SearchParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            ClientAppDataHandler clientAppUserDatahandler = new ClientAppDataHandler(sqlTransaction);
            List<ClientAppDTO> clientAppDTOList = clientAppUserDatahandler.GetAllClientAppDTO(searchParameters);
            log.LogMethodExit(clientAppDTOList);
            return clientAppDTOList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientAppUserDTOList"></param>
        /// <param name="sqlTransaction"></param>
        public void Save(List<ClientAppUserDTO> clientAppUserDTOList , SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(clientAppUserDTOList, sqlTransaction);

            foreach (ClientAppUserDTO clientAppUserDTO in clientAppUserDTOList)
            {
                ClientAppUser clientAppUser = new ClientAppUser(executionContext, clientAppUserDTO);
                clientAppUser.Save(sqlTransaction);
            }
            log.LogMethodExit();
        }
    }
}
