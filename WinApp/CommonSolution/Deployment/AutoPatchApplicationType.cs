/********************************************************************************************
 * Project Name - Patch Application Type
 * Description  - Bussiness logic of patch application type
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        17-Feb-2016   Raghuveera          Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Deployment
{
    /// <summary>
    /// Patch application type will creates and modifies the application type   
    /// </summary>
    public class AutoPatchApplicationType
    {
        private AutoPatchApplTypeDTO autoPatchApplTypeDTO;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Default constructor
        /// </summary>
        public AutoPatchApplicationType()
        {
            log.Debug("Starts-AutoPatchApplicationType() default constructor");
            autoPatchApplTypeDTO = null;
            log.Debug("Ends-AutoPatchApplicationType() default constructor");
        }
        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="autoPatchApplTypeDTO">Parameter of the type AutoPatchApplTypeDTO</param>
        public AutoPatchApplicationType(AutoPatchApplTypeDTO autoPatchApplTypeDTO)
        {
            log.Debug("Starts-AutoPatchApplicationType(autoPatchApplTypeDTO) parameterized constructor.");
            this.autoPatchApplTypeDTO = autoPatchApplTypeDTO;
            log.Debug("Ends-AutoPatchApplicationType(autoPatchApplTypeDTO) parameterized constructor.");
        }
        /// <summary>
        /// Saves the patch application type
        /// application type will be inserted if PatchApplicationTypeId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save()
        {
            log.Debug("Starts-Save() method.");
            ExecutionContext applicationTypeUserContext = ExecutionContext.GetExecutionContext();
            AutoPatchApplTypeDataHandler autoPatchApplTypeDataHandler = new AutoPatchApplTypeDataHandler();
            if (autoPatchApplTypeDTO.PatchApplicationTypeId <= 0)
            {
                int autoPatchApplTypeId = autoPatchApplTypeDataHandler.InsertAutoPatchApplType(autoPatchApplTypeDTO, applicationTypeUserContext.GetUserId(), applicationTypeUserContext.GetSiteId());
                autoPatchApplTypeDTO.PatchApplicationTypeId = autoPatchApplTypeId;
            }
            else
            {
                if (autoPatchApplTypeDTO.IsChanged == true)
                {
                    autoPatchApplTypeDataHandler.UpdateAutoPatchApplType(autoPatchApplTypeDTO, applicationTypeUserContext.GetUserId(), applicationTypeUserContext.GetSiteId());
                    autoPatchApplTypeDTO.AcceptChanges();
                }
            }
            log.Debug("Ends-Save() method.");
        }
    }
    /// <summary>
    /// Manages the list of patch application type
    /// </summary>
    public class AutoPatchApplTypeList
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Returns the patch application type list
        /// </summary>
        public List<AutoPatchApplTypeDTO> GetAllAutoPatchApplTypes(List<KeyValuePair<AutoPatchApplTypeDTO.SearchByAutoPatchApplTypeParameters, string>> searchParameters)
        {
            log.Debug("Starts-GetAllAutoPatchApplTypes(searchParameters) method.");
            AutoPatchApplTypeDataHandler autoPatchApplTypeDataHandler = new AutoPatchApplTypeDataHandler();
            log.Debug("Ends-GetAllAutoPatchApplTypes(searchParameters) method by returning the result of autoPatchApplTypeDataHandler.GetAutoPatchApplTypeList() call.");
            return autoPatchApplTypeDataHandler.GetAutoPatchApplTypeList(searchParameters);
        }
    }
}
