/********************************************************************************************
 * Project Name - Requisition Templates
 * Description  - Bussiness logic of Requisition Templates
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        08-Aug-2016   Suneetha.S          Created 
 *2.70        16-Jul-2019   Dakshakh raj        Modified : Save() method Insert/Update method returns DTO.
 *2.110.0    11-Dec-2020   Mushahid Faizan     Modified : Web Inventory changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// To create Requisition templates
    /// </summary> 
    public class RequisitionTemplatesBL
    {
        private RequisitionTemplatesDTO requisitionTemplatesDTO;
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of RequisitionTemplates class
        /// </summary>
        private RequisitionTemplatesBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the Templates id as the parameter
        /// Would fetch the Templates object from the database based on the id passed. 
        /// </summary>
        /// <param name="templateId">Req Template id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public RequisitionTemplatesBL(ExecutionContext executionContext, int templateId, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, templateId, sqlTransaction);
            RequisitionTemplatesDataHandler requisitionTemplatesDataHandler = new RequisitionTemplatesDataHandler(sqlTransaction);
            requisitionTemplatesDTO = requisitionTemplatesDataHandler.GetRequisitionTemplates(templateId);
            if (requisitionTemplatesDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, " Order Type", templateId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates templates type object using the requisitionTemplatesDTO
        /// </summary>
        /// <param name="requisitionTemplatesDTO">RequisitionTemplatesDTO object</param>
        public RequisitionTemplatesBL(ExecutionContext executionContext, RequisitionTemplatesDTO requisitionTemplatesDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(requisitionTemplatesDTO);
            this.requisitionTemplatesDTO = requisitionTemplatesDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the templates 
        /// Checks if the template id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>id</returns>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (requisitionTemplatesDTO.IsChangedRecursive == false
                && requisitionTemplatesDTO.TemplateId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            RequisitionTemplatesDataHandler reqTemplatesDataHandler = new RequisitionTemplatesDataHandler(sqlTransaction);
            Validate(sqlTransaction);
            if (requisitionTemplatesDTO.TemplateId < 0)
            {
                requisitionTemplatesDTO = reqTemplatesDataHandler.Insert(requisitionTemplatesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                requisitionTemplatesDTO.AcceptChanges();
                InventoryActivityLogDTO InventoryActivityLogDTO = new InventoryActivityLogDTO(ServerDateTime.Now, " Requisition Templates Inserted ",
                                                                requisitionTemplatesDTO.GUID, false, executionContext.GetSiteId(), "RequisitionTemplates", -1, requisitionTemplatesDTO.TemplateId + ":" + requisitionTemplatesDTO.TemplateName.ToString(), -1, executionContext.GetUserId(),
                                                                ServerDateTime.Now, executionContext.GetUserId(), ServerDateTime.Now);
                InventoryActivityLogBL inventoryActivityLogBL = new InventoryActivityLogBL(executionContext, InventoryActivityLogDTO);
                inventoryActivityLogBL.Save(sqlTransaction);
            }
            else
            {
                if (requisitionTemplatesDTO.IsChanged == true)
                {
                    requisitionTemplatesDTO = reqTemplatesDataHandler.Update(requisitionTemplatesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    requisitionTemplatesDTO.AcceptChanges();
                    InventoryActivityLogDTO InventoryActivityLogDTO = new InventoryActivityLogDTO(ServerDateTime.Now, " Requisition Templates Updated ",
                                                                requisitionTemplatesDTO.GUID, false, executionContext.GetSiteId(), "RequisitionTemplates", -1, requisitionTemplatesDTO.TemplateId + ":" + requisitionTemplatesDTO.TemplateName.ToString(), -1, executionContext.GetUserId(),
                                                                ServerDateTime.Now, executionContext.GetUserId(), ServerDateTime.Now);
                    InventoryActivityLogBL inventoryActivityLogBL = new InventoryActivityLogBL(executionContext, InventoryActivityLogDTO);
                    inventoryActivityLogBL.Save(sqlTransaction);
                }
            }

            if (requisitionTemplatesDTO.RequisitionTemplateLinesListDTO != null)
            {
                foreach (RequisitionTemplateLinesDTO requisitionTemplateLinesDTO in requisitionTemplatesDTO.RequisitionTemplateLinesListDTO)
                {
                    if (requisitionTemplateLinesDTO.TemplateId == -1)
                    {
                        requisitionTemplateLinesDTO.TemplateId = requisitionTemplatesDTO.TemplateId;
                    }
                    if (requisitionTemplateLinesDTO.IsChanged == true)
                    {
                        RequisitionTemplateLinesBL requisitionTemplateLinesBL = new RequisitionTemplateLinesBL(executionContext, requisitionTemplateLinesDTO);
                        requisitionTemplateLinesBL.Save(sqlTransaction);
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the RequisitionTemplatesDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public void Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (string.IsNullOrEmpty(requisitionTemplatesDTO.TemplateName))//Template Name shouldn't be null
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1072);
                throw new ValidationException(errorMessage);
            }
            if (requisitionTemplatesDTO.RequisitionType < 0)//Requisition Type shouldn't be null
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1065);
                throw new ValidationException(errorMessage);
            }
            if (requisitionTemplatesDTO.RequestingDept == -1 || requisitionTemplatesDTO.RequestingDept == 0)
            {
                if (requisitionTemplatesDTO.RequisitionType > -1)
                {
                    InventoryDocumentTypeList inventoryDocumentTypeList = new InventoryDocumentTypeList(executionContext);
                    InventoryDocumentTypeDTO inventoryDocumentTypeDTO = inventoryDocumentTypeList.GetInventoryDocumentType(requisitionTemplatesDTO.RequisitionType);
                    if (inventoryDocumentTypeDTO.Code == "MLRQ")
                    {
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 1066);
                        throw new ValidationException(errorMessage);
                    }
                }
                else
                {
                    requisitionTemplatesDTO.RequestingDept = -1;
                }
            }

            if (requisitionTemplatesDTO.FromDepartment < 0)//from dept shouldn't be null
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1067);
                throw new ValidationException(errorMessage);
            }
            if (requisitionTemplatesDTO.ToDepartment < 0)//to dept shouldn't be null
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1068);
                throw new ValidationException(errorMessage);
            }
            if (requisitionTemplatesDTO.ToDepartment == requisitionTemplatesDTO.FromDepartment) // from and to location should not be same
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1071);
                throw new ValidationException(errorMessage);
            }
            //if (string.IsNullOrEmpty(requisitionTemplatesDTO.Status))//template status should be selected
            //{
            //    string errorMessage = MessageContainerList.GetMessage(executionContext, 1073);
            //    throw new ValidationException(errorMessage);
            //}
            log.LogMethodExit();
        }
          
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public RequisitionTemplatesDTO GetTemplates { get { return requisitionTemplatesDTO; } }
    }

    /// <summary>
    /// Manages the list of RequisitionTemplate
    /// </summary>
    public class RequisitionTemplateList
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<RequisitionTemplatesDTO> requisitionTemplatesDTOList = new List<RequisitionTemplatesDTO>();

        /// <summary>
        /// Default constructor with executionContext
        /// </summary>
        public RequisitionTemplateList(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parmeterised constructor with 2 params
        /// </summary>
        /// <param name="requisitionTemplatesDTOList"></param>
        /// <param name="executionContext"></param>
        public RequisitionTemplateList(ExecutionContext executionContext, List<RequisitionTemplatesDTO> requisitionTemplatesDTOList)//added
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, requisitionTemplatesDTOList);
            this.requisitionTemplatesDTOList = requisitionTemplatesDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the RequisitionTemplate list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>RequisitionTemplate list</returns>
        public List<RequisitionTemplatesDTO> GetAllTemplates(List<KeyValuePair<RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
         {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            RequisitionTemplatesDataHandler reqTemplatesDataHandler = new RequisitionTemplatesDataHandler(sqlTransaction);
            List<RequisitionTemplatesDTO> requisitionTemplatesDTOList =  reqTemplatesDataHandler.GetRequisitionTemplateList(searchParameters);
            log.LogMethodExit(requisitionTemplatesDTOList);
            return requisitionTemplatesDTOList;
        }

        /// <summary>
        /// Returns the RequisitionTemplate DTO
        /// </summary>
        /// <param name="templateId">templateId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>RequisitionTemplate DTO</returns>
        public RequisitionTemplatesDTO GetTemplate(int templateId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(templateId, sqlTransaction);
            RequisitionTemplatesDataHandler reqTemplatesDataHandler = new RequisitionTemplatesDataHandler(sqlTransaction);
            RequisitionTemplatesDTO requisitionTemplatesDTO = reqTemplatesDataHandler.GetRequisitionTemplates(templateId);
            log.LogMethodExit(requisitionTemplatesDTO);
            return requisitionTemplatesDTO;
        }

        public List<RequisitionTemplatesDTO> GetAllRequisitionTemplates(List<KeyValuePair<RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters, string>> searchParameters,
           bool loadChildRecords = false, bool activeChildRecords = true, int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null)//added
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            RequisitionTemplatesDataHandler requisitionTemplatesDataHandler = new RequisitionTemplatesDataHandler(sqlTransaction);
            this.requisitionTemplatesDTOList = requisitionTemplatesDataHandler.GetRequisitionTemplatesList(searchParameters, currentPage, pageSize);
            if (requisitionTemplatesDTOList != null && requisitionTemplatesDTOList.Any() && loadChildRecords)
            {
                Build(requisitionTemplatesDTOList, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(requisitionTemplatesDTOList);
            return requisitionTemplatesDTOList;
        }

        private void Build(List<RequisitionTemplatesDTO> requisitionTemplatesDTOList, bool activeChildRecords, SqlTransaction sqlTransaction)//added
        {
            Dictionary<int, RequisitionTemplatesDTO> requisitionTemplatesDTODictionary = new Dictionary<int, RequisitionTemplatesDTO>();
            List<int> requisitionTemplatesIdList = new List<int>();
            for (int i = 0; i < requisitionTemplatesDTOList.Count; i++)
            {
                if (requisitionTemplatesDTODictionary.ContainsKey(requisitionTemplatesDTOList[i].TemplateId))
                {
                    continue;
                }
                requisitionTemplatesDTODictionary.Add(requisitionTemplatesDTOList[i].TemplateId, requisitionTemplatesDTOList[i]);
                requisitionTemplatesIdList.Add(requisitionTemplatesDTOList[i].TemplateId);
            }
            RequisitionTemplateLinesList requisitionTemplateLinesListBL = new RequisitionTemplateLinesList(executionContext);
            List<RequisitionTemplateLinesDTO> requisitionTemplateLinesDTOList = requisitionTemplateLinesListBL.GetRequisitionTemplateLinesDTOList(requisitionTemplatesIdList, sqlTransaction);

            if (requisitionTemplateLinesDTOList != null && requisitionTemplateLinesDTOList.Any())
            {
                for (int i = 0; i < requisitionTemplateLinesDTOList.Count; i++)
                {
                    if (requisitionTemplatesDTODictionary.ContainsKey(requisitionTemplateLinesDTOList[i].TemplateId) == false)
                    {
                        continue;
                    }
                    RequisitionTemplatesDTO requisitionTemplatesDTO = requisitionTemplatesDTODictionary[requisitionTemplateLinesDTOList[i].TemplateId];
                    if (requisitionTemplatesDTO.RequisitionTemplateLinesListDTO == null)
                    {
                        requisitionTemplatesDTO.RequisitionTemplateLinesListDTO = new List<RequisitionTemplateLinesDTO>();
                    }
                    requisitionTemplatesDTO.RequisitionTemplateLinesListDTO.Add(requisitionTemplateLinesDTOList[i]);
                }
            }
        }

        /// <summary>
        /// Returns the no of Requisition Templates matching the search Parameters
        /// </summary>
        /// <param name="searchParameters"> search criteria</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        /// <returns></returns>
        public int GetRequisitionTemplatesCount(List<KeyValuePair<RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)//added
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            RequisitionTemplatesDataHandler requisitionTemplatesDataHandler = new RequisitionTemplatesDataHandler(sqlTransaction);
            int requisitionTemplatesCount = requisitionTemplatesDataHandler.GetRequisitionTemplatesCount(searchParameters);
            log.LogMethodExit(requisitionTemplatesCount);
            return requisitionTemplatesCount;
        }        
    }
}
