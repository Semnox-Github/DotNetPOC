/********************************************************************************************
 * Project Name - ClientBL 
 * Description  - Bussiness logic of the Client BL class
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By      Remarks          
 *********************************************************************************************
 *1.00        04-May-2016    Rakshith         Created 
 *2.70        19-Jun-2019    Nitin            Modified for guest app
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.ClientApp
{
    public class Client
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ClientDTO clientDTO;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Client()
        {
            log.Debug("Starts-Client() default constructor.");
            clientDTO = null;
            log.Debug("Ends-Client() default constructor.");
        }

        //Constructor Call Corresponding Data Hander besed id
        //And return Correspond Object
        //EX: "'Client"'  Request  ====>  ""Client"" DataHandler
        public Client(int clientId)  : this()
        {
            log.Debug("Starts-Client (clientId) parameterized constructor.");
            ClientDatahandler clientDatahandler = new ClientDatahandler();
            clientDTO = clientDatahandler.GetClientDTOById(clientId);
            log.Debug("Ends-Client (clientId) parameterized constructor.");
        }

        //Constructor Initialises with Corresponding Object
        public Client(ClientDTO clientDTO)  : this()
        {
            log.Debug("Starts-Client (clientDTO) Parameterized constructor.");
            this.clientDTO = clientDTO;
            log.Debug("Ends-Client (clientDTO) Parameterized constructor.");
        }


        /// <summary>
        /// Used For Save 
        /// It may by Insert Or Update
        /// </summary>
        public int Save()
        {
            ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
            ClientDatahandler clientDatahandler = new ClientDatahandler();
             
            try
            {
                if (clientDTO.ClientId <= 0)
                {
                    int programId = clientDatahandler.InsertClient(clientDTO, machineUserContext.GetUserId());
                    clientDTO.ClientId = programId;
                    return programId;
                }
                else
                {
                    if (clientDTO.IsChanged == true)
                    {
                        clientDatahandler.UpdateClient(clientDTO, machineUserContext.GetUserId());
                        clientDTO.AcceptChanges();
                    }
                    return 0;
                }
            }
            catch (Exception expn)
            {
                throw new System.Exception(expn.Message.ToString());
            }
        }

        /// <summary>
        /// Delete the ClientDTO based on Id
        /// </summary>
        public int Delete(int clientId)
        {
            try
            {
                ClientDatahandler clientDatahandler = new ClientDatahandler();
                return clientDatahandler.DeleteClient(clientId);
            }
            catch (Exception expn)
            {
                throw new System.Exception(expn.Message.ToString());
            }
        }

        /// <summary>
        /// gets the ClientDTO
        /// </summary>
        public ClientDTO GetClientDTO
        {
            get { return clientDTO; }
        }



        /// <summary>
        /// Validate the ClientDTO  based on token
        /// </summary>
        /// <param name="companyGuid">companyGuid</param>
        /// <returns>returns List<ClientValidationStruct> </returns>
        public List<ClientValidationStruct> ValidateClient(string token)
        {
            List<ClientValidationStruct> clientDetails = new List<ClientValidationStruct>();

            try
            {
                List<KeyValuePair<ClientDTO.SearchParameters, string>> searchClientParameter = new List<KeyValuePair<ClientDTO.SearchParameters, string>>();
                searchClientParameter.Add(new KeyValuePair<ClientDTO.SearchParameters, string>(ClientDTO.SearchParameters.SECURITY_TOKEN, token));

                List<ClientDTO> clientList = new ClientDatahandler().GetAllClients(searchClientParameter);

                if (clientList.Count == 1)
                {
                    foreach (ClientDTO clientDTO in clientList)
                    {
                        if (clientDTO.Active != true)
                        {
                            clientDetails.Add(new ClientValidationStruct("ERROR", "License is not Active!"));
                        }
                        else
                        {
                            clientDetails.Add(new ClientValidationStruct("CompanyName", clientDTO.CompanyName));
                            clientDetails.Add(new ClientValidationStruct("GatewayUrl", clientDTO.GatewayUrl));
                        }
                    }
                }
                else
                {
                    clientDetails.Add(new ClientValidationStruct("ERROR", "Security Token Invalid!"));
                }

            }
            catch
            {
                throw new Exception();
            }
            return clientDetails;
        }

        /// <summary>
        /// return the record from the database based on appId
        /// </summary>
        /// <param name="appId">clientId</param>
        /// <returns>return the ClientDTO object</returns>
        /// or null
        public void GetClientForGuestApp(String appId, string releaseNumber) 
        {
            log.LogMethodEntry(appId, releaseNumber);
            ClientDatahandler clientDatahandler = new ClientDatahandler();
            clientDTO = clientDatahandler.GetClientDTOByAppId(appId, releaseNumber);
            log.LogMethodExit(clientDTO);
        }

    }

    /// <summary>
    /// Class ClientList contains methods
    /// </summary>
    public class ClientList
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        /// <summary>
        /// Returns Search based on Parqameters And returns  List<ClientDTO>   
        /// </summary>
        public List<ClientDTO> GetAllClients(List<KeyValuePair<ClientDTO.SearchParameters, string>> searchParameters)
        {
            try
            {
                log.Debug("Starts-GetAllClients(searchParameters) method");
                ClientDatahandler clientDatahandler = new ClientDatahandler();
                log.Debug("Ends-GetAllClients(searchParameters) method by returning the result clientDatahandler.GetAllClients(searchParameters) call");
                return clientDatahandler.GetAllClients(searchParameters);
            }
            catch (Exception expn)
            {
                throw new System.Exception(expn.Message.ToString());
            }
        }
    }
}
