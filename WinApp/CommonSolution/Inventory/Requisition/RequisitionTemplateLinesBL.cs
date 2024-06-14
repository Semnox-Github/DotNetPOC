/********************************************************************************************
 * Project Name - Requisition Templates
 * Description  - Bussiness logic of Requisition Templates Lines
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        11-Aug-2016   Suneetha.S          Created 
 *2.70        16-Jul-2019   Dakshakh raj        Modified : Save() method Insert/Update method returns DTO.
 *2.110.0    15-Dec-2020   Mushahid Faizan     Modified : Web Inventory Changes
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
    public class RequisitionTemplateLinesBL
    {
       private RequisitionTemplateLinesDTO requisitionTemplateLinesDTO;
       private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of Requisition Template Lines class
        /// </summary>
        public RequisitionTemplateLinesBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the Templates id as the parameter
        /// Would fetch the Templates object from the database based on the id passed. 
        /// </summary>
        /// <param name="templateLineId">Req Template Line id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public RequisitionTemplateLinesBL(ExecutionContext executionContext, int templateLineId, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, templateLineId, sqlTransaction);
            RequisitionTemplatesLinesDH requisitionTemplatesLinesDH = new RequisitionTemplatesLinesDH(sqlTransaction);
            requisitionTemplateLinesDTO = requisitionTemplatesLinesDH.GetRequisitionTemplateLine(templateLineId);
            if (requisitionTemplateLinesDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, " Requisition Template Lines ", templateLineId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates templates type object using the reqTemplatesDTO
        /// </summary>
        /// <param name="requisitionTemplateLinesDTO">RequisitionTemplateLinesDTO object</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public RequisitionTemplateLinesBL(ExecutionContext executionContext, RequisitionTemplateLinesDTO requisitionTemplateLinesDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext,requisitionTemplateLinesDTO); 
            this.requisitionTemplateLinesDTO = requisitionTemplateLinesDTO;
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
            if (requisitionTemplateLinesDTO.IsChanged == false
                && requisitionTemplateLinesDTO.TemplateLineId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            RequisitionTemplatesLinesDH reqTemplatesLinesDataHandler = new RequisitionTemplatesLinesDH(sqlTransaction);
            List<ValidationError> validationErrors = Validate(sqlTransaction);
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }          
            if (requisitionTemplateLinesDTO.TemplateLineId < 0)
            {
                requisitionTemplateLinesDTO = reqTemplatesLinesDataHandler.Insert(requisitionTemplateLinesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                requisitionTemplateLinesDTO.AcceptChanges();
            }
            else
            {
                if (requisitionTemplateLinesDTO.IsChanged)
                {
                    requisitionTemplateLinesDTO = reqTemplatesLinesDataHandler.Update(requisitionTemplateLinesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    requisitionTemplateLinesDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the RequisitionTemplateLinesDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public RequisitionTemplateLinesDTO GetTemplates { get { return requisitionTemplateLinesDTO; } }
    }

    /// <summary>
    /// Manages the list of RequisitionTemplate
    /// </summary>
    public class RequisitionTemplateLinesList
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<RequisitionTemplateLinesDTO> requisitionTemplateLinesDTOList= new List<RequisitionTemplateLinesDTO>();

        /// <summary>
        /// Default constructor with executionContext
        /// </summary>
        public RequisitionTemplateLinesList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parmeterised constructor with 2 params
        /// </summary>
        /// <param name="requisitionLinesDTOList"></param>
        /// <param name="executionContext"></param>
        public RequisitionTemplateLinesList(ExecutionContext executionContext, List<RequisitionTemplateLinesDTO> requisitionTemplateLinesDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, requisitionTemplateLinesDTOList);
            this.requisitionTemplateLinesDTOList = requisitionTemplateLinesDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the RequisitionTemplate list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>RequisitionTemplate list</returns>
        public List<RequisitionTemplateLinesDTO> GetAllTemplateLines(List<KeyValuePair<RequisitionTemplateLinesDTO.SearchByTemplateLinesParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            RequisitionTemplatesLinesDH reqTemplateLinesDataHandler = new RequisitionTemplatesLinesDH(sqlTransaction);
            List<RequisitionTemplateLinesDTO> requisitionTemplateLinesDTOList = reqTemplateLinesDataHandler.GetRequisitionTemplateList(searchParameters);
            log.LogMethodExit(requisitionTemplateLinesDTOList);
            return requisitionTemplateLinesDTOList;
        }

        /// <summary>
        /// Gets the RequisitionTemplateLinesDTO List for requisitionTemplatesIdList
        /// </summary>
        /// <param name="requisitionTemplatesIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of requisitionTemplateLinesDTOList</returns>
        public List<RequisitionTemplateLinesDTO> GetRequisitionTemplateLinesDTOList(List<int> requisitionTemplatesIdList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(requisitionTemplatesIdList);
            RequisitionTemplatesLinesDH requisitionTemplatesLinesDH = new RequisitionTemplatesLinesDH(sqlTransaction);
            this.requisitionTemplateLinesDTOList = requisitionTemplatesLinesDH.GetRequisitionTemplateLinesDTOList(requisitionTemplatesIdList, sqlTransaction);
            log.LogMethodExit(requisitionTemplateLinesDTOList);
            return requisitionTemplateLinesDTOList;
        }

        /// <summary>
        /// Returns the RequisitionTemplate DTO
        /// </summary>
        /// <param name="templateId">templateId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>RequisitionTemplate </returns>
        /// <returns>RequisitionTemplate DTO</returns>
        public RequisitionTemplateLinesDTO GetTemplateLine(int templateId,SqlTransaction sqlTransaction= null)
        {
            log.LogMethodEntry(templateId, sqlTransaction);
            RequisitionTemplatesLinesDH reqTemplateLinesDataHandler = new RequisitionTemplatesLinesDH(sqlTransaction);
            RequisitionTemplateLinesDTO requisitionTemplateLinesDTO = reqTemplateLinesDataHandler.GetRequisitionTemplateLine(templateId);
            log.LogMethodExit(requisitionTemplateLinesDTO);
            return requisitionTemplateLinesDTO;
        }

        /// <summary>
        /// Returns the List of Requisitions DTO
        /// </summary>
        /// <param name="templateId">templateId</param>
        /// <param name="isActive">isActive</param>
        /// <param name="locationId">locationId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Requisitions DTO List</returns>
        public List<RequisitionTemplateLinesDTO> GetRequisitionsTemplateListForLoad(int templateId, bool isActive, int locationId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(templateId, isActive, locationId, sqlTransaction);
            RequisitionTemplatesLinesDH requisitionLinesDataHandler = new RequisitionTemplatesLinesDH(sqlTransaction);
            List<RequisitionTemplateLinesDTO> requisitionTemplateLinesDTOList = requisitionLinesDataHandler.GetRequisitionTemplateLineList(templateId, isActive, locationId);
            log.LogMethodExit(requisitionTemplateLinesDTOList);
            return requisitionTemplateLinesDTOList;
        }
    }
}
