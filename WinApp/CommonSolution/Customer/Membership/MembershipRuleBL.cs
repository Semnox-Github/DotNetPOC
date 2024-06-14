/* Project Name - Semnox.Parafait.Cards
* Description  - Business call object of the MembershipRule
* 
**************
**Version Log
************** 
*Version     Date           Modified By         Remarks          
********************************************************************************************* 
*2.60        20-Feb-2019    Nagesh Badiger     Added new SaveUpdateMembershipRule and constructor in MembershipRulesList class
*2.60.2      22-May-2019    Jagan Mohana       Added new Valdiate() for validation
*2.70.2       19-Jul-2019      Girish Kundar    Modified : Save() method. Now Insert/Update method returns the DTO instead of Id. 
*2.80        21-May-2020      Girish Kundar       Modified : Made default constructor as Private  
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
    /// Membership BL which deals with save, update and loads the dto based on the id passed. 
    /// </summary>
    public class MembershipRuleBL
    {
        private MembershipRuleDTO membershipRuleDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of MembershipRule class
        ///</summary>
        private  MembershipRuleBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the MembershipRule DTO parameter
        /// </summary>
        /// <param name="membershipRuleDTO">Parameter of the type MembershipRuleDTO</param>       
        /// <param name="executionContext">ExecutionContext</param>       
        public MembershipRuleBL(ExecutionContext executionContext, MembershipRuleDTO membershipRuleDTO)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext,membershipRuleDTO);
            this.membershipRuleDTO = membershipRuleDTO;
            log.LogMethodExit();
        }

        /// <summary>
        ///Constructor will fetch the MembershipRule DTO based on the membershipRule id passed 
        /// </summary>
        /// <param name="membershipRuleId">MembershipRule id</param>
        public MembershipRuleBL(ExecutionContext executionContext,int membershipRuleId, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, membershipRuleId,sqlTransaction);
            MembershipRuleDataHandler membershipRuleDataHandler = new MembershipRuleDataHandler(sqlTransaction);
            membershipRuleDTO = membershipRuleDataHandler.GetMembershipRule(membershipRuleId);
            if (membershipRuleDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "MembershipRule", membershipRuleId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the membershipRule record
        /// Checks if the MembershipRuleID is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            MembershipRuleDataHandler membershipRuleDataHandler = new MembershipRuleDataHandler(sqlTransaction);
            List<ValidationError> validationErrorList = Validate();
            if (validationErrorList.Count > 0)
            {
                throw new ValidationException("Validation Failed", validationErrorList);
            }
            if (membershipRuleDTO.MembershipRuleID < 0)
            {
                membershipRuleDTO = membershipRuleDataHandler.InsertMembershipRule(membershipRuleDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                membershipRuleDTO.AcceptChanges();
                CreateRoamingData(executionContext);
            }
            else
            {
                if (membershipRuleDTO.IsChanged)
                {
                    membershipRuleDTO = membershipRuleDataHandler.UpdateMembershipRule(membershipRuleDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    membershipRuleDTO.AcceptChanges();
                    CreateRoamingData(executionContext);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates Roaming Data for Membership
        /// </summary>
        /// <param name="machineUserContext">machineUserContext</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        private void CreateRoamingData(ExecutionContext machineUserContext, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(machineUserContext, sqlTransaction);
            MembershipRuleDataHandler membershipRuleDataHandler = new MembershipRuleDataHandler(sqlTransaction);
            MembershipRuleDTO updatedMembershipRuleDTO = membershipRuleDataHandler.GetMembershipRule(this.membershipRuleDTO.MembershipRuleID);
            DBSynchLogService dBSynchLogService = new DBSynchLogService(machineUserContext, "membershiprule", updatedMembershipRuleDTO.Guid, updatedMembershipRuleDTO.SiteId);
            dBSynchLogService.CreateRoamingDataOnAllSites(sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the DTO
        /// </summary>
        /// <returns>List of ValidationError</returns>
        private List<ValidationError> Validate()
        {
            List<ValidationError> validationErrorList = new List<ValidationError>();
            if (string.IsNullOrEmpty(membershipRuleDTO.RuleName))
            {
                ValidationError validationError = new ValidationError(MessageContainerList.GetMessage(executionContext, "Membership Rule"), MessageContainerList.GetMessage(executionContext, "Rule Name"), MessageContainerList.GetMessage(executionContext, 1480)); /// Please enter membership rule name
                validationErrorList.Add(validationError);
            }
            if (membershipRuleDTO.QualificationWindow > 0 && string.IsNullOrEmpty(membershipRuleDTO.UnitOfQualificationWindow))
            {
                ValidationError validationError = new ValidationError(MessageContainerList.GetMessage(executionContext, "Membership Rule"), MessageContainerList.GetMessage(executionContext, "Unit Of Qualification Window"), MessageContainerList.GetMessage(executionContext, 1481)); /// Please enter Unit Of Qualification Window
                validationErrorList.Add(validationError);
            }
            if (membershipRuleDTO.QualificationWindow == 0 && string.IsNullOrEmpty(membershipRuleDTO.UnitOfQualificationWindow))
            {
                membershipRuleDTO.UnitOfQualificationWindow = "D";
            }
            if (membershipRuleDTO.RetentionWindow == 0)
            {
                ValidationError validationError = new ValidationError(MessageContainerList.GetMessage(executionContext, "Membership Rule"), MessageContainerList.GetMessage(executionContext, "Retention Window"), MessageContainerList.GetMessage(executionContext, 1482)); /// Please enter Retention Window
                validationErrorList.Add(validationError);
            }
            if (string.IsNullOrEmpty(membershipRuleDTO.UnitOfRetentionWindow))
            {
                ValidationError validationError = new ValidationError(MessageContainerList.GetMessage(executionContext, "Membership Rule"), MessageContainerList.GetMessage(executionContext, "Unit Of Retention Window"), MessageContainerList.GetMessage(executionContext, 1483)); /// Please enter Unit of Retention Window
                validationErrorList.Add(validationError);
            }
            return validationErrorList;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public MembershipRuleDTO getMembershipRuleDTO { get { return membershipRuleDTO; } }
    }

    /// <summary>
    /// Manages the list of membershipRules
    /// </summary>
    public class MembershipRulesList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<MembershipRuleDTO> membershipRuleDTOList;
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>    
        /// <param name="executionContext">executionContext</param>
        public MembershipRulesList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.membershipRuleDTOList = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="membershipRuleDTOList">membershipRuleDTOList</param>
        /// <param name="executionContext">executionContext</param>
        public MembershipRulesList(List<MembershipRuleDTO> membershipRuleDTOList, ExecutionContext executionContext)
        {
            log.LogMethodEntry(membershipRuleDTOList, executionContext);
            this.executionContext = executionContext;
            this.membershipRuleDTOList = membershipRuleDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the membershipRule list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns> List of MembershipRuleDTO</returns>
        public List<MembershipRuleDTO> GetAllMembershipRule(List<KeyValuePair<MembershipRuleDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            MembershipRuleDataHandler membershipRuleDataHandler = new MembershipRuleDataHandler(sqlTransaction);
            List<MembershipRuleDTO> membershipRuleDTOList = membershipRuleDataHandler.GetAllMembershipRuleList(searchParameters);
            log.LogMethodExit(membershipRuleDTOList);
            return membershipRuleDTOList;
        }


        /// <summary>
        /// Save or Update MembershipRule details
        /// </summary>
        public void SaveUpdateMembershipRule()
        {
            log.LogMethodEntry();
            try
            {
                if (membershipRuleDTOList != null && membershipRuleDTOList.Any())
                {
                    foreach (MembershipRuleDTO membershipRuleDTO in membershipRuleDTOList)
                    {
                        MembershipRuleBL membershipRuleBL = new MembershipRuleBL(executionContext, membershipRuleDTO);
                        membershipRuleBL.Save();
                    }
                }
            }
            catch (ValidationException valEx)
            {
                log.Error(valEx);
                throw valEx;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw new Exception(ex.Message, ex);
            }
            log.LogMethodExit();
        }

    }
}
