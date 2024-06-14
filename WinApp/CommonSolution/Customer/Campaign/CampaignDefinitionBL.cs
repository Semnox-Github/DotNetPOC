/********************************************************************************************
 * Project Name - CampaignDefinition BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.110.0    19-Jan-2020      Prajwal S       Created                                                
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.GenericUtilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer.Campaign
{
    /// <summary>
    /// Business logic for CampaignDefinition class.
    /// </summ
    public class CampaignDefinitionBL
    {
        CampaignDefinitionDTO campaignDefinitionDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        private CampaignDefinitionBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with the CampaignDefinitionId parameter
        /// </summary>
        /// <param name="campaignDefinitionId">CampaignDefinitionId</param>
        /// <param name="loadChildRecords">To load the child DTO Records</param>
        public CampaignDefinitionBL(ExecutionContext executionContext, int campaignDefinitionId, bool loadChildRecords = false, bool activeChildRecords = false, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(campaignDefinitionId, loadChildRecords, activeChildRecords);
            LoadCampaignDefinition(campaignDefinitionId, loadChildRecords, activeChildRecords, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the CampaignDefinition id as the parameter
        /// Would fetch the CampaignDefinition object from the database based on the id passed. 
        /// </summary>
        /// <param name="id">Id</param>
        private void LoadCampaignDefinition(int id, bool loadChildRecords = false, bool activeChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(id, executionContext, sqlTransaction);
            CampaignDefinitionDataHandler campaignDefinitionDataHandler = new CampaignDefinitionDataHandler(sqlTransaction);
            campaignDefinitionDTO = campaignDefinitionDataHandler.GetCampaignDefinition(id);
            ThrowIfCampaignDefinitionIsNull(id);
            if (loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(campaignDefinitionDTO);
        }

        private void ThrowIfCampaignDefinitionIsNull(int campaignDefinitionId)
        {
            log.LogMethodEntry();
            if (campaignDefinitionDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "CampaignDefinition", campaignDefinitionId);
                log.LogMethodExit(null, "Throwing Exception - "+ message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="parameterscampaignDefinitionDTO">scampaignDefinitionDTO</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public CampaignDefinitionBL(ExecutionContext executionContext, CampaignDefinitionDTO parameterscampaignDefinitionDTO, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, parameterscampaignDefinitionDTO, sqlTransaction);

            if (parameterscampaignDefinitionDTO.CampaignDefinitionId > -1)
            {
                LoadCampaignDefinition(parameterscampaignDefinitionDTO.CampaignDefinitionId, true, true, sqlTransaction);//added sql
                ThrowIfCampaignDefinitionIsNull(parameterscampaignDefinitionDTO.CampaignDefinitionId);
                Update(parameterscampaignDefinitionDTO, sqlTransaction);
            }
            else
            {
                ValidateName(parameterscampaignDefinitionDTO.Name);
                ValidateDescription(parameterscampaignDefinitionDTO.Description);
                ValidateStartDateAndEndDate(parameterscampaignDefinitionDTO.StartDate, parameterscampaignDefinitionDTO.EndDate);
                campaignDefinitionDTO = new CampaignDefinitionDTO(-1, parameterscampaignDefinitionDTO.Name, parameterscampaignDefinitionDTO.Description, parameterscampaignDefinitionDTO.StartDate, parameterscampaignDefinitionDTO.EndDate, parameterscampaignDefinitionDTO.Recurr, parameterscampaignDefinitionDTO.ScheduleId, parameterscampaignDefinitionDTO.IsActive);
                if (parameterscampaignDefinitionDTO.ScheduleCalendarDTO != null)
                {
                    if (parameterscampaignDefinitionDTO.ScheduleCalendarDTO.ScheduleId > -1)
                    {
                        string message = MessageContainerList.GetMessage(executionContext, 2196, "ScheduleCalendar", parameterscampaignDefinitionDTO.ScheduleCalendarDTO.ScheduleId);
                        log.LogMethodExit(null, "Throwing Exception - " + message);
                        throw new EntityNotFoundException(message);
                    }
                    ScheduleCalendarDTO parameterScheduleCalendarDTO = parameterscampaignDefinitionDTO.ScheduleCalendarDTO;
                    var scheduleCalendarDTO = new ScheduleCalendarDTO(-1, parameterScheduleCalendarDTO.ScheduleName, parameterScheduleCalendarDTO.ScheduleTime, parameterScheduleCalendarDTO.ScheduleEndDate, parameterScheduleCalendarDTO.RecurFlag, parameterScheduleCalendarDTO.RecurFrequency, parameterScheduleCalendarDTO.RecurEndDate, parameterScheduleCalendarDTO.RecurType, parameterScheduleCalendarDTO.IsActive);
                    scheduleCalendarDTO.ScheduleCalendarExclusionDTOList = parameterScheduleCalendarDTO.ScheduleCalendarExclusionDTOList;
                    scheduleCalendarDTO.JobScheduleDTOList = parameterScheduleCalendarDTO.JobScheduleDTOList;
                    ScheduleCalendarBL scheduleCalendarBL = new ScheduleCalendarBL(executionContext, scheduleCalendarDTO);
                    List<ValidationError> validationErrors = scheduleCalendarBL.Validate(sqlTransaction);
                    if (validationErrors.Any())
                    {
                        string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                        log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                        throw new ValidationException(message, validationErrors);
                    }
                    campaignDefinitionDTO.ScheduleCalendarDTO = scheduleCalendarBL.ScheduleCalendarDTO;
                }

                if (parameterscampaignDefinitionDTO.CampaignDiscountDefinitionDTO != null)
                {
                    campaignDefinitionDTO.CampaignDiscountDefinitionDTO = new CampaignDiscountDefinitionDTO();
                    if (parameterscampaignDefinitionDTO.CampaignDiscountDefinitionDTO.CampaignDiscountDefinitionId > -1)
                    {
                        string message = MessageContainerList.GetMessage(executionContext, 2196, "CampaignDiscountDefinition", parameterscampaignDefinitionDTO.CampaignDiscountDefinitionDTO.CampaignDiscountDefinitionId);
                        log.LogMethodExit(null, "Throwing Exception - " + message);
                        throw new EntityNotFoundException(message);
                    }
                    CampaignDiscountDefinitionDTO parameterCampaignDiscountDefinitionDTO = parameterscampaignDefinitionDTO.CampaignDiscountDefinitionDTO;
                    var CampaignDiscountDefinitionDTO = new CampaignDiscountDefinitionDTO(-1, -1, parameterCampaignDiscountDefinitionDTO.DiscountId, parameterCampaignDiscountDefinitionDTO.ExpiryDate, parameterCampaignDiscountDefinitionDTO.ValidFor, parameterCampaignDiscountDefinitionDTO.ValidForDaysMonths, parameterCampaignDiscountDefinitionDTO.IsActive);
                    CampaignDiscountDefinitionBL campaignDiscountDefinitionBL = new CampaignDiscountDefinitionBL(executionContext, CampaignDiscountDefinitionDTO);
                    campaignDefinitionDTO.CampaignDiscountDefinitionDTO = campaignDiscountDefinitionBL.CampaignDiscountDefinitionDTO;
                }

                if (parameterscampaignDefinitionDTO.CampaignCommunicationDefinitionDTOList != null && parameterscampaignDefinitionDTO.CampaignCommunicationDefinitionDTOList.Any())
                {
                    campaignDefinitionDTO.CampaignCommunicationDefinitionDTOList = new List<CampaignCommunicationDefinitionDTO>();
                    foreach (CampaignCommunicationDefinitionDTO parameterCampaignCommunicationDefinitionDTO in parameterscampaignDefinitionDTO.CampaignCommunicationDefinitionDTOList)
                    {
                        if (parameterCampaignCommunicationDefinitionDTO.CampaignCommunicationDefinitionId > -1)
                        {
                            string message = MessageContainerList.GetMessage(executionContext, 2196, "CampaignCommunicationDefinition", parameterCampaignCommunicationDefinitionDTO.CampaignCommunicationDefinitionId);
                            log.LogMethodExit(null, "Throwing Exception - " + message);
                            throw new EntityNotFoundException(message);
                        }
                        var campaignCommunicationDefinitionDTO = new CampaignCommunicationDefinitionDTO(-1, -1, parameterCampaignCommunicationDefinitionDTO.MessagingClientId, parameterCampaignCommunicationDefinitionDTO.MessageTemplateId, parameterCampaignCommunicationDefinitionDTO.Retry, parameterCampaignCommunicationDefinitionDTO.IsActive);
                        CampaignCommunicationDefinitionBL campaignCommunicationDefinitionBL = new CampaignCommunicationDefinitionBL(executionContext, campaignCommunicationDefinitionDTO);
                        campaignDefinitionDTO.CampaignCommunicationDefinitionDTOList.Add(campaignCommunicationDefinitionBL.CampaignCommunicationDefinitionDTO);
                    }
                }

                if (parameterscampaignDefinitionDTO.CampaignCustomerProfileMapDTOList != null && parameterscampaignDefinitionDTO.CampaignCustomerProfileMapDTOList.Any())
                {
                    campaignDefinitionDTO.CampaignCustomerProfileMapDTOList = new List<CampaignCustomerProfileMapDTO>();
                    foreach (CampaignCustomerProfileMapDTO parameterCampaignCustomerProfileMapDTO in parameterscampaignDefinitionDTO.CampaignCustomerProfileMapDTOList)
                    {
                        if (parameterCampaignCustomerProfileMapDTO.CampaignCustomerProfileMapId > -1)
                        {
                            string message = MessageContainerList.GetMessage(executionContext, 2196, "CampaignCustomerProfileMap", parameterCampaignCustomerProfileMapDTO.CampaignCustomerProfileMapId);
                            log.LogMethodExit(null, "Throwing Exception - " + message);
                            throw new EntityNotFoundException(message);
                        }
                        var campaignCustomerProfileMapDTO = new CampaignCustomerProfileMapDTO(-1, -1, parameterCampaignCustomerProfileMapDTO.CampaignCustomerProfileId, parameterCampaignCustomerProfileMapDTO.IsActive);
                        CampaignCustomerProfileMapBL campaignCustomerProfileMapBL = new CampaignCustomerProfileMapBL(executionContext, campaignCustomerProfileMapDTO);
                        campaignDefinitionDTO.CampaignCustomerProfileMapDTOList.Add(campaignCustomerProfileMapBL.CampaignCustomerProfileMapDTO);
                    }

                }

            }
            log.LogMethodExit();
        }
        

        private void Update(CampaignDefinitionDTO parametercampaignDefinitionDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parametercampaignDefinitionDTO, sqlTransaction);
            ChangeName(parametercampaignDefinitionDTO.Name);
            ChangeDescription(parametercampaignDefinitionDTO.Description);
            ChangeStartDateAndEndDate(parametercampaignDefinitionDTO.StartDate, parametercampaignDefinitionDTO.EndDate);
            ChangeIsActive(parametercampaignDefinitionDTO.IsActive);
            ChangeRecurr(parametercampaignDefinitionDTO.Recurr); ;
            if (parametercampaignDefinitionDTO.ScheduleCalendarDTO != null)
            {
                if(campaignDefinitionDTO.ScheduleCalendarDTO == null)
                {
                    ScheduleCalendarDTO parameterScheduleCalendarDTO = parametercampaignDefinitionDTO.ScheduleCalendarDTO;
                    var scheduleCalendarDTO = new ScheduleCalendarDTO(-1, parameterScheduleCalendarDTO.ScheduleName, parameterScheduleCalendarDTO.ScheduleTime, parameterScheduleCalendarDTO.ScheduleEndDate, parameterScheduleCalendarDTO.RecurFlag, parameterScheduleCalendarDTO.RecurFrequency, parameterScheduleCalendarDTO.RecurEndDate, parameterScheduleCalendarDTO.RecurType, parameterScheduleCalendarDTO.IsActive);
                    scheduleCalendarDTO.JobScheduleDTOList = parameterScheduleCalendarDTO.JobScheduleDTOList;
                    scheduleCalendarDTO.ScheduleCalendarExclusionDTOList = parameterScheduleCalendarDTO.ScheduleCalendarExclusionDTOList;
                    ScheduleCalendarBL scheduleCalendarBL = new ScheduleCalendarBL(executionContext, scheduleCalendarDTO);
                    List<ValidationError> validationErrors = scheduleCalendarBL.Validate(sqlTransaction);
                    if (validationErrors.Any())
                    {
                        string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                        log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                        throw new ValidationException(message, validationErrors);
                    }
                    campaignDefinitionDTO.ScheduleCalendarDTO = scheduleCalendarBL.ScheduleCalendarDTO;
                }
                else 
                {
                    ScheduleCalendarBL scheduleCalendarBL = new ScheduleCalendarBL(executionContext, campaignDefinitionDTO.ScheduleCalendarDTO);
                    scheduleCalendarBL.Update(parametercampaignDefinitionDTO.ScheduleCalendarDTO, sqlTransaction);
                }
            }

            if (parametercampaignDefinitionDTO.CampaignDiscountDefinitionDTO != null)
            {
                if (campaignDefinitionDTO.CampaignDiscountDefinitionDTO == null)
                {
                    CampaignDiscountDefinitionDTO parameterCampaignDiscountDefinitionDTO = parametercampaignDefinitionDTO.CampaignDiscountDefinitionDTO;
                    var CampaignDiscountDefinitionDTO = new CampaignDiscountDefinitionDTO(-1, -1, parameterCampaignDiscountDefinitionDTO.DiscountId, parameterCampaignDiscountDefinitionDTO.ExpiryDate, parameterCampaignDiscountDefinitionDTO.ValidFor, parameterCampaignDiscountDefinitionDTO.ValidForDaysMonths, parameterCampaignDiscountDefinitionDTO.IsActive);
                    CampaignDiscountDefinitionBL campaignDiscountDefinitionBL = new CampaignDiscountDefinitionBL(executionContext, CampaignDiscountDefinitionDTO);
                    campaignDefinitionDTO.CampaignDiscountDefinitionDTO = campaignDiscountDefinitionBL.CampaignDiscountDefinitionDTO;
                }
                else
                {
                    CampaignDiscountDefinitionBL campaignDiscountDefinitionBL = new CampaignDiscountDefinitionBL(executionContext, campaignDefinitionDTO.CampaignDiscountDefinitionDTO);
                    campaignDiscountDefinitionBL.Update(parametercampaignDefinitionDTO.CampaignDiscountDefinitionDTO, sqlTransaction);
                }
            }

            Dictionary<int, CampaignCommunicationDefinitionDTO> campaignCommunicationDefinitionDTODictionary = new Dictionary<int, CampaignCommunicationDefinitionDTO>();
            if (campaignDefinitionDTO.CampaignCommunicationDefinitionDTOList != null &&
                campaignDefinitionDTO.CampaignCommunicationDefinitionDTOList.Any())
            {
                foreach (var campaignCommunicationDefinitionDTO in campaignDefinitionDTO.CampaignCommunicationDefinitionDTOList)
                {
                    campaignCommunicationDefinitionDTODictionary.Add(campaignCommunicationDefinitionDTO.CampaignCommunicationDefinitionId, campaignCommunicationDefinitionDTO);
                }
             }
            if (parametercampaignDefinitionDTO.CampaignCommunicationDefinitionDTOList != null &&
                parametercampaignDefinitionDTO.CampaignCommunicationDefinitionDTOList.Any())
            {
                foreach (var parameterCampaignCommunicationDefinitionDTO in parametercampaignDefinitionDTO.CampaignCommunicationDefinitionDTOList)
                {
                    if (campaignCommunicationDefinitionDTODictionary.ContainsKey(parameterCampaignCommunicationDefinitionDTO.CampaignCommunicationDefinitionId))
                    {
                        CampaignCommunicationDefinitionBL campaignCommunicationDefinition = new CampaignCommunicationDefinitionBL(executionContext, campaignCommunicationDefinitionDTODictionary[parameterCampaignCommunicationDefinitionDTO.CampaignCommunicationDefinitionId]);
                        campaignCommunicationDefinition.Update(parameterCampaignCommunicationDefinitionDTO, sqlTransaction);
                    }
                    else if (parameterCampaignCommunicationDefinitionDTO.CampaignCommunicationDefinitionId > -1)
                    {
                        CampaignCommunicationDefinitionBL campaignCommunicationDefinition = new CampaignCommunicationDefinitionBL(executionContext, parameterCampaignCommunicationDefinitionDTO.CampaignCommunicationDefinitionId, sqlTransaction);
                        if (campaignDefinitionDTO.CampaignCommunicationDefinitionDTOList == null)
                        {
                            campaignDefinitionDTO.CampaignCommunicationDefinitionDTOList = new List<CampaignCommunicationDefinitionDTO>();
                        }
                        campaignDefinitionDTO.CampaignCommunicationDefinitionDTOList.Add(campaignCommunicationDefinition.CampaignCommunicationDefinitionDTO);
                        campaignCommunicationDefinition.Update(parameterCampaignCommunicationDefinitionDTO, sqlTransaction);
                    }
                    else
                    {
                        CampaignCommunicationDefinitionBL campaignCommunicationDefinitionBL = new CampaignCommunicationDefinitionBL(executionContext, parameterCampaignCommunicationDefinitionDTO);
                        if (campaignDefinitionDTO.CampaignCommunicationDefinitionDTOList == null)
                        {
                            campaignDefinitionDTO.CampaignCommunicationDefinitionDTOList = new List<CampaignCommunicationDefinitionDTO>();
                        }
                        campaignDefinitionDTO.CampaignCommunicationDefinitionDTOList.Add(campaignCommunicationDefinitionBL.CampaignCommunicationDefinitionDTO);
                    }
                }
            }

            Dictionary<int, CampaignCustomerProfileMapDTO> campaignCustomerProfileMapDTODictionary = new Dictionary<int, CampaignCustomerProfileMapDTO>();
            if (campaignDefinitionDTO.CampaignCustomerProfileMapDTOList != null &&
                campaignDefinitionDTO.CampaignCustomerProfileMapDTOList.Any())
            {
                foreach (var campaignCustomerProfileMapDTO in campaignDefinitionDTO.CampaignCustomerProfileMapDTOList)
                {
                    campaignCustomerProfileMapDTODictionary.Add(campaignCustomerProfileMapDTO.CampaignCustomerProfileMapId, campaignCustomerProfileMapDTO);
                }
            }
            if (parametercampaignDefinitionDTO.CampaignCustomerProfileMapDTOList != null &&
                parametercampaignDefinitionDTO.CampaignCustomerProfileMapDTOList.Any())
            {
                foreach (var parameterCampaignCustomerProfileMapDTO in parametercampaignDefinitionDTO.CampaignCustomerProfileMapDTOList)
                {
                    if (campaignCustomerProfileMapDTODictionary.ContainsKey(parameterCampaignCustomerProfileMapDTO.CampaignCustomerProfileMapId))
                    {
                        CampaignCustomerProfileMapBL campaignCustomerProfileMap = new CampaignCustomerProfileMapBL(executionContext, campaignCustomerProfileMapDTODictionary[parameterCampaignCustomerProfileMapDTO.CampaignCustomerProfileMapId]);
                        campaignCustomerProfileMap.Update(parameterCampaignCustomerProfileMapDTO, sqlTransaction);
                    }
                    else if (parameterCampaignCustomerProfileMapDTO.CampaignCustomerProfileMapId > -1)
                    {
                        CampaignCustomerProfileMapBL campaignCustomerProfileMap = new CampaignCustomerProfileMapBL(executionContext, parameterCampaignCustomerProfileMapDTO.CampaignCustomerProfileMapId, sqlTransaction);
                        if (campaignDefinitionDTO.CampaignCustomerProfileMapDTOList == null)
                        {
                            campaignDefinitionDTO.CampaignCustomerProfileMapDTOList = new List<CampaignCustomerProfileMapDTO>();
                        }
                        campaignDefinitionDTO.CampaignCustomerProfileMapDTOList.Add(campaignCustomerProfileMap.CampaignCustomerProfileMapDTO);
                        campaignCustomerProfileMap.Update(parameterCampaignCustomerProfileMapDTO, sqlTransaction);
                    }
                    else
                    {
                        CampaignCustomerProfileMapBL campaignCustomerProfileMapBL = new CampaignCustomerProfileMapBL(executionContext, parameterCampaignCustomerProfileMapDTO);
                        if (campaignDefinitionDTO.CampaignCustomerProfileMapDTOList == null)
                        {
                            campaignDefinitionDTO.CampaignCustomerProfileMapDTOList = new List<CampaignCustomerProfileMapDTO>();
                        }
                        campaignDefinitionDTO.CampaignCustomerProfileMapDTOList.Add(campaignCustomerProfileMapBL.CampaignCustomerProfileMapDTO);
                    }
                }
            }

        }

        public void ChangeIsActive(bool isActive)
        {
            log.LogMethodEntry(isActive);
            if (campaignDefinitionDTO.IsActive == isActive)
            {
                log.LogMethodExit(null, "No changes to campaignDefinition IsActive");
                return;
            }
            campaignDefinitionDTO.IsActive = isActive;
            log.LogMethodExit();
        }

        public void ChangeRecurr(bool recurr)
        {
            log.LogMethodEntry(recurr);
            if (campaignDefinitionDTO.Recurr == recurr)
            {
                log.LogMethodExit(null, "No changes to campaignDefinition Recurr");
                return;
            }
            campaignDefinitionDTO.Recurr = recurr;
            log.LogMethodExit();
        }

        public void ChangeStartDateAndEndDate(DateTime? startDate, DateTime? endDate)
        {
            log.LogMethodEntry(startDate, endDate);
            if (campaignDefinitionDTO.StartDate == startDate)
            {
                log.LogMethodExit(null, "No changes to campaignDefinition startDate");
                return;
            }
            campaignDefinitionDTO.StartDate = startDate;
            if (campaignDefinitionDTO.EndDate == endDate)
            {
                log.LogMethodExit(null, "No changes to campaignDefinition endDate");
                return;
            }
            campaignDefinitionDTO.EndDate = endDate;
            ValidateStartDateAndEndDate(startDate, endDate);
            log.LogMethodExit();
        }

        public void ChangeName(string name)
        {
            log.LogMethodEntry(name);
            if (campaignDefinitionDTO.Name == name)
            {
                log.LogMethodExit(null, "No changes to campaignDefinition Name");
                return;
            }
            ValidateName(name);
            campaignDefinitionDTO.Name = name;
            log.LogMethodExit();
        }

        public void ChangeDescription(string description)
        {
            log.LogMethodEntry(description);
            if (campaignDefinitionDTO.Description == description)
            {
                log.LogMethodExit(null, "No changes to campaignDefinition Description");
                return;
            }
            ValidateDescription(description);
            campaignDefinitionDTO.Description = description;
            log.LogMethodExit();
        }

        private void ValidateName(string name)
        {
            log.LogMethodEntry(name);
            if (string.IsNullOrWhiteSpace(name))
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1132);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("Name is empty.", "campaignDefinition", "name", errorMessage);
            }
            if (name.Length > 100)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2197, MessageContainerList.GetMessage(executionContext, "Name"), 100);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("Name greater than 100 characters.", "campaignDefinition", "name", errorMessage);
            }
            log.LogMethodExit();
        }

        private void ValidateDescription(string description)
        {
            log.LogMethodEntry(description);
            if (string.IsNullOrWhiteSpace(description))
            {
                log.LogMethodExit(null, "description empty");
                return;
            }
            if (description.Length > 100)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2197, MessageContainerList.GetMessage(executionContext, "Description"), 100);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("description greater than 100 characters.", "campaignDefinition", "description", errorMessage);
            }
            log.LogMethodExit();
        }

        private void ValidateStartDateAndEndDate(DateTime? startDate, DateTime? endDate)
        {
            log.LogMethodEntry(startDate, endDate);
            if (startDate != DateTime.MinValue &&
                endDate != DateTime.MinValue &&
                startDate > endDate)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "EndDate"));
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException(errorMessage, "CampaignDefinition", "EndDate", errorMessage);
            }
            log.LogMethodExit();
        }



        /// <summary>
        /// Builds the child records for CampaignDefinition object.
        /// </summary>
        /// <param name="activeChildRecords">activeChildRecords holds either true or false</param>
        /// <param name="sqlTransaction"></param>
        private void Build(bool activeChildRecords, SqlTransaction sqlTransaction)    //added build
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            ScheduleCalendarListBL scheduleCalendarListBL = new ScheduleCalendarListBL(executionContext);
            List<ScheduleCalendarDTO> scheduleCalendarDTOList = scheduleCalendarListBL.GetScheduleCalendarDTOList(new List<int>() { campaignDefinitionDTO.ScheduleId }, false, sqlTransaction);
            if (scheduleCalendarDTOList.Count > 0)
            {
                campaignDefinitionDTO.ScheduleCalendarDTO = scheduleCalendarDTOList[0];
            }

            CampaignDiscountDefinitionListBL campaignDiscountDefinitionListBL = new CampaignDiscountDefinitionListBL(executionContext);
            List<CampaignDiscountDefinitionDTO> campaignDiscountDefinitionDTOList = campaignDiscountDefinitionListBL.GetCampaignDiscountDefinitionDTOList(new List<int>() { campaignDefinitionDTO.CampaignDefinitionId }, false, sqlTransaction);
            if (campaignDiscountDefinitionDTOList.Count > 0)
            {
                campaignDefinitionDTO.CampaignDiscountDefinitionDTO = campaignDiscountDefinitionDTOList[0];
            }

            CampaignCommunicationDefinitionListBL campaignCommunicationDefinitionListBL = new CampaignCommunicationDefinitionListBL(executionContext);
            List<CampaignCommunicationDefinitionDTO> campaignCommunicationDefinitionDTOList = campaignCommunicationDefinitionListBL.GetCampaignCommunicationDefinitionDTOList(new List<int>() { campaignDefinitionDTO.CampaignDefinitionId }, activeChildRecords, sqlTransaction);
            if (campaignCommunicationDefinitionDTOList.Count != 0 && campaignCommunicationDefinitionDTOList.Any())
            {
                campaignDefinitionDTO.CampaignCommunicationDefinitionDTOList = campaignCommunicationDefinitionDTOList;
            }
            log.LogMethodExit();

            CampaignCustomerProfileMapListBL campaignCustomerProfileMapListBL = new CampaignCustomerProfileMapListBL(executionContext);
            List<CampaignCustomerProfileMapDTO> campaignCustomerProfileMapDTOList = campaignCustomerProfileMapListBL.GetCampaignCustomerProfileMapDTOList(new List<int>() { campaignDefinitionDTO.CampaignDefinitionId }, activeChildRecords, sqlTransaction);
            if (campaignCustomerProfileMapDTOList.Count != 0 && campaignCustomerProfileMapDTOList.Any())
            {
                campaignDefinitionDTO.CampaignCustomerProfileMapDTOList = campaignCustomerProfileMapDTOList;
            }
            log.LogMethodExit();
        }
        

        /// <summary>
        /// Saves the campaignDefinition
        /// Checks if the User id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);

            // Will Save the Child ScheduleCalendarDTO First
            log.Debug("CampaignDefinitionDTO.ScheduleCalendarDTO Value :" + campaignDefinitionDTO.ScheduleCalendarDTO);
            if (campaignDefinitionDTO.ScheduleCalendarDTO != null)
            {
                log.Debug("ScheduleCalendarDTO.IsChanged Value :" + campaignDefinitionDTO.ScheduleCalendarDTO.IsChanged);
                log.Debug("updatedScheduleCalendarDTO Value :" + campaignDefinitionDTO.ScheduleCalendarDTO);
                if (campaignDefinitionDTO.ScheduleCalendarDTO.IsChanged)
                {
                    ScheduleCalendarBL ScheduleCalendarBL = new ScheduleCalendarBL(executionContext, campaignDefinitionDTO.ScheduleCalendarDTO);
                    ScheduleCalendarBL.Save(sqlTransaction);
                    if (campaignDefinitionDTO.ScheduleId != campaignDefinitionDTO.ScheduleCalendarDTO.ScheduleId)
                    {
                        campaignDefinitionDTO.ScheduleId = campaignDefinitionDTO.ScheduleCalendarDTO.ScheduleId;
                    }
                }
            }

            CampaignDefinitionDataHandler campaignDefinitionDataHandler = new CampaignDefinitionDataHandler(sqlTransaction);
            if (campaignDefinitionDTO.CampaignDefinitionId < 0)
            {
                campaignDefinitionDTO = campaignDefinitionDataHandler.Insert(campaignDefinitionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                campaignDefinitionDTO.AcceptChanges();
            }
            else
            {
                if (campaignDefinitionDTO.IsChanged)
                {
                    campaignDefinitionDTO = campaignDefinitionDataHandler.Update(campaignDefinitionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    campaignDefinitionDTO.AcceptChanges();
                }
                
            }
            // Will Save the Child CampaignDiscountDefinitionDTO
            log.Debug("CampaignDefinitionDTO.CampaignDiscountDefinitionDTO Value :" + campaignDefinitionDTO.CampaignDiscountDefinitionDTO);
            if (campaignDefinitionDTO.CampaignDiscountDefinitionDTO != null)
            {
                if (campaignDefinitionDTO.CampaignDiscountDefinitionDTO.CampaignDefinitionId != campaignDefinitionDTO.CampaignDefinitionId)
                {
                    campaignDefinitionDTO.CampaignDiscountDefinitionDTO.CampaignDefinitionId = campaignDefinitionDTO.CampaignDefinitionId;
                }
                log.Debug("CampaignDiscountDefinitionDTO.IsChanged Value :" + campaignDefinitionDTO.CampaignDiscountDefinitionDTO.IsChanged);
                log.Debug("updatedcampaignDiscountDefinitionDTO Value :" + campaignDefinitionDTO.CampaignDiscountDefinitionDTO);
                if (campaignDefinitionDTO.CampaignDiscountDefinitionDTO.IsChanged)
                {
                    CampaignDiscountDefinitionBL campaignDiscountDefinitionBL = new CampaignDiscountDefinitionBL(executionContext, campaignDefinitionDTO.CampaignDiscountDefinitionDTO);
                    campaignDiscountDefinitionBL.Save(sqlTransaction);
                }
            }

            // Will Save the Child CampaignCommunicationDefinitionDTO
            log.Debug("campaignDefinitionDTO.CampaignCommunicationDefinitionDTO Value :" + campaignDefinitionDTO.CampaignCommunicationDefinitionDTOList);
            if (campaignDefinitionDTO.CampaignCommunicationDefinitionDTOList != null && campaignDefinitionDTO.CampaignCommunicationDefinitionDTOList.Any())
            {
                List<CampaignCommunicationDefinitionDTO> updatedCampaignCommunicationDefinitionDTOList = new List<CampaignCommunicationDefinitionDTO>();
                foreach (CampaignCommunicationDefinitionDTO campaignCommunicationDefinitionDTO in campaignDefinitionDTO.CampaignCommunicationDefinitionDTOList)
                {
                    if (campaignCommunicationDefinitionDTO.CampaignDefinitionId != campaignDefinitionDTO.CampaignDefinitionId)
                    {
                        campaignCommunicationDefinitionDTO.CampaignDefinitionId = campaignDefinitionDTO.CampaignDefinitionId;
                    }
                    log.Debug("CampaignCommunicationDefinitionDTO.IsChanged Value :" + campaignCommunicationDefinitionDTO.IsChanged);
                    if (campaignCommunicationDefinitionDTO.IsChanged)
                    {
                        updatedCampaignCommunicationDefinitionDTOList.Add(campaignCommunicationDefinitionDTO);
                    }
                }
                log.Debug("updatedCampaignCommunicationDefinitionDTO Value :" + updatedCampaignCommunicationDefinitionDTOList);
                if (updatedCampaignCommunicationDefinitionDTOList.Any())
                {
                    CampaignCommunicationDefinitionListBL campaignCommunicationDefinitionListBL = new CampaignCommunicationDefinitionListBL(executionContext);
                    campaignCommunicationDefinitionListBL.Save(updatedCampaignCommunicationDefinitionDTOList, sqlTransaction);
                }
            }

            // Will Save the Child CampaignCustomerProfileMapDTO
            log.Debug("campaignDefinitionDTO.CampaignCustomerProfileMapDTO Value :" + campaignDefinitionDTO.CampaignCustomerProfileMapDTOList);
            if (campaignDefinitionDTO.CampaignCustomerProfileMapDTOList != null && campaignDefinitionDTO.CampaignCustomerProfileMapDTOList.Any())
            {
                List<CampaignCustomerProfileMapDTO> updatedCampaignCustomerProfileMapDTOList = new List<CampaignCustomerProfileMapDTO>();
                foreach (CampaignCustomerProfileMapDTO campaignCustomerProfileMapDTO in campaignDefinitionDTO.CampaignCustomerProfileMapDTOList)
                {
                    if (campaignCustomerProfileMapDTO.CampaignDefinitionId != campaignDefinitionDTO.CampaignDefinitionId)
                    {
                        campaignCustomerProfileMapDTO.CampaignDefinitionId = campaignDefinitionDTO.CampaignDefinitionId;
                    }
                    log.Debug("CampaignCustomerProfileMapDTO.IsChanged Value :" + campaignCustomerProfileMapDTO.IsChanged);
                    if (campaignCustomerProfileMapDTO.IsChanged)
                    {
                        updatedCampaignCustomerProfileMapDTOList.Add(campaignCustomerProfileMapDTO);
                    }
                }
                log.Debug("updatedCampaignCustomerProfileMapDTO Value :" + updatedCampaignCustomerProfileMapDTOList);
                if (updatedCampaignCustomerProfileMapDTOList.Any())
                {
                    CampaignCustomerProfileMapListBL campaignCustomerProfileMapListBL = new CampaignCustomerProfileMapListBL(executionContext);
                    campaignCustomerProfileMapListBL.Save(updatedCampaignCustomerProfileMapDTOList, sqlTransaction);
                }
            }
            log.LogMethodExit();
        }



        /// <summary>
        /// Gets the DTO
        /// </summary>
        public CampaignDefinitionDTO CampaignDefinitionDTO
        {
            get
            {
                CampaignDefinitionDTO result = new CampaignDefinitionDTO(campaignDefinitionDTO);
                return result;
            }
        }
    }

    /// <summary>
    /// Manages the list of CampaignDefinition
    /// </summary>
    /// 

    public class CampaignDefinitionListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public CampaignDefinitionListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        public CampaignDefinitionListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        
        /// <summary>
        /// Returns the CampaignDefinition list
        /// </summary>
        public List<CampaignDefinitionDTO> GetCampaignDefinitionDTOList(List<KeyValuePair<CampaignDefinitionDTO.SearchByCampaignDefinitionParameters, string>> searchParameters, bool loadChildRecords = false, bool loadActiveChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadChildRecords, loadActiveChildRecords);
            CampaignDefinitionDataHandler campaignDefinitionDataHandler = new CampaignDefinitionDataHandler(sqlTransaction);
            List<CampaignDefinitionDTO> campaignDefinitionDTOsList = campaignDefinitionDataHandler.GetCampaignDefinitionList(searchParameters, sqlTransaction);
            if (campaignDefinitionDTOsList != null && campaignDefinitionDTOsList.Any() && loadChildRecords)
            {
                Build(campaignDefinitionDTOsList, loadActiveChildRecords, sqlTransaction);

            }
            log.LogMethodExit(campaignDefinitionDTOsList);
            return campaignDefinitionDTOsList;
        }


        private void Build(List<CampaignDefinitionDTO> campaignDefinitionDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(campaignDefinitionDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, CampaignDefinitionDTO> campaignDefinitionDTOIdMap = new Dictionary<int, CampaignDefinitionDTO>();
            List<int> campaignDefinitionIdList = new List<int>();
            Dictionary<int, CampaignDefinitionDTO> scheduleIdCampaignDefinitionDTODictionary = new Dictionary<int, CampaignDefinitionDTO>();
            List<int> scheduleIdList = new List<int>();
            for (int i = 0; i < campaignDefinitionDTOList.Count; i++)
            {
                if (campaignDefinitionDTOIdMap.ContainsKey(campaignDefinitionDTOList[i].CampaignDefinitionId))
                {
                    continue;
                }
                campaignDefinitionDTOIdMap.Add(campaignDefinitionDTOList[i].CampaignDefinitionId, campaignDefinitionDTOList[i]);
                campaignDefinitionIdList.Add(campaignDefinitionDTOList[i].CampaignDefinitionId);
                if (campaignDefinitionDTOList[i].ScheduleId == -1 ||
                   scheduleIdCampaignDefinitionDTODictionary.ContainsKey(campaignDefinitionDTOList[i].ScheduleId))
                {
                    continue;
                }
                scheduleIdCampaignDefinitionDTODictionary.Add(campaignDefinitionDTOList[i].ScheduleId, campaignDefinitionDTOList[i]);
                scheduleIdList.Add(campaignDefinitionDTOList[i].ScheduleId);
            }

            ScheduleCalendarListBL ScheduleCalendarListBL = new ScheduleCalendarListBL(executionContext);
            List<ScheduleCalendarDTO> ScheduleCalendarDTOList = ScheduleCalendarListBL.GetScheduleCalendarDTOList(scheduleIdList, activeChildRecords, sqlTransaction);
            if (ScheduleCalendarDTOList != null && ScheduleCalendarDTOList.Any())
            {
                foreach (ScheduleCalendarDTO scheduleCalendarDTO in ScheduleCalendarDTOList)
                {
                    if (scheduleIdCampaignDefinitionDTODictionary.ContainsKey(scheduleCalendarDTO.ScheduleId) == false)
                    {
                        continue;
                    }
                    CampaignDefinitionDTO campaignDefinitionDTO = scheduleIdCampaignDefinitionDTODictionary[scheduleCalendarDTO.ScheduleId];

                    campaignDefinitionDTO.ScheduleCalendarDTO = scheduleCalendarDTO;
                }
            }

            CampaignDiscountDefinitionListBL campaignDiscountDefinitionListBL = new CampaignDiscountDefinitionListBL(executionContext);
            List<CampaignDiscountDefinitionDTO> campaignDiscountDefinitionDTOList = campaignDiscountDefinitionListBL.GetCampaignDiscountDefinitionDTOList(campaignDefinitionIdList, activeChildRecords, sqlTransaction);
            if (campaignDiscountDefinitionDTOList != null && campaignDiscountDefinitionDTOList.Any())
            {
                foreach (var campaignDiscountDefinitionDTO in campaignDiscountDefinitionDTOList)
                {
                    if (campaignDefinitionDTOIdMap.ContainsKey(campaignDiscountDefinitionDTO.CampaignDefinitionId) == false)
                    {
                        continue;
                    }
                    campaignDefinitionDTOIdMap[campaignDiscountDefinitionDTO.CampaignDefinitionId].CampaignDiscountDefinitionDTO = campaignDiscountDefinitionDTO;
                }
            }

            CampaignCommunicationDefinitionListBL campaignCommunicationDefinitionListBL = new CampaignCommunicationDefinitionListBL(executionContext);
            List<CampaignCommunicationDefinitionDTO> campaignCommunicationDefinitionDTOList = campaignCommunicationDefinitionListBL.GetCampaignCommunicationDefinitionDTOList(campaignDefinitionIdList, activeChildRecords, sqlTransaction);
            if (campaignCommunicationDefinitionDTOList != null && campaignCommunicationDefinitionDTOList.Any())
            {
                for (int i = 0; i < campaignCommunicationDefinitionDTOList.Count; i++)
                {
                    if (campaignDefinitionDTOIdMap.ContainsKey(campaignCommunicationDefinitionDTOList[i].CampaignDefinitionId) == false)
                    {
                        continue;
                    }
                    CampaignDefinitionDTO campaignDefinitionDTO = campaignDefinitionDTOIdMap[campaignCommunicationDefinitionDTOList[i].CampaignDefinitionId];
                    if (campaignDefinitionDTO.CampaignCommunicationDefinitionDTOList == null)
                    {
                        campaignDefinitionDTO.CampaignCommunicationDefinitionDTOList = new List<CampaignCommunicationDefinitionDTO>();
                    }
                    campaignDefinitionDTO.CampaignCommunicationDefinitionDTOList.Add(campaignCommunicationDefinitionDTOList[i]);
                }
            }

            CampaignCustomerProfileMapListBL campaignCustomerProfileMapListBL = new CampaignCustomerProfileMapListBL(executionContext);
            List<CampaignCustomerProfileMapDTO> campaignCustomerProfileMapDTOList = campaignCustomerProfileMapListBL.GetCampaignCustomerProfileMapDTOList(campaignDefinitionIdList, activeChildRecords, sqlTransaction);
            if (campaignCustomerProfileMapDTOList != null && campaignCustomerProfileMapDTOList.Any())
            {
                for (int i = 0; i < campaignCustomerProfileMapDTOList.Count; i++)
                {
                    if (campaignDefinitionDTOIdMap.ContainsKey(campaignCustomerProfileMapDTOList[i].CampaignDefinitionId) == false)
                    {
                        continue;
                    }
                    CampaignDefinitionDTO campaignDefinitionDTO = campaignDefinitionDTOIdMap[campaignCustomerProfileMapDTOList[i].CampaignDefinitionId];
                    if (campaignDefinitionDTO.CampaignCustomerProfileMapDTOList == null)
                    {
                        campaignDefinitionDTO.CampaignCustomerProfileMapDTOList = new List<CampaignCustomerProfileMapDTO>();
                    }
                    campaignDefinitionDTO.CampaignCustomerProfileMapDTOList.Add(campaignCustomerProfileMapDTOList[i]);
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// This method should be used to Save and Update the CampaignDefinition.
        /// </summary>
        public List<CampaignDefinitionDTO> Save(List<CampaignDefinitionDTO> campaignDefinitionDTOList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<CampaignDefinitionDTO> savedCampaignDefinitionDTOList = new List<CampaignDefinitionDTO>();
            if (campaignDefinitionDTOList == null || campaignDefinitionDTOList.Any() == false)
            {
                log.LogMethodExit(savedCampaignDefinitionDTOList);
                return savedCampaignDefinitionDTOList;
            }
            foreach (CampaignDefinitionDTO campaignDefinitionDTO in campaignDefinitionDTOList)
            {
                CampaignDefinitionBL campaignDefinitionBL = new CampaignDefinitionBL(executionContext, campaignDefinitionDTO, sqlTransaction);
                campaignDefinitionBL.Save(sqlTransaction);
                savedCampaignDefinitionDTOList.Add(campaignDefinitionBL.CampaignDefinitionDTO);
            }
            log.LogMethodExit(savedCampaignDefinitionDTOList);
            return savedCampaignDefinitionDTOList;
        }

    }
}

