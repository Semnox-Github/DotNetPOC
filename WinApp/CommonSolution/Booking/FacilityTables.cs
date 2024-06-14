/********************************************************************************************
 * Project Name - Booking                                                                     
 * Description  - Business logic class for Facility Table
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.60         14-May-2019  Mushahid Faizan       Added Save method and FacilityTablesList class
 ********************************************************************************************/

using Semnox.Core.Utilities;
using Semnox.Parafait.POS;
using Semnox.Parafait.User;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Booking
{
    /// <summary>
    /// FacilityTables BL class
    /// </summary>
    public class FacilityTables
    {
        private static readonly Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private FacilityTableDTO facilityTableDTO;
        private ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public FacilityTables(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.facilityTableDTO = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="facilityTableDTO"></param>
        public FacilityTables(ExecutionContext executionContext, FacilityTableDTO facilityTableDTO)
        {
            log.LogMethodEntry(executionContext, facilityTableDTO);
            this.executionContext = executionContext;
            this.facilityTableDTO = facilityTableDTO;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the Table id as the parameter
        /// Would fetch the Facility Table object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">optional sql transaction</param>
        public FacilityTables(ExecutionContext executionContext, int id)
        {
            log.LogMethodEntry(id);
            this.executionContext = executionContext;
            FacilityTableDataHandler facilityTableDataHandler = new FacilityTableDataHandler();
            facilityTableDTO = facilityTableDataHandler.GetFacilityTableDTO(id);
            log.LogMethodExit();
        }
        /// <summary>
        /// Saves the Facility Tables
        /// Checks if the Table Id is not less or equal to 0 and Active is true
        /// If it is less than or equal to 0 and Active is true, then inserts
        /// If Active is False then it will be deleted
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            FacilityTableDataHandler facilityTableDataHandler = new FacilityTableDataHandler(sqlTransaction);
            if (facilityTableDTO.Active)
            {
                if (facilityTableDTO.TableId < 0)
                {
                    int id = facilityTableDataHandler.InsertFacilityTables(facilityTableDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    facilityTableDTO.TableId = id;
                }
                else
                {
                    if (facilityTableDTO.IsChanged)
                    {
                        facilityTableDataHandler.UpdateFacilityTables(facilityTableDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                        facilityTableDTO.AcceptChanges();
                    }
                }
            }
            else
            {
                if (facilityTableDTO.TableId >= 0)
                {
                    facilityTableDataHandler.DeleteFacilityTables(facilityTableDTO.TableId);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// GetTableStatus(string loginId, string macAddress, int facilityId, int siteId)
        /// </summary>
        /// <returns>list of FacilityTableDTO object </returns>
        public List<FacilityTableDTO> GetTableStatus(string loginId, string macAddress, int facilityId, int siteId)
        {
            log.LogMethodEntry();

            List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> searchParameters = new List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>>();
            searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.IP_ADDRESS, macAddress));

            //fetching pos machine DTO based on mac address passed. if not found it will throw an exception
            List<POSMachineDTO> POSMachineDTOList = new POSMachineList(executionContext).GetAllPOSMachines(searchParameters);
            //if (POSMachineDTOList.Count <= 0)
            //    throw new Exception("No POS machine found for " + macAddress + " MACAddress");
            if (POSMachineDTOList == null || POSMachineDTOList.Any() == false)
            {
                throw new Exception("No POS machine found for " + macAddress + " MACAddress");
            }
            //getting userDTO based on login id passed
            Users users = new Users(loginId);
            UsersDTO userDTO = users.GetUserDTO;

            //fetching default values set in management studio
            ParafaitDefaultsBL parafaitDefaultsBL = new ParafaitDefaultsBL(executionContext, "ENABLE_ORDER_SHARE_ACROSS_POS_COUNTERS");
            bool enableOrderShareAcrossPosCounters = ((parafaitDefaultsBL.ParafaitDefaultsDTO.DefaultValue.CompareTo("Y") == 0) ? true : false);

            parafaitDefaultsBL = new ParafaitDefaultsBL(executionContext, "ENABLE_ORDER_SHARE_ACROSS_USERS");
            bool enableOrderShareAcrossUsers = ((parafaitDefaultsBL.ParafaitDefaultsDTO.DefaultValue.CompareTo("Y") == 0) ? true : false);

            parafaitDefaultsBL = new ParafaitDefaultsBL(executionContext, "ENABLE_ORDER_SHARE_ACROSS_POS");
            bool enableOrderShareAcrossPOS = ((parafaitDefaultsBL.ParafaitDefaultsDTO.DefaultValue.CompareTo("Y") == 0) ? true : false);

            //creating the params class by setting fetched values
            FacilityTableParams facilityTableParams = new FacilityTableParams(facilityId, enableOrderShareAcrossPOS, enableOrderShareAcrossPosCounters, enableOrderShareAcrossUsers, POSMachineDTOList[0], userDTO);

            FacilityTableDataHandler facilityTableDataHandler = new FacilityTableDataHandler();

            log.LogMethodExit();

            return facilityTableDataHandler.GetTableStatus(facilityTableParams);
        }

        public List<FacilityTableDTO> GetOpenFacilityTableDTOList(List<KeyValuePair<FacilityTableDTO.SearchByParameters, string>> searchParameters,
                                                              int POSTypeId,
                                                              string POSMachineName,
                                                              ExecutionContext executionContext)
        {
            log.LogMethodEntry(searchParameters, POSTypeId, POSMachineName);
            bool enableOrderShareAcrossPOSCounters = ParafaitDefaultContainer.GetParafaitDefault<bool>(executionContext, "ENABLE_ORDER_SHARE_ACROSS_POS_COUNTERS");
            bool enableOrderShareAcrossUsers = ParafaitDefaultContainer.GetParafaitDefault<bool>(executionContext, "ENABLE_ORDER_SHARE_ACROSS_USERS");
            bool enableOrderShareAcrossPOS = ParafaitDefaultContainer.GetParafaitDefault<bool>(executionContext, "ENABLE_ORDER_SHARE_ACROSS_POS");
            FacilityTableDataHandler facilityTableDataHandler = new FacilityTableDataHandler();
            List<FacilityTableDTO> facilityTableDTOList = facilityTableDataHandler.GetOpenOrderFacilityTableDTOList(searchParameters,
                                                                                                       POSTypeId,
                                                                                                       executionContext.GetUserPKId(),
                                                                                                       executionContext.GetMachineId(),
                                                                                                       POSMachineName,
                                                                                                       enableOrderShareAcrossPOSCounters,
                                                                                                       enableOrderShareAcrossUsers,
                                                                                                       enableOrderShareAcrossPOS);
            log.LogMethodExit(facilityTableDTOList);
            return facilityTableDTOList;
        }

        /// <summary>
        ///  Gets the DTO
        /// </summary>
        public FacilityTableDTO FacilityTableDTO
        {
            get
            {
                return facilityTableDTO;
            }
        }
    }
    /// <summary>
    /// Manages the list of FacilityTables List
    /// </summary>
    public class FacilityTablesList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<FacilityTableDTO> facilityTableList;

        /// <summary>
        /// Parameterized Constructor having Execution Context
        /// </summary>
        public FacilityTablesList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.facilityTableList = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="facilityTableList"></param>
        /// <param name="executionContext"></param>
        public FacilityTablesList(ExecutionContext executionContext, List<FacilityTableDTO> facilityTableList)
        {
            log.LogMethodEntry(executionContext, facilityTableList);
            this.facilityTableList = facilityTableList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the FacilityTable list
        /// </summary>
        public List<FacilityTableDTO> GetAllFacilityTableList(List<KeyValuePair<FacilityTableDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            FacilityTableDataHandler facilityTableDataHandler = new FacilityTableDataHandler();
            log.LogMethodExit();
            return facilityTableDataHandler.GetAllFacilityTableLayout(searchParameters);
        }

        /// <summary>
        /// This method should be used to Save and Update the Facility Tables details for Web Management Studio.
        /// </summary>
        public void SaveUpdateFacilityTablesList()
        {
            log.LogMethodEntry();
            try
            {
                if (facilityTableList != null && facilityTableList.Any())
                {
                    foreach (FacilityTableDTO facilityTableDTO in facilityTableList)
                    {
                        FacilityTables facilityTables = new FacilityTables(executionContext, facilityTableDTO);
                        facilityTables.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }
    }
}

