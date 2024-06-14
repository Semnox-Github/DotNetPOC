/********************************************************************************************
 * Project Name - HR Module
 * Description  - BL Class for Leave Template
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.0      15-Oct-2019  Indrajeet Kumar          Created 
 *2.90        20-May-2020   Vikas Dwivedi           Modified as per the Standard CheckList
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using System.Data.SqlClient;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// Business Logic for LeaveTemplateBL
    /// </summary>
    public class LeaveTemplateBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private LeaveTemplateDTO leaveTemplateDTO;
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor of LeaveTemplateBL
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private LeaveTemplateBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates LeaveTemplateBL object using the LeaveTemplateDTO
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="leaveTemplateDTO">leaveTemplateDTO</param>
        public LeaveTemplateBL(ExecutionContext executionContext, LeaveTemplateDTO leaveTemplateDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, leaveTemplateDTO);
            this.leaveTemplateDTO = leaveTemplateDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the leaveTemplate id as the parameter
        /// Would fetch the leaveTemplate object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext object is passed as parameter</param>
        /// <param name="leaveTemplateId">id of LeaveTemplate Object</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public LeaveTemplateBL(ExecutionContext executionContext, int leaveTemplateId, SqlTransaction sqlTransaction = null)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext, leaveTemplateId, sqlTransaction);
            LeaveTemplateDataHandler leaveTemplateDataHandler = new LeaveTemplateDataHandler(sqlTransaction);
            this.leaveTemplateDTO = leaveTemplateDataHandler.GetLeaveTemplateDTO(leaveTemplateId);
            if (leaveTemplateDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "LeaveTemplateDTO", leaveTemplateId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the LeaveTemplateBL
        /// Checks if the LeaveTemplateBL id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            LeaveTemplateDataHandler leaveTemplateDataHandler = new LeaveTemplateDataHandler(sqlTransaction);
            if (leaveTemplateDTO.LeaveTemplateId < 0)
            {
                leaveTemplateDTO = leaveTemplateDataHandler.Insert(leaveTemplateDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                leaveTemplateDTO.AcceptChanges();
            }
            else
            {
                if (leaveTemplateDTO.IsChanged)
                {
                    leaveTemplateDTO = leaveTemplateDataHandler.Update(leaveTemplateDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    leaveTemplateDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the LeaveTemplateDTO based on LeaveTemplateId
        /// </summary>
        internal void Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                LeaveTemplateDataHandler leaveTemplateDataHandler = new LeaveTemplateDataHandler(sqlTransaction);
                leaveTemplateDataHandler.Delete(leaveTemplateDTO.LeaveTemplateId);
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Deleting LeaveTemplateDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Validates the LeaveTemplateDTO
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            ValidationError validationError = null;
            return validationErrorList;
            // Validation Logic here 
        }

        /// <summary>
        /// Gets LeaveTemplateDTO Object
        /// </summary>
        public LeaveTemplateDTO GetLeaveTemplateDTO
        {
            get { return leaveTemplateDTO; }
        }
    }

    /// <summary>
    /// Manages the list of LeaveTemplateBL
    /// </summary>
    public class LeaveTemplateListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<LeaveTemplateDTO> leaveTemplateDTOList = new List<LeaveTemplateDTO>();
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parametized Constructor with ExecutionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public LeaveTemplateListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with ExecutionContext and DTO Parameter
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="leaveTemplateDTOList"></param>
        public LeaveTemplateListBL(ExecutionContext executionContext, List<LeaveTemplateDTO> leaveTemplateDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, leaveTemplateDTOList);
            this.leaveTemplateDTOList = leaveTemplateDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the LeaveTemplateBL list
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<LeaveTemplateDTO> GetAllLeaveTemplateList(List<KeyValuePair<LeaveTemplateDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            LeaveTemplateDataHandler leaveTemplateDataHandler = new LeaveTemplateDataHandler(sqlTransaction);
            List<LeaveTemplateDTO> leaveTemplateDTOList = leaveTemplateDataHandler.GetLeaveTemplateDTOList(searchParameters, sqlTransaction);
            log.LogMethodExit(leaveTemplateDTOList);
            return leaveTemplateDTOList;
        }

        /// <summary>
        /// Save the LeaveTemplateBL List
        /// </summary>
        public List<LeaveTemplateDTO> SaveUpdateLeaveTemplate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<LeaveTemplateDTO> leaveTemplateDTOLists = new List<LeaveTemplateDTO>();
            if (leaveTemplateDTOList != null)
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (LeaveTemplateDTO LeaveTemplateDTO in leaveTemplateDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            LeaveTemplateBL leaveTemplateBL = new LeaveTemplateBL(executionContext, LeaveTemplateDTO);
                            leaveTemplateBL.Save(sqlTransaction);
                            leaveTemplateDTOLists.Add(leaveTemplateBL.GetLeaveTemplateDTO);
                            parafaitDBTrx.EndTransaction();

                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw;
                        }
                    }
                    log.LogMethodExit();
                }
            }
            return leaveTemplateDTOLists;
        }

        /// <summary>
        /// Delete the LeaveTemplate
        /// <summary>
        public void Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (leaveTemplateDTOList != null && leaveTemplateDTOList.Any())
            {
                foreach (LeaveTemplateDTO leaveTemplateDTO in leaveTemplateDTOList)
                {
                    if (leaveTemplateDTO.IsChanged && leaveTemplateDTO.LeaveTemplateId >-1)
                    {
                        try
                        {
                            LeaveTemplateBL leaveTemplateBL = new LeaveTemplateBL(executionContext, leaveTemplateDTO);
                            leaveTemplateBL.Delete( sqlTransaction);
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
                    }
                }
            }
            log.LogMethodExit();
        }
    }
}
