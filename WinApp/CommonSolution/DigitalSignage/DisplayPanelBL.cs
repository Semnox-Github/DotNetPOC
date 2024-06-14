/********************************************************************************************
 * Project Name - DisplayPanel BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
 *1.00        06-Mar-2017      Lakshminarayana     Created 
 *2.40        28-Sep-2018      Jagan Mohan         Added new constructor DisplayPanelBL, DisplayPanelListBL and
 *                                                 DisplayPanelListOperations
 *2.70.2        30-Jul-2019      Dakshakh raj        Modified : Save() method Insert/Update method returns DTO.
 *2.90        28-Jul-2020      Mushahid Faizan     Modified : 3 tier changes for Rest API.
 *2.110.0    25-Nov-2020       Prajwal S          Modified : DisplayPanelBL(ExecutionContext executionContext, int id, SqlTransaction sqltransaction = null)
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    /// Business logic for DisplayPanel class.
    /// </summary>
    public class DisplayPanelBL
    {
        private DisplayPanelDTO displayPanelDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        ///  Default constructor of DisplayPanelBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private DisplayPanelBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the displayPanel id as the parameter would fetch the displayPanel object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="id">id</param>
        /// <param name="sqltransaction">sqltransaction</param>
        public DisplayPanelBL(ExecutionContext executionContext, int id, SqlTransaction sqltransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqltransaction);
            DisplayPanelDataHandler displayPanelDataHandler = new DisplayPanelDataHandler(sqltransaction);
            displayPanelDTO = displayPanelDataHandler.GetDisplayPanelDTO(id);
            if (displayPanelDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "DisplayPanel", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodEntry(displayPanelDTO);
        }

        /// <summary>
        /// Creates DisplayPanelBL object using the DisplayPanelDTO
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="displayPanelDTO">displayPanelDTO</param>
        public DisplayPanelBL(ExecutionContext executionContext, DisplayPanelDTO displayPanelDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, displayPanelDTO);
            this.displayPanelDTO = displayPanelDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// / Saves the DisplayPanel
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqltransaction">sqltransaction</param>
        public void Save(SqlTransaction sqltransaction = null)
        {
            log.LogMethodEntry(sqltransaction);
            DisplayPanelDataHandler displayPanelDataHandler = new DisplayPanelDataHandler(sqltransaction);
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (displayPanelDTO.PanelId < 0)
            {
                displayPanelDTO = displayPanelDataHandler.InsertDisplayPanel(displayPanelDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                displayPanelDTO.AcceptChanges();
            }
            else
            {
                if (displayPanelDTO.IsChanged)
                {
                    displayPanelDTO = displayPanelDataHandler.UpdateDisplayPanel(displayPanelDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    displayPanelDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            string errorMessage = string.Empty;
            if (displayPanelDTO == null)
            {
                //Validation to be implemented.
            }

            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public DisplayPanelDTO DisplayPanelDTO
        {
            get
            {
                return displayPanelDTO;
            }
        }

        /// <summary>
        /// Starts the remote PC.
        /// </summary>
        /// <returns>bool value</returns>
        public bool StartPC()
        {
            log.LogMethodEntry();
            bool success = false;
            if (displayPanelDTO != null && !string.IsNullOrEmpty(displayPanelDTO.MACAddress))
            {
                Regex regex = new Regex("^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$");
                if (regex.IsMatch(displayPanelDTO.MACAddress))
                {
                    string macAddress = displayPanelDTO.MACAddress.Replace(":", "").Replace("-", "");
                    WOLClass client = new WOLClass();
                    client.Connect(new
                       IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
                       0x2fff); // port=12287 let's use this one 
                    client.SetClientToBrodcastMode();
                    //set sending bites
                    int counter = 0;
                    //buffer to be send
                    byte[] bytes = new byte[1024];   // more than enough :-)
                                                     //first 6 bytes should be 0xFF
                    for (int y = 0; y < 6; y++)
                        bytes[counter++] = 0xFF;
                    //now repeate MAC 16 times
                    for (int y = 0; y < 16; y++)
                    {
                        int i = 0;
                        for (int z = 0; z < 6; z++)
                        {
                            bytes[counter++] =
                                byte.Parse(macAddress.Substring(i, 2),
                                NumberStyles.HexNumber);
                            i += 2;
                        }
                    }

                    //now send wake up packet
                    try
                    {
                        client.Send(bytes, 1024);
                        success = true;
                    }
                    catch (Exception ex)
                    {
                        success = false;
                        log.Error("Error while sending wake request to device", ex); 
                    }
                }
            }
            log.LogMethodExit(success);
            return success;
        }

        /// <summary>
        /// Shuts down the remote PC.
        /// </summary>
        /// <returns>bool value</returns>
        public bool ShutdownPC()
        {
            log.LogMethodEntry();
            bool success = false;
            try
            {
                string computername = Convert.ToString(displayPanelDTO.PCName);
                string command = "";
                string shutdowninsec = "30";
                if (displayPanelDTO.ShutdownSec != null && displayPanelDTO.ShutdownSec > 0)
                {
                    shutdowninsec = Convert.ToString(displayPanelDTO.ShutdownSec);
                }
                Process process = new System.Diagnostics.Process();
                ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.WorkingDirectory = @"C:\windows\system32\";
                startInfo.FileName = "cmd.exe";
                startInfo.CreateNoWindow = true;
                command = "shutdown -s -f -t " + shutdowninsec + " -m \\\\" + computername + "";
                //command = "shutdown -s -f -t 00 -m \\\\" + computername + "";
                startInfo.Arguments = "/user:Administrator \"cmd /K " + command + "\"";
                process.StartInfo = startInfo;
                process.Start();
                success = true;
                //process.WaitForExit(30*1000);
            }
            catch (Exception ex)
            {
                success = false;
                log.Error("Error while shutting down PC: " + displayPanelDTO.PCName, ex); 
            }
            log.LogMethodExit(success);
            return success;
        }
    }

    /// <summary>
    /// Used for starting the remote PC.
    /// </summary>
    class WOLClass : UdpClient
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public WOLClass()
            : base()
        {
        }
        //this is needed to send broadcast packet
        public void SetClientToBrodcastMode()
        {
            if (this.Active)
                this.Client.SetSocketOption(SocketOptionLevel.Socket,
                                          SocketOptionName.Broadcast, 0);
        }
    }

    /// <summary>
    /// Manages the list of DisplayPanel
    /// </summary>
    public class DisplayPanelListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<DisplayPanelDTO> displayPanelDTOList;
        private ExecutionContext executionContext;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DisplayPanelListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Display Panel List BL
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public DisplayPanelListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.displayPanelDTOList = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="displayPanelList">displayPanelList</param>
        /// <param name="executionContext">executionContext</param>
        public DisplayPanelListBL(ExecutionContext executionContext, List<DisplayPanelDTO> displayPanelList) : this(executionContext)
        {
            log.LogMethodEntry(displayPanelList, executionContext);
            this.displayPanelDTOList = displayPanelList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the DisplayPanel list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<DisplayPanelDTO> GetDisplayPanelDTOList(List<KeyValuePair<DisplayPanelDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            DisplayPanelDataHandler displayPanelDataHandler = new DisplayPanelDataHandler(sqlTransaction);
            List<DisplayPanelDTO> displayPanelList = displayPanelDataHandler.GetDisplayPanelDTOList(searchParameters);
            log.LogMethodExit(displayPanelList);
            return displayPanelList;
        }

        /// <summary>
        /// Saves the  list of displayPanelDTO.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (displayPanelDTOList == null ||
                displayPanelDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < displayPanelDTOList.Count; i++)
            {
                var displayPanelDTO = displayPanelDTOList[i];
                if (displayPanelDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    DisplayPanelBL displayPanelBL = new DisplayPanelBL(executionContext, displayPanelDTO);
                    displayPanelBL.Save();
                }
                catch (SqlException sqlEx)
                {
                    log.Error(sqlEx);
                    log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                    if (sqlEx.Number == 547)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving displayPanelDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("displayPanelDTO", displayPanelDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Save start PC
        /// </summary>
        public void DisplayPanelListStartPC()
        {
            try
            {
                log.LogMethodEntry();
                foreach (DisplayPanelDTO displayPanelDTO in displayPanelDTOList)
                {
                    DisplayPanelBL displayPanelBL = new DisplayPanelBL(executionContext, displayPanelDTO);
                    displayPanelBL.StartPC();
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Save shutdown pc
        /// </summary>
        public void DisplayPanelListShutdownPC()
        {
            try
            {
                log.LogMethodEntry();
                foreach (DisplayPanelDTO displayPanelDTO in displayPanelDTOList)
                {
                    DisplayPanelBL displayPanelBL = new DisplayPanelBL(executionContext, displayPanelDTO);
                    displayPanelBL.ShutdownPC();
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Save restart pc
        /// </summary>
        public void DisplayPanelListRestartPC()
        {
            try
            {
                log.LogMethodEntry();
                foreach (DisplayPanelDTO displayPanelDTO in displayPanelDTOList)
                {
                    DisplayPanelBL displayPanelBL = new DisplayPanelBL(executionContext, displayPanelDTO);
                    displayPanelDTO.RestartFlag = "Y";
                    displayPanelBL.Save();
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }

        public DateTime? GetDisplayPanelModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            DisplayPanelDataHandler displayPanelDataHandler = new DisplayPanelDataHandler(null);
            DateTime? result = displayPanelDataHandler.GetDisplayPanelModuleLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
