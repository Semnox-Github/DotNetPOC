using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.User
{
    public class ManagementFormsBL
    {
        private ManagementFormsDTO managementFormsDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);        

        /// <summary>
        /// Parameterized constructor of ManagementFormsBL class
        /// </summary>
        private ManagementFormsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates ManagementFormsBL object using the ManagementFormsDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="managementFormsDTO">ManagementFormsDTO object</param>
        public ManagementFormsBL(ExecutionContext executionContext, ManagementFormsDTO managementFormsDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, managementFormsDTO);
            this.managementFormsDTO = managementFormsDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates ManagementFormsBL object using the 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="id"></param>
        /// <param name="sqlTransaction"></param>
        public ManagementFormsBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            ManagementFormsDataHandler managementFormsDataHandler = new ManagementFormsDataHandler(sqlTransaction);
            managementFormsDTO = managementFormsDataHandler.GetManagementFormsDTO(id);
            log.LogMethodExit();
        }
        /// <summary>
        /// Saves the ManagementForms
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ManagementFormsDataHandler managementFormsDataHandler = new ManagementFormsDataHandler(sqlTransaction);
            if (managementFormsDTO.ManagementFormId < 0)
            {
                managementFormsDTO = managementFormsDataHandler.Insert(managementFormsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                managementFormsDTO.AcceptChanges();
                AddManagementFormAccess(sqlTransaction);
            }
            else if (managementFormsDTO.IsChanged)
            {
                ManagementFormsDTO existingDTO = new ManagementFormsBL(executionContext, managementFormsDTO.ManagementFormId, sqlTransaction).ManagementFormsDTO;
                managementFormsDTO = managementFormsDataHandler.Update(managementFormsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                managementFormsDTO.AcceptChanges();
                if (existingDTO.FormName.ToLower().ToString() != managementFormsDTO.FormName.ToLower().ToString())
                {
                    RenameManagementFormAccess(existingDTO.FormName, sqlTransaction);
                }
                if (existingDTO.IsActive != managementFormsDTO.IsActive)
                {
                    UpdateManagementFormAccess(existingDTO.FormName, existingDTO.GroupName, existingDTO.FunctionGroup, existingDTO.IsActive, existingDTO.Guid, sqlTransaction);
                }
            }
            log.LogMethodExit();
        }

        private void AddManagementFormAccess(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (managementFormsDTO.ManagementFormId > -1)
            {
                ManagementFormsDataHandler managementFormsDataHandler = new ManagementFormsDataHandler(sqlTransaction);
                managementFormsDataHandler.AddManagementFormAccess(managementFormsDTO.FormName, managementFormsDTO.GroupName, managementFormsDTO.FunctionGroup, managementFormsDTO.Guid, executionContext.GetSiteId());
                log.LogMethodExit();
            }
            log.LogMethodExit();
        }
        private void RenameManagementFormAccess(string existingFormName, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (managementFormsDTO.ManagementFormId > -1)
            {
                ManagementFormsDataHandler managementFormsDataHandler = new ManagementFormsDataHandler(sqlTransaction);
                managementFormsDataHandler.RenameManagementFormAccess(managementFormsDTO.FormName, managementFormsDTO.GroupName, existingFormName, managementFormsDTO.FunctionGroup, executionContext.GetSiteId(), managementFormsDTO.Guid);
                log.LogMethodExit();
            }
            log.LogMethodExit();
        }
        private void UpdateManagementFormAccess(string formName, string mainMenu, string functionGroup, bool updatedIsActive, string functionGuid, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (managementFormsDTO.ManagementFormId > -1)
            {
                ManagementFormsDataHandler managementFormsDataHandler = new ManagementFormsDataHandler(sqlTransaction);
                managementFormsDataHandler.UpdateManagementFormAccess(formName, mainMenu, functionGroup, executionContext.GetSiteId(), updatedIsActive, functionGuid);
                log.LogMethodExit();
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ManagementFormsDTO ManagementFormsDTO
        {
            get
            {
                return managementFormsDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of ManagementForms
    /// </summary>
    public class ManagementFormsListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<ManagementFormsDTO> managementFormsDTOList;        
        /// <summary>
        /// Parameterized constructor having ExecutionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public ManagementFormsListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.managementFormsDTOList = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="managementFormsDTOList"></param>
        public ManagementFormsListBL(ExecutionContext executionContext, List<ManagementFormsDTO> managementFormsDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, managementFormsDTOList);
            this.managementFormsDTOList = managementFormsDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the ProductKeyDTO List
        /// </summary>
        public List<ManagementFormsDTO> GetManagementFormsDTOList(List<KeyValuePair<ManagementFormsDTO.SearchByParameters, string>> searchParameters,  SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ManagementFormsDataHandler managementFormsDataHandler = new ManagementFormsDataHandler(sqlTransaction);
            List<ManagementFormsDTO> managementFormsDTOList = managementFormsDataHandler.GetAllManagementFormsDTOList(searchParameters);            
            log.LogMethodExit(managementFormsDTOList);
            return managementFormsDTOList;
        }        
    }
}