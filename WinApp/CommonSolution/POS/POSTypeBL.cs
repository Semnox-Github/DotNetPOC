/********************************************************************************************
 * Project Name - POSTypeBL
 * Description  - Business logic
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *2.50       23-Jan-2019    Jagan Mohana      Created constructor POSTypeListBL and added new method SaveUpdateDeletePOSTypeList
 *2.50       15-Feb-2019    Nagesh Badiger    modified SaveWithoutValidation method
 *2.70       07-Jul-2019    Mehraj            Added DeletePOSTypes() and DeletePOSTypesList() method
 *2.70.2     17-Oct-2019    Dakshakh raj      Modified : Issue fix for IN-claus in Sql Injection
 *2.80       10-May-2020    Girish Kundar     Modified: REST API Changes merge from WMS 
*2.90        26-Jun-2020    Girish Kundar         Modified as per the Standard CheckList 
 *2.150.3     27-APR-2023   Sweedol             Modified: Management form access new architecture changes
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.User;
namespace Semnox.Parafait.POS
{
    /// <summary>
    /// Business logic for POSType class.
    /// </summary>
    public class POSTypeBL
    {
        POSTypeDTO pOSTypeDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of POSTypeBL class
        /// </summary>
        private  POSTypeBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the pOSType id as the parameter
        /// Would fetch the pOSType object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public POSTypeBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            POSTypeDataHandler pOSTypeDataHandler = new POSTypeDataHandler(sqlTransaction);
            pOSTypeDTO = pOSTypeDataHandler.GetPOSTypeDTO(id);
            if (pOSTypeDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "POSType", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit(pOSTypeDTO);
        }

        /// <summary>
        /// Creates POSTypeBL object using the POSTypeDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="pOSTypeDTO">POSTypeDTO object</param>
        public POSTypeBL(ExecutionContext executionContext, POSTypeDTO pOSTypeDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, pOSTypeDTO);
            this.pOSTypeDTO = pOSTypeDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the POSType
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = Validate(sqlTransaction);
            if (validationErrorList.Count > 0)
            {
                throw new ValidationException("Validation Failed", validationErrorList);
            }

            SaveWithoutValidation(sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete a individual POSType
        /// </summary>
        /// <param name="posTypeId"></param>
        /// <param name="sqlTransaction"></param>
        public void DeletePOSTypes(POSTypeDTO pOSTypeDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(pOSTypeDTO);
            try
            {
                POSTypeDataHandler pOSTypeDataHandler = new POSTypeDataHandler(sqlTransaction);
                UpdateManagementFormAccess(pOSTypeDTO.POSTypeName, pOSTypeDTO.IsActive, pOSTypeDTO.Guid, sqlTransaction);
                pOSTypeDataHandler.DeletePOSType(pOSTypeDTO.POSTypeId);
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw new Core.Utilities.ForeignKeyException(MessageContainerList.GetMessage(executionContext, 1869));
            }
        }

        internal void SaveWithoutValidation(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            POSTypeDataHandler pOSTypeDataHandler = new POSTypeDataHandler(sqlTransaction);
            if (pOSTypeDTO.IsActive)
            {
                if (pOSTypeDTO.POSTypeId < 0)
                {
                    pOSTypeDTO = pOSTypeDataHandler.InsertPOSType(pOSTypeDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    pOSTypeDTO.AcceptChanges();
                    AddManagementFormAccess(sqlTransaction);
                }
                else
                {
                    if (pOSTypeDTO.IsChanged)
                    {
                        POSTypeDTO existingPosTypeDTO = new POSTypeBL(executionContext, pOSTypeDTO.POSTypeId, sqlTransaction).POSTypeDTO;
                        pOSTypeDTO = pOSTypeDataHandler.UpdatePOSType(pOSTypeDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                        pOSTypeDTO.AcceptChanges();
                        if (existingPosTypeDTO.POSTypeName.ToLower().ToString() != pOSTypeDTO.POSTypeName.ToLower().ToString())
                        {
                            RenameManagementFormAccess(existingPosTypeDTO.POSTypeName, sqlTransaction);
                        }
                        if (existingPosTypeDTO.IsActive != pOSTypeDTO.IsActive)
                        {
                            UpdateManagementFormAccess(pOSTypeDTO.POSTypeName, pOSTypeDTO.IsActive, pOSTypeDTO.Guid, sqlTransaction);
                        }
                    }
                }
            }
            else
            {
                if (pOSTypeDTO.POSTypeId >= 0)
                {
                    pOSTypeDataHandler.DeletePOSType(pOSTypeDTO.POSTypeId);
                }
            }
            log.LogMethodExit();
        }
        private void AddManagementFormAccess(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (pOSTypeDTO.POSTypeId > -1)
            {
                POSTypeDataHandler pOSTypeDataHandler = new POSTypeDataHandler(sqlTransaction);
                pOSTypeDataHandler.AddManagementFormAccess(pOSTypeDTO.POSTypeName, pOSTypeDTO.Guid, executionContext.GetSiteId(), pOSTypeDTO.IsActive);
                log.LogMethodExit();
            }
            log.LogMethodExit();
        }
        private void RenameManagementFormAccess(string existingFormName, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (pOSTypeDTO.POSTypeId > -1)
            {
                POSTypeDataHandler pOSTypeDataHandler = new POSTypeDataHandler(sqlTransaction);
                pOSTypeDataHandler.RenameManagementFormAccess(pOSTypeDTO.POSTypeName, existingFormName, executionContext.GetSiteId(), pOSTypeDTO.Guid);
                log.LogMethodExit();
            }
            log.LogMethodExit();
        }
        private void UpdateManagementFormAccess(string formName, bool updatedIsActive, string functionGuid, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (pOSTypeDTO.POSTypeId > -1)
            {
                POSTypeDataHandler pOSTypeDataHandler = new POSTypeDataHandler(sqlTransaction);
                pOSTypeDataHandler.UpdateManagementFormAccess(formName, executionContext.GetSiteId(), updatedIsActive, functionGuid);
                log.LogMethodExit();
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public POSTypeDTO POSTypeDTO
        {
            get
            {
                return pOSTypeDTO;
            }
        }

        /// <summary>
        /// Validates the customer relationship DTO
        /// </summary>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            POSTypeListBL pOSTypeListBL = new POSTypeListBL(executionContext);
            List<KeyValuePair<POSTypeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<POSTypeDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<POSTypeDTO.SearchByParameters, string>(POSTypeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParameters.Add(new KeyValuePair<POSTypeDTO.SearchByParameters, string>(POSTypeDTO.SearchByParameters.POS_TYPE_NAME, pOSTypeDTO.POSTypeName.ToString()));
            List<POSTypeDTO> pOSTypeDTOList = pOSTypeListBL.GetPOSTypeDTOList(searchParameters);
            if (string.IsNullOrWhiteSpace(pOSTypeDTO.POSTypeName))
            {
                validationErrorList.Add(new ValidationError("POSType", "POSTypeName", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Name"))));
            }
            if (pOSTypeDTOList != null && pOSTypeDTO.POSTypeId < 0)
            {
                validationErrorList.Add(new ValidationError("POSType", "POSTypeName", MessageContainerList.GetMessage(executionContext, 1904))); // Duplicate Postype name not allowed
            }
            if (pOSTypeDTOList != null && pOSTypeDTO.POSTypeId > 0 && pOSTypeDTOList.Any(x=>x.POSTypeId != pOSTypeDTO.POSTypeId))
            {
                validationErrorList.Add(new ValidationError("POSType", "POSTypeName", MessageContainerList.GetMessage(executionContext, 1904))); // Duplicate Postype name not allowed
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        public List<ValidationError> ValidateLicensedPOSMachines(SqlTransaction sqlTransaction = null)
        {
            List<ValidationError> validationErrorList = new List<ValidationError>();
            int licensedNumberOfPOSMachines = 0;
            POSMachineList pOSMachineList = new POSMachineList(executionContext);
            List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> searchParameters = new List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>>();
            searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.ISACTIVE,"1"));
            List<POSMachineDTO> pOSMachineDTOList = pOSMachineList.GetAllPOSMachines(searchParameters, false, false, sqlTransaction);
            if (pOSMachineDTOList != null && pOSMachineDTOList.Count > 0)
            {
                CanAddPOSMachines(ref licensedNumberOfPOSMachines, sqlTransaction);
                if (pOSMachineDTOList.Count > licensedNumberOfPOSMachines)
                {
                    validationErrorList.Add(new ValidationError("NoOfLicensedPOSMachines", "NoOfPOSMachines", MessageContainerList.GetMessage(executionContext, 1871))); /// Number of registered POS Machines is more than the number of licensed POS Machines
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
        /// <summary>
        /// Number of registered POS Machines is more than the number of licensed POS Machines
        /// </summary>
        private void CanAddPOSMachines(ref int licensedNumberOfPOSMachines, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            int siteId = executionContext.GetSiteId();
            string siteKey = "";
            string dummy = "";
            log.LogVariableState("siteId", siteId);
            POSTypeDataHandler pOSTypeDataHandler = new POSTypeDataHandler(sqlTransaction);
            pOSTypeDataHandler.ReadKeysFromDB(siteId, ref siteKey, ref dummy);
            if (siteKey == "")
            {
                licensedNumberOfPOSMachines = 0;
                log.LogMethodExit("siteKey == ''");
                return;
            }
            try
            {
                var noOfPOSMachinesLicensedCode = pOSTypeDataHandler.NoOfPOSMachinesLicensed(siteId);
                if (noOfPOSMachinesLicensedCode != null)
                {
                    string codeValue = Encryption.Decrypt(noOfPOSMachinesLicensedCode.ToString());
                    string[] codeValueArray = codeValue.Split('|');
                    if (codeValueArray[0] == siteKey)
                    {
                        licensedNumberOfPOSMachines = Convert.ToInt32(codeValueArray[1]);
                    }
                }
                else
                {
                    log.Error("null code");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                licensedNumberOfPOSMachines = 0;
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of POSType
    /// </summary>
    public class POSTypeListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<POSTypeDTO> posTypeDTOList;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public POSTypeListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.posTypeDTOList = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized  constructor
        /// </summary>
        /// <param name="pOSTypeDTO"></param>
        /// <param name="executionContext"></param>
        public POSTypeListBL(ExecutionContext executionContext, List<POSTypeDTO> posTypeDTOList)
        {
            log.LogMethodEntry(posTypeDTOList, executionContext);
            this.executionContext = executionContext;
            this.posTypeDTOList = posTypeDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the POSType list
        /// </summary>
        public List<POSTypeDTO> GetPOSTypeDTOList(List<KeyValuePair<POSTypeDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            POSTypeDataHandler pOSTypeDataHandler = new POSTypeDataHandler(sqlTransaction);
            List<POSTypeDTO> returnValue = pOSTypeDataHandler.GetPOSTypeDTOList(searchParameters);
            log.LogMethodExit(returnValue);
            return returnValue;
        }
        /// <summary>
        /// Get POSMachines by RoleID
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public List<POSTypeDTO> GetPOSCounterByRoleId(int roleId, ExecutionContext executionContext)
        {
            List<POSTypeDTO> pOSTypeDTOList = null;

            ManagementFormAccessListBL managementFormAccessListBL = new ManagementFormAccessListBL(executionContext);
            List<KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>>();
            searchParams.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.MAIN_MENU, "POS Counter"));
            searchParams.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.ACCESS_ALLOWED, "1"));
            searchParams.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.ROLE_ID, roleId.ToString()));
            searchParams.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.ISACTIVE, "1"));
            List<ManagementFormAccessDTO> managementFormAccessDTOList = managementFormAccessListBL.GetManagementFormAccessDTOList(searchParams);
            if (managementFormAccessDTOList != null && managementFormAccessDTOList.Count > 0)
            {
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < managementFormAccessDTOList.Count; i++)
                {
                    //stringBuilder.Append("'");
                    stringBuilder.Append(managementFormAccessDTOList[i].FormName);
                    //stringBuilder.Append("'");
                    if (managementFormAccessDTOList.Count > 1 && i != managementFormAccessDTOList.Count - 1)
                        stringBuilder.Append(",");
                }

                POSTypeListBL pOSTypeListBL = new POSTypeListBL(executionContext);
                List<KeyValuePair<POSTypeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<POSTypeDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<POSTypeDTO.SearchByParameters, string>(POSTypeDTO.SearchByParameters.POS_TYPE_NAME_LIST, stringBuilder.ToString()));
                pOSTypeDTOList = pOSTypeListBL.GetPOSTypeDTOList(searchParameters);
            }
            return pOSTypeDTOList;
        }
        /// <summary>
        /// Save or update pos management counters list
        /// </summary>
        public void Save(bool isLicensedPOSMachines = false)
        {
            try
            {
                log.LogMethodEntry();
                if (posTypeDTOList != null)
                {
                    foreach (POSTypeDTO posTypeDto in posTypeDTOList)
                    {
                        POSTypeBL posTypeBLObj = new POSTypeBL(executionContext, posTypeDto);
                        if (!isLicensedPOSMachines)
                        {
                            List<ValidationError> validationErrorList = posTypeBLObj.ValidateLicensedPOSMachines(null);
                            if (validationErrorList.Count > 0)
                            {
                                throw new POSMachinesLicenseException(validationErrorList[0].Message);
                            }
                        }
                        posTypeBLObj.Save(null);
                    }
                }
                log.LogMethodExit();
            }
            catch (SqlException sqlEx)
            {
                log.Error(sqlEx);
                log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                if (sqlEx.Number == 1869)
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
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Delete POSTypes
        /// </summary>
        public void DeletePOSTypesList()
        {
            log.LogMethodEntry();
            try
            {
                foreach (POSTypeDTO posTypeDto in posTypeDTOList)
                {
                    POSTypeBL posTypeBLObj = new POSTypeBL(executionContext, posTypeDto);
                    posTypeBLObj.DeletePOSTypes(posTypeDto);
                }
            }
            catch (SqlException sqlEx)
            {
                log.Error(sqlEx);
                log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                if (sqlEx.Number == 1869)
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
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }
    }
}
