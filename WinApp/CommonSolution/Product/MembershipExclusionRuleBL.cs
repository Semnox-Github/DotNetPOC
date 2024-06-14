/********************************************************************************************
 * Project Name - MembershipExclusionRule Bussiness Layer
 * Description  - Bussiness Layer of the MembershipExclusionRule class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        24-Jan-2019   Ankitha C Kothwal     Created 
 *2.60.2      23-May-2019   Mushahid Faizan      Modified SaveUpdateMembershipExclusionRuleList()
 *2.110.00    26-Nov-2020   Abhishek            Modified:Modified to 3 Tier Standard
 ********************************************************************************************/
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Product
{
    public class MembershipExclusionRuleBL
    {
        private MembershipExclusionRuleDTO membershipExclusionRuleDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of MembershipExclusionRuleBL class
        /// </summary>
        private MembershipExclusionRuleBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the membershipExclusionRule id as the parameter
        /// Would fetch the membershipExclusionRule object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public MembershipExclusionRuleBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            MembershipExclusionRuleDataHandler membershipExclusionRuleDataHandler = new MembershipExclusionRuleDataHandler(sqlTransaction);
            membershipExclusionRuleDTO = membershipExclusionRuleDataHandler.GetMembershipExclusionRuleDTO(id);
            if (membershipExclusionRuleDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, " Membership Exclusion Rule", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates MembershipExclusionRuleBL object using the MembershipExclusionRuleDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="membershipExclusionRuleDTO">MembershipExclusionRuleDTO object</param>
        public MembershipExclusionRuleBL(ExecutionContext executionContext, MembershipExclusionRuleDTO membershipExclusionRuleDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, membershipExclusionRuleDTO);
            this.membershipExclusionRuleDTO = membershipExclusionRuleDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the MembershipExclusionRule
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (membershipExclusionRuleDTO.IsChanged == false
                && membershipExclusionRuleDTO.Id > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            MembershipExclusionRuleDataHandler membershipExclusionRuleDataHandler = new MembershipExclusionRuleDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate(sqlTransaction);
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (membershipExclusionRuleDTO.Id < 0)
            {
                membershipExclusionRuleDTO = membershipExclusionRuleDataHandler.Insert(membershipExclusionRuleDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                membershipExclusionRuleDTO.AcceptChanges();
            }
            else
            {
                if (membershipExclusionRuleDTO.IsChanged)
                {
                    membershipExclusionRuleDTO = membershipExclusionRuleDataHandler.Update(membershipExclusionRuleDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    membershipExclusionRuleDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the MembershipExclusionRuleDTO 
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
        public MembershipExclusionRuleDTO MembershipExclusionRuleDTO { get { return membershipExclusionRuleDTO; } }

        /// <summary>
        /// Delete the MembershipExclusionRule based on id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sQLTrx"></param>
        public void Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                MembershipExclusionRuleDataHandler membershipExclusionRuleDataHandler = new MembershipExclusionRuleDataHandler(sqlTransaction);
                membershipExclusionRuleDataHandler.Delete(membershipExclusionRuleDTO.Id);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of MembershipExclusionRuleList
    /// </summary>
    public class MembershipExclusionRuleListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<MembershipExclusionRuleDTO> membershipExclusionRuleDTOList;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public MembershipExclusionRuleListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor for MembershipExclusionRuleDTO Collection.
        /// </summary>
		/// <param name="executionContext"></param>
        /// <param name="membershipExclusionRuleDTOList"></param>
        public MembershipExclusionRuleListBL(ExecutionContext executionContext, List<MembershipExclusionRuleDTO> membershipExclusionRuleDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, membershipExclusionRuleDTOList);
            this.membershipExclusionRuleDTOList = membershipExclusionRuleDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the MembershipExclusionRule list
        /// </summary>
        public List<MembershipExclusionRuleDTO> GetMembershipExclusionRuleDTOList(List<KeyValuePair<MembershipExclusionRuleDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            MembershipExclusionRuleDataHandler membershipExclusionRuleDataHandler = new MembershipExclusionRuleDataHandler(sqlTransaction);
            List<MembershipExclusionRuleDTO> membershipExclusionRuleDTOList = membershipExclusionRuleDataHandler.GetMembershipExclusionRuleDTOList(searchParameters);
            log.LogMethodExit(membershipExclusionRuleDTOList);
            return membershipExclusionRuleDTOList;
        }

        /// <summary>
        /// This method should be used to Save and Update the Membership Exclusion List for Web Management Studio.
        /// </summary>
        public void Save()
        {
            if (membershipExclusionRuleDTOList != null)
            {
                foreach (MembershipExclusionRuleDTO membershipExclusionRuleDTO in membershipExclusionRuleDTOList)
                {
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            MembershipExclusionRuleBL membershipExclusionRuleBL = new MembershipExclusionRuleBL(executionContext, membershipExclusionRuleDTO);
                            membershipExclusionRuleBL.Save(parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            log.LogMethodExit(null, "Throwing Exception : " + valEx.Message);
                            throw;
                        }
                        catch (SqlException sqlEx)
                        {
                            log.Error(sqlEx);
                            parafaitDBTrx.RollBack();
                            log.LogMethodExit(null, "Throwing SQL Exception : " + sqlEx.Message);
                            if (sqlEx.Number == 547)
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869)); //Unable to delete this record.Please check the reference record first.
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
                        log.LogMethodExit();
                    }
                }
            }
        }

        /// <summary>
        /// Populates the Membership Exclusion using searchParameters
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<MembershipExclusionRuleDTO> PopulateMembershipExclusion(List<KeyValuePair<MembershipExclusionRuleDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<MembershipExclusionRuleDTO> populateMembershipExclusionRuleDTOList;
            MembershipExclusionRuleDataHandler membershipExclusionRuleDataHandler = new MembershipExclusionRuleDataHandler(sqlTransaction);
            membershipExclusionRuleDTOList = membershipExclusionRuleDataHandler.GetMembershipExclusionRuleDTOList(searchParameters);
            if (membershipExclusionRuleDTOList == null)
            {
                membershipExclusionRuleDTOList = new List<MembershipExclusionRuleDTO>();
            }
            searchParameters.Add(new KeyValuePair<MembershipExclusionRuleDTO.SearchByParameters, string>(MembershipExclusionRuleDTO.SearchByParameters.LAST_UPDATED_BY, executionContext.GetUserId().ToString()));
            populateMembershipExclusionRuleDTOList = membershipExclusionRuleDataHandler.PopulateMembershipExclusionRuleList(searchParameters);
            if (populateMembershipExclusionRuleDTOList != null)
            {
                membershipExclusionRuleDTOList.AddRange(populateMembershipExclusionRuleDTOList);
            }
            // For Non Membership members empty row is inserting
            MembershipExclusionRuleDTO membershipExclusionRuleDTO = new MembershipExclusionRuleDTO();
            membershipExclusionRuleDTOList.Add(membershipExclusionRuleDTO);
            log.LogMethodExit(membershipExclusionRuleDTOList);
            return membershipExclusionRuleDTOList;
        }

        /// <summary>
        /// Hard Deletions for Membership Exclusion Rule
        /// </summary>
        public void Delete()
        {
            log.LogMethodEntry();
            if (membershipExclusionRuleDTOList != null && membershipExclusionRuleDTOList.Count > 0)
            {
                foreach (MembershipExclusionRuleDTO membershipExclusionRuleDTO in membershipExclusionRuleDTOList)
                {
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            if (membershipExclusionRuleDTO.IsChanged && membershipExclusionRuleDTO.IsActive == false)
                            {
                                parafaitDBTrx.BeginTransaction();
                                MembershipExclusionRuleBL membershipExclusionRuleBL = new MembershipExclusionRuleBL(executionContext, membershipExclusionRuleDTO);
                                membershipExclusionRuleBL.Delete(parafaitDBTrx.SQLTrx);
                                parafaitDBTrx.EndTransaction();
                            }
                        }
                        catch (SqlException sqlEx)
                        {
                            log.Error(sqlEx);
                            parafaitDBTrx.RollBack();
                            log.LogMethodExit(null, "Throwing SQL Exception : " + sqlEx.Message);
                            if (sqlEx.Number == 547)
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869)); //Unable to delete this record.Please check the reference record first.
                            }
                            else
                            {
                                throw;
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Message);
                            parafaitDBTrx.RollBack();
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