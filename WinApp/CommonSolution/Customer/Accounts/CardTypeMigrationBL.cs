/********************************************************************************************
 * Project Name - Customer
 * Description  - BL of CardTypeMigration - handles the migration task for WMS 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.80        27-May-2020   Girish Kundar           Created 
 *2.120.1     09-Jun-2021   Deeksha                 Modified as part of AWS concurrent programs enhancement.   
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.JobUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer.Accounts
{
    public class CardTypeMigrationBL
    {
        private List<CardTypeDTO> cardTypeDTOList;
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private Utilities utilities = new Utilities();
        string displayMessage = string.Empty;
        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        public CardTypeMigrationBL(ExecutionContext executionContext, List<CardTypeDTO> cardTypeDTOList)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.cardTypeDTOList = cardTypeDTOList;
            log.LogMethodExit();
        }

        public string StartMigration()
        {
            log.LogMethodEntry();
            
            try
            {
                if (ValidateData())
                {
                    if (PendingMigration())
                    {
                        DateTime currentRunTime = DateTime.Now;
                        CardTypeMigrationDataHandler cardTypeMigrationDataHandler = new CardTypeMigrationDataHandler(null);
                        DateTime? lastRunTimeValue = cardTypeMigrationDataHandler.GetCardTypeMembershipMigrationEngineRunTime(executionContext.GetUserId(), executionContext.GetSiteId());
                        DateTime lastRunTime = (lastRunTimeValue == null) ? DateTime.Now.AddYears(-25) : Convert.ToDateTime(lastRunTimeValue);
                        if (CanRunCardTypeMembership(lastRunTime))
                        {
                            displayMessage = MessageContainerList.GetMessage(executionContext, "The card migration will begin shortly");
                        }
                        else
                        {
                            log.LogMethodExit("Not able to run the migration.Please check staus or event log");
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Not able to run the migration.Please check staus or event log"));

                        }
                    }
                    else
                    {
                        displayMessage = MessageContainerList.GetMessage(executionContext, "There are not records for card migration");
                    }
                }
               
            }
            catch (ValidationException valEx)
            {
                log.Error(valEx);
                throw (valEx);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message + " : " + displayMessage);
            }

            log.LogMethodExit();
            return displayMessage;
        }

        private bool CanRunCardTypeMembership(DateTime? lastRunTime)
        {
            log.LogMethodEntry();
            bool returnValue = true;
            ConcurrentPrograms concurrentProgramBL = null;
            ConcurrentProgramList concurrentProgramList = new ConcurrentProgramList(executionContext);
            List<KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>> programSearch = new List<KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>>();
            programSearch.Add(new KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>(ConcurrentProgramsDTO.SearchByProgramsParameters.EXECUTABLE_NAME, "CardTypeMembershipMigration.exe"));
            programSearch.Add(new KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>(ConcurrentProgramsDTO.SearchByProgramsParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<ConcurrentProgramsDTO> concurrentProgramsListDTO = concurrentProgramList.GetAllConcurrentPrograms(programSearch, true, true);
            if (concurrentProgramsListDTO == null)
            {
                log.Debug("The concurrent program is not added to Database.Please run the N6 script");
                // throw new Exception(MessageContainerList.GetMessage(executionContext, "The concurrent program is not added to Database.Please run the N6 script"));
                returnValue = false;
            }
            if (concurrentProgramsListDTO[0].ConcurrentProgramSchedulesDTOList == null ||
                concurrentProgramsListDTO[0].ConcurrentProgramSchedulesDTOList.Count == 0) // If there is no schedules create one 
            {
                int minute = 0;
                minute = (int)Math.Ceiling(DateTime.Now.Minute / 5.0) * 5;
                ConcurrentProgramSchedulesDTO concurrentProgramSchedulesDTO = new ConcurrentProgramSchedulesDTO(-1,
                              concurrentProgramsListDTO[0].ProgramId, DateTime.Now.Date,
                              DateTime.Now.Hour.ToString() + ":" + minute,
                              -1, DateTime.Now.Date.AddDays(1), true, executionContext.GetSiteId(), "",
                              false, DateTime.Now, executionContext.GetUserId(), "", -1,
                              executionContext.GetUserId(), DateTime.Now);
                concurrentProgramSchedulesDTO.IsChanged = true;
                concurrentProgramsListDTO[0].ConcurrentProgramSchedulesDTOList.Add(concurrentProgramSchedulesDTO);
                ConcurrentPrograms concurrentPrograms = new ConcurrentPrograms(executionContext, concurrentProgramsListDTO[0],null,null,null,null);
                concurrentPrograms.Save();
                return true;
            }
            List<Tuple<DateTime, string, string>> runList = new List<Tuple<DateTime, string, string>>();
            if (concurrentProgramsListDTO != null && concurrentProgramsListDTO.Count > 0)
            {
                foreach (ConcurrentProgramsDTO concurrentProgramsDTO in concurrentProgramsListDTO)
                {
                    ConcurrentRequestList concurrentRequestList = new ConcurrentRequestList();
                    List<KeyValuePair<ConcurrentRequestsDTO.SearchByRequestParameters, string>> searchParameters = new List<KeyValuePair<ConcurrentRequestsDTO.SearchByRequestParameters, string>>();
                    searchParameters.Add(new KeyValuePair<ConcurrentRequestsDTO.SearchByRequestParameters, string>(ConcurrentRequestsDTO.SearchByRequestParameters.PROGRAM_ID, concurrentProgramsDTO.ProgramId.ToString()));
                    List<ConcurrentRequestsDTO> concurrentRequestsListDTO = concurrentRequestList.GetAllConcurrentRequests(searchParameters);
                    if (concurrentRequestsListDTO != null && concurrentRequestsListDTO.Count > 0)
                    {
                        concurrentRequestsListDTO = concurrentRequestsListDTO.OrderByDescending(t => t.ActualStartTime).ToList();
                        if (!String.IsNullOrEmpty(concurrentRequestsListDTO[0].ActualStartTime) && Convert.ToDateTime(concurrentRequestsListDTO[0].ActualStartTime) > lastRunTime)
                        {
                            runList.Add(new Tuple<DateTime, string, string>(Convert.ToDateTime(concurrentRequestsListDTO[0].ActualStartTime), concurrentRequestsListDTO[0].Status, concurrentRequestsListDTO[0].Phase));
                        }
                    }
                }
            }
            if (runList != null && runList.Count > 0)
            {
                runList = runList.OrderByDescending(t => t.Item1).ToList();
                if (runList[0].Item1 > lastRunTime && (runList[0].Item2 == "Error" || runList[0].Item3 == "Running"))
                    returnValue = false;
            }
            if (concurrentProgramsListDTO[0].ConcurrentProgramSchedulesDTOList != null &&
                   concurrentProgramsListDTO[0].ConcurrentProgramSchedulesDTOList.Any())
            {
                int minute = 0;
                minute = (int)Math.Ceiling(DateTime.Now.Minute / 5.0) * 5;
                concurrentProgramsListDTO[0].ConcurrentProgramSchedulesDTOList.ForEach(x => x.IsActive = false); // Make any existing schedules to false
                ConcurrentProgramSchedulesDTO newSchedulesDTO = new ConcurrentProgramSchedulesDTO(-1,
                             concurrentProgramsListDTO[0].ProgramId, DateTime.Now.Date,
                             DateTime.Now.Hour.ToString() + ":"+ minute,
                             -1, DateTime.Now.Date.AddYears(1), true, "");
                newSchedulesDTO.IsChanged = true;
                concurrentProgramsListDTO[0].ConcurrentProgramSchedulesDTOList.Add(newSchedulesDTO);
                concurrentProgramBL = new ConcurrentPrograms(executionContext, concurrentProgramsListDTO[0],null,null,null,null);
                concurrentProgramBL.Save();
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        public bool ValidateData()
        {
            log.LogMethodEntry();
            bool retValue = false;
            DataTable dtMaxDuration = utilities.executeDataTable(@"SELECT Bdate, Max(Qdate) MQdate 
                                           from (
                                                  select  CASE unitofQualificationWindow 
                                                             WHEN 'D' THEN DATEADD(day,QualificationWindow, Cast(getdate() as date))  
                                                             WHEN 'M' THEN DATEADD(Month,QualificationWindow, Cast(getdate() as date))  
									                         WHEN 'Y' THEN DATEADD(Year,QualificationWindow, Cast(getdate() as date))  
                                                              END QDate, Cast(getdate() as date) bDate
                                                    from MembershipRule mr, Membership m
                                                   where m.MembershipRuleID = mr.MembershipRuleID
                                                     and m.IsActive =1
                                                     and mr.IsActive =1
                                                     and (m.site_id = @siteId or @siteId = -1)
                                                ) a
                                         group by bdate", new SqlParameter("@siteId", executionContext.GetSiteId()));
            DateTime maxDurationBaseDate = DateTime.MinValue;
            DateTime maxDurationQDate = DateTime.MinValue;
            if (dtMaxDuration.Rows.Count > 0)
            {
                maxDurationBaseDate = Convert.ToDateTime(dtMaxDuration.Rows[0]["Bdate"]);
                if (dtMaxDuration.Rows[0]["MQdate"].ToString() != "")
                    maxDurationQDate = Convert.ToDateTime(dtMaxDuration.Rows[0]["MQdate"]);
            }
            if (maxDurationQDate == DateTime.MinValue)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1558));
            }

            List<KeyValuePair<CardTypeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CardTypeDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<CardTypeDTO.SearchByParameters, string>(CardTypeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            CardTypeListBL cardTypeListBL = new CardTypeListBL(executionContext);
            List<CardTypeDTO> existingCardTypeDTOList = cardTypeListBL.GetCardTypeDTOList(searchParameters);
            if (cardTypeDTOList.Exists(x => x.CardType == "NONCARDTYPEMIGRATIONENTRY"))
            {
                cardTypeDTOList.RemoveAll(card => card.CardType == "NONCARDTYPEMIGRATIONENTRY");
            }
            foreach (CardTypeDTO cardTypeDTO in cardTypeDTOList)
            {

                if (cardTypeDTO.CardTypeMigrated == false)
                {
                    if (cardTypeDTO.MembershipId == -1)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1565));
                    }
                    else
                    {
                        var tempDTOList = existingCardTypeDTOList.Where(x => x.MembershipId == cardTypeDTO.MembershipId && x.CardTypeId != cardTypeDTO.CardTypeId).ToList();
                        if (tempDTOList != null && tempDTOList.Any())
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1566));
                        }
                    }
                    if (cardTypeDTO.ExistingTriggerSource == 0)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1559));
                    }
                    int tempQualifyingDuration;
                    if (string.IsNullOrEmpty(cardTypeDTO.QualifyingDuration.ToString()) ||
                        !Int32.TryParse(cardTypeDTO.QualifyingDuration.ToString(), out tempQualifyingDuration))
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1561));
                    }
                    else
                    {
                        if (tempQualifyingDuration <= 0)
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1561));
                        }
                        DateTime tempDate = maxDurationBaseDate;
                        tempDate = tempDate.AddDays(Convert.ToUInt32(cardTypeDTO.QualifyingDuration));
                        if (tempDate < maxDurationQDate)
                        {
                            System.TimeSpan diffResult = maxDurationQDate.ToUniversalTime().Subtract(maxDurationBaseDate.ToUniversalTime());
                            if (cardTypeDTO.QualifyingDurationProceed == false)
                            {
                                displayMessage = MessageContainerList.GetMessage(executionContext, 1562, diffResult.TotalDays);
                                return false; 
                            }  
                        }
                    }
                    decimal loyaltyPointConversionRatio;
                    if (string.IsNullOrEmpty(cardTypeDTO.LoyaltyPointConvRatio.ToString()) || !Decimal.TryParse(cardTypeDTO.LoyaltyPointConvRatio.ToString(), out loyaltyPointConversionRatio))
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1560));
                    }

                    else
                    {
                        if (loyaltyPointConversionRatio <= 0)
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1560));
                        }
                        var tempDTOList = existingCardTypeDTOList.Where(x => x.LoyaltyPointConvRatio == cardTypeDTO.LoyaltyPointConvRatio && x.CardTypeId != cardTypeDTO.CardTypeId).ToList();
                        if (tempDTOList.Count == 0 && existingCardTypeDTOList.Count > 1)
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1567));
                        }
                    }

                    int tempMigrationOrder;
                    if (cardTypeDTO.MigrationOrder == null || !Int32.TryParse(cardTypeDTO.MigrationOrder.ToString(), out tempMigrationOrder))
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1569));
                    }
                    else
                    {
                        if (tempMigrationOrder < 0)
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1569));
                        }
                        var tempDTOList = existingCardTypeDTOList.Where(x => x.MigrationOrder == cardTypeDTO.MigrationOrder && x.CardTypeId != cardTypeDTO.CardTypeId).ToList();
                        if (tempDTOList != null && tempDTOList.Any())
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1570));
                        }
                    }
                }
            }

            retValue = true;
            log.LogMethodExit(retValue);
            return retValue;
        }

        private bool PendingMigration()
        {
            log.LogMethodEntry();
            bool retValue = true;

            var tempDTOList = cardTypeDTOList.Where(x => x.CardTypeMigrated == false).ToList();
            if (tempDTOList != null && tempDTOList.Count() == 0)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1590));
            }
            log.LogMethodExit(retValue);
            return retValue;
        }
    }
}
