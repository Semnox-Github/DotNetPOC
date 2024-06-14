/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - HRApproval logs BL
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.140.0     23-June-2021  Prashanth            Created for POS UI Redesign 
 ********************************************************************************************/

using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.User
{
    public class HRApprovalLogsBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private HRApprovalLogsDTO hrApprovalLogsDTO;

        public HRApprovalLogsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public HRApprovalLogsBL(ExecutionContext executionContext, HRApprovalLogsDTO hrApprovalLogsDTO) : this(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.hrApprovalLogsDTO = hrApprovalLogsDTO;
            log.LogMethodExit();
        }

        public HRApprovalLogsBL(ExecutionContext executionContext, string action, string approvalLevel, string approverId,
            string createdBy, string lastUpdatedBy, string entity, int posMachineId, string remarks, DateTime? approvalTime = null) : this(executionContext)
        {
            log.LogMethodEntry();
            if (hrApprovalLogsDTO == null)
            {
                hrApprovalLogsDTO = new HRApprovalLogsDTO();
            }
            hrApprovalLogsDTO.Action = action;
            hrApprovalLogsDTO.ApprovalLevel = approvalLevel;
            hrApprovalLogsDTO.ApprovalTime = approvalTime;
            hrApprovalLogsDTO.ApproverId = approverId;
            hrApprovalLogsDTO.CreatedBy = createdBy;
            hrApprovalLogsDTO.LastUpdatedBy = lastUpdatedBy;
            hrApprovalLogsDTO.Entity = entity;
            hrApprovalLogsDTO.POSMachineId = posMachineId;
            hrApprovalLogsDTO.Remarks = remarks;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the HRApprovalLogs details to database
        /// </summary>
        public void Save(SqlTransaction sqltrxn)
        {
            log.LogMethodEntry(sqltrxn);
            if (hrApprovalLogsDTO.IsChanged == false)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            HRApprovalLogsDataHandler hrApprovalLogsDataHandler = new HRApprovalLogsDataHandler(sqltrxn);
            if (hrApprovalLogsDTO.ApprovalLogId < 0)
            {
                hrApprovalLogsDTO = hrApprovalLogsDataHandler.InsertHRApprovalLogs(hrApprovalLogsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                hrApprovalLogsDTO.AcceptChanges();
            }
            else if (hrApprovalLogsDTO.IsChanged)
            {
                hrApprovalLogsDTO = hrApprovalLogsDataHandler.Update(hrApprovalLogsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                hrApprovalLogsDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// gets the GetHRApprovalLogsDTO
        /// </summary>
        public HRApprovalLogsDTO GetHRApprovalLogsDTO
        {
            get { return hrApprovalLogsDTO; }
        }

    }

    public class HRApprovalLogsListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<HRApprovalLogsDTO> hrApprovalLogsDTOList = new List<HRApprovalLogsDTO>();
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public HRApprovalLogsListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="hrApprovalLogsDTOList"></param>
        public HRApprovalLogsListBL(ExecutionContext executionContext,
                                                List<HRApprovalLogsDTO> hrApprovalLogsDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, hrApprovalLogsDTOList);
            this.hrApprovalLogsDTOList = hrApprovalLogsDTOList;
            log.LogMethodExit();
        }


        /// <summary>
        ///  Returns the HRApprovalLogsDTO list
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns>HRApprovalLogsDTOList</returns>
        public List<HRApprovalLogsDTO> GetHRApprovalLogsDTOList(List<KeyValuePair<HRApprovalLogsDTO.SearchByParameters, string>> searchParameters,
                                                         SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<HRApprovalLogsDTO> hrApprovalLogDTOs = new List<HRApprovalLogsDTO>();
            HRApprovalLogsDataHandler hrApprovalLogsDataHandler = new HRApprovalLogsDataHandler(sqlTransaction);
            hrApprovalLogDTOs = hrApprovalLogsDataHandler.GetHRApprovalLogsDTOs(searchParameters, sqlTransaction);
            log.LogMethodExit(hrApprovalLogDTOs);
            return hrApprovalLogDTOs;
        }

        public void Save(SqlTransaction sqlTransaction)
        {
            try
            {
                if (hrApprovalLogsDTOList != null && hrApprovalLogsDTOList.Count != 0)
                {
                    foreach (HRApprovalLogsDTO hrApprovalLogsDTO in hrApprovalLogsDTOList)
                    {
                        HRApprovalLogsBL hrApprovalLogsBL = new HRApprovalLogsBL(executionContext, hrApprovalLogsDTO);
                        hrApprovalLogsBL.Save(sqlTransaction);
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
        }
    }
}
