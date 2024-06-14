/********************************************************************************************
 * Project Name - Customer
 * Description  - CustomerUseCaseFactory class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0          05-Dec-2020      Vikas Dwivedi             Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System.Configuration;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer.Membership;

namespace Semnox.Parafait.Customer
{
    public class CustomerUseCaseFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static IMembershipUseCases GetMembershipUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IMembershipUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteMembershipUseCases(executionContext);
            }
            else
            {
                result = new LocalMembershipUseCases(executionContext);
            }
            log.LogMethodExit(result);
            return result;
        }

        public static ICustomerUseCases GetCustomerUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ICustomerUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteCustomerUseCases(executionContext);
            }
            else
            {
                result = new LocalCustomerUseCases(executionContext);
            }
            log.LogMethodExit(result);
            return result;
        }

        public static ICustomerRelationshipTypeUseCases GetCustomerRelationshipTypeUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ICustomerRelationshipTypeUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteCustomerRelationshipTypeUseCases(executionContext);
            }
            else
            {
                result = new LocalCustomerRelationshipTypeUseCases(executionContext);
            }
            log.LogMethodExit(result);
            return result;
        }

        public static ICustomerFeedBackSurveyDataUseCases GetCustomerFeedbackSurveyDatas(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ICustomerFeedBackSurveyDataUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteCustomerFeedBackSurveyDataUseCases(executionContext);
            }
            else
            {
                result = new LocalCustomerFeedBackSurveyDataUseCases(executionContext);
            }
            log.LogMethodExit(result);
            return result;
        }
        public static ICustomerFeedbackSurveyDetailsUseCases GetCustomerFeedbackSurveyDetails(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ICustomerFeedbackSurveyDetailsUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteCustomerFeedbackSurveyDetailsUseCases(executionContext);
            }
            else
            {
                result = new LocalCustomerFeedbackSurveyDetailsUseCases(executionContext);
            }
            log.LogMethodExit(result);
            return result;
        }
        public static ICustomerFeedbackSurveyUseCases GetCustomerFeedbackSurveys(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ICustomerFeedbackSurveyUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteCustomerFeedbackSurveyUseCases(executionContext);
            }
            else
            {
                result = new LocalCustomerFeedbackSurveyUseCases(executionContext);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// GetCustomerFeedbackQuestionsUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static ICustomerFeedbackQuestionsUseCases GetCustomerFeedbackQuestionsUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ICustomerFeedbackQuestionsUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteCustomerFeedbackQuestionsUseCases(executionContext);
            }
            else
            {
                result = new LocalCustomerFeedbackQuestionsUseCases(executionContext);
            }
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// GetCustomerFeedbackResponseUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static ICustomerFeedbackResponseUseCases GetCustomerFeedbackResponseUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ICustomerFeedbackResponseUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteCustomerFeedbackResponseUseCases(executionContext);
            }
            else
            {
                result = new LocalCustomerFeedbackResponseUseCases(executionContext);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// GetMembershipRuleUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static IMembershipRuleUseCases GetMembershipRuleUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IMembershipRuleUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteMembershipRuleUseCases(executionContext);
            }
            else
            {
                result = new LocalMembershipRuleUseCases(executionContext);
            }
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// GetDefaultAddressUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static IDefaultAddressUseCases GetDefaultAddressUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IDefaultAddressUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteDefaultAddressUseCases(executionContext);
            }
            else
            {
                result = new LocalDefaultAddressUseCases(executionContext);
            }
            log.LogMethodExit(result);
            return result;
        }

        public static ICustomerUIMetadataUseCases GetCustomerUIMetadataUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ICustomerUIMetadataUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteCustomerUIMetadataUseCases(executionContext);
            }
            else
            {
                result = new LocalCustomerUIMetadataUseCases(executionContext);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// GetAddressTypeUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static IAddressTypeUseCases GetAddressTypeUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IAddressTypeUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteAddressTypeUseCases(executionContext);
            }
            else
            {
                result = new LocalAddressTypeUseCases(executionContext);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// GetCustomerFeedbackSurveyDataSets
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static ICustomerFeedBackSurveyDataSetUseCases GetCustomerFeedbackSurveyDataSets(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ICustomerFeedBackSurveyDataSetUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteCustomerFeedBackSurveyDataSetUseCases(executionContext);
            }
            else
            {
                result = new LocalCustomerFeedBackSurveyDataSetUseCases(executionContext);
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
