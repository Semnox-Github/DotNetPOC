//using Semnox.Core.Messages;


using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Customer.Membership
{
    /// <summary>
    /// MembershipMasterList class
    /// </summary>
    public class MembershipMasterList
    {
        private static  readonly Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ConcurrentDictionary<int, MembershipMasterList> membershipMasterDictionary = new ConcurrentDictionary<int, MembershipMasterList>();
        private List<MembershipDTO> membershipDTOList;
        /// <summary>
        /// Returns the  membership list 
        /// </summary>
        public static List<MembershipDTO> GetMembershipDTOList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            MembershipMaster(executionContext);
            List<MembershipDTO> membershipDTOList = membershipMasterDictionary[executionContext.GetSiteId()].GetMembershipDTOList();
            log.LogMethodExit(membershipDTOList);
            return membershipDTOList;
        }

        /// <summary>
        /// Returns the  base membership list 
        /// </summary>
        public static List<MembershipDTO> GetBaseMembershipList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            MembershipMaster(executionContext);
            List<MembershipDTO> membershipDTOList = membershipMasterDictionary[executionContext.GetSiteId()].GetBaseMembershipList();
            log.LogMethodExit(membershipDTOList);
            return membershipDTOList;
        }
        /// <summary>
        /// Returns the  membership qualification range 
        /// </summary>
        public static List<DateTime?> GetMembershipQualificationRange(ExecutionContext executionContext, int membershipId, DateTime runForDate)
        {
            log.LogMethodEntry(executionContext, membershipId, runForDate);
            MembershipMaster(executionContext);
            List<DateTime?> dateTimeList = membershipMasterDictionary[executionContext.GetSiteId()].GetMembershipQualificationRangePvt(executionContext,membershipId, runForDate);
            log.LogMethodExit(dateTimeList);
            return dateTimeList;
        }
        /// <summary>
        /// Returns the  membership retention range 
        /// </summary>
        public static List<DateTime?> GetMembershipRetentionRange(ExecutionContext executionContext, int membershipId, DateTime runForDate)
        {
            log.LogMethodEntry(membershipId, runForDate);
            MembershipMaster(executionContext);
            List<DateTime?> rangeDates = membershipMasterDictionary[executionContext.GetSiteId()].GetMembershipRetentionRangePvt(executionContext, membershipId, runForDate);
            log.LogMethodExit(rangeDates);
            return rangeDates;
        }
		/// <summary>
        /// Returns the  membership retention range 
        /// </summary>
        public static List<DateTime?> GetCurrentRetentionRange(ExecutionContext executionContext, int membershipId, DateTime runForDate)
        {
            log.LogMethodEntry(membershipId, runForDate);
            MembershipMaster(executionContext);
            List<DateTime?> rangeDates = membershipMasterDictionary[executionContext.GetSiteId()].GetCurrentRetentionRangePvt(executionContext, membershipId, runForDate);
            log.LogMethodExit(rangeDates);
            return rangeDates;
        }

        /// <summary>
        /// Returns the  membership qualification message 
        /// </summary>
        public static string GetQualificationMessage(ExecutionContext executionContext, int membershipId, double customerLoyaltyPoints, DateTime runForDate)
        {
            log.LogMethodEntry(membershipId, customerLoyaltyPoints, runForDate);
            MembershipMaster(executionContext);
            string msg = membershipMasterDictionary[executionContext.GetSiteId()].GetQualificationMessagePrivate(executionContext, membershipId, customerLoyaltyPoints, runForDate);
            log.LogMethodExit(msg);
            return msg;
        }
        /// <summary>
        /// checks membership levels
        /// </summary>
        public static bool LowerThanCurrentMembershipLevel(ExecutionContext executionContext, int currentMembershipId, int newMembershipId)
        {
            log.LogMethodEntry(currentMembershipId, newMembershipId);
            MembershipMaster(executionContext);
            bool lowerThanCurrentMembershipLevel = membershipMasterDictionary[executionContext.GetSiteId()].LowerThanCurrentMembershipLevel(currentMembershipId, newMembershipId);
            log.LogMethodExit(lowerThanCurrentMembershipLevel);
            return lowerThanCurrentMembershipLevel;
        } 

        /// <summary>
        /// Returns next level membershipId
        /// </summary>
        public static int GetNextLevelMembership(ExecutionContext executionContext, int membershipId)
        {
            log.LogMethodEntry(membershipId);
            MembershipMaster(executionContext);
            int nextLevelMembership = membershipMasterDictionary[executionContext.GetSiteId()].GetNextLevelMembership(membershipId);
            log.LogMethodExit(nextLevelMembership);
            return nextLevelMembership;
        }

        /// <summary>
        /// Returns previous level membershipId
        /// </summary>
        public static int GetPreviousLevelMembership(ExecutionContext executionContext, int membershipId)
        {
            log.LogMethodEntry(membershipId);
            MembershipMaster(executionContext);
            int prevLevelMembership = membershipMasterDictionary[executionContext.GetSiteId()].GetPreviousLevelMembership(membershipId);
            log.LogMethodExit(prevLevelMembership);
            return prevLevelMembership;
        }

        /// <summary>
        /// checks membership eligiblity 
        /// </summary>
        public static bool IsEligibleForMembership(ExecutionContext executionContext, int membershipId, double loyaltyPoints)
        {
            log.LogMethodEntry(membershipId, loyaltyPoints);
            MembershipMaster(executionContext);
            bool isEligibleForMembership = membershipMasterDictionary[executionContext.GetSiteId()].IsEligibleForMembership(membershipId, loyaltyPoints);
            log.LogMethodExit(isEligibleForMembership);
            return isEligibleForMembership;
        }

        /// <summary>
        /// checks membership retention 
        /// </summary>
        public static bool IsEligibleForRetention(ExecutionContext executionContext, int membershipId, double loyaltyPoints)
        {
            log.LogMethodEntry(membershipId, loyaltyPoints);
            MembershipMaster(executionContext);
            bool isEligibleForRetention = membershipMasterDictionary[executionContext.GetSiteId()].IsEligibleForRetention(membershipId, loyaltyPoints);
            log.LogMethodExit(isEligibleForRetention);
            return isEligibleForRetention;
        }

        /// <summary>
        /// Returns the  membership rewards list 
        /// </summary>
        public static List<MembershipRewardsDTO> GetMembershipRewards(ExecutionContext executionContext, int membershipId)
        {
            log.LogMethodEntry(membershipId);
            MembershipMaster(executionContext);
            List<MembershipRewardsDTO> membershipRewardsDTOList = membershipMasterDictionary[executionContext.GetSiteId()].GetMembershipRewards(membershipId);
            log.LogMethodExit(membershipRewardsDTOList);
            return membershipRewardsDTOList;
        }
        /// <summary>
        /// Returns the  membership rewards DTO 
        /// </summary>
        public static MembershipRewardsDTO GetMembershipRewardDTO(ExecutionContext executionContext, int membershipId, int membershipRewardsId)
        {
            log.LogMethodEntry(membershipId, membershipRewardsId);
            MembershipMaster(executionContext);
            MembershipRewardsDTO membershipRewardsDTO = membershipMasterDictionary[executionContext.GetSiteId()].GetMembershipRewardDTO(membershipId, membershipRewardsId);
            log.LogMethodExit(membershipRewardsDTO);
            return membershipRewardsDTO;
        }
        /// <summary>
        /// Tells wheter Membership is VIP or not
        /// </summary>
        public static Boolean IsVIPMembership(ExecutionContext executionContext, int membershipId)
        {
            log.LogMethodEntry(membershipId);
            MembershipMaster(executionContext);
            bool isVIPMembership = membershipMasterDictionary[executionContext.GetSiteId()].IsVIPMembership(membershipId);
            log.LogMethodExit(isVIPMembership);
            return isVIPMembership;
        }

        private Boolean IsVIPMembership(int membershipId)
        {
            log.LogMethodEntry(membershipId);
            bool isVIPMembership = false;
            if (this.membershipDTOList != null)
            {
                List<MembershipDTO> membershipDTOListIn = this.membershipDTOList.Where(mem => mem.MembershipID == membershipId).ToList();
                if (membershipDTOListIn != null && membershipDTOListIn.Count > 0)
                {
                    isVIPMembership = membershipDTOListIn[0].VIP;  
                }
            }
            log.LogMethodExit(isVIPMembership, "Return value from IsVIPMembership");
            return isVIPMembership;
        }

        private List<MembershipRewardsDTO> GetMembershipRewards(int membershipId)
        {
            log.LogMethodEntry(membershipId);
            List<MembershipRewardsDTO> membershipRewardsDTOList = new List<MembershipRewardsDTO>();
            if (this.membershipDTOList != null)
            {
                List<MembershipDTO> membershipDTOListIn = this.membershipDTOList.Where(mem => mem.MembershipID == membershipId).ToList();
                if (membershipDTOListIn != null && membershipDTOListIn.Count > 0)
                {
                    if (membershipDTOListIn[0].MembershipRewardsDTOList != null && membershipDTOListIn[0].MembershipRewardsDTOList.Count > 0)
                    {
                        membershipRewardsDTOList = membershipDTOListIn[0].MembershipRewardsDTOList.ToList();
                    }
                }
            }
            log.LogMethodExit(membershipRewardsDTOList, "Return value from GetMembershipRewards");
            return membershipRewardsDTOList;
        }

        private MembershipRewardsDTO GetMembershipRewardDTO(int membershipId, int membershipRewardsId)
        {
            log.LogMethodEntry(membershipId, membershipRewardsId);
            List<MembershipRewardsDTO> membershipRewardsDTOList = new List<MembershipRewardsDTO>();
            MembershipRewardsDTO membershipRewardsDTO= new MembershipRewardsDTO();
            if (this.membershipDTOList != null)
            {
                List<MembershipDTO> membershipDTOListin = this.membershipDTOList.Where(mem => mem.MembershipID == membershipId).ToList();
                if (membershipDTOListin != null && membershipDTOListin.Count > 0)
                {
                    
                    if (membershipDTOListin[0].MembershipRewardsDTOList != null && membershipDTOListin[0].MembershipRewardsDTOList.Count > 0)
                    {
                        membershipRewardsDTOList = membershipDTOListin[0].MembershipRewardsDTOList.Where(mr => mr.MembershipRewardsId == membershipRewardsId).ToList();
                        if (membershipRewardsDTOList != null && membershipRewardsDTOList.Count > 0)
                        {
                            membershipRewardsDTO = membershipRewardsDTOList[0];
                        }
                    }
                }
            }
            log.LogMethodExit(membershipRewardsDTO, "Return value from GetMembershipRewardDTO");
            return membershipRewardsDTO;
        }
        private bool IsEligibleForMembership(int membershipId, double loyaltyPoints)
        {
            log.LogMethodEntry(membershipId, loyaltyPoints);
            bool eligibleForMembership = false;
            if (this.membershipDTOList != null)
            {
                List<MembershipDTO> membershipDTOListIn = this.membershipDTOList.Where(mem => mem.MembershipID == membershipId).ToList();
                log.LogVariableState("membershipDTOList", membershipDTOListIn);
                if (membershipDTOListIn != null && membershipDTOList.Count > 0 )
                {
                    if(membershipDTOListIn[0].MembershipRuleDTORecord != null )
                    {
                        log.LogVariableState("membershipDTOList[0].MembershipRuleDTORecord", membershipDTOListIn[0].MembershipRuleDTORecord);
                        if (loyaltyPoints >= membershipDTOListIn[0].MembershipRuleDTORecord.QualifyingPoints)
                            eligibleForMembership = true;
                    }
                }
            }
            log.LogMethodExit(eligibleForMembership, "Return value from IsEligibleForMembership");
            return eligibleForMembership;
        }

        private bool IsEligibleForRetention(int membershipId, double loyaltyPoints)
        {
            log.LogMethodEntry(membershipId, loyaltyPoints);
            bool eligibleForRetention = false;
            if (this.membershipDTOList != null)
            {
                List<MembershipDTO> membershipDTOListIn = this.membershipDTOList.Where(mem => mem.MembershipID == membershipId).ToList();
                log.LogVariableState("membershipDTOList", membershipDTOListIn);
                if (membershipDTOListIn != null && membershipDTOList.Count > 0)
                {
                    if (membershipDTOListIn[0].MembershipRuleDTORecord != null)
                    {
                        log.LogVariableState("membershipDTOList[0].MembershipRuleDTORecord", membershipDTOListIn[0].MembershipRuleDTORecord);
                        if (loyaltyPoints >= membershipDTOListIn[0].MembershipRuleDTORecord.RetentionPoints)
                            eligibleForRetention = true;
                    }
                }
            }
            log.LogMethodExit(eligibleForRetention, "Return value from IsEligibleForRetention");
            return eligibleForRetention;
        }

        private int GetPreviousLevelMembership(int membershipId)
        {
            log.LogMethodEntry(membershipId);
            int previousMembershipId = -1;
            if (this.membershipDTOList != null)
            {
                List<MembershipDTO> membershipDTOListin = this.membershipDTOList.Where(mem => mem.MembershipID == membershipId).ToList();
                log.LogVariableState("membershipDTOList", membershipDTOListin);
                if (membershipDTOListin != null && membershipDTOListin.Count > 0)
                {
                    previousMembershipId = membershipDTOListin[0].BaseMembershipID;
                }
            }
            log.LogMethodExit(previousMembershipId, "Return value from GetPreviousLevelMembership");
            return previousMembershipId;
        }
        private int GetNextLevelMembership(int membershipId)
        {
            log.LogMethodEntry(membershipId);
            int nextMembershipId = -1;
            if (this.membershipDTOList != null )
            {
                List<MembershipDTO> membershipDTOListIn = this.membershipDTOList.Where(mem => mem.BaseMembershipID == membershipId).ToList();
                log.LogVariableState("membershipDTOList", membershipDTOListIn);
                if (membershipDTOListIn != null && membershipDTOListIn.Count > 0)
                {
                    nextMembershipId = membershipDTOListIn[0].MembershipID;
                }
            }
            log.LogMethodExit(nextMembershipId, "Return value from GetNextLevelMembership");
            return nextMembershipId;
        }

        private static void MembershipMaster(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            if (membershipMasterDictionary.ContainsKey(executionContext.GetSiteId()) == false)
            {
                MembershipMasterList membershipMaster = new MembershipMasterList(executionContext);
                membershipMasterDictionary[executionContext.GetSiteId()] = membershipMaster;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns qualification date range for the membership
        /// </summary>
        private List<DateTime?> GetMembershipQualificationRangePvt(ExecutionContext executionContext, int membershipId, DateTime runForDate)
        {
            log.LogMethodEntry(membershipId, runForDate);
            List<DateTime?> dateRange = null;
            if (this.membershipDTOList != null)
            {
                List<MembershipDTO> membershipDTOListIn = this.membershipDTOList.Where(mem => mem.MembershipID == membershipId).ToList();
                if (membershipDTOListIn != null && membershipDTOListIn.Count > 0)
                {
                    if (membershipDTOListIn[0].MembershipRuleDTORecord != null)
                    {
                        dateRange = GetDateRange(executionContext, runForDate, membershipDTOListIn[0].MembershipRuleDTORecord.QualificationWindow, membershipDTOListIn[0].MembershipRuleDTORecord.UnitOfQualificationWindow, false);
                    }
                }
            }
            if (dateRange == null)
            {
                throw new ValidationException("Unable to retrive qualification range for membership id:" + membershipId);
            }
            log.LogMethodExit(dateRange, "Return value from GetMembershipQualificationRange");
            return dateRange;
        }

        private List<DateTime?> GetMembershipRetentionRangePvt(ExecutionContext executionContext, int membershipId, DateTime runForDate)
        {
            log.LogMethodEntry(executionContext, membershipId, runForDate);
            List<DateTime?> dateRange = null;
            if (this.membershipDTOList != null)
            {
                List<MembershipDTO> membershipDTOListIn = this.membershipDTOList.Where(mem => mem.MembershipID == membershipId).ToList();
                if (membershipDTOListIn != null && membershipDTOListIn.Count > 0)
                {
                    if (membershipDTOListIn[0].MembershipRuleDTORecord != null)
                    {
                        dateRange = GetDateRange(executionContext, runForDate, membershipDTOListIn[0].MembershipRuleDTORecord.RetentionWindow, membershipDTOListIn[0].MembershipRuleDTORecord.UnitOfRetentionWindow, true);
                    }
                }
            }
            log.LogMethodExit(dateRange);
            return dateRange;
        }
		
		 private List<DateTime?> GetCurrentRetentionRangePvt(ExecutionContext executionContext, int membershipId, DateTime runForDate)
        {
            log.LogMethodEntry(executionContext, membershipId, runForDate);
            List<DateTime?> dateRange = null;
            if (this.membershipDTOList != null)
            {
                List<MembershipDTO> membershipDTOListIn = this.membershipDTOList.Where(mem => mem.MembershipID == membershipId).ToList();
                if (membershipDTOListIn != null && membershipDTOListIn.Count > 0)
                {
                    if (membershipDTOListIn[0].MembershipRuleDTORecord != null)
                    {
                        dateRange = GetDateRange(executionContext, runForDate, membershipDTOListIn[0].MembershipRuleDTORecord.RetentionWindow, membershipDTOListIn[0].MembershipRuleDTORecord.UnitOfRetentionWindow, false);
                    }
                }
            }
            if (dateRange == null)
            {
                throw new ValidationException("Unable to retrive retention range for membership id:" + membershipId);
            }
            log.LogMethodExit(dateRange, "Return value from GetMembershipQualificationRange");
            return dateRange;
        }

        private string GetQualificationMessagePrivate(ExecutionContext executionContext, int membershipId, double customerLoyaltyPoints, DateTime runForDate)
        {
            log.LogMethodEntry(executionContext, membershipId, customerLoyaltyPoints, runForDate);
            List<DateTime?> dateRange = null;
            string message = "";
            if (this.membershipDTOList != null)
            {
                List<MembershipDTO> membershipDTOListIn = this.membershipDTOList.Where(mem => mem.MembershipID == membershipId).ToList();
                if (membershipDTOListIn != null && membershipDTOListIn.Count > 0)
                {
                    if (membershipDTOListIn[0].MembershipRuleDTORecord != null)
                    {
                        dateRange = GetDateRange(executionContext, runForDate, membershipDTOListIn[0].MembershipRuleDTORecord.QualificationWindow, membershipDTOListIn[0].MembershipRuleDTORecord.UnitOfQualificationWindow, false);
                        if(membershipDTOListIn[0].MembershipRuleDTORecord.QualifyingPoints > customerLoyaltyPoints)
                        {
                            double diff = membershipDTOListIn[0].MembershipRuleDTORecord.QualifyingPoints - customerLoyaltyPoints;
                            // message = MessageContainerList.GetMessage(executionContext, 520, diff.ToString(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT")), membershipDTOListIn[0].MembershipName);
                            message = "Earn "+ diff.ToString(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT")) + " points more to become "+ membershipDTOListIn[0].MembershipName +" Member ";
                        }
                    }
                }
            }
            log.LogMethodExit(dateRange, "Return value from GetMembershipQualificationRange");
            return message;
        }


        /// <summary>
        /// Returns retention date range for the membership
        /// </summary>
        private bool LowerThanCurrentMembershipLevel(int currentMembershipId, int newMembershipId)
        {
            log.LogMethodEntry(currentMembershipId, newMembershipId);
            bool checkResult = true;
            if (this.membershipDTOList != null)
            {
                if (currentMembershipId == newMembershipId)
                    checkResult = false;
                else
                {
                    //check whether hirerachy is defined
                    if (this.membershipDTOList.Exists(mp => mp.BaseMembershipID != -1))
                    {
                        //hirerachy is defined
                        int nextLevelMembershipId = currentMembershipId; 
                        do
                        {
                            nextLevelMembershipId = GetNextLevelMembership(nextLevelMembershipId);
                            if (nextLevelMembershipId == newMembershipId)
                            { checkResult = false; break; }

                        } while (nextLevelMembershipId != -1);
                    }
                    else
                    { //no hirerachy hence no low or high level
                        checkResult = false;
                    }
                }
            }
            log.LogMethodExit(checkResult, "Return value from LowerThanCurrentMembershipLevel");
            return checkResult;
        }


        private List<MembershipDTO> GetBaseMembershipList()
        {
            log.LogMethodEntry();
            List<MembershipDTO> baseList;

            baseList = this.membershipDTOList.Where(mem => mem.BaseMembershipID == -1).ToList();
            log.LogMethodExit(baseList, "Return value from GetBaseMembershipList");
            return baseList;
        }
        /// <summary>
        /// Returns the membership list
        /// </summary>
        protected List<MembershipDTO> GetMembershipDTOList()
        {
            return membershipDTOList;
        }
        /// <summary>
        ///  constructor of MembershipMaster class
        /// </summary>
        protected MembershipMasterList(ExecutionContext executionContext)
        {
            MembershipDataHandler membershipDataHandler = new MembershipDataHandler(null);
            List<KeyValuePair<MembershipDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<MembershipDTO.SearchByParameters, string>>();
            searchParam.Add(new KeyValuePair<MembershipDTO.SearchByParameters, string>(MembershipDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParam.Add(new KeyValuePair<MembershipDTO.SearchByParameters, string>(MembershipDTO.SearchByParameters.IS_ACTIVE, "1"));
            membershipDTOList = membershipDataHandler.GetAllMembershipList(searchParam, true, executionContext.GetSiteId());
            if(membershipDTOList == null)
            {
                membershipDTOList = new List<MembershipDTO>();
            }
        }

       
        private List<DateTime?> GetDateRange(ExecutionContext executionContext, DateTime runForDate, int windowValue, string unitOfWindow, bool retentionRange)
        {
            log.LogMethodEntry(executionContext, runForDate, windowValue, unitOfWindow, retentionRange);
            List<DateTime?> dateTimeList = new List<DateTime?>();
            if (windowValue == 0)
            {
                dateTimeList.Add(null);
                dateTimeList.Add(null);
            }
            else
            {
                //int businessStartHour = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "BUSINESS_DAY_START_TIME", 6);
                DateTime baseDate = runForDate;// runForDate.Date.AddHours(businessStartHour);// DateTime.Now;
                log.LogVariableState("baseDate", baseDate);
                DateTime dateValue = ((unitOfWindow == "D") ? baseDate.AddDays(windowValue) : ((unitOfWindow == "M") ? baseDate.AddMonths(windowValue) : ((unitOfWindow == "Y") ? baseDate.AddYears(windowValue) : DateTime.MinValue)));
                log.LogVariableState("dateValue", dateValue);
                if (dateValue == DateTime.MinValue)
                    throw new Exception("Unit of date Period is not set");
                else
                {

                    System.TimeSpan diff = dateValue.Subtract(baseDate);
                    if (retentionRange)
                    {
                        dateTimeList.Add(baseDate);
                        dateTimeList.Add(baseDate+ diff);
                    }
                    else
                    { 
                        DateTime fromDate = baseDate - diff;
                        dateTimeList.Add(fromDate);
                        dateTimeList.Add(baseDate);
                    }
                }
            }
            log.LogVariableState("dateTimeList", dateTimeList);
            log.LogMethodExit();
            return dateTimeList;
        }

        /// <summary>
        /// Returns the  membership rewards list 
        /// </summary>
        public static MembershipDTO GetMembershipDTO(ExecutionContext executionContext, int membershipId)
        {
            log.LogMethodEntry(membershipId);
            MembershipMaster(executionContext);
            MembershipDTO membershipDTO = membershipMasterDictionary[executionContext.GetSiteId()].GetMembershipDTO(membershipId);
            log.LogMethodExit("membershipDTO");
            return membershipDTO;
        }

        private MembershipDTO GetMembershipDTO(int membershipId)
        {
            log.LogMethodEntry(membershipId);
            MembershipDTO membershipDTO = null;
            if (this.membershipDTOList != null)
            {
                List<MembershipDTO> membershipDTOListIn = this.membershipDTOList.Where(mem => mem.MembershipID == membershipId).ToList();
                if (membershipDTOListIn != null && membershipDTOListIn.Any())
                {
                    membershipDTO = membershipDTOListIn[0]; 
                }
            }
            log.LogMethodExit(membershipDTO, "Return value from GetMembershipDTO");
            return membershipDTO;
        }
    }
}
