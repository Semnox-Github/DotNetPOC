/********************************************************************************************
 * Project Name - Maintenance
 * Description  - Bussiness logic of Maintenance Images
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By      Remarks          
 *********************************************************************************************
 *2.150.3    21-Mar-2022      Abhishek         Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Maintenance
{
    /// <summary>
    /// This defines the various classification of images
    /// Like building etc, to create a high level grouping of the images
    /// </summary>
    public class MaintenanceImagesBL
    {
        private MaintenanceImagesDTO maintenanceImagesDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of Images class
        /// </summary>
        /// called in Images mapper class
        private MaintenanceImagesBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="maintenanceImagesDTO"></param>
        public MaintenanceImagesBL(ExecutionContext executionContext, MaintenanceImagesDTO maintenanceImagesDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, maintenanceImagesDTO);
            this.maintenanceImagesDTO = maintenanceImagesDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the image id as the parameter
        /// Would fetch the image object from the database based on the id passed. 
        /// </summary>
        /// <param name="imageId">ImageId</param>
        public MaintenanceImagesBL(ExecutionContext executionContext, int imageId, SqlTransaction sqlTransaction = null)
             : this(executionContext)
        {
            log.LogMethodEntry(executionContext, imageId, sqlTransaction);
            MaintenanceImagesDataHandler maintenanceImagesDataHandler = new MaintenanceImagesDataHandler(sqlTransaction);
            maintenanceImagesDTO = maintenanceImagesDataHandler.GetMaintenanceImages(imageId);
            log.LogMethodExit(maintenanceImagesDTO);
        }

        /// <summary>
        /// Saves the images
        /// Checks if the image id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (maintenanceImagesDTO.IsChanged == false
                   && maintenanceImagesDTO.ImageId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            MaintenanceImagesDataHandler imagesDataHandler = new MaintenanceImagesDataHandler(sqlTransaction);
            if (maintenanceImagesDTO.ImageId < 0)
            {
                maintenanceImagesDTO = imagesDataHandler.Insert(maintenanceImagesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                maintenanceImagesDTO.AcceptChanges();
            }
            else
            {
                if (maintenanceImagesDTO.IsChanged)
                {
                    maintenanceImagesDTO = imagesDataHandler.Update(maintenanceImagesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    maintenanceImagesDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
       
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public MaintenanceImagesDTO GetMaintenanceImages { get { return maintenanceImagesDTO; } }
    }

    /// <summary>
    /// Manages the list of images
    /// </summary>
    public class MaintenanceImagesListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<MaintenanceImagesDTO> maintenanceImagesDTOList;

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        public MaintenanceImagesListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="maintenanceImagesDTOList"></param>
        public MaintenanceImagesListBL(ExecutionContext executionContext, List<MaintenanceImagesDTO> maintenanceImagesDTOList)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext, maintenanceImagesDTOList);
            this.maintenanceImagesDTOList = maintenanceImagesDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the images list
        /// </summary>
        public List<MaintenanceImagesDTO> GetAllMaintenanceImages(List<KeyValuePair<MaintenanceImagesDTO.SearchByImagesParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            MaintenanceImagesDataHandler maintenanceImagesDataHandler = new MaintenanceImagesDataHandler(sqlTransaction);
            List<MaintenanceImagesDTO> maintenanceImagesDTOList = maintenanceImagesDataHandler.GetMaintenanceImagesDTOList(searchParameters);
            log.LogMethodExit(maintenanceImagesDTOList);
            return maintenanceImagesDTOList;
        }

        /// <summary>
        /// Gets the ImagesDTO List for maintChklstdetIdList
        /// </summary>
        /// <param name="maintChklstdetIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of MaintenanceJobStatusDTO</returns>
        public List<MaintenanceImagesDTO> GetMaintenanceImagesDTOList(List<int> maintChklstdetIdList, bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(maintChklstdetIdList, activeRecords, sqlTransaction);
            MaintenanceImagesDataHandler maintenanceImagesDataHandler = new MaintenanceImagesDataHandler(sqlTransaction);
            List<MaintenanceImagesDTO> maintenanceImagesDTOList = maintenanceImagesDataHandler.GetMaintenanceImagesDTOList(maintChklstdetIdList, activeRecords);
            log.LogMethodExit(maintenanceImagesDTOList);
            return maintenanceImagesDTOList;
        }

        /// <summary>
        /// Save MaintenanceImages
        /// </summary>
        public List<MaintenanceImagesDTO> Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<MaintenanceImagesDTO> savedMaintenanceImagesDTOList = new List<MaintenanceImagesDTO>();
            if (maintenanceImagesDTOList == null || maintenanceImagesDTOList.Any() == false)
            {
                log.LogMethodExit(savedMaintenanceImagesDTOList);
                return savedMaintenanceImagesDTOList;
            }
            foreach (MaintenanceImagesDTO maintenanceImagesDTO in maintenanceImagesDTOList)
            {
                MaintenanceImagesBL maintenanceImagesBL = new MaintenanceImagesBL(executionContext, maintenanceImagesDTO);
                maintenanceImagesBL.Save(sqlTransaction);
                savedMaintenanceImagesDTOList.Add(maintenanceImagesBL.GetMaintenanceImages);
            }
            log.LogMethodExit(savedMaintenanceImagesDTOList);
            return savedMaintenanceImagesDTOList;
        }
    }
}
