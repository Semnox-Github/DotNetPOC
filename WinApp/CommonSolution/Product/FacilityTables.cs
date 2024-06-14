/********************************************************************************************
 * Project Name - Booking                                                                     
 * Description  - Business logic class for Facility Table
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.60         14-May-2019  Mushahid Faizan       Added Save method and FacilityTablesList class
 *2.140.0      30-Nov-2021  Abhishek              WMS issue fix : modified save to handle active and inactive of tables
 ********************************************************************************************/

using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
//using Semnox.Parafait.Booking;
//using Semnox.Parafait.POS;
using Semnox.Parafait.Product;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Product
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
            if (facilityTableDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, MessageContainerList.GetMessage(executionContext,"Facility Table"), id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Saves the Facility Tables
        /// Checks if the Table Id is not less or equal to 0 and Active is true
        /// If it is less than or equal to 0 and Active is true, then inserts
        /// If Active is False then it will be deleted
        /// else updates
        /// </summary>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            FacilityTableDataHandler facilityTableDataHandler = new FacilityTableDataHandler(sqlTransaction);
            if (facilityTableDTO.TableId < 0)
            {
                facilityTableDTO = facilityTableDataHandler.InsertFacilityTables(facilityTableDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                facilityTableDTO.AcceptChanges();
            }
            else
            {
                if (facilityTableDTO.IsChanged)
                {
                    facilityTableDTO = facilityTableDataHandler.UpdateFacilityTables(facilityTableDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    facilityTableDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the facilityTableDTO   
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            if (facilityTableDTO.FacilityId < 0)
            {
                validationErrorList.Add(new ValidationError("facilityTable", "FacilityId", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Facility"))));
            }
            if (string.IsNullOrEmpty(facilityTableDTO.TableName))
            {
                validationErrorList.Add(new ValidationError("facilityTable", "TableName", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Table Name"))));
            }
            if (!string.IsNullOrWhiteSpace(facilityTableDTO.TableName) && facilityTableDTO.TableName.Length > 100)
            {
                validationErrorList.Add(new ValidationError("facilityTable", "TableName", MessageContainerList.GetMessage(executionContext, 2197, MessageContainerList.GetMessage(executionContext, "Table Name"), 100)));
            }
            if (!string.IsNullOrWhiteSpace(facilityTableDTO.InterfaceInfo1) && facilityTableDTO.InterfaceInfo1.Length > 50)
            {
                validationErrorList.Add(new ValidationError("facilityTable", "InterfaceInfo", MessageContainerList.GetMessage(executionContext, 2197, MessageContainerList.GetMessage(executionContext, "InterfaceInfo1"), 50)));
            }
            if (!string.IsNullOrWhiteSpace(facilityTableDTO.InterfaceInfo2) && facilityTableDTO.InterfaceInfo2.Length > 50)
            {
                validationErrorList.Add(new ValidationError("facilityTable", "InterfaceInfo", MessageContainerList.GetMessage(executionContext, 2197, MessageContainerList.GetMessage(executionContext, "InterfaceInfo2"), 50)));
            }
            if (!string.IsNullOrWhiteSpace(facilityTableDTO.InterfaceInfo3) && facilityTableDTO.InterfaceInfo3.Length > 50)
            {
                validationErrorList.Add(new ValidationError("facilityTable", "InterfaceInfo", MessageContainerList.GetMessage(executionContext, 2197, MessageContainerList.GetMessage(executionContext, "InterfaceInfo3"), 50)));
            }
            if (!string.IsNullOrWhiteSpace(facilityTableDTO.Remarks) && facilityTableDTO.Remarks.Length > 200)
            {
                validationErrorList.Add(new ValidationError("facilityTable", "Remarks", MessageContainerList.GetMessage(executionContext, 2197, MessageContainerList.GetMessage(executionContext, "Remarks"), 200)));
            }
            return validationErrorList;
        }

        /// <summary>
        ///GetTableStatus(string loginId, string macAddress, int facilityId, int siteId)
        /// </summary>
        /// <returns>list of FacilityTableDTO object </returns>
        public List<FacilityTableDTO> GetTableStatus(string loginId, int posMachineId, int posTypeId, string posMachineName, int facilityId, int siteId)
        {
            log.LogMethodEntry();
            //getting userDTO based on login id passed

            //fetching default values set in management studio
            ParafaitDefaultsBL parafaitDefaultsBL = new ParafaitDefaultsBL(executionContext, "ENABLE_ORDER_SHARE_ACROSS_POS_COUNTERS");
            bool enableOrderShareAcrossPosCounters = ((parafaitDefaultsBL.ParafaitDefaultsDTO.DefaultValue.CompareTo("Y") == 0) ? true : false);

            parafaitDefaultsBL = new ParafaitDefaultsBL(executionContext, "ENABLE_ORDER_SHARE_ACROSS_USERS");
            bool enableOrderShareAcrossUsers = ((parafaitDefaultsBL.ParafaitDefaultsDTO.DefaultValue.CompareTo("Y") == 0) ? true : false);

            parafaitDefaultsBL = new ParafaitDefaultsBL(executionContext, "ENABLE_ORDER_SHARE_ACROSS_POS");
            bool enableOrderShareAcrossPOS = ((parafaitDefaultsBL.ParafaitDefaultsDTO.DefaultValue.CompareTo("Y") == 0) ? true : false);

            //creating the params class by setting fetched values
            FacilityTableParams facilityTableParams = new FacilityTableParams(facilityId, enableOrderShareAcrossPOS,
                                                          enableOrderShareAcrossPosCounters, enableOrderShareAcrossUsers,
                                                          posMachineId, posTypeId, posMachineName, loginId);
            FacilityTableDataHandler facilityTableDataHandler = new FacilityTableDataHandler();
            log.LogMethodExit();
            List<FacilityTableDTO> facilityTableDTOList = facilityTableDataHandler.GetTableStatus(facilityTableParams);
            return facilityTableDTOList;
        }

        public List<FacilityTableDTO> GetOpenFacilityTableDTOList(List<KeyValuePair<FacilityTableDTO.SearchByParameters, string>> searchParameters,
                                                              int POSTypeId,
                                                              string POSMachineName,
                                                              ExecutionContext executionContext)
        {
            log.LogMethodEntry(searchParameters, POSTypeId, POSMachineName);
            bool enableOrderShareAcrossPOSCounters = ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "ENABLE_ORDER_SHARE_ACROSS_POS_COUNTERS");
            bool enableOrderShareAcrossUsers = ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "ENABLE_ORDER_SHARE_ACROSS_USERS");
            bool enableOrderShareAcrossPOS = ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "ENABLE_ORDER_SHARE_ACROSS_POS");
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
        public List<FacilityTableDTO> GetAllFacilityTableList(List<KeyValuePair<FacilityTableDTO.SearchByParameters, string>> searchParameters,
                                       SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            FacilityTableDataHandler facilityTableDataHandler = new FacilityTableDataHandler(sqlTransaction);
            List<FacilityTableDTO> facilityTableDTOList = facilityTableDataHandler.GetAllFacilityTableLayout(searchParameters);
            log.LogMethodExit(facilityTableDTOList);
            return facilityTableDTOList;
        }

        /// <summary>
        /// This method should be used to Save and Update the Facility Tables details for Web Management Studio.
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                if (facilityTableList != null && facilityTableList.Any())
                {
                    foreach (FacilityTableDTO facilityTableDTO in facilityTableList)
                    {
                        FacilityTables facilityTables = new FacilityTables(executionContext, facilityTableDTO);
                        facilityTables.Save(sqlTransaction);
                    }
                }
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
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }
    }
}

