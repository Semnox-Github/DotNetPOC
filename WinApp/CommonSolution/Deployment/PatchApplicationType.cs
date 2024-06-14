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
    public class PatchApplicationType
    {
        private PatchApplicationTypeDTO patchApplicationTypeDTO;
       Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Default constructor
        /// </summary>
        public PatchApplicationType()
        {
            log.Debug("Starts-PatchApplicationType() default constructor");
            patchApplicationTypeDTO = null;
            log.Debug("Ends-PatchApplicationType() default constructor");
        }
        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="patchApplicationTypeDTO">Parameter of the type PatchApplicationTypeDTO</param>
        public PatchApplicationType(PatchApplicationTypeDTO patchApplicationTypeDTO)
        {
            log.Debug("Starts-PatchApplicationType(patchApplicationTypeDTO) parameterized constructor.");
            this.patchApplicationTypeDTO = patchApplicationTypeDTO;
            log.Debug("Ends-PatchApplicationType(patchApplicationTypeDTO) parameterized constructor.");
        }
        /// <summary>
        /// Saves the patch application type
        /// application type will be inserted if PatchApplicationTypeId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save()
        {
            log.Debug("Starts-Save() method.");
             ExecutionContext applicationTypeUserContext =  ExecutionContext.GetExecutionContext();
            PatchApplicationTypeDataHandler patchApplicationTypeDataHandler = new PatchApplicationTypeDataHandler();
            if (patchApplicationTypeDTO.PatchApplicationTypeId <= 0)
            {
                int patchApplicationTypeId = patchApplicationTypeDataHandler.InsertPatchApplicationType(patchApplicationTypeDTO, applicationTypeUserContext.GetUserId(), applicationTypeUserContext.GetSiteId());
                patchApplicationTypeDTO.PatchApplicationTypeId = patchApplicationTypeId;
            }
            else
            {
                if (patchApplicationTypeDTO.IsChanged == true)
                {
                    patchApplicationTypeDataHandler.UpdatePatchApplicationType(patchApplicationTypeDTO, applicationTypeUserContext.GetUserId(), applicationTypeUserContext.GetSiteId());
                    patchApplicationTypeDTO.AcceptChanges();
                }
            }
            log.Debug("Ends-Save() method.");
        }
    }
    /// <summary>
    /// Manages the list of patch application type
    /// </summary>
    public class PatchApplicationTypeList
    {
       Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Returns the patch application type list
        /// </summary>
        public List<PatchApplicationTypeDTO> GetAllPatchApplicationTypes(List<KeyValuePair<PatchApplicationTypeDTO.SearchByPatchApplicationTypeParameters, string>> searchParameters)
        {
            log.Debug("Starts-GetAllPatchApplicationTypes(searchParameters) method.");
            PatchApplicationTypeDataHandler patchApplicationTypeDataHandler = new PatchApplicationTypeDataHandler();
            log.Debug("Ends-GetAllPatchApplicationTypes(searchParameters) method by returning the result of patchApplicationTypeDataHandler.GetPatchApplicationTypeList() call.");
            return patchApplicationTypeDataHandler.GetPatchApplicationTypeList(searchParameters);
        }
    }
}
