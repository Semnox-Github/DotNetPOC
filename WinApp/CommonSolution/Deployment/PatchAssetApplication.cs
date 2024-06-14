/********************************************************************************************
 * Project Name - Patch Asset Application
 * Description  - Bussiness logic of patch asset application 
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
    /// Patch asset application will creates and modifies the application    
    /// </summary>
    public class PatchAssetApplication
    {
        private PatchAssetApplicationDTO patchAssetApplicationDTO;
       Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Default constructor
        /// </summary>
        public PatchAssetApplication()
        {
            log.Debug("Starts-PatchAssetApplication() default constructor");
            patchAssetApplicationDTO = null;
            log.Debug("Ends-PatchAssetApplication() default constructor");
        }
        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="patchAssetApplicationDTO">Parameter of the type PatchAssetApplicationDTO</param>
        public PatchAssetApplication(PatchAssetApplicationDTO patchAssetApplicationDTO)
        {
            log.Debug("Starts-PatchAssetApplication(patchAssetApplicationDTO) parameterized constructor.");
            this.patchAssetApplicationDTO = patchAssetApplicationDTO;
            log.Debug("Ends-PatchAssetApplication(patchAssetApplicationDTO) parameterized constructor.");
        }
        /// <summary>
        /// Saves the patch asset application
        /// asset application will be inserted if PatchAssetApplicationId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save()
        {
            log.Debug("Starts-Save() method.");
             ExecutionContext assetApplicationUserContext =  ExecutionContext.GetExecutionContext();
            PatchAssetApplicationDataHandler patchAssetApplicationDataHandler = new PatchAssetApplicationDataHandler();
            if (patchAssetApplicationDTO.PatchAssetApplicationId <= 0)
            {
                int patchAssetApplicationId = patchAssetApplicationDataHandler.InsertPatchAssetApplication(patchAssetApplicationDTO, assetApplicationUserContext.GetUserId(), assetApplicationUserContext.GetSiteId());
                patchAssetApplicationDTO.PatchAssetApplicationId = patchAssetApplicationId;
            }
            else
            {
                if (patchAssetApplicationDTO.IsChanged == true)
                {
                    patchAssetApplicationDataHandler.UpdatePatchAssetApplication(patchAssetApplicationDTO, assetApplicationUserContext.GetUserId(), assetApplicationUserContext.GetSiteId());
                    patchAssetApplicationDTO.AcceptChanges();
                }
            }
            log.Debug("Ends-Save() method.");
        }
    }
    /// <summary>
    /// Manages the list of patch asset application
    /// </summary>
    public class PatchAssetApplicationList
    {
       Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Returns the patch asset application list
        /// </summary>
        public List<PatchAssetApplicationDTO> GetAllPatchAssetApplications(List<KeyValuePair<PatchAssetApplicationDTO.SearchByPatchAssetApplicationParameters, string>> searchParameters)
        {
            log.Debug("Starts-GetAllPatchAssetApplications(searchParameters) method.");
            PatchAssetApplicationDataHandler patchApplicationTypeDataHandler = new PatchAssetApplicationDataHandler();
            log.Debug("Ends-GetAllPatchAssetApplications(searchParameters) method by returning the result of patchApplicationTypeDataHandler.GetPatchAssetApplicationList() call.");
            return patchApplicationTypeDataHandler.GetPatchAssetApplicationList(searchParameters);
        }
    }
}
