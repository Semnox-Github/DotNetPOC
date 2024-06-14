/********************************************************************************************
 * Project Name - Product
 * Description  - LocalMembershipExclusionRuleUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
*2.140.00   14-Sep-2021      B Mahesh Pai            Created 
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
   public class LocalMembershipExclusionRuleUseCases:IMembershipExclusionRuleUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalMembershipExclusionRuleUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<MembershipExclusionRuleDTO>> GetMembershipExclusionRules(List<KeyValuePair<MembershipExclusionRuleDTO.SearchByParameters, string>>
                          searchParameters, bool isPopulate, string productId)
        {
            return await Task<List<MembershipExclusionRuleDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                MembershipExclusionRuleListBL membershipList = new MembershipExclusionRuleListBL(executionContext);
                List<MembershipExclusionRuleDTO> membershipExclusionRuleDTOList = new List<MembershipExclusionRuleDTO>();
                if (isPopulate)
                {
                    List<MembershipExclusionRuleDTO> membershipExclusionRuleList = membershipList.PopulateMembershipExclusion(searchParameters);
                    if (membershipExclusionRuleList != null && membershipExclusionRuleList.Count > 0)
                    {
                        if (Convert.ToInt32(productId) >= 0)
                        {
                            if (membershipExclusionRuleList.FindAll(m => m.MembershipId == -1 && m.Id > -1 && m.ProductId > -1).Count > 0)
                            {
                                membershipExclusionRuleDTOList = membershipExclusionRuleList;
                            }
                            else
                            {
                                membershipExclusionRuleList.Find(m => m.Id == -1 && m.ProductId == -1 && m.MembershipId == -1).ProductId = Convert.ToInt32(productId);
                                membershipExclusionRuleDTOList = membershipExclusionRuleList;
                            }
                        }
                    }
                }
                else
                {
                    membershipExclusionRuleDTOList = membershipList.GetMembershipExclusionRuleDTOList(searchParameters);
                }
                return membershipExclusionRuleDTOList;
            });
        }
        public async Task<string> SaveMembershipExclusionRules(List<MembershipExclusionRuleDTO> membershipExclusionRuleDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                log.LogMethodEntry(membershipExclusionRuleDTOList);
                if (membershipExclusionRuleDTOList == null)
                {
                    throw new ValidationException("MembershipExclusionRuleDTOList is Empty");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (MembershipExclusionRuleDTO membershipExclusionRuleDTO in membershipExclusionRuleDTOList)
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
                            throw valEx;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw ex;
                        }
                    }
                }

                result = "Success";
                log.LogMethodExit(result);
                return result;
            });
        }

        public async Task<string> DeleteMembershipExclusionRules(List<MembershipExclusionRuleDTO> membershipExclusionRuleDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(membershipExclusionRuleDTOList);
                    MembershipExclusionRuleListBL membershipExclusionRuleList = new MembershipExclusionRuleListBL(executionContext, membershipExclusionRuleDTOList);
                    membershipExclusionRuleList.Delete();
                    result = "Success";
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    result = "Falied";
                }
                log.LogMethodExit(result);
                return result;
            });
        }
    }
}
