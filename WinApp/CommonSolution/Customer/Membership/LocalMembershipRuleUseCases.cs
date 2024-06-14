/********************************************************************************************
 * Project Name - MembershipRule
 * Description  - MembershipRuleUseCases class
 *
 **************
 ** Version Log
  **************
  * Version      Date              Modified By         Remarks
 *********************************************************************************************
 2.120.0         05-May-2021       B Mahesh Pai       Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer.Membership
{
    class LocalMembershipRuleUseCases:IMembershipRuleUseCases
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalMembershipRuleUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<MembershipRuleDTO>> GetMembershipRules(List<KeyValuePair<MembershipRuleDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)

        {
            return await Task<List<MembershipRuleDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                MembershipRulesList membershipRulesList = new MembershipRulesList(executionContext);
                List<MembershipRuleDTO> membershipRuleDTOList = membershipRulesList.GetAllMembershipRule(searchParameters, sqlTransaction);

                log.LogMethodExit(membershipRuleDTOList);
                return membershipRuleDTOList;
            });
        }
        public async Task<string> SaveMembershipRules(List<MembershipRuleDTO> membershipRuleDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(membershipRuleDTOList);
                    if (membershipRuleDTOList == null)
                    {
                        throw new ValidationException("membershipRuleDTOList is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            MembershipRulesList membershipRulesBL = new MembershipRulesList(membershipRuleDTOList, executionContext);
                            membershipRulesBL.SaveUpdateMembershipRule();
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
