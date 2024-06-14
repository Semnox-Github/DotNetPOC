/********************************************************************************************
 * Project Name - Maintenance
 * Description  - A high level structure created to classify the comments 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *2.150.3    21-Mar-2022    Abhishek         Created 
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
    /// This defines the various classification of comments
    /// Like building etc, to create a high level grouping of the comments
    /// </summary>
    public class MaintenanceCommentsBL
    {
        private MaintenanceCommentsDTO maintenanceCommentsDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of MaintenanceCommentsBL class
        /// </summary>
        /// called in comments mapper class
        private MaintenanceCommentsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="maintenanceCommentsDTO"></param>
        public MaintenanceCommentsBL(ExecutionContext executionContext, MaintenanceCommentsDTO maintenanceCommentsDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, maintenanceCommentsDTO);
            this.maintenanceCommentsDTO = maintenanceCommentsDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the comments id as the parameter
        /// Would fetch the MaintenanceComments object from the database based on the id passed. 
        /// </summary>
        /// <param name="commentId">commentId</param>
        public MaintenanceCommentsBL(ExecutionContext executionContext, int commentId, SqlTransaction sqlTransaction = null)
             : this(executionContext)
        {
            log.LogMethodEntry(executionContext, commentId, sqlTransaction);
            MaintenanceCommentsDataHandler maintenanceCommentsDataHandler = new MaintenanceCommentsDataHandler(sqlTransaction);
            maintenanceCommentsDTO = maintenanceCommentsDataHandler.GetMaintenanceComments(commentId);
            log.LogMethodExit(maintenanceCommentsDTO);
        }

        /// <summary>
        /// Saves the MaintenanceComments
        /// Checks if the MaintenanceComments id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (maintenanceCommentsDTO.IsChanged == false
                   && maintenanceCommentsDTO.CommentId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            MaintenanceCommentsDataHandler maintenanceCommentsDataHandler = new MaintenanceCommentsDataHandler(sqlTransaction);
            if (maintenanceCommentsDTO.CommentId < 0)
            {
                maintenanceCommentsDTO = maintenanceCommentsDataHandler.Insert(maintenanceCommentsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                maintenanceCommentsDTO.AcceptChanges();
            }
            else
            {
                if (maintenanceCommentsDTO.IsChanged)
                {
                    maintenanceCommentsDTO = maintenanceCommentsDataHandler.Update(maintenanceCommentsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    maintenanceCommentsDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public MaintenanceCommentsDTO GetMaintenanceComments { get { return maintenanceCommentsDTO; } }
    }

    /// <summary>
    /// Manages the list of MaintenanceComments
    /// </summary>
    public class MaintenanceCommentsListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<MaintenanceCommentsDTO> maintenanceCommentsDTOList;

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        public MaintenanceCommentsListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="maintenanceCommentsDTOList"></param>
        public MaintenanceCommentsListBL(ExecutionContext executionContext, List<MaintenanceCommentsDTO> maintenanceCommentsDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, maintenanceCommentsDTOList);
            this.maintenanceCommentsDTOList = maintenanceCommentsDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the MaintenanceComments list
        /// </summary>
        public List<MaintenanceCommentsDTO> GetAllMaintenanceComments(List<KeyValuePair<MaintenanceCommentsDTO.SearchByCommentsParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            MaintenanceCommentsDataHandler maintenanceCommentsDataHandler = new MaintenanceCommentsDataHandler(sqlTransaction);
            List<MaintenanceCommentsDTO> maintenanceCommentsDTOList = maintenanceCommentsDataHandler.GetMaintenanceCommentsDTOList(searchParameters);
            log.LogMethodExit(maintenanceCommentsDTOList);
            return maintenanceCommentsDTOList;
        }

        /// <summary>
        /// Gets the MaintenanceComments List for maintChklstdetIdList
        /// </summary>
        /// <param name="maintChklstdetIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of CommentsDTO</returns>
        public List<MaintenanceCommentsDTO> GetMaintenanceCommentsDTOList(List<int> maintChklstdetIdList, bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(maintChklstdetIdList, activeRecords);
            MaintenanceCommentsDataHandler maintenanceCommentsDataHandler = new MaintenanceCommentsDataHandler(sqlTransaction);
            List<MaintenanceCommentsDTO> maintenanceCommentsDTOList = maintenanceCommentsDataHandler.GetMaintenanceCommentsDTOList(maintChklstdetIdList, activeRecords);
            log.LogMethodExit(maintenanceCommentsDTOList);
            return maintenanceCommentsDTOList;
        }

        /// <summary>
        /// Save MaintenanceComments
        /// </summary>
        public List<MaintenanceCommentsDTO> Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<MaintenanceCommentsDTO> savedMaintenanceCommentsDTOList = new List<MaintenanceCommentsDTO>();
            if (maintenanceCommentsDTOList == null || maintenanceCommentsDTOList.Any() == false)
            {
                log.LogMethodExit(savedMaintenanceCommentsDTOList);
                return savedMaintenanceCommentsDTOList;
            }
            foreach (MaintenanceCommentsDTO maintenanceCommentsDTO in maintenanceCommentsDTOList)
            {
                MaintenanceCommentsBL maintenanceCommentsBL = new MaintenanceCommentsBL(executionContext, maintenanceCommentsDTO);
                maintenanceCommentsBL.Save(sqlTransaction);
                savedMaintenanceCommentsDTOList.Add(maintenanceCommentsBL.GetMaintenanceComments);
            }
            log.LogMethodExit(savedMaintenanceCommentsDTOList);
            return savedMaintenanceCommentsDTOList;
        }
    }
}
