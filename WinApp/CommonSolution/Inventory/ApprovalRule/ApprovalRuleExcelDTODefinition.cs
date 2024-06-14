/********************************************************************************************
 * Project Name - Inventory
 * Description  - Data object of Approval Rule  Excel DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.110.0    14-Oct-2020   Mushahid Faizan Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using Semnox.Parafait.User;

namespace Semnox.Parafait.Inventory
{
    public class ApprovalRuleExcelDTODefinition : ComplexAttributeDefinition
    {

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="fieldName"></param>
        public ApprovalRuleExcelDTODefinition(ExecutionContext executionContext, string fieldName) : base(fieldName, typeof(ApprovalRuleDTO))
        {

            attributeDefinitionList.Add(new SimpleAttributeDefinition("ApprovalRuleID", "ApprovalRuleID", new IntValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("DocumentTypeID", "DocumentTypeID", new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("RoleId", "Role", new UserRolesValueConverter(executionContext)));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("NumberOfApprovalLevels", "NumberOfApprovalLevels", new IntValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("IsActive", "IsActive", new BooleanValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("LastUpdatedBy", "LastUpdatedBy", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("LastUpdatedDate", "LastUpdatedDate", new NullableDateTimeValueConverter()));

        }
    }

    class UserRolesValueConverter : ValueConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        List<KeyValuePair<int, UserRolesDTO>> userRolesIdCategoryDTOKeyValuePair;
        List<KeyValuePair<string, UserRolesDTO>> userRolesNameCategoryDTOKeyValuePair;
      
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public UserRolesValueConverter(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            userRolesIdCategoryDTOKeyValuePair = new List<KeyValuePair<int, UserRolesDTO>>();
            userRolesNameCategoryDTOKeyValuePair = new List<KeyValuePair<string, UserRolesDTO>>();
            List<UserRolesDTO> userRolesList = new List<UserRolesDTO>();

            UserRolesList userRolesDTOList = new UserRolesList(executionContext);
            List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>> searchParams = new List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>>();
            searchParams.Add(new KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>(UserRolesDTO.SearchByUserRolesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            userRolesList = userRolesDTOList.GetAllUserRoles(searchParams);

            if (userRolesList != null && userRolesList.Count > 0)
            {
                foreach (UserRolesDTO userRolesDTO in userRolesList)
                {
                    userRolesIdCategoryDTOKeyValuePair.Add(new KeyValuePair<int, UserRolesDTO>(userRolesDTO.RoleId, userRolesDTO));
                    userRolesNameCategoryDTOKeyValuePair.Add(new KeyValuePair<string, UserRolesDTO>(userRolesDTO.Role, userRolesDTO));
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts userRolesname to userRolesid
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        public override object FromString(string stringValue)
        {
            log.LogMethodEntry(stringValue);
            int userRolesId = -1;

            for (int i = 0; i < userRolesNameCategoryDTOKeyValuePair.Count; i++)
            {
                if (userRolesNameCategoryDTOKeyValuePair[i].Key == stringValue)
                {
                    userRolesNameCategoryDTOKeyValuePair[i] = new KeyValuePair<string, UserRolesDTO>(userRolesNameCategoryDTOKeyValuePair[i].Key, userRolesNameCategoryDTOKeyValuePair[i].Value);
                    userRolesId = userRolesNameCategoryDTOKeyValuePair[i].Value.RoleId;
                }
            }

            log.LogMethodExit(userRolesId);
            return userRolesId;
        }
        /// <summary>
        /// Converts userRolesid to userRolesname
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>

        public override string ToString(object value)
        {
            log.LogMethodEntry(value);
            string userRolesName = string.Empty;

            for (int i = 0; i < userRolesIdCategoryDTOKeyValuePair.Count; i++)
            {
                if (userRolesIdCategoryDTOKeyValuePair[i].Key == Convert.ToInt32(value))
                {
                    userRolesIdCategoryDTOKeyValuePair[i] = new KeyValuePair<int, UserRolesDTO>(userRolesIdCategoryDTOKeyValuePair[i].Key, userRolesIdCategoryDTOKeyValuePair[i].Value);

                    userRolesName = userRolesIdCategoryDTOKeyValuePair[i].Value.Role;
                }
            }

            log.LogMethodExit(userRolesName);
            return userRolesName;
        }
    }

}
