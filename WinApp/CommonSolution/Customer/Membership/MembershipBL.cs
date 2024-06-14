/* Project Name - MembershipBL
* Description  - Business call object of the Membership
* 
**************
**Version Log
**************
*Version     Date           Modified By             Remarks          
********************************************************************************************* 
*2.60        20-Feb-2019    Nagesh Badiger          Added new SaveUpdateMembership() and created constructor.
*            20-May-2019    Jagan Mohana Rao        Added new Validate() for validation and added sql transaction to SaveUpdateMembership()
*2.60.2      22-May-2019    Jagan Mohana            Added Transaction to SaveUpdateMembership()
*2.70        02-Aug-2019    Jagan Mohana            Removed the GetAllMembershipList() method.
*2.70.2        19-Jul-2019     Girish Kundar       Modified : Save() method. Now Insert/Update method returns the DTO instead of Id. 
*2.70.2        21-Aug-2019    Laster Menezes          Added methods GetRetentionPoints(), GetRetentionRange(), GetQualificationRange(), GetQualifyingPoints(), 
*                                                   GetDateRange(), GetNextLevelMembershipName(), IsStandAloneMembership.
 *2.90        21-May-2020      Girish Kundar       Modified : Made default constructor as Private   
 *2.130.3     16-Dec-2021     Abhishek              WMS fix : Added two parameters loadChildRecords,loadActiveChildRecords
 *2.140.1     20-Dec-2021     Abhishek              WMS fix : Added two parameters loadChildRecords,loadActiveChildRecords
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.DBSynch;

namespace Semnox.Parafait.Customer.Membership
{
    /// <summary>
    /// MembershipBL class
    /// </summary>
    public class MembershipBL
    {
        private MembershipDTO membershipDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of Membership class
        /// </summary>
        private  MembershipBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext); 
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the MembershipDTO parameter
        /// </summary>
        /// <param name="membershipDTO">Parameter of the type MembershipDTO</param>       
        /// <param name="executionContext">ExecutionContext</param>       
        public MembershipBL(ExecutionContext executionContext, MembershipDTO membershipDTO)
            :this(executionContext)
        {
            log.LogMethodEntry(membershipDTO , executionContext);
            this.membershipDTO = membershipDTO;
            log.LogMethodExit();
        }

        /// <summary>
        ///Constructor will fetch the Membership DTO based on the membership id passed 
        /// </summary>
        /// <param name="membershipId">Membership id</param>
        /// <param name="sqlTransaction">sqlTransaction </param>
        public MembershipBL(ExecutionContext executionContext, int membershipId , SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, membershipId, sqlTransaction);
            MembershipDataHandler membershipDataHandler = new MembershipDataHandler(sqlTransaction);
            membershipDTO = membershipDataHandler.GetMembership(membershipId, executionContext.GetSiteId());
            if (membershipDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "Membership", membershipId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the membership record
        /// Checks if the MembershipID is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            MembershipDataHandler membershipDataHandler = new MembershipDataHandler(sqlTransaction);
            List<ValidationError> validationErrorList = Validate();
            if (validationErrorList.Count > 0)
            {
                throw new ValidationException("Validation Failed", validationErrorList);
            }
            if (membershipDTO.MembershipID < 0)
            {
                membershipDTO = membershipDataHandler.InsertMembership(membershipDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                membershipDTO.AcceptChanges();
                CreateRoamingData(executionContext, sqlTransaction);
            }
            else
            {
                if (membershipDTO.IsChanged)
                {
                    membershipDTO = membershipDataHandler.UpdateMembership(membershipDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    membershipDTO.AcceptChanges();
                    CreateRoamingData(executionContext, sqlTransaction);
                }
            }
            if (membershipDTO.MembershipRewardsDTOList != null && membershipDTO.MembershipRewardsDTOList.Count > 0)
            {
                foreach (MembershipRewardsDTO membershipRewardsDTO in membershipDTO.MembershipRewardsDTOList)
                {
                    if (membershipRewardsDTO.MembershipID == -1)
                        membershipRewardsDTO.MembershipID = membershipDTO.MembershipID;

                    MembershipRewardsBL membershipRewardsBL = new MembershipRewardsBL(executionContext, membershipRewardsDTO);
                    membershipRewardsBL.Save(sqlTransaction);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates the Roaming Data for Membership
        /// </summary>
        /// <param name="machineUserContext">machineUserContext</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        private void CreateRoamingData(ExecutionContext machineUserContext, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(machineUserContext, sqlTransaction);
            MembershipDataHandler membershipDataHandler = new MembershipDataHandler(sqlTransaction);
            MembershipDTO updatedMembershipDTO = membershipDataHandler.GetMembership(this.membershipDTO.MembershipID, machineUserContext.GetSiteId());
            DBSynchLogService dBSynchLogService = new DBSynchLogService(machineUserContext, "membership", updatedMembershipDTO.Guid, updatedMembershipDTO.SiteId);
            dBSynchLogService.CreateRoamingDataOnAllSites(sqlTransaction);
            log.LogMethodExit();
        }
        /// <summary>
        /// Validate the membership details
        /// </summary>
        /// <returns></returns>
        private List<ValidationError> Validate()
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            if (string.IsNullOrEmpty(membershipDTO.MembershipName))
            {
                ValidationError validationError = new ValidationError(MessageContainerList.GetMessage(executionContext, "Membership"), MessageContainerList.GetMessage(executionContext, "Membership Name"), MessageContainerList.GetMessage(executionContext, 1855)); /// Please enter valid value for Membership Name
                validationErrorList.Add(validationError);
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public MembershipDTO getMembershipDTO { get { return membershipDTO; } }

        public double GetRetentionPoints()
        {
            log.LogMethodEntry();
            double retentionPoints = 0;
            MembershipRuleBL membershipRuleBL = new MembershipRuleBL(executionContext, this.membershipDTO.MembershipRuleID);
            if (membershipRuleBL.getMembershipRuleDTO != null)
            {
                retentionPoints = membershipRuleBL.getMembershipRuleDTO.RetentionPoints;
            }
            log.LogMethodExit();
            return retentionPoints;
        }

        public List<DateTime?> GetRetentionRange(DateTime? EffectiveTodate)
        {
            log.LogMethodEntry(EffectiveTodate);
            List<DateTime?> retentionRange = null;
            MembershipRuleBL membershipRuleBL = new MembershipRuleBL(executionContext,this.membershipDTO.MembershipRuleID);
            retentionRange = GetRetentionRange(EffectiveTodate, membershipRuleBL.getMembershipRuleDTO.RetentionWindow, membershipRuleBL.getMembershipRuleDTO.UnitOfRetentionWindow);
            log.LogMethodExit(retentionRange);
            return retentionRange;
        }

        public List<DateTime?> GetQualificationRange()
        {
            log.LogMethodEntry();
            List<DateTime?> qualificationRange = null;
            if (this.membershipDTO != null)
            {
                DateTime runForDate = ServerDateTime.Now.Date;
                MembershipRuleBL membershipRuleBL = new MembershipRuleBL(executionContext,this.membershipDTO.MembershipRuleID);
                qualificationRange = MembershipMasterList.GetMembershipQualificationRange(executionContext, membershipDTO.MembershipID, runForDate);
            }
            log.LogMethodExit(qualificationRange);
            return qualificationRange;
        }

        public double GetQualifyingPoints()
        {
            log.LogMethodEntry();
            double qualifyingPoints = 0;
            if (this.membershipDTO != null)
            {
                MembershipRuleBL membershipRuleBL = new MembershipRuleBL(executionContext,this.membershipDTO.MembershipRuleID);
                if (membershipRuleBL.getMembershipRuleDTO != null)
                {
                    qualifyingPoints = membershipRuleBL.getMembershipRuleDTO.QualifyingPoints;
                }
            }
            log.LogMethodExit(qualifyingPoints);
            return qualifyingPoints;
        }


        public List<DateTime?> GetRetentionRange(DateTime? effectiveToDate, int windowValue, string unitOfWindow)
        {
            log.LogMethodEntry();
            if (effectiveToDate == DateTime.MinValue || effectiveToDate == DateTime.MaxValue)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Please provide valid Effective To Date"));
            }
            List<DateTime?> dateTimeList = new List<DateTime?>();
            if (windowValue == 0)
            {
                dateTimeList.Add(null);
                dateTimeList.Add(null);
            }
            else
            {
                DateTime baseDate = DateTime.Now;

                DateTime dateValue = ((unitOfWindow == "D") ? baseDate.AddDays(windowValue) : ((unitOfWindow == "M") ? baseDate.AddMonths(windowValue) : ((unitOfWindow == "Y") ? baseDate.AddYears(windowValue) : DateTime.MinValue)));
                if (dateValue == DateTime.MinValue)
                    throw new Exception("Unit of date Period is not set");
                else
                {
                    System.TimeSpan diff = dateValue.Subtract(baseDate);
                    dateTimeList.Add(effectiveToDate - diff);
                    dateTimeList.Add(effectiveToDate);
                }
            }
            log.LogMethodExit(dateTimeList);
            return dateTimeList;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pointsEarnedInRetentionRange"></param>
        /// <returns></returns>
        public double GetRequiredPointsToRetainMembership(double pointsEarnedInRetentionRange)
        {
            log.LogMethodEntry(pointsEarnedInRetentionRange);
            double requiredPointsToRetainMembership = 0;
            double retentionPoints = 0;
            retentionPoints = GetRetentionPoints();
            requiredPointsToRetainMembership = retentionPoints - pointsEarnedInRetentionRange;
            log.LogMethodExit(requiredPointsToRetainMembership);
            return requiredPointsToRetainMembership;
        }


        /// <summary>
        /// Get Next Level MembershipName
        /// </summary>
        /// <returns>nextLevelMembershipName</returns>
        public string GetNextLevelMembershipName()
        {
            log.LogMethodEntry();
            string nextLevelMembershipName = string.Empty;
            int nextlevelMembershipId = MembershipMasterList.GetNextLevelMembership(executionContext, membershipDTO.MembershipID);
            if (nextlevelMembershipId != -1)
            {
                MembershipBL membershipBL = new MembershipBL(executionContext, nextlevelMembershipId);
                if (membershipBL.getMembershipDTO != null)
                {
                    nextLevelMembershipName = membershipBL.getMembershipDTO.MembershipName;
                }
            }
            log.LogMethodExit(nextLevelMembershipName);
            return nextLevelMembershipName;
        }


        public bool IsStandAloneMembership()
        {
            log.LogMethodEntry();
            bool isStandAloneMembership = false;
            int nextlevelmembershipId = -1;
            int previousLevelMembershipId = -1;
            nextlevelmembershipId = MembershipMasterList.GetNextLevelMembership(executionContext, membershipDTO.MembershipID);
            previousLevelMembershipId = MembershipMasterList.GetPreviousLevelMembership(executionContext, membershipDTO.MembershipID);
            if(nextlevelmembershipId == -1 && previousLevelMembershipId == -1)
            {
                isStandAloneMembership = true;
            }
            log.LogMethodExit(isStandAloneMembership);
            return isStandAloneMembership;
        }
    }

    /// <summary>
    /// Manages the list of memberships
    /// </summary>
    public class MembershipsList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<MembershipDTO> membershipDTOList;
        private readonly ExecutionContext executionContext;

        public MembershipsList( )
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>    
        /// <param name="executionContext"></param>
        public MembershipsList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.membershipDTOList = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="membershipRuleDTOList"></param>
        /// <param name="executionContext"></param>
        public MembershipsList(List<MembershipDTO> membershipDTOList, ExecutionContext executionContext)
        {
            log.LogMethodEntry(membershipDTOList, executionContext);
            this.executionContext = executionContext;
            this.membershipDTOList = membershipDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the membership list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="loadChildRecords">loadChildRecords</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>List of MembershipDTO</returns>
        public List<MembershipDTO> GetAllMembership(List<KeyValuePair<MembershipDTO.SearchByParameters, string>> searchParameters, int siteId, bool loadChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, siteId, loadChildRecords, sqlTransaction);
            MembershipDataHandler membershipDataHandler = new MembershipDataHandler(sqlTransaction);
            List<MembershipDTO> membershipDTOList = membershipDataHandler.GetAllMembershipList(searchParameters, loadChildRecords, siteId);
            log.LogMethodExit(membershipDTOList);
            return membershipDTOList;
        }

        /// <summary>
        /// Returns the membership list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="loadChildRecords">loadChildRecords</param>
        /// <param name="loadActiveChildRecords">loadActiveChildRecords</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>List of MembershipDTO</returns>
        public List<MembershipDTO> GetAllMemberships(List<KeyValuePair<MembershipDTO.SearchByParameters, string>> searchParameters, bool loadChildRecords = false, bool loadActiveChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadChildRecords, sqlTransaction);
            MembershipDataHandler membershipDataHandler = new MembershipDataHandler(sqlTransaction);
            List<MembershipDTO> membershipDTOList = membershipDataHandler.GetAllMembershipList(searchParameters, loadChildRecords, executionContext.GetSiteId(), loadActiveChildRecords);
            log.LogMethodExit(membershipDTOList);
            return membershipDTOList;
        }

        /// <summary>
        /// Save or Update Membership details
        /// </summary>
        public void SaveUpdateMembership()
        {
            log.LogMethodEntry();
            if (membershipDTOList != null && membershipDTOList.Any())
            {
                foreach (MembershipDTO membershipDTO in membershipDTOList)
                {
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            MembershipBL membershipBL = new MembershipBL(executionContext, membershipDTO);
                            membershipBL.Save(parafaitDBTrx.SQLTrx);
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
                            throw new Exception(ex.Message, ex);
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
        public DateTime? GetMembershipLastUpdateTime(int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(siteId);
            MembershipDataHandler membershipDataHandler = new MembershipDataHandler(sqlTransaction);
            DateTime? result = membershipDataHandler.GetMemershipModuleLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}